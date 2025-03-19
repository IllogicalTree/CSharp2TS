using CSharp2TS.CLI;
using CSharp2TS.CLI.Generators;
using System.Reflection;

namespace CSharp2TS.Tests {
    public class GenerationTests {
        private Options options;
        private Generator generator;

        [SetUp]
        public void Setup() {
            string outputFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "TestResults");
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            options = new Options {
                OutputFolder = outputFolder,
                AssemblyFolder = assemblyFolder,
                AssemblyFileFilter = "CSharp2TS.Tests.dll"
            };

            Directory.CreateDirectory(outputFolder);

            generator = new Generator();
            generator.Run(options);
        }

        [TearDown]
        public void TearDown() {
            if (Directory.Exists(options.OutputFolder)) {
                Directory.Delete(options.OutputFolder, true);
            }
        }

        [Test]
        public void Generation_TestClass() {
            // Arrange
            string file = Path.Combine(options.OutputFolder, "TestClass.ts");

            if (!File.Exists(file)) {
                Assert.Fail("File does not exist.");
            }

            string contents = File.ReadAllText(Path.Combine(options.OutputFolder, "TestClass.ts"));

            // Assert
            string expected = @"import Object from './Object';

interface TestClass {
  thisIsANumber: number;
  thisIsMaybeAString: string;
  thisIsAGuid: string;
  thisIsMaybeAGuid: string | null;
  thisIsAnObjectArray: Object[];
  thisIsAStringList: string[];
  nestedObject: TestClass;
}

export default TestClass;
";


            Assert.That(contents, Is.EqualTo(expected));
        }
    }
}
