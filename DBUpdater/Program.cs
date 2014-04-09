using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=localhost; Database=CRM; Integrated Security=SSPI;";
            
            var updater = new DBUpdater(connectionString, "SERVICE", "Packs");
            
            updater.InstallUpdates();

            Console.WriteLine(updater.Log.ToString());
        }
    }
}
