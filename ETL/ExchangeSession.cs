using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ETL
{
    public sealed class ExchangeSession
    {
        private int session_id = -1;
        private readonly SqlConnection connection;
        private const string CONST_OpenSessionProcedure = "Pstart_NewSea_BezDt";
        private const string CONST_CloseSessionProcedure = "Pclose_SeaBezDt";
        private Dictionary<IEntityAdapter, IEntityTranslator> exchange_list = new Dictionary<IEntityAdapter, IEntityTranslator>();
        public ExchangeSession(SqlConnection connection)
        {
            this.connection = connection;
        }
        private void AddParameter(SqlCommand command, string name, SqlDbType type, ParameterDirection direction, object value)
        {
            command.Parameters.Add(
                new SqlParameter()
                {
                    ParameterName = name,
                    SqlDbType     = type,
                    Direction     = direction,
                    Value         = value
                });
        }
        private int Open()
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = CONST_OpenSessionProcedure;
                AddParameter(command, "pr",  SqlDbType.VarChar, ParameterDirection.Input, "Мониторинг");
                AddParameter(command, "vk",  SqlDbType.VarChar, ParameterDirection.Input, "Договора");
                AddParameter(command, "sea", SqlDbType.Int,     ParameterDirection.Output, null);
                command.ExecuteNonQuery();
                session_id = (int)command.Parameters["sea"].Value;
            }
            return session_id;
        }
        private void Close()
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = CONST_CloseSessionProcedure;
                AddParameter(command, "sea", SqlDbType.Int, ParameterDirection.Input, session_id);
                command.ExecuteNonQuery();
            }
        }
        public void Enlist(IEntityAdapter adapter, IEntityTranslator translator)
        {
            exchange_list.Add(adapter, translator);
        }
        public void DoExchange()
        {
            Open();
            foreach (KeyValuePair<IEntityAdapter, IEntityTranslator> item in exchange_list)
            {
                Exchange(item.Key, item.Value);
            }
            Close();
        }
        private void Exchange(IEntityAdapter adapter, IEntityTranslator translator)
        {
            using (IComWrapper cursor = adapter.GetCursor())
            {
                if (cursor != null)
                {
                    while ((bool)cursor.Call("Следующий"))
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            translator.Translate(cursor, command);
                            AddParameter(command, "id_sea", SqlDbType.Int, ParameterDirection.Input, session_id);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            Console.WriteLine(string.Format("Импорт завершён успешно: {0}", adapter.ToString()));
        }
    }
}
