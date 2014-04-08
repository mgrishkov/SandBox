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
        public string FileName
        {
            get
            {
                return String.Format("{0}.sql", Version);
            }
        }

        public Pack()
        {
            Log = new StringBuilder();
        }

        public void Load()
        {           

            var fileName = Directory.GetFiles(Settings.PacksFolder)
                .Where(x => x.EndsWith(FileName))
                .FirstOrDefault();

            if(String.IsNullOrWhiteSpace(fileName))
            {
                using (var mmf = MemoryMappedFile.OpenExisting(fileName))
                using (var stream = mmf.CreateViewStream())
                using (var binReader = new BinaryReader(stream))
                {
                    var content = binReader.ReadBytes((int)stream.Length);
                    Script = Encoding.UTF8.GetString(content);
                };   
            }
            else
            {
                throw new FileNotFoundException(String.Format("Pack for version {0} has not been found.", Version), String.Format("{0}.sql", Version));
            };
        }
        
        public void Install()
        {
            if (String.IsNullOrWhiteSpace(Settings.ConnectionString))
                throw new ArgumentException("Settings.Connectionstring is Empty");
           
            if(String.IsNullOrWhiteSpace(Script))
                throw new ArgumentException("Pack has not been loaded yet");

            using (var conn = new SqlConnection(Settings.ConnectionString))
            using (var cmd = new SqlCommand(Script, conn))
            {
                conn.Open();
                conn.InfoMessage += readInfoMessage;
                cmd.ExecuteNonQuery();
            }            
        }

        private void readInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Log.AppendFormat("{0:dd.MM.yyyy HH:mm:ss.ms} - Info {1}{2}", DateTime.Now, e.Message, Environment.NewLine);
            //Log.AppendFormat("{0:dd.MM.yyyy HH:mm:ss.ms} - Error Source: {1}, Errors: {2}", DateTime.Now, e.Source, e.Errors);
        }
    }
}
