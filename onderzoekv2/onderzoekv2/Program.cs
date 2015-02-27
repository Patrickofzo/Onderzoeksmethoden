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
            Test<string> stringTest = new Test<string>(new string[] { "hallo", "ingmar" });
		}
	}

	interface ISearchTree<TData> where TData : IComparable, IComparable<TData>
	{
		void Add(TData element);
		bool Find(TData element);
	}

    class RedBlack<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>{
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

    class BPlusTree<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
		CSharpTest.Net.Collections.BPlusTree<TData, int> _tree;
		public BPlusTree(TData[] data){
			this._tree = new CSharpTest.Net.Collections.BPlusTree<TData, int>();
            foreach (TData ele in data)
            {
                this.Add(ele);
            }
		}

		public void Add(TData element){
			this._tree.Add(element, 0);
		}

		public bool Find(TData element){
			return this._tree.Contains(new KeyValuePair<TData, int>(element, 0));
		}
	}

    class BTree<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
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

	class AvlTree<T> : ISearchTree<T> where T : IComparable, IComparable<T>{
		System.DataStructures.AvlTree<T> _tree;
		public AvlTree(T[] data){
            this._tree = new System.DataStructures.AvlTree<T>(data);
		}

		public void Add(T element){
			this._tree.Add(element);
		}

		public bool Find(T element){
			return this._tree.Contains(element);
		}
	}

    class ScapeGoat<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        SpaceGoat<TData> _tree;
		public ScapeGoat(TData[] data){
            this._tree = new SpaceGoat<TData>();
            foreach (TData ele in data)
            {
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

    class BinHeap<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        BinaryHeap<TData> _tree;
        public BinHeap(TData[] data){
            this._tree = new BinaryHeap<TData>();
            foreach (TData ele in data)
            {
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

    class Beap<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
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

