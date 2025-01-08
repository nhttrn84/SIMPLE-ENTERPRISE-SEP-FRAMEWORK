using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.Interfaces.Authenticate
{
    internal interface IAuthenticateForm
    {
        event EventHandler SubmitClicked;
        event EventHandler SwitchClicked;

        void ShowForm();
        void HideForm();
        void ShowError(string errorMessage);
    }
}
