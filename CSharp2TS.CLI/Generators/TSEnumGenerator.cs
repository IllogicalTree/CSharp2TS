using System.Text;

namespace CSharp2TS.CLI.Generators {
    public class TSEnumGenerator : GeneratorBase {
        private IList<TSProperty> items;

        public TSEnumGenerator(Type type) : base(type) {
            items = new List<TSProperty>();
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

                items.Add(new TSProperty(item.Name, number));
            }
        }

        private string BuildTsFile() {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"enum {Type.Name} {{");

            foreach (var field in items) {
                builder.AppendLine($"  {field.Name} = {field.Number},");
            }

            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine($"export default {Type.Name};");

            return builder.ToString();
        }

        private record TSProperty(string Name, int Number);
    }
}
