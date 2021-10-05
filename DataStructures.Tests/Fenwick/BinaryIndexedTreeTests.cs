using DataStructures.Fenwick;
using NUnit.Framework;
using FluentAssertions;
using System;

namespace DataStructures.Tests.Fenwick
{
    [TestFixture]
    internal class BinaryIndexedTreeTests
    {
        [Test]
        public void Constructor_ArrayIsTooBig_ThrowException()
        {
            var array = new int[1001];

            Action act = () => new BinaryIndexedTree(array);
            act.Should().Throw<Exception>();
        }

        [Test]
        public void GetSum_CreateBITAndRequestSum_ReturnCorrect()
        {
            int[] array = { 2, 1, 1, 3, 2, 3, 4, 5, 6, 7, 8, 9 };
            var tree = new BinaryIndexedTree(array);
            var expectedSum = 12;

            var resultedSum = tree.GetSum(5);

            resultedSum.Should().Be(expectedSum);
        }

        [Test]
        public void UpdateTree_UpdateTreeAndRequestSum_GetSum()
        {
            int[] array = { 2, 1, 1, 3, 2, 3, 4, 5, 6, 7, 8, 9 };
            var tree = new BinaryIndexedTree(array);
            var expectedSum = 18;

            array[3] += 6;
            tree.UpdateTree(3, 6);

            var resultedSum = tree.GetSum(5);
            resultedSum.Should().Be(expectedSum);
        }
    }
}
