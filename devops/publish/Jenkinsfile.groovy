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
def imageTag = mainContainer.generateRandomTag()
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

// node('vizit-mainframe-testing-node') {
//     env.ENVIRONMENT = "Development"
// 
//     stage('Checkout') {
//         checkout scm
//     }
//     
//     stage('Build and push images to the registry') {
//         docker.withRegistry("https://$registryUrl", 'abedor_docker_registry_credentials') {
//             dockerHelper.buildAndPush(mainContainer)
//             echo "Pushed container: ${mainContainer.getFullImageName()}"
//             
//             dockerHelper.buildAndPush(webAppContainer)
//             echo "Pushed container: ${webAppContainer.getFullImageName()}"
//         }
//     }
// }

node('vizit-mainframe-k8s-master') {
    stage('Checkout') {
        checkout scm
    }
    
    stage('Apply K8S config') {
        sh "helm install offlogs devops/publish/chart"
    }
}
