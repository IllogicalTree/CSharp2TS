using CSharp2TS.CLI.Generators;

namespace CSharp2TS.CLI {
    public class Program {
        private static void Main(string[] args) {
            Generator generator = new Generator();
            Options? options;

            if (args.Length == 1) {
                options = OptionParser.ParseFromFile(args[0]);
            } else {
                options = OptionParser.ParseFromArgs(args);
            }

            string? errorMessage = OptionParser.Validate(options);

            if (!string.IsNullOrWhiteSpace(errorMessage)) {
                Console.WriteLine(errorMessage);
                return;
            }

            generator.Run(options!);
        }
    }
}
