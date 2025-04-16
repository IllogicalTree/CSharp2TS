using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase<TSInterfaceAttribute> {
        private IList<TSProperty> properties;
        private IList<string> genericParameters;

        public TSInterfaceGenerator(TypeDefinition type, Options options) : base(type, options) {
            properties = [];
            genericParameters = [];
        }

        public override string Generate() {
            ParseTypes(Type);

            return BuildTsFile();
        }

        private void ParseTypes(TypeDefinition typeDef) {
            if (typeDef == Type && typeDef.HasGenericParameters) {
                ParseGenericParams();
            }

            foreach (var property in typeDef.Properties) {
                if (property.IsSpecialName || property.HasAttribute<TSExcludeAttribute>() || IsRecordEqualityContract(property)) {
                    continue;
                }

                var tsType = GetTSPropertyType(property.PropertyType, Options.ModelOutputFolder!);

                properties.Add(new TSProperty(property.Name.ToCamelCase(), tsType.TSTypeFull));
            }

            if (typeDef.BaseType != null) {
                ParseTypes(typeDef.BaseType.Resolve());
            }
        }

        private void ParseGenericParams() {
            foreach (var genericParam in Type.GenericParameters) {
                genericParameters.Add(genericParam.Name);
            }
        }

        private bool IsRecordEqualityContract(PropertyDefinition property) {
            return property.PropertyType.FullName == typeof(Type).FullName && property.FullName.EndsWith("::EqualityContract()");
        }

        public override string GetFileName() {
            return ApplyCasing(GetCleanedTypeName(Type));
        }

        private string BuildTsFile() {
            return new TSInterfaceTemplate {
                TypeName = GetCleanedTypeName(Type),
                Imports = Imports.Select(i => i.Value).ToList(),
                Properties = properties,
                GenericParameters = genericParameters,
            }.TransformText();
        }
    }
}
