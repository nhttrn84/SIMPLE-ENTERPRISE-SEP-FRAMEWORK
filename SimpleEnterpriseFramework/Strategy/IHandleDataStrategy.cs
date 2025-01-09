using SimpleEnterpriseFramework.DBSetting.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.Strategy
{
    public interface IHandleDataStrategy
    {
        void HandleData(SQLServerDAO sqlServerDAO, Dictionary<string, string> data, string strNameTable, string database);
    }
}
