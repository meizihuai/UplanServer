using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Text;

namespace UplanServer
{
   

    public class OracleHelper
    {
        private string NKConnectString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=111.53.74.132)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=oss)));Persist Security Info=True;User ID=npo;Password=Smart9080;";
        public OracleHelper(string ip, int port, string seviceName, string usr, string pwd)
        {
            NKConnectString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));Persist Security Info=True;User ID={3};Password={4};";
            NKConnectString = string.Format(NKConnectString, new string[] { ip, port+"", seviceName, usr, pwd });
        }
        public OracleHelper(string cfg)
        {
            string[] st = cfg.Split(';');
            if (st.Length < 5)
            {
                throw new Exception("ora配置字段无效");
            }
            string ip = st[0];
            string port = st[1];
            string seviceName = st[2];
            string usr = st[3];
            string pwd = st[4];
            NKConnectString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));Persist Security Info=True;User ID={3};Password={4};";
            NKConnectString = string.Format(NKConnectString, new string[] { ip, port, seviceName, usr, pwd });
        }
        struct OracleParaList
        {
            public List<string> dataList;
        }
        public string SqlCMDListQuickByPara(string databaseName, DataTable dt)
        {
            return SubSqlCMDListQuickByPara(databaseName, dt);
        }
        private string SubSqlCMDListQuickByPara(string databaseName, DataTable dt)
        {
            if (dt==null)
                return "dt=null";
            using (OracleConnection conn = new OracleConnection(NKConnectString))
            {           
                try
                {
                    conn.Open();
                    string sql = "insert into " + databaseName + " ({0}) values ({1})";
                    string measTypes = "";
                    string measValues = "";
                    foreach (DataColumn c in dt.Columns)
                    {
                        measTypes = measTypes + c.ColumnName + ",";
                        measValues = measValues + ":" + c.ColumnName + ",";
                    }
                    measTypes = measTypes.Substring(0, measTypes.Length - 1);
                    measValues = measValues.Substring(0, measValues.Length - 1);
                    sql = string.Format(sql, measTypes, measValues);
                    int sumCount = dt.Rows.Count;
                    int readIndex = 0;
                    while (true)
                    {
                        int perCount = 500;
                        int restCount = sumCount - readIndex;
                        if (perCount > restCount)
                            perCount = restCount;
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.ArrayBindCount = perCount;
                        cmd.CommandText = sql;
                        int arrayCount = perCount;
                        List<OracleParaList> fatherList = new List<OracleParaList>();
                        for (var i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            OracleParaList itm = new OracleParaList();
                            itm.dataList = new List<string>();
                            for (var j = readIndex; j <= perCount + readIndex - 1; j++)
                            {
                                DataRow itmRow = dt.Rows[j];
                                string valueItm = itmRow[i].ToString();
                                itm.dataList.Add(valueItm);
                            }
                            fatherList.Add(itm);
                        }
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            OracleParameter PARM = new OracleParameter(dt.Columns[i].ColumnName, OracleDbType.Varchar2);
                            PARM.Direction = ParameterDirection.Input;
                            PARM.Value = fatherList[i].dataList.ToArray();
                            cmd.Parameters.Add(PARM);
                        }
                        cmd.ExecuteNonQuery();
                        readIndex = readIndex + perCount;
                        if (readIndex >= sumCount)
                            break;
                    }
                    conn.Close();
                    return "success";
                }
                catch (Exception ex)
                {
                    // MsgBox(ex.ToString)
                    // File.WriteAllText("d:\oraerr.txt", ex.ToString)
                    conn.Close();
                    return ex.ToString();
                }
            }
        }
        public string SQLGetFirstRowCell(string sql)
        {
            try
            {
                DataTable dt = SqlGetDT(sql);
                if (dt==null)
                    return "";
                if (dt.Rows.Count == 0)
                    return "";
                string str = dt.Rows[0][0].ToString();
                if (str == null) return "";
                if (str ==DBNull.Value.ToString()) return "";
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string SQLInfo(string CmdString)
        {
            using (OracleConnection conn = new OracleConnection(NKConnectString))
            {
                try
                {
                    conn.Open();
                    OracleCommand SQLCommand = new OracleCommand(CmdString, conn);
                    object obj = SQLCommand.ExecuteScalar();
                    string str = "";
                    if (obj != null && !(obj is DBNull))
                    {
                        str = obj.ToString();
                    }
                    conn.Close();
                    return str;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return ex.Message;
                }
            }
        }
        public string SqlCMD(string sql)
        {
            using (OracleConnection conn = new OracleConnection(NKConnectString))
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.CommandTimeout = 99999999;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "success";
                }
                catch (Exception ex)
                {
                    // File.WriteAllText("d:\oraerrCmd.txt", ex.ToString & vbCrLf & sql)
                    conn.Close();

                    return ex.Message;
                }
            }
        }
        public string SqlCMDList(List<string> sqlList)
        {
            if (sqlList==null)
                return "sqlList=null";
            using (OracleConnection conn = new OracleConnection(NKConnectString))
            {
                try
                {
                    conn.Open();
                    string str = "";
                    foreach (var sq in sqlList)
                        str = str + sq + ";";
                    str = "begin" + Environment.NewLine + str + " " + Environment.NewLine + "end;";
                    OracleCommand cmd = new OracleCommand(str, conn);
                    cmd.CommandTimeout = 99999999;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return "success";
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return ex.ToString();
                }
            }
        }

        public DataTable SqlGetDT(string sql)
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = new OracleConnection(NKConnectString))
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.CommandTimeout = 99999999;
                    OracleDataAdapter sda = new OracleDataAdapter(cmd);
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (Exception ex)
                {
                   // File.WriteAllText(@"d:\oraerrGet.txt", ex.ToString() + Environment.NewLine + sql);
                    conn.Close();
                    return dt;
                }
            }
        }
        public bool SqlIsIn(string sql)
        {
            DataTable dt = SqlGetDT(sql);
            if (dt==null)
                return false;
            if (dt.Rows.Count == 0)
                return false;
            return true;
        }

        public string[] GetOraTableColumns(string tableName)
        {
            try
            {
                string sql = "select COLUMN_NAME from user_tab_columns where table_name ='" + tableName.ToUpper() + "'";
                DataTable dt =SqlGetDT(sql);
                if (dt==null)
                    return null;
                if (dt.Rows.Count == 0)
                    return null;
                List<string> list = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    if (row[0] != null)
                        list.Add(row[0].ToString());
                }
                return list.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetOraTableColumnsOnDt(string tableName, bool isRemoveId = true)
        {
            try
            {
                string[] cols = GetOraTableColumns(tableName);
                if (cols!=null)
                    return null/* TODO Change to default(_) if this is not a reference type */;
                if (cols.Length == 0)
                    return null/* TODO Change to default(_) if this is not a reference type */;
                DataTable dt = new DataTable();
                foreach (var col in cols)
                {
                    if (isRemoveId)
                    {
                        if (col == "ID")
                            continue;
                    }
                    dt.Columns.Add(col);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
        }
        public DataTable GetOraTableInfo(string tableName)
        {
            try
            {
                string sql = "select * from user_tab_columns where table_name ='" + tableName.ToUpper() + "'";
                DataTable dt = SqlGetDT(sql);
                if (dt == null)
                    return null;
                if (dt.Rows.Count == 0)
                    return null;
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetUserTables()
        {
            try
            {
                string sql = "select * from user_tab_comments";
                DataTable dt = SqlGetDT(sql);
                if (dt == null)
                    return null;
                if (dt.Rows.Count == 0)
                    return null;
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string OracleSelectPage(string sql, long startIndex, long count)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from ( ");
            sb.Append("select A.*, Rownum RN from ( ");
            sb.Append(sql);
            sb.Append(") A ");
            sb.Append(" where Rownum<=" + startIndex + count + " )");
            sb.Append("where RN>=" + startIndex);
            return sb.ToString();
        }


        public NormalResponse InsertByDik(string tableName,Dictionary<string,object> dik)
        {
            try
            {
                if (dik == null)
                {
                    return new NormalResponse(false, "写入数据库字段为空");
                }
                string sql = "insert into " + tableName + "({0}) values ({1})";
                string keys = "";
                foreach (string itm in dik.Keys)
                {
                    keys = keys + "," + itm;
                }
                keys = keys.Substring(1, keys.Length - 1);
                string values = "";
                foreach (object itm in dik.Values)
                {
                    string tmp = "";
                    if (itm != null)
                    {
                        tmp = itm.ToString();
                    }
                    values = values + ",'" + tmp + "'";
                }
                values = values.Substring(1, values.Length - 1);
                sql = string.Format(sql, keys, values);
                string result= SqlCMD(sql);
                if (result == "success")
                {
                    return new NormalResponse(true, result);
                }
                else
                {
                    return new NormalResponse(false, result,"",sql);
                }
                
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
            
        }
        public NormalResponse UpdateByDik(string tableName, Dictionary<string, object> dik,int id,string where="")
        {
            try
            {
                string sql = "";
                if (id > 0)
                {
                   sql= "update " + tableName + " set " + "{0} where id=" + id + (where == "" ? "" : " and " + where);
                }
                else
                {
                    sql = "update " + tableName + " set " + "{0} "+ (where == "" ? "" : " where " + where);
                }
                string sets = "";
                foreach (var itm in dik)
                {
                    string tmp = "";
                    if (itm.Value != null)
                    {
                        tmp = itm.Value.ToString();
                    }
                    sets = sets + "," + itm.Key.ToString() + "='" + tmp + "'";
                }
                
                sets = sets.Substring(1, sets.Length - 1);
                sql = string.Format(sql, sets);
                string result = SqlCMD(sql);
                if (result == "success")
                {
                    return new NormalResponse(true, result);
                }
                else
                {
                    return new NormalResponse(false, result, "", sql);
                }
            }
            catch (Exception e)
            {
                return new NormalResponse(false, e.ToString());
            }
           
        }


    }

}