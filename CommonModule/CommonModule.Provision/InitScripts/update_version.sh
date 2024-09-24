#!/bin/bash

BASE_DIR=$(cd "$(dirname "$0")" && pwd | sed 's|/CommonModule/CommonModule.Provision/InitScripts||')

# Define the path to the environment folder
ENV_FOLDER="$BASE_DIR/Angular.WebClient/src/core/environments"

# List all environment files and extract their names, including environment.ts
ENV_FILES=$(ls "$ENV_FOLDER" | grep -oE 'environment(\.[^.]+)?\.ts')

# Generate a new build version
NEW_BUILD_VERSION=$(uuidgen | tr '[:upper:]' '[:lower:]' | tr -d '-')

# Loop through each environment file and update the build version
for ENV_FILE in $ENV_FILES;
do
  FULL_PATH="$ENV_FOLDER/$ENV_FILE"
  sed -i '' "s/buildVersion: '.*'/buildVersion: '$NEW_BUILD_VERSION'/" "$FULL_PATH"
  echo "buildVersion updated to $NEW_BUILD_VERSION in $FULL_PATH"
done

VERSION_FOLDER="$BASE_DIR/CommonModule/CommonModule.Facade"

# Write the new build version to version.txt in the VERSION_FOLDER
echo "$NEW_BUILD_VERSION" > "$VERSION_FOLDER/version.txt"
echo "New build version written to $VERSION_FOLDER/version.txt"