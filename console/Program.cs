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
            ILog log = new FileLog();
            log.Write("start import");

            string source  = ConfigurationManager.AppSettings["source"];;
            string target  = ConfigurationManager.ConnectionStrings["target"].ConnectionString;
            string version = ConfigurationManager.AppSettings["version"];

            Stopwatch timer = null;
            try
            {
                timer = Stopwatch.StartNew();
                DoWork(source, target, version);
            }
            catch (Exception ex)
            {
                log.Write(ex);
            }
            finally
            {
                if (timer != null) timer.Stop();
                log.Write(string.Format("time used = {0} seconds",
                    TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds).TotalSeconds));
                log.Write("stop import");
            }
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