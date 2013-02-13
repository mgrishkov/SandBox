using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;


namespace MD5Hash
{
    class Program
    {
        static void Main(string[] args)
        {
            //string l_text = "A123456:12328:Qwe!23:100:USD";
            double l_amount = Double.TryParse(",01", out l_amount) ? Convert.ToDouble(",01") : 0;

            Console.WriteLine((int)(l_amount * 100));

            string l_text = String.Format("{0}:{1}:{2}:{3}:{4}",
                    509, 390886, "ZP6qvYip", (int)(l_amount * 100), "USD");
            byte[] l_input = Encoding.UTF8.GetBytes(l_text);
            byte[] l_output = MD5.Create().ComputeHash(l_input);
            
            string l_res = Convert.ToBase64String(l_output);
            Console.WriteLine("Base64: {0}", l_res);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < l_output.Length; i++)
            {
                sBuilder.Append(l_output[i].ToString("x2"));
            }
            l_res = sBuilder.ToString();
            Console.WriteLine("StringBulder: {0}", l_res);
            Console.ReadKey();
        }
    }
}
