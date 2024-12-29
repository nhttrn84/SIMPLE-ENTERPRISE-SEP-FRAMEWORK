using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Builders.UIBuilder
{
    public class BasicTextBoxBuilder : IBasicTextBox
    {
        private TextBox _instance;
        public BasicTextBoxBuilder()
        {
            _instance = new TextBox();
            _instance.AutoSize = true;
            _instance.Font = new Font("Arial", 10F, FontStyle.Regular);
        }

        public IBasicTextBox BorderStyle(BorderStyle borderStyle)
        {
            _instance.BorderStyle = borderStyle;
            return this;
        }

        public TextBox Build()
        {
            return _instance;
        }

        public IBasicTextBox ContentColor(Color foreColor)
        {
            _instance.ForeColor = foreColor;
            return this;
        }

        public IBasicTextBox EnterEventHandler(EventHandler handler)
        {
            _instance.Enter += handler;
            return this;
        }

        public IBasicTextBox IsPasswordField(bool isPasswordField)
        {
            _instance.UseSystemPasswordChar = isPasswordField;
            return this;
        }

        public IBasicTextBox LeaveEventHandler(EventHandler handler)
        {
            _instance.Leave += handler;
            return this;
        }

        public IBasicTextBox Name(string name)
        {
            _instance.Name = name;
            return this;
        }

        public IBasicTextBox Position(Point position)
        {
            _instance.Location = position;
            return this;
        }

        public IBasicTextBox Size(Size size)
        {
            _instance.Size = size;
            return this;
        }

        public IBasicTextBox TabIndex(int tabIndex)
        {
            _instance.TabIndex = tabIndex;
            return this;
        }

        public IBasicTextBox TabStop(bool tabStop)
        {
            _instance.TabStop = tabStop;
            return this;
        }

        public IBasicTextBox Text(string text)
        {
            _instance.Text = text;
            return this;
        }
    }
}
