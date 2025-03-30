namespace CSharp2TS.Core.Attributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TSExcludeAttribute : Attribute {
    }
}
