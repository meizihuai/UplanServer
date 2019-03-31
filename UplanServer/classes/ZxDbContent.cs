using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UplanServer
{
    public class ZxDbContent
    {
        private static OracleHelper ora = Module.ora;
        public static bool IsExist(string tableName,int id = 0, string where = "")
        {
            string sql = "";
            if (id > 0)
            {
                sql = "select id from " + tableName + " where id=" + id + "";
            }
            else
            {
                sql = "select id from " + tableName + (where==""?"":" where "+where);
            }
            return ora.SqlIsIn(sql);
        }
    }
}