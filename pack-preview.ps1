<#
.SYNOPSIS
    Shows the NuGet packages (and versions) that `dotnet pack qowaiv.slnx` would produce.

.DESCRIPTION
    Read-only: queries evaluated MSBuild properties per project. It does not build,
    restore, or create any packages. Each package also gets a .snupkg because
    IncludeSymbols is enabled in props/package.props.

.EXAMPLE
    ./tools/pack-preview.ps1
#>
[CmdletBinding()]
param(
    [string] $Solution = (Join-Path $PSScriptRoot 'qowaiv.slnx')
)

$ErrorActionPreference = 'Stop'
$solutionPath = (Resolve-Path $Solution).Path
$solutionDir = Split-Path $solutionPath -Parent

$projects = dotnet sln $solutionPath list |
    Where-Object { $_ -match '\.csproj\s*$' } |
    ForEach-Object {
        $project = $_.Trim()
        if ([System.IO.Path]::IsPathRooted($project)) { $project } else { Join-Path $solutionDir $project }
    }

$packages = foreach ($project in $projects) {
    $info = dotnet msbuild $project -nologo `
        -getProperty:PackageId `
        -getProperty:PackageVersion `
        -getProperty:IsPackable | ConvertFrom-Json

    if ($info.Properties.IsPackable -eq 'true') {
        [pscustomobject]@{
            Package = $info.Properties.PackageId
            Version = $info.Properties.PackageVersion
        }
    }
}

$packages | Sort-Object Package | Format-Table -AutoSize
Write-Host "Total: $($packages.Count) packages (+ a .snupkg for each)."
