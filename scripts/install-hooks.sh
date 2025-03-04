#!/bin/sh

echo "Installing Git pre-commit hook..."
mkdir -p .git/hooks
cp scripts/pre-commit .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit

if [ -f .git/hooks/pre-commit ]; then
  echo "✅ Pre-commit hook installed!"
else
  echo "❌ ERROR: Failed to install the pre-commit hook!"
  exit 1
fi
