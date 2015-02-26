using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

//NuGet packages
using CSharpTest.Net.Collections;
using System.DataStructures;

namespace onderzoek {
	class MainClass {
		public static void Main(string[] args) {
			StringTests("stringdata");
		}

		public static void StringTests(string filename){
			using(StreamReader sr = new StreamReader(filename)) {
				string[] data = sr.ReadLine().Split(new char[]{ ' ' });

				ISearchTree<string> rb = new RedBlack<string>(data);
				ISearchTree<string> b = new BTree<string>(data);
				ISearchTree<string> bplus = new BPlusTree<string>(data);
				ISearchTree<string> avl = new AvlTree<string>(data);
				ISearchTree<string> sg = new ScapeGoat<string>(data);
				ISearchTree<string> bh = new BinaryHeap<string>(data);
				ISearchTree<string> beap = new Beap<string>(data);
			}
		}
	}

	interface ISearchTree<TData> where TData : IComparable
	{
		void Add(TData element);
		bool Find(TData element);
	}

	class RedBlack<TData> : ISearchTree<TData> where TData : IComparable{
		RedBlackTree<RedBlackNode<TData, int>, TData, int> _tree;

		public RedBlack(){
			this._tree = new RedBlackTree<RedBlackNode<TData, int>, TData, int>();
		}
		public RedBlack(TData[] elements){
			this._tree = new RedBlackTree<RedBlackNode<TData, int>, TData, int>();
			foreach(TData ele in elements) {
				this.Add(ele);
			}
		}

		public void Add(TData element){
			RedBlackNode<TData, int> node = new RedBlackNode<TData, int>(element, 0);
			this._tree.InsertNode(node);
		}

		public bool Find(TData element){
			return this._tree.Find(element) != null;
		}
	}

	class BPlusTree<TData> : ISearchTree<TData> where TData : IComparable{
		CSharpTest.Net.Collections.BPlusTree<TData, int> _tree;
		public BPlusTree(TData[] data){
			this._tree = new CSharpTest.Net.Collections.BPlusTree<TData, int>();
		}

		public void Add(TData element){
			this._tree.Add(TData, 0);
		}

		public bool Find(TData element){
			return this._tree.Contains(new KeyValuePair<TData, int>(element, 0));
		}
	}

	class BTree<TData> : ISearchTree<TData> where TData : IComparable{
		CSharpTest.Net.Collections.BTreeList<TData> _tree;
		public BTree(TData[] data){
			this._tree = new BTreeList<TData>();
			foreach(TData ele in data){
				this.Add(ele);
			}
		}

		public void Add(TData element){
			this._tree.Add(element);
		}

		public bool Find(TData element){
			return this._tree.Contains(element);
		}
	}

	class AvlTree<TData> : ISearchTree<TData> where TData : IComparable{
		System.DataStructures.AvlTree<TData> _tree;
		public AvlTree(TData[] data){
			this._tree = new System.DataStructures.AvlTree<TData>(data);
		}

		public void Add(TData element){
			this._tree.Add(element);
		}

		public bool Find(TData element){
			return this._tree.Contains(element);
		}
	}

	class ScapeGoat<TData> : ISearchTree<TData> where TData : IComparable{
		public ScapeGoat(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

		public bool Find(TData element){
			return false;
		}
	}

	class BinaryHeap<TData> : ISearchTree<TData> where TData : IComparable{
		public BinaryHeap(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

		public bool Find(TData element){
			return false;
		}
	}

	class Beap<TData> : ISearchTree<TData> where TData : IComparable{
		public Beap(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

		public bool Find(TData element){
			return false;
		}
	}
}

