﻿using Algorithms.Searches;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Tests.Search
{
    public class BinarySearcherTests
    {       
        [Test]
        public void FindIndex_ItemPresent_IndexCorrect([Random(0, 1000, 1000)]int n)
        {
            // Arrange
            var searcher = new BinarySearcher<int>();
            var random = Randomizer.CreateRandomizer();
            var arrayToSearch = Enumerable.Range(0, n).Select(x => random.Next(0, 1000)).OrderBy(x => x).ToArray();
            var randomIndex = random.Next(0, n);

            // Act
            var actualIndex = searcher.FindIndex(arrayToSearch, arrayToSearch[randomIndex]);

            // Assert
            Assert.AreEqual(arrayToSearch[actualIndex], arrayToSearch[randomIndex]);
        }

        [Test]
        public void FindIndex_ItemMissing_MinusOneReturned([Random(0, 1000, 1000)]int n, [Random(-100, 1100, 1000)]int missingItem)
        {
            // Arrange
            var searcher = new BinarySearcher<int>();
            var random = Randomizer.CreateRandomizer();
            var arrayToSearch = Enumerable.Range(0, n).Select(x => random.Next(0, 1000))
                .Where(x => x != missingItem).OrderBy(x => x).ToArray();
            var expectedIndex = -1;

            // Act
            var actualIndex = searcher.FindIndex(arrayToSearch, missingItem);

            // Assert
            Assert.AreEqual(expectedIndex, actualIndex);
        }
    }
}
