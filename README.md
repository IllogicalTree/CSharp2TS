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

The CSharp2TS tool can be run with a config or command line arguments.

To create an empty config file run `csharp2ts create-config`

To create a basic axios api client run `csharp2ts create-axios-api-client`

### Config File (Optional)

This will create a csharp2ts.config file:

```json
{
    "GenerateModels": false,
    "ModelOutputFolder": null,
    "ModelAssemblyPaths": [],
    
    "GenerateServices": false,
    "ServicesOutputFolder": null,
    "ServicesAssemblyPaths": [],
    "ServiceGenerator": "axios",
    "ApiClientPath": null,
    
    "FileNameCasingStyle": "pascal"
}
```

## Usage

**Run using config**

Run using config: `csharp2ts -c C:\path_to_config.json`

**Run using command line**

Usage: `csharp2ts [option]`

| Option                               | Description                                                  |
| ------------------------------------ | ------------------------------------------------------------ |
| --model-output-folder, -mo <path>    | The folder where the generated model files will be saved     |
| --model-assembly-path, -ma <path>    | The path to the model assembly                               |
| --file-casing, -fc <path>            | The file name casing style (camel \| pascal)                 |
| --services-output-folder, -so <path> | The folder where the services will be saved                  |
| --services-assembly-path, -sa <path> | The path to the assembly with the controllers                |
| --service-generator, -sg <path>      | The type of service - currently only Axios is supported      |
| --api-client-path, -ac <path>        | The path to the api client file. The file must export an "apiClient" for use in the services |

**Commands**

Usage: `csharp2ts [command]`

| Command                 | Description                           |
| ----------------------- | ------------------------------------- |
| -h, -help, --help       | Show command and command line options |
| create-config           | Create a default config file          |
| create-axios-api-client | Create an Axios API client file       |
