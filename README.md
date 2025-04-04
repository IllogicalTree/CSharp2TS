# CSharp2TS
[![Build & Deploy](https://github.com/ormesam/CSharp2TS/actions/workflows/deploy-package.yml/badge.svg)](https://github.com/ormesam/CSharp2TS/actions/workflows/deploy-package.yml)
## Install

**Core**

WIP

**CLI Tool**

Add the GitHub nuget as a feed:

1. Create a GitHub API token with read access for packages: https://docs.github.com/en/packages/learn-github-packages/introduction-to-github-packages#authenticating-to-github-packages

2. `dotnet nuget add source --username YOUR_GITHUB_USERNAME --password YOUR_GITHUB_TOKEN --store-password-in-clear-text --name github-ormesam "https://nuget.pkg.github.com/ormesam/index.json"`

3. Install tool: `dotnet tool install -g csharp2ts.cli`

## Getting Started

The CSharp2TS tool can be run with a config file or command line arguments.

### Config File (Optional)

To create an empty config file run `csharp2ts create-config` in the folder you want the config file to be created.

This will create a csharp2ts.json file:

```json
{
    "GenerateModels": false, // Set to true to generate models
    "ModelOutputFolder": null, // Set to the path of the model output folder (string)
    "ModelAssemblyPaths": [], // Set to an array of paths to the assemblies you want to generate models from (string[])
    
    "GenerateServices": false, // Set to true to generate services
    "ServicesOutputFolder": null, // Set to the path of the service output folder (string)
    "ServicesAssemblyPaths": [], // Set to an array of paths to the assemblies you want to generate services from (string[])
    "ServiceGenerator": "axios", // Only axios supported at the current time
    
    "FileNameCasingStyle": "pascal" // 'pascal' or 'camel'
}
```

## Usage

**Run using config**

Run using config: `csharp2ts -c C:\path_to_config.json`

**Run using command line**

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
