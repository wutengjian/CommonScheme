//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data.SQLite;
//using System.Data;
////using Dapper;

//namespace CommonScheme.Net
//{
//    public class DBSQLiteKit
//    {
//        private static string DBPath;
//        private static IDbConnection conn;
//        public static void Instance(string dbPath)
//        {
//            if (System.IO.File.Exists(dbPath) == false)
//                System.IO.File.Create(dbPath);
//            conn = new SQLiteConnection("data source=" + dbPath);
//            DBPath = dbPath;
//        }
//        public static void CreateDB(string dbPath)
//        {
//            dbPath = string.IsNullOrEmpty(dbPath) ? DBPath : dbPath;
//            if (System.IO.File.Exists(dbPath) == false)
//                System.IO.File.Create(dbPath);
//            conn = new SQLiteConnection("data source=" + dbPath);
//            conn.Open();
//            conn.Close();
//        }
//        public static void DeleteDB(string dbPath)
//        {
//            dbPath = string.IsNullOrEmpty(dbPath) ? DBPath : dbPath;
//            if (System.IO.File.Exists(dbPath))
//                System.IO.File.Delete(dbPath);
//        }
//        public static void CreateTable(string sql)
//        {
//            if (conn.State != System.Data.ConnectionState.Open)
//                conn.Open();
//            SQLiteCommand cmd = new SQLiteCommand();
//            cmd.Connection = (SQLiteConnection)conn;
//            cmd.CommandText = sql;
//            //cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int)";
//            cmd.ExecuteNonQuery();
//            conn.Close();
//        }
//        public static void DeleteTable(string tableName)
//        {
//            if (conn.State != System.Data.ConnectionState.Open)
//                conn.Open();
//            SQLiteCommand cmd = new SQLiteCommand();
//            cmd.Connection = (SQLiteConnection)conn;
//            cmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
//            cmd.ExecuteNonQuery();
//            conn.Close();
//        }

//        public static void Execute(string sql, Dictionary<string, object> parms = null)
//        {
//            if (conn.State != System.Data.ConnectionState.Open)
//                conn.Open();
//            SQLiteCommand cmd = new SQLiteCommand();
//            cmd.Connection = (SQLiteConnection)conn;
//            cmd.CommandText = sql;
//            foreach (var key in parms.Keys)
//            {
//                cmd.Parameters.Add(new SQLiteParameter(key, parms[key]));
//            }
//            cmd.ExecuteNonQuery();
//            conn.Close();
//        }
//    }
//}
