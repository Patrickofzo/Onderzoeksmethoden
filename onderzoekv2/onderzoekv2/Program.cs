using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

//NuGet packages
using CSharpTest.Net.Collections;
using System.DataStructures;

namespace onderzoek
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string fname = "testresults.txt";
            StreamWriter sw;
            if(File.Exists(fname)){
            sw = File.AppendText(fname);
            }
            else{
            sw = new StreamWriter(fname);
            }
            performIntTests(sw, 10);
            sw.Close();
        }

        public static void performIntTests(StreamWriter sw, int iterations)
        {
            Dictionary<string, Tuple<float, long>> createMeans = new Dictionary<string,Tuple<float,long>>();
            Dictionary<string, Tuple<float, long>> findMeans = new Dictionary<string,Tuple<float,long>>();
            Test<int> test;
            bool use = true;
            sw.WriteLine("Creating Integer Trees");
            for (int length = 1000; length <= 1001; length *= 10)
            {
                
                while (iterations > 0) {
                    iterations--;
                    test = new Test<int>(randomInts(length));
                    use = true;
                    foreach (KeyValuePair<string, Tuple<float, long>> kv in test.CreateTrees())
                    {
                        if (kv.Value.Item1 <= 0 || kv.Value.Item2 <= 0)
                        {
                            use = false;
                            break;
                        }
                    }
                    if (!use)
                    {
                        iterations++;
                        // iterate again to use another dataset
                    }
                    else{
                        try
                        {
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in test.CreateTrees())
                            {
                                createMeans[kv.Key] = new Tuple<float,long>(
                                    createMeans[kv.Key].Item1 + kv.Value.Item1,    
                                    createMeans[kv.Key].Item2 + kv.Value.Item2
                                );
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in test.CreateTrees())
                            {
                                createMeans[kv.Key] = new Tuple<float,long>(
                                    kv.Value.Item1,    
                                    kv.Value.Item2
                                );
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, Tuple<float, long>> kv in createMeans)
                {
                    sw.WriteLine("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1 / iterations, kv.Value.Item2 / iterations);
                }
            }
        }

        public static int[] randomInts(int length)
        {
            int[] list = new int[length];
            Random random = new Random();
            for (int i = 0; i < list.Length; i++)
                list[i] = i;
            list = list.OrderBy(x => random.Next()).ToArray();
            return list;
        }

        public static string[] randomStrings(int length)
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_!@#$%&*()";
            string[] list = new string[length];
            Random random1 = new Random();
            Random random2 = new Random();
            for (int i = 0; i < length; i++)
                for (int j = 0; j < 50; j++)
                    list[i] = list[i] + s[random1.Next(s.Length)];
            return list;
        }
    }

    interface ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        string Name();
        void Add(TData element);
        Tuple<string, long, bool, float> Find(TData element);
    }

    class RedBlack<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        RedBlackTree<RedBlackNode<TData, int>, TData, int> _tree;
        public string Name() { return "RedBlk"; }

        public RedBlack()
        {
            this._tree = new RedBlackTree<RedBlackNode<TData, int>, TData, int>();
        }
        public RedBlack(TData[] elements)
        {
            this._tree = new RedBlackTree<RedBlackNode<TData, int>, TData, int>();
            foreach (TData ele in elements)
            {
                this.Add(ele);
            }
        }

        public void Add(TData element)
        {
            RedBlackNode<TData, int> node = new RedBlackNode<TData, int>(element, 0);
            this._tree.InsertNode(node);
        }

        public Tuple<string, long, bool, float> Find(TData element)
        {
            GC.WaitForFullGCComplete();
            RedBlackNode<TData, int> node;
            float m = GC.GetTotalMemory(false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            node = this._tree.Find(element);
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, node != null, GC.GetTotalMemory(false));
        }
    }

    class BPlusTree<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        CSharpTest.Net.Collections.BPlusTree<TData, int> _tree;
        public string Name() { return "BPlus"; }

        public BPlusTree(TData[] data)
        {
            this._tree = new CSharpTest.Net.Collections.BPlusTree<TData, int>();
            foreach (TData ele in data)
            {
                this.Add(ele);
            }
        }

        public void Add(TData element)
        {
            this._tree.Add(element, 0);
        }

        public Tuple<string, long, bool, float> Find(TData element)
        {
            bool b;
            GC.WaitForFullGCComplete();
            float m = GC.GetTotalMemory(false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(new KeyValuePair<TData, int>(element, 0));
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, GC.GetTotalMemory(false));
        }
    }

    class BTree<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        CSharpTest.Net.Collections.BTreeList<TData> _tree;
        public string Name() { return "BTree"; }

        public BTree(TData[] data)
        {
            this._tree = new BTreeList<TData>();
            foreach (TData ele in data)
            {
                this.Add(ele);
            }
        }

        public void Add(TData element)
        {
            this._tree.Add(element);
        }

        public Tuple<string, long, bool, float> Find(TData element)
        {
            bool b;
            GC.WaitForFullGCComplete();
            float m = GC.GetTotalMemory(false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, GC.GetTotalMemory(false));
        }
    }

    class AvlTree<T> : ISearchTree<T> where T : IComparable, IComparable<T>
    {
        System.DataStructures.AvlTree<T> _tree;
        public string Name() { return "AVL"; }

        public AvlTree(T[] data)
        {
            this._tree = new System.DataStructures.AvlTree<T>(data);
        }

        public void Add(T element)
        {
            this._tree.Add(element);
        }

        public Tuple<string, long, bool, float> Find(T element)
        {
            bool b;
            GC.WaitForFullGCComplete();
            float m = GC.GetTotalMemory(false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, GC.GetTotalMemory(false));
        }
    }

    class ScapeGoat<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        SpaceGoat<TData> _tree;
        public string Name() { return "Goat"; }
        public ScapeGoat(TData[] data)
        {
            this._tree = new SpaceGoat<TData>();
            foreach (TData ele in data)
            {
                this.Add(ele);
            }
        }

        public void Add(TData element)
        {
            this._tree.Add(element);
        }

        public Tuple<string, long, bool, float> Find(TData element)
        {
            bool b;
            GC.WaitForFullGCComplete();
            float m = GC.GetTotalMemory(false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, GC.GetTotalMemory(false));
        }
    }

    class BinHeap<TData> : ISearchTree<TData> where TData : IComparable, IComparable<TData>
    {
        BinaryHeap<TData> _tree;
        public string Name() { return "BinHeap"; }
        public BinHeap(TData[] data)
        {
            this._tree = new BinaryHeap<TData>();
            foreach (TData ele in data)
            {
                this.Add(ele);
            }
        }

        public void Add(TData element)
        {
            this._tree.Add(element);
        }

        public Tuple<string, long, bool, float> Find(TData element)
        {
            bool b;
            GC.WaitForFullGCComplete();
            float m = GC.GetTotalMemory(false);
            Stopwatch timer = new Stopwatch();
            timer.Start();
            b = this._tree.Contains(element);
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, GC.GetTotalMemory(false));
        }
    }

}

