pipeline {
    agent {
        docker {
            image 'ubuntu:focal'
            args '-u root --privileged'
            reuseNode true
        }
    }
    
    options { 
        disableConcurrentBuilds() 
        gitlabBuilds(builds: ['prepairing_os', 'prepairing_soft', 'build', 'test'])
    }
    
    triggers {
        gitlab(triggerOnPush: true, triggerOnMergeRequest: true, branchFilterType: 'All')
    }
    
    environment {
        TZ = "UTC"
        DOTNET_CLI_HOME = "/tmp/DOTNET_CLI_HOME"
        HOME = '/tmp'
        ASPNETCORE_ENVIRONMENT = "Development"
        DOTNET_USE_POLLING_FILE_WATCHER = true  
        ASPNETCORE_URLS = "http://+:80" 
        
        // Postgres
        POSTGRES_CONNECTION_RETRIES = 5
        POSTGRES_USER = "postgres"
        POSTGRES_PASSWORD = "postgres"
        POSTGRES_DATABASE = "template1"
        
        // Zookeeper
        ZOOKEEPER_CLIENT_PORT = 2181
        ZOOKEEPER_TICK_TIME = 2000
        
        // Kafka
        KAFKA_HOME = "./devops/common/binable/kafka"
        KAFKA_BROKER_ID = 1
        KAFKA_ZOOKEEPER_CONNECT = "localhost:2181"
        KAFKA_LISTENERS = "INSIDE://:9092,OUTSIDE://:9094"
        KAFKA_ADVERTISED_LISTENERS = "INSIDE://:9092,OUTSIDE://localhost:9094"
        KAFKA_LISTENER_SECURITY_PROTOCOL_MAP = "INSIDE:PLAINTEXT,OUTSIDE:PLAINTEXT"
        KAFKA_INTER_BROKER_LISTENER_NAME = "INSIDE"
        
        // OffLogs
        ConnectionStrings__DefaultConnection = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;Pooling=true;"
        Kafka__Servers = "localhost:9094"
        Hibernate__IsShowSql = "false"
        
        PATH = "$PATH:$KAFKA_HOME/bin"
    }
    
    stages {
        
        stage('Prepare OS') {
            steps {
                updateGitlabCommitStatus name: 'prepairing_os', state: 'running'
                
                // Time
                sh 'ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone'
                
                // Fix: The configured user limit (128) on the number of inotify instances 
                // has been reached, or the per-process limit on the number of open file descriptors has been reached.
                sh 'echo "fs.inotify.max_user_watches=524288" >> /etc/sysctl.conf'
                sh 'echo "fs.inotify.max_user_instances=524288" >> /etc/sysctl.conf'
                sh 'sysctl -p'

                sh 'apt-get update'
                sh 'apt-get install -y apt-transport-https wget ca-certificates apt-transport-https debconf-utils net-tools sudo netcat'
                sh 'apt-get install -y default-jre'
            }
        }
        
        stage('Install .Net Core') {
            steps {
                echo 'Install .Net Core 5.0'
                sh 'wget https://packages.microsoft.com/config/ubuntu/20.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb'
                sh 'dpkg -i packages-microsoft-prod.deb'
                sh 'apt-get update'
                sh 'apt-get install -y dotnet-sdk-5.0'
            }
        }
        
        stage('Assign permissions') {
            steps {
                sh 'chmod -R 700 $KAFKA_HOME'
                sh 'chmod -R 700 ./devops/common/kafka/boot.sh'
                sh 'chmod -R 700 ./devops/common/zookeeper/boot.sh'
                
                updateGitlabCommitStatus name: 'prepairing_os', state: 'success'
            }
        }
        
        stage('System Info') {
            steps {
                echo 'Current user $USER'
                sh 'pwd'
            }
        }
        
        stage('Init Zookeeper') {
            steps {
                updateGitlabCommitStatus name: 'prepairing_soft', state: 'running'
                
                sh './devops/common/zookeeper/boot.sh &'
                sh 'until nc -z localhost 2181; do sleep 1; done'
                echo "Zookeeper is started"
            }
        }
        
        stage('Init Kafka') {
            steps {
                sh './devops/common/kafka/boot.sh &'
                sh 'until nc -z localhost 9094; do sleep 1; done'
                echo "Kafka is started"
            }
        }
        
        stage('Init PostgreSQL') {
            steps {
                sh 'apt-get install -y postgresql postgresql-client'
                sh 'pg_ctlcluster 12 main start'
                sh 'netstat -tulpn | grep LISTEN'
                sh 'pg_isready'
                sh 'sudo -u postgres psql -c "ALTER USER postgres PASSWORD \'postgres\';"'
                sh 'PGPASSWORD=postgres psql -h localhost --username=$POSTGRES_USER --dbname=$POSTGRES_DATABASE -c "select 1"'
                echo 'Postgre SQL is started'
                
                updateGitlabCommitStatus name: 'prepairing_soft', state: 'success'
            }
        }
        
        stage('Build') {
            steps {
                updateGitlabCommitStatus name: 'build', state: 'running'
                sh 'echo "{}" > appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Migrations/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Console/appsettings.Local.json'
                sh 'dotnet restore'
                sh 'dotnet build'
                updateGitlabCommitStatus name: 'build', state: 'success'
            }
        }
        
        stage('Unit Tests') {
            steps {
                updateGitlabCommitStatus name: 'test', state: 'running'
                sh 'dotnet test --logger trx --results-directory /var/temp ./OffLogs.Api.Tests.Unit'
                sh 'dotnet test --logger trx --results-directory /var/temp ./OffLogs.Business.Common.Tests.Unit'
            }
        }
        
        stage('Integration Tests') {
            steps {
                sh 'dotnet test --logger trx --results-directory /var/temp ./OffLogs.Api.Tests.Integration'
            }
        }
        
    }
    
    post {
        success {
            updateGitlabCommitStatus name: 'test', state: 'success'
        }
        
        failure {
            updateGitlabCommitStatus name: 'test', state: 'failed'
        }
        
        aborted {
            updateGitlabCommitStatus name: 'test', state: 'canceled'
        }
        
        unsuccessful {
            updateGitlabCommitStatus name: 'test', state: 'canceled'
        }
    }
}
