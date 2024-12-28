using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SimpleEnterpriseFramework.DBSetting.SQLServer
{
    class SqlServerProcessor
    {
        private readonly SqlConnection connectionSqlServer;

        public SqlServerProcessor(string connection)
        {
            this.connectionSqlServer = new SqlConnection(connection);
        }

        public DataTable GetAllData(string sqlCommand)
        {
            connectionSqlServer.Open();
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand, connectionSqlServer);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                connectionSqlServer.Close();
            }
        }

        public bool IsExist(string sqlCommand)
        {
            SqlCommand command = new SqlCommand(sqlCommand, connectionSqlServer);
            connectionSqlServer.Open();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                return reader.HasRows;
            }
            finally
            {
                connectionSqlServer.Close();
            }
        }

        public int ExecuteNonQuery(string sqlCommand, Dictionary<string, object> parameters = null)
        {
            SqlCommand command = new SqlCommand(sqlCommand, connectionSqlServer);
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    command.Parameters.AddWithValue("@" + entry.Key, entry.Value ?? DBNull.Value);
                }
            }

            connectionSqlServer.Open();
            try
            {
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                connectionSqlServer.Close();
            }
        }
    }
}
