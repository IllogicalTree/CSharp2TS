namespace CSharp2TS.CLI.Generators.Entities {
    public record TSServiceMethod(
        string MethodName,
        string HttpMethod,
        string Route,
        string ReturnType,
        TSServiceMethodParam[] RouteParams,
        TSServiceMethodParam[] QueryParams,
        TSServiceMethodParam? BodyParam,
        string QueryString) {

        public IList<TSServiceMethodParam> AllParams {
            get {
                List<TSServiceMethodParam> allParams = [.. RouteParams, .. QueryParams];

                if (BodyParam != null) {
                    allParams.Add(BodyParam);
                }

                return allParams;
            }
        }
    }
}
