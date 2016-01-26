using System;
using System.IO;

namespace ETL
{
    public sealed class FileLog : ILog
    {
        private static object sync_root = new object();
        private string GetFileName()
        {
            string path_to_log = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            if (!Directory.Exists(path_to_log))
            {
                Directory.CreateDirectory(path_to_log);
            }

            return Path.Combine(path_to_log, string.Format("{0}_{1:dd.MM.yyyy}.log",
                AppDomain.CurrentDomain.FriendlyName,
                DateTime.Now));
        }
        private string CreateLogEntry(string log_message)
        {
            return string.Format("[{0:dd.MM.yyyy HH:mm:ss.fff}] {1}",
                DateTime.Now,
                log_message);
        }
        public void Write(Exception exception)
        {
            if (exception == null) return;
            Write(CreateLogEntry(string.Format("[{0}.{1}()] {2}\r\n{3}",
                exception.TargetSite.DeclaringType,
                exception.TargetSite.Name,
                exception.Message,
                exception.StackTrace)));
        }
        public void Write(string log_message)
        {
            try
            {
                lock (sync_root)
                {
                    using (StreamWriter file = new StreamWriter(GetFileName(), true))
                    {
                        file.WriteLine(CreateLogEntry(log_message));
                    }
                }
            }
            catch
            {
                // do nothing
            }
        }
    }
}
