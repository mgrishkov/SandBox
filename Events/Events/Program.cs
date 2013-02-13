using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            Dummy d = new Dummy();
            d.ValRead += d_ValRead;
            d.ValChanged += d_ValChanged;
            int val = d.Val;
            d.Val = 10;
            Console.ReadKey();

        }

        static void d_ValChanged(object sender, Dummy.DummyEventArgs e)
        {
            Console.WriteLine(e.Operation);
        }

        static void d_ValRead(object sender, EventArgs e)
        {
            Console.WriteLine("READ");
        }
    }
}
