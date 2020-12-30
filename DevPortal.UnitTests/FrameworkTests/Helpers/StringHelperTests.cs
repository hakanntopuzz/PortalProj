using AB.Framework.UnitTests;
using DevPortal.Framework;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.FrameworkTests.Helpers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class StringHelperTests : LooseBaseTestFixture
    {
        [Test]
        public void EncryptMD5GetByteArray_NoCondition_ReturnByteArray()
        {
            // Arrange
            string value = "value";

            // Act
            var result = StringHelper.EncryptMD5GetByteArray(value);

            // Assert
            result.Should().HaveCount(16);
        }

        [Test]
        [TestCase("value", 5)]
        [TestCase("şşş ğğğ", 13)]
        public void StringToBytes_ParametersInTestCases_ReturnBytes(string text, int utf8Length)
        {
            // Act
            var result = StringHelper.StringToBytes(text);

            // Assert
            result.Length.Should().Be(utf8Length);
        }

        [Test]
        public void BytesToHexString_NoCondition_ReturnString()
        {
            // Arrange
            byte[] buffer = new byte[6] { 100, 101, 102, 103, 104, 105 };

            // Act
            var result = StringHelper.BytesToHexString(buffer);

            // Assert
            result.Length.Should().Be(buffer.Length * 2);
        }

        [Test]
        public void HexStringToBytes_NoCondition_ReturnBytes()
        {
            // Arrange
            const string text = "AFFC00DDEEFFCE";

            // Act
            var result = StringHelper.HexStringToBytes(text);

            // Assert
            result.Length.Should().Be(text.Length / 2);
        }
    }
}