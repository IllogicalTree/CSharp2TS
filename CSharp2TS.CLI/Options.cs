namespace CSharp2TS.CLI {
    public class Options {
        public string OutputFolder { get; set; } = string.Empty;
        public string AssemblyPath { get; set; } = string.Empty;
        public string FileNameCasingStyle { get; set; } = Consts.PascalCase;
    }
}
