namespace CSharp2TS.CLI {
    public class Options {
        public string OutputFolder { get; set; } = string.Empty;
        public string AssemblyPath { get; set; } = string.Empty;
        public string ServicesOutputFolder { get; set; } = string.Empty;
        public string ServicesAssemblyPath { get; set; } = string.Empty;
        public string ServiceGenerator { get; set; } = Consts.AxiosService;
        public string FileNameCasingStyle { get; set; } = Consts.PascalCase;
    }
}
