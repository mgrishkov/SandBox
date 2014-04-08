using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    public class DBUpdater
    {
        private List<Pack> _packs;

        public Settings Settings { get; private set; }

        public IEnumerable<Pack> Packs
        {
            get { return _packs; }
        }
        public Version OldVersion { get; private set; }
        public Version NewVersion { get; private set; }
        public StringBuilder Log { get; private set; }

        public DBUpdater(string connectionString,
                         string serviceSchema = "APP",
                         string packFolder = "Packs")
        {
            Log = new StringBuilder();

            Settings.ConnectionString = connectionString;
            Settings.ServiceSchema = serviceSchema;
            Settings.PacksFolder = packFolder;

            CreateSchema();
            GetCurrentVersion();
            LoadPacks();
        }

        public void InstallUpdates()
        {
            var toInstall = Packs
                .Where(x => x.Version > OldVersion)
                .OrderBy(x => x.Version);

            foreach(var itm in toInstall)
            {
                Log.AppendFormat("Installing pack {0}...{1}", itm.Version, Environment.NewLine);
                itm.Load();
                itm.Install();

                Log.Append(itm.Log);
                
                Log.AppendFormat("Oack {0} has been installed{1}{1}", itm.Version, Environment.NewLine);
            };
        }

        private void LoadPacks()
        {
            var files = Directory.GetFiles(Settings.PacksFolder)
                .Where(x => x.EndsWith(".sql"));
            foreach (var itm in files)
            {
                var file = new FileInfo(itm);
                Version version = null;

                if(Version.TryParse(file.Name, out version))
                {
                    var pack = new Pack()
                    {
                        Version = version
                    };
                    _packs.Add(pack);
                };
            };

            NewVersion = _packs.Max(x => x.Version);
        }

        private void CreateSchema()
        {
            var script = String.Format(
                @"if(schema_id('[{0}]') is null) 
                    exec ('create schema [{0}]')
                go

                if(object_id('[{0}].[DBVersion]') is null)
                    create table [{0}].[DBVersion]
                    ( 
                        [ID] int identity,
                        [Major] int not null, 
                        [Minor] int not null,
                        [Build] int not null,
                        [Revision] int not null,
                        [InstallationStarted] datetime2 not null,
                        [InstallationFinished] datetime2,
                        constraint PK#DBVersion primary key ([ID]),
                        constraint UK#DBVersion unique ([Major], [Minor], [Build], [Revision])
                    );
                go", 
                Settings.ServiceSchema);

            using (var conn = new SqlConnection(Settings.ConnectionString))
            using (var cmd = new SqlCommand(script, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            };
        }

        private void GetCurrentVersion()
        {
            var script = String.Format("select top 1 [Major], [Minor], [Build], [Revision] from [{0}].[DBVersion] order by [Major] desc, [Minor] desc, [Build] desc [Revision] desc", Settings.ServiceSchema);
            using (var conn = new SqlConnection(Settings.ConnectionString))
            using (var cmd = new SqlCommand(script, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int major = reader.GetFieldValue<int>(0);
                        int minor = reader.GetFieldValue<int>(1);
                        int build = reader.GetFieldValue<int>(2);
                        int revision = reader.GetFieldValue<int>(3);

                        OldVersion = new Version(major, minor, build, revision);
                    }
                    else
                    {
                        OldVersion = new Version();
                    };
                };
            };
        }

    }
}
