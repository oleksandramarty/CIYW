#!/bin/bash

echo "Caching all localizations in Redis..."

# Database connection details
db_host="localhost"
db_port="5432"
db_name="CIYW_LocalizationDb"
db_user="postgres"
db_password="postgres"

# Redis connection details
redis_host="localhost"
redis_port="6379"

# Export password to avoid password prompt
export PGPASSWORD=$db_password

# Initialize error counter
errorAdded=0

# Function to map localeId to locale code
get_locale_code() {
  case $1 in
    1) echo "en" ;;
    2) echo "es" ;;
    3) echo "fr" ;;
    4) echo "ua" ;;
    5) echo "ru" ;;
    6) echo "de" ;;
    7) echo "it" ;;
    *) echo "unknown" ;;
  esac
}

currentIndex=0

# Query to count all localizations
totalCount=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT COUNT(*) FROM \"$db_name\".\"Locales\".\"Localizations\";")

# Query to list all localizations
sql="SELECT \"Id\", \"Key\", \"Value\", \"LocaleId\", \"IsPublic\" FROM \"$db_name\".\"Locales\".\"Localizations\";"

log_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/provision_logs.txt"

# Execute the query and process the results
psql -h $db_host -p $db_port -d $db_name -U $db_user -t -A -F $'\t' -c "$sql" | while IFS=$'\t' read -r id key value localeId isPublic
do
  # Increment the index
  ((currentIndex++))
  
  # Get the locale code from localeId
  localeCode=$(get_locale_code $localeId)

  # Determine the value for isPublic
  if [ "$isPublic" == "t" ]; then
    isPublicValue=1
  else
    isPublicValue=0
  fi

  # Set the localization in Redis using the new key pattern
  if redis-cli -h $redis_host -p $redis_port SET "localization_cache:$localeCode:$key:$isPublicValue" "$value"; then
    percentage=$((currentIndex * 100 / totalCount))
    echo "Redis cached: $currentIndex/$totalCount = $percentage%"
  else
    ((errorAdded++))
    echo "Error caching localization with ID $id." >> "$log_file"
  fi
done

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "CACHE LOCALIZATIONS" "$totalCount" "0" "$errorAdded"