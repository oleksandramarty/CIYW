#!/bin/bash

# Get the base directory
BASE_DIR=$(cd "$(dirname "$0")" && pwd | sed 's|/CommonModule/CommonModule.Provision/InitScripts||')

# Microservice params
microservices=("Localizations" "Expenses" "AuthGateway" "Dictionaries" "AuditTrail")
microserviceClientApiNames=("localizations-client.ts" "expenses-client.ts" "auth-client.ts" "dictionaries-client.ts" "audit-trail-client.ts")
microserviceClientApiClassNames=("LocalizationClient" "ExpenseClient" "AuthClient" "DictionaryClient" "AuditTrailClient")

# Loop through each microservice and generate the client API configuration
for index in "${!microservices[@]}"; do
  microserviceName=${microservices[$index]}
  microserviceClientApiName=${microserviceClientApiNames[$index]}
  microserviceClientApiClassName=${microserviceClientApiClassNames[$index]}

  # Set the output directory and file name
  outputDir="$BASE_DIR/$microserviceName/$microserviceName.ClientApi"
  outputFileName="nswagconfig.nswag"
  outputClientFile="$BASE_DIR/Angular.WebClient/src/core/api-clients/$microserviceClientApiName"

  # Create the directory if it doesn't exist
  mkdir -p "$outputDir"

  # Write the content to the specified file
  cat <<EOL > "$outputDir/$outputFileName"
{
  "runtime": "Net70",
  "defaultVariables": null,
  "documentGenerator": {
    "aspNetCoreToOpenApi": {
      "project": "$microserviceName.ClientApi.csproj",
      "msBuildProjectExtensionsPath": null,
      "configuration": null,
      "runtime": null,
      "targetFramework": null,
      "noBuild": false,
      "verbose": true,
      "workingDirectory": null,
      "requireParametersWithoutDefault": false,
      "apiGroupNames": null,
      "defaultPropertyNameHandling": "Default",
      "defaultReferenceTypeNullHandling": "Null",
      "defaultDictionaryValueReferenceTypeNullHandling": "NotNull",
      "defaultResponseReferenceTypeNullHandling": "NotNull",
      "defaultEnumHandling": "Integer",
      "flattenInheritanceHierarchy": false,
      "generateKnownTypes": true,
      "generateEnumMappingDescription": false,
      "generateXmlObjects": false,
      "generateAbstractProperties": false,
      "generateAbstractSchemas": true,
      "ignoreObsoleteProperties": false,
      "allowReferencesWithProperties": false,
      "excludedTypeNames": [],
      "serviceHost": null,
      "serviceBasePath": null,
      "serviceSchemes": [],
      "infoTitle": "My Title",
      "infoDescription": null,
      "infoVersion": "1.0.0",
      "documentTemplate": null,
      "documentProcessorTypes": [],
      "operationProcessorTypes": [],
      "typeNameGeneratorType": null,
      "schemaNameGeneratorType": null,
      "contractResolverType": null,
      "serializerSettingsType": null,
      "useDocumentProvider": true,
      "documentName": "swagger",
      "aspNetCoreEnvironment": null,
      "createWebHostBuilderMethod": null,
      "startupType": null,
      "allowNullableBodyParameters": true,
      "output": null,
      "outputType": "Swagger2",
      "newLineBehavior": "Auto",
      "assemblyPaths": [],
      "assemblyConfig": null,
      "referencePaths": [],
      "useNuGetCache": false
    }
  },
  "codeGenerators": {
    "openApiToTypeScriptClient": {
      "className": "$microserviceClientApiClassName",
      "moduleName": "",
      "namespace": "",
      "typeScriptVersion": 5.4,
      "template": "Angular",
      "promiseType": "Promise",
      "httpClass": "HttpClient",
      "withCredentials": false,
      "useSingletonProvider": false,
      "injectionTokenType": "InjectionToken",
      "rxJsVersion": 7.8,
      "dateTimeType": "Date",
      "nullValue": "Undefined",
      "generateClientClasses": true,
      "generateClientInterfaces": false,
      "generateOptionalParameters": true,
      "exportTypes": true,
      "wrapDtoExceptions": false,
      "exceptionClass": "ApiException",
      "clientBaseClass": null,
      "wrapResponses": false,
      "wrapResponseMethods": [],
      "generateResponseClasses": true,
      "responseClass": "SwaggerResponse",
      "protectedMethods": [],
      "configurationClass": null,
      "useTransformOptionsMethod": false,
      "useTransformResultMethod": false,
      "generateDtoTypes": true,
      "operationGenerationMode": "SingleClientFromOperationId",
      "markOptionalProperties": true,
      "generateCloneMethod": false,
      "typeStyle": "Class",
      "enumStyle": "Enum",
      "useLeafType": false,
      "classTypes": [],
      "extendedClasses": [],
      "extensionCode": null,
      "generateDefaultValues": true,
      "excludedTypeNames": [],
      "excludedParameterNames": [],
      "handleReferences": false,
      "generateConstructorInterface": true,
      "convertConstructorInterfaceData": false,
      "importRequiredTypes": true,
      "useGetBaseUrlMethod": false,
      "baseUrlTokenName": "API_BASE_URL_$microserviceName",
      "queryNullValue": "",
      "useAbortSignal": false,
      "inlineNamedDictionaries": false,
      "inlineNamedAny": false,
      "templateDirectory": null,
      "typeNameGeneratorType": "null",
      "propertyNameGeneratorType": null,
      "enumNameGeneratorType": null,
      "serviceHost": null,
      "serviceSchemes": null,
      "output": "$outputClientFile",
      "newLineBehavior": "Auto"
    }
  }
}
EOL

  # Check if the client API file exists and remove it
  if [ -f "$outputClientFile" ]; then
    rm "$outputClientFile"
  fi

  # Run nswag run in the output directory
  cd "$outputDir" && nswag run

  # Check if the output file exists and remove it
  if [ -f "$outputFileName" ]; then
    rm "$outputFileName"
  fi



  echo "Client API configuration for $microserviceName generated successfully."
done