using CSharp2TS.CLI;

namespace CSharp2TS.Tests {
    public class OptionTests {
        [Test]
        public void OptionParser_NoCommands() {
            Assert.That(OptionParser.ParseFromArgs([]), Is.Null);
        }

        [Test]
        [TestCase("-o")]
        [TestCase("--output-folder")]
        public void OptionParser_Args_OutputFolder(string option) {
            // Arrange
            string outputFolder = "output folder";

            // Act
            var result = OptionParser.ParseFromArgs([option, outputFolder])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.OutputFolder, Is.EqualTo(outputFolder));
            Assert.That(noValueResult.OutputFolder, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCase("-a")]
        [TestCase("--assembly-path")]
        public void OptionParser_Args_AssemblyPath(string option) {
            // Arrange
            string assemblyFile = "assembly file";

            // Act
            var result = OptionParser.ParseFromArgs([option, assemblyFile])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.AssemblyPath, Is.EqualTo(assemblyFile));
            Assert.That(noValueResult.AssemblyPath, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCase("-so")]
        [TestCase("--services-output-folder")]
        public void OptionParser_Args_ServicesOutputFolder(string option) {
            // Arrange
            string outputFolder = "output folder";

            // Act
            var result = OptionParser.ParseFromArgs([option, outputFolder])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.ServicesOutputFolder, Is.EqualTo(outputFolder));
            Assert.That(noValueResult.ServicesOutputFolder, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCase("-sa")]
        [TestCase("--services-assembly-path")]
        public void OptionParser_Args_ServicesAssemblyPath(string option) {
            // Arrange
            string assemblyFile = "assembly file";

            // Act
            var result = OptionParser.ParseFromArgs([option, assemblyFile])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.ServicesAssemblyPath, Is.EqualTo(assemblyFile));
            Assert.That(noValueResult.ServicesAssemblyPath, Is.EqualTo(string.Empty));
        }

        [Test]
        [TestCase("-sg")]
        [TestCase("--service-generator")]
        public void OptionParser_Args_ServiceGenerator(string option) {
            // Arrange
            string serviceGenerator = "test";

            // Act
            var result = OptionParser.ParseFromArgs([option, serviceGenerator])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.ServiceGenerator, Is.EqualTo(serviceGenerator));
            Assert.That(noValueResult.ServiceGenerator, Is.EqualTo(Consts.AxiosService));
        }

        [Test]
        [TestCase("-fc")]
        [TestCase("--file-casing")]
        public void OptionParser_Args_FileNameCasingStyle(string option) {
            // Arrange
            string casingStyle = "camel";

            // Act
            var result = OptionParser.ParseFromArgs([option, casingStyle])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.FileNameCasingStyle, Is.EqualTo(casingStyle));
            Assert.That(noValueResult.FileNameCasingStyle, Is.EqualTo(Consts.PascalCase));
        }

        [Test]
        [TestCase("-ac")]
        [TestCase("--api-client-path")]
        public void OptionParser_Args_ApiClientPath(string option) {
            // Arrange
            string path = "/test/";

            // Act
            var result = OptionParser.ParseFromArgs([option, path])!;
            var noValueResult = OptionParser.ParseFromArgs([option])!;

            // Assert
            Assert.That(result.ApiClientPath, Is.EqualTo(path));
            Assert.That(noValueResult.ApiClientPath, Is.EqualTo(string.Empty));
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
