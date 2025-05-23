﻿using CSharp2TS.CLI.Templates;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Mono.Cecil;

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
            if (Directory.Exists(options.ModelOutputFolder)) {
                Directory.Delete(options.ModelOutputFolder, true);
            }

            Directory.CreateDirectory(options.ModelOutputFolder!);

            foreach (var assemblyPath in options.ModelAssemblyPaths) {
                using (var assembly = LoadAssembly(assemblyPath)) {
                    GenerateInterfaces(assembly.MainModule, options);
                    GenerateEnums(assembly.MainModule, options);
                }
            }
        }

        private void GenerateServices() {
            if (Directory.Exists(options.ServicesOutputFolder)) {
                Directory.Delete(options.ServicesOutputFolder, true);
            }

            Directory.CreateDirectory(options.ServicesOutputFolder!);

            GenerateApiClient();

            foreach (var assemblyPath in options.ServicesAssemblyPaths) {
                using (var assembly = LoadAssembly(assemblyPath)) {
                    GenerateServices(assembly.MainModule, options);
                }
            }
        }

        private AssemblyDefinition LoadAssembly(string assemblyPath) {
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath)!);

            return AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters {
                AssemblyResolver = resolver,
            });
        }

        private void GenerateApiClient() {
            string apiClientTemplate = new TSAxiosApiClientTemplate().TransformText();
            string path = Path.Combine(options.ServicesOutputFolder!, "apiClient.ts");

            File.WriteAllText(path, apiClientTemplate);
        }

        private void GenerateInterfaces(ModuleDefinition module, Options options) {
            var types = GetTypesByAttribute(module, typeof(TSInterfaceAttribute));

            foreach (TypeDefinition type in types) {
                GenerateFile(options.ModelOutputFolder!, new TSInterfaceGenerator(type, options));
            }
        }

        private void GenerateEnums(ModuleDefinition module, Options options) {
            var types = GetTypesByAttribute(module, typeof(TSEnumAttribute));

            foreach (TypeDefinition type in types) {
                GenerateFile(options.ModelOutputFolder!, new TSEnumGenerator(type, options));
            }
        }

        private void GenerateServices(ModuleDefinition module, Options options) {
            var types = GetTypesByAttribute(module, typeof(TSServiceAttribute));

            foreach (TypeDefinition type in types) {
                GenerateFile(options.ServicesOutputFolder!, new TSAxiosServiceGenerator(type, options));
            }
        }

        private IEnumerable<TypeDefinition> GetTypesByAttribute(ModuleDefinition module, Type attributeType) {
            foreach (var type in module.GetTypes()) {
                if (type.HasAttribute(attributeType)) {
                    yield return type;
                }
            }
        }

        private void GenerateFile<TAttribute>(string outputFolder, GeneratorBase<TAttribute> generator) where TAttribute : TSAttributeBase {
            string output = generator.Generate();
            string folder = Path.Combine(outputFolder, generator.GetFolderLocation());

            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            string file = Path.Combine(folder, $"{generator.GetFileName()}.ts");

            if (File.Exists(file)) {
                throw new InvalidOperationException($"File {file} already exists.");
            }

            File.WriteAllText(file, output);
        }
    }
}
