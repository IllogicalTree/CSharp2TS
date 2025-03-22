using CSharp2TS.CLI;

namespace CSharp2TS.Tests {
    public class OptionTests {
        [Test]
        public void OptionParser_NoCommands() {
            Assert.That(OptionParser.ParseFromArgs([]), Is.Null);
        }

        [Test]
        public void OptionParser_ParseRequired_Short() {
            // Arrange
            string outputOption = "-o";
            string outputFolder = "output folder";
            string assemblyOption = "-a";
            string assemblyFolder = "assembly folder";
            string assemblyFilterOption = "-af";
            string assemblyFilter = "assembly filter";

            // Act
            var options = OptionParser.ParseFromArgs([outputOption, outputFolder, assemblyOption, assemblyFolder, assemblyFilterOption, assemblyFilter]);

            // Assert
            Assert.That(options.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(options.AssemblyFolder, Is.EqualTo(assemblyFolder));
            Assert.That(options.AssemblyFileFilter, Is.EqualTo(assemblyFilter));
        }

        [Test]
        public void OptionParser_ParseRequired_Long() {
            // Arrange
            string outputOption = "--output-folder";
            string outputFolder = "output folder";
            string assemblyOption = "--assembly-folder";
            string assemblyFolder = "assembly folder";
            string assemblyFilterOption = "-af";
            string assemblyFilter = "assembly filter";

            // Act
            var options = OptionParser.ParseFromArgs([outputOption, outputFolder, assemblyOption, assemblyFolder, assemblyFilterOption, assemblyFilter]);

            // Assert
            Assert.That(options.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(options.AssemblyFolder, Is.EqualTo(assemblyFolder));
            Assert.That(options.AssemblyFileFilter, Is.EqualTo(assemblyFilter));
        }
    }
}
