using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartClasses.Hierarchy;

namespace Hierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new HierarchyList<string, string>("R", "ROOT");
            Node<string, string> b1 = list.Root.AddNode("B1", "Branch1");
            b1.AddNode("L1-1", "Leaf1-1");
            b1.AddNode("L1-2", "Leaf1-2");
            b1.AddNode("L1-3", "Leaf1-3");

            Node<string, string> b2 = list.Root.AddNode("B2", "Branch2");
            Node<string, string> b21 = b2.AddNode("B2-1", "Branch2-1");
            b21.AddNode("L2-1-1", "Leaf2-1-1");
            b21.AddNode("L2-1-2", "Leaf2-1-2");
            b21.AddNode("L2-1-3", "Leaf2-1-3");
            Node<string, string> b22 = b2.AddNode("B2-2", "Branch2-2");
            b22.AddNode("L2-2-1", "Leaf2-2-1");
            b22.AddNode("L2-2-2", "Leaf2-2-2");
            b22.AddNode("L2-2-3", "Leaf2-2-3");
            Node<string, string> b23 = b2.AddNode("B2-3", "Branch2-3");
            b23.AddNode("L2-3-1", "Leaf2-3-1");
            b23.AddNode("L2-3-2", "Leaf2-3-2");
            b23.AddNode("L2-3-3", "Leaf2-3-3");

            Console.WriteLine(list.Root.ToString());

            Console.ReadKey();                        
        }
    }
}
