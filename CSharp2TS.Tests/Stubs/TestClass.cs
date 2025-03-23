using CSharp2TS.Core.Attributes;

namespace CSharp2TS.Tests.Stubs {
    [TSInterface]
    public class TestClass {
        public int ThisIsANumber { get; set; }
        public string? ThisIsMaybeAString { get; set; }
        public Guid ThisIsAGuid { get; set; }
        public Guid? ThisIsMaybeAGuid { get; set; }
        public object[] ThisIsAnObjectArray { get; set; } = [];
        public IList<string> ThisIsAStringList { get; set; } = [];
        public TestClass? NestedObject { get; set; }
    }
}
