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

        public bool IsBodyRawFile => BodyParam?.Property.TSType == TSType.File && BodyParam.Property.TSType != TSType.FormData;
        public bool IsBodyFormData => BodyParam?.Property.TSType is TSType.File or TSType.FormData || (BodyParam?.IsFormData ?? false);
        public bool IsResponseFile => ReturnType.TSType == TSType.File;
    }
}
