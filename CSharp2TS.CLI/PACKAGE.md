CSharp2TS.CLI is a tool to generate TypeScript files for C# classes, enums and API endpoints. 

## Getting Started

Install the **CSharp2TS.Core** nuget package in your project. It contains the attributes required to mark your models and controllers for generation by this tool.

## Usage

The CSharp2TS tool can be run with a config file or command line arguments.

**Run using config**

Run using config: `csharp2ts -c C:\path_to_config.json`

**Run using command line**

To run from the command line, specify at least the output and assembly arguments for the models or services

Usage: `csharp2ts [option]`

| Option                               | Description                                              |
| ------------------------------------ | -------------------------------------------------------- |
| --model-output-folder, -mo <path>    | The folder where the generated model files will be saved |
| --model-assembly-path, -ma <path>    | The path to the model assembly                           |
| --file-casing, -fc <path>            | The file name casing style (camel \| pascal)             |
| --services-output-folder, -so <path> | The folder where the services will be saved              |
| --services-assembly-path, -sa <path> | The path to the assembly with the controllers            |
| --service-generator, -sg <path>      | The type of service - currently only Axios is supported  |

**Commands**

Usage: `csharp2ts [command]`

| Command           | Description                           |
| ----------------- | ------------------------------------- |
| -h, -help, --help | Show command and command line options |
| create-config     | Create a default config file          |

**Config File (Optional)**

To create an empty config file run `csharp2ts create-config` in the folder you want the file to be created.

This will create a csharp2ts.json file:

```json
{
    "GenerateModels": false,
    "ModelOutputFolder": "path_to_model_output_folder",
    "ModelAssemblyPaths": ["path_to_assembly_with_models_1", "path_to_assembly_with_models_2"],
    
    "GenerateServices": false,
    "ServicesOutputFolder": "path_to_service_output_folder",
    "ServicesAssemblyPaths": ["path_to_assembly_with_models_1", "path_to_assembly_with_models_2"],
    "ServiceGenerator": "axios", // Only axios supported at the current time
    
    "FileNameCasingStyle": "pascal" // 'pascal' or 'camel'
}
```
