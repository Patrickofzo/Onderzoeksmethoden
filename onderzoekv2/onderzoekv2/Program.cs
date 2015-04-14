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
            StreamWriter sw_create_mem, sw_create_spd, sw_search_mem, sw_search_spd;
            sw_create_mem = CreateWriter("results\\create_memory.csv");
            sw_create_spd = CreateWriter("results\\create_speed.csv");
            sw_search_mem = CreateWriter("results\\search_memory.csv");
            sw_search_spd = CreateWriter("results\\search_speed.csv");
            performIntTests(sw_create_mem, sw_create_spd, sw_search_mem, sw_search_spd, 10, 10);
            sw_create_mem.Close();
            sw_create_spd.Close();
            sw_search_mem.Close();
            sw_search_spd.Close();
            Console.ReadLine();
        }

        public static StreamWriter CreateWriter(string filename)
        {
            if (File.Exists(filename))
                return File.AppendText(filename);
            else
                return new StreamWriter(filename); 
        }

        public static void performStringTests(StreamWriter sw_create_mem, StreamWriter sw_create_spd, StreamWriter sw_search_mem, StreamWriter sw_search_spd, int create_iterations, int find_iterations)
        {
            Console.WriteLine("String Test");
            Console.WriteLine("\r{0}\t{1}\t{2}\t{3}", "-", "+", "-", "+");
            Dictionary<string, Tuple<float, long>> createMeans;
            Dictionary<string, Tuple<float, long>> findMeans;
            Test<string> test;
            bool use = true;
            bool useFind = true;
            int negatives, goodies, negaFinds, goodieFinds;
            sw_create_mem.WriteLine("# # # # # # # # # # # # # # # #");
            sw_create_spd.WriteLine("# # # # # # # # # # # # # # # #");
            sw_search_mem.WriteLine("# # # # # # # # # # # # # # # #");
            sw_search_spd.WriteLine("# # # # # # # # # # # # # # # #");
            sw_create_mem.WriteLine("Creating Integer Trees (memory)");
            sw_create_spd.WriteLine("Creating Integer Trees (speed)");
            sw_search_mem.WriteLine("Searching Integer Trees (memory)");
            sw_search_spd.WriteLine("Searching Integer Trees (speed)");
            bool firstWrite = true;
            for (int length = 1000; length <= 1000 * 1000; length *= 10)
            {
                createMeans = new Dictionary<string, Tuple<float, long>>();
                findMeans = new Dictionary<string, Tuple<float, long>>();
                negatives = 0;
                goodies = 0;
                negaFinds = 0;
                goodieFinds = 0;
                
                Console.WriteLine("{0}\t\t{1}", DateTime.Now, length);
                for (int i = 0; i < create_iterations; i++)
                {
                    test = new Test<string>(randomStrings(length));
                    use = true;
                    var newTrees = test.CreateTrees();
                    foreach (KeyValuePair<string, Tuple<float, long>> kv in newTrees)
                    {
                        if (firstWrite)
                        {
                            sw_create_mem.Write(",{0}", kv.Key);
                            sw_create_spd.Write(",{0}", kv.Key);
                            sw_search_mem.Write(",{0}", kv.Key);
                            sw_search_spd.Write(",{0}", kv.Key);
                        }
                        if (/*kv.Value.Item1 <= 0 || */kv.Value.Item2 <= 0)
                        {
                            use = false;
                            break;
                        }
                    }
                    if (firstWrite)
                    {
                        sw_create_mem.WriteLine();
                        sw_create_spd.WriteLine();
                        sw_search_mem.WriteLine();
                        sw_search_spd.WriteLine();
                        firstWrite = false;
                    }
                    if (!use)
                    {
                        negatives += 1;
                        Console.Write("\r{0}\t{1}\t{2}\t{3}", negatives, goodies, negaFinds, goodieFinds);
                        i--;
                        // iterate again to use another dataset
                    }
                    else
                    {
                        goodies += 1;
                        Console.Write("\r{0}\t{1}\t{2}\t{3}", negatives, goodies, negaFinds, goodieFinds);

                        sw_create_mem.Write("{0}", length);
                        sw_create_spd.Write("{0},", length);
                        foreach (KeyValuePair<string, Tuple<float, long>> kv in newTrees)
                        {
                            //sw_create_mem.Write("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1, kv.Value.Item2);
                            sw_create_mem.Write(",{0}", kv.Value.Item1);
                            sw_create_spd.Write(",{0},", kv.Value.Item2);
                        }
                        sw_create_mem.WriteLine();
                        sw_create_spd.WriteLine();

                        try
                        {
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in newTrees)
                            {
                                createMeans[kv.Key] = new Tuple<float, long>(
                                    createMeans[kv.Key].Item1 + (kv.Value.Item1 / create_iterations),
                                    createMeans[kv.Key].Item2 + (kv.Value.Item2 / create_iterations)
                                );
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in test.CreateTrees())
                            {
                                createMeans[kv.Key] = new Tuple<float, long>(
                                    kv.Value.Item1 / create_iterations,
                                    kv.Value.Item2 / create_iterations
                                );
                            }
                        }
                    }
                    // Do Search Tests
                    for (int j = 0; j < find_iterations && use; j++)
                    {
                        useFind = true;
                        var findResults = test.FindInTrees();
                        foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                        {
                            if (/*kv.Value.Item1 <= 0 || */kv.Value.Item2 <= 0)
                            {
                                useFind = false;
                                break;
                            }
                        }
                        if (!useFind)
                        {
                            negaFinds += 1;
                            Console.Write("\r{0}\t{1}\t{2}\t{3}", negatives, goodies, negaFinds, goodieFinds);
                            j--;
                            // iterate again to use another dataset
                        }
                        else
                        {
                            goodieFinds += 1;
                            Console.Write("\r{0}\t{1}\t{2}\t{3}", negatives, goodies, negaFinds, goodieFinds);

                            sw_search_mem.Write("{0}", length);
                            sw_search_spd.Write("{0},", length);
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                            {
                                sw_search_mem.Write(",{0}", kv.Value.Item1);
                                sw_search_spd.Write(",{0}", kv.Value.Item2);
                            }
                            sw_search_mem.WriteLine();
                            sw_search_spd.WriteLine();

                            continue;
                            try
                            {
                                foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                                {
                                    findMeans[kv.Key] = new Tuple<float, long>(
                                        findMeans[kv.Key].Item1 + (kv.Value.Item1 / (find_iterations * create_iterations)),
                                        findMeans[kv.Key].Item2 + (kv.Value.Item2 / (find_iterations * create_iterations))
                                    );
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                                {
                                    findMeans[kv.Key] = new Tuple<float, long>(
                                        kv.Value.Item1 / (find_iterations * create_iterations),
                                        kv.Value.Item2 / (find_iterations * create_iterations)
                                    );
                                }
                            }
                        }
                    }
                }
                /*foreach (KeyValuePair<string, Tuple<float, long>> kv in createMeans)
                {
                    sw_create.WriteLine("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1, kv.Value.Item2);
                }
                foreach (KeyValuePair<string, Tuple<float, long>> kv in findMeans)
                {
                    sw_find.WriteLine("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1, kv.Value.Item2);
                }*/
                sw_search_mem.WriteLine();
                sw_search_spd.WriteLine();
                sw_create_mem.WriteLine();
                sw_create_spd.WriteLine();

                sw_create_mem.Flush();
                sw_search_mem.Flush();
                sw_create_spd.Flush();
                sw_search_spd.Flush();
                Console.WriteLine();
            }
        }

        public static void performIntTests(StreamWriter sw_create_mem, StreamWriter sw_create_spd, StreamWriter sw_search_mem, StreamWriter sw_search_spd, int create_iterations, int find_iterations)
        {
            Dictionary<string, Tuple<float, long>> createMeans;
            Dictionary<string, Tuple<float, long>> findMeans;
            Test<int> test;
            bool use = true;
            bool useFind = true;
            int negatives, goodies, negaFinds, goodieFinds;
            sw_create_mem.WriteLine("# # # # # # # # # # # # # # # #");
            sw_create_spd.WriteLine("# # # # # # # # # # # # # # # #");
            sw_search_mem.WriteLine("# # # # # # # # # # # # # # # #");
            sw_search_spd.WriteLine("# # # # # # # # # # # # # # # #");
            sw_create_mem.WriteLine("Creating Integer Trees (memory)");
            sw_create_spd.WriteLine("Creating Integer Trees (speed)");
            sw_search_mem.WriteLine("Searching Integer Trees (memory)");
            sw_search_spd.WriteLine("Searching Integer Trees (speed)");
            bool firstWrite = true;

            for (int length = 1000; length <= 1000 * 1000; length *= 10)
            {
                createMeans = new Dictionary<string, Tuple<float, long>>();
                findMeans = new Dictionary<string, Tuple<float, long>>();
                negatives = 0;
                goodies = 0;
                negaFinds = 0;
                goodieFinds = 0;
                Console.WriteLine("{0}\t\t{1}", DateTime.Now, length);
                for (int i = 0; i < create_iterations; i++ )
                {
                    test = new Test<int>(randomInts(length));
                    use = true;
                    var newTrees = test.CreateTrees();
                    foreach (KeyValuePair<string, Tuple<float, long>> kv in newTrees)
                    {
                        if (firstWrite)
                        {
                            sw_create_mem.Write(",{0}", kv.Key);
                            sw_create_spd.Write(",{0}", kv.Key);
                            sw_search_mem.Write(",{0}", kv.Key);
                            sw_search_spd.Write(",{0}", kv.Key);
                        }
                        if (/*kv.Value.Item1 <= 0 || */kv.Value.Item2 <= 0)
                        {
                            use = false;
                            break;
                        }
                    }
                    if (firstWrite) {
                        sw_create_mem.WriteLine();
                        sw_create_spd.WriteLine();
                        sw_search_mem.WriteLine();
                        sw_search_spd.WriteLine();
                        firstWrite = false;
                    }
                        
                    if (!use)
                    {
                        negatives += 1;
                        Console.Write("\r{0}\t{1}", negatives, goodies);
                        i--;
                        // iterate again to use another dataset
                    }
                    else
                    {
                        goodies += 1;
                        Console.Write("\r{0}\t{1}", negatives, goodies);

                        sw_create_mem.Write("{0}", length);
                        sw_create_spd.Write("{0}", length);
                        foreach (KeyValuePair<string, Tuple<float, long>> kv in newTrees)
                        {
                            //sw_create_mem.Write("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1, kv.Value.Item2);
                            sw_create_mem.Write(",{0}", kv.Value.Item1);
                            sw_create_spd.Write(",{0}", kv.Value.Item2);
                        }
                        sw_create_mem.WriteLine();
                        sw_create_spd.WriteLine();

                        try
                        {
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in newTrees)
                            {
                                createMeans[kv.Key] = new Tuple<float, long>(
                                    createMeans[kv.Key].Item1 + (kv.Value.Item1 / create_iterations),
                                    createMeans[kv.Key].Item2 + (kv.Value.Item2 / create_iterations)
                                );
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in test.CreateTrees())
                            {
                                createMeans[kv.Key] = new Tuple<float, long>(
                                    kv.Value.Item1 / create_iterations,
                                    kv.Value.Item2 / create_iterations
                                );
                            }
                        }
                    }
                    // Do Search Tests
                    for (int j = 0; j < find_iterations && use; j++)
                    {
                        useFind = true;
                        var findResults = test.FindInTrees();
                        foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                        {
                            if (/*kv.Value.Item1 <= 0 || */kv.Value.Item2 <= 0)
                            {
                                useFind = false;
                                break;
                            }
                        }
                        if (!useFind)
                        {
                            negaFinds += 1;
                            Console.Write("\r{0}\t{1}\t{2}\t{3}", negatives, goodies, negaFinds, goodieFinds);
                            j--;
                            // iterate again to use another dataset
                        }
                        else
                        {
                            goodieFinds += 1;
                            Console.Write("\r{0}\t{1}\t{2}\t{3}", negatives, goodies, negaFinds, goodieFinds);

                            sw_search_mem.Write("{0}", length);
                            sw_search_spd.Write("{0}", length);
                            foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                            {
                                sw_search_mem.Write(",{0}", kv.Value.Item1);
                                sw_search_spd.Write(",{0}", kv.Value.Item2);
                            }
                            sw_search_mem.WriteLine();
                            sw_search_spd.WriteLine();

                            continue;
                            try
                            {
                                foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                                {
                                    findMeans[kv.Key] = new Tuple<float, long>(
                                        findMeans[kv.Key].Item1 + (kv.Value.Item1 / (find_iterations * create_iterations)),
                                        findMeans[kv.Key].Item2 + (kv.Value.Item2 / (find_iterations * create_iterations))
                                    );
                                }
                            }
                            catch (KeyNotFoundException)
                            {
                                foreach (KeyValuePair<string, Tuple<float, long>> kv in findResults)
                                {
                                    findMeans[kv.Key] = new Tuple<float, long>(
                                        kv.Value.Item1 / (find_iterations * create_iterations),
                                        kv.Value.Item2 / (find_iterations * create_iterations)
                                    );
                                }
                            }
                        }
                    }
                }
                /*foreach (KeyValuePair<string, Tuple<float, long>> kv in createMeans)
                {
                    sw_create.WriteLine("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1, kv.Value.Item2);
                }
                foreach (KeyValuePair<string, Tuple<float, long>> kv in findMeans)
                {
                    sw_find.WriteLine("{0},{1},{2},{3}", kv.Key, length, kv.Value.Item1, kv.Value.Item2);
                }*/

                sw_search_mem.WriteLine();
                sw_search_spd.WriteLine();
                sw_create_mem.WriteLine();
                sw_create_spd.WriteLine();

                sw_create_mem.Flush();
                sw_search_mem.Flush();
                sw_create_spd.Flush();
                sw_search_spd.Flush();
                Console.WriteLine();
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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Process proc = Process.GetCurrentProcess();
            float m = proc.PrivateMemorySize64;
            node = this._tree.Find(element);
            proc = Process.GetCurrentProcess();
            m = proc.PrivateMemorySize64 - m;
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, node != null, m);
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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Process proc = Process.GetCurrentProcess();
            float m = proc.PrivateMemorySize64;
            b = this._tree.Contains(new KeyValuePair<TData, int>(element, 0));
            proc = Process.GetCurrentProcess();
            m = proc.PrivateMemorySize64 - m;
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, m);
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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Process proc = Process.GetCurrentProcess();
            float m = proc.PrivateMemorySize64;
            b = this._tree.Contains(element);
            proc = Process.GetCurrentProcess();
            m = proc.PrivateMemorySize64 - m;
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, m);
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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Process proc = Process.GetCurrentProcess();
            float m = proc.PrivateMemorySize64;
            b = this._tree.Contains(element);
            proc = Process.GetCurrentProcess();
            m = proc.PrivateMemorySize64 - m;
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, m);
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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Process proc = Process.GetCurrentProcess();
            float m = proc.PrivateMemorySize64;
            b = this._tree.Contains(element);
            proc = Process.GetCurrentProcess();
            m = proc.PrivateMemorySize64 - m;
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, m);
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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Process proc = Process.GetCurrentProcess();
            float m = proc.PrivateMemorySize64;
            b = this._tree.Contains(element);
            proc = Process.GetCurrentProcess();
            m = proc.PrivateMemorySize64 - m;
            timer.Stop();
            return new Tuple<string, long, bool, float>(this.Name(), timer.Elapsed.Ticks, b, m);
        }
    }

}

