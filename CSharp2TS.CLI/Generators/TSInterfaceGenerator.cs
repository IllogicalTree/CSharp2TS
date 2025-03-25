using System.Reflection;
using System.Text;
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase {
        private IDictionary<Type, TSImport> imports;
        private IList<TSProperty> fields;

        public TSInterfaceGenerator(Type type, Options options) : base(type, options) {
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
                if (property.GetCustomAttribute<TSExcludeAttribute>() != null) {
                    continue;
                }

                var tsType = GetTSPropertyType(property.PropertyType);

                if (!imports.ContainsKey(tsType.Type) && tsType.IsObject && tsType.Type != Type) {
                    AddTSImport(tsType);
                }

                fields.Add(new TSProperty(property.Name, tsType.TSTypeFull));
            }
        }

        private void AddTSImport(TSPropertyGenerationInfo tsType) {
            var tsAttribute = tsType.Type.GetCustomAttribute<TSAttributeBase>(false);
            string currentFolder = Path.Combine(Options.OutputFolder!, FolderLocation ?? string.Empty);
            string targetFolder = Path.Combine(Options.OutputFolder!, tsAttribute?.Folder ?? string.Empty);

            string relativePath = GetRelativeImportPath(currentFolder, targetFolder);

            string importPath = $"{relativePath}{GetTypeFileName(tsType.TSType)}";
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
