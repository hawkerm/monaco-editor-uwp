# Run this script using PowerShell to download and 
# install dependencies into the project directory before building.

# Reference to Monaco Version to Use in the Package
$monaco_version = "0.13.0"

# ------------------------
$monaco_tgz_url = "https://registry.npmjs.org/monaco-editor/-/monaco-editor-$monaco_version.tgz"
$sharp_zip_lib_url = "https://github.com/icsharpcode/SharpZipLib/releases/download/0.86.0.518/ICSharpCode.SharpZipLib.dll"
$temp_dir_name = ".temp"

function Get-ScriptDirectory {
    Split-Path -parent $PSCommandPath
}

$script_dir = Get-ScriptDirectory

Push-Location $script_dir

function Extract-TGZ {
    Param([string]$gzArchiveName, [string] $destFolder)
    
    $inStream = [System.IO.File]::OpenRead($gzArchiveName)
    $gzipStream = New-Object ICSharpCode.SharpZipLib.GZip.GZipInputStream -ArgumentList $inStream

    $tarArchive = [ICSharpCode.SharpZipLib.Tar.TarArchive]::CreateInputTarArchive($gzipStream);
    $tarArchive.ExtractContents($destFolder);
    $tarArchive.Close()

    $gzipStream.Close()
    $inStream.Close()
}

$message  = "Erase monaco-editor directory and download Monaco dependency?"
$question = "Are you sure you want to proceed?"

$choices = New-Object Collections.ObjectModel.Collection[Management.Automation.Host.ChoiceDescription]
$choices.Add((New-Object Management.Automation.Host.ChoiceDescription -ArgumentList '&Yes', "Proceed with clean-up and download."))
$choices.Add((New-Object Management.Automation.Host.ChoiceDescription -ArgumentList '&No', "Abort."))

$decision = $Host.UI.PromptForChoice($message, $question, $choices, 1)
if ($decision -eq 0) {
    # Remove Old Dependency
    Remove-Item ".\MonacoEditorComponent\monaco-editor" -Force -Recurse -ErrorAction SilentlyContinue

    # Create Temp Directory and Output
    New-Item -Name $temp_dir_name -ItemType Directory | Out-Null
    New-Item -Name ".\MonacoEditorComponent\monaco-editor" -ItemType Directory | Out-Null

    Write-Host "Downloading SharpZipLib"
    [Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
    Invoke-WebRequest -Uri $sharp_zip_lib_url -OutFile ".\$temp_dir_name\SharpZipLib.dll"

    Write-Host "Downloading Monaco"

    [Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
    Invoke-WebRequest -Uri $monaco_tgz_url -OutFile ".\$temp_dir_name\monaco.tgz"

    Write-Host "Extracting..."

    # Load Sharp Zip Lib so we can unpack Monaco
    # Load in memory so we can delete the dll after.
    [System.Reflection.Assembly]::Load([IO.File]::ReadAllBytes("$script_dir\$temp_dir_name\SharpZipLib.dll")) | Out-Null

    Extract-TGZ "$script_dir\$temp_dir_name\monaco.tgz" "$script_dir\$temp_dir_name\monaco"

    Copy-Item -Path ".\$temp_dir_name\monaco\package\*" -Destination ".\MonacoEditorComponent\monaco-editor" -Recurse

    # Clean-up Temp Dir
    Remove-Item $temp_dir_name -Force -Recurse -ErrorAction SilentlyContinue
}

Pop-Location