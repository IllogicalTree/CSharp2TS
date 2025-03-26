# CSharp2TS

## Install

WIP

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
