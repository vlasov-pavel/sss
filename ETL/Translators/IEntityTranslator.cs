using System;
using System.Data.SqlClient;

namespace ETL
{
    public interface IEntityTranslator
    {
        void Translate(IComWrapper com_object, SqlCommand sql_command);
    }
}
