using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCHDAAutomation;

namespace OPCHDA
{
    class Program
    {
        private 

        static void Main(string[] args)
        {
            OPCHDAServer m_srv = new OPCHDAServer();
            m_srv.UseUTC = true;
            m_srv.Connect("Matrikon.OPC.Simulation.1");
            OPCHDAItem l_item = m_srv.OPCHDAItems.AddItem("Bucket Brigade.ArrayOfReal8", 1);
            object l_date_from = (object)Convert.ToDateTime("09.04.2012 12:34:32").ToUniversalTime();
            object l_date_to = (object)Convert.ToDateTime("09.04.2012 12:34:52").ToUniversalTime();
            OPCHDAHistory l_hist = l_item.ReadRaw(ref l_date_from, ref l_date_to);
            foreach (OPCHDAValue l_val in l_hist)
            {
                string l_msg = String.Format("{0} - {1} - {2}", l_val.TimeStamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"),
                    l_val.DataValue, l_val.Quality.ToString());
                Console.WriteLine(l_msg);
            };

            Console.ReadKey();
            m_srv.Disconnect();
        }

    }
}
