using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase<TSInterfaceAttribute> {
        private IList<TSProperty> properties;

        public TSInterfaceGenerator(TypeDefinition type, Options options) : base(type, options) {
            properties = [];
        }

        public override string Generate() {
            ParseTypes(Type);

            return BuildTsFile();
        }

        private void ParseTypes(TypeDefinition typeDef) {
            foreach (var property in typeDef.Properties) {
                if (property.IsSpecialName || property.HasAttribute<TSExcludeAttribute>() || IsRecordEqualityContract(property)) {
                    continue;
                }

                var tsType = GetTSPropertyType(property.PropertyType);

                if (tsType.Type != Type) {
                    TryAddTSImport(tsType, Options.ModelOutputFolder, Options.ModelOutputFolder);
                }

                properties.Add(new TSProperty(property.Name.ToCamelCase(), tsType.TSTypeFull));
            }

            if (typeDef.BaseType != null) {
                ParseTypes(typeDef.BaseType.Resolve());
            }
        }

        private bool IsRecordEqualityContract(PropertyDefinition property) {
            return property.PropertyType.FullName == typeof(Type).FullName && property.FullName.EndsWith("::EqualityContract()");
        }

        private string BuildTsFile() {
            return new TSInterfaceTemplate {
                TypeName = Type.Name,
                Imports = Imports.Select(i => i.Value).ToList(),
                Properties = properties,
            }.TransformText();
        }
    }
}
