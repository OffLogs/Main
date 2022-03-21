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

def containers = [mainContainer, migrationContainer]

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

    stage('Set environment vars') {
        containers.collect {
            // Kafka
            it.envVariables.put('Kafka__Servers', '192.168.110.6:29092,192.168.110.6:29093')
            it.envVariables.put('Kafka__ProducerId', 'offlogs-reducer-1')
            it.envVariables.put('Kafka__Topic_Logs', 'production-logs')
            it.envVariables.put('Kafka__Topic_Notification', 'offlogs-notification-logs')
            it.envVariables.put('Kafka__ConsumerClientId', 'client-1')
        
            withCredentials([
                    usernamePassword(credentialsId: "offlogs_production_db_credentials", usernameVariable: 'USER_NAME', passwordVariable: 'PASSWORD')
            ]) {
                it.envVariables.put(
                    'ConnectionStrings__DefaultConnection',
                    "User ID=${USER_NAME};Password=${PASSWORD};Host=192.168.110.6;Port=5432;Database=offlogs;Pooling=true;"
                )
            }
            withCredentials([
                    usernamePassword(credentialsId: "offlogs_production_smtp_credentials", usernameVariable: 'USER_NAME', passwordVariable: 'PASSWORD')
            ]) {
                it.envVariables.put('Smtp__Server', 'smtp-pulse.com')
                it.envVariables.put('Smtp__UserName', USER_NAME)
                it.envVariables.put('Smtp__Password', PASSWORD)
                it.envVariables.put('Smtp__From__Name', 'OffLogs')
                it.envVariables.put('Smtp__From__Email', 'support@offlogs.com')
                it.envVariables.put('Smtp__Port', '2525')
                it.envVariables.put('Smtp__EnableSsl', 'true')
            }
            withCredentials([string(credentialsId: "offlogs_production_user_jwt", variable: 'AUTH_SECRET')]) {
                it.envVariables.put('App__Auth__SymmetricSecurityKey', AUTH_SECRET)
            }
            withCredentials([string(credentialsId: "offlogs_production_application_jwt", variable: 'AUTH_SECRET')]) {
                it.envVariables.put('App__Application__SymmetricSecurityKey', AUTH_SECRET)
            }
        }
    }

    stage('Apply K8S config') {
        String bashScript = """
            helm upgrade offlogs --install \
            --set images.frontApi.tag=${imageTag} \
            --set images.api.tag=${imageTag} \
            --set images.web.tag=${imageTag} \
            --set images.worker.tag=${imageTag} \
        """
        container.envVariables.each {
            bashScript = "$bashScript --set pods.env.${it.key}=\"$it.value\""
        }
        bashScript = "$bashScript devops/publish/chart"
    }
}
