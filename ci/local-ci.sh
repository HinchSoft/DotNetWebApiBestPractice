#!/bin/sh
set -eu

cd "$(git rev-parse --show-toplevel)"

marker="$(git rev-parse --git-dir)/ci-passed"

if [ "$(cat "$marker" 2>/dev/null)" = "$(git rev-parse HEAD)" ]; then
  echo "gate already passed for $(git rev-parse --short HEAD)"
else
  dotnet build DotNetWebApiBestPractice.slnx -c Release -warnaserror
  dotnet test DotNetWebApiBestPractice.slnx -c Release --no-build
  dotnet format DotNetWebApiBestPractice.slnx --verify-no-changes
  git rev-parse HEAD > "$marker"
fi

if [ "${1:-}" = "--signoff" ]; then
  gh signoff
fi
