using CSharp2TS.CLI;

namespace CSharp2TS.Tests {
    class OptionValidatorTests {

        [Test]
        public void Validate_NullOptions_ReturnsErrorMessage() {
            // Arrange & Act
            var result = OptionParser.Validate(null);

            // Assert
            Assert.That(result, Is.EqualTo("Failed to parse options"));
        }

        [Test]
        public void Validate_EmptyOptions_ReturnsErrorMessage() {
            // Arrange
            var options = new Options();

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("No generation tasks specified"));
        }

        [Test]
        public void Validate_ValidModelOptions_ReturnsNull() {
            // Arrange
            var options = new Options {
                OutputFolder = "out",
                AssemblyPath = "assembly.dll",
                FileNameCasingStyle = Consts.PascalCase,
                ServiceGenerator = Consts.AxiosService
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Validate_ValidServiceOptions_ReturnsNull() {
            // Arrange
            var options = new Options {
                ServicesOutputFolder = "services",
                ServicesAssemblyPath = "services.dll",
                FileNameCasingStyle = Consts.PascalCase,
                ServiceGenerator = Consts.AxiosService
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Validate_InvalidServiceGenerator_ReturnsErrorMessage() {
            // Arrange
            var options = new Options {
                ServicesOutputFolder = "services",
                ServicesAssemblyPath = "services.dll",
                ServiceGenerator = "invalid"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Contains.Substring("Invalid service generator"));
        }

        [Test]
        public void Validate_InvalidCasingStyle_ReturnsErrorMessage() {
            // Arrange
            var options = new Options {
                OutputFolder = "out",
                AssemblyPath = "assembly.dll",
                FileNameCasingStyle = "invalid"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Contains.Substring("Invalid file name casing style"));
        }

        [Test]
        public void Validate_MissingModelOutputFolder_ReturnsErrorMessage() {
            // Arrange
            var options = new Options {
                AssemblyPath = "assembly.dll"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert0
            Assert.That(result, Is.EqualTo("Models output folder is required"));
        }

        [Test]
        public void Validate_MissingModelAssemblyPath_ReturnsErrorMessage() {
            // Arrange
            var options = new Options {
                OutputFolder = "out"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Models assembly path is required"));
        }

        [Test]
        public void Validate_MissingServiceOutputFolder_ReturnsErrorMessage() {
            // Arrange
            var options = new Options {
                ServicesAssemblyPath = "services.dll"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Services output folder is required"));
        }

        [Test]
        public void Validate_MissingServiceAssemblyPath_ReturnsErrorMessage() {
            // Arrange
            var options = new Options {
                ServicesOutputFolder = "services"
            };

            // Act
            var result = OptionParser.Validate(options);

            // Assert
            Assert.That(result, Is.EqualTo("Services assembly path is required"));
        }
    }
}
