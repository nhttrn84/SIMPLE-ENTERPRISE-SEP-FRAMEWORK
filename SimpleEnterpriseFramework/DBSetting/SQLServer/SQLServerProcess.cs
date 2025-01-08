using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SimpleEnterpriseFramework.DBSetting.DAO
{
    public class SQLServerProcess : AbstractProcessData
    {
        private static SQLServerProcess _instance;
        private static readonly object _lock = new object();
        private string _connectionString;

        private SQLServerProcess()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            this.connection = new SqlConnection(_connectionString);
        }

        public static SQLServerProcess Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SQLServerProcess();
                        }
                    }
                }
                return _instance;
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    connection = new SqlConnection(_connectionString);
                }
            }
        }

        public override DataTable LoadData(string sql)
        {
            connection.Open();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public override int ExecuteData(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            connection.Open();
            try
            {
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public override bool isExist(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            connection.Open();
            try
            {
                using (SqlDataReader dr = sqlCommand.ExecuteReader())
                {
                    return dr.Read();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public override List<string> GetDatabaseNames()
        {
            List<string> databaseNames = new List<string>();
            connection.Open();
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT name FROM sys.databases", connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            databaseNames.Add(dataReader[0].ToString());
                        }
                    }
                }
                return databaseNames;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public override List<string> GetAllTablesName()
        {
            List<string> listTables = new List<string>();
            connection.Open();
            try
            {
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
                return listTables;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public override List<string> GetPrimaryKeyColumns(string tableName)
        {
            List<string> primaryKeyColumns = new List<string>();
            string query = @"
                SELECT COLUMN_NAME
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                WHERE TABLE_NAME = @TableName AND CONSTRAINT_NAME = (
                    SELECT CONSTRAINT_NAME
                    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                    WHERE TABLE_NAME = @TableName AND CONSTRAINT_TYPE = 'PRIMARY KEY'
                )";

            connection.Open();
            try
            {
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
                return primaryKeyColumns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
