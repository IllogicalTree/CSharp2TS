using System.Text.Json;

namespace CSharp2TS.CLI {
    public static class OptionParser {
        public static Options? ParseFromArgs(string[] args) {
            if (args.Length == 0) {
                return null;
            }

            return new Options {
                OutputFolder = TryParseSwitch(args, "--output-folder", "-o") ?? string.Empty,
                AssemblyPath = TryParseSwitch(args, "--assembly-path", "-a") ?? string.Empty,
                FileNameCasingStyle = TryParseSwitch(args, "--file-casing", "-fc") ?? Consts.PascalCase,
            };
        }

        public static Options? ParseFromFile(string optionsPath) {
            if (!File.Exists(optionsPath)) {
                throw new FileNotFoundException($"Config file does not exist at path: {optionsPath}");
            }

            using (var stream = File.OpenRead(optionsPath)) {
                return JsonSerializer.Deserialize<Options>(stream, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                });
            }
        }

        private static string? TryParseSwitch(string[] args, params string[] switches) {
            for (int i = 0; i < switches.Length; i++) {
                int idx = Array.IndexOf(args, switches[i]);

                if (idx != -1 && args.Length > idx + 1) {
                    return args[idx + 1];
                }
            }

            return null;
        }

        public static string? Validate(Options? options) {
            if (options == null) {
                return "Failed to parse options";
            }

            if (string.IsNullOrWhiteSpace(options.OutputFolder)) {
                return "Output folder is required";
            }

            if (string.IsNullOrWhiteSpace(options.AssemblyPath)) {
                return "Assembly path is required";
            }

            if (options.FileNameCasingStyle != Consts.CamelCase && options.FileNameCasingStyle != Consts.PascalCase) {
                return $"Invalid file name casing style ({Consts.CamelCase} | {Consts.PascalCase})";
            }

            return null;
        }
    }
}
