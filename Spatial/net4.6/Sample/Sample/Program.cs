using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        //Action required to load native assemblies
        //To deploy an application that uses spatial data types to a machine that does not have 'System CLR Types for SQL Server' installed you also need to deploy the native assembly SqlServerSpatial110.dll.Both x86 (32 bit) and x64(64 bit) versions of this assembly have been added to your project under the SqlServerTypes\x86 and SqlServerTypes\x64 subdirectories.The native assembly msvcr100.dll is also included in case the C++ runtime is not installed. 
        //
        //You need to add code to load the correct one of these assemblies at runtime (depending on the current architecture). 
        //
        //ASP.NET applications
        //For ASP.NET applications, add the following line of code to the Application_Start method in Global.asax.cs: 
        //
        //    SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));
        //
        //Desktop applications
        //For desktop applications, add the following line of code to run before any spatial operations are performed: 
        //
        //    SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
        //
        //


        //http://stackoverflow.com/a/40880753/966570
        //https://social.msdn.microsoft.com/Forums/sqlserver/en-US/9d2af509-2fd6-4daf-b3df-632c92a3b2e8/how-to-create-a-line-in-a-spatial-type-geometry-or-geography?referrer=http://social.msdn.microsoft.com/Forums/sqlserver/en-US/9d2af509-2fd6-4daf-b3df-632c92a3b2e8/how-to-create-a-line-in-a-spatial-type-geometry-or-geography?forum=sqlspatial
        
        //https://blogs.msdn.microsoft.com/davidlean/2008/10/30/sql-2008-spatial-samples-part-3-of-9-sql-builder-api/
        static void Main(string[] args)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            var data = LoadData().ToList();

            //build line
            var line = BuildLine(data);
            Console.WriteLine( $"Points: {(Int32)line.STNumPoints()}, distance: {(Double)line.STLength()}" );

            //simplify line
            var simplifiedLine = line.Reduce(8.0d); // total 24, 0.1 - 20, 0.4 - 19, 8.0 - 18
            Console.WriteLine($"Points: {(Int32)simplifiedLine.STNumPoints()}, distance: {(Double)simplifiedLine.STLength()}");

            Console.ReadKey();

        }

        private static SqlGeography BuildLine(List<SqlGeography> data)
        {
            var lineConstructor = new SqlGeographyBuilder();
            lineConstructor.SetSrid(4326);
            lineConstructor.BeginGeography(OpenGisGeographyType.LineString);
            lineConstructor.BeginFigure((double)data[0].Lat, (double)data[0].Long);
            for (var i = 1; i < data.Count; i++)
            {
                var point = data[i];
                lineConstructor.AddLine((double)point.Lat, (double)point.Long);
            }
            lineConstructor.EndFigure();
            lineConstructor.EndGeography();

            var resultLine = lineConstructor.ConstructedGeography;

            return resultLine.MakeValid();
        }

        public static IEnumerable<SqlGeography> LoadData()
        {
            using (var conn = new SqlConnection("Server=localhost;Database=Spatial;Integrated Security=True;"))
            using (var cmd = new SqlCommand("select Point from dbo.SpatialData order by [Timestamp]", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //data.Add(SqlGeography.Deserialize((SqlBytes)reader["Point"])); -- not works
                        yield return SqlGeography.Deserialize(reader.GetSqlBytes(0));
                    }
                    reader.Close();
                }
                conn.Close();
            }
        }
    }
}
