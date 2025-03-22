using CSharp2TS.CLI.Generators;

namespace CSharp2TS.Tests {
    public class GeneratorBaseTests {
        private class TestGenerator : GeneratorBase {
            public TSPropertyGenerationInfo TestGetTSPropertyType(Type type) => GetTSPropertyType(type);

            public TestGenerator() : base(typeof(object)) { }

            public override string Generate() {
                throw new NotImplementedException();
            }
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
            Assert.That(result.TSType, Is.EqualTo(name));
            Assert.That(result.TSTypeFull, Is.EqualTo(name));
            Assert.That(result.IsObject, Is.EqualTo(isObject));
        }

        [Test]
        [TestCase(typeof(List<string>), "string", "string[]", false)]
        [TestCase(typeof(bool[]), "boolean", "boolean[]", false)]
        [TestCase(typeof(IEnumerable<int>), "number", "number[]", false)]
        [TestCase(typeof(ICollection<Guid>), "string", "string[]", false)]
        [TestCase(typeof(ICollection<Object>), "Object", "Object[]", true)]
        public void GetTSPropertyType_ShouldReturnExpectedTypeForCollections(Type inputType, string tsType, string tsTypeFull, bool isObject) {
            var result = generator.TestGetTSPropertyType(inputType);
            Assert.That(result.TSType, Is.EqualTo(tsType));
            Assert.That(result.TSTypeFull, Is.EqualTo(tsTypeFull));
            Assert.That(result.IsObject, Is.EqualTo(isObject));
        }

        [Test]
        [TestCase(typeof(bool?), "boolean", "boolean | null", false)]
        [TestCase(typeof(int?), "number", "number | null", false)]
        [TestCase(typeof(Guid?), "string", "string | null", false)]
        public void GetTSPropertyType_ShouldReturnExpectedTypeForNullables(Type inputType, string tsType, string tsTypeFull, bool isObject) {
            var result = generator.TestGetTSPropertyType(inputType);
            Assert.That(result.TSType, Is.EqualTo(tsType));
            Assert.That(result.TSTypeFull, Is.EqualTo(tsTypeFull));
            Assert.That(result.IsObject, Is.EqualTo(isObject));
        }
    }
}
