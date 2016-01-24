using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ETL
{
    public abstract class TranslatorBase : IEntityTranslator
    {
        protected readonly DateTime CONST_min_date = new DateTime(2000, 1, 1);
        protected List<TranslationRule> _Rules;
        protected string _InsertCommand;
        protected TranslatorBase()
        {
            CreateTranslationRules();
            SetupInsertCommandText();
        }
        protected abstract void CreateTranslationRules();
        protected abstract void SetupInsertCommandText();
        public abstract void Translate(IComWrapper com_object, SqlCommand sql_command);
        protected object GetValue(TranslationRule rule, IComWrapper com_object)
        {
            object value = com_object.Get(rule.Field);
            if (value == DBNull.Value) return value;
            if (rule.SourceType == SqlDbType.VarChar)
            {
                return (string)value;
            }
            else if (rule.SourceType == SqlDbType.Int)
            {
                return (int)value;
            }
            else if (rule.SourceType == SqlDbType.DateTime)
            {
                DateTime test = (DateTime)value;
                return test < CONST_min_date ? CONST_min_date : test;
            }
            else if (rule.SourceType == SqlDbType.Money)
            {
                return Convert.ToDecimal(value);
            }
            else if (rule.SourceType == SqlDbType.Bit && rule.TargetType == SqlDbType.Int)
            {
                return (bool)value ? 1 : 0;
            }
            return value;
        }
        protected void AddParameter(SqlCommand command, string name, SqlDbType type, object value)
        {
            command.Parameters.Add(
                new SqlParameter()
                {
                    ParameterName = name,
                    SqlDbType     = type,
                    Direction     = ParameterDirection.Input,
                    Value         = value
                });
        }
    }
}
