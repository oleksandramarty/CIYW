#!/bin/bash

echo "----------------------"
echo "Dropping databases..."
echo "----------------------"

# Database connection details
db_host="localhost"
db_port="5432"
db_user="postgres"
db_password="postgres"

# Export password to avoid password prompt
export PGPASSWORD=$db_password

# Print the value of ASPNETCORE_ENVIRONMENT
echo "ASPNETCORE_ENVIRONMENT is set to: $ASPNETCORE_ENVIRONMENT"

# Create a variable based on the value of ASPNETCORE_ENVIRONMENT
if [ "$ASPNETCORE_ENVIRONMENT" = "Development" ]; then
  # Array of database names
  dbNames=("CIYW_LocalizationDb" "CIYW_ExpenseDb" "CIYW_UserDb" "CIYW_DictionaryDb" "CIYW_AuditTrailDb")
  
  # Loop through each database name and drop it
  for db_name in "${dbNames[@]}"; do
    # Construct the SQL command to drop the database
    echo "Dropping database: $db_name"
    sql="DROP DATABASE IF EXISTS \"$db_name\";"
    
    # Execute the SQL command
    psql -h $db_host -p $db_port -U $db_user -c "$sql"
  done
else
      echo "Dropping database: CIYW_MonolithDb"
      sql="DROP DATABASE IF EXISTS \"CIYW_MonolithDb\";"
      
      # Execute the SQL command
      psql -h $db_host -p $db_port -U $db_user -c "$sql"
fi

# Unset the password variable for security
unset PGPASSWORD

echo "----------------------"
echo "Databases dropped."
echo "----------------------"