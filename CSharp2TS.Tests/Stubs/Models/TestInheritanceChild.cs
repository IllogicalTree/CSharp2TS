using CSharp2TS.Core.Attributes;

namespace CSharp2TS.Tests.Stubs.Models {
    [TSInterface]
    public class TestInheritanceChild : TestInheritance {
        public int ChildClassProperty { get; set; }
    }
}
