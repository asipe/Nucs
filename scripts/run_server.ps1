$path = Split-Path (Split-Path $MyInvocation.MyCommand.Path)
$path = Join-Path $path 'debug\apps\Nucs.Console\nucs.exe'
Write-Host -ForegroundColor Green "Starting $path"
$process = new-object System.Diagnostics.ProcessStartInfo $path
$process.Verb = 'runas'
[System.Diagnostics.Process]::Start($process)
