using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneObject
{
    public class Program
    {
        public class C1 : ICloneable
        {
            public C1 Clone()
            {
                return (C1)this.MemberwiseClone();
            }
            object ICloneable.Clone()
            {
                return Clone();
            }

            public string Data;
            public C1()
            {
                Data = String.Empty;
            }
        }
        
        public static void Main(string[] args)
        {
            var l_c = new C1();
            l_c.Data = "Test";
            Console.ReadKey();
            Console.WriteLine(l_c.Data);
            Console.ReadKey();

            var l_c1 = l_c.Clone();
            l_c1.Data = "New Test";
            Console.WriteLine("original = {0}; coppied = {1}", l_c.Data, l_c1.Data);
            Console.ReadKey();
        }
    }
}
