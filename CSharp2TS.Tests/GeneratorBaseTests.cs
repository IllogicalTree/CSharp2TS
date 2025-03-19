using CSharp2TS.CLI.Generators;

namespace CSharp2TS.CLI.Tests.Generators {
    public class GeneratorBaseTests {
        private class TestGenerator : GeneratorBase {
            public (string Name, bool IsObject) TestGetTSPropertyType(Type type) => GetTSPropertyType(type);
        }

        private TestGenerator generator;

        [SetUp]
        public void Setup() {
            generator = new TestGenerator();
        }

        [Test]
        [TestCase(typeof(char), "string", false)]
        [TestCase(typeof(string), "string", false)]
        [TestCase(typeof(Guid), "string", false)]
        [TestCase(typeof(sbyte), "number", false)]
        [TestCase(typeof(byte), "number", false)]
        [TestCase(typeof(short), "number", false)]
        [TestCase(typeof(ushort), "number", false)]
        [TestCase(typeof(int), "number", false)]
        [TestCase(typeof(uint), "number", false)]
        [TestCase(typeof(long), "number", false)]
        [TestCase(typeof(ulong), "number", false)]
        [TestCase(typeof(float), "number", false)]
        [TestCase(typeof(double), "number", false)]
        [TestCase(typeof(decimal), "number", false)]
        [TestCase(typeof(bool), "boolean", false)]
        [TestCase(typeof(object), "Object", true)]
        public void GetTSPropertyType_ShouldReturnExpectedType(Type inputType, string name, bool isObject) {
            var result = generator.TestGetTSPropertyType(inputType);
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.IsObject, Is.EqualTo(isObject));
        }

        [Test]
        [TestCase(typeof(List<string>), "string[]", false)]
        [TestCase(typeof(bool[]), "boolean[]", false)]
        [TestCase(typeof(IEnumerable<int>), "number[]", false)]
        [TestCase(typeof(ICollection<Guid>), "string[]", false)]
        [TestCase(typeof(ICollection<Object>), "Object[]", true)]
        public void GetTSPropertyType_ShouldReturnExpectedTypeForCollections(Type inputType, string name, bool isObject) {
            var result = generator.TestGetTSPropertyType(inputType);
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.IsObject, Is.EqualTo(isObject));
        }

        [Test]
        public void GetTSPropertyType_ShouldThrowExceptionForInvalidGenericType() {
            Assert.Throws<Exception>(() => generator.TestGetTSPropertyType(typeof(Dictionary<int, string>)));
        }
    }
}
