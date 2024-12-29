using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.DBSetting.Membership.CORs.Executor
{
    public class ValidateMemberExecutor : IMembershipExecutor
    {
        IMembershipExecutor _next;

        public ValidateMemberExecutor(IMembershipExecutor next)
        {
            _next = next;
        }

        public void Execute(List<string> boxValue, List<TextBox> boxInstance)
        {
            //Register logic here:
            if (boxValue.Count > 2)
            {

            }
            //Login logic here:
            else
            {

            }

            //go next
            if (_next != null)
            {
                _next.Execute(boxValue, boxInstance);
            }
            
        }

        public void SetNext(IMembershipExecutor executor)
        {
            _next = executor;
        }
    }
}
