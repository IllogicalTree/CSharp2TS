using CSharp2TS.Core.Attributes;

namespace CSharp2TS.Tests.Stubs.Models {
    [TSInterface(Folder = "Models/Sub")]
    public class TestClassWithSubFolder {
        public int ThisIsANumber { get; set; }
        public TestClass? TestClass { get; set; }
    }
}
