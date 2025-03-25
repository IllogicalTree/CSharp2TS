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
                AssemblyPath = "SomeFolder"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Output folder is required"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Validate_NullAssemblyPath(string? assemblyPath) {
            // Arrange
            var options = new Options {
                OutputFolder = "SomeFolder",
                AssemblyPath = assemblyPath
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Assembly path is required"));
        }

        [Test]
        public void Validate_Valid() {
            // Arrange
            var options = new Options {
                OutputFolder = "OutputFolder",
                AssemblyPath = "AssemblyPath"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
