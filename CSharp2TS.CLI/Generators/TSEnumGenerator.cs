using CSharp2TS.CLI.Templates;

namespace CSharp2TS.CLI.Generators {
    public class TSEnumGenerator : GeneratorBase {
        private IList<TSEnumProperty> items;

        public TSEnumGenerator(Type type, Options options) : base(type, options) {
            items = new List<TSEnumProperty>();
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            var enumItems = Type.GetFields();

            foreach (var item in enumItems) {
                if (item.IsSpecialName) {
                    continue;
                }

                int number = Convert.ToInt32(item.GetRawConstantValue());

                items.Add(new TSEnumProperty(item.Name, number));
            }
        }

        private string BuildTsFile() {
            return new TSEnumTemplate {
                Items = items,
                Type = Type,
            }.TransformText();
        }
    }

    public record TSEnumProperty(string Name, int Number);
}
