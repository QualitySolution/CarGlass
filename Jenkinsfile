properties([parameters([booleanParam(defaultValue: false, description: 'Выкладывать сборку на сервер files.qsolution.ru', name: 'Publish')])])
node {
   stage('CarGlass') {
      checkout([
         $class: 'GitSCM',
         branches: scm.branches,
         doGenerateSubmoduleConfigurations: scm.doGenerateSubmoduleConfigurations,
         extensions: scm.extensions + [[$class: 'RelativeTargetDirectory', relativeTargetDir: 'CarGlass']],
         userRemoteConfigs: scm.userRemoteConfigs
      ])
   }
   stage('QSProjects') {
      checkout([$class: 'GitSCM', branches: [[name: '*/release/1.5']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'RelativeTargetDirectory', relativeTargetDir: 'QSProjects']], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/QualitySolution/QSProjects.git']]])
      sh 'nuget restore QSProjects/QSProjectsLib.sln'
   }
   stage('Gtk.DataBindings') {
      checkout changelog: false, poll: false, scm: [$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'RelativeTargetDirectory', relativeTargetDir: 'Gtk.DataBindings']], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/QualitySolution/Gtk.DataBindings.git']]]
   }
   stage('GammaBinding') {
      checkout changelog: false, poll: false, scm: [$class: 'GitSCM', branches: [[name: '*/master']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'RelativeTargetDirectory', relativeTargetDir: 'GammaBinding']], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/QualitySolution/GammaBinding.git']]]
   }
   stage('My-FyiReporting') {
      checkout changelog: false, scm: [$class: 'GitSCM', branches: [[name: '*/QSBuild']], doGenerateSubmoduleConfigurations: false, extensions: [[$class: 'RelativeTargetDirectory', relativeTargetDir: 'My-FyiReporting']], submoduleCfg: [], userRemoteConfigs: [[url: 'https://github.com/QualitySolution/My-FyiReporting.git']]]
      sh 'nuget restore My-FyiReporting/MajorsilenceReporting.sln'
      //sh 'nuget restore My-FyiReporting/MajorsilenceReporting-Linux-GtkViewer.sln'
   }
   stage('Build') {
   	    sh 'nuget restore CarGlass/CarGlass.sln'
        sh 'rm -f CarGlass/WinInstall/CarGlass-*.exe'
        sh 'CarGlass/WinInstall/makeWinInstall.sh'
        recordIssues enabledForFailure: true, tool: msBuild()
        archiveArtifacts artifacts: 'CarGlass/WinInstall/CarGlass-*.exe', onlyIfSuccessful: true
   }
   if (params.Publish) {
      stage('Publish'){
         sh 'scp CarGlass/WinInstall/CarGlass-*.exe a218160_qso@a218160.ftp.mchost.ru:subdomains/files/httpdocs/CarGlass/'
      }
   }
}
