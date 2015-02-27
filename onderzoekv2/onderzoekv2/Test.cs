using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace onderzoek
{
    class Test<T> where T : IComparable, IComparable<T>
    {
        public Test(T[] data){
            ISearchTree<T> rb = new RedBlack<T>(data);
            ISearchTree<T> b = new BTree<T>(data);
            ISearchTree<T> bplus = new BPlusTree<T>(data);
            ISearchTree<T> avl = new AvlTree<T>(data);
            ISearchTree<T> sg = new ScapeGoat<T>(data);
            ISearchTree<T> bh = new BinHeap<T>(data);
            ISearchTree<T> beap = new Beap<T>(data);
            ISearchTree<T>[] trees = new ISearchTree<T>[]{
                rb, b, bplus, avl, sg, bh, beap
            };
            foreach (ISearchTree<T> tree in trees)
            {
                Console.WriteLine(tree.Find(data[0]));
            }
            Console.ReadLine();

        }
    }
}
