using System.Reflection;
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

            Console.WriteLine("Config successfully created");
        }

        private static void ShowIntro() {
            var versionString = Assembly.GetEntryAssembly()?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion
                .ToString();

            Console.WriteLine($"csharp2ts v{versionString}");
            Console.WriteLine("-------------");
            Console.WriteLine("Run csharp2ts [-h | -help] to see commands");
            Console.WriteLine("-------------");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("  cshart2ts <path to config>");
            Console.WriteLine("  --- OR ---");
            Console.WriteLine("  cshart2ts <arguments>");
            Console.WriteLine("-------------");
        }

        private static void ShowHelp() {
            Console.WriteLine("csharp2ts");
            Console.WriteLine("-------------");
            Console.WriteLine("Config:");
            Console.WriteLine("  csharp2ts <path to config>");
            Console.WriteLine("-------------");
            Console.WriteLine("Arguments:");
            Console.WriteLine("  Usage: csharp2ts [option] [option args]");
            Console.WriteLine("  --output-folder, -o:      The folder where the generated files will be saved");
            Console.WriteLine("  --assembly-path, -a:      The path to the assembly");
            Console.WriteLine("Example");
            Console.WriteLine("  csharp2ts -o ./output -a ./assemblies -af *.dll");
            Console.WriteLine("-------------");
        }
    }
}
