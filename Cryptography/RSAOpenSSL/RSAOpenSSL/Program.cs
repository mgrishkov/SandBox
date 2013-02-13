using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSSL.Core;
using OpenSSL.Crypto;
using OpenSSL.X509;

namespace RSAOpenSSL
{
    class Program
    {
        static void Main(string[] args)
        {
            string l_data = "Encode ME!";
            Console.WriteLine(l_data);

            byte[] l_ebdata = EncodeRSA(l_data);
            string l_edata = BytesToStr(l_ebdata);
            Console.WriteLine(l_edata);

            string l_ddata = DecodeRSA(l_ebdata);
            Console.WriteLine(l_ddata);

            Console.ReadKey();
        }

        public static byte[] EncodeRSA(string Data)
        {
            byte[] l_res = null;
            using(RSA l_rsa = RSA.FromPrivateKey(BIO.File("D:\\Work\\Other\\ForexStars\\_incomming\\project1074.ppk", "r")))
            {
                byte[] l_bdata = StrToBytes(Data);
                l_res = l_rsa.PrivateEncrypt(l_bdata, RSA.Padding.PKCS1);
            };
            return l_res;
        }
        public static string DecodeRSA(byte[] Data)
        {
            string l_res = null;
            using (RSA l_rsa = RSA.FromPrivateKey(BIO.File("D:\\Work\\Other\\ForexStars\\_incomming\\project1074.ppk", "r")))
            {
                byte[] l_bdata = l_rsa.PublicDecrypt(Data, RSA.Padding.PKCS1);
                l_res = BytesToStr(l_bdata);
            };
            return l_res;
        }
        private static byte[] StrToBytes(string Data)
        {
            UnicodeEncoding l_conv = new UnicodeEncoding();
            return l_conv.GetBytes(Data);
        }
        private static string BytesToStr(byte[] Data)
        {
            UnicodeEncoding l_conv = new UnicodeEncoding();
            return l_conv.GetString(Data);
        }
    }
}
