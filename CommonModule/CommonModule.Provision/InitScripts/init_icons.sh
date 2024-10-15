#!/bin/bash

echo "Initializing icons..."

# Path to the CSV file
csv_file_icons=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/InitData/icons.csv"

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
bulkCounter=0
bulkInsertSQL=""

log_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/provision_logs.txt"

# Read the icons CSV file line by line
while IFS=';' read -r id title iconCategoryId isActive;
do
  # Skip the header line
  if [ "$id" != "id" ]; then
    ((totalRecords++))
    # Check if the icon with the specific ID already exists
    icon_exists=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT 1 FROM \"$db_name\".\"Dictionaries\".\"Icons\" WHERE \"Id\" = $id;")

    # If the icon does not exist, prepare the SQL for bulk insert
    if [ -z "$icon_exists" ]; then
      # Convert boolean values to integers
      isActiveBool=$( [ "$isActive" == "1" ] && echo true || echo false )

      # Construct the SQL command
      bulkInsertSQL+="INSERT INTO \"$db_name\".\"Dictionaries\".\"Icons\" (\"Id\", \"Title\", \"IconCategoryId\", \"IsActive\") VALUES ($id, '$title', $iconCategoryId, $isActiveBool);"
      ((bulkCounter++))

      # If bulkCounter reaches 900, execute the bulk insert
      if [ $bulkCounter -ge 900 ]; then
        if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$bulkInsertSQL"; then
          echo "Bulk insert of $bulkCounter icons added successfully."
        else
          ((errorAdded+=bulkCounter))
          echo "Error adding bulk icons."
        fi
        # Reset the counter and SQL variable
        bulkCounter=0
        bulkInsertSQL=""
      fi
    else
      ((alreadyExist++))
      echo "Icon with ID $id already exists. Skipping insertion."
    fi
  fi
done < "$csv_file_icons"

# Insert any remaining icons
if [ $bulkCounter -gt 0 ]; then
  if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$bulkInsertSQL"; then
    echo "Bulk insert of $bulkCounter remaining icons added successfully."
  else
    ((errorAdded+=bulkCounter))
    echo "Error adding remaining bulk icons." >> "$log_file"
  fi
fi

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "ICONS" "$totalRecords" "$alreadyExist" "$errorAdded"
echo "Icons initialized."