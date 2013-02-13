using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace MongoDB
{
    class Program
    {
        public class Book
        {
            public ObjectId _id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public DateTime PublishDate { get; set; }
            public string Bublisher { get; set; }
            public int Totalpages { get; set; }
            public string Note { get; set; }
            public object books { get; set; }

        }
        static void Main(string[] args)
        {
            var connectionString = "mongodb://localhost/?safe=true";
            var server = MongoServer.Create(connectionString);
            var database = server.GetDatabase("test");
            var collection = database.GetCollection<Book>("books");

            var newBook = new Book()
            {
                Author = "Tester",
                PublishDate = DateTime.Now,
                Bublisher = "Teeester",
                Note = "Sth"
            };

            collection.Save(newBook);

            var data = from e in collection.AsQueryable<Book>()
                       select e;
            data.ToList().ForEach(x => Console.WriteLine(x.Bublisher));
            Console.ReadKey();
        }
    }
}
