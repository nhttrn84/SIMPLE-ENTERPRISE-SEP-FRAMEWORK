using SimpleEnterpriseFramework.Builders.UIBuilder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEnterpriseFramework
{
    public partial class BaseFormBuilder : Form, IFormBuilder
    {
        protected Panel main;

        protected string title;
        protected List<FormTextField> textFields = new List<FormTextField>();
        protected List<Button> buttons = new List<Button>();

        public IFormBuilder SetTitle(string new_title)
        {
            title = new_title;
            return this;
        }

        public IFormBuilder AddFormText(TextBox box, string labelName)
        {
            FormTextField textField = new FormTextField
            {
                ControlLabel = new Label { Text = labelName },
                ControlTextBox = box
            };
            textFields.Add(textField);
            return this;
        }


        public Panel Build()
        {
            PanelSet panelSet = new PanelSet();
            
            main = panelSet.CreateFLPanelControls("main", new Size(468, 452), new Point(335, 0), 1, SystemColors.ButtonHighlight);


            Label titleLabel = new Label()
            {
                Name = "titleLabel",
                Text = "Login Form", // Nội dung của Title
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top, // Đặt Dock để luôn nằm trên cùng
                Height = 50 // Chiều cao của Label
            };
            main.Controls.Add(titleLabel);

            TableLayoutPanel tlb = panelSet.CreateTLP("main", textFields);
            tlb.Padding = new Padding(10, 20, 10, 20);
            tlb.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; // Tạo viền ô (nếu muốn)
            main.Controls.Add(tlb);

            // Tạo FlowLayoutPanel cho các nút
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel()
            {
                Name = "buttonPanel",
                Dock = DockStyle.Bottom,
                AutoSize = true,
                BackColor = SystemColors.Control
            };

            foreach (var button in buttons)
            {// Tạo khoảng cách giữa các nút
                buttonPanel.Controls.Add(button);
            }

            main.Controls.Add(buttonPanel);

            // Sắp xếp thứ tự các phần tử
            main.Controls.SetChildIndex(titleLabel, 3); // Tiêu đề trên cùng
            main.Controls.SetChildIndex(tlb, 0);       // Nội dung ở giữa Nút ở dưới

            return main;
        }

        public IFormBuilder AddButton(Button button)
        {
            buttons.Add(button);
            return this;
        }

        public IFormBuilder CreateFLP(string name, int tabIndex)
        {
            throw new NotImplementedException();
        }
    }
}
