#!/bin/bash

echo "Updating cache version in Redis..."

# Get the current timestamp
current_timestamp=$(date +%Y%m%d%H%M%S)

# Get the absolute path of the script directory
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)

# Define the path to insert_cache_value.sh
INSERT_CACHE_VALUE_SCRIPT="$SCRIPT_DIR/insert_cache_value.sh"

# Ensure the script is executable
chmod +x "$INSERT_CACHE_VALUE_SCRIPT"

# Execute the script with the correct path
"$INSERT_CACHE_VALUE_SCRIPT" "localization_cache:version:localization" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:version:category" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:version:currency" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:version:country" $current_timestamp

echo "Cache version updated in Redis."