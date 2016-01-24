using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ETL
{
    public sealed class Catalog_Files_Translator : TranslatorBase
    {
        public Catalog_Files_Translator() : base() { }
        protected override void CreateTranslationRules()
        {
            _Rules = new List<TranslationRule>()
            {
                new TranslationRule("code",     SqlDbType.VarChar, SqlDbType.VarChar),
                new TranslationRule("fnm",      SqlDbType.VarChar, SqlDbType.VarChar),
                new TranslationRule("dog_code", SqlDbType.VarChar, SqlDbType.VarChar)
            };
        }
        protected override void SetupInsertCommandText()
        {
            _InsertCommand = "INSERT [TPrSrv_DgwFiles_Stack] "
            + "(id_sea, code, fnm, dog_code) "
            + "VALUES "
            + "(@id_sea, @code, @fnm, @dog_code);";
        }
        public override void Translate(IComWrapper com_object, SqlCommand command)
        {
            command.CommandText = _InsertCommand;
            command.CommandType = CommandType.Text;
            if (command.Parameters.Count > 0) command.Parameters.Clear();
            foreach (TranslationRule rule in _Rules)
            {
                AddParameter(command, rule.Field, rule.TargetType, GetValue(rule, com_object));
            }
        }
    }
}
