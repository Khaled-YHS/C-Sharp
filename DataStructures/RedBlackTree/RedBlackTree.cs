using System;
using System.Collections.Generic;

namespace DataStructures.RedBlackTree
{
    /// <summary>
    ///     A self-balancing bindary tree.
    /// </summary>
    /// <remarks>
    ///     A red-black tree is a self-balancing binary search tree (BST) that
    ///     stores a color with each node. A node's color can either be red or
    ///     black. Several properties are maintained to ensure the tree remains
    ///     balanced.
    ///     <list type="number">
    ///         <item>
    ///             <term>A red node does not have a red child.</term>
    ///         </item>
    ///         <item>
    ///             <term>All null nodes are considered black.</term>
    ///         </item>
    ///         <item>
    ///             <term>
    ///                 Every path from a node to its descendant leaf nodes
    ///             has the same number of black nodes.
    ///             </term>
    ///         </item>
    ///         <item>
    ///             <term>(Optional) The root is always black.</term>
    ///         </item>
    ///     </list>
    ///     Red-black trees are generally slightly more unbalanced than an
    ///     AVL tree, but insertion and deletion is generally faster.
    ///     See https://en.wikipedia.org/wiki/Red%E2%80%93black_tree for more information.
    /// </remarks>
    /// <typeparam name="TKey">Type of key for the tree.</typeparam>
    public class RedBlackTree<TKey>
    {
        /// <summary>
        ///     Gets the number of nodes in the tree.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Comparer to use when comparing key values.
        /// </summary>
        private readonly Comparer<TKey> comparer;

