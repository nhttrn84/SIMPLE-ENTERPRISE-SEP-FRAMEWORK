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
            // Create the main panel to hold everything
            Panel mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.ButtonHighlight
            };

            // Create a TableLayoutPanel to center-align the content
            TableLayoutPanel centerPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 1,
                BackColor = SystemColors.ButtonHighlight
            };
            centerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            centerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainPanel.Controls.Add(centerPanel);

            // Create a FlowLayoutPanel (StackPanel1) for the overall layout
            FlowLayoutPanel stackPanel1 = new FlowLayoutPanel()
            {
                Name = "stackPanel1",
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Padding = new Padding(20),
                BackColor = SystemColors.ControlLight
            };

            // Add the title to StackPanel1
            Label titleLabel = new Label()
            {
                Name = "titleLabel",
                Text = title ?? "Default Title",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true
            };
            stackPanel1.Controls.Add(titleLabel);

            // Create StackPanel2 for form text fields
            FlowLayoutPanel stackPanel2 = new FlowLayoutPanel()
            {
                Name = "stackPanel2",
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Padding = new Padding(10),
                BackColor = SystemColors.ControlLight
            };

            // Add form text fields to StackPanel2
            foreach (var textField in textFields)
            {
                stackPanel2.Controls.Add(textField.ControlLabel);
                stackPanel2.Controls.Add(textField.ControlTextBox);
            }
            stackPanel1.Controls.Add(stackPanel2);

            // Create StackPanel3 for buttons
            FlowLayoutPanel stackPanel3 = new FlowLayoutPanel()
            {
                Name = "stackPanel3",
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Padding = new Padding(10),
                BackColor = SystemColors.ControlDark
            };

            // Add buttons to StackPanel3
            foreach (var button in buttons)
            {
                stackPanel3.Controls.Add(button);
            }
            stackPanel1.Controls.Add(stackPanel3);

            // Add StackPanel1 to the TableLayoutPanel and center it
            centerPanel.Controls.Add(stackPanel1);
            centerPanel.SetColumn(stackPanel1, 0);
            centerPanel.SetRow(stackPanel1, 0);

            return mainPanel;
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