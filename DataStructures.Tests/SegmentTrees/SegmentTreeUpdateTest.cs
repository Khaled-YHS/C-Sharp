using System;
using DataStructures.SegmentTrees;
using NUnit.Framework;

namespace DataStructures.Tests.SegmentTrees
{
    [TestFixture]
    public class SegmentTreeUpdateTests
    {
        private readonly SegmentTreeUpdate testTree = new SegmentTreeUpdate(new [] {8, 9, 1, 4, 8, 7, 2});

        [TestCase(2, 3, 1, 4, 24)]
        [TestCase(0, 3, 1, 4, 24)]
        public void Update_Test(int node, int value, int left, int right, int aftQuery)
        {
            testTree.Update(node, value);
            Assert.AreEqual(aftQuery, testTree.Query(left, right));
        }

    }
}