using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateCBR.ru.cbr.www;
using System.Data;

namespace ExchangeRateCBR
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal l_res = GetExchangeRate("USD");

            Console.WriteLine(l_res.ToString());
            
            Console.ReadKey();
        }

        public static decimal GetExchangeRate(string CurrancyCode)
        {
            DailyInfo l_info = new DailyInfo();
            DataSet l_data = l_info.GetCursOnDate(DateTime.Now.Date);
            DataTable l_tbl = l_data.Tables["ValuteCursOnDate"];
            decimal l_res = (from row in l_tbl.AsEnumerable()
                             where row.Field<string>("VchCode") == CurrancyCode
                             select row.Field<decimal>("Vcurs") / row.Field<decimal>("Vnom")).First<decimal>();
            return l_res;
        }
    }
}
