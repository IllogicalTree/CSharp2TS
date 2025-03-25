using CSharp2TS.CLI;

namespace CSharp2TS.Tests {
    public class OptionTests {
        [Test]
        public void OptionParser_NoCommands() {
            Assert.That(OptionParser.ParseFromArgs([]), Is.Null);
        }

        [Test]
        public void OptionParser_Args_OutputFolder() {
            // Arrange
            string outputOption1 = "-o";
            string outputOption2 = "--output-folder";
            string outputFolder = "output folder";

            // Act
            var result1 = OptionParser.ParseFromArgs([outputOption1, outputFolder])!;
            var result2 = OptionParser.ParseFromArgs([outputOption2, outputFolder])!;

            // Assert
            Assert.That(result1.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(result2.OutputFolder, Is.EqualTo(outputFolder));
        }

        [Test]
        public void OptionParser_Args_AssemblyPath() {
            // Arrange
            string assemblyOption1 = "-a";
            string assemblyOption2 = "--assembly-path";
            string assemblyPath = "assembly path";

            // Act
            var result1 = OptionParser.ParseFromArgs([assemblyOption1, assemblyPath])!;
            var result2 = OptionParser.ParseFromArgs([assemblyOption2, assemblyPath])!;

            // Assert
            Assert.That(result1.AssemblyPath, Is.EqualTo(assemblyPath));
            Assert.That(result2.AssemblyPath, Is.EqualTo(assemblyPath));
        }

        [Test]
        public void OptionParser_Args_FileNameCasingStyle_Valid() {
            // Arrange
            string option1 = "-fc";
            string option2 = "--file-casing";
            string casing = Consts.CamelCase;

            // Act
            var result1 = OptionParser.ParseFromArgs([option1, casing])!;
            var result2 = OptionParser.ParseFromArgs([option2, casing])!;

            // Assert
            Assert.That(result1.FileNameCasingStyle, Is.EqualTo(casing));
            Assert.That(result2.FileNameCasingStyle, Is.EqualTo(casing));
        }

        [Test]
        public void OptionParser_Args_FileNameCasingStyle_Default() {
            // Act
            var result1 = OptionParser.ParseFromArgs(["at least one arg"])!;

            // Assert
            Assert.That(result1.FileNameCasingStyle, Is.EqualTo(Consts.PascalCase));
        }

        [Test]
        public void OptionParser_Config_Exists() {
            // Arrange
            string fileName = "config.json";

            // Act
            var options = OptionParser.ParseFromFile(fileName);

            // Assert
            Assert.That(File.Exists(fileName), Is.True);
            Assert.That(options, Is.Not.Null);
            Assert.That(options.OutputFolder, Is.EqualTo("output"));
            Assert.That(options.AssemblyPath, Is.EqualTo("assembly"));
            Assert.That(options.FileNameCasingStyle, Is.EqualTo("camel"));
        }

        [Test]
        public void OptionParser_Config_NotExists() {
            // Arrange
            string fileName = "missing-config.json";

            // Assert
            Assert.Throws<FileNotFoundException>(() => OptionParser.ParseFromFile(fileName));
        }
    }
}
