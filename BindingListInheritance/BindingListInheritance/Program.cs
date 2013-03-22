using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BindingListInheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new DummyList();
            list.Fill();
            foreach (var itm in list)
            {
                Console.WriteLine(itm.SomeProperty);
            };
            Console.ReadKey();
        }
    }
}
