using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.DBSetting.Membership.CORs.Executor
{
    public class PasswordMatchExecutor : IMembershipExecutor
    {
        IMembershipExecutor _next;

        public PasswordMatchExecutor(IMembershipExecutor next)
        {
            _next = next;
        }

        public void Execute(List<string> boxValue, List<TextBox> boxInstance)
        {
            bool _isPassed = true;

            if (boxValue.Count > 2)
            {
                if (boxValue[2] != boxValue[1])
                {
                    boxInstance[2].Text = "Password mismatch";
                    boxInstance[2].ForeColor = System.Drawing.Color.Red;
                    boxInstance[2].UseSystemPasswordChar = false;
                    _isPassed = false;
                }
            }

            if (_next != null && _isPassed)
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
