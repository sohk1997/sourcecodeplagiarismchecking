using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Root.Extension
{
    public class DatabaseParam
    {
        internal DatabaseParam() { }
        internal string ParamName { get; set; }
        internal object Value { get; set; }
        internal DbType DbType { get; set; }
    }
    public static class ParamConstructor
    {
        public static DatabaseParam ToParam(this object value, string paramName, DbType dbType)
        {
            return new DatabaseParam(){
                ParamName = "@" + paramName,
                DbType = dbType,
                Value = value
            };
        }
    }
}
