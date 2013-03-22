using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.Data;

namespace LinqAndDataTable
{
    class Program
    {
        public class Dummy
        {
            public string colA { get; set; }

            public Dummy(DataRow dr)
            {
                colA = (string)dr["colA"];
            }
        }
        public class DummyList : BindingList<Dummy>
        {
            private object _locker = new object();

            public void AsyncFill(DataTable dt)
            {
                try
                {
                    Monitor.Enter(_locker);
                    Parallel.ForEach(dt.AsEnumerable(), x => this.Add(new Dummy(x)));
                }
                finally
                {
                    Monitor.Exit(_locker);
                };
            }

        }
        static void Main(string[] args)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("colA", typeof(String)));
            dt.Columns.Add(new DataColumn("colB", typeof(String)));

            for(int i = 0; i < 10; i++)
            {
                DataRow row = dt.NewRow();
                row["colA"] = String.Format("{0} Atest", i);
                row["colB"] = String.Format("{0} Btest", i);
                dt.Rows.Add(row); 
            };

            var dl = new DummyList();
            dl.AsyncFill(dt);

            foreach (var itm in dl)
            {
                Console.WriteLine(itm.colA);
            };
            Console.ReadKey();
        }
    }
}
