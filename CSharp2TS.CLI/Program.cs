using CSharp2TS.CLI.Generators;

namespace CSharp2TS.CLI {
    public class Program {
        private static void Main(string[] args) {
            new Generator().Run(OptionParser.ParseOptions(args));
        }
    }
}
