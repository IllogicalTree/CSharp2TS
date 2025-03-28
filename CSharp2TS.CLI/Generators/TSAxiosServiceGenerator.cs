using System.Reflection;
using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CSharp2TS.CLI.Generators {
    public class TSAxiosServiceGenerator : GeneratorBase {
        private IList<TSServiceMethod> items;

        public TSAxiosServiceGenerator(Type type, Options options) : base(type, options) {
            items = new List<TSServiceMethod>();
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
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
                var parametersArray = method.GetParameters()
                    .Select(i => new TSServiceMethodParam(ToCamelCase(i.Name!), GetTSPropertyType(i.ParameterType)))
                    .ToArray();

                string parameters = string.Join(", ", parametersArray.Select(i => $"{i.Name}: {i.Property.TSTypeFull}"));
                string queryString = GetQueryString(httpMethodAttribute, parametersArray);

                TryAddTSImport(returnType, Options.ServicesOutputFolder, Options.OutputFolder);

                foreach (var param in parametersArray) {
                    TryAddTSImport(param.Property, Options.ServicesOutputFolder, Options.OutputFolder);
                }

                items.Add(new TSServiceMethod(
                    name,
                    httpMethod,
                    route,
                    returnType.TSType,
                    parameters,
                    queryString));
            }
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

        private string GetQueryString(HttpMethodAttribute httpMethodAttribute, TSServiceMethodParam[] parameters) {
            var route = httpMethodAttribute.Template;

            if (string.IsNullOrEmpty(route)) {
                return string.Empty;
            }

            string queryString = string.Empty;

            foreach (var param in parameters) {
                if (route.Contains($"{{{param.Name}}}")) {
                    continue;
                }

                queryString += $"&{param.Name}=${{{param.Name}}}";
            }

            if (string.IsNullOrWhiteSpace(queryString)) {
                queryString = $"?{queryString}";
            }

            return queryString;
        }

        private string BuildTsFile() {
            return new TSAxiosServiceTemplate {
                Items = items,
                Imports = imports.Select(i => i.Value).ToList(),
                Type = Type,
            }.TransformText();
        }
    }
}
