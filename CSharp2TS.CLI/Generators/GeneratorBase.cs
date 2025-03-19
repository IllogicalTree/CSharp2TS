using System.Collections;

namespace CSharp2TS.CLI.Generators {
    public abstract class GeneratorBase {
        private readonly Type[] stringTypes = { typeof(char), typeof(string), typeof(Guid) };
        private readonly Type[] numberTypes = {
            typeof(sbyte), typeof(byte), typeof(short),
            typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float),
            typeof(double), typeof(decimal)
        };

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

            return new TSPropertyGenerationInfo(rawTsType, tsType, isObject);
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

        protected string ToCamelCase(string value) {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public record TSPropertyGenerationInfo(string TSType, string TSTypeFull, bool IsObject);
    }
}
