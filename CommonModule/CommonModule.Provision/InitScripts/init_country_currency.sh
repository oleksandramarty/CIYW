#!/bin/bash

echo "Initializing country-currency relationships..."

# Path to the CSV file
csv_file=$(cd "$(dirname "$0")" && pwd | sed 's|/InitScripts||')"/InitData/country_currency.csv"

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
# while IFS=';' read -r countryCode currencyCode; do
while IFS=';' read -r countryId countryCode currencyId currencyCode; do
  # Skip the header line
  if [ "$countryCode" != "countryCode" ]; then
    ((totalRecords++))

    # Fetch CountryId and CurrencyId for each line
    # countryId=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT \"Id\" FROM \"$db_name\".\"Dictionaries\".\"Countries\" WHERE \"Code\" = '$countryCode';" | xargs)
    # currencyId=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT \"Id\" FROM \"$db_name\".\"Dictionaries\".\"Currencies\" WHERE \"Code\" = '$currencyCode';" | xargs)

    # Check if the country-currency relationship already exists
    relationship_exists=$(psql -h $db_host -p $db_port -d $db_name -U $db_user -t -c "SELECT 1 FROM \"$db_name\".\"Dictionaries\".\"CountryCurrencies\" WHERE \"CountryId\" = $countryId AND \"CurrencyId\" = $currencyId;")

    # If the relationship does not exist, prepare the SQL for bulk insert
    if [ -z "$relationship_exists" ]; then
      if [ "$isBulkUpdate" == "true" ]; then
        bulkInsertSQL+="INSERT INTO \"$db_name\".\"Dictionaries\".\"CountryCurrencies\" (\"CountryId\", \"CurrencyId\") VALUES ($countryId, $currencyId);"
        ((bulkCounter++))

        # If bulkCounter reaches 500, execute the bulk insert
        if [ $bulkCounter -ge 500 ]; then
          if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$bulkInsertSQL"; then
            echo "Bulk insert of $bulkCounter country-currency relationships added successfully."
          else
            ((errorAdded+=bulkCounter))
            echo "Error adding bulk country-currency relationships."
          fi
          # Reset the counter and SQL variable
          bulkCounter=0
          bulkInsertSQL=""
        fi
      else
        # Non-bulk insert
        sql="INSERT INTO \"$db_name\".\"Dictionaries\".\"CountryCurrencies\" (\"CountryId\", \"CurrencyId\") VALUES ($countryId, $currencyId);"
        if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$sql"; then
          echo "Country-currency relationship with $countryId;$countryCode;$currencyId;$currencyCode; added successfully."
        else
          ((errorAdded++))
          echo "Error adding country-currency relationship with $countryId;$countryCode;$currencyId;$currencyCode;." >> "$log_file"
        fi
      fi
    else
      ((alreadyExist++))
      echo "Country-currency relationship with $countryId;$countryCode;$currencyId;$currencyCode; already exists. Skipping insertion."
    fi
  fi
done < "$csv_file"

# Insert any remaining country-currency relationships
if [ $bulkCounter -gt 0 ]; then
  if psql -h $db_host -p $db_port -d $db_name -U $db_user -c "$bulkInsertSQL"; then
    echo "Bulk insert of $bulkCounter remaining country-currency relationships added successfully."
  else
    ((errorAdded+=bulkCounter))
    echo "Error adding remaining bulk country-currency relationships." >> "$log_file"
  fi
fi

# Unset the password variable for security
unset PGPASSWORD

# Call the add_logs.sh script
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)
chmod +x "$SCRIPT_DIR/add_logs.sh"
"$SCRIPT_DIR/add_logs.sh" "COUNTRY_CURRENCY" "$totalRecords" "$alreadyExist" "$errorAdded"
echo "Country-currency relationships initialized."