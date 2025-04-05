using System.Reflection;
using CSharp2TS.CLI;
using CSharp2TS.CLI.Generators;

namespace CSharp2TS.Tests {
    public class GenerationTests {
        private Options options;
        private Generator generator;

        [SetUp]
        public void Setup() {
            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "TestResults");
            string assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CSharp2TS.Tests.dll");

            options = new Options {
                ModelOutputFolder = outputFolder,
                ModelAssemblyPaths = [assemblyPath],
                ServiceGenerator = Consts.AxiosService,
                FileNameCasingStyle = Consts.PascalCase,
                ServicesAssemblyPaths = [assemblyPath],
                ServicesOutputFolder = Path.Combine(outputFolder, "Services"),
                GenerateModels = true,
                GenerateServices = true,
            };

            Directory.CreateDirectory(outputFolder);

            generator = new Generator(options);
            generator.Run();
        }

        [Test]
        [TestCase("TestClass.ts", "Expected\\TestClassResult.ts")]
        public void Generation_TestClass(string generatedFile, string expectedFile) {
            TestFilesMatch(generatedFile, expectedFile);
        }

        [Test]
        [TestCase("TestEnum.ts", "Expected\\TestEnumResult.ts")]
        public void Generation_TestEnum(string generatedFile, string expectedFile) {
            TestFilesMatch(generatedFile, expectedFile);
        }

        [Test]
        [TestCase("ParentClass.ts", "Expected\\ParentClass.ts")]
        [TestCase("ChildClass.ts", "Expected\\ChildClass.ts")]
        public void Generation_TestInheritance(string generatedFile, string expectedFile) {
            TestFilesMatch(generatedFile, expectedFile);
        }

        [Test]
        public void Generation_TestClassSubFolder() {
            // Arrange
            string file = Path.Combine(options.ModelOutputFolder!, "SubFolder1", "SubFolder2", "TestClassInFolder.ts");

            TestFilesMatch(file, "Expected\\TestClassInFolder.ts");
        }

        [Test]
        public void Generation_TestEnumSubFolder() {
            // Arrange
            string file = Path.Combine(options.ModelOutputFolder!, "Enums", "TestEnumInFolder.ts");

            TestFilesMatch(file, "Expected\\TestEnumInFolder.ts");
        }

        private void TestFilesMatch(string generatedFile, string expectedFile) {
            // Arrange
            generatedFile = Path.Combine(options.ModelOutputFolder!, generatedFile);

            if (!File.Exists(generatedFile)) {
                Assert.Fail("Generated file does not exist.");
            }

            if (!File.Exists(expectedFile)) {
                Assert.Fail("Expected file does not exist.");
            }

            // Act
            string generated = File.ReadAllText(generatedFile);
            string expected = File.ReadAllText(expectedFile);

            // Assert
            Assert.That(generated, Is.EqualTo(expected));
        }
    }
}
