#!/bin/sh
set -eu

repo_root="$(git rev-parse --show-toplevel)"
cd "$repo_root"

git config --local core.hooksPath .githooks

chmod +x .githooks/pre-push ci/local-ci.sh ci/setup-hooks.sh 2>/dev/null || true

echo "core.hooksPath set to .githooks (local to this repository)"
