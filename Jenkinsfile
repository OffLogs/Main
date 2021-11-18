pipeline {
    agent any
    
    stages {
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
            }
        }
        stage('Test') {
            steps {
                echo 'Hello 2'
            }
        }
    }
}
