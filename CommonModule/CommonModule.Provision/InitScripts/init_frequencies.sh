#!/bin/bash

echo "Initializing frequencies..."

# Path to the CSV file
csv_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/InitData/frequencies.csv"

# Database connection details
db_host="localhost"
db_port="5432"
source "$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')/provision_helpers.sh"
db_name=$(getDbNameDictionary)
db_user="postgres"
db_password="postgres"

# Export password to avoid password prompt
export PGPASSWORD=$db_password

# Initialize counters
totalRecords=0
alreadyExist=0
errorAdded=0

log_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/provision_logs.txt"
# Read the CSV file line by line
while IFS=';' read -r id title description type isActive;
do
  # Skip the header line
  if [ "$id" != "id" ]; then
    ((totalRecords++))
    # Check if the frequency with the specific ID already exists
    frequency_exists=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT 1 FROM \"$db_name\".\"Dictionaries\".\"Frequencies\" WHERE \"Id\" = $id;")

    # If the frequency does not exist, prepare the SQL for insert
    if [ -z "$frequency_exists" ]; then
      # Convert boolean values to integers
      isActiveBool=$( [ "$isActive" == "1" ] && echo true || echo false )

      # Non-bulk insert
      sql="INSERT INTO \"$db_name\".\"Dictionaries\".\"Frequencies\"
      (\"Id\", \"Title\", \"Description\", \"IsActive\", \"Type\")
      VALUES ($id, '$title', '$description', $isActiveBool, $type);"
      if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$sql"; then
        echo "Frequency with ID $id added successfully."
      else
        ((errorAdded++))
        echo "Error adding frequency with ID $id." >> "$log_file"
      fi
    else
      ((alreadyExist++))
      echo "Frequency with ID $id already exists. Skipping insertion."
    fi
  fi
done < "$csv_file"

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "FREQUENCIES" "$totalRecords" "$alreadyExist" "$errorAdded"
echo "Frequencies initialized."