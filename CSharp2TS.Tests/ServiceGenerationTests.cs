using CSharp2TS.CLI;
using CSharp2TS.CLI.Generators;
using System.Reflection;

namespace CSharp2TS.Tests {
    public class ServiceGenerationTests {
        private Options options;
        private Generator generator;

        [SetUp]
        public void Setup() {
            string modelOutputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "TestResults");
            string serviceOutputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "TestResults", "Services");
            string assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CSharp2TS.Tests.dll");

            options = new Options {
                ModelOutputFolder = modelOutputFolder,
                ServiceGenerator = Consts.AxiosService,
                FileNameCasingStyle = Consts.PascalCase,
                ServicesAssemblyPaths = [assemblyPath],
                ServicesOutputFolder = serviceOutputFolder,
                GenerateServices = true,
            };

            Directory.CreateDirectory(serviceOutputFolder);

            generator = new Generator(options);
            generator.Run();
        }

        [Test]
        [TestCase("CustomRouteService.ts", "Expected\\CustomRouteService.ts")]
        [TestCase("NoRouteService.ts", "Expected\\NoRouteService.ts")]
        [TestCase("TemplatedRouteService.ts", "Expected\\TemplatedRouteService.ts")]
        public void Generation_TestRoute(string generatedFile, string expectedFile) {
            TestFilesMatch(generatedFile, expectedFile);
        }

        [Test]
        [TestCase("ActionResult_TestService.ts", "Expected\\TestService.ts")]
        [TestCase("IActionResult_TestService.ts", "Expected\\TestService.ts")]
        [TestCase("AsyncActionResult_TestService.ts", "Expected\\TestService.ts")]
        [TestCase("AsyncIActionResult_TestService.ts", "Expected\\TestService.ts")]
        public void Generation_TestService(string generatedFile, string expectedFile) {
            TestFilesMatch(generatedFile, expectedFile);
        }

        private void TestFilesMatch(string generatedFile, string expectedFile) {
            // Arrange
            generatedFile = Path.Combine(options.ServicesOutputFolder!, generatedFile);

            if (!File.Exists(generatedFile)) {
                Assert.Fail("Generated file does not exist.");
            }

            if (!File.Exists(expectedFile)) {
                Assert.Fail("Expected file does not exist.");
            }

            // Act
            string generated = File.ReadAllText(generatedFile);
            string expected = File.ReadAllText(expectedFile);

            // Skip commented line
            generated = string.Join(Environment.NewLine, generated.Split(Environment.NewLine).Skip(1));
            expected = string.Join(Environment.NewLine, expected.Split(Environment.NewLine).Skip(1));

            // Assert
            Assert.That(generated, Is.EqualTo(expected));
        }
    }
}
