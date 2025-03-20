using CSharp2TS.Attributes;
using System.Reflection;

namespace CSharp2TS.CLI.Generators {
    public class Generator {
        public void Run(Options options) {
            if (!Directory.Exists(options.AssemblyFolder)) {
                throw new InvalidOperationException("Assembly folder does not exist.");
            }

            if (Directory.Exists(options.OutputFolder)) {
                Directory.Delete(options.OutputFolder, true);
            }

            Directory.CreateDirectory(options.OutputFolder);

            foreach (string assemblyPath in Directory.GetFiles(options.AssemblyFolder, options.AssemblyFileFilter ?? "*.dll")) {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                GenerateInterfaces(assembly, options);
                GenerateEnums(assembly, options);
            }
        }

        private void GenerateInterfaces(Assembly assembly, Options options) {
            var types = GetTypesByAttribute(assembly, typeof(TSInterfaceAttribute));

            foreach (Type type in types) {
                GenerateFile(options.OutputFolder, new TSInterfaceGenerator(type));
            }
        }

        private void GenerateEnums(Assembly assembly, Options options) {
            var types = GetTypesByAttribute(assembly, typeof(TSEnumAttribute));

            foreach (Type type in types) {
                GenerateFile(options.OutputFolder, new TSEnumGenerator(type));
            }
        }

        private IEnumerable<Type> GetTypesByAttribute(Assembly assembly, Type attributeType) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(attributeType, false).Length > 0) {
                    yield return type;
                }
            }
        }

        private void GenerateFile(string outputFolder, GeneratorBase generator) {
            string output = generator.Generate();
            string file = Path.Combine(outputFolder, $"{generator.Type.Name}.ts");

            if (File.Exists(file)) {
                throw new InvalidOperationException($"File {file} already exists.");
            }

            File.WriteAllText(file, output);
        }
    }
}
