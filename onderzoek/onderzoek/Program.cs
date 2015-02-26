using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
		public BPlusTree(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

		public bool Find(TData element){
			return false;
		}
	}

	class BTree<TData> : ISearchTree<TData> where TData : IComparable{
		public BTree(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

		public bool Find(TData element){
			return false;
		}
	}

	class AvlTree<TData> : ISearchTree<TData> where TData : IComparable{
		public AvlTree(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

		public bool Find(TData element){
			return false;
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

