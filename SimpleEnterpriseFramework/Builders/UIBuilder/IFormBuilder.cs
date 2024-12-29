using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Builders.UIBuilder
{
    public interface IFormBuilder
    {
        IFormBuilder SetTitle(string title);
        IFormBuilder AddFormText(TextBox box, string labelName);
        IFormBuilder AddButton(Button button);

        IFormBuilder CreateFLP(string name, int tabIndex);
    }
}
