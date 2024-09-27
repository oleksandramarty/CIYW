#!/bin/bash

echo "Updating cache version in Redis..."

# Database connection details
db_host="localhost"
db_port="5432"
db_name_localization="CIYW_LocalizationDb"
db_name_dictionary="CIYW_DictionaryDb"
db_user="postgres"
db_password="postgres"

# Export password to avoid password prompt
export PGPASSWORD=$db_password

# Get the current timestamp
current_timestamp=$(date +%Y%m%d%H%M%S)

# Get the absolute path of the script directory
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)

# Define the path to insert_cache_value.sh
INSERT_CACHE_VALUE_SCRIPT="$SCRIPT_DIR/insert_cache_value.sh"

# Ensure the script is executable
chmod +x "$INSERT_CACHE_VALUE_SCRIPT"

# Execute the script with the correct path
# Select count from localization table where localeId = 1
totalPublicCount=$(psql -h $db_host -p $db_port -d $db_name_localization -U $db_user -t -c "SELECT COUNT(*) FROM \"$db_name_localization\".\"Locales\".\"Localizations\" WHERE \"LocaleId\" = 1 AND \"IsPublic\" = TRUE;")
totalCount=$(psql -h $db_host -p $db_port -d $db_name_localization -U $db_user -t -c "SELECT COUNT(*) FROM \"$db_name_localization\".\"Locales\".\"Localizations\" WHERE \"LocaleId\" = 1;")
"$INSERT_CACHE_VALUE_SCRIPT" "localization_cache:version:localization" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "localization_cache:count:localization" "$totalPublicCount:$totalCount"

totalCount=$(psql -h $db_host -p $db_port -d $db_name_dictionary -U $db_user -t -c "SELECT COUNT(*) FROM \"$db_name_dictionary\".\"Dictionaries\".\"Categories\";")
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:version:category" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:count:category" $totalCount

totalCount=$(psql -h $db_host -p $db_port -d $db_name_dictionary -U $db_user -t -c "SELECT COUNT(*) FROM \"$db_name_dictionary\".\"Dictionaries\".\"Currencies\";")
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:version:currency" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:count:currency" $totalCount

totalCount=$(psql -h $db_host -p $db_port -d $db_name_dictionary -U $db_user -t -c "SELECT COUNT(*) FROM \"$db_name_dictionary\".\"Dictionaries\".\"Countries\";")
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:version:country" $current_timestamp
"$INSERT_CACHE_VALUE_SCRIPT" "dictionary_cache:count:country" $totalCount

# Unset the password variable for security
unset PGPASSWORD

echo "Cache version updated in Redis."