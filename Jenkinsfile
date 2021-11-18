pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:5.0'
        }
    }
    
    
    stages {
        stage('Info') {
            steps {
                echo 'Current user ${USER}'
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
