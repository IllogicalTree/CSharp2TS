using System.Text.Json;
using CSharp2TS.CLI.Generators;

namespace CSharp2TS.CLI {
    public class Program {
        private static void Main(string[] args) {
            if (args.Length == 0) {
                ShowIntro();
                return;
            }

            Options? options;

            if (args.Length == 1) {
                string[] helpCommands = ["-h", "-help", "--help"];

                if (helpCommands.Contains(args[0])) {
                    ShowHelp();
                    return;
                }

                if (args[0] == "create-config") {
                    CreateDefaultConfig();
                    return;
                }

                if (args[0] == "create-axios-api-client") {
                    TSAxiosServiceGenerator.GenerateApiClient();
                    Console.WriteLine("Axios API Client created successfully");
                    return;
                }

                options = OptionParser.ParseFromFile(args[0]);
            } else {
                options = OptionParser.ParseFromArgs(args);
            }

            string? errorMessage = OptionParser.Validate(options);

            if (!string.IsNullOrWhiteSpace(errorMessage)) {
                Console.WriteLine(errorMessage);
                return;
            }

            Generator generator = new Generator(options!);
            generator.Run();
        }

        private static void CreateDefaultConfig() {
            Options options = new Options();

            using (var stream = File.Create("csharp2ts.json")) {
                JsonSerializer.Serialize(stream, options);
            }

            Console.WriteLine("Config created successfully");
        }

        private static void ShowIntro() {
            Console.WriteLine("csharp2ts");
            Console.WriteLine("-------------");
            Console.WriteLine("Run csharp2ts [-h | -help] to see commands");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  cshart2ts <path to config>");
            Console.WriteLine("  --- OR ---");
            Console.WriteLine("  cshart2ts <arguments>");
            Console.WriteLine("-------------");
        }

        private static void ShowHelp() {
            Console.WriteLine("csharp2ts");
            Console.WriteLine("-------------");
            Console.WriteLine("Create empty config file:");
            Console.WriteLine("  cshart2ts create-config");
            Console.WriteLine();
            Console.WriteLine("Create basic axios api client:");
            Console.WriteLine("  cshart2ts create-axios-api-client");
            Console.WriteLine();
            Console.WriteLine("Config:");
            Console.WriteLine("  csharp2ts <path to config>");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  Usage: csharp2ts [option] [option args]");
            Console.WriteLine("  --model-output-folder, -mo:      The folder where the generated model files will be saved");
            Console.WriteLine("  --model-assembly-path, -ma:      The path to the model assembly");
            Console.WriteLine("  --service-output-folder, -so:    The folder where the generated service files will be saved");
            Console.WriteLine("  --service-assembly-path, -sa:    The path to the service assembly");
            Console.WriteLine("  --service-generator, -sg:        The type of service generator - currently only axios supported");
            Console.WriteLine("  --api-client-path, -ac:          The path to the api client file. The file must export an \"apiClient\" for use in the services. The file is generated if it doesn't exist.");
            Console.WriteLine($"  --file-casing, -fc:              The file name casing style ({Consts.CamelCase} | {Consts.PascalCase} (default))");
            Console.WriteLine();
            Console.WriteLine("Example Usage");
            Console.WriteLine("  csharp2ts -o ./output -a ./assembly.dll -fc camel");
            Console.WriteLine("-------------");
        }
    }
}
