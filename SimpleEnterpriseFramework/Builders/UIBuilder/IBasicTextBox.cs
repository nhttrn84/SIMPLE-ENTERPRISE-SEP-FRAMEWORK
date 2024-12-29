using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Builders.UIBuilder
{
    public interface IBasicTextBox
    {
        IBasicTextBox Name(string name);
        IBasicTextBox TabIndex(int tabIndex);
        IBasicTextBox TabStop(bool tabStop);
        IBasicTextBox Text(string text);
        IBasicTextBox ContentColor(Color foreColor);
        IBasicTextBox BorderStyle(BorderStyle borderStyle);
        IBasicTextBox Position(Point position);
        IBasicTextBox Size(Size size);
        IBasicTextBox EnterEventHandler(EventHandler handler);
        IBasicTextBox LeaveEventHandler(EventHandler handler);
        IBasicTextBox IsPasswordField(bool isPasswordField);

        TextBox Build();
    }
}
