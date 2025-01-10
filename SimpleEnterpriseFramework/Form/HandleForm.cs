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
using SimpleEnterpriseFramework.Observer;
using SimpleEnterpriseFramework.Strategy;

namespace SimpleEnterpriseFramework
{
    public partial class HandleForm : BaseForm
    {
        public enum SaveType
        {
            Insert = 0,
            Update = 1,
            Delete = 3,
        }

        private MainForm mainFormRef;
        SaveType _sType;
        List<ObservableFormTextField> _fields = new List<ObservableFormTextField>();
        SQLServerDAO sqlServerDAO = new SQLServerDAO();
        string _tableName = "";
        string _databaseName = "";
        HashSet<string> excludedColumns = new HashSet<string>();
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
            btnConfirm.Enabled = false;

            buttonLayoutPanel.Controls.Add(btnCancel, 0, 0);
            buttonLayoutPanel.Controls.Add(btnConfirm, 2, 0);
            panelBtn.Controls.Add(buttonLayoutPanel);
            Controls.Add(panelBtn);
            panelBtn.ResumeLayout(false);
            buttonLayoutPanel.ResumeLayout(false);
            ResumeLayout(false);

            excludedColumns = sqlServerDAO.GetExcludedColumns(tableName);
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
                string columnName = col.HeaderText; // Get the column name (header text)
                string initialValue = type == SaveType.Update
                    ? row.Cells[col.Index].Value?.ToString() ?? ""
                    : ""; // Get the value for updates or leave empty for new entries

                // Skip columns that are in excludedColumns, but NOT in primaryKeyColumns and type is Insert
                if (excludedColumns.Contains(columnName))
                {
                    if (type == SaveType.Insert)
                        continue; // Skip the column if it's in excludedColumns and type is Insert

                    // If type is Update, ensure excluded column is added regardless of being in primaryKeyColumns
                }

                // Create a form text field for the column
                FormTextField formTextField = new FormTextField(columnName, initialValue);

                // Disable the field if it's a primary key during updates
                if (type == SaveType.Update && primaryKeyColumns.Contains(columnName))
                {
                    formTextField.setDisable();
                }

                var observableField = new ObservableFormTextField(formTextField);
                observableField.Attach(new NotifyButton(btnConfirm));


                // Add the form text field to the collection
                _fields.Add(observableField);
            }

            TableLayoutPanel panelBody = new TableLayoutPanel() { Name = "rowPanel", Dock = DockStyle.Fill, Padding = new Padding(0, 2, 0, 0) };
            panelBody.RowCount = _fields.Count;
            panelBody.ColumnCount = 1;
            panelBody.AutoScroll = true;

            for (int i = 0; i < _fields.Count; ++i)
            {
                panelBody.Controls.Add(_fields[i].GetFormTextField().ControlLabel, 0, i);
                panelBody.Controls.Add(_fields[i].GetFormTextField().ControlTextBox, 1, i);
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
                    acc[curr.GetFormTextField().LabelText] = curr.GetFormTextField().Value;
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
            StrategyFactory.GetInstance().GetStrategy(_sType).HandleData(sqlServerDAO, insertDict, _tableName, _databaseName);
            this.mainFormRef.ReloadData();
            this.Dispose();
        }
    }
}
