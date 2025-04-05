using CSharp2TS.Core.Attributes;

namespace CSharp2TS.Tests.Stubs.Models {
    [TSInterface(Folder = "SubFolder1/SubFolder2")]
    public class TestClassInFolder {
        public int ThisIsANumber { get; set; }
        public TestClass? TestClass { get; set; }
    }
}
