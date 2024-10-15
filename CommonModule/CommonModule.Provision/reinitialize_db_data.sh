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

chmod +x "$SCRIPT_DIR/init_category_icons.sh"
"$SCRIPT_DIR/init_category_icons.sh"

chmod +x "$SCRIPT_DIR/init_icons.sh"
"$SCRIPT_DIR/init_icons.sh" $isBulkUpdate

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
