using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics; //tijdelijk


namespace onderzoek
{
	class Test<T> where T : IComparable, IComparable<T>
	{
        Random _rnd;
        T[] data;
        ISearchTree<T>[] trees;
		public Test(T[] data){
            this.data = data;
            this._rnd = new Random();
		}


        public Dictionary<string, Tuple<float, long>> CreateTrees(){
            Dictionary<String, Tuple<float, long>> creationData = new Dictionary<string, Tuple<float, long>>();
            float delta = 0;
            Process proc;
            Stopwatch timer = new Stopwatch();
            GC.WaitForFullGCComplete();
            timer.Start();
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64;
            ISearchTree<T> rb = new RedBlack<T>(this.data);
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64 - delta;
            //sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", rb.Name(), delta, timer.ElapsedTicks);
            creationData.Add(rb.Name(), new Tuple<float, long>(delta, timer.ElapsedTicks));


            GC.WaitForFullGCComplete();
            timer.Restart();
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64;
            ISearchTree<T> b = new BTree<T>(this.data);
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64 - delta;
            //sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", b.Name(), delta, timer.ElapsedTicks);
            creationData.Add(b.Name(), new Tuple<float, long>(delta, timer.ElapsedTicks));

            GC.WaitForFullGCComplete();
            timer.Restart();
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64;
            ISearchTree<T> bplus = new BPlusTree<T>(this.data);
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64 - delta;
            //sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", bplus.Name(), delta, timer.ElapsedTicks);
            creationData.Add(bplus.Name(), new Tuple<float, long>(delta, timer.ElapsedTicks));

            timer.Restart();
            GC.WaitForFullGCComplete();
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64;
            ISearchTree<T> avl = new AvlTree<T>(this.data);
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64 - delta;
            //sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", avl.Name(), delta, timer.ElapsedTicks);
            creationData.Add(avl.Name(), new Tuple<float, long>(delta, timer.ElapsedTicks));

            GC.WaitForFullGCComplete();
            timer.Restart();
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64;
            ISearchTree<T> sg = new ScapeGoat<T>(this.data);
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64 - delta;
            //sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", sg.Name(), delta, timer.ElapsedTicks);
            creationData.Add(sg.Name(), new Tuple<float, long>(delta, timer.ElapsedTicks));

            GC.WaitForFullGCComplete();
            timer.Restart();
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64;
            ISearchTree<T> bh = new BinHeap<T>(this.data);
            proc = Process.GetCurrentProcess();
            delta = proc.PrivateMemorySize64 - delta;
            //sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", bh.Name(), delta, timer.ElapsedTicks);
            creationData.Add(bh.Name(), new Tuple<float, long>(delta, timer.ElapsedTicks));

            timer.Stop();
            this.trees = new ISearchTree<T>[]{
				rb, b, bplus, avl, sg, bh
			};
            return creationData;
        }

        public Dictionary<string, Tuple<float, long>> FindInTrees(){
            Dictionary<String, Tuple<float, long>> searchData = new Dictionary<string, Tuple<float, long>>();
            int i = this._rnd.Next(this.data.Length);
            foreach (ISearchTree<T> tree in this.trees)
            {
                Tuple<string, long, bool, float> result = tree.Find(this.data[i]);
                    
                searchData.Add(result.Item1, new Tuple<float, long>(result.Item4, result.Item2));
            }
            return searchData;
        }
	}
}
