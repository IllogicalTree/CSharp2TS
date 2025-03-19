using CSharp2TS.Attributes;
using System.Reflection;

namespace CSharp2TS.CLI.Generators {
    public class Generator {
        public void Run(Options options) {
            if (!Directory.Exists(options.AssemblyFolder)) {
                throw new InvalidOperationException("Assembly folder does not exist.");
            }

            if (!Directory.Exists(options.OutputFolder)) {
                Directory.CreateDirectory(options.OutputFolder);
            }

            foreach (string assemblyPath in Directory.GetFiles(options.AssemblyFolder, options.AssemblyFileFilter ?? "*.dll")) {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                var types = GetTypesByAttribute(assembly);

                foreach (Type type in types) {
                    var generator = new TSInterfaceGenerator(type);
                    string output = generator.Generate();
                    File.WriteAllText(Path.Combine(options.OutputFolder, $"{type.Name}.ts"), output);
                }
            }
        }

        private IEnumerable<Type> GetTypesByAttribute(Assembly assembly) {
            foreach (Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(TSInterfaceAttribute), false).Length > 0) {
                    yield return type;
                }
            }
        }
    }
}
