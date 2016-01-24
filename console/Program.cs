using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ETL;
using System.Diagnostics;
using System.Data.SqlClient;

namespace SSSETL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Data exchange is running ...");

            string source  = ConfigurationManager.AppSettings["source"];;
            string target  = ConfigurationManager.ConnectionStrings["target"].ConnectionString;
            string version = ConfigurationManager.AppSettings["version"];

            try
            {
                Stopwatch timer = Stopwatch.StartNew();
                DoWork(source, target, version);
                timer.Stop();
                Console.WriteLine("Data exchange has been done successfully.");
                Console.WriteLine(string.Format("Time used: {0} seconds.",
                    TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds).TotalSeconds));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(false);
        }
        private static void DoWork(string source, string target, string version)
        {
            using (ComConnector connector = new ComConnector(version, source))
            {
                connector.Connect();
                using (SqlConnection connection = new SqlConnection(target))
                {
                    connection.Open();
                    ExchangeSession session = new ExchangeSession(connection);
                    session.Enlist(new Catalog_InternalDocument_Adapter(connector), new Catalog_InternalDocument_Translator());
                    session.Enlist(new Catalog_Files_Adapter(connector), new Catalog_Files_Translator());
                    session.DoExchange();
                }
            }
        }
    }
}