using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace InvokePHP.Classes
{
    public static class COpenSSLPHP
    {
        private static string GetPHPPath()
        {
            string l_res = String.Format(@"""{0}\PHP\php.exe""", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            return l_res;
        }
        public static string Sign(string Data)
        {
            string l_res = String.Empty;
            string l_sign_file = Directory.GetCurrentDirectory() + @"\PHP\sign.php";
            l_res = InvokeProcess(l_sign_file, Data);
            return l_res;
        }
        public static string Encrypt(string Data)
        {
            string l_res = String.Empty;
            string l_sign_file = Directory.GetCurrentDirectory() + @"\PHP\encrypt.php";
            l_res = InvokeProcess(l_sign_file, Data);
            return l_res;
        }
        private static string InvokeProcess(string PHPFile, string Data)
        {
            string l_res = String.Empty;
            using (Process l_prc = new Process())
            {
                ProcessStartInfo l_st_info = new ProcessStartInfo(GetPHPPath(), "spawn")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    Arguments = string.Format("{0} {1} {2}", "-f", PHPFile, Data)
                };
                l_prc.StartInfo = l_st_info;
                l_prc.Start();
                StreamReader l_st_output = l_prc.StandardOutput;
                l_res = l_st_output.ReadToEnd();
            };
            return l_res;
        }
    }

}
