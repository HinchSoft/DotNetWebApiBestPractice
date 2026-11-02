$ErrorActionPreference = "Stop"

$repoRoot = (git rev-parse --show-toplevel).Trim()
Set-Location $repoRoot

git config --local core.hooksPath .githooks
git config --local alias.push-signoff "!git push && gh signoff"

Write-Host "core.hooksPath set to .githooks and alias.push-signoff configured (local to this repository)"
