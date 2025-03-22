using CSharp2TS.CLI;

namespace CSharp2TS.Tests {
    class OptionValidatorTests {
        [Test]
        public void Validate_Null() {
            // Arrange
            Options? options = null;

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Failed to parse options"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Validate_NullOutputfolder(string? outputFolder) {
            // Arrange
            var options = new Options {
                OutputFolder = outputFolder,
                AssemblyFolder = "SomeFolder"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Output folder is required"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Validate_NullAssemblyFolder(string? assemblyFolder) {
            // Arrange
            var options = new Options {
                OutputFolder = "SomeFolder",
                AssemblyFolder = assemblyFolder
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Assembly folder is required"));
        }

        [Test]
        public void Validate_Valid() {
            // Arrange
            var options = new Options {
                OutputFolder = "OutputFolder",
                AssemblyFolder = "AssemblyFolder"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
