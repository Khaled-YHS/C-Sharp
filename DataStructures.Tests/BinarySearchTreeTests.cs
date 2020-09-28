using System;
using DataStructures.BinarySearchTree;
using NUnit.Framework;
using System.Collections.Generic;

namespace DataStructures.Tests
{
    [TestFixture]
    class BinarySearchTreeTests
    {
        BinarySearchTree<int> testTree = new BinarySearchTree<int>();
        
        [OneTimeSetUp]
        public void SetUp()
        {
            testTree.Insert(8);
            testTree.Insert(5);
            testTree.Insert(1);
            testTree.Insert(4);
            testTree.Insert(7);
            testTree.Insert(9);
            testTree.Insert(3);
        }

        [Test, Order(1)]
        public void Order_Test()
        {
            List<int> expectedList = new List<int>(new int [] {8, 5, 1, 4, 3, 7, 9});
            Assert.AreEqual(expectedList, testTree.Preorder());
            expectedList = new List<int>(new int [] {1, 3, 4, 5, 7, 8, 9});
            Assert.AreEqual(expectedList, testTree.Inorder());
            expectedList = new List<int>(new int [] {3, 4, 1, 7, 5, 9, 8});
            Assert.AreEqual(expectedList, testTree.Postorder());   
        }

        [Test, Order(2)]
        public void Insert_Inorder_Test()
        {
            testTree.Insert(4);
            List<int> expectedList = new List<int>(new int [] {1, 3, 4, 4, 5, 7, 8, 9});
            Assert.AreEqual(expectedList, testTree.Inorder());
        }

        [Test, Order(3)]
        public void Remove_Postorder_Test()
        {
            testTree.Remove(3);
            List<int> expectedList = new List<int>(new int [] {4, 4, 1, 7, 5, 9, 8});
            Assert.AreEqual(expectedList, testTree.Postorder());
        }

        [TestCase(5, false)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [Order(4)]
        public void Search_Test(int value, bool isNull)
        {
            Node<int>? temp = testTree.Search(value);
            Assert.AreEqual(isNull, temp == null);
        }
    }
}