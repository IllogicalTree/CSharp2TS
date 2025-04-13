using CSharp2TS.CLI.Generators.Entities;
using CSharp2TS.CLI.Utility;
using CSharp2TS.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Mono.Cecil;

namespace CSharp2TS.CLI.Generators {
    public abstract class GeneratorBase<TAttribute> where TAttribute : TSAttributeBase {
        private static readonly Type[] stringTypes = [typeof(char), typeof(string), typeof(Guid)];
        private static readonly Type[] dateTypes = [typeof(DateTime), typeof(DateTimeOffset)];
        private static readonly Type[] voidTypes = [typeof(void), typeof(Task), typeof(ActionResult), typeof(IActionResult)];
        private static readonly Type[] fileCollectionTypes = [typeof(FormFileCollection), typeof(IFormFileCollection)];
        private static readonly Type[] fileTypes = [typeof(FormFile), typeof(IFormFile), .. fileCollectionTypes];
        private static readonly Type[] numberTypes = [
            typeof(sbyte), typeof(byte), typeof(short),
            typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float),
            typeof(double), typeof(decimal)
        ];

        protected IDictionary<string, TSImport> Imports { get; }

        public TypeDefinition Type { get; }
        public Options Options { get; }
        public string? FolderLocation => Type.GetCustomAttributeValue<string?>(typeof(TSAttributeBase), nameof(TSAttributeBase.Folder));

        public abstract string Generate();

        protected GeneratorBase(TypeDefinition type, Options options) {
            Imports = new Dictionary<string, TSImport>();
            Type = type;
            Options = options;
        }

        protected TSPropertyGenerationInfo GetTSPropertyType(TypeReference type) {
            string tsType;

            TryExtractFromGenericIfRequired(typeof(Task<>), ref type);
            TryExtractFromGenericIfRequired(typeof(ActionResult<>), ref type);

            bool isDictionary = TryExtractFromDictionary(ref type);
            bool isCollection = false;

            if (!isDictionary) {
                isCollection = TryExtractFromCollection(ref type);
            }

            bool isNullable = TryExtractFromGenericIfRequired(typeof(Nullable<>), ref type);
            bool isObject = false;

            if (stringTypes.Any(i => SimpleTypeCheck(type, i))) {
                tsType = "string";
            } else if (numberTypes.Any(i => SimpleTypeCheck(type, i))) {
                tsType = "number";
            } else if (type.FullName == typeof(bool).FullName) {
                tsType = "boolean";
            } else if (dateTypes.Any(i => SimpleTypeCheck(type, i))) {
                tsType = "Date";
            } else if (voidTypes.Any(i => SimpleTypeCheck(type, i))) {
                tsType = "void";
            } else if (fileTypes.Any(i => SimpleTypeCheck(type, i))) {
                tsType = "File";

                if (fileCollectionTypes.Any(i => SimpleTypeCheck(type, i))) {
                    isCollection = true;
                }
            } else {
                tsType = type.Name;
                isObject = true;
            }

            string rawTsType = tsType;

            if (isNullable) {
                tsType += " | null";

                if (isCollection || isDictionary) {
                    tsType = $"({tsType})";
                }
            }

            if (isDictionary) {
                tsType = $"{{ [key: string]: {tsType} }}";
            }

            if (isCollection) {
                tsType += "[]";
            }

            return new TSPropertyGenerationInfo(type, rawTsType, tsType, isObject);
        }

        private bool SimpleTypeCheck(TypeReference typeReference, Type type) {
            return typeReference.FullName == type.FullName;
        }

        private bool TryExtractFromDictionary(ref TypeReference type) {
            if (!type.IsGenericInstance) {
                return false;
            }

            bool isDictionary = type.GetElementType().FullName == typeof(IDictionary<,>).FullName ||
                type.Resolve().Interfaces
                    .Where(i => i.InterfaceType.IsGenericInstance)
                    .Where(i => i.InterfaceType.GetElementType().FullName == typeof(IDictionary<,>).FullName)
                    .Any();

            // If it's a dictionary type, extract the generic arguments
            if (isDictionary && type is GenericInstanceType genericInstance && genericInstance.GenericArguments.Count > 1) {
                type = genericInstance.GenericArguments[1];
                return true;
            }

            return false;
        }

        private bool TryExtractFromCollection(ref TypeReference type) {
            if (type.IsArray) {
                type = ((ArrayType)type).ElementType;
                return true;
            }

            if (!type.IsGenericInstance) {
                return false;
            }

            // Check if type is IEnumerable, or implements IEnumerable<T>
            bool isCollection = type.GetElementType().FullName == typeof(IEnumerable<>).FullName ||
                type.Resolve().Interfaces
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

        public virtual string GetTypeFileName(string typeName) {
            if (Options.FileNameCasingStyle == Consts.CamelCase) {
                return typeName.ToCamelCase();
            }

            // We assume PascalCase for C# types by default
            return typeName;
        }

        protected void TryAddTSImport(TSPropertyGenerationInfo tsType, string? currentFolderRoot, string? targetFolderRoot) {
            if (currentFolderRoot == null || targetFolderRoot == null || Imports.ContainsKey(tsType.Type.FullName) || !tsType.IsObject) {
                return;
            }

            var targetCustomFolder = tsType.Type.Resolve()
                .GetCustomAttributeValue<string?>(typeof(TSAttributeBase), nameof(TSAttributeBase.Folder));

            string currentFolder = Path.Combine(currentFolderRoot, FolderLocation ?? string.Empty);
            string targetFolder = Path.Combine(targetFolderRoot, targetCustomFolder ?? string.Empty);

            string relativePath = FolderUtility.GetRelativeImportPath(currentFolder, targetFolder);
            string importPath = $"{relativePath}{GetTypeFileName(tsType.TSType)}";

            Imports.Add(tsType.Type.FullName, new TSImport(tsType.TSType, importPath));
        }
    }
}
