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
		public Test(T[] data, string fname){
            Random random = new Random();
			StreamWriter sw;
			if(File.Exists(fname)){
				sw = File.AppendText(fname);
			}
			else{
				sw = new StreamWriter(fname);
			}
            sw.WriteLine();
            sw.WriteLine(DateTime.Now.ToString());
			float delta = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
			GC.WaitForFullGCComplete();
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> rb = new RedBlack<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
            sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", rb.Name(), delta, timer.ElapsedTicks);
            
            timer.Restart();
			GC.WaitForFullGCComplete();
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> b = new BTree<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
            sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", b.Name(), delta, timer.ElapsedTicks);
            
            timer.Restart();
			GC.WaitForFullGCComplete();
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> bplus = new BPlusTree<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
            sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", bplus.Name(), delta, timer.ElapsedTicks);

            timer.Restart();
			GC.WaitForFullGCComplete();
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> avl = new AvlTree<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
            sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", avl.Name(), delta, timer.ElapsedTicks);

            timer.Restart();
            GC.WaitForFullGCComplete();
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> sg = new ScapeGoat<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
            sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", sg.Name(), delta, timer.ElapsedTicks);

            timer.Restart();
            GC.WaitForFullGCComplete();
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> bh = new BinHeap<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
            sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Create", bh.Name(), delta, timer.ElapsedTicks);
            timer.Stop();
			ISearchTree<T>[] trees = new ISearchTree<T>[]{
				rb, b, bplus, avl, sg, bh
			};

            int i;
            //for (int a = 0; a < 10; a++)
            //{
                i = random.Next(100000);
                foreach (ISearchTree<T> tree in trees)
                {
                    Tuple<string, long, bool, float> result = tree.Find(data[i]);
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Find", result.Item1, result.Item4, result.Item2);
                }
            //}
			sw.Close();

		}
	}
}