        /// <summary>
        ///     Reference to the root node.
        /// </summary>
        private RedBlackTreeNode<TKey>? root;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedBlackTree{TKey}"/> class.
        /// </summary>
        public RedBlackTree()
        {
            comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedBlackTree{TKey}"/> class
        ///     using the specified comparer.
        /// </summary>
        /// <param name="customComparer">Comparer to use when comparing keys.</param>
        public RedBlackTree(Comparer<TKey> customComparer)
        {
            comparer = customComparer;
        }

        /// <summary>
        ///     Add a single node to the tree.
        /// </summary>
        /// <param name="key">Key value to add.</param>
        public void Add(TKey key)
        {
            if (root is null)
            {
                // Case 3
                // New node is root
                root = new RedBlackTreeNode<TKey>(key, null);
                root.Color = NodeColor.Black;
                Count++;
                return;
            }

            // Regular binary tree insertion
            var node = Add(root, key);

            // Get which side child was added to
            int childDir;
            if (node.Left is not null)
            {
                var compareResult = comparer.Compare(node.Left.Key, key);
                childDir = compareResult == 0 ? -1 : 1;
            }
            else
            {
                childDir = 1;
            }

            // Return tree to valid state
            do
            {
                if (node.Color == NodeColor.Black)
                {
                    // Case 1
                    break;
                }
                else if (node.Parent is null)
                {
                    // Case 4
                    node.Color = NodeColor.Black;
                    break;
                }

                // Remaining insert cases need uncle
                var grandparent = node.Parent;
                var parentDir = comparer.Compare(node.Key, node.Parent.Key);
                RedBlackTreeNode<TKey>? uncle;
                if (parentDir < 0)
                {
                    uncle = grandparent.Right;
                }
                else
                {
                    uncle = grandparent.Left;
                }

                // Case 5 & 6
                if (uncle is null || uncle.Color == NodeColor.Black)
                {
                    AddCase56(node, parentDir, childDir);

                    break;
                }

                // Case 2
                node.Color = NodeColor.Black;
                uncle!.Color = NodeColor.Black;
                grandparent.Color = NodeColor.Red;

                // Keep root black
                if (node.Parent.Parent is null)
                {
                    node.Parent.Color = NodeColor.Black;
                }
                else
                {
                    childDir = comparer.Compare(node.Parent.Key, node.Parent.Parent.Key);
                }

                // Set current node as parent to move up tree
                node = node.Parent.Parent;
            }
            while (node is not null);

            Count++;
        }

        /// <summary>
        ///     Add multiple nodes to the tree.
        /// </summary>
        /// <param name="keys">Key values to add.</param>
        public void AddRange(IEnumerable<TKey> keys)
        {
            foreach (TKey key in keys)
            {
                Add(key);
            }
        }

        /// <summary>
        ///     Remove a node from the tree.
        /// </summary>
        /// <param name="key">Key value to remove.</param>
        public void Remove(TKey key)
        {
            // Search for node
            var node = Remove(root, key);

            // Simple cases
            node = RemoveSimpleCases(node);

            // Exit if deleted node was not non-root black leaf
            if (node is null)
            {
                return;
            }

            // Delete node
            var dir = comparer.Compare(node.Key, node.Parent!.Key);
            if (dir < 0)
            {
                node.Parent.Left = null;
            }
            else
            {
                node.Parent.Right = null;
            }

            // Recolor tree
            do
            {
                dir = comparer.Compare(node.Key, node.Parent.Key);

                // Determine current node's sibling and nephews
                RedBlackTreeNode<TKey>? sibling,
                                        distantNephew = null,
                                        closeNewphew = null;
                if (dir < 0)
                {
                    sibling = node.Parent.Right;
                    if (sibling is not null)
                    {
                        distantNephew = sibling.Right;
                        closeNewphew = sibling.Left;
                    }
                }
                else
                {
                    sibling = node.Parent.Left;
                    if (sibling is not null)
                    {
                        distantNephew = sibling.Left;
                        closeNewphew = sibling.Right;
                    }
                }

                if (sibling is not null && sibling.Color == NodeColor.Red)
                {
                    RemoveCase3(sibling, closeNewphew, distantNephew, dir);
                    break;
                }
                else if (distantNephew is not null && distantNephew.Color == NodeColor.Red)
                {
                    RemoveCase6(sibling!, distantNephew, dir);
                    break;
                }
                else if (closeNewphew is not null && closeNewphew.Color == NodeColor.Red)
                {
                    RemoveCase5(sibling!, dir);
                    break;
                }
                else if (node.Parent.Color == NodeColor.Red)
                {
                    RemoveCase4(sibling!);
                    break;
                }
                else
                {
                    // Case 1
                    sibling!.Color = NodeColor.Red;
                    node = node.Parent;
                }
            }
            while (node.Parent is not null);    // Case 2: Reached root

            Count--;
        }

        /// <summary>
        ///     Check if given node is in the tree.
        /// </summary>
        /// <param name="key">Key value to search for.</param>
        /// <returns>Whether or not the node is in the tree.</returns>
        public bool Contains(TKey key)
        {
            var node = root;
            while (node is not null)
            {
                var compareResult = comparer.Compare(key, node.Key);
                if (compareResult < 0)
                {
                    node = node.Left;
                }
                else if (compareResult > 0)
                {
                    node = node.Right;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Get the minimum value in the tree.
        /// </summary>
        /// <returns>Minimum value in tree.</returns>
        public TKey GetMin()
        {
            if (root is null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }

            return GetMin(root).Key;
        }

        /// <summary>
        ///     Get the maximum value in the tree.
        /// </summary>
        /// <returns>Maximum value in tree.</returns>
        public TKey GetMax()
        {
            if (root is null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }

            return GetMax(root).Key;
        }

        /// <summary>
        ///     Get keys in order from smallest to largest as defined by the comparer.
        /// </summary>
        /// <returns>Keys in tree in order from smallest to largest.</returns>
        public IEnumerable<TKey> GetKeysInOrder()
        {
            List<TKey> result = new List<TKey>();
            InOrderWalk(root);
            return result;

            void InOrderWalk(RedBlackTreeNode<TKey>? node)
            {
                if (node is null)
                {
                    return;
                }

                InOrderWalk(node.Left);
                result.Add(node.Key);
                InOrderWalk(node.Right);
            }
        }

        /// <summary>
        ///     Get keys in the pre-order order.
        /// </summary>
        /// <returns>Keys in pre-order order.</returns>
        public IEnumerable<TKey> GetKeysPreOrder()
        {
            var result = new List<TKey>();
            PreOrderWalk(root);
            return result;

            void PreOrderWalk(RedBlackTreeNode<TKey>? node)
            {
                if (node is null)
                {
                    return;
                }

                result.Add(node.Key);
                PreOrderWalk(node.Left);
                PreOrderWalk(node.Right);
            }
        }

        /// <summary>
        ///     Get keys in the post-order order.
        /// </summary>
        /// <returns>Keys in the post-order order.</returns>
        public IEnumerable<TKey> GetKeysPostOrder()
        {
            var result = new List<TKey>();
            PostOrderWalk(root);
            return result;

            void PostOrderWalk(RedBlackTreeNode<TKey>? node)
            {
                if (node is null)
                {
                    return;
                }

                PostOrderWalk(node.Left);
                PostOrderWalk(node.Right);
                result.Add(node.Key);
            }
        }

        /// <summary>
        ///     Perform binary tree insertion.
        /// </summary>
        /// <param name="node">Root of subtree to search from.</param>
        /// <param name="key">Key value to insert.</param>
        /// <returns>Node that the new node was added to.</returns>
        private RedBlackTreeNode<TKey> Add(RedBlackTreeNode<TKey> node, TKey key)
        {
            int compareResult;
            while (true)
            {
                compareResult = comparer.Compare(key, node!.Key);
                if (compareResult < 0)
                {
                    if (node.Left is null)
                    {
                        node.Left = new RedBlackTreeNode<TKey>(key, node);
                        break;
                    }
                    else
                    {
                        node = node.Left;
                    }
                }
                else if (compareResult > 0)
                {
                    if (node.Right is null)
                    {
                        node.Right = new RedBlackTreeNode<TKey>(key, node);
                        break;
                    }
                    else
                    {
                        node = node.Right;
                    }
                }
                else
                {
                    throw new ArgumentException($"Key \"{key}\" already exists in tree!");
                }
            }

            return node;
        }

        /// <summary>
        ///     Perform rotations needed for cases 5 and 6 of insertion.
        /// </summary>
        /// <param name="node">Parent of node just inserted.</param>
        /// <param name="parentDir">The side node is on of its parent.</param>
        /// <param name="childDir">The side the child node is on.</param>
        /// <returns>Node in same position as before but with rotations applied.</returns>
        private RedBlackTreeNode<TKey> AddCase56(RedBlackTreeNode<TKey> node, int parentDir, int childDir)
        {
            if (parentDir < 0)
            {
                // Case 5
                if (childDir > 0)
                {
                    node = RotateLeft(node);
                }

                // Case 6
                node = RotateRight(node.Parent!);
                node.Color = NodeColor.Black;
                node.Right!.Color = NodeColor.Red;
            }
            else
            {
                // Case 5
                if (childDir < 0)
                {
                    node = RotateRight(node);
                }

                // Case 6
                node = RotateLeft(node.Parent!);
                node.Color = NodeColor.Black;
                node.Left!.Color = NodeColor.Red;
            }

            // Update root if it changed
            if (node.Parent is null)
            {
                root = node;
            }

            return node;
        }

        /// <summary>
        ///     Search for the node to be deleted.
        /// </summary>
        /// <param name="node">Node to start search from.</param>
        /// <param name="key">Key to search for.</param>
        /// <returns>Node to be deleted.</returns>
        private RedBlackTreeNode<TKey> Remove(RedBlackTreeNode<TKey>? node, TKey key)
        {
            if (node is null)
            {
                throw new InvalidOperationException("Tree is empty!");
            }
            else if (!Contains(key))
            {
                throw new KeyNotFoundException($"Key {key} is not in the tree!");
            }

            // Find node
            int dir;
            while (true)
            {
                dir = comparer.Compare(key, node!.Key);
                if (dir < 0)
                {
                    node = node.Left;
                }
                else if (dir > 0)
                {
                    node = node.Right;
                }
                else
                {
                    break;
                }
            }

            return node;
        }

        /// <summary>
        ///     Simple removal cases where black height doesn't change.
        /// </summary>
        /// <param name="node">Node to remove.</param>
        /// <returns>Non-root black leaf node or null. Null indicates that removal was performed.</returns>
        private RedBlackTreeNode<TKey>? RemoveSimpleCases(RedBlackTreeNode<TKey> node)
        {
            // Loop until node is a non-root black leaf or node can be deleted without changing black height
            do
            {
                if (node.Parent is null && node.Left is null && node.Right is null)
                {
                    // Node to delete is root and has no children
                    root = null;
                    Count--;
                    return null;
                }
                else if (node.Left is not null && node.Right is not null)
                {
                    // Node has two children. Swap pointers
                    var successor = GetMin(node.Right);
                    node.Key = successor.Key;
                    node = successor;
                }
                else if (node.Color == NodeColor.Red)
                {
                    // Node is red so it must have no children since it doesn't have two children
                    var dir = comparer.Compare(node.Key, node.Parent!.Key);
                    if (dir < 0)
                    {
                        node.Parent.Left = null;
                    }
                    else
                    {
                        node.Parent.Right = null;
                    }

                    Count--;
                    return null;
                }
                else
                {
                    // Node is black and has at most one child. If it has a child it must be red.
                    var child = node.Left ?? node.Right;

                    // Continue to recoloring if node is leaf
                    if (child is null)
                    {
                        break;
                    }

                    child.Color = NodeColor.Black;
                    child.Parent = node.Parent;

                    if (node.Parent is null)
                    {
                        // Root has one child
                        root = child;
                    }
                    else
                    {
                        // Replace node with child
                        var dir = comparer.Compare(node.Key, node.Parent.Key);
                        if (dir < 0)
                        {
                            node.Parent.Left = child;
                        }
                        else
                        {
                            node.Parent.Right = child;
                        }
                    }

                    Count--;
                    return null;
                }
            }
            while (node.Color == NodeColor.Red || node.Left is not null || node.Right is not null);

            return node;
        }

        /// <summary>
        ///     Perform case 3 of removal.
        /// </summary>
        /// <param name="sibling">Sibling of removed node.</param>
        /// <param name="closeNephew">Close nephew of removed node.</param>
        /// <param name="distantNephew">Distant nephew of removed node.</param>
        /// <param name="childDir">Side of parent the removed node was.</param>
        /// <returns>Updated sibling node.</returns>
        private RedBlackTreeNode<TKey> RemoveCase3(RedBlackTreeNode<TKey>? sibling, RedBlackTreeNode<TKey>? closeNephew, RedBlackTreeNode<TKey>? distantNephew, int childDir)
        {
            // Rotate and recolor
            if (childDir < 0)
            {
                sibling = RotateLeft(sibling!.Parent!);
                sibling.Left!.Color = NodeColor.Red;
                sibling.Color = NodeColor.Black;
            }
            else
            {
                sibling = RotateRight(sibling!.Parent!);
                sibling.Right!.Color = NodeColor.Red;
                sibling.Color = NodeColor.Black;
            }

            // Correct for rotation about root
            if (sibling.Parent is null)
            {
                root = sibling;
            }

            // Get new distant newphew
            sibling = closeNephew;
            if (childDir < 0)
            {
                distantNephew = sibling is null ? null : sibling.Right;
            }
            else
            {
                distantNephew = sibling is null ? null : sibling.Left;
            }

            // Parent is red, sibling is black
            if (distantNephew is not null && distantNephew.Color == NodeColor.Red)
            {
                return RemoveCase6(sibling!, distantNephew, childDir);
            }

            // Get new close nephew
            if (sibling is null)
            {
                closeNephew = null;
            }
            else
            {
                closeNephew = childDir < 0 ? sibling!.Left : sibling!.Right;
            }

            // Sibling is black, distant nephew is black
            if (closeNephew is not null && closeNephew.Color == NodeColor.Red)
            {
                return RemoveCase5(sibling!, childDir);
            }

            // Final recoloring
            return RemoveCase4(sibling!);
        }

        /// <summary>
        ///     Perform case 4 of removal.
        /// </summary>
        /// <param name="sibling">Sibling of removed node.</param>
        /// <returns>Updated sibling node.</returns>
        private RedBlackTreeNode<TKey> RemoveCase4(RedBlackTreeNode<TKey> sibling)
        {
            sibling.Color = NodeColor.Red;
            sibling.Parent!.Color = NodeColor.Black;
            return sibling;
        }

        /// <summary>
        ///     Perform case 5 of removal.
        /// </summary>
        /// <param name="sibling">Sibling of removed node.</param>
        /// <param name="childDir">Side of parent removed node was on.</param>
        /// <returns>Updated sibling node.</returns>
        private RedBlackTreeNode<TKey> RemoveCase5(RedBlackTreeNode<TKey> sibling, int childDir)
        {
            RedBlackTreeNode<TKey>? distantNephew;
            if (childDir < 0)
            {
                sibling = RotateRight(sibling);
                sibling.Color = NodeColor.Black;
                if (sibling.Right is not null)
                {
                    sibling.Right.Color = NodeColor.Red;
                }

                distantNephew = sibling.Right;

                // closeNephew = sibling.Left;.
            }
            else
            {
                sibling = RotateLeft(sibling);
                sibling.Color = NodeColor.Black;
                if (sibling.Left is not null)
                {
                    sibling.Left.Color = NodeColor.Red;
                }

                distantNephew = sibling.Left;

                // closeNephew = sibling.Right;.
            }

            return RemoveCase6(sibling, distantNephew!, childDir);
        }

        /// <summary>
        ///     Perform case 6 of removal.
        /// </summary>
        /// <param name="sibling">Sibling of removed node.</param>
        /// <param name="distantNephew">Distant nephew of removed node.</param>
        /// <param name="childDir">Side of parent removed node was on.</param>
        /// <returns>Updated sibling node.</returns>
        private RedBlackTreeNode<TKey> RemoveCase6(RedBlackTreeNode<TKey> sibling, RedBlackTreeNode<TKey> distantNephew, int childDir)
        {
            sibling = childDir < 0 ? RotateLeft(sibling.Parent!) : RotateRight(sibling.Parent!);
            sibling.Color = sibling.Parent!.Color;
            sibling.Parent.Color = NodeColor.Black;
            distantNephew.Color = NodeColor.Black;

            if (sibling.Parent is null)
            {
                root = sibling;
            }

            return sibling;
        }

        /// <summary>
        ///     Perform a left (counter-clockwise) rotation.
        /// </summary>
        /// <param name="node">Node to rotate about.</param>
        /// <returns>New node with rotation applied.</returns>
        private RedBlackTreeNode<TKey> RotateLeft(RedBlackTreeNode<TKey> node)
        {
            var temp1 = node;
            var temp2 = node!.Right!.Left;

            node = node.Right;
            node.Parent = temp1.Parent;
            if (node.Parent is not null)
            {
                var nodeDir = comparer.Compare(node.Key, node.Parent.Key);
                if (nodeDir < 0)
                {
                    node.Parent.Left = node;
                }
                else
                {
                    node.Parent.Right = node;
                }
            }

            node.Left = temp1;
            node.Left.Parent = node;

            node.Left.Right = temp2;
            if (temp2 is not null)
            {
                node.Left.Right!.Parent = temp1;
            }

            return node;
        }

        /// <summary>
        ///     Perform a right (clockwise) rotation.
        /// </summary>
        /// <param name="node">Node to rotate about.</param>
        /// <returns>New node with rotation applied.</returns>
        private RedBlackTreeNode<TKey> RotateRight(RedBlackTreeNode<TKey> node)
        {
            var temp1 = node;
            var temp2 = node!.Left!.Right;

            node = node.Left;
            node.Parent = temp1.Parent;
            if (node.Parent is not null)
            {
                var nodeDir = comparer.Compare(node.Key, node.Parent.Key);
                if (nodeDir < 0)
                {
                    node.Parent.Left = node;
                }
                else
                {
                    node.Parent.Right = node;
                }
            }

            node.Right = temp1;
            node.Right.Parent = node;

            node.Right.Left = temp2;
            if (temp2 is not null)
            {
                node.Right.Left!.Parent = temp1;
            }

            return node;
        }

        /// <summary>
        ///     Helper function to get node instance with minimum key value
        ///     in the specified subtree.
        /// </summary>
        /// <param name="node">Node specifying root of subtree.</param>
        /// <returns>Minimum value in node's subtree.</returns>
        private RedBlackTreeNode<TKey> GetMin(RedBlackTreeNode<TKey> node)
        {
            while (node.Left is not null)
            {
                node = node.Left;
            }

            return node;
        }

        /// <summary>
        ///     Helper function to get node instance with maximum key value
        ///     in the specified subtree.
        /// </summary>
        /// <param name="node">Node specifyng root of subtree.</param>
        /// <returns>Maximum value in node's subtree.</returns>
        private RedBlackTreeNode<TKey> GetMax(RedBlackTreeNode<TKey> node)
        {
            while (node.Right is not null)
            {
                node = node.Right;
            }

            return node;
        }
    }
}
