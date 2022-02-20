@Library('common')
import com.shared.jenkins.docker.DockerHelper
import com.shared.jenkins.docker.DockerContainer

def dockerHelper = new DockerHelper(this)

def registryUrl = 'docker.subs.itproject.club'

def containers = [
    new DockerContainer(
        name: 'offlogs-migration-production',
        dockerFile: 'devops/publish/migrations/Dockerfile',
        isRunAlways: false,
        isRunInBackground: false,
        registryUrl: registryUrl,
    ),
    new DockerContainer(
        name: 'offlogs-api-production',
        dockerFile: 'devops/publish/api/Dockerfile',
        port: '6056:80',
        registryUrl: registryUrl,
    ),
    new DockerContainer(
        name: 'offlogs-api-frontend-production',
        dockerFile: 'devops/publish/front/Dockerfile',
        port: '6057:80',
        registryUrl: registryUrl,
    ),
    new DockerContainer(
        name: 'offlogs-worker-production',
        dockerFile: 'devops/publish/worker/Dockerfile',
        registryUrl: registryUrl,   
    ),
    new DockerContainer(
        name: 'offlogs-web-production',
        dockerFile: 'devops/publish/web/Dockerfile',
        port: '6058:80',
        registryUrl: registryUrl,   
    ),
];

node('vizit-mainframe-testing-node') {
    env.ENVIRONMENT = "Development"

    stage('Checkout') {
        checkout scm
    }
    
    stage('Build and restore projects') {
        docker.image('mcr.microsoft.com/dotnet/sdk:6.0').inside('') { c ->
            sh 'echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json'
            sh 'echo "{}" > OffLogs.Console/appsettings.Local.json'
            sh 'echo "{}" > OffLogs.Api/appsettings.Local.json'
            sh 'echo "{}" > OffLogs.Api.Frontend/appsettings.Local.json'
            sh 'echo "{}" > OffLogs.Migrations/appsettings.Local.json'
            sh 'echo "{}" > OffLogs.WorkerService/appsettings.Local.json'
            sh 'dotnet restore --verbosity=q .'
            sh 'dotnet build --verbosity=q .'
        }
    }
    
    stage('Build and push images to the registry') {
        docker.withRegistry("https://$registryUrl", 'abedor_docker_registry_credentials') {
            stage('Build containers') {
                for (String container: containers) {
                    dockerHelper.buildAndPush(container)
                }
            }
        }
    }
}
