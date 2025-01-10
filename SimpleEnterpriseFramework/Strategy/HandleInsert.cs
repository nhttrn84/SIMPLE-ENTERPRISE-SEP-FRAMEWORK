using SimpleEnterpriseFramework.DBSetting.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Strategy
{
    public class HandleInsert : IHandleDataStrategy
    {
        public void HandleData(SQLServerDAO sqlServerDAO, Dictionary<string, string> data, string strNameTable, string database)
        {
            try
            {
                sqlServerDAO.InsertData(data, strNameTable, database);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
