using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace LocalDB.Models
{
    public class DocumentDataContext : DataContext
    {
        public static string _dbConnectionString = "Data Source=LocalDB.sdf";

        public DocumentDataContext()
            : base(_dbConnectionString)
        { }
        public DocumentDataContext(string connectionString)
            : base(_dbConnectionString)
        { }

        // Specify a single table for the to-do items.
        public Table<Document> Documents;
    }
}
