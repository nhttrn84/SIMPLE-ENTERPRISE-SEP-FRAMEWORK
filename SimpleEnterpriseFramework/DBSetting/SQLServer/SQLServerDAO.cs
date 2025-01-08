using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using SimpleEnterpriseFramework.DBSetting.Membership.HashPassword;
using static SimpleEnterpriseFramework.DBSetting.DAO.SQLServerProcess;

namespace SimpleEnterpriseFramework.DBSetting.DAO
{
    public class SQLServerDAO : AbstractDAO
    {
        public SQLServerDAO()
        {
            this.ProcessData = SQLServerProcess.Instance;
        }

        public void UpdateConnectionString(string newConnectionString)
        {
            SQLServerProcess.Instance.ConnectionString = newConnectionString;
        }

        public override DataTable LoadData(string strNameTable)
        {
            string sql = "Select * From " + strNameTable;
            DataTable result = ProcessData.LoadData(sql);
            return result;
        }

        public override string GetPrimaryKey(string strNameTable)
        {
            string sql = "SELECT u.COLUMN_NAME, c.CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS c INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS u ON c.CONSTRAINT_NAME = u.CONSTRAINT_NAME where u.TABLE_NAME = '" + strNameTable + "' AND c.TABLE_NAME = '" + strNameTable + "' and c.CONSTRAINT_TYPE = 'PRIMARY KEY'";
            DataTable result = ProcessData.LoadData(sql);
            return result.Rows[0].Field<string>(0);
        }

        public override List<string> GetPrimaryKeyColumns(string strNameTable)
        {
            return this.ProcessData.GetPrimaryKeyColumns(strNameTable);
        }

        public override List<string> GetDatabaseNames()
        {
            return this.ProcessData.GetDatabaseNames();
        }

        public override List<string> GetAllTablesName()
        {
            return this.ProcessData.GetAllTablesName();
        }

        public override bool InsertData(Dictionary<string, string> data, string strNameTable, string database)
        {
            string sql = $"USE {database} Insert Into {strNameTable} Values(";
            for (int i = 0; i < data.Count; i++)
            {
                if (i < data.Count - 1)
                {
                    sql += ("N'" + data.ElementAt(i).Value + "', ");
                }
                else
                {
                    sql += ("N'" + data.ElementAt(i).Value + "')");
                }
            }

            try
            {
                ProcessData.ExecuteData(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public override bool UpdateData(Dictionary<string, string> data, string strNameTable, string database)
        {
            // Retrieve primary key columns for the table
            List<string> primaryKeyColumns = this.ProcessData.GetPrimaryKeyColumns(database);

            string sql = $"USE {database} UPDATE {strNameTable} SET ";
            int index = 0;

            // Construct the SET clause
            List<string> setClause = new List<string>();
            foreach (var entry in data)
            {
                if(!primaryKeyColumns.Contains(entry.Key))
                {
                    setClause.Add($"{entry.Key} = '{entry.Value}'");
                }
            }
            sql += string.Join(",", setClause);

            // Construct the WHERE clause using the primary key columns
            sql += " WHERE ";
            List<string> setclause2 = new List<string>();
            foreach (var key in primaryKeyColumns)
            {
                setclause2.Add($"{key} = '{data[key]}'");
            }
            sql += string.Join(" AND ", setclause2);

            try
            {
                ProcessData.ExecuteData(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }


        public override bool DeleteData(string strNameTable, string primaryKey, string keyValue)
        {
            string sql = "Delete From " + strNameTable + " Where " + primaryKey + " = " + keyValue;

            try
            {
                ProcessData.ExecuteData(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public override void CreateAccountTable()
        {
            if (!isExistTableAccount())
            {
                string sql = "Create Table Account(ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, username varchar(30), password varchar(30), isLogin bit)";
                ProcessData.ExecuteData(sql);
            }
        }

        private bool isExistTableAccount()
        {
            string sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES Where Table_Schema = 'dbo'  AND Table_Name = 'Account'";
            DataTable dataTable = ProcessData.LoadData(sql);
            return dataTable.Rows.Count != 0;
        }

        public override bool ValidateUser(string username, string password)
        {
            if (Authentication(username, password))
            {
                string sql = string.Format("Update Account Set isLogin = 'true' where username ='{0}'", username);
                if (ProcessData.ExecuteData(sql) != 0)
                    return true;
            }
            return false;
        }

        private bool Authentication(string username, string password)
        {
            var sql = string.Format("Select * from Account Where username = '{0}'", username);
            DataTable data = ProcessData.LoadData(sql);
            if (data.Rows.Count != 0)
            {
                string u = data.Rows[0][1].ToString();
                string p = data.Rows[0][2].ToString();
                string hashedPassword = HashPassword.hashPassword(password);
                return username == u && hashedPassword == p;
            }
            return false;
        }

        public override bool CreateUser(string username, string password)
        {
            if (isExistUser(username)) return false;
            string hashedPassword = HashPassword.hashPassword(password);
            string sql = string.Format("Insert Into Account Values('{0}','{1}','false')", username, hashedPassword);
            if (ProcessData.ExecuteData(sql) != 0)
                return true;
            return false;
        }

        private bool isExistUser(string username)
        {
            string sql = string.Format("Select * From Account Where username = '{0}'", username);
            var data = ProcessData.LoadData(sql);
            return data.Rows.Count != 0;
        }

        public override bool SignOutUser(string username)
        {
            string sql = string.Format("Update Account Set isLogin = 'false' where username ='{0}'", username);
            if (ProcessData.ExecuteData(sql) != 0)
                return true;
            return false;
        }
    }
}
