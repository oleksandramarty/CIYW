#!/bin/bash

echo "Reinitializing the database with data..."

# Get the absolute path of the script directory
SCRIPT_DIR=$(cd "$(dirname "$0")" && pwd)"/InitScripts"

# Check if isBulkUpdate parameter is passed
isBulkUpdate=$1

# Make each script executable and run it
chmod +x "$SCRIPT_DIR/init_roles.sh"
"$SCRIPT_DIR/init_roles.sh"

chmod +x "$SCRIPT_DIR/init_users.sh"
"$SCRIPT_DIR/init_users.sh"

chmod +x "$SCRIPT_DIR/init_categories.sh"
"$SCRIPT_DIR/init_categories.sh" $isBulkUpdate

chmod +x "$SCRIPT_DIR/init_frequencies.sh"
"$SCRIPT_DIR/init_frequencies.sh"

chmod +x "$SCRIPT_DIR/init_countries.sh"
"$SCRIPT_DIR/init_countries.sh" $isBulkUpdate

chmod +x "$SCRIPT_DIR/init_currencies.sh"
"$SCRIPT_DIR/init_currencies.sh" $isBulkUpdate

chmod +x "$SCRIPT_DIR/init_country_currency.sh"
"$SCRIPT_DIR/init_country_currency.sh" $isBulkUpdate

chmod +x "$SCRIPT_DIR/init_balance_types.sh"
"$SCRIPT_DIR/init_balance_types.sh" $isBulkUpdate

chmod +x "$SCRIPT_DIR/init_locales.sh"
"$SCRIPT_DIR/init_locales.sh"

chmod +x "$SCRIPT_DIR/init_localizations.sh"
"$SCRIPT_DIR/init_localizations.sh" $isBulkUpdate

chmod +x "$SCRIPT_DIR/check_localizations.sh"
"$SCRIPT_DIR/check_localizations.sh"

chmod +x "$SCRIPT_DIR/init_cache_localizations.sh"
"$SCRIPT_DIR/init_cache_localizations.sh"

dictionary_cache=("category" "country" "currency" "frequencies" "balance_types" "locales")

for dictionary in "${dictionary_cache[@]}"; do
  chmod +x "$SCRIPT_DIR/init_cache_dictionary_$dictionary.sh"
  "$SCRIPT_DIR/init_cache_dictionary_$dictionary.sh"
done

chmod +x "$SCRIPT_DIR/init_cache_version.sh"
"$SCRIPT_DIR/init_cache_version.sh"
