using System;

namespace ETL
{
    public interface ILog
    {
        void Write(string message);
        void Write(Exception ex);
    }
}
