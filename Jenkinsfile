pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:5.0-focal'
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
                sh 'apt update'
                sh 'apt install -y apt-transport-https wget ca-certificates'
                sh 'apt upgrade -y'
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
