node('development') {
    properties([
        disableConcurrentBuilds(),
        gitLabConnection('gitlab_lampego'),
    ])

    String testScriptParameters = '--logger=trx --no-restore --no-build --results-directory=./results'
    String postresUserPassword = 'postgres'

    Map<String, String> containerEnvVars = [
        // Postgres
        'POSTGRES_USER': postresUserPassword,
        'POSTGRES_PASSWORD': postresUserPassword,
        'POSTGRES_DATABASE': "template1",

        'ConnectionStrings__DefaultConnection': "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Pooling=true;",
        'Kafka__Servers': "localhost:9094",
        'Hibernate__IsShowSql': "false"
    ]

    preconfigureAndStart(({ networkId ->
        runStage(Stage.CHECKOUT) {
            checkout scm
        }

        def testImage = docker.build('offlogs-test-image', '--file=./devops/test/Dockerfile .')
        String containerEnvVarString = mapToEnvVars(containerEnvVars)
        testImage.inside(containerEnvVarString.concat(" --network=$networkId")) {

            runStage(Stage.BUILD) {
                sh 'dotnet restore'
            }

            runStage(Stage.INIT_ZOOKEEPER) {
                sh './devops/common/zookeeper/boot.sh &'
                sh 'until nc -z localhost 2181; do sleep 1; done'
                echo "Zookeeper is started"
            }

            runStage(Stage.INIT_KAFKA) {
                sh './devops/common/kafka/boot.sh &'
                sh 'until nc -z localhost 9094; do sleep 1; done'
                echo "Kafka is started"
            }

            runStage(Stage.INIT_DB) {
                sh 'pg_ctlcluster 12 main start'
                sh 'netstat -tulpn | grep LISTEN'
                sh 'pg_isready'
                sh "sudo -u postgres psql -c \"ALTER USER postgres PASSWORD '$postresUserPassword';\""
                sh "PGPASSWORD=postgres psql -h localhost --username=$postresUserPassword --dbname=$postresUserPassword -c \"select 1\""
                echo 'Postgre SQL is started'
            }

            runStage(Stage.RUN_MIGRATIONS) {
                sh 'dotnet run --project="./OffLogs.Migrations/OffLogs.Migrations.csproj"'
            }

            runStage(Stage.RUN_UNIT_TESTS_API) {
                sh 'dotnet test --logger trx --results-directory /tmp/test ./OffLogs.Api.Tests.Unit'
            }

            runStage(Stage.RUN_UNIT_TESTS_BUSINESS_LOGIC) {
                sh 'dotnet test --logger trx --results-directory /tmp/test ./OffLogs.Business.Common.Tests.Unit'
            }

            runStage(Stage.RUN_INTEGRATION_TESTS_API) {
                sh 'dotnet test --logger trx --results-directory /tmp/test ./OffLogs.Api.Tests.Integration'
            }
        }
    } as Closure<String>))
}

enum Stage {
    CHECKOUT('Checkout'),
    BUILD('Build'),
    INIT_ZOOKEEPER('Init Zookeeper'),
    INIT_KAFKA('Init Kafka'),
    INIT_DB('Init DB'),
    RUN_MIGRATIONS('Run migrations'),
    RUN_UNIT_TESTS_API('Run unit tests. Api'),
    RUN_UNIT_TESTS_BUSINESS_LOGIC('Run unit tests. Business logic'),
    RUN_INTEGRATION_TESTS_API('Run unit tests. Business logic'),

//    SAVE_ARTIFACTS('Save artifacts'),

    private final String name;

    private Stage(String s) {
        this.name = s;
    }

    String toString() {
        return this.name;
    }

    static String[] toListOfStrings() {
        def result = []
        for (def stage: values()) {
            result.add(stage.toString())
        }
        return result
    }
}

def mapToEnvVars(Map<String, String> list) {
    String result = ''
    list.each {
        result = "$result -e $it.key=\"$it.value\""
    }
    return result
}

def preconfigureAndStart(Closure<String> inner) {
    def networkId = UUID.randomUUID().toString()
    try {
        sh "docker network rm ${networkId}"
    } catch(Exception exception) {
        println exception.getMessage()
    }
    try {
        sh "docker network create ${networkId}"
        gitlabBuilds(builds: Stage.toListOfStrings()) {
            inner.call(networkId)
        }
    } finally {
        sh "docker network rm ${networkId}"
    }
}

def runStage(Stage stageAction, Closure callback) {
    stage(stageAction.toString()) {
        try {
            updateGitlabCommitStatus name: stageAction.toString(), state: 'running'
            callback()
            updateGitlabCommitStatus name: stageAction.toString(), state: 'success'
        } catch (Exception e) {
            updateGitlabCommitStatus name: stageAction.toString(), state: 'failed'
            throw new Exception(e.getMessage())
        }
    }
}
