namespace CSharp2TS.Core.Attributes {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TSEndpointAttribute : Attribute {
        public Type ReturnType { get; private set; }

        public TSEndpointAttribute(Type returnType) {
            ReturnType = returnType;
        }
    }
}
