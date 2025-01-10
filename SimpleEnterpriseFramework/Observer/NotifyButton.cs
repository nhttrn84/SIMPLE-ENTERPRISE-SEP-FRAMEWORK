using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Observer
{
    internal class NotifyButton : IObserver
    {
        private readonly Button _button;

        public NotifyButton(Button button)
        {
            _button = button;
        }

        public void Update()
        {
            // Enable the button based on the condition
            _button.Enabled = true;
        }
    }
}
