#!/bin/bash

echo "Updating cache version in Redis..."

# Get the absolute path of the script directory
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)

# Define the path to insert_cache_value.sh
INSERT_CACHE_VALUE_SCRIPT="$SCRIPT_DIR/insert_cache_value.sh"

# Ensure the script is executable
chmod +x "$INSERT_CACHE_VALUE_SCRIPT"

# Execute the script with the correct path
"$INSERT_CACHE_VALUE_SCRIPT" "version:localization_public" $(uuidgen | tr -d '-')
"$INSERT_CACHE_VALUE_SCRIPT" "version:localization" $(uuidgen | tr -d '-')
"$INSERT_CACHE_VALUE_SCRIPT" "version:category" $(uuidgen | tr -d '-')
"$INSERT_CACHE_VALUE_SCRIPT" "version:currency" $(uuidgen | tr -d '-')
"$INSERT_CACHE_VALUE_SCRIPT" "version:country" $(uuidgen | tr -d '-')
"$INSERT_CACHE_VALUE_SCRIPT" "version:locale" $(uuidgen | tr -d '-')

echo "Cache version updated in Redis."