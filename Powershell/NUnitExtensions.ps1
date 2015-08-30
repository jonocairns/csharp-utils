function Copy-FilesFromDirectories($filter, $dest, $sourceDir, $config) {
	$itemsInFolder = Get-ChildItem $filter -Recurse | where { ! $_.PSIsContainer }
	Foreach($itemInFolder in $itemsInFolder) {
        $projectDirectory = $itemInFolder.Directory.FullName.replace("$sourceDir\","").replace("\bin\$config", "");
        $destinationProjectDir = "$dest\$projectDirectory"
        New-Item $destinationProjectDir -ItemType Directory -force > null
        $name = $itemInFolder.Name
        $destination = "$destinationProjectDir\$name"
        $filePath = $itemInFolder.FullName
		Copy-Item -path $filePath -dest $destination -force
	}
}

function Run-Nunit ($toolsDir, $testAssembly, $resultsDir)
{
    $nunit = "$toolsDir\NUnit\bin\nunit-console.exe"
    exec { &$nunit $testAssembly }
}