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
            connectionData = "Data Source=KIMTRINH\\SQLEXPRESS; Integrated Security=True;";
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
            connectionData = $"Data Source=KIMTRINH\\SQLEXPRESS; Integrated Security=True; Initial Catalog={databaseName};";
        }

        public void ResetDatabaseName()
        {
            connectionData = "Data Source=KIMTRINH\\SQLEXPRESS; Integrated Security=True;";
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

    }
}
