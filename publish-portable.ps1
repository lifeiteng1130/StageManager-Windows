param(
    [Parameter(Mandatory=$true)][string]$DotnetPath,
    [Parameter(Mandatory=$true)][string]$OutputDirectory
)
$ErrorActionPreference = 'Stop'
& $DotnetPath publish (Join-Path $PSScriptRoot 'StageManager\StageManager.csproj') -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:RestoreSources=https://api.nuget.org/v3/index.json -o $OutputDirectory
if ($LASTEXITCODE -ne 0) { throw 'Publish failed' }
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'LICENSE') -Destination $OutputDirectory
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'PORTABLE-README.zh-CN.txt') -Destination (Join-Path $OutputDirectory 'README.zh-CN.txt')
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'Restore-Windows.cmd') -Destination (Join-Path $OutputDirectory '恢复窗口.cmd')
[IO.File]::WriteAllText((Join-Path $OutputDirectory 'portable.flag'), '')
