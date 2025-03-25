# CSharp2TS

## Install

WIP

## Getting Started

The CSharp2TS tool can be run with a config or command line arguments.

### From Config

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









csharp2ts
-------------
Create empty config file:
  cshart2ts create-config
-------------
Config:
  csharp2ts <path to config>
-------------
Arguments:
  Usage: csharp2ts [option] [option args]
  --output-folder, -o:      The folder where the generated files will be saved
  --assembly-path, -a:      The path to the assembly
  --file-casing, -fc:       The file name casing style (camel | pascal (default))
Example
  csharp2ts -o ./output -a ./assembly.dll
-------------
