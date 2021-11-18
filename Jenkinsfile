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
    
    options {
        gitlabBuilds(builds: ['Preparing', 'Build', 'Test'])
    }
    
    triggers {
        gitlab(triggerOnPush: true, triggerOnMergeRequest: true, branchFilterType: 'All')
    }
    
    stages {
        stage('Info') {
            steps {
                echo 'Current user ${USER}'
            }
        }
        
        stage('Preparing') {
            steps {
                updateGitlabCommitStatus name: 'Preparing', state: 'running'
                sh 'apt-get update'
                sh 'apt-get install -y apt-transport-https wget ca-certificates'
                sh 'apt-get upgrade -y'
                updateGitlabCommitStatus name: 'Preparing', state: 'success'
            }
        }
        
        stage('Build') {
            steps {
                updateGitlabCommitStatus name: 'Build', state: 'running'
                sh 'echo "{}" > appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Migrations/appsettings.Local.json'
                sh 'echo "{}" > OffLogs.Console/appsettings.Local.json'
                sh 'dotnet restore'
                sh 'dotnet build'
                updateGitlabCommitStatus name: 'Build', state: 'success'
            }
        }
        
        stage('Test') {
            steps {
                updateGitlabCommitStatus name: 'Test', state: 'running'
                sh 'dotnet test --logger trx --results-directory /var/temp ./OffLogs.Api.Tests.Unit'
            }
        }
    }
    
    post {
        success {
            updateGitlabCommitStatus name: 'Test', state: 'success'
        }
        
        failure {
            updateGitlabCommitStatus name: 'Test', state: 'failed'
        }
        
        aborted {
            updateGitlabCommitStatus name: 'Test', state: 'canceled'
        }
        
        unsuccessful {
            updateGitlabCommitStatus name: 'Test', state: 'canceled'
        }
    }
}
