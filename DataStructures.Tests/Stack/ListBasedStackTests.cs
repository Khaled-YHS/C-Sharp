using DataStructures.Stack;
using FluentAssertions;
using NUnit.Framework;

namespace DataStructures.Tests.Stack
{
    public static class ListBasedStackTests
    {
        [Test]
        public static void CountTest()
        {
            var stack = new ListBasedStack<int>(new[] { 0, 1, 2, 3, 4 });
            stack.Count.Should().Be(5);
        }

        [Test]
        public static void ClearTest()
        {
            var stack = new ListBasedStack<int>(new[] { 0, 1, 2, 3, 4 });
            stack.Clear();
            stack.Count.Should().Be(0);
        }

        [Test]
        public static void ContainsTest()
        {
            var stack = new ListBasedStack<int>(new[] { 0, 1, 2, 3, 4 });

            Assert.Multiple(() =>
            {
                stack.Contains(0).Should().BeTrue();
                stack.Contains(1).Should().BeTrue();
                stack.Contains(2).Should().BeTrue();
                stack.Contains(3).Should().BeTrue();
                stack.Contains(4).Should().BeTrue();
            });
        }

        [Test]
        public static void PeekTest()
        {
            var stack = new ListBasedStack<int>(new[] { 0, 1, 2, 3, 4 });

            Assert.Multiple(() =>
            {
                stack.Peek().Should().Be(4);
                stack.Peek().Should().Be(4);
                stack.Peek().Should().Be(4);
            });
        }

        [Test]
        public static void PopTest()
        {
            var stack = new ListBasedStack<int>(new[] { 0, 1, 2, 3, 4 });

            Assert.Multiple(() =>
            {
                stack.Pop().Should().Be(4);
                stack.Pop().Should().Be(3);
                stack.Pop().Should().Be(2);
                stack.Pop().Should().Be(1);
                stack.Pop().Should().Be(0);
            });
        }

        [Test]
        public static void PushTest()
        {
            var stack = new ListBasedStack<int>();

            stack.Push(0);
            stack.Peek().Should().Be(0);

            stack.Push(1);
            stack.Peek().Should().Be(1);

            stack.Push(2);
            stack.Peek().Should().Be(2);

            stack.Push(3);
            stack.Peek().Should().Be(3);

            stack.Push(4);
            stack.Peek().Should().Be(4);
        }
    }
}
