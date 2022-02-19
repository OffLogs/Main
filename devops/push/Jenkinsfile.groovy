@Library('common')
import com.shared.jenkins.docker.DockerHelper
import com.shared.jenkins.docker.DockerContainer

def dockerHelper = new DockerHelper(this)

def containers = [
    new DockerContainer(
        name: 'offlogs-migration-production',
        dockerFile: 'devops/publish/migrations/Dockerfile',
        isRunAlways: false,
        isRunInBackground: false
    ),
    new DockerContainer(
        name: 'offlogs-api-production',
        dockerFile: 'devops/publish/api/Dockerfile',
        port: '6056:80'
    ),
    new DockerContainer(
        name: 'offlogs-api-frontend-production',
        dockerFile: 'devops/publish/front/Dockerfile',
        port: '6057:80'
    ),
    new DockerContainer(
        name: 'offlogs-worker-production',
        dockerFile: 'devops/publish/worker/Dockerfile',
    ),
    new DockerContainer(
        name: 'offlogs-web-production',
        dockerFile: 'devops/publish/web/Dockerfile',
        port: '6058:80',
    ),
];

node('vizit-mainframe-testing-node') {
    env.ENVIRONMENT = "Development"

    stage('Checkout') {
        checkout scm
    }
    
    stage('Build and push') {
        dockerHelper.buildAndPush(containers[0])
    }
}
