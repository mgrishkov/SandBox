using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DBUpdater
{
    public class Pack
    {
        public Version Version { get; set; }
        public string Script { get; private set; }
        public StringBuilder Log { get; private set; }
        public FileInfo File { get; set; }

        public Pack()
        {
            Log = new StringBuilder();
        }

        public void Load()
        {
            using (var stream = File.OpenText())
            {
                Script = stream.ReadToEnd();
            };
        }
        
        public void Install()
        {
            if (String.IsNullOrWhiteSpace(Settings.ConnectionString))
                throw new ArgumentException("Settings.Connectionstring is Empty");
           
            if(String.IsNullOrWhiteSpace(Script))
                throw new ArgumentException("Pack has not been loaded yet");

            using (var conn = new SqlConnection(Settings.ConnectionString))
            {
                conn.Open();
                conn.InfoMessage += readInfoMessage;

                var commands = Script.Split(new string[] { "GO", "go", "Go", "gO" }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var sql in commands)
                {
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    };
                }
            }
            
        }

        private void readInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Log.AppendFormat("{0:dd.MM.yyyy HH:mm:ss.ms} - Info {1}{2}", DateTime.Now, e.Message, Environment.NewLine);
            //Log.AppendFormat("{0:dd.MM.yyyy HH:mm:ss.ms} - Error Source: {1}, Errors: {2}", DateTime.Now, e.Source, e.Errors);
        }
    }
}
