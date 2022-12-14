node('testing-node') {
    properties([
        disableConcurrentBuilds(),
    ])

    stage("Test test") {
        checkout scm
    }

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

        // Redis
        'Redis__Server': "localhost:6379",

        'ConnectionStrings__DefaultConnection': "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Pooling=true;Include Error Detail=true;Log Parameters=true;",
        'Kafka__Servers': "localhost:9094",
        'Hibernate__IsShowSql': "false"
    ]

    runStage(Stage.UPDATE_GIT_STATUS) {
        updateGithubCommitStatus('Set PENDING status', 'PENDING')
    }

    preconfigureAndStart(({ networkId ->
        runStage(Stage.CLEAN) {
            // Clean before build
            cleanWs()
        }
    
        runStage(Stage.CHECKOUT) {
            sh """
                git config --global http.postBuffer 2048M
                git config --global http.maxRequestBuffer 1024M
                git config --global core.compression 9
            """
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
                sh 'pg_isready'
                sh "sudo -u postgres psql -c \"ALTER USER postgres PASSWORD '$postresUserPassword';\""
                sh "PGPASSWORD=postgres psql -h localhost --username=$postresUserPassword --dbname=$postresUserPassword -c \"select 1\""
                echo 'Postgre SQL is started'
            }

            runStage(Stage.INIT_REDIS) {
                sh '/usr/bin/redis-server &'
                sh 'until nc -z localhost 6379; do sleep 1; done'
                echo "Redis is started"
                
                sh 'netstat -tulpn | grep LISTEN'
            }

            runStage(Stage.RUN_MIGRATIONS) {
                sh 'dotnet run --no-restore --no-build --project ./OffLogs.Migrations'
            }

            runStage(Stage.RUN_API_UNIT_TESTS) {
                sh 'dotnet test --logger trx --verbosity=normal --results-directory /tmp/test ./OffLogs.Api.Tests.Unit'
            }
            
            runStage(Stage.RUN_BUSINESS_LOGIC_UNIT_TESTS) {
                sh 'dotnet test --logger trx --verbosity=normal --results-directory /tmp/test ./OffLogs.Business.Common.Tests.Unit'
            }
                        
            runStage(Stage.RUN_INTEGRATION_TESTS) {
                sh 'dotnet test --logger trx --verbosity=normal --results-directory /tmp/test ./OffLogs.Api.Tests.Integration'
            }
        }

        runStage(Stage.UPDATE_GIT_STATUS) {
            updateGithubCommitStatus('Set SUCCESS status', 'SUCCESS')
        }
    } as Closure<String>))
}

enum Stage {
    UPDATE_GIT_STATUS('Update git status'),
    CLEAN('Clean'),
    CHECKOUT('Checkout'),
    BUILD('Build projects'),
    ASSIGN_PERMISSIONS('Assign Permissions'),
    INIT_ZOOKEEPER('Init Zookeeper'),
    INIT_KAFKA('Init Kafka'),
    INIT_DB('Init DB'),
    INIT_REDIS('Init Redis'),
    RUN_MIGRATIONS('Run migrations'),
    RUN_API_UNIT_TESTS('Run API unit tests'),
    RUN_BUSINESS_LOGIC_UNIT_TESTS('Run Business logic unit tests'),
    RUN_INTEGRATION_TESTS('Run integration tests'),

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
        return result.reverse()
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
        inner(networkId)
    } catch(Exception exception) {
        updateGithubCommitStatus(exception.getMessage(), 'ERROR')
        println exception.getMessage()
    } finally {
        sh "docker network rm ${networkId}"
    }
}

def runStage(Stage stageAction, Closure callback) {
    stage(stageAction.toString()) {
        try {
            callback()
        } catch (Exception e) {
            throw new Exception(e.getMessage())
        }
    }
}

def getRepoURL() {
  sh "git config --get remote.origin.url > .git/remote-url"
  return readFile(".git/remote-url").trim()
}

def getCommitSha() {
  sh "git rev-parse HEAD > .git/current-commit"
  return readFile(".git/current-commit").trim()
}

def updateGithubCommitStatus(String message, String state) {
        String commitHash = getCommitSha()
        String repositoryUrl = getRepoURL()

        // workaround https://issues.jenkins-ci.org/browse/JENKINS-38674
        step([
            $class: 'GitHubCommitStatusSetter',
            reposSource: [$class: "ManuallyEnteredRepositorySource", url: repositoryUrl],
            commitShaSource: [$class: "ManuallyEnteredShaSource", sha: commitHash],
            errorHandlers: [[$class: 'ShallowAnyErrorHandler']],
            statusResultSource: [ $class: "ConditionalStatusResultSource", results: [[$class: "AnyBuildResult", message: message, state: state]] ]
        ])
    }
