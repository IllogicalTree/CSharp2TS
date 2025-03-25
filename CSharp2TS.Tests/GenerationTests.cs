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
                OutputFolder = outputFolder,
                AssemblyPath = assemblyPath,
            };

            Directory.CreateDirectory(outputFolder);

            generator = new Generator(options);
            generator.Run();
        }

        [Test]
        public void Generation_TestClass() {
            // Arrange
            string file = Path.Combine(options.OutputFolder!, "TestClass.ts");

            if (!File.Exists(file)) {
                Assert.Fail("File does not exist.");
            }

            string contents = File.ReadAllText(Path.Combine(options.OutputFolder!, "TestClass.ts"));

            // Assert
            string expected = @"// Auto-generated from TestClass.cs

import Object from './Object';
import TestClassWithFolder from './Models/TestClassWithFolder';
import TestEnumWithFolder from './Enums/TestEnumWithFolder';

interface TestClass {
  thisIsANumber: number;
  thisIsMaybeAString: string;
  thisIsAGuid: string;
  thisIsMaybeAGuid: string | null;
  thisIsAnObjectArray: Object[];
  thisIsAStringList: string[];
  nestedObject: TestClass;
  objectInAnotherFolder: TestClassWithFolder;
  enumInAnotherFolder: TestEnumWithFolder | null;
}

export default TestClass;
";

            Assert.That(contents, Is.EqualTo(expected));
        }

        [Test]
        public void Generation_TestInheritance() {
            // Arrange
            string file = Path.Combine(options.OutputFolder!, "TestInheritanceChild.ts");

            if (!File.Exists(file)) {
                Assert.Fail("File does not exist.");
            }

            string contents = File.ReadAllText(Path.Combine(options.OutputFolder!, "TestInheritanceChild.ts"));

            // Assert
            string expected = @"// Auto-generated from TestInheritanceChild.cs

interface TestInheritanceChild {
  childClassProperty: number;
  parentClassProperty: number;
}

export default TestInheritanceChild;
";

            Assert.That(contents, Is.EqualTo(expected));
        }

        [Test]
        public void Generation_TestEnum() {
            // Arrange
            string file = Path.Combine(options.OutputFolder!, "TestEnum.ts");

            if (!File.Exists(file)) {
                Assert.Fail("File does not exist.");
            }

            string contents = File.ReadAllText(Path.Combine(options.OutputFolder!, "TestEnum.ts"));

            // Assert
            string expected = @"// Auto-generated from TestEnum.cs

enum TestEnum {
  Value1 = 1,
  Value2 = 2,
  Value3 = 3,
}

export default TestEnum;
";

            Assert.That(contents, Is.EqualTo(expected));
        }

        [Test]
        public void Generation_TestClassFolder() {
            // Arrange
            string file = Path.Combine(options.OutputFolder!, "Models", "TestClassWithFolder.ts");

            string contents = File.ReadAllText(Path.Combine(options.OutputFolder!, "Models", "TestClassWithFolder.ts"));

            // Assert
            string expected = @"// Auto-generated from TestClassWithFolder.cs

import TestClass from '../TestClass';

interface TestClassWithFolder {
  thisIsANumber: number;
  testClass: TestClass;
}

export default TestClassWithFolder;
";

            Assert.That(contents, Is.EqualTo(expected));

            // Assert
            Assert.That(File.Exists(file), Is.True);
        }

        [Test]
        public void Generation_TestClassSubFolder() {
            // Arrange
            string file = Path.Combine(options.OutputFolder!, "Models", "Sub", "TestClassWithSubFolder.ts");

            string contents = File.ReadAllText(Path.Combine(options.OutputFolder!, "Models", "Sub", "TestClassWithSubFolder.ts"));

            // Assert
            string expected = @"// Auto-generated from TestClassWithSubFolder.cs

import TestClass from '../../TestClass';

interface TestClassWithSubFolder {
  thisIsANumber: number;
  testClass: TestClass;
}

export default TestClassWithSubFolder;
";

            Assert.That(contents, Is.EqualTo(expected));

            // Assert
            Assert.That(File.Exists(file), Is.True);
        }

        [Test]
        public void Generation_TestEnumFolder() {
            // Arrange
            string file = Path.Combine(options.OutputFolder!, "Enums", "TestEnumWithFolder.ts");

            // Assert
            Assert.That(File.Exists(file), Is.True);
        }
    }
}
