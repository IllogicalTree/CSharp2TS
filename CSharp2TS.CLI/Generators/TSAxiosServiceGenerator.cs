using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public class TSAxiosServiceGenerator : GeneratorBase<TSServiceAttribute> {
        private string apiClientImportPath;
        private IList<TSServiceMethod> items;

        public TSAxiosServiceGenerator(TypeDefinition type, Options options) : base(type, options) {
            apiClientImportPath = "./";
            items = [];
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            var methods = Type.Methods;
            apiClientImportPath = GetApiClientImport();

            foreach (var method in methods) {
                if (method == null || method.IsSpecialName || method.HasCustomAttribute<TSExcludeAttribute>()) {
                    continue;
                }

                var httpMethodAttribute = GetHttpAttribute(method);

                if (httpMethodAttribute == null) {
                    continue;
                }

                string name = method.Name.ToCamelCase();
                string route = GetRoute(httpMethodAttribute);
                var returnType = GetReturnType(method);

                var allParams = method.Parameters.ToArray();
                var routeParams = GetRouteParams(route, allParams);
                var queryParams = GetQueryParams(route, allParams);
                TSServiceMethodParam? bodyParam = null;

                if (httpMethodAttribute.HttpMethod != Consts.HttpGet) {
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
                    httpMethodAttribute.HttpMethod,
                    route,
                    returnType.TSType,
                    routeParams,
                    queryParams,
                    bodyParam,
                    queryString));
            }
        }

        private string GetApiClientImport() {
            string currentFolder = Path.Combine(Options.ServicesOutputFolder!, FolderLocation ?? string.Empty);
            return FolderUtility.GetRelativeImportPath(currentFolder, Options.ServicesOutputFolder!);
        }

        private TSServiceMethodParam[] GetRouteParams(string template, ParameterDefinition[] allParams) {
            if (string.IsNullOrWhiteSpace(template)) {
                return [];
            }

            return allParams
                .Where(i => template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(i.Name!.ToCamelCase(), GetTSPropertyType(i.ParameterType)))
                .ToArray();
        }

        private TSServiceMethodParam[] GetQueryParams(string template, ParameterDefinition[] allParams) {
            if (string.IsNullOrWhiteSpace(template)) {
                return [];
            }

            return allParams
                .Where(i => !template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(i.Name!.ToCamelCase(), GetTSPropertyType(i.ParameterType)))
                .Where(row => !row.Property.IsObject)
                .ToArray();
        }

        private TSServiceMethodParam? GetBodyParam(string template, ParameterDefinition[] allParams) {
            if (string.IsNullOrWhiteSpace(template)) {
                return allParams
                    .Select(i => new TSServiceMethodParam(i.Name!.ToCamelCase(), GetTSPropertyType(i.ParameterType)))
                    .Where(row => row.Property.IsObject)
                    .FirstOrDefault();
            }

            return allParams
                .Where(i => !template.Contains($"{{{i.Name}}}"))
                .Select(i => new TSServiceMethodParam(i.Name!.ToCamelCase(), GetTSPropertyType(i.ParameterType)))
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

        private string GetRoute(HttpAttribute httpMethodAttribute) {
            string controllerRoute;
            string controllerName = Type.Name.Replace("Controller", string.Empty, StringComparison.OrdinalIgnoreCase).ToLowerInvariant();

            if (Type.TryGetAttribute<RouteAttribute>(out CustomAttribute? attribute)) {
                string? controllerTemplate = (string)attribute!.ConstructorArguments[0].Value;
                controllerRoute = controllerTemplate.Replace("[controller]", controllerName, StringComparison.OrdinalIgnoreCase);
            } else {
                controllerRoute = controllerName;
            }

            if (!string.IsNullOrWhiteSpace(httpMethodAttribute.Template)) {
                controllerRoute += "/" + httpMethodAttribute.Template.Replace("{", "${");
            }

            return controllerRoute;
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

        private TSPropertyGenerationInfo GetReturnType(MethodDefinition method) {
            if (method.TryGetAttribute<TSEndpointAttribute>(out CustomAttribute? attribute)) {
                var customReturnType = attribute!.ConstructorArguments[0].Value as TypeReference;

                if (customReturnType != null) {
                    return GetTSPropertyType(customReturnType);
                }
            }

            return GetTSPropertyType(method.ReturnType);
        }

        private string BuildTsFile() {
            return new TSAxiosServiceTemplate {
                Items = items,
                ApiClientImportPath = apiClientImportPath,
                Imports = imports.Select(i => i.Value).ToList(),
                TypeName = Type.Name,
            }.TransformText();
        }

        private record HttpAttribute(string HttpMethod, string? Template);
    }
}
