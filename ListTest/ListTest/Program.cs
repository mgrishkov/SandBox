using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<object> mainList = new List<object>();
            mainList.Add(DateTime.Now);

            List<object> subList = new List<object>();
            subList.Add(mainList[0]);
            Console.WriteLine("mainList[0]: {0}; subList: {1}", mainList[0], subList[0]);

            subList[0] = DateTime.UtcNow;

            Console.WriteLine("mainList[0]: {0}; subList: {1}", mainList[0], subList[0]);
            Console.ReadKey();
        }
    }
}
