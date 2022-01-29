node('vizit-mainframe-testing-node') {
    properties([
        disableConcurrentBuilds(),
        gitLabConnection('gitlab_lampego'),
    ])


    String testScriptParameters = '--logger=trx --no-restore --no-build --results-directory=./results'
    String postresUserPassword = 'postgres'

    Map<String, String> containerEnvVars = [
        // Zookeeper
        'ZOOKEEPER_CLIENT_PORT': 2181,
        'ZOOKEEPER_TICK_TIME': 2000,
    
        // Kafka
        'KAFKA_HOME': "./devops/common/binable/kafka",
        'KAFKA_BROKER_ID': 1,
        'KAFKA_ZOOKEEPER_CONNECT': "localhost:2181",
        'KAFKA_LISTENERS': "INSIDE://:9092,OUTSIDE://:9094",
        'KAFKA_ADVERTISED_LISTENERS': "INSIDE://:9092,OUTSIDE://localhost:9094",
        'KAFKA_LISTENER_SECURITY_PROTOCOL_MAP': "INSIDE:PLAINTEXT,OUTSIDE:PLAINTEXT",
        'KAFKA_INTER_BROKER_LISTENER_NAME': "INSIDE",
    
        // Postgres
        'POSTGRES_CONNECTION_RETRIES': 5,
        'POSTGRES_USER': postresUserPassword,
        'POSTGRES_PASSWORD': postresUserPassword,
        'POSTGRES_DATABASE': "template1",

        'ConnectionStrings__DefaultConnection': "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Pooling=true;",
        'Kafka__Servers': "localhost:9094",
        'Hibernate__IsShowSql': "false"
    ]

    preconfigureAndStart(({ networkId ->
        runStage(Stage.CLEAN) {
            // Clean before build
            cleanWs()
        }
    
        runStage(Stage.CHECKOUT) {
            checkout scm
        }
        
        def testImage = docker.build('offlogs-test-image', '--file=./devops/test/Dockerfile .')
        String containerEnvVarString = mapToEnvVars(containerEnvVars)
        testImage.inside(containerEnvVarString.concat(" --network=$networkId")) {

            runStage(Stage.BUILD) {
                sh 'echo "{}" > appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Migrations/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Console/appsettings.Local.json'
                sh 'dotnet build --'
            }

            runStage(Stage.ASSIGN_PERMISSIONS) {
                sh 'chmod -R 700 $KAFKA_HOME'
                sh 'chmod -R 700 ./devops/common/kafka/boot.sh'
                sh 'chmod -R 770 ./devops/common/zookeeper/boot.sh'
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
                sh 'dotnet run --no-restore --no-build --project ./OffLogs.Migrations'
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
    CLEAN('Clean'),
    CHECKOUT('Checkout'),
    BUILD('Build projects'),
    ASSIGN_PERMISSIONS('Assign Permissions'),
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
