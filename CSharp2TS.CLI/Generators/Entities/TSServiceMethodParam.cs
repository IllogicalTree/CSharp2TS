namespace CSharp2TS.CLI.Generators.Entities {
    public record TSServiceMethodParam(string Name, TSType Property, bool IsBodyParam, bool IsFormData, bool IsFile);
}
