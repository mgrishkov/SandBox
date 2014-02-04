using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ParseWebPage
{
    class Program
    {
        static void Main(string[] args)
        {
            var wmid = 0m;
            var web = new HtmlWeb();
            var page = web.Load(@"http://passport.webmoney.ru/asp/VerifyWMID.asp?wmid=Z705923888994");
            var elements = page.DocumentNode.SelectNodes("//div[@class=\"insideMainContent\"]/table/tr/td[@class=\"message1L\"]");

            var wmidPattern = @"WMID\d{12}";
            var element = elements.FirstOrDefault(x => Regex.IsMatch(x.InnerText, wmidPattern));
            if(element != null)
            {
                var wmidMatch = Regex.Match(element.InnerHtml, wmidPattern);
                if(wmidMatch.Success)
                {
                    Decimal.TryParse(wmidMatch.Value.Substring(4, 12), out wmid);
                }
                Console.WriteLine(wmid);
            }
            Console.ReadKey();
        }
    }
}
