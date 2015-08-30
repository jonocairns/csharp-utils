properties {
	#----------
	#Properties
	#PSake properties, these cannot be changed once they're defined
	#----------

	#----------	
	#Deployment Variables
	#----------	

	#----------	
	#Environmental Variables for Development
	#----------	
	$projectName = "Obrien.Connect"
    $buildNumber = "DEV"
	$config = "Release"
    
	#----------	
	#Directories
	#----------
	$baseDir = Resolve-Path .
    $sourceDir = "$baseDir\Source"
	$toolsDir = "$baseDir\Tools"
	$scriptsDir = "$baseDir\Scripts"
    $automationDir = "$baseDir\Automation"
    $artefactsDir = "$baseDir\Artefacts"
    		
	#----------	
	#Variable dependent properties
	#Properties that depend on other variables to be defined first
	#----------
	$solutionFilePath = "$sourceDir\$projectName.sln"
	
	#----------	
	#Project Specific Variables
	#----------
	$visualStudioVersion = "12.0"
}

#----------	
#Includes
#Script files to include for function definitions
#----------
$path = Resolve-Path .
Include Scripts\Logger.ps1 -ErrorAction Stop
Include Scripts\VisualStudioExtensions.ps1 -ErrorAction Stop
Include Scripts\NUnitExtensions.ps1 -ErrorAction Stop
Include Scripts\SystemExtensions.ps1 -ErrorAction Stop

#-------------
#Build Targets
#Targets to build
#-------------

#-------------------------
#Targets
#-------------------------

task default -depends initial

task initial -depends preamble, clean, compile, copyDlls, runUnitTests, postamble

#-----------
#Build Steps
#Steps to execute
#-----------

task preamble {
	Log "Excecuting build number: $buildNumber Mode: $config" Green
}

task postamble {
	$date = Get-Date
	Log "Build number $buildNumber complete at $date" Green
}

task clean {
	Log "Cleaning solution $solutionFilePath" Green
	Clean-VisualStudioSolution $solutionFilePath $config $visualStudioVersion
	Clean-VisualStudioSolution $solutionFilePath "Debug" $visualStudioVersion
}

task compile {
	Log "Building solution $solutionFilePath in $config mode" Green
    Build-VisualStudioSolution $solutionFilePath $config $visualStudioVersion
}

task copyDlls {
    Log "Recreating artefacts directory" Green
    Remove-Item $artefactsDir -Recurse -Confirm:$false -ErrorAction SilentlyContinue
    New-Item -ItemType Directory $artefactsDir
    Log "Artefacts directory recreated" Green
    
    Copy-FilesFromDirectories "$sourceDir\*Test*\bin\$config" $artefactsDir $sourceDir $config
    Remove-Item "$artefactsDir\AxGatewayTestApplication" -Recurse
}

task runUnitTests {
    Log "Running Unit Tests" Green  
    $tests = Get-ChildItem $artefactsDir  -filter "*test.dll" -Recurse
    set-alias mst "${env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

    foreach($test in $tests) {
        $directory = $test.Directory.FullName
        $testToRunName = $test.Directory.Name + ".dll"
        if ($test.Name -eq $testToRunName) {
            Push-Location $directory
                Log "Running test suite $testToRunName" Green
                exec { mst $testToRunName /TestCaseFilter:"TestCategory!=Local" /UseVsixExtensions:true }
            Pop-Location    
        }
    }
}

task createAndUploadDeploymentPackages {
    $labelForPackages = $projectName + "_" + $buildNumber
    & "$automationDir\CreateDeploymentPackage.ps1" -buildDirectory $baseDir -buildLabel $labelForPackages -sourcePath $sourceDir -configuration "Release-Dev"
	& "$automationDir\UploadPackageToBlobStorage.ps1" -BuildLabel $labelForPackages
    & "$automationDir\CreateDeploymentPackage.ps1" -buildDirectory $baseDir -buildLabel $labelForPackages -sourcePath $sourceDir -configuration "Release-UAT"
    & "$automationDir\UploadPackageToBlobStorage.ps1" -BuildLabel $labelForPackages
}

#-----------
#Generalised functions
#Any other generic functions can be stored here
#-----------