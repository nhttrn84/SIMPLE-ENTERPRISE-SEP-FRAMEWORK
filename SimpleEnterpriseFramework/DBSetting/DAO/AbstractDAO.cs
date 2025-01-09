using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.DBSetting.DAO
{
    public abstract class AbstractDAO
    {
        protected AbstractProcessData ProcessData;

        public abstract HashSet<string> GetExcludedColumns(string strNameTable);
        
        public abstract List<string> GetPrimaryKeyColumns(string strNameTable);

        public abstract DataTable LoadData(string strNameTable);

        public abstract List<string> GetDatabaseNames();

        public abstract List<string> GetAllTablesName();

        public abstract bool InsertData(Dictionary<string, string> data, string strNameTable, string database);

        public abstract bool UpdateData(Dictionary<string, string> data, string strNameTable, string database);

        public abstract bool DeleteData(Dictionary<string, string> data, string strNameTable, string database);

        public abstract void CreateAccountTable();

        public abstract bool ValidateUser(string username, string password);

        public abstract bool CreateUser(string username, string password);

        public abstract bool SignOutUser(string username);
    }
}
