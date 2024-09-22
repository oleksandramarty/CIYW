#!/bin/bash

clear

# Microservice name
microserviceName="AuthGateway"
microserviceCLientApiPath="AuthGateway.AuthGateway.ClientApi"
microserviceClientApiName="AuthClient.ts"

BASE_DIR=$(cd "$(dirname "$0")" && pwd | sed 's|/CommonModule/CommonModule.Provision/InitScripts||')

echo $BASE_DIR

outputDir="$BASE_DIR/$microserviceCLientApiPath"
outputFileName="nswagconfig.nswag"

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
      "className": "ApiClient",
      "moduleName": "",
      "namespace": "",
      "typeScriptVersion": 2.7,
      "template": "Angular",
      "promiseType": "Promise",
      "httpClass": "HttpClient",
      "withCredentials": false,
      "useSingletonProvider": false,
      "injectionTokenType": "InjectionToken",
      "rxJsVersion": 6.0,
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
      "baseUrlTokenName": "API_BASE_URL",
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
      "output": "$BASE_DIR/AngularWebClient/src/core/api-clients/$microserviceClientApiName",
      "newLineBehavior": "Auto"
    }
  }
}
EOL

echo "Client API configuration for $microserviceName generated successfully."