#!/bin/bash

echo "Caching countries in Redis..."

# Database connection details
db_host="localhost"
db_port="5432"
db_name="CIYW_DictionaryDb"
db_user="postgres"
db_password="postgres"

# Redis connection details
redis_host="localhost"
redis_port="6379"

# Export password to avoid password prompt
export PGPASSWORD=$db_password

# Initialize error counter
errorAdded=0

# Initialize global currentIndex to track across all tables
currentIndex=0

# Function to calculate total count for a table
calculate_total_count() {
  local sql=$1
  psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "$sql"
}

# Function to cache table data
cache_table_data() {
  local table_name=$1
  local key_pattern=$2
  local sql=$3
  local total_count=$4

  # Execute the query and process the results
  psql -h $db_host -p $db_port -d $db_name -U $db_user -t -A -F $'\t' -c "$sql" | while IFS=$'\t' read -r id title code titleEn isActive
  do
    # Increment the global index
    ((currentIndex++))

    # Determine the value for isActive
    if [ "$isActive" == "t" ]; then
      isActiveValue=1
    else
      isActiveValue=0
    fi

    # Set the data in Redis using the specified key pattern
    key=$(echo "$key_pattern" | sed "s/{ID}/$id/")
    if redis-cli -h $redis_host -p $redis_port SET "$key" "{\"id\":\"$id\",\"title\":\"$title\",\"code\":\"$code\",\"titleEn\":\"$titleEn\",\"isActive\":$isActiveValue}"; then
      percentage=$((currentIndex * 100 / total_count))
      echo "Redis cached dictionaries for $table_name: $currentIndex/$total_count = $percentage%"
    else
      ((errorAdded++))
      echo "Error caching $table_name with ID $id." >> "$log_file"
    fi
  done

  echo "Finished caching $table_name." >> "$log_file"
}

log_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/provision_logs.txt"

# Calculate total count for countries
totalCountCountries=$(calculate_total_count "SELECT COUNT(*) FROM \"Dictionaries\".\"Countries\";")

# Cache countries
cache_table_data "Countries" "dictionary_cache:country:{ID}" "SELECT \"Id\", \"Title\", \"Code\", \"TitleEn\", \"IsActive\" FROM \"Dictionaries\".\"Countries\";" "$totalCountCountries"

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "CACHE COUNTRIES" "$totalCountCountries" "0" "$errorAdded"