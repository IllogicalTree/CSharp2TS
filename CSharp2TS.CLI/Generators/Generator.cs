using System.Reflection;
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.CLI.Generators {
    public class Generator {
        private readonly Options options;

        public Generator(Options options) {
            this.options = options;
        }

        public void Run() {
            if (options.GenerateModels) {
                GenerateModels();
            }

            if (options.GenerateServices) {
                GenerateServices();
            }
        }

        private void GenerateModels() {
            if (!File.Exists(options.AssemblyPath)) {
                throw new FileNotFoundException($"Model assembly does not exist at {options.AssemblyPath}");
            }

            if (Directory.Exists(options.OutputFolder)) {
                Directory.Delete(options.OutputFolder, true);
            }

            Directory.CreateDirectory(options.OutputFolder);

            Assembly assembly = Assembly.LoadFrom(options.AssemblyPath);

            GenerateInterfaces(assembly, options);
            GenerateEnums(assembly, options);
        }

        private void GenerateServices() {
            if (!File.Exists(options.ServicesAssemblyPath)) {
                throw new FileNotFoundException($"Service assembly does not exist at {options.ServicesAssemblyPath}");
            }

            if (Directory.Exists(options.ServicesOutputFolder)) {
                Directory.Delete(options.ServicesOutputFolder, true);
            }

            Directory.CreateDirectory(options.ServicesOutputFolder);

            Assembly assembly = Assembly.LoadFrom(options.ServicesAssemblyPath);

            GenerateServices(assembly, options);
        }

        private void GenerateInterfaces(Assembly assembly, Options options) {
            var types = GetTypesByAttribute(assembly, typeof(TSInterfaceAttribute));

            foreach (Type type in types) {
                GenerateFile(options.OutputFolder, new TSInterfaceGenerator(type, options));
            }
        }

        private void GenerateEnums(Assembly assembly, Options options) {
            var types = GetTypesByAttribute(assembly, typeof(TSEnumAttribute));

            foreach (Type type in types) {
                GenerateFile(options.OutputFolder, new TSEnumGenerator(type, options));
            }
        }

        private void GenerateServices(Assembly assembly, Options options) {
            var types = GetTypesByAttribute(assembly, typeof(TSServiceAttribute));

            foreach (Type type in types) {
                GenerateFile(options.ServicesOutputFolder, new TSAxiosServiceGenerator(type, options));
            }
        }

        private IEnumerable<Type> GetTypesByAttribute(Assembly assembly, Type attributeType) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttribute(attributeType, false) != null) {
                    yield return type;
                }
            }
        }

        private void GenerateFile(string outputFolder, GeneratorBase generator) {
            string output = generator.Generate();
            string folder = Path.Combine(outputFolder, generator.FolderLocation ?? string.Empty);

            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            string file = Path.Combine(folder, $"{generator.GetTypeFileName(generator.Type.Name)}.ts");

            if (File.Exists(file)) {
                throw new InvalidOperationException($"File {file} already exists.");
            }

            File.WriteAllText(file, output);
        }
    }
}
