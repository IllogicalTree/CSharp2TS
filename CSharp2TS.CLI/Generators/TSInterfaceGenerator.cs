using System.Reflection;
using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase {
        private IList<TSProperty> properties;

        public TSInterfaceGenerator(Type type, Options options) : base(type, options) {
            properties = new List<TSProperty>();
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            var properties = Type.GetProperties();

            foreach (PropertyInfo property in properties) {
                if (property.GetCustomAttribute<TSExcludeAttribute>() != null) {
                    continue;
                }

                var tsType = GetTSPropertyType(property.PropertyType);

                if (tsType.Type != Type) {
                    TryAddTSImport(tsType, Options.OutputFolder, Options.OutputFolder);
                }

                this.properties.Add(new TSProperty(ToCamelCase(property.Name), tsType.TSTypeFull));
            }
        }

        private string BuildTsFile() {
            return new TSInterfaceTemplate {
                Type = Type,
                Imports = imports.Select(i => i.Value).ToList(),
                Properties = properties,
            }.TransformText();
        }
    }
}
