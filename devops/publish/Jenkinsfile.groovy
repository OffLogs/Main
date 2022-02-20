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
def migrationContainer = new DockerContainer(
    name: 'offlogs-migration-production',
    dockerFile: 'devops/publish/image/common/Dockerfile',
    isRunAlways: false,
    isRunInBackground: false,
    registryUrl: registryUrl,
);
def webAppContainer = new DockerContainer(
    name: 'offlogs-web-production',
    dockerFile: 'devops/publish/image/web/Dockerfile',
    registryUrl: registryUrl,
);

node('vizit-mainframe-testing-node') {
    env.ENVIRONMENT = "Development"

    stage('Checkout') {
        checkout scm
    }
    
    stage('Build and push images to the registry') {
        docker.withRegistry("https://$registryUrl", 'abedor_docker_registry_credentials') {
            stage('Build containers') {
                dockerHelper.buildAndPush(mainContainer)
                echo "Pushed container: ${mainContainer.getFullImageName()}"
                
                dockerHelper.buildAndPush(webAppContainer)
                echo "Pushed container: ${webAppContainer.getFullImageName()}"
            }
        }
    }
}
