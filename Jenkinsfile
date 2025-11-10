properties([parameters([booleanParam(defaultValue: false, description: 'Выкладывать сборку на сервер files.qsolution.ru', name: 'Publish')])])
node {
   stage('CarGlass') {
      checkout([
         $class: 'GitSCM',
         branches: scm.branches,
         doGenerateSubmoduleConfigurations: scm.doGenerateSubmoduleConfigurations,
         extensions: scm.extensions + [submodule(disableSubmodules: false, recursiveSubmodules: true)],
         userRemoteConfigs: scm.userRemoteConfigs
      ])
   }
   stage('Restore Packages') {
         sh 'nuget restore My-FyiReporting/MajorsilenceReporting.sln'
        sh 'nuget restore QSProjects/QSProjectsLib.sln'
   	    sh 'nuget restore CarGlass.sln'        
   }
   stage('Build') {
        sh 'rm -f WinInstall/CarGlass-*.exe'
        sh 'WinInstall/makeWinInstall.sh'
        archiveArtifacts artifacts: 'WinInstall/CarGlass-*.exe', onlyIfSuccessful: true
   }
   if (params.Publish) {
      stage('Publish'){
         sh 'scp WinInstall/CarGlass-*.exe root@odysseus.srv.qsolution.ru:/var/www/files/CarGlass/'
      }
   }
}
