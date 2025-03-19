using System.Reflection;
using System.Text;

namespace CSharp2TS.CLI.Generators {
    public class TSInterfaceGenerator : GeneratorBase {
        private Type type;
        private IList<string> imports;
        private IList<TSProperty> fields;

        public TSInterfaceGenerator(Type type) {
            this.type = type;
            imports = new List<string>();
            fields = new List<TSProperty>();
        }

        public string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            var properties = type.GetProperties();

            foreach (PropertyInfo property in properties) {
                var tsType = GetTSPropertyType(property.PropertyType);

                if (tsType.IsObject && tsType.TSType != type.Name) {
                    imports.Add(tsType.TSType);
                }

                fields.Add(new TSProperty(property.Name, tsType.TSTypeFull));
            }
        }

        private string BuildTsFile() {
            StringBuilder builder = new StringBuilder();

            foreach (var import in imports) {
                builder.AppendLine($"import {import} from './{import}';");
            }

            if (imports.Count > 0) {
                builder.AppendLine();
            }

            builder.AppendLine($"interface {type.Name} {{");

            foreach (var field in fields) {
                builder.AppendLine($"  {ToCamelCase(field.Name)}: {field.Type};");
            }

            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine($"export default {type.Name};");

            return builder.ToString();
        }

        private record TSProperty(string Name, string Type);
    }
}
