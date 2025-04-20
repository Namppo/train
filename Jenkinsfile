// Jenkinsfile
pipeline {
    agent any

    // (원하는 경우 여기에 triggers { githubPush() } 또는 pollSCM('H/5 * * * *') 추가)
    environment {
        // 복사할 대상 디렉터리와 서비스 명
        TARGET_DIR = 'C:\\Node_Server\\Build'
        SERVICE_NAME = 'MyNodeApp'
    }

    stages {
        stage('Checkout') {
            steps {
                // SCM 설정대로 체크아웃
                checkout scm
            }
        }

        stage('Deploy') {
            steps {
                echo ">> Copying Build artifacts to ${env.TARGET_DIR}"
                // robocopy 로 동기화 (/MIR: 미러, /NFL·/NDL: 로깅 최소화)
                bat """
                robocopy "%WORKSPACE%\\Build" "${env.TARGET_DIR}" /MIR /NFL /NDL
                """

                echo ">> Restarting Windows service ${env.SERVICE_NAME}"
                // NSSM 으로 서비스 재시작
                bat "nssm restart ${env.SERVICE_NAME}"
                // 또는 PowerShell로: bat "powershell -Command \"Restart-Service ${env.SERVICE_NAME} -Force\""
            }
        }
    }

    post {
        success {
            echo '✅ Deployment succeeded'
        }
        failure {
            echo '❌ Deployment failed'
        }
    }
}
