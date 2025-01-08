using Org.BouncyCastle.Asn1.Crmf;
using SimpleEnterpriseFramework.Builders.UIBuilder;
using SimpleEnterpriseFramework.DBSetting;
using SimpleEnterpriseFramework.DBSetting.DAO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace SimpleEnterpriseFramework
{
    public partial class HandleForm : BaseForm
    {
        public enum SaveType
        {
            Insert = 0,
            Update = 1,
        }

        private MainForm mainFormRef;
        SaveType _sType;
        List<FormTextField> _fields = new List<FormTextField>();
        SQLServerDAO sqlServerDAO = new SQLServerDAO();
        string _tableName = "";
        string _databaseName = "";
        List<string> primaryKeyColumns = new List<string>();


        public HandleForm(MainForm mainForm, SaveType type, DataGridViewRow row, string tableName, string databaseName) : base(tableName, "Handle Form", new Size(width: 480, height: 450))
        {
            InitializeComponent();
            this.mainFormRef = mainForm;
            this._tableName = tableName;
            this._sType = type;
            this._databaseName = databaseName;


            Button btnCancel = new ButtonBuilder()
                .Name("btncancel")
                .Text("Cancel")
                .BackgroundColor(Color.FromArgb(255, 255, 255))
                .ContentColor(Color.Black)
                .Position(new Point(130, 208))
                .Size(new Size(116, 4))
                .WithAnchorStyles(AnchorStyles.Right)
                .ClickHandler((sender, e) => { btnCancel_Click(sender, e); })
                .Build();


            Button btnConfirm = new ButtonBuilder()
                .Name("btnConfirm")
                .Text("")
                .BackgroundColor(Color.FromArgb(255, 255, 255))
                .ContentColor(Color.Black)
                .Position(new Point(236, 4))
                .Size(new Size(100, 32))
                .WithAnchorStyles(AnchorStyles.Left)
                .ClickHandler((sender, e) => { btnConfirm_Click(sender, e); })
                .Build();

            buttonLayoutPanel.Controls.Add(btnCancel, 0, 0);
            buttonLayoutPanel.Controls.Add(btnConfirm, 2, 0);
            panelBtn.Controls.Add(buttonLayoutPanel);
            Controls.Add(panelBtn);
            panelBtn.ResumeLayout(false);
            buttonLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);

            primaryKeyColumns = sqlServerDAO.GetPrimaryKeyColumns(tableName);

            if (type == SaveType.Insert)
            {
                Text = "INSERT";
                btnConfirm.Text = "Insert";
            }
            else
            {
                Text = "UPDATE";
                btnConfirm.Text = "Update";
            }

            foreach (DataGridViewColumn col in row.DataGridView.Columns)
            {
                string label = col.HeaderText;
                string value = type == SaveType.Update ? row.Cells[col.Index].Value.ToString() : "";
                FormTextField formTextField = new FormTextField(label, value);
                if(type == SaveType.Update && primaryKeyColumns.Contains(label))
                {
                    formTextField.setDisable();
                }
                _fields.Add(formTextField);
            }

            TableLayoutPanel panelBody = new TableLayoutPanel() { Name = "rowPanel", Dock = DockStyle.Fill, Padding = new Padding(0, 2, 0, 0) };
            panelBody.RowCount = _fields.Count;
            panelBody.ColumnCount = 1;

            for (int i = 0; i < _fields.Count; ++i)
            {
                panelBody.Controls.Add(_fields[i].ControlLabel, 0, i);
                panelBody.Controls.Add(_fields[i].ControlTextBox, 1, i);
            }

            panelBody.Controls.Add(new Control());


            Controls.Add(panelBody);
        }

        public Dictionary<string, string> GetFormDataAsDict()
        {
            return this
                ._fields
                .Aggregate(new Dictionary<string, string>(), (acc, curr) =>
                {
                    acc[curr.LabelText] = curr.Value;
                    return acc;
                });
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> insertDict = this.GetFormDataAsDict();
            if (this._sType == SaveType.Insert)
            {
                sqlServerDAO.InsertData(insertDict, this._tableName, this._databaseName);
            }
            else if (this._sType == SaveType.Update)
            {
                sqlServerDAO.UpdateData(insertDict, this._tableName, this._databaseName);
            }
            this.mainFormRef.ReloadData();
            this.Dispose();
        }
    }
}
