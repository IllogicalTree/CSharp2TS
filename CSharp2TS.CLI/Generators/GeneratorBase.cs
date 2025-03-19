namespace CSharp2TS.CLI.Generators {
    public abstract class GeneratorBase {
        private readonly Type[] stringTypes = { typeof(char), typeof(string), typeof(Guid) };
        private readonly Type[] numberTypes = {
            typeof(sbyte), typeof(byte), typeof(short),
            typeof(ushort), typeof(int), typeof(uint),
            typeof(long), typeof(ulong), typeof(float),
            typeof(double), typeof(decimal)
        };

        protected (string Name, bool IsObject) GetTSPropertyType(Type type) {
            string result = string.Empty;

            bool isCollection = CheckCollectionType(ref type);
            bool isObject = false;

            if (stringTypes.Contains(type)) {
                result = "string";
            } else if (numberTypes.Contains(type)) {
                result = "number";
            } else if (type == typeof(bool)) {
                result = "boolean";
            } else {
                result = type.Name;
                isObject = true;
            }

            if (isCollection) {
                result += "[]";
            }

            return (result, isObject);
        }

        private bool CheckCollectionType(ref Type type) {
            if (type.IsArray) {
                type = type.GetElementType() ?? type;
                return true;
            }

            if (!type.IsGenericType) {
                return false;
            }

            if (!typeof(IEnumerable<object>).IsAssignableFrom(type) && type.GetGenericArguments().Length > 1) {
                throw new Exception($"The generic type {type.FullName} must implement IEnumerable<T> and must have no more than 1 generic argument.");
            }

            type = type.GetGenericArguments()[0];

            return true;
        }

        protected string ToCamelCase(string value) {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
