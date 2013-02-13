using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvokePHP.Classes;

namespace InvokePHP
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string l_text = "qwerty~123";

            Console.WriteLine(COpenSSLPHP.Sign(l_text));
            Console.WriteLine();
            Console.WriteLine(COpenSSLPHP.Encrypt(l_text));
            Console.ReadKey();
        }


        
    }
}
