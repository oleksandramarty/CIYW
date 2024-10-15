#!/bin/bash

echo "Initializing category icons..."

# Path to the CSV file
csv_file_category_icons=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/InitData/category_icons.csv"

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

# Read the category icons CSV file line by line
while IFS=';' read -r id title isActive;
do
  # Skip the header line
  if [ "$id" != "id" ]; then
    ((totalRecords++))
    # Check if the category icon with the specific ID already exists
    category_icon_exists=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT 1 FROM \"$db_name\".\"Dictionaries\".\"IconCategories\" WHERE \"Id\" = $id;")

    # If the category icon does not exist, prepare the SQL for insert
    if [ -z "$category_icon_exists" ]; then
      # Convert boolean values to integers
      isActiveBool=$( [ "$isActive" == "1" ] && echo true || echo false )

      # Non-bulk insert
      sql="INSERT INTO \"$db_name\".\"Dictionaries\".\"IconCategories\"
      (\"Id\", \"Title\", \"IsActive\")
      VALUES ($id, '$title', $isActiveBool);"
      if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$sql"; then
        echo "Category icon with ID $id added successfully."
      else
        ((errorAdded++))
        echo "Error adding category icon with ID $id." >> "$log_file"
      fi
    else
      ((alreadyExist++))
      echo "Category icon with ID $id already exists. Skipping insertion."
    fi
  fi
done < "$csv_file_category_icons"

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "CATEGORY ICONS" "$totalRecords" "$alreadyExist" "$errorAdded"
echo "Category icons initialized."