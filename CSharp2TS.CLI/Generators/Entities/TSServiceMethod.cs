namespace CSharp2TS.CLI.Generators.Entities {
    public record TSServiceMethod(
        string MethodName,
        string HttpMethod,
        string Route,
        TSProperty ReturnType,
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

        public bool IsBodyRawFile => BodyParam != null && BodyParam.Property.TSType == TSType.File && BodyParam.Property.TSType != TSType.FormData;
    }
}
