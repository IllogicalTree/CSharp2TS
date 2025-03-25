using CSharp2TS.CLI;

namespace CSharp2TS.Tests {
    public class OptionTests {
        [Test]
        public void OptionParser_NoCommands() {
            Assert.That(OptionParser.ParseFromArgs([]), Is.Null);
        }

        [Test]
        public void OptionParser_Args_Short() {
            // Arrange
            string outputOption = "-o";
            string outputFolder = "output folder";
            string assemblyOption = "-a";
            string assemblyPath = "assembly path";

            // Act
            var options = OptionParser.ParseFromArgs([outputOption, outputFolder, assemblyOption, assemblyPath])!;

            // Assert
            Assert.That(options.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(options.AssemblyPath, Is.EqualTo(assemblyPath));
        }

        [Test]
        public void OptionParser_Args_Long() {
            // Arrange
            string outputOption = "--output-folder";
            string outputFolder = "output folder";
            string assemblyOption = "--assembly-path";
            string assemblyPath = "assembly path";

            // Act
            var options = OptionParser.ParseFromArgs([outputOption, outputFolder, assemblyOption, assemblyPath])!;

            // Assert
            Assert.That(options.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(options.AssemblyPath, Is.EqualTo(assemblyPath));
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
