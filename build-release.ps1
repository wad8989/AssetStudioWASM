<#
.SYNOPSIS
    Builds AssetStudioWASM (dotnet publish, matching the workspace's
    "Build WASM - Publish" task) and packages dist/ into a release zip.
#>

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$RepoRoot = $PSScriptRoot
$WebAdapterDir = Join-Path $RepoRoot "AssetStudio.WebAdapter"
$DistDir = Join-Path $RepoRoot "dist"

$version = Read-Host "Enter version string (leave blank for 'dev')"
if ([string]::IsNullOrWhiteSpace($version)) {
    $version = "dev"
}

$AppBundleDir = Join-Path $DistDir "AppBundle"
if (Test-Path $AppBundleDir) {
    # Only dist/AppBundle is build output (Deploy:Dist MSBuild target); dist/AssetStudioWASM.js
    # is a hand-maintained wrapper that isn't regenerated, so leave the rest of dist/ alone.
    Write-Host "Cleaning dist/AppBundle..." -ForegroundColor Cyan
    Remove-Item -Recurse -Force $AppBundleDir
}

Write-Host "Building ($Configuration)..." -ForegroundColor Cyan
Push-Location $WebAdapterDir
try {
    dotnet publish -c $Configuration
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed with exit code $LASTEXITCODE"
    }
}
finally {
    Pop-Location
}

if (-not (Test-Path $DistDir)) {
    throw "dist/ not found at $DistDir - build did not produce expected output"
}

$zipName = "AssetStudioWASM-$version.zip"
$zipPath = Join-Path $RepoRoot $zipName

Write-Host "Packaging $zipName..." -ForegroundColor Cyan

$stagingRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("AssetStudioWASM-pack-" + [System.Guid]::NewGuid())
$stagingDist = Join-Path (Join-Path $stagingRoot "AssetStudioWASM") "dist"
New-Item -ItemType Directory -Force -Path $stagingDist | Out-Null

try {
    Copy-Item -Path (Join-Path $DistDir "*") -Destination $stagingDist -Recurse -Force

    if (Test-Path $zipPath) {
        Remove-Item $zipPath -Force
    }
    Compress-Archive -Path (Join-Path $stagingRoot "AssetStudioWASM") -DestinationPath $zipPath -CompressionLevel Optimal
}
finally {
    Remove-Item -Recurse -Force $stagingRoot -ErrorAction SilentlyContinue
}

Write-Host "Created $zipPath" -ForegroundColor Green
