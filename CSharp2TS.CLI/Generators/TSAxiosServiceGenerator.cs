using System.Reflection;
using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CSharp2TS.CLI.Generators {
    public class TSAxiosServiceGenerator : GeneratorBase {
        private IList<TSServiceMethod> items;
        private bool generateApiClient;
        private string apiClientPath;
        private string apiClientFileName;

        public TSAxiosServiceGenerator(Type type, Options options) : base(type, options) {
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
            var methods = Type.GetMethods();

            foreach (var method in methods) {
                var httpMethodAttribute = method.GetCustomAttribute<HttpMethodAttribute>();

                if (httpMethodAttribute == null || method.GetCustomAttribute(typeof(TSExcludeAttribute)) != null) {
                    continue;
                }

                string name = ToCamelCase(method.Name);
                string httpMethod = GetHttpMethod(httpMethodAttribute);
                string route = GetRoute(httpMethodAttribute);
                var returnType = GetTSPropertyType(method.ReturnType);

                var allParams = method.GetParameters();
                var routeParams = GetRouteParams(httpMethodAttribute, allParams);
                var queryParams = GetQueryParams(httpMethodAttribute, allParams);
                var bodyParam = GetBodyParam(httpMethodAttribute, allParams);

                string queryString = GetQueryString(httpMethodAttribute, queryParams);

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

        private TSServiceMethodParam[] GetRouteParams(HttpMethodAttribute httpMethodAttribute, ParameterInfo[] allParams) {
            if (string.IsNullOrWhiteSpace(httpMethodAttribute.Template)) {
                return [];
            }

            return allParams
                .Where(i => httpMethodAttribute.Template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                .ToArray();
        }

        private TSServiceMethodParam[] GetQueryParams(HttpMethodAttribute httpMethodAttribute, ParameterInfo[] allParams) {
            if (string.IsNullOrWhiteSpace(httpMethodAttribute.Template)) {
                return [];
            }

            return allParams
                .Where(i => !httpMethodAttribute.Template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                .Where(row => !row.Property.IsObject)
                .ToArray();
        }

        private TSServiceMethodParam? GetBodyParam(HttpMethodAttribute httpMethodAttribute, ParameterInfo[] allParams) {
            if (httpMethodAttribute is HttpGetAttribute) {
                return null;
            }

            if (string.IsNullOrWhiteSpace(httpMethodAttribute.Template)) {
                return allParams
                    .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                    .Where(row => row.Property.IsObject)
                    .FirstOrDefault();
            }

            return allParams
                .Where(i => !httpMethodAttribute.Template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                .FirstOrDefault();
        }

        private string GetHttpMethod(HttpMethodAttribute httpMethodAttribute) {
            if (httpMethodAttribute is HttpGetAttribute) {
                return Consts.HttpGet;
            } else if (httpMethodAttribute is HttpPostAttribute) {
                return Consts.HttpPost;
            } else if (httpMethodAttribute is HttpPutAttribute) {
                return Consts.HttpPut;
            } else if (httpMethodAttribute is HttpDeleteAttribute) {
                return Consts.HttpDelete;
            } else if (httpMethodAttribute is HttpPatchAttribute) {
                return Consts.HttpPatch;
            }

            throw new InvalidOperationException("Unknown HTTP method");
        }

        private string GetRoute(HttpMethodAttribute httpMethodAttribute) {
            string url = $"/{Type.Name.Replace("Controller", string.Empty)}/";

            if (!string.IsNullOrWhiteSpace(httpMethodAttribute.Template)) {
                url += httpMethodAttribute.Template.Replace("{", "${");
            }

            return url;
        }

        private string GetQueryString(HttpMethodAttribute httpMethodAttribute, TSServiceMethodParam[] queryParameters) {
            var route = httpMethodAttribute.Template;

            if (string.IsNullOrEmpty(route)) {
                return string.Empty;
            }

            IList<string> querySections = [];

            foreach (var param in queryParameters) {
                if (route.Contains($"{{{param.Name}}}")) {
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
                Type = Type,
            }.TransformText();
        }
    }
}
