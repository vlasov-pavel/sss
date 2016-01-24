using System;
using System.Data;

namespace ETL
{
    public sealed class TranslationRule
    {
        public TranslationRule(string field, SqlDbType source_type, SqlDbType target_type) : base()
        {
            Field      = field;
            SourceType = source_type;
            TargetType = target_type;
        }
        public string    Field      { private set; get; }
        public SqlDbType SourceType { private set; get; }
        public SqlDbType TargetType { private set; get; }
    }
}
