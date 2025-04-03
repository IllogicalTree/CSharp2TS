using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Templates;
using CSharp2TS.Core.Attributes;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public class TSEnumGenerator : GeneratorBase<TSEnumAttribute> {
        private IList<TSEnumProperty> items;

        public TSEnumGenerator(TypeDefinition type, Options options) : base(type, options) {
            items = [];
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            foreach (var item in Type.Fields) {
                if (item.IsSpecialName) {
                    continue;
                }

                int number = Convert.ToInt32(item.Constant);

                items.Add(new TSEnumProperty(item.Name, number));
            }
        }

        private string BuildTsFile() {
            return new TSEnumTemplate {
                Items = items,
                TypeName = Type.Name,
            }.TransformText();
        }
    }
}
