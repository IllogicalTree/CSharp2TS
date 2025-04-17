﻿namespace CSharp2TS.CLI {
    public class Options {
        public bool GenerateModels { get; set; }
        public string? ModelOutputFolder { get; set; }
        public string[] ModelAssemblyPaths { get; set; } = [];

        public bool GenerateServices { get; set; }
        public string? ServicesOutputFolder { get; set; }
        public string[] ServicesAssemblyPaths { get; set; } = [];
        public string ServiceGenerator { get; set; } = Consts.AxiosService;

        public string FileNameCasingStyle { get; set; } = Consts.PascalCase;
        public bool UseNullableStrings { get; set; } = false;
    }
}
