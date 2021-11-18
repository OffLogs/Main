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
                updateGitlabCommitStatus name: 'build', state: 'created'
                echo 'Current user ${USER}'
            }
        }
        
        stage('Preparing') {
            steps {
                updateGitlabCommitStatus name: 'build', state: 'preparing'
                sh 'apt-get update'
                sh 'apt-get install -y apt-transport-https wget ca-certificates'
                sh 'apt-get upgrade -y'
            }
        }
        
        stage('Build') {
            steps {
                updateGitlabCommitStatus name: 'build', state: 'pending'
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
                updateGitlabCommitStatus name: 'build', state: 'running'
                sh 'dotnet test --logger trx --results-directory /var/temp ./OffLogs.Api.Tests.Unit'
            }
        }
        
        post {
            success {
                updateGitlabCommitStatus name: 'build', state: 'success'
            }
            failure {
                updateGitlabCommitStatus name: 'build', state: 'failed'
            }
            aborted {
                updateGitlabCommitStatus name: 'build', state: 'canceled'
            }
            unsuccessful {
                updateGitlabCommitStatus name: 'build', state: 'canceled'
            }
        }
    }
}
