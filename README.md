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

### Config File (Optional)

To create an empty config file run `csharp2ts create-config`

This will create a csharp2ts.config file:

```json
{
  "OutputFolder": "",
  "AssemblyPath": "",
  "FileNameCasingStyle": "pascal"
}
```

## Usage

**Config**

Run using config: `csharp2ts C:\path_to_config.json`

**Command line args**

Usage: `csharp2ts [option] [option args]`

| option                | Option Args                                        |
| --------------------- | -------------------------------------------------- |
| --output-folder \| -o | The folder where the generated files will be saved |
| --assembly-path \| -a | The path to the assembly                           |
| --file-casing \| -fc  | The file name casing style (camel                  |

Example

`csharp2ts -o ./output -a ./assembly.dll`
