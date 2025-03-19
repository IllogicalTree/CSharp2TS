namespace CSharp2TS.CLI {
    public static class OptionParser {
        public static Options ParseOptions(string[] args) {
            if (args.Length == 0) {
                throw new InvalidOperationException("No arguments provided.");
            }

            return new Options {
                OutputFolder = TryParseSwitch(args, "--output-folder", "-o") ?? throw new InvalidOperationException("No output folder provided."),
                AssemblyFolder = TryParseSwitch(args, "--assembly-folder", "-a") ?? throw new InvalidOperationException("No assembly folder provided."),
                AssemblyFileFilter = TryParseSwitch(args, "--assembly-filter", "-af"),
            };
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
    }
}
