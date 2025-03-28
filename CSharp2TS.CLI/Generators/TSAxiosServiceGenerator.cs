using CSharp2TS.CLI.Templates;

namespace CSharp2TS.CLI.Generators {
    public class TSAxiosServiceGenerator : GeneratorBase {
        private IList<TSServiceMethod> items;

        public TSAxiosServiceGenerator(Type type, Options options) : base(type, options) {
            items = new List<TSServiceMethod>();
        }

        public override string Generate() {
            ParseTypes();

            return BuildTsFile();
        }

        private void ParseTypes() {
            var methods = Type.GetMethods();

            foreach (var method in methods) {
            }
        }

        private string BuildTsFile() {
            return new TSAxiosServiceTemplate {
                Items = items,
                Type = Type,
            }.TransformText();
        }
    }

    public record TSServiceMethod(string MethodName, string ReturnType);
}
