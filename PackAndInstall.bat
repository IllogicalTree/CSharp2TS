call dotnet pack -c Release
dotnet tool uninstall -g csharp2ts.cli
dotnet tool install -g --add-source ./CSharp2TS.CLI/nupkg csharp2ts.cli
pause