using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.DBSetting.Membership.CORs
{
    public interface IMembershipExecutor
    {
        void SetNext(IMembershipExecutor executor);

        //0: username, 1: password, 2: repassword
        void Execute(List<String> boxValue, List<TextBox> boxInstance); 


    }
}
