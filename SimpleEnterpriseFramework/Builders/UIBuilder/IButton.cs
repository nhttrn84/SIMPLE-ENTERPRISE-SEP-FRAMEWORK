using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Builders.UIBuilder
{
    public interface IButton
    {
        IButton Name(String name);
        IButton Text(string text);
        IButton BackgroundColor(Color backgroundColor);
        IButton ContentColor(Color contentColor);
        IButton Position(Point pos);
        IButton Size(Size size);
        IButton ClickHandler(EventHandler handler);
        IButton WithMouseEnterEventHandler(EventHandler handler);
        IButton WithMouseLeaveEventHandler(EventHandler handler);
        IButton WithAnchorStyles(AnchorStyles anchorStyles);
        IButton WithImage(Image image);

        Button Build();
    }
}
