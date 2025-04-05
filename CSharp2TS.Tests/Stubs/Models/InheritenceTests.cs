using CSharp2TS.Core.Attributes;

namespace CSharp2TS.Tests.Stubs.Models {
    [TSInterface]
    public class ParentClass {
        public int ParentClassProperty { get; set; }
    }

    [TSInterface]
    public class ChildClass : ParentClass {
        public int ChildClassProperty { get; set; }
    }
}
