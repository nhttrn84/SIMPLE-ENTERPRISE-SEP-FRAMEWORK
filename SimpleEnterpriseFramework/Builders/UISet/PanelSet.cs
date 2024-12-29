using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework.Builders.UIBuilder
{
    class PanelSet
    {
        public TableLayoutPanel CreateTLP(string panelName, List<FormTextField> controls)
        {
            TableLayoutPanel panel = new TableLayoutPanel() { 
                Name = panelName, Dock = DockStyle.Fill, Padding = new Padding(10, 5, 20, 0)};
            panel.RowCount = controls.Count;
            panel.ColumnCount = 2;

            for (int i = 0; i < controls.Count; ++i)
            {
                panel.Controls.Add(controls[i].ControlLabel, 0, i);
                panel.Controls.Add(controls[i].ControlTextBox, 1, i);
            }

            panel.Controls.Add(new Control());
            return panel;
        }
        public Panel CreateFLPanelControls(string panelName, Size size, Point point, int tabIndex, Color backColor)
        {
            Panel panel = new Panel();

            panel.Name = panelName;
            panel.Size = size;
            panel.Location = point;
            panel.TabIndex = tabIndex;
            panel.BackColor = backColor;

            return panel;
        }
    }
}
