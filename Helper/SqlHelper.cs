using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace System
{
    public class MySqlHelper
    {

        private static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

        public static MySqlDataReader GetDataReader(out string error, string sql)
        {
            MySqlDataReader dr = null;
            error = string.Empty;
            try
            {
                var conn = new MySqlConnection(ConnectionString);
                var comm = new MySqlCommand(sql, conn);
                conn.Open();
                dr = comm.ExecuteReader();

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return dr;
        }

        // 读取数据 datatable
        public static DataTable GetDataTable(out string sError, string sSQL)
        {
            DataTable dt = null;
            sError = string.Empty;

            MySqlConnection myConn = null;
            try
            {
                myConn = new MySqlConnection(ConnectionString);
                MySqlCommand myCommand = new MySqlCommand(sSQL, myConn);
                myConn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(myCommand);
                dt = new DataTable();
                adapter.Fill(dt);
                myConn.Close();
            }
            catch (Exception ex)
            {
                sError = ex.Message;
            }
            return dt;
        }

        // 读取数据 dataset
        public static DataSet GetDataSet(out string sError, string sSQL, MySqlParameter[] para = null)
        {
            DataSet ds = null;
            sError = string.Empty;

            MySqlConnection myConn = null;
            try
            {
                myConn = new MySqlConnection(ConnectionString);
                MySqlCommand myCmd = new MySqlCommand(sSQL, myConn);
                if (para != null)
                    myCmd.Parameters.AddRange(para);
                myConn.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(myCmd);
                ds = new DataSet();
                adapter.Fill(ds);
                myConn.Close();
            }
            catch (Exception ex)
            {
                sError = ex.Message;
            }
            return ds;
        }

        // 取最大的ID
        public static Int32 GetMaxID(out string sError, string sKeyField, string sTableName)
        {
            DataTable dt = GetDataTable(out sError, "select IFNULL(max(" + sKeyField + "),0) as MaxID from " + sTableName);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }

            return 0;
        }

        // 插入，修改，删除，是否使用事务
        public static bool UpdateData(out string sError, string sSQL, MySqlParameter[] param = null, bool bUseTransaction = false)
        {
            int iResult = 0;
            sError = string.Empty;

            MySqlConnection myConn = null;

            if (!bUseTransaction)
            {
                try
                {
                    myConn = new MySqlConnection(ConnectionString);
                    MySqlCommand myCmd = new MySqlCommand(sSQL, myConn);
                    myConn.Open();
                    if (param != null)
                        myCmd.Parameters.AddRange(param);
                    iResult = myCmd.ExecuteNonQuery();
                    myConn.Close();
                }
                catch (Exception ex)
                {
                    sError = ex.Message;
                    iResult = -1;
                }
            }
            else // 使用事务
            {
                MySqlTransaction myTrans = null;
                try
                {
                    myConn = new MySqlConnection(ConnectionString);
                    myConn.Open();
                    myTrans = myConn.BeginTransaction();
                    MySqlCommand myCmd = new MySqlCommand(sSQL, myConn);
                    myCmd.Transaction = myTrans;
                    iResult = myCmd.ExecuteNonQuery();
                    myTrans.Commit();
                    myConn.Close();
                }
                catch (Exception ex)
                {
                    sError = ex.Message;
                    iResult = -1;
                    myTrans.Rollback();
                }
            }

            return iResult > 0;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static bool Exists(string sql, MySqlParameter[] param = null)
        {
            var myConn = new MySqlConnection(ConnectionString);
            MySqlCommand myCmd = new MySqlCommand(sql, myConn);
            myConn.Open();
            if (param != null)
                myCmd.Parameters.AddRange(param);
            var result = myCmd.ExecuteScalar();
            myConn.Close();
            return (int)result > 0 ? true : false;
        }
    }
}