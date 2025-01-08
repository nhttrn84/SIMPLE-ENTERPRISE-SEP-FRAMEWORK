using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.DBSetting
{
    public class DatabaseInfo
    {
        private static DatabaseInfo _instance;
        public string connectionData;

        private DatabaseInfo() {
            connectionData = "Data Source=DESKTOP-67S48US\\SQLEXPRESS; Integrated Security=True;";
        }

        public static DatabaseInfo Instance { 
            
            get {
                if (_instance == null)
                    _instance = new DatabaseInfo();

                return _instance;
            } 
        }

        public void SetDatabaseName(string databaseName)
        {
            connectionData = $"Data Source=DESKTOP-67S48US\\SQLEXPRESS; Integrated Security=True; Initial Catalog={databaseName};";
        }

        public void ResetDatabaseName()
        {
            connectionData = "Data Source=DESKTOP-67S48US\\SQLEXPRESS; Integrated Security=True;";
        }

        public List<string> GetAllTablesName()
        {
            List<string> listTables = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionData))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.tables WHERE name != 'sysdiagrams'", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listTables.Add(reader[0].ToString());
                        }
                    }
                }
            }
            return listTables;
        }

        public List<string> GetPrimaryKeyColumns(string tableName)
        {
            List<string> primaryKeyColumns = new List<string>();
            string query = $@"
        SELECT COLUMN_NAME
        FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
        WHERE TABLE_NAME = @TableName AND CONSTRAINT_NAME = (
            SELECT CONSTRAINT_NAME
            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
            WHERE TABLE_NAME = @TableName AND CONSTRAINT_TYPE = 'PRIMARY KEY'
        )";

            using (SqlConnection connection = new SqlConnection(connectionData))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            primaryKeyColumns.Add(reader["COLUMN_NAME"].ToString());
                        }
                    }
                }
            }
            return primaryKeyColumns;
        }


    }
}
