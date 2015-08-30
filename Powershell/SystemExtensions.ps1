#
# Powershell script for adding/removing entries to the hosts file.
#
# Known limitations:
# - does not handle entries with comments afterwards ("<ip>    <host>    # comment")
#
 
$file = "$env:windir\System32\drivers\etc\hosts"
 
function Add-Host([string]$ip, [string]$hostname) {
	remove-host $hostname
	$ip + "`t`t" + $hostname | Out-File -encoding ASCII -append $file
}
 
function Remove-Host([string]$hostname) {
	$c = Get-Content $file
	$newLines = @()
	
	foreach ($line in $c) {
		$bits = [regex]::Split($line, "\t+")
		if ($bits.count -eq 2) {
			if ($bits[1] -ne $hostname) {
				$newLines += $line
			}
		} else {
			$newLines += $line
		}
	}
	
	# Write file
	Clear-Content $file
	foreach ($line in $newLines) {
		$line | Out-File -encoding ASCII -append $file
	}
}