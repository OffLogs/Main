@Library('common')
import com.shared.jenkins.docker.DockerHelper
import com.shared.jenkins.docker.DockerContainer

def dockerHelper = new DockerHelper(this)

def registryUrl = 'docker.subs.itproject.club'

def mainContainer = new DockerContainer(
    name: 'offlogs-main-production',
    dockerFile: 'devops/publish/image/common/Dockerfile',
    registryUrl: registryUrl,
);
mainContainer.generateRandomTag()
def imageTag = mainContainer.tag

def migrationContainer = new DockerContainer(
    name: 'offlogs-migration-production',
    dockerFile: 'devops/publish/image/common/Dockerfile',
    isRunAlways: false,
    isRunInBackground: false,
    registryUrl: registryUrl,
    tag: imageTag,
);
def webAppContainer = new DockerContainer(
    name: 'offlogs-web-production',
    dockerFile: 'devops/publish/image/web/Dockerfile',
    registryUrl: registryUrl,
    tag: imageTag,
);

properties([
    disableConcurrentBuilds()
])

node('vizit-mainframe-testing-node') {
    env.ENVIRONMENT = "Development"

    stage('Checkout') {
        cleanWs()
        sh """
            git config --global http.postBuffer 2048M
            git config --global http.maxRequestBuffer 1024M
            git config --global core.compression 9
        """
        checkout scm
    }

    stage('Build and push main image to the registry') {
        docker.withRegistry("https://$registryUrl", 'abedor_docker_registry_credentials') {
            dockerHelper.buildAndPush(mainContainer)
            echo "Pushed container: ${mainContainer.getFullImageName()}"
        }
    }
    
    stage('Build and push web image to the registry') {
        docker.withRegistry("https://$registryUrl", 'abedor_docker_registry_credentials') {
            dockerHelper.buildAndPush(webAppContainer)
            echo "Pushed container: ${webAppContainer.getFullImageName()}"
        }
    }
}

node('vizit-mainframe-k8s-master') {
    stage('Checkout') {
        cleanWs()
        sh """
            git config --global http.postBuffer 2048M
            git config --global http.maxRequestBuffer 1024M
            git config --global core.compression 9
        """
        checkout scm
    }

    stage('Apply K8S config') {
        sh """
            helm upgrade offlogs --install \
            --set images.frontApi.tag=${imageTag} \
            --set images.api.tag=${imageTag} \
            --set images.web.tag=${imageTag} \
            --set images.worker.tag=${imageTag} \
            devops/publish/chart
        """
    }
}
