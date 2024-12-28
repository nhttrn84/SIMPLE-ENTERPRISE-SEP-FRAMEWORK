using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace SimpleEnterpriseFramework.Builders.UIBuilder
{
    public class ButtonBuilder : IButton
    {
        private Button _instance;
        public ButtonBuilder() { 
            _instance = new Button();
            _instance.TabIndex = 0;
            _instance.UseVisualStyleBackColor = false;
            _instance.AutoSize = true;
            _instance.Cursor = Cursors.Hand;   
            _instance.Font = new Font("Arial", 10F, FontStyle.Regular);
        }

        public IButton BackgroundColor(Color backgroundColor)
        {
            _instance.BackColor = backgroundColor;
            return this;
        }

        public IButton ContentColor(Color contentColor)
        {
            _instance.ForeColor = contentColor;
            return this;
        }

        public IButton Name(string name)
        {
            _instance.Name = name;   
            return this;
        }

        public IButton Position(Point pos)
        {
            _instance.Location = pos;
            return this;
        }

        public IButton Size(Size size)
        {
            _instance.Size = size;
            return this;
        }

        public IButton Text(string text)
        {
            _instance.Text = text;
            return this;
        }

        public IButton ClickHandler(EventHandler handler)
        {
            _instance.Click += handler;
            return this;
        }

        public Button Build()
        {
            return _instance;
        }

        public IButton WithMouseEnterEventHandler(EventHandler handler)
        {
            _instance.MouseEnter += handler;
            return this;

        }

        public IButton WithMouseLeaveEventHandler(EventHandler handler)
        {
            _instance.MouseLeave += handler;
            return this;
        }

        public IButton WithAnchorStyles(AnchorStyles anchorStyles)
        {
            _instance.Anchor = anchorStyles;
            return this;
        }

        public IButton WithImage(Image image)
        {
            _instance.Image = image;
            return this;
        }
    }
}
