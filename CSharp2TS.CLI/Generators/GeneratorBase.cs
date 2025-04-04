using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public abstract class GeneratorBase<TAttribute> where TAttribute : TSAttributeBase {
        private readonly Type[] stringTypes = { typeof(char), typeof(string), typeof(Guid) };
        private readonly Type[] dateTypes = { typeof(DateTime), typeof(DateTimeOffset) };
        private readonly Type[] voidTypes = { typeof(void), typeof(Task) };
        private readonly Type[] objectTypes = { typeof(ActionResult), typeof(IActionResult) };
        private readonly Type[] numberTypes = {
            typeof(sbyte), typeof(byte), typeof(short),
            typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float),
            typeof(double), typeof(decimal)
        };

        protected IDictionary<TypeDefinition, TSImport> imports { get; }

        public TypeDefinition Type { get; }
        public Options Options { get; }
        public string? FolderLocation => Type.GetCustomAttributeValue<string?>(typeof(TSAttributeBase), nameof(TSAttributeBase.Folder));

        public abstract string Generate();

        protected GeneratorBase(TypeDefinition type, Options options) {
            imports = new Dictionary<TypeDefinition, TSImport>();
            Type = type;
            Options = options;
        }

        protected TSPropertyGenerationInfo GetTSPropertyType(TypeReference type) {
            string tsType;

            TryExtractFromGenericIfRequired(typeof(ActionResult<>), ref type);
            TryExtractFromGenericIfRequired(typeof(Task<>), ref type);

            bool isCollection = CheckCollectionType(ref type);
            bool isNullable = TryExtractFromGenericIfRequired(typeof(Nullable<>), ref type);
            bool isObject = false;

            if (stringTypes.Any(i => PrimitiveTypeEquals(type, i))) {
                tsType = "string";
            } else if (numberTypes.Any(i => PrimitiveTypeEquals(type, i))) {
                tsType = "number";
            } else if (type.FullName == typeof(bool).FullName) {
                tsType = "boolean";
            } else if (dateTypes.Any(i => PrimitiveTypeEquals(type, i))) {
                tsType = "Date";
            } else if (voidTypes.Any(i => PrimitiveTypeEquals(type, i))) {
                tsType = "void";
            } else if (objectTypes.Any(i => PrimitiveTypeEquals(type, i))) {
                tsType = "object";
            } else {
                tsType = type.Name;
                isObject = true;
            }

            string rawTsType = tsType;

            if (isCollection) {
                tsType += "[]";
            }

            if (isNullable) {
                tsType += " | null";
            }

            return new TSPropertyGenerationInfo(type, rawTsType, tsType, isObject);
        }

        private bool PrimitiveTypeEquals(TypeReference typeReference, Type type) {
            return typeReference.FullName == type.FullName;
        }

        private bool CheckCollectionType(ref TypeReference type) {
            if (type.IsArray) {
                type = type.GetElementType()?.Resolve() ?? type;
                return true;
            }

            if (!type.IsGenericInstance) {
                return false;
            }

            // Check if type implements IEnumerable<T>
            bool isCollection = type.Resolve().Interfaces
                .Where(i => i.InterfaceType.IsGenericInstance)
                .Where(i => i.InterfaceType.GetElementType().FullName == typeof(IEnumerable<>).FullName)
                .Any();

            // If it's a collection type, extract the generic argument
            if (isCollection && type is GenericInstanceType genericInstance && genericInstance.GenericArguments.Count > 0) {
                type = genericInstance.GenericArguments[0];
                return true;
            }

            return false;
        }

        private bool TryExtractFromGenericIfRequired(Type type, ref TypeReference typeRef) {
            if (typeRef.Resolve().FullName != type.FullName) {
                return false;
            }

            typeRef = ((GenericInstanceType)typeRef).GenericArguments[0];

            return true;
        }

        protected string GetRelativeImportPath(string currentFolder, string targetFolder) {
            if (string.Equals(currentFolder, targetFolder, StringComparison.InvariantCultureIgnoreCase)) {
                return "./";
            }

            currentFolder = currentFolder.Replace('\\', '/');
            targetFolder = targetFolder.Replace('\\', '/');

            string relativePath = Path.GetRelativePath(currentFolder, targetFolder).Replace('\\', '/');

            if (!relativePath.StartsWith('.')) {
                relativePath = $"./{relativePath}";
            }

            return $"{relativePath}/";
        }

        public string GetTypeFileName(string typeName) {
            if (Options.FileNameCasingStyle == Consts.CamelCase) {
                return typeName.ToCamelCase();
            }

            // We assume PascalCase for C# types by default
            return typeName;
        }

        protected void TryAddTSImport(TSPropertyGenerationInfo tsType, string? currentFolderRoot, string? targetFolderRoot) {
            if (currentFolderRoot == null || targetFolderRoot == null || imports.ContainsKey(tsType.Type.Resolve()) || !tsType.IsObject) {
                return;
            }

            var targetCustomFolder = tsType.Type.Resolve()
                .GetCustomAttributeValue<string?>(typeof(TSAttributeBase), nameof(TSAttributeBase.Folder));

            string currentFolder = Path.Combine(currentFolderRoot, FolderLocation ?? string.Empty);
            string targetFolder = Path.Combine(targetFolderRoot, targetCustomFolder ?? string.Empty);

            string relativePath = GetRelativeImportPath(currentFolder, targetFolder);
            string importPath = $"{relativePath}{GetTypeFileName(tsType.TSType)}";

            imports.Add(tsType.Type.Resolve(), new TSImport(tsType.TSType, importPath));
        }
    }
}
