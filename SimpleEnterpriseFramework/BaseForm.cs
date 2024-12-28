using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SimpleEnterpriseFramework
{
    public class BaseForm: Form
    {
        private static readonly Size _min = new Size(width: 600, height: 400);
        protected Panel main;
        protected string title;
        private BaseForm()
        {
            MinimumSize = _min;
        }

        protected BaseForm(string name, string title, Size size) : this()
        {
            Name = name;
            Controls.Clear();
            Controls.AddRange(new Control[] { this.main });
            Size = size;
        }
    }
}
