using Mono.Cecil;

namespace CSharp2TS.CLI.Utility {
    public static class Extensions {
        public static string ToCamelCase(this string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return value;
            }

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static bool HasCustomAttribute(this TypeDefinition typeDef, Type type) {
            return typeDef.CustomAttributes
                .Where(a => a.AttributeType.FullName == type.FullName)
                .Any();
        }

        public static bool HasCustomAttribute<T>(this PropertyDefinition typeReference) {
            return typeReference.CustomAttributes
                .Where(a => a.AttributeType.FullName == typeof(T).FullName)
                .Any();
        }

        public static bool HasCustomAttribute<T>(this MethodDefinition typeReference) {
            return typeReference.CustomAttributes
                .Where(a => a.AttributeType.FullName == typeof(T).FullName)
                .Any();
        }

        public static T? GetCustomAttributeValue<T>(this TypeDefinition typeDef, Type type, string propertyName, bool checkBaseType = true) {
            var attribute = typeDef.CustomAttributes
                .Where(a => a.AttributeType.FullName == type.FullName)
                .FirstOrDefault();

            if (attribute == null && checkBaseType) {
                attribute = typeDef.CustomAttributes
                    .Where(a => a.AttributeType.Resolve().BaseType.FullName == type.FullName)
                    .FirstOrDefault();
            }

            if (attribute == null) {
                return default;
            }

            return attribute.Properties
                .Where(f => f.Name == propertyName)
                .Select(f => (T)f.Argument.Value)
                .FirstOrDefault();
        }

        public static bool TryGetAttribute<T>(this MethodDefinition typeDef, out CustomAttribute? attribute) {
            attribute = typeDef.CustomAttributes
                .Where(a => a.AttributeType.FullName == typeof(T).FullName)
                .FirstOrDefault();

            return attribute != null;
        }

        public static bool TryGetAttribute<T>(this TypeDefinition typeDef, out CustomAttribute? attribute) {
            attribute = typeDef.CustomAttributes
                .Where(a => a.AttributeType.FullName == typeof(T).FullName)
                .FirstOrDefault();

            return attribute != null;
        }

        public static bool TryGetHttpAttributeTemplate<T>(this MethodDefinition typeDef, out string template) {
            if (!typeDef.TryGetAttribute<T>(out CustomAttribute? attribute)) {
                template = string.Empty;
                return false;
            }

            if (attribute!.HasConstructorArguments) {
                template = attribute.ConstructorArguments[0].Value.ToString() ?? string.Empty;
            } else {
                template = string.Empty;
            }

            return true;
        }
    }
}
