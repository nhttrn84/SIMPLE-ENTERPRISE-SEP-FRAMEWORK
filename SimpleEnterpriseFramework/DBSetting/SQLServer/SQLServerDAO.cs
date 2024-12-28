using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.DBSetting.SQLServer
{
    class SqlServerDAO
    {
        private readonly SqlServerProcessor processor;

        public SqlServerDAO(string connection)
        {
            processor = new SqlServerProcessor(connection);
        }

        public void CreateMemberTable()
        {
            string checkTableSql = "SELECT * FROM information_schema.tables WHERE table_schema = 'dbo' AND table_name = 'member'";
            DataTable dataTable = processor.GetAllData(checkTableSql);
            bool tableExists = dataTable.Rows.Count > 0;

            if (!tableExists)
            {
                string createTableSql = "CREATE TABLE member (id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, username VARCHAR(30), password VARCHAR(30), isLogin BIT)";
                processor.ExecuteNonQuery(createTableSql);
            }
        }

        public bool CreateNewUser(string username, string password)
        {
            string checkUserSql = $"SELECT * FROM member WHERE username = '{username}'";
            DataTable dataTable = processor.GetAllData(checkUserSql);

            if (dataTable.Rows.Count > 0)
            {
                Console.WriteLine("User already exists.");
                return false;
            }

            string insertUserSql = "INSERT INTO member (username, password, isLogin) VALUES (@username, @password, 0)";
            var parameters = new Dictionary<string, object> { { "username", username }, { "password", password } };

            if (processor.ExecuteNonQuery(insertUserSql, parameters) > 0)
            {
                Console.WriteLine("User created successfully.");
                return true;
            }

            Console.WriteLine("Failed to create user.");
            return false;
        }

        public bool Authenticate(string username, string password)
        {
            string sql = $"SELECT * FROM member WHERE username = '{username}'";
            DataTable data = processor.GetAllData(sql);

            if (data.Rows.Count > 0)
            {
                string dbPassword = data.Rows[0]["password"].ToString();
                return password == dbPassword;
            }

            return false;
        }

        public bool CheckUserLogin(string username, string password)
        {
            if (Authenticate(username, password))
            {
                string updateLoginSql = "UPDATE member SET isLogin = 1 WHERE username = @username";
                var parameters = new Dictionary<string, object> { { "username", username } };
                return processor.ExecuteNonQuery(updateLoginSql, parameters) > 0;
            }

            return false;
        }

        public DataTable GetAllData(string tableName)
        {
            string sql = $"SELECT * FROM {tableName}";
            return processor.GetAllData(sql);
        }

        public string GetPrimaryKey(string tableName)
        {
            string sql = $@"SELECT COLUMN_NAME 
                            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                            WHERE TABLE_NAME = '{tableName}'";
            DataTable dataTable = processor.GetAllData(sql);
            return dataTable.Rows[0]["COLUMN_NAME"].ToString();
        }

        public List<string> GetAllFieldsName(string tableName)
        {
            string sql = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
            DataTable dataTable = processor.GetAllData(sql);
            return dataTable.Rows.Cast<DataRow>().Select(row => row["COLUMN_NAME"].ToString()).ToList();
        }

        public bool Insert(Dictionary<string, object> data, string tableName)
        {
            string columns = string.Join(", ", data.Keys);
            string parameters = string.Join(", ", data.Keys.Select(k => "@" + k));
            string sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

            return processor.ExecuteNonQuery(sql, data) > 0;
        }

        public bool Update(Dictionary<string, object> data, string tableName)
        {
            string primaryKey = GetPrimaryKey(tableName);
            string setClause = string.Join(", ", data.Keys.Where(k => k != primaryKey).Select(k => $"{k} = @{k}"));
            string sql = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey} = @{primaryKey}";

            return processor.ExecuteNonQuery(sql, data) > 0;
        }

        public bool Delete(string tableName, object primaryKeyValue)
        {
            string primaryKey = GetPrimaryKey(tableName);
            string sql = $"DELETE FROM {tableName} WHERE {primaryKey} = @primaryKey";
            var parameters = new Dictionary<string, object> { { "primaryKey", primaryKeyValue } };

            return processor.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
