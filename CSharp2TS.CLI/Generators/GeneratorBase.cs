using System.Collections;
using System.Reflection;
using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.Core.Attributes;

namespace CSharp2TS.CLI.Generators {
    public abstract class GeneratorBase {
        private readonly Type[] stringTypes = { typeof(char), typeof(string), typeof(Guid) };
        private readonly Type[] dateTypes = { typeof(DateTime), typeof(DateTimeOffset) };
        private readonly Type[] numberTypes = {
            typeof(sbyte), typeof(byte), typeof(short),
            typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float),
            typeof(double), typeof(decimal)
        };

        protected IDictionary<Type, TSImport> imports { get; }

        public Type Type { get; }
        public Options Options { get; }

        public string? FolderLocation => Type.GetCustomAttribute<TSAttributeBase>()?.Folder;

        public abstract string Generate();

        protected GeneratorBase(Type type, Options options) {
            Type = type;
            Options = options;
            imports = new Dictionary<Type, TSImport>();
        }

        protected TSPropertyGenerationInfo GetTSPropertyType(Type type) {
            string tsType = string.Empty;

            bool isCollection = CheckCollectionType(ref type);
            bool isNullable = IsNullable(ref type);
            bool isObject = false;

            if (stringTypes.Contains(type)) {
                tsType = "string";
            } else if (numberTypes.Contains(type)) {
                tsType = "number";
            } else if (type == typeof(bool)) {
                tsType = "boolean";
            } else if (dateTypes.Contains(type)) {
                tsType = "Date";
            } else if (type == typeof(void)) {
                tsType = "void";
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

        private bool CheckCollectionType(ref Type type) {
            if (type.IsArray) {
                type = type.GetElementType() ?? type;
                return true;
            }

            if (!type.IsGenericType) {
                return false;
            }

            if (!typeof(IEnumerable).IsAssignableFrom(type)) {
                return false;
            }

            type = type.GetGenericArguments()[0];

            return true;
        }

        private bool IsNullable(ref Type type) {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                type = type.GetGenericArguments()[0];
                return true;
            }

            return false;
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

        protected string ToCamelCase(string value) {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public string GetTypeFileName(string typeName) {
            if (Options.FileNameCasingStyle == Consts.CamelCase) {
                return ToCamelCase(typeName);
            }

            // We assume PascalCase for C# types by default
            return typeName;
        }

        protected void TryAddTSImport(TSPropertyGenerationInfo tsType, string? currentFolderRoot, string? targetFolderRoot) {
            if (currentFolderRoot == null || targetFolderRoot == null || imports.ContainsKey(tsType.Type) || !tsType.IsObject) {
                return;
            }

            var tsAttribute = tsType.Type.GetCustomAttribute<TSAttributeBase>(false);
            string currentFolder = Path.Combine(currentFolderRoot, FolderLocation ?? string.Empty);
            string targetFolder = Path.Combine(targetFolderRoot, tsAttribute?.Folder ?? string.Empty);

            string relativePath = GetRelativeImportPath(currentFolder, targetFolder);

            string importPath = $"{relativePath}{GetTypeFileName(tsType.TSType)}";
            imports.Add(tsType.Type, new TSImport(tsType.TSType, importPath));
        }
    }
}
