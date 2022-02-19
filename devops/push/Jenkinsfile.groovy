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
    
    stage('Build and push') {
        docker.withRegistry("https://$registryUrl", 'abedor_docker_registry_credentials') {
            dockerHelper.buildAndPush(containers[0])
        }
    }
}
