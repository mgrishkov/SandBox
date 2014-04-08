using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    public class Settings
    {
        public static string PacksFolder { get; internal set; }

        public static string ConnectionString { get; internal set; }
        public static string ServiceSchema { get; internal set; }

    }
}
