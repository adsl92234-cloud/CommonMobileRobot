using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using AML;
using System.Threading;

namespace AML
{
    public class MySQL
    {
        private static Object SqlLock = new Object();
        private static AML_LogPage Log;
        public static string ConnString = "server= 127.0.0.1;uid=root;pwd=admin;database=amldb_foupcart";
        public static void Initial(string SQL_Host, string SQL_User, string SQL_Pass, string SQL_Name, AML_LogPage _Log = null)
        {
            ConnString = "server=" + SQL_Host + ";uid=" + SQL_User + ";pwd=" + SQL_Pass + ";database=" + SQL_Name;
            if (_Log != null)
                Log = _Log;
        }
        public static bool TestSQLConnect()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "SELECT sv_value FROM sv WHERE sv_id = 8000001";
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                object ExeScalar = cmd.ExecuteScalar();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //===================================Basic function=============================================
        public static void Set_data(string Value_Name, string table, string condition, string New_Value)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "UPDATE " + table + " SET " + Value_Name + " = " + New_Value + " WHERE " + condition;
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Set SQL error: " + Value_Name + "/" + table + "/" + condition + "/" + New_Value + ":" + ex.ToString());
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Set SQL Lock time out : 1 sec");
                }
            }
        }
        public static string Read_data(string Value_Name, string table, string condition)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "SELECT " + Value_Name + " FROM " + table + " WHERE " + condition;
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                object ExeScalar = cmd.ExecuteScalar();
                conn.Close();
                if (ExeScalar != null)
                    return ExeScalar.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Read SQL error: " + Value_Name + "/" + table + "/" + condition + ":" + ex.ToString());
                return null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Read SQL Lock time out : 1 sec");
                }
            }
        }

        public static string[] Read1DArray_Data(string Value_Name, string table, string condition)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "SELECT " + Value_Name + " FROM " + table + " WHERE " + condition;
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                MySqlDataReader myReader;
                List<string> data_array = new List<string>();
                myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    data_array.Add(myReader.GetString(0));
                }
                myReader.Close();
                conn.Close();
                return data_array.ToArray();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Read 1D array fail " + Value_Name + "/" + table + "/" + condition + ":" + ex.ToString());
                return null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Read1DArray_Data SQL Lock time out : 1 sec");
                }
            }
        }
        public static string[] Read1DArray_DistinctData(string Value_Name, string table, string condition)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "SELECT distinct " + Value_Name + " FROM " + table + " WHERE " + condition;
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                MySqlDataReader myReader;
                List<string> data_array = new List<string>();
                myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    data_array.Add(myReader.GetString(0));
                }
                myReader.Close();
                conn.Close();
                return data_array.ToArray();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Read 1D_distinct array fail " + Value_Name + "/" + table + "/" + condition + ":" + ex.ToString());
                return null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Read1DArray_DistinctData SQL Lock time out : 1 sec");
                }
            }
        }
        public static DataTable Read2DArray_Data(string Value_Name, string table, string condition)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "SELECT " + Value_Name + " FROM " + table + " WHERE " + condition;
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                MySqlDataAdapter myAdapter;
                DataTable ResultTable;
                myAdapter = new MySqlDataAdapter(CmdText, conn);
                ResultTable = new DataTable();
                myAdapter.Fill(ResultTable);
                conn.Close();
                return ResultTable;
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Read 2D array fail " + Value_Name + "/" + table + "/" + condition + ":" + ex.ToString());
                return null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Read2DArray_Data SQL Lock time out : 1 sec");
                }
            }
        }
        public static DataTable Read2DArray_Data(string Value_Name, string table, string condition, Dictionary<string, object> parameters)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                using (MySqlConnection conn = new MySqlConnection(ConnString))
                using (MySqlCommand cmd = new MySqlCommand("SELECT " + Value_Name + " FROM " + table + " WHERE " + condition, conn))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.AddWithValue(p.Key, p.Value);
                    conn.Open();
                    DataTable ResultTable = new DataTable();
                    using (MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd))
                        myAdapter.Fill(ResultTable);
                    return ResultTable;
                }
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Read 2D array fail " + Value_Name + "/" + table + "/" + condition + ":" + ex.ToString());
                return null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Read2DArray_Data SQL Lock time out : 1 sec");
                }
            }
        }
        public static void Truncate_Table(string table)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText = "TRUNCATE TABLE " + table;
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Insert_CJ(string CJID, string From_To, string FoupList, string Port, string SourceFab = "", string TargetFab = "", string State = "Wait",string Batchid = "")
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "DELETE FROM pool_cj WHERE cjid = '" + CJID + "'";
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                cmd.ExecuteNonQuery();
                CmdText = "INSERT INTO pool_cj(cjid,from_to,fouplist,port,sourcefab,targetfab,state,batchid,createtime) VALUES ('" + CJID + "','" + From_To + "','" + FoupList + "','" + Port + "','" + SourceFab + "','" + TargetFab + "','" + State + "','" + Batchid + "', NOW()" + ")";
                cmd = new MySqlCommand(CmdText, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Inser CJ fail " + CJID + "/" + From_To + "/" + FoupList + ":" + ex.ToString());
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Inser CJ SQL Lock time out : 1 sec");
                }
            }
        }
        public static void Insert_FoupInfo(string Port, string Storage, string CJID, string CartID, string FoupID, string State = "Wait", string LotID = "", string HotLot = "N")
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(SqlLock, 1000, ref lockTaken);
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "INSERT INTO pool_foupinfo(port,storage,cartid,foupid,cjid,state,lotid,hotlot,createtime) VALUES ('" + Port + "','" + Storage + "','" + CartID + "','" + FoupID + "','" + CJID + "','" + State + "','" + LotID + "','" + HotLot + "'," + "NOW())";
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.WriteSystemLog("SQL", "Inser CJ fail " + CJID + "/" + CartID + "/" + FoupID + ":" + ex.ToString());
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(SqlLock);
                else
                {
                    if (Log != null)
                        Log.WriteSystemLog("SQL_Lock", "Inser CJ SQL Lock time out : 1 sec");
                }
            }
        }

        public static void Insert_Alarm(string ALID, string ALTX = "")
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText;
            if (ALTX != "")
                CmdText = "INSERT INTO am VALUES (" + ALID + ",'" + ALTX + "',128,1)";
            else
                CmdText = "INSERT INTO am VALUES (" + ALID + ",'Undefined Alarm-" + DateTime.Now.ToString("yyyyMMdd") + "',128,1)";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);

            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void Insert_Alarm_history(string ALID, string ALTX = "")
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText = "INSERT INTO alarm_history VALUES (NOW()," + ALID + ",128,1)";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);

            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void Insert_Access(string NewAccount, string NewPassword, string NewAccess)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText = "INSERT INTO account VALUES ('" + NewAccount + "','" + NewPassword + "'," + NewAccess + ",NULL,NULL)";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void Insert_PPBODY(string ccode, string pparm, string descrip)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText = "INSERT INTO ppbody VALUES (" + ccode + ",'" + pparm + "','" + descrip + "')";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void Insert_PPList(string[] PP)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText = "TRUNCATE TABLE pplist";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);
            cmd.ExecuteNonQuery();
            for (int i = 0; i < PP.Length; i++)
            {
                if (PP[i].Contains("'"))
                    continue;
                CmdText = "INSERT INTO pplist VALUES ('" + PP[i] + "')";
                cmd = new MySqlCommand(CmdText, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        public static void Insert_SQL_SV(string SVID, string name, string value, string fromat1, string format2, string unit, string description)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT IGNORE INTO `sv` VALUES ('"
                + SVID + "','" + name + "','" + value + "','" + fromat1 + "','" + format2 + "','" + unit + "','" + description + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void Insert_SQL_EC(string ECID, string name, string value, string ec_default, string format1, string min, string max, string unit, string description)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT IGNORE INTO `ec` VALUES ('"
                + ECID + "','" + name + "','" + value + "','" + ec_default + "','" + format1 + "','" + min + "','" + max + "','" + unit + "','" + description + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public static void Delete_Acount(string Account)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("", conn);
            cmd.CommandText = "DELETE FROM account WHERE name = '" + Account + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "OPTIMIZE TABLE account";
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void Delete(string table, string condition)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("", conn);
            cmd.CommandText = "INSERT INTO " + table + "_history SELECT * FROM " + table + " WHERE " + condition;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM " + table + " WHERE " + condition;
            cmd.ExecuteNonQuery();
            //OPTIMIZE TABLE  
            cmd.CommandText = "OPTIMIZE TABLE " + table;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void NoHistoryDelete(string table, string condition)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("", conn);
            cmd.CommandText = "DELETE FROM " + table + " WHERE " + condition;
            cmd.ExecuteNonQuery();
            //OPTIMIZE TABLE  
            cmd.CommandText = "OPTIMIZE TABLE " + table;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void SetSV(string SVID, string SV_Value)
        {
            Set_data("sv_value", "sv", "sv_id = " + SVID, "'" + SV_Value + "'");
        }
        public static void SetSV(string[] SVID, string[] SV_Value)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            if (SVID.Length != SV_Value.Length)
                return;
            string CommandText = "UPDATE sv SET sv_value = CASE sv_id ";
            for (int i = 0; i < SVID.Length; i++)
                CommandText = CommandText + "WHEN " + SVID[i] + " THEN '" + SV_Value[i] + "' ";

            CommandText = CommandText + "END where sv_id in (" + WT_Library.StringArrayCombine(SVID, ",") + ")";
            MySqlCommand cmd = new MySqlCommand(CommandText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void SetEC(string ECID, string EC_Value)
        {
            Set_data("ec_value", "ec", "ec_id = " + ECID, "'" + EC_Value + "'");
        }
        public static void SetEC(string[] ECID, string[] EC_Value)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            if (ECID.Length != EC_Value.Length)
                return;
            string CommandText = "UPDATE ec SET ec_value = CASE ec_id ";
            for (int i = 0; i < ECID.Length; i++)
                CommandText = CommandText + "WHEN " + ECID[i] + " THEN '" + EC_Value[i] + "' ";

            CommandText = CommandText + "END where ec_id in (" + WT_Library.StringArrayCombine(ECID, ",") + ")";
            MySqlCommand cmd = new MySqlCommand(CommandText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static string ReadSV(string SVID)
        {
            return Read_data("sv_value", "sv", "sv_id = " + SVID);
        }
        public static string ReadEC(string ECID)
        {
            return Read_data("ec_value", "ec", "ec_id = " + ECID);
        }

        public static void Raw(string CommandText)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(CommandText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static string[] Read1DArray_CloumnNameData(string table_name)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConnString);
                conn.Open();
                string CmdText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table_name + "'";
                MySqlCommand cmd = new MySqlCommand(CmdText, conn);
                MySqlDataReader myReader;
                List<string> data_array = new List<string>();
                myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    data_array.Add(myReader.GetString(0));
                }
                myReader.Close();
                conn.Close();
                return data_array.ToArray();
            }
            catch (Exception ex)
            {
                //if (Log != null)
                //    Log.WriteSystemLog("SQL", "Read 1D array fail " + Value_Name + "/" + table + "/" + condition + ":" + ex.ToString());
                return null;
            }
        }
        public static void Add_ColumnAfter(string Column_Name, string Type, string table_name,string After_column)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();

            string CmdText = "ALTER TABLE " + table_name + " ADD " + Column_Name + " " + Type + " AFTER " + After_column + ";";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        //===================================DieMap function=============================================
        public static void Insert_DieMap(string Map)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            string CmdText = "INSERT INTO diemap(map) VALUES ('" + Map + "')";
            MySqlCommand cmd = new MySqlCommand(CmdText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void SetDieMap(string Map, string State)
        {
            Set_data("state", "diemap", "map = " + Map, "'" + State + "'");
        }
        public static void SetDieMap(string[] Map, string[] State)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            if (Map.Length != State.Length)
                return;
            string CommandText = "UPDATE diemap SET state = CASE map ";
            for (int i = 0; i < Map.Length; i++)
                CommandText = CommandText + "WHEN " + Map[i] + " THEN '" + State[i] + "' ";

            CommandText = CommandText + "END where map in (" + WT_Library.StringArrayCombine(Map, ",") + ")";
            MySqlCommand cmd = new MySqlCommand(CommandText, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
