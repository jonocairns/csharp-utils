Function Log ($message, $colour, $backgroundColour) {
	if ($backgroundColour -eq $null) {
		Write-Host $message -Foreground $colour
	} 
	else {
		Write-Host $message -Foreground $colour -Background $backgroundColour
	}
}