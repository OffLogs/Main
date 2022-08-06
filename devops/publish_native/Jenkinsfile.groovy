@Library('common')
import com.shared.jenkins.docker.DockerHelper
import com.shared.jenkins.docker.DockerContainer

def dockerHelper = new DockerHelper(this)
public Map<String, String> envVariables = new HashMap<String, String>()

def mainContainer = new DockerContainer(
    name: 'offlogs-main-production',
    dockerFile: 'devops/publish/image/common/Dockerfile',
);

def migrationContainer = new DockerContainer(
    name: 'offlogs-main-production',
    dockerFile: 'devops/publish/image/common/Dockerfile',
    isRunAlways: false,
    isRunInBackground: false,
);
def webAppContainer = new DockerContainer(
    name: 'offlogs-web-production',
    dockerFile: 'devops/publish/image/web/Dockerfile',
);

properties([
    disableConcurrentBuilds()
])

node('lampego-web-1') {
    env.ENVIRONMENT = "Development"

    stage('CleanUp Docker') {
        sh """
            docker image prune -f
        """
    }

    stage('Checkout') {
        // cleanWs()
        sh """
            git config --global http.postBuffer 2048M
            git config --global http.maxRequestBuffer 1024M
            git config --global core.compression 0
        """
        checkout scm
    }

    stage('Build main image') {
        dockerHelper.buildContainer(mainContainer)
    }

    stage('Build web image') {
        dockerHelper.buildContainer(webAppContainer)
    }

    stage('Set environment vars') {    
        envVariables.put('Serilog__IsSendEmailIfError', 'true')
        envVariables.put('Serilog__MinimumLevel__Default', 'Debug')

        // Kafka
        envVariables.put('Kafka__Servers', '10.10.0.2:29092,10.10.0.2:29093')
        envVariables.put('Kafka__ProducerId', 'offlogs-reducer')
        envVariables.put('Kafka__ConsumerClientId', 'offlogs-client')
        envVariables.put('Kafka__Topic__Logs', 'offlogs-production-logs')
        envVariables.put('Kafka__Topic__Notifications', 'offlogs-notification-logs')
    
        withCredentials([
                usernamePassword(credentialsId: "offlogs_production_db_credentials", usernameVariable: 'USER_NAME', passwordVariable: 'PASSWORD')
        ]) {
            envVariables.put(
                'ConnectionStrings__DefaultConnection',
                "User ID=${USER_NAME};Password=${PASSWORD};Host=10.10.0.2;Port=5432;Database=offlogs;Pooling=true;"
            )
        }
        withCredentials([
                usernamePassword(credentialsId: "offlogs_production_smtp_credentials", usernameVariable: 'USER_NAME', passwordVariable: 'PASSWORD')
        ]) {
            envVariables.put('Smtp__Server', 'smtp-pulse.com')
            envVariables.put('Smtp__UserName', USER_NAME)
            envVariables.put('Smtp__Password', PASSWORD)
            envVariables.put('Smtp__From__Name', 'OffLogs')
            envVariables.put('Smtp__From__Email', 'support@offlogs.com')
            envVariables.put('Smtp__Port', '465')
            envVariables.put('Smtp__EnableSsl', 'true')
        }
        withCredentials([string(credentialsId: "offlogs_production_user_jwt", variable: 'AUTH_SECRET')]) {
            envVariables.put('App__Auth__SymmetricSecurityKey', AUTH_SECRET)
        }
        withCredentials([string(credentialsId: "offlogs_production_application_jwt", variable: 'AUTH_SECRET')]) {
            envVariables.put('App__Application__SymmetricSecurityKey', AUTH_SECRET)
        }
        withCredentials([string(credentialsId: "offlogs_production_recaptcha_secret", variable: 'AUTH_SECRET')]) {
            envVariables.put('ReCaptcha__Secret', AUTH_SECRET)
        }
    }

    stage('Run migrations') {
        dockerHelper.stopContainer(migrationContainer)
            
        migrationContainer.envVariables = envVariables.clone()
        migrationContainer.envVariables.put('PROJECT_DIR', 'OffLogs.Migrations')
        dockerHelper.runContainer(migrationContainer)
    }

    stage('Run common API') {
        mainContainer.tagName = 'offlogs-api';
        mainContainer.port = '6105:80';
        dockerHelper.stopContainer(mainContainer)
        
        mainContainer.envVariables = envVariables.clone()
        mainContainer.envVariables.put('PROJECT_DIR', 'OffLogs.Api')
        dockerHelper.runContainer(mainContainer)
    }

    stage('Run frontend API') {
        mainContainer.tagName = 'offlogs-api-frontend';
        mainContainer.port = '6106:80';
        dockerHelper.stopContainer(mainContainer)
        
        mainContainer.envVariables = envVariables.clone()
        mainContainer.envVariables.put('PROJECT_DIR', 'OffLogs.Api.Frontend')
        dockerHelper.runContainer(mainContainer)
    }

    stage('Run worker') {
        mainContainer.tagName = 'offlogs-worker';
        mainContainer.port = '';
        dockerHelper.stopContainer(mainContainer)
        
        mainContainer.envVariables = envVariables.clone()
        mainContainer.envVariables.put('PROJECT_DIR', 'OffLogs.WorkerService')
        dockerHelper.runContainer(mainContainer)
    }

    stage('Run web app') {
        webAppContainer.port = '6107:80';
        dockerHelper.stopContainer(webAppContainer)
        dockerHelper.runContainer(webAppContainer)
    } 
}
