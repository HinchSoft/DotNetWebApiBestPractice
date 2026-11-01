$ErrorActionPreference = "Stop"

$repoRoot = (git rev-parse --show-toplevel).Trim()
Set-Location $repoRoot

git config --local core.hooksPath .githooks

Write-Host "core.hooksPath set to .githooks (local to this repository)"
