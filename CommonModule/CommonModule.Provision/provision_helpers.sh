#!/bin/bash

# Define variables
db_name="CIYW_MonolithDb"
localization_db="CIYW_LocalizationDb"
dictionary_db="CIYW_DictionaryDb"
users_db="CIYW_Users"

# Function to get the database name based on the environment
getDbName() {
  local db=$1
  if [ "$ASPNETCORE_ENVIRONMENT" = "Development" ]; then
    echo "$db"
  else
    echo "$db_name"
  fi
}

getDbNameLocalization() {
  getDbName "$localization_db"
}

getDbNameDictionary() {
  getDbName "$dictionary_db"
}

getDbNameUser() {
  getDbName "$users_db"
}