using System;
using System.Data;
using System.Data.SqlClient;

namespace SimpleEnterpriseFramework.DBSetting.SQLServer
{
    public class SQLServer : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public SQLServer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool OpenConnection()
        {
            Console.WriteLine($"Connection string: {_connectionString}");
            try
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening connection: {ex.Message}");
                return false;
            }
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
