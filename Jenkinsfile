pipeline {
    agent any
    
    stages {
        stage('Build') {
            steps {
                echo 'Hello 1'
            }
        }
        stage('Test') {
            steps {
                echo 'Hello 2'
            }
        }
    }
    
    post {
        always {           
        }
    }
}
