using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.TddKatas
{
    ////İHTİYAÇLAR:
    //
    // Fibonacci sayılarını hesaplama
    // F(0) = 0
    // F(1) = 1
    // F(n) = F(n - 1) + F(n - 2)
    //
    /// 0 -> 0 dönmeli
    /// 1 -> 1 dönmeli
    /// 2 -> 1 dönmeli
    /// 3 -> 2 dönmeli
    /// 4 -> 3 dönmeli
    /// 5 -> 5 dönmeli
    /// 6 -> 8 dönmeli
    /// Negatif sayılar için -1 dönmeli. (opsiyonel)
    [TestFixture]
    public class FibonacciTests
    {
        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        [TestCase(4, 3)]
        [TestCase(5, 5)]
        [TestCase(6, 8)]
        public void GetNumber_IndexSupplied_ReturnFibonacciNumber(int index, int fibonacciNumber)
        {
            // Arrange

            // Act
            var result = Fibonacci.GetNumber(index);

            // Assert
            result.Should().Be(fibonacciNumber, $"{index}. fibonacci number is {fibonacciNumber}");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-2)]
        public void GetNumber_NegativeIndex_ReturnMinus1(int index)
        {
            // Arrange

            // Act
            var result = Fibonacci.GetNumber(index);

            // Assert
            result.Should().Be(-1, "negative numbers should return -1");
        }
    }

    public static class Fibonacci
    {
        public static int GetNumber(int index)
        {
            if (IsNegative(index))
            {
                return -1;
            }

            if (index < 2)
            {
                return index;
            }

            return GetNumber(index - 1) + GetNumber(index - 2);
        }

        static bool IsNegative(int index)
        {
            return index < 0;
        }
    }
}