using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Hier kunnen we varieren met type zoekboom
            DancingTree tree = new DancingTree();
            //Console input (bijv: "a 3" als add een 3 aan de zoekboom)
            string[] command;
            string text;
            text = Console.ReadLine();
            command = text.Split();
            //Soort command dat is gegeven in de console
            switch (command[0])
            {
                case "f":
                    tree.find(command[1]);
                    break;
                case "a":
                    tree.add(command[1]);
                    break;
                case "d":
                    tree.delete(command[1]);
                    break;
            }
        }
    }

}
