using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.DBSetting.DAO
{
    public abstract class AbstractProcessData
    {
        protected SqlConnection connection;
        public abstract DataTable LoadData(string sql);

        public abstract int ExecuteData(string sql);

        public abstract bool isExist(string sql);

        public abstract List<string> GetDatabaseNames();

        public abstract List<string> GetAllTablesName();

        public abstract List<string> GetPrimaryKeyColumns(string tableName);
    }
}
