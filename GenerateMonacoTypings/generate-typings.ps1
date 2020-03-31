# This script run after downloading the npm package dependencies inorder to generate the C# typings.
# https://github.com/hez2010/TypedocConverter

# Requires .NET Core Runtime 3.1.x to be installed

# ------------------------
$monaco_file = ".\node_modules\monaco-editor\monaco.d.ts"
$typedoc_bin_url = $env:npm_package_config_typedocConverter #"https://github.com/hez2010/TypedocConverter/releases/download/v1.5/Windows_x64.zip"
$temp_dir_name = ".temp"

function Get-ScriptDirectory {
    Split-Path -parent $PSCommandPath
}

$script_dir = Get-ScriptDirectory

Push-Location $script_dir

# Create Temp Directory and Output
New-Item -Name $temp_dir_name -ItemType Directory | Out-Null

# Make sure we can see our 'monaco.d.ts' file

if (!(Test-Path $monaco_file -PathType Leaf)) {
    Write-Error "Monaco Definitions Not Found, run npm install first."

    Pop-Location
    exit
}

# Copy monaco.d.ts to monaco.ts in temp folder (need to change extension)

Copy-Item $monaco_file -Destination (Join-Path $temp_dir_name "monaco.ts")

Push-Location $temp_dir_name

# Run typedoc to generate json representation
Invoke-Expression "npx typedoc monaco.ts --json monaco.json"

# Need TypedocConverter next
Write-Host "Downloading TypedocConverter"

[Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls"
Invoke-WebRequest -Uri $typedoc_bin_url -OutFile "TypedocConverter.zip"

Write-Host "Extracting..."
Expand-Archive "TypedocConverter.zip" -DestinationPath .

# Now run TypedocConverter on our monaco.json

Invoke-Expression ".\TypedocConverter.exe --inputfile monaco.json --splitfiles true --outputdir ../$env:npm_package_config_outdir --promise-type WinRT"

Pop-Location

# Clean-up Temp Dir
Remove-Item $temp_dir_name -Force -Recurse -ErrorAction SilentlyContinue

Pop-Location
