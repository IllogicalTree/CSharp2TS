using System.Reflection;
using System.Text;
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase {
        private IDictionary<Type, TSImport> imports;
        private IList<TSProperty> fields;

        public TSInterfaceGenerator(Type type) : base(type) {
            imports = new Dictionary<Type, TSImport>();
            fields = new List<TSProperty>();
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            var properties = Type.GetProperties();

            foreach (PropertyInfo property in properties) {
                var tsType = GetTSPropertyType(property.PropertyType);

                if (!imports.ContainsKey(tsType.Type) && tsType.IsObject && tsType.TSType != Type.Name) {
                    CreateTSImport(tsType);
                }

                fields.Add(new TSProperty(property.Name, tsType.TSTypeFull));
            }
        }

        private void CreateTSImport(TSPropertyGenerationInfo tsType) {
            string importPath;

            var tsAttribute = tsType.Type.GetCustomAttribute<TSAttributeBase>(false);

            if (string.IsNullOrWhiteSpace((tsAttribute?.Folder))) {
                importPath = $"./{tsType.TSType}";
            } else {
                importPath = $"./{tsAttribute.Folder}/{tsType.TSType}";
            }

            imports.Add(tsType.Type, new TSImport(tsType.TSType, importPath));
        }

        private string BuildTsFile() {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"// Auto-generated from {Type.Name}.cs");
            builder.AppendLine();

            foreach (var import in imports) {
                builder.AppendLine($"import {import.Value.Name} from '{import.Value.Path}';");
            }

            if (imports.Count > 0) {
                builder.AppendLine();
            }

            builder.AppendLine($"interface {Type.Name} {{");

            foreach (var field in fields) {
                builder.AppendLine($"  {ToCamelCase(field.Name)}: {field.Type};");
            }

            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine($"export default {Type.Name};");

            return builder.ToString();
        }

        private record TSProperty(string Name, string Type);
        private record TSImport(string Name, string Path);
    }
}
