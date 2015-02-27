using System.Collections.Generic;

namespace System.DataStructures
{
	/// <summary>
	/// A binary search tree (BST).
	/// </summary>
	/// <typeparam name="T">Type of <see cref="BinarySearchTree{T}"/>.</typeparam>
	[Serializable]
	internal class BinarySearchTree<T> : BinaryTreeBase<BinaryTreeNode<T>, T>
		where T : IComparable<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BinarySearchTree{T}"/> class.
		/// </summary>
		public BinarySearchTree() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinarySearchTree{T}"/> class, populating it with the items from the
		/// <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <param name="values">Items to populate <see cref="BinarySearchTree{T}"/>.</param>
		public BinarySearchTree(IEnumerable<T> values)
			: this()
		{
			AddRange(values);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinarySearchTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public BinarySearchTree(IComparer<T> comparer)
			: base(comparer) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinarySearchTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public BinarySearchTree(Func<T, T, int> comparer)
			: base(comparer) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinarySearchTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="comparer">The comparer.</param>
		public BinarySearchTree(IEnumerable<T> values, IComparer<T> comparer)
			: base(comparer)
		{
			AddRange(values);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinarySearchTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="comparer">The comparer.</param>
		public BinarySearchTree(IEnumerable<T> values, Func<T, T, int> comparer)
			: base(comparer)
		{
			AddRange(values);
		}

		/// <summary>
		/// Inserts a new node with the specified value at the appropriate location in the <see cref="BinarySearchTree{T}"/>.
		/// </summary>
		/// <remarks>
		/// This method is an O(log n) operation.
		/// </remarks>
		/// <param name="item">Value to insert.</param>
		public override void Add(T item)
		{
			if (Root == null)
			{
				Root = new BinaryTreeNode<T>(item);
			}
			else
			{
				InsertNode(item);
			}
			Count++;
		}

		/// <summary>
		/// Removes a node with the specified value from the <see cref="BinarySearchTree{T}"/>.
		/// </summary>
		/// <remarks>
		/// This method is an O(log n) operation.
		/// </remarks>
		/// <param name="item">Item to remove from the the <see cref="BinarySearchTree{T}"/>.</param>
		/// <returns>True if the item was removed; otherwise false.</returns>
		public override bool Remove(T item)
		{
			BinaryTreeNode<T> nodeToRemove = Root;
			BinaryTreeNode<T> parent = null;

			while ((nodeToRemove != null) && (!item.IsEqual(nodeToRemove.Value, Comparer)))
			{
				parent = nodeToRemove;
				nodeToRemove = item.IsLessThan(nodeToRemove.Value, Comparer) ? nodeToRemove.Left : nodeToRemove.Right;
			}
			if (nodeToRemove == null)
			{
				return false;
			}
			if (Count == 1)
			{
				Root = null;
			}
			else if (nodeToRemove.Left == null && nodeToRemove.Right == null)
			{
				// nodeToRemove is a leaf
				if (nodeToRemove.Value.IsLessThan(parent.Value, Comparer))
				{
					parent.Left = null;
				}
				else
				{
					parent.Right = null;
				}
			}
			else if (nodeToRemove.Left == null)
			{
				// nodeToRemove has only a right subtree
				if (nodeToRemove.Value.IsLessThan(parent.Value, Comparer))
				{
					parent.Left = nodeToRemove.Right;
				}
				else
				{
					parent.Right = nodeToRemove.Right;
				}
			}
			else if (nodeToRemove.Right == null)
			{
				// nodeToRemove has only a left subtree
				if (nodeToRemove.Value.IsLessThan(parent.Value, Comparer))
				{
					parent.Left = nodeToRemove.Left;
				}
				else
				{
					parent.Right = nodeToRemove.Left;
				}
			}
			else
			{
				// nodeToRemove has both a left and right subtree
				BinaryTreeNode<T> largestValue = nodeToRemove.Left;

				// find the largest value in the left subtree of nodeToRemove
				while (largestValue.Right != null)
				{
					largestValue = largestValue.Right;
				}

				// find the parent of the largest value in the left subtree of nodeToDelete and sets its right property to null.
				FindParent(largestValue.Value).Right = null;

				// set value of nodeToRemove to the value of largestValue
				nodeToRemove.Value = largestValue.Value;
			}

			Count--;
			return true;
		}

		/// <summary>
		/// Called by the Add method. Finds the location where to put the node in the <see cref="BinarySearchTree{T}"/>.
		/// </summary>        
		/// <param name="value">Value to insert into the Bst.</param>
		private void InsertNode(T value)
		{
			BinaryTreeNode<T> current = Root;
			while (true)
			{
				if (value.IsLessThan(current.Value, Comparer))
				{
					//We go left and if arrived at a leaf insert the node and return
					if (current.Left != null)
					{
						current = current.Left;
					}
					else
					{
						current.Left = new BinaryTreeNode<T>(value);
						return;
					}
				}
				else if (value.IsGreaterThan(current.Value, Comparer))
				{
					//We go right and if arrived at a leaf insert the node and return
					if (current.Right != null)
					{
						current = current.Right;
					}
					else
					{
						current.Right = new BinaryTreeNode<T>(value);
						return;
					}
				}
					// The value to insert is already present, we simply return
				else
				{
					return;
				}
			}
		}
	}
}