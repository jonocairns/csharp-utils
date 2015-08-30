function Build-VisualStudioSolution ($thingToBuild, $configuration, $visualStudioVersion = "12.0", $platform = "Any CPU") {
    set-alias msb "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
    exec { msb /nologo $thingToBuild /p:VisualStudioVersion=$visualStudioVersion /m /p:Configuration=$configuration /p:Platform=$platform }
}

function Clean-VisualStudioSolution ($thingToClean, $configuration, $visualStudioVersion = "12.0", $platform = "Any CPU") {
    set-alias msb "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
	exec { msb $thingToClean /t:Clean /p:Configuration=$configuration /m /nologo /p:VisualStudioVersion=$visualStudioVersion /p:Platform=$platform } 
}

function WebDeploy-MVCSolution ($thingToPublish, $publishProfile, $configuration, $visualStudioVersion = "12.0") {
	exec { msbuild $thingToPublish /p:DeployOnBuild=true /p:PublishProfile=$publishProfile /p:Configuration=$configuration /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}

function Package-MVCSolution ($thingToPublish, $configuration, $packagePath, $projectName, $packageName, $buildNumber, $iisAppPath, $visualStudioVersion = "12.0") {	
	exec { msbuild $thingToPublish /p:DeployOnBuild=true /P:Configuration="$configuration;PackageLocation=$packagePath\$projectName-$buildNumber-$configuration-$packageName.zip" /P:DeployIisAppPath="$iisAppPath" /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}

function Package-WebApiSolution ($thingToPublish, $configuration, $packagePath, $projectName, $packageName, $buildNumber, $iisAppPath, $visualStudioVersion = "12.0") {	
	exec { msbuild $thingToPublish /T:Package /P:Configuration="$configuration;PackageLocation=$packagePath\$projectName-$buildNumber-$configuration-$packageName.zip" /P:DeployIisAppPath="$iisAppPath" /m /nologo /p:VisualStudioVersion=$visualStudioVersion } 
}