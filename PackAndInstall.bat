call dotnet pack -c Release
dotnet tool uninstall -g csharp2ts.cli
dotnet tool install -g --add-source ./nupkg csharp2ts.cli
pause