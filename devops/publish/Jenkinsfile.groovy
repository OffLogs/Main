@Library('common')
import com.shared.jenkins.docker.DockerHelper
import com.shared.jenkins.docker.DockerContainer

def dockerHelper = new DockerHelper(this)

def containers = [
    new DockerContainer(
        tag: 'offlogs-migration-production',
        dockerFile: 'devops/publish/migrations/Dockerfile',
        isRunAlways: false,
        isRunInBackground: false
    ),
    new DockerContainer(
        tag: 'offlogs-api-production',
        dockerFile: 'devops/publish/api/Dockerfile',
        port: '6056:80'
    ),
    new DockerContainer(
        tag: 'offlogs-api-frontend-production',
        dockerFile: 'devops/publish/front/Dockerfile',
        port: '6057:80'
    ),
    new DockerContainer(
        tag: 'offlogs-worker-production',
        dockerFile: 'devops/publish/worker/Dockerfile',
    ),
    new DockerContainer(
        tag: 'offlogs-web-production',
        dockerFile: 'devops/publish/worker/Dockerfile',
        port: '6058:80',
    ),
];

node('abedor-mainframe-web') {
    env.ENVIRONMENT = "Development"

    stage('Checkout') {
        checkout scm
    }

    stage('Init variables') {
        String dbConnectionString;
        
        containers.collect {
            // Kafka
            it.envVariables.put('Kafka__Servers', '127.0.0.1:29092')
            it.envVariables.put('Kafka__ProducerId', 'offlogs-reducer-1')
            it.envVariables.put('Kafka__Topic_Logs', 'production-logs')
            it.envVariables.put('Kafka__Topic_Notification', 'offlogs-notification-logs')
            it.envVariables.put('Kafka__ConsumerClientId', 'client-1')
        
            withCredentials([
                    usernamePassword(credentialsId: "offlogs_production_db_credentials", usernameVariable: 'USER_NAME', passwordVariable: 'PASSWORD')
            ]) {
                it.envVariables.put(
                    'ConnectionStrings__DefaultConnection',
                    "User ID=${USER_NAME};Password=${PASSWORD};Host=192.168.99.6;Port=5432;Database=offlogs;Pooling=true;"
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
                it.envVariables.put('Smtp__Port', '465')
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

    stage('Stop containers') {
        for (String container: containers) {
            dockerHelper.stopContainer(container)
        }
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
    
    stage('Build containers') {
        for (String container: containers) {
            dockerHelper.buildContainer(container)
        }
    }

    stage('Run migrations') {
        dockerHelper.runContainer(containers[0])
    }

    stage('Start containers') {
        def containersToRun = containers.drop(1)
        for (String container: containersToRun) {
            dockerHelper.runContainer(container)
        }
    }
}
