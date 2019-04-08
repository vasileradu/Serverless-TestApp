param (
    [Parameter(Mandatory=$false)][string]$phasePath="phase*"
)

$basePath = "C:\Projects\Serverless-TestApp\Evaluation\Results\$phasePath"

Get-ChildItem $basePath -Directory | ForEach-Object {

	$inputPath = $_;
	$outputPath = "influx_" + [string]$_.Name + ".out"

	Write-Host "input: $inputPath"
	Write-Host "output: $outputPath"

	& .\..\ConvertAllCsvToInflux.ps1 -path $inputPath -output $outputPath;
	
	$upperBound = 15MB # calculated by Powershell	
	$writtenLines = 0;
	$maxLines = 80000;	
	
	if((Get-ChildItem -path $outputPath).Length -ge $upperBound){
		
		Write-Host "Spliting large file: $outputPath"  -ForegroundColor Yellow
				
		$i=0; Get-Content $outputPath -ReadCount $maxLines | %{$i++; $_ | ForEach-Object { return "$_`n" } | Out-File $outputPath"_"$i -Append -NoNewLine -Encoding "ascii"}

		Rename-Item -Path $outputPath -NewName "split_$outputPath"
	}
}

Write-Host "------------- All done ------------------ "  -ForegroundColor Green