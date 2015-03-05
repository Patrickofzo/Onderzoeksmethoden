using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace onderzoek
{
	class Test<T> where T : IComparable, IComparable<T>
	{
		public Test(T[] data, string fname){
			StreamWriter sw;
			if(File.Exists(fname)){
				sw = File.AppendText(fname);
			}
			else{
				sw = new StreamWriter(fname);
			}
			float delta = 0;
			delta = GC.GetTotalMemory(false);
			ISearchTree<T> rb = new RedBlack<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "RedBlack", delta);

			ISearchTree<T> b = new BTree<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "BTree", delta);

			ISearchTree<T> bplus = new BPlusTree<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "BPlus", delta);

			ISearchTree<T> avl = new AvlTree<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "AVL", delta);

			ISearchTree<T> sg = new ScapeGoat<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "ScapeGoat", delta);

			ISearchTree<T> bh = new BinHeap<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "BinHeap", delta);

			ISearchTree<T> beap = new Beap<T>(data);
			delta = GC.GetTotalMemory(false) - delta;
			sw.WriteLine("{0}\t{1}\t{2}", "Create", "Beap", delta);

			ISearchTree<T>[] trees = new ISearchTree<T>[]{
				rb, b, bplus, avl, sg, bh, beap
			};
			foreach (ISearchTree<T> tree in trees)
			{
				Tuple<string, int, bool, float> result = tree.Find(data[0]);
				sw.WriteLine("{0}\t{1}\t{2}\t{3}", "Find", result.Item1, result.Item4, result.Item1);
			}
			sw.Close();
			Console.ReadLine();

		}
	}
}
