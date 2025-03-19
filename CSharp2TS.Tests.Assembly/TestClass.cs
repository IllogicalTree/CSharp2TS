using CSharp2TS.Attributes;

namespace CSharp2TS.Tests {
    [TSInterface]
    public class TestClass {
        public int ThisIsANumber { get; set; }
        public string? ThisIsMaybeAString { get; set; }
        public Guid ThisIsAGuid { get; set; }
        public object[] ThisIsAnObjectArray { get; set; } = [];
        public TestClass? NestedObject { get; set; }
    }
}
