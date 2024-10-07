# !!!DO NOT FORGET TO UPDATE MIGRATION NAME!!!

## Replace ' to ''

## GUIDS FOR LOCALIZATIONS
`550e8400-e29b-41d4-a716-100000000000`

Follow the rule `a{LOCALE_ID}` where `LOCALE_ID` is a locale identifier. 
For example, `a3` for French means all IDs should be `550e8400-e29b-41d4-a716-{ID}00000000000` and follow the increment rule:

- `550e8400-e29b-41d4-a716-{ID}00000000001`
- `550e8400-e29b-41d4-a716-{ID}00000000002`
- `550e8400-e29b-41d4-a716-{ID}00000000003`
- ...
- `550e8400-e29b-41d4-a716-{ID}..........n`

## For new locale update `init_cache_localizations.sh`
```shell
# Function to map localeId to locale code
get_locale_code() {
  case $1 in
    1) echo "en" ;;
    2) echo "es" ;;
    3) echo "fr" ;;
    4) echo "ua" ;;
    5) echo "ru" ;;
    6) echo "de" ;;
    7) echo "it" ;;
    *) echo "unknown" ;;
  esac
}
```

## For new locale update `check_localizations.sh`
```shell
csv_files=(
"a1_localizations_en.csv" 
"a2_localizations_es.csv"
"a2_localizations_fr.csv"
"a2_localizations_ua.csv"
"a2_localizations_ru.csv"
"a2_localizations_de.csv"
)
  ```

## Provision flags
```shell
# Set the dropMigrations parameter
dropMigrations=false
# Set the addNewMigration parameter
addNewMigration=false
# Set the isBulkUpdate parameter
isBulkUpdate=false
```

## New Microservice
```shell
#Check DataBaseScripts Arrays for microservices
```
### Get CountryCurrency csv file from DatabaseScripts
```sql
SELECT
    CO."Id" || ';' || CO."Code" || ';' || CU."Id" || ';' || CU."Code" || ';' AS Result
FROM "CIYW_DictionaryDb"."Dictionaries"."CountryCurrencies" AS CC
         JOIN "CIYW_DictionaryDb"."Dictionaries"."Countries" CO ON CO."Id" = CC."CountryId"
         JOIN "CIYW_DictionaryDb"."Dictionaries"."Currencies" CU ON CU."Id" = CC."CurrencyId"
ORDER BY CO."Id", CU."Id";
```

### GET Localization default key
```sql
SELECT
    "Id" || ';en;' || "Key" || ';' AS Result
FROM "CIYW_LocalizationDb"."Locales"."Localizations"
WHERE "LocaleId" = 1
ORDER BY "Id";
```