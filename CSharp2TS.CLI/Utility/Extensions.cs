using Mono.Cecil;

namespace CSharp2TS.CLI.Utility {
    public static class Extensions {
        public static bool HasCustomAttribute(this TypeDefinition typeDef, Type type) {
            return typeDef.CustomAttributes
                .Where(a => a.AttributeType.FullName == type.FullName)
                .Any();
        }

        public static bool HasCustomAttribute(this PropertyDefinition typeReference, Type type) {
            return typeReference.CustomAttributes
                .Where(a => a.AttributeType.FullName == type.FullName)
                .Any();
        }

        public static T? GetCustomAttributeValue<T>(this TypeDefinition typeDef, Type type, string propertyName) {
            var attribute = typeDef.CustomAttributes
                .Where(a => a.AttributeType.FullName == type.FullName)
                .FirstOrDefault();

            if (attribute == null) {
                attribute = typeDef.CustomAttributes
                    .Where(a => a.AttributeType.Resolve().BaseType.FullName == type.FullName)
                    .FirstOrDefault();

                if (attribute == null) {
                    return default;
                }
            }

            return attribute.Properties
                .Where(f => f.Name == propertyName)
                .Select(f => (T)f.Argument.Value)
                .FirstOrDefault();
        }
    }
}
