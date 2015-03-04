using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

//NuGet packages
using CSharpTest.Net.Collections;
using System.DataStructures;

namespace onderzoek {
	class MainClass {
		public static void Main(string[] args) {
            Test<string> stringTest = new Test<string>(new string[] { "hallo", "ingmar" });
		}

        public int[] randomInts() 
        {
            int[] list = new int[1000];
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
                list[i] = random.Next(int.MaxValue);
            return list;
        }

        public char[] randomChar()
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] list = new char[1000];
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
                list[i] = s[random.Next(s.Length)];
            return list;
        }

        public string[] randomString()
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string[] list = new string[1000];
            Random random1 = new Random();
            Random random2 = new Random();
            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 10; j++)
                    if(random2.Next(5) == 1)
                        list[i] = list[i] + s[random1.Next(s.Length)];
            return list;
        }
	}

	interface ISearchTree<TData> where TData : IComparable, IComparable<TData>
	{
		void Add(TData element);
        Tuple<int, bool> Find(TData element);
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

        public Tuple<int, bool> Find(TData element)
        {
            RedBlackNode<TData, int> node;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            node = this._tree.Find(element);
            timer.Stop();
            return new Tuple<int, bool>(timer.Elapsed.Milliseconds, node != null);
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

        public Tuple<int, bool> Find(TData element)
        {
            bool b;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(new KeyValuePair<TData, int>(element, 0));
            timer.Stop();
            return new Tuple<int, bool>(timer.Elapsed.Milliseconds, b);
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

        public Tuple<int, bool> Find(TData element)
        {
            bool b;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<int, bool>(timer.Elapsed.Milliseconds, b);
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

        public Tuple<int, bool> Find(T element)
        {
            bool b;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<int, bool>(timer.Elapsed.Milliseconds, b);
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

        public Tuple<int, bool> Find(TData element)
        {
            bool b;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<int, bool>(timer.Elapsed.Milliseconds, b);
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

        public Tuple<int, bool> Find(TData element)
        {
            bool b;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<int, bool>(timer.Elapsed.Milliseconds, b);
		}
	}

    class Beap<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
		public Beap(TData[] data){
			Console.WriteLine("Hello");
		}

		public void Add(TData element){
		}

        public Tuple<int, bool> Find(TData element)
        {
            return new Tuple<int, bool>(0, false);
		}
	}
}

