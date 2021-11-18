pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:5.0'
            args '-u root --privileged'
        }
    }
    
    environment {
        DOTNET_CLI_HOME = "/tmp/DOTNET_CLI_HOME"
        HOME = '/tmp'
        ASPNETCORE_ENVIRONMENT = "Development"
        DOTNET_USE_POLLING_FILE_WATCHER = true  
        ASPNETCORE_URLS = "http://+:80" 
    }
    
    stages {
        stage('Info') {
            steps {
                echo 'Current user ${USER}'
            }
        }
        
        stage('Preparing') {
            steps {
                sh 'apt-get update'
                sh 'apt-get install -y apt-transport-https wget ca-certificates'
                sh 'apt-get upgrade -y'
            }
        }
        
        stage('Build') {
            steps {
                sh 'echo "{}" > appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Migrations/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Console/appsettings.Local.json'
                sh 'dotnet restore'
                sh 'dotnet build'
                gitlabCommitStatus(name: 'test') {
                    sh 'dotnet test --logger trx --results-directory /var/temp ./OffLogs.Api.Tests.Unit'
                }
            }
        }
        
        stage('Test') {
            steps {
                echo 'Hello 1'
                echo 'Hello 1'
            }
        }
    }
}
