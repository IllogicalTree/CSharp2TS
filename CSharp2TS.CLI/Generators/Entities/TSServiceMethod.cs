namespace CSharp2TS.CLI.Generators.Entities {
    public record TSServiceMethod(
        string MethodName,
        string HttpMethod,
        string Route,
        string ReturnType,
        TSServiceMethodParam[] Params,
        string QueryString);
}
