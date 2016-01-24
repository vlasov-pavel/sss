using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ETL
{
    public sealed class Catalog_InternalDocument_Translator : TranslatorBase
    {
        public Catalog_InternalDocument_Translator() : base() { }
        protected override void CreateTranslationRules()
        {
            _Rules = new List<TranslationRule>()
            {
                new TranslationRule("code",        SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("regnom",      SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("dt",          SqlDbType.DateTime, SqlDbType.DateTime),
                new TranslationRule("folder_code", SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("folder_dsc",  SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("Auth_FIO",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("dep_code",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("dep_dsc",     SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("crsp_code",   SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("crsp_dsc",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("from_dt",     SqlDbType.DateTime, SqlDbType.DateTime),
                new TranslationRule("to_dt",       SqlDbType.DateTime, SqlDbType.DateTime),
                new TranslationRule("otw_dsc",     SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("dobj_code",   SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("dobj_dsc",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("cust_code",   SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("customer",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("sost_dsc",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("nomer",       SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("naimen",      SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("grif",        SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("wid_dsc",     SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("ot_dt",       SqlDbType.DateTime, SqlDbType.DateTime),
                new TranslationRule("org_code",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("org_dsc",     SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("ask_code",    SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("ask_dsc",     SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("summa",       SqlDbType.Money,    SqlDbType.Money),
                new TranslationRule("flg_rast",    SqlDbType.Bit,      SqlDbType.Int),
                new TranslationRule("SdlNom",      SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("uso",         SqlDbType.VarChar,  SqlDbType.VarChar),
                new TranslationRule("flg_est",     SqlDbType.Bit,      SqlDbType.Int),
                new TranslationRule("flg_dop",     SqlDbType.Bit,      SqlDbType.Int),
                new TranslationRule("descrip",     SqlDbType.VarChar,  SqlDbType.VarChar)
            };
        }
        protected override void SetupInsertCommandText()
        {
            _InsertCommand = "INSERT [TPrSrv_DgwHeader_Stack] "
            + "(id_sea, code, regnom, dt, folder_code, folder_dsc, Auth_code, Auth_FIO, dep_code, dep_dsc, crsp_code, crsp_dsc, "
            + "from_dt, to_dt, otw_code, otw_dsc, dobj_code, dobj_dsc, cust_code, customer, sost_int, sost_dsc, nomer, "
            + "naimen, grif_code, grif, wid_code, wid_dsc, ot_dt, org_code, org_dsc, ask_code, ask_dsc, summa, flg_rast, "
            + "SdlNom, uso, flg_est, flg_dop, descrip) "
            + "VALUES "
            + "(@id_sea, @code, @regnom, @dt, @folder_code, @folder_dsc, @Auth_code, @Auth_FIO, @dep_code, @dep_dsc, @crsp_code, @crsp_dsc, "
            + "@from_dt, @to_dt, @otw_code, @otw_dsc, @dobj_code, @dobj_dsc, @cust_code, @customer, @sost_int, @sost_dsc, @nomer, "
            + "@naimen, @grif_code, @grif, @wid_code, @wid_dsc, @ot_dt, @org_code, @org_dsc, @ask_code, @ask_dsc, @summa, @flg_rast, "
            + "@SdlNom, @uso, @flg_est, @flg_dop, @descrip);";
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
            AddParameter(command, "Auth_code", SqlDbType.VarChar, DBNull.Value);
            AddParameter(command, "otw_code",  SqlDbType.VarChar, DBNull.Value);
            AddParameter(command, "sost_int",  SqlDbType.Int,     DBNull.Value);
            AddParameter(command, "grif_code", SqlDbType.Int,     DBNull.Value);
            AddParameter(command, "wid_code",  SqlDbType.VarChar, DBNull.Value);
        }
    }
}
