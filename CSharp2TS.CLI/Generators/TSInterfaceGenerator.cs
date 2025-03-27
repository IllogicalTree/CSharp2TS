using System.Reflection;
using CSharp2TS.CLI.Templates;
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase {
        private IDictionary<Type, TSImport> imports;
        private IList<TSProperty> properties;

        public TSInterfaceGenerator(Type type, Options options) : base(type, options) {
            imports = new Dictionary<Type, TSImport>();
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

                if (!imports.ContainsKey(tsType.Type) && tsType.IsObject && tsType.Type != Type) {
                    AddTSImport(tsType);
                }

                this.properties.Add(new TSProperty(ToCamelCase(property.Name), tsType.TSTypeFull));
            }
        }

        private void AddTSImport(TSPropertyGenerationInfo tsType) {
            var tsAttribute = tsType.Type.GetCustomAttribute<TSAttributeBase>(false);
            string currentFolder = Path.Combine(Options.OutputFolder, FolderLocation ?? string.Empty);
            string targetFolder = Path.Combine(Options.OutputFolder, tsAttribute?.Folder ?? string.Empty);

            string relativePath = GetRelativeImportPath(currentFolder, targetFolder);

            string importPath = $"{relativePath}{GetTypeFileName(tsType.TSType)}";
            imports.Add(tsType.Type, new TSImport(tsType.TSType, importPath));
        }

        private string BuildTsFile() {
            return new TSInterfaceTemplate {
                Type = Type,
                Imports = imports.Select(i => i.Value).ToList(),
                Properties = properties,
            }.TransformText();
        }
    }

    public record TSProperty(string Name, string Type);
    public record TSImport(string Name, string Path);
}
