#!/bin/bash

echo "Initializing categories..."

# Path to the CSV file
csv_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/InitData/categories.csv"

# Check if isBulkUpdate parameter is passed
isBulkUpdate=$1

# Database connection details
db_host="localhost"
db_port="5432"
db_name="CIYW_DictionaryDb"
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
# Read the CSV file line by line
while IFS=';' read -r id parentId title icon color isActive isPositive;
do
  # Skip the header line
  if [ "$id" != "id" ]; then
    ((totalRecords++))
    # Check if the category with the specific ID already exists
    category_exists=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT 1 FROM \"$db_name\".\"Dictionaries\".\"Categories\" WHERE \"Id\" = $id;")

    # If the category does not exist, prepare the SQL for bulk insert
    if [ -z "$category_exists" ]; then
      # Convert boolean values to integers
      isActiveBool=$( [ "$isActive" == "1" ] && echo true || echo false )
      isPositiveBool=$( [ "$isPositive" == "1" ] && echo true || echo false )
      
      # Handle null parentId
      if [ -z "$parentId" ]; then
        parentId="NULL"
      fi

      if [ "$isBulkUpdate" == "true" ]; then
        # Construct the SQL command
        bulkInsertSQL+="INSERT INTO \"$db_name\".\"Dictionaries\".\"Categories\" 
        (\"Id\", \"Title\", \"Icon\", \"Color\", \"IsActive\", \"ParentId\", \"IsPositive\") 
        VALUES ($id, '$title', '$icon', '$color', $isActiveBool, $parentId, $isPositiveBool);"
        ((bulkCounter++))

        # If bulkCounter reaches 500, execute the bulk insert
        if [ $bulkCounter -ge 500 ]; then
          if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$bulkInsertSQL"; then
            echo "Bulk insert of $bulkCounter categories added successfully."
          else
            ((errorAdded+=bulkCounter))
            echo "Error adding bulk categories."
          fi
          # Reset the counter and SQL variable
          bulkCounter=0
          bulkInsertSQL=""
        fi
      else
        # Non-bulk insert
        sql="INSERT INTO \"$db_name\".\"Dictionaries\".\"Categories\" 
        (\"Id\", \"Title\", \"Icon\", \"Color\", \"IsActive\", \"ParentId\", \"IsPositive\") 
        VALUES ($id, '$title', '$icon', '$color', $isActiveBool, $parentId, $isPositiveBool);"
        if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$sql"; then
          echo "Category with ID $id added successfully."
        else
          ((errorAdded++))
          echo "Error adding category with ID $id." >> "$log_file"
        fi
      fi
    else
      ((alreadyExist++))
      echo "Category with ID $id already exists. Skipping insertion."
    fi
  fi
done < "$csv_file"

# Insert any remaining categories
if [ $bulkCounter -gt 0 ]; then
  if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$bulkInsertSQL"; then
    echo "Bulk insert of $bulkCounter remaining categories added successfully."
  else
    ((errorAdded+=bulkCounter))
    echo "Error adding remaining bulk categories." >> "$log_file"
  fi
fi

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "CATEGORIES" "$totalRecords" "$alreadyExist" "$errorAdded"
echo "Categories initialized."