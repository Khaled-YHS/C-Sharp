﻿using Algorithms.Numeric;
using NUnit.Framework;

namespace Algorithms.Tests.Numeric
{
    public static class PerfectSquareTests
    {
        [Test]
        [TestCase(-4, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = false)]
        [TestCase(4, ExpectedResult = true)]
        [TestCase(9, ExpectedResult = true)]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(16, ExpectedResult = true)]
        [TestCase(70, ExpectedResult = false)]
        [TestCase(81, ExpectedResult = true)]
        public static bool IsPerfectSquare_ResultIsCorrect(int number)
        {
            // Arrange

            // Act
            var result = PerfectSquareChecker.IsPerfectSquare(number);

            // Assert
            return result;
        }
        [Test]
        [TestCase(-4, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = false)]
        [TestCase(4, ExpectedResult = true)]
        [TestCase(9, ExpectedResult = true)]
        [TestCase(10, ExpectedResult = false)]
        [TestCase(16, ExpectedResult = true)]
        [TestCase(70, ExpectedResult = false)]
        [TestCase(81, ExpectedResult = true)]
        public static bool IsPerfectSquareInt_ResultIsCorrect(int number)
        {
            // Arrange

            // Act
            var (IsPerfectSquare, _) = PerfectSquareChecker.IsPerfectSquareIntOp(number);

            // Assert
            return IsPerfectSquare;
        }
    }
}
