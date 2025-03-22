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
            string assemblyFolder = "assembly folder";
            string assemblyFilterOption = "-af";
            string assemblyFilter = "assembly filter";

            // Act
            var options = OptionParser.ParseFromArgs([outputOption, outputFolder, assemblyOption, assemblyFolder, assemblyFilterOption, assemblyFilter])!;

            // Assert
            Assert.That(options.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(options.AssemblyFolder, Is.EqualTo(assemblyFolder));
            Assert.That(options.AssemblyFileFilter, Is.EqualTo(assemblyFilter));
        }

        [Test]
        public void OptionParser_Args_Long() {
            // Arrange
            string outputOption = "--output-folder";
            string outputFolder = "output folder";
            string assemblyOption = "--assembly-folder";
            string assemblyFolder = "assembly folder";
            string assemblyFilterOption = "-af";
            string assemblyFilter = "assembly filter";

            // Act
            var options = OptionParser.ParseFromArgs([outputOption, outputFolder, assemblyOption, assemblyFolder, assemblyFilterOption, assemblyFilter])!;

            // Assert
            Assert.That(options.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(options.AssemblyFolder, Is.EqualTo(assemblyFolder));
            Assert.That(options.AssemblyFileFilter, Is.EqualTo(assemblyFilter));
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
            Assert.That(options.AssemblyFolder, Is.EqualTo("assembly"));
            Assert.That(options.AssemblyFileFilter, Is.EqualTo("*.dll"));
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
