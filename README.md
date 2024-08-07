# sample-open-api-nswag
NSwag openApi document generation

## Install NSwag CLI
To install locally
```
dotnet new tool-manifest
```
Now the dotnet tool can be installed
```
dotnet tool install NSwag.ConsoleCore
```

## Install NSwag NuGet Dependencies
Install the following packages
```
NSwag.AspNetCore
NSwag.MSBuild
```

## Register and Use NSwag
In order to plug NSwag in, it needs to be registered in the DI container
```
builder.Services.AddOpenApiDocument();
builder.Services.AddEndpointsApiExplorer(); // this may be needed if there are minimal API
```

and used in the pipeline
```
app.UseOpenApi();
app.UseSwaggerUi();
```

## Configure PostBuild OpenApi Document Generation

### Configure Doocument Generator
Navigate to the project
```
cd src/WebApiOne
```

And create a `nswag.json` with
```
nswag new
```

and remove all its content and add just the following
```
{
  "runtime": "Net80",
  "defaultVariables": null,
  "documentGenerator": {    
  }
}
```
where the document generator section is responsible for defining the `swagger.json` document generation
via MSBuild.

Now, within the `documentGenerator` section add the object `aspNetCoreToOpenApi`

```
{
  "runtime": "Net80",
  "defaultVariables": null,
  "documentGenerator": {
    "aspNetCoreToOpenApi": {
      "project": "WebApiOne.csproj",
      "defaultUrlTemplate": "api/{controller}/{id?}",
      "configuration": null,
      "noBuild": true,
      "verbose": true,
      "requireParametersWithoutDefault": false,
      "apiGroupNames": null,
      "defaultPropertyNameHandling": "Default",
      "defaultReferenceTypeNullHandling": "Null",
      "defaultDictionaryValueReferenceTypeNullHandling": "NotNull",
      "defaultResponseReferenceTypeNullHandling": "NotNull",
      "generateOriginalParameterNames": true,
      "defaultEnumHandling": "String",
      "flattenInheritanceHierarchy": false,
      "generateKnownTypes": true,
      "generateEnumMappingDescription": false,
      "generateXmlObjects": false,
      "generateAbstractProperties": false,
      "generateAbstractSchemas": true,
      "ignoreObsoleteProperties": false,
      "allowReferencesWithProperties": false,
      "useXmlDocumentation": true,
      "resolveExternalXmlDocumentation": true,
      "excludedTypeNames": [],
      "serviceSchemes": [],
      "infoTitle": "WebApiOne Project",
      "infoDescription": "OpenAPI Specification for WebApiOne",
      "infoVersion": "1.0.0",
      "documentProcessorTypes": [],
      "operationProcessorTypes": [],
      "useDocumentProvider": true,
      "documentName": "v1",
      "aspNetCoreEnvironment": null,
      "createWebHostBuilderMethod": null,
      "startupType": null,
      "allowNullableBodyParameters": true,
      "useHttpAttributeNameAsOperationId": false,
      "output": ".openapi/openapi.json",
      "outputType": "OpenApi3",
      "newLineBehavior": "Auto",
      "assemblyPaths": [],
      "referencePaths": [],
      "useNuGetCache": false
    }
  }
}
```

### Configure MSBuild
Add within the `PropertyGroup` tags the instruction to run a post build event
```
<RunPostBuildEvent>OnBuildSuccess</RunPostBuildEvent>
```

Add the following MSBuild target section which restores dotnet tool and runs the `nswag` command 
```
<!-- Defines the Target for post build event and condition to be met inorder to be executed. -->
<Target Name="NSwag" AfterTargets="PostBuildEvent" Condition=" '$(Configuration)' == 'Debug' ">
    <!-- A good practice to restore the project nuget packages to make sure the next step doesn't fail. -->
    <Exec Command="dotnet tool restore"></Exec>
    <!-- An exec command to generate swagger.json file as part of the build process.
    EnvironmentVariables = allows you to set the project environment variable
    WorkingDirectory = holds the directory path from which the command has to be executed
    Command = holds the command to be executed when this exec block is executed during post build process
    . -->
    <Exec WorkingDirectory="$(ProjectDir)" EnvironmentVariables="ASPNETCORE_ENVIRONMENT=Development" Command="$(NSwagExe_Net80) run nswag.json" />
</Target>
```

### Run nswag in CLI
Alternatively, assuming the dotnet tool `NSwag.ConsoleCore` is installed

the command to generate the OpenApi document is
```
dotnet nswag run nswag.json
```