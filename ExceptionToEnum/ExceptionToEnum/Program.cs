using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace ExceptionToEnum
{
    public enum Firstexception { FirstError = 101001, SecondError = 101002 }
    public enum Secondtexception { FirstError = 102001, SecondError = 102002 }

    public static class ExceptionExtension
    {
        public static T Parse<T>(this Exception e) where T : struct, IConvertible
        {
            T result = default(T);
            string message = e.Message;
            var error = Regex.Match(message, "[0-9]{6}");
            if (error.Success)
            {
                result = (T)Enum.Parse(typeof(T), error.Value);
            };
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                throw new Exception("101001 TestMessage");
            }
            catch (Exception e)
            {
                var result = e.Parse<Firstexception>();
                Console.WriteLine(result);
            }
            Console.ReadKey();
        }
    }
}
