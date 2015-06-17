using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting to {0}", args[0]);
            var result = MassaK.Providers.ScalesProvider.ReadWeight(args[0]);
            Console.WriteLine("Weight has been read, Result: {0}", result.IsOK);
            if (!result.IsOK)
            {
                Console.WriteLine(result.Message);
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Results {0} {1}", result.Reading, result.UnitOfMeasurement);
            Console.ReadKey();
        }
    }
}
