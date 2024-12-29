using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.DBSetting.Membership.CORs
{
    public class EmptyFieldExecutor : IMembershipExecutor
    {
        IMembershipExecutor _next;
        public EmptyFieldExecutor(IMembershipExecutor next)
        {
            _next = next;
        }
        public void Execute(List<string> boxValue, List<TextBox> boxInstance)
        {
            bool _isPassed = true;

            if (boxValue[0] == "" || boxValue[0] == "Account" || boxValue[0] == "Empty field")
            {
                boxInstance[0].Text = "Empty field";
                boxInstance[0].ForeColor = System.Drawing.Color.Red;
                _isPassed = false;
            }

            if (boxValue[1] == "" || boxValue[1] == "Password" || boxValue[1] == "Empty field")
            {
                boxInstance[1].Text = "Empty field";
                boxInstance[1].ForeColor = System.Drawing.Color.Red;
                _isPassed = false;
            }

            if (boxValue.Count > 2)
            {
                if (boxValue[2] == "" || boxValue[2] == "RePassword" 
                    || boxValue[2] == "Empty field" || boxValue[2] == "Password mismatch")
                {
                    boxInstance[2].Text = "Empty field";
                    boxInstance[2].ForeColor = System.Drawing.Color.Red;
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
