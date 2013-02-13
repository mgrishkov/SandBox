using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;



namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
                        
            /*
            string l_public = RSA.ToXmlString(true);
            File.WriteAllText("public.xml", l_public);
            string l_private = RSA.ToXmlString(false);
            File.WriteAllText("private.xml", l_private);
            */

            //byte[]  encryptedData = RSAEncrypt(dataToEncrypt);
            //Console.WriteLine(ByteConverter.GetString(encryptedData));
            /*
            byte[] decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);
            Console.WriteLine(ByteConverter.GetString(decryptedData));
            */
            string l_data = @"<Request><Project>1074</Project><DateTime>2012-04-25T20:00:15</DateTime><Sign></Sign><Balance/></Request>";
            RSAChilat(l_data);


            Console.ReadKey();
        }

        static void RSAChilat(string Data)
        {
            Chilkat.PrivateKey l_key = new Chilkat.PrivateKey();
            bool l_res = l_key.LoadPemFile(@"D:\Work\Other\ForexStars\_incomming\project1074.ppk");
            string l_pr_key_xml = l_key.GetXml();
            
            
            Chilkat.Rsa rsa = new Chilkat.Rsa();
            rsa.ImportPrivateKey(l_pr_key_xml);
            //rsa.UnlockComponent("30-day trial");
            rsa.UnlockComponent("RSA$TEAM$BEAN_495C86FD5RkU");
            rsa.EncodingMode = "hex";
            rsa.LittleEndian = false;
            string l_sign = rsa.SignStringENC(Data, "sha-1");
            
            
        }

        static public byte[] RSAEncrypt(byte[] DataToEncrypt)
        {
            byte[] l_res = null;
            try
            {
                   
                //Import the RSA Key information. This needs
                //to include the public key information.
                X509Certificate2 l_cert = new X509Certificate2(@"D:\Work\Other\ForexStars\_incomming\project1074.ppk");
                RSACryptoServiceProvider RSA = l_cert.PublicKey.Key as RSACryptoServiceProvider;
                l_res = RSA.Encrypt(DataToEncrypt, false);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
            };
            return l_res;
        }
        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] l_res = null;
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);
                l_res = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
            }
            return l_res;

        }

        public static void SaveInContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = ContainerName;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            // Display the key information to the console.
            Console.WriteLine("Key added to container: \n  {0}", rsa.ToXmlString(true));
        }
        public static void DeleteKeyFromContainer(string ContainerName)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = ContainerName;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

            // Delete the key entry in the container.
            rsa.PersistKeyInCsp = false;

            // Call Clear to release resources and delete the key from the container.
            rsa.Clear();

            Console.WriteLine("Key deleted.");
        }

    }
}
