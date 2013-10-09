using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalDB.Models;

namespace LocalDB
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dc = new DocumentDataContext())
            {
                if (!dc.DatabaseExists())
                {
                    dc.CreateDatabase();
                };
                if (!dc.Documents.Any(x => x.Title == "Test"))
                {
                    var doc = new Document()
                    {
                        Title = "Test",
                        Author = "TestUser",
                        Body = "zxczczczcz",
                        CreationTime = DateTime.Now
                    };
                    dc.Documents.InsertOnSubmit(doc);
                    dc.SubmitChanges();
                }
                else
                {
                    var test = dc.Documents.Single(x => x.Title == "Test");
                    Console.WriteLine(test.Body);
                };
            }
            Console.ReadKey();
        }
    }
}
