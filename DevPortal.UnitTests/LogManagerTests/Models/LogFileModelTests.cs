using AB.Framework.UnitTests;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.LogManagerTests.ModelTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LogFileModelTests : LooseBaseTestFixture
    {
        #region members & setup

        LogFileModel logFile;

        [SetUp]
        public void Initialize()
        {
        }

        #endregion

        [Test]
        public void Path_NoCondition_ReturnStringPath()
        {
            // Arrange
            var path = "url";
            logFile = new LogFileModel
            {
                Path = path
            };

            //Act
            var result = logFile.Path;

            // Assert
            result.Should().Be(path);
        }

        [Test]
        public void Name_NoCondition_ReturnStringName()
        {
            // Arrange
            var path = "url\\a";
            logFile = new LogFileModel
            {
                Path = path
            };

            //Act
            var result = logFile.Name;

            // Assert
            result.Should().Be("a");
        }

        [Test]
        public void DateModified_NoCondition_ReturnString()
        {
            // Arrange
            string dateTime = "";
            logFile = new LogFileModel
            {
                DateModified = dateTime
            };

            //Act
            var result = logFile.DateModified;

            // Assert
            result.Should().Be(dateTime);
        }

        [Test]
        public void Size_NoCondition_ReturnStringSize()
        {
            // Arrange
            string size = "123 KB";
            logFile = new LogFileModel
            {
                Size = size
            };

            //Act
            var result = logFile.Size;

            // Assert
            result.Should().Be(size);
        }

        [Test]
        public void ColoringLogHeaderText_NoCondition_ReturnStringSize()
        {
            // Arrange
            logFile = new LogFileModel
            {
                Text = "Text INFO"
            };

            //Act
            var result = logFile.ColoringLogHeaderText;

            // Assert
            result.Should().Be("Text <span class='text-info font-weight-bold'>INFO</span>");
        }

        [Test]
        public void ColoringLogHeaderText_TextNull_ReturnNull()
        {
            // Arrange
            logFile = new LogFileModel
            {
                Text = ""
            };

            //Act
            var result = logFile.ColoringLogHeaderText;

            // Assert
            result.Should().BeNull();
        }
    }
}