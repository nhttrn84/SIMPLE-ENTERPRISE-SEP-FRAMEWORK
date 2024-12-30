using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.DBSetting.DAO
{
    public class SQLServerProcess : AbstractProcessData
    {
        private string _connectionString;

        public SQLServerProcess(string connectionString)
        {
            _connectionString = connectionString;
            connection = new SqlConnection(connectionString);
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
            this.connection.Open();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                connection.Close();

                return table;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
        }

        public override int ExecuteData(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);

            connection.Open();

            int rs = 1;
            try
            {
                rs = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }

            connection.Close();
            return rs;
        }

        public override bool isExist(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            connection.Open();

            SqlDataReader dr = sqlCommand.ExecuteReader();

            if (dr.Read() == true)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }
        }
    }
}
