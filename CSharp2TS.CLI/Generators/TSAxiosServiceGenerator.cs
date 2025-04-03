using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public class TSAxiosServiceGenerator : GeneratorBase<TSServiceAttribute> {
        private IList<TSServiceMethod> items;
        private bool generateApiClient;
        private string apiClientPath;
        private string apiClientFileName;

        public TSAxiosServiceGenerator(TypeDefinition type, Options options) : base(type, options) {
            items = new List<TSServiceMethod>();
            generateApiClient = string.IsNullOrWhiteSpace(options.ApiClientPath);
            apiClientFileName = generateApiClient ? "apiClient" : Path.GetFileNameWithoutExtension(options.ApiClientPath)!;
            apiClientPath = generateApiClient ? options.ServicesOutputFolder! : Path.GetDirectoryName(options.ApiClientPath)!;
        }

        public override string Generate() {
            if (generateApiClient) {
                GenerateApiClient();
            }

            ParseTypes();

            return BuildTsFile();
        }

        public static void GenerateApiClient() {
            string apiClientTemplate = new TSAxiosApiClientTemplate().TransformText();

            using (var streamWriter = new StreamWriter("apiClient.ts")) {
                streamWriter.Write(apiClientTemplate);
            }
        }

        private void ParseTypes() {
            var methods = Type.Methods;

            foreach (var method in methods) {
                if (method == null || method.IsSpecialName || method.HasCustomAttribute(typeof(TSExcludeAttribute))) {
                    continue;
                }

                var httpMethodAttribute = GetHttpAttribute(method);

                if (httpMethodAttribute == null) {
                    continue;
                }

                string name = ToCamelCase(method.Name);
                string? httpMethod = httpMethodAttribute.HttpMethod;
                string route = ToCamelCase(Type.Name.Replace("Controller", string.Empty)) + "/" + httpMethodAttribute.Template ?? string.Empty;
                var returnType = GetTSPropertyType(method.ReturnType);

                var allParams = method.Parameters.ToArray();
                var routeParams = GetRouteParams(route, allParams);
                var queryParams = GetQueryParams(route, allParams);
                TSServiceMethodParam? bodyParam = null;

                if (httpMethod != Consts.HttpGet) {
                    bodyParam = GetBodyParam(route, allParams);
                }

                string queryString = GetQueryString(route, queryParams);

                TryAddTSImport(returnType, Options.ServicesOutputFolder, Options.ModelOutputFolder);

                foreach (var param in routeParams) {
                    TryAddTSImport(param.Property, Options.ServicesOutputFolder, Options.ModelOutputFolder);
                }

                foreach (var param in queryParams) {
                    TryAddTSImport(param.Property, Options.ServicesOutputFolder, Options.ModelOutputFolder);
                }

                if (bodyParam != null) {
                    TryAddTSImport(bodyParam.Property, Options.ServicesOutputFolder, Options.ModelOutputFolder);
                }

                items.Add(new TSServiceMethod(
                    name,
                    httpMethod,
                    route,
                    returnType.TSType,
                    routeParams,
                    queryParams,
                    bodyParam,
                    queryString));
            }
        }

        private TSServiceMethodParam[] GetRouteParams(string template, ParameterDefinition[] allParams) {
            if (string.IsNullOrWhiteSpace(template)) {
                return [];
            }

            return allParams
                .Where(i => template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                .ToArray();
        }

        private TSServiceMethodParam[] GetQueryParams(string template, ParameterDefinition[] allParams) {
            if (string.IsNullOrWhiteSpace(template)) {
                return [];
            }

            return allParams
                .Where(i => !template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                .Where(row => !row.Property.IsObject)
                .ToArray();
        }

        private TSServiceMethodParam? GetBodyParam(string template, ParameterDefinition[] allParams) {
            if (string.IsNullOrWhiteSpace(template)) {
                return allParams
                    .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                    .Where(row => row.Property.IsObject)
                    .FirstOrDefault();
            }

            return allParams
                .Where(i => !template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                .FirstOrDefault();
        }

        private HttpAttribute? GetHttpAttribute(MethodDefinition methodDefinition) {
            string template;

            if (methodDefinition.TryGetHttpAttributeTemplate<HttpGetAttribute>(out template)) {
                return new HttpAttribute(Consts.HttpGet, template);
            } else if (methodDefinition.TryGetHttpAttributeTemplate<HttpPostAttribute>(out template)) {
                return new HttpAttribute(Consts.HttpPost, template);
            } else if (methodDefinition.TryGetHttpAttributeTemplate<HttpPutAttribute>(out template)) {
                return new HttpAttribute(Consts.HttpPut, template);
            } else if (methodDefinition.TryGetHttpAttributeTemplate<HttpDeleteAttribute>(out template)) {
                return new HttpAttribute(Consts.HttpDelete, template);
            } else if (methodDefinition.TryGetHttpAttributeTemplate<HttpPatchAttribute>(out template)) {
                return new HttpAttribute(Consts.HttpPatch, template);
            }

            return null;
        }

        private string GetRoute(HttpMethodAttribute httpMethodAttribute) {
            string url = $"/{Type.Name.Replace("Controller", string.Empty)}/";

            if (!string.IsNullOrWhiteSpace(httpMethodAttribute.Template)) {
                url += httpMethodAttribute.Template.Replace("{", "${");
            }

            return url;
        }

        private string GetQueryString(string template, TSServiceMethodParam[] queryParameters) {
            if (string.IsNullOrEmpty(template)) {
                return string.Empty;
            }

            IList<string> querySections = [];

            foreach (var param in queryParameters) {
                if (template.Contains($"{{{param.Name}}}")) {
                    continue;
                }

                querySections.Add($"{param.Name}=${{{param.Name}}}");
            }

            if (querySections.Count == 0) {
                return string.Empty;
            }

            return $"?{string.Join('&', querySections)}";
        }

        private string BuildTsFile() {
            return new TSAxiosServiceTemplate {
                ApiClientPath = GetRelativeImportPath(Options.ServicesOutputFolder!, apiClientPath) + apiClientFileName,
                Items = items,
                Imports = imports.Select(i => i.Value).ToList(),
                TypeName = Type.Name,
            }.TransformText();
        }

        private record HttpAttribute(string HttpMethod, string? Template);
    }
}
