﻿using System;
using Algorithms.Numeric;
using NUnit.Framework;

namespace Algorithms.Tests.Numeric
{
    public static class PerfectNumberTests
    {
        [Test]
        [TestCase(6)]
        [TestCase(28)]
        [TestCase(496)]
        [TestCase(8128)]
        public static void PerfectNumberWork(int number)
        {
            // Arrange

            // Act
            var result = PerfectNumber.IsPerfectNumber(number);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(-2)]
        public static void AliquotSumShouldThrowEx(int number)
        {
            // Arrange

            // Assert
            Assert.Throws<ArgumentException>(() => Aliquot.AliquotSum(number));
        }
    }
}
