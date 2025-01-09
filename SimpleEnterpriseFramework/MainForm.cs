using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SimpleEnterpriseFramework.Builders.UIBuilder;
using SimpleEnterpriseFramework.DBSetting;
using SimpleEnterpriseFramework.DBSetting.DAO;

namespace SimpleEnterpriseFramework
{
    public partial class MainForm : BaseForm
    {
        private Button btnAddRow, btnEditRow, btnDeleteRow, btnLogout;

        public MainForm(string name) : base(name, "Main Form", new Size(width: 1540, height: 813))
        {
            InitializeComponent();
            InitializeLayout();
            InitializeDatabase();
        }

        private void InitializeLayout()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));

            btnAddRow = new ButtonBuilder()
                .Name("btnAddRow")
                .Text("New Row")
                .BackgroundColor(Color.FromArgb(31, 38, 62))
                .ContentColor(Color.White)
                .Size(new Size(250, 64))
                .ClickHandler((sender, e) => { btnAddRow_Click(sender, e); })
                .WithImage((Image)resources.GetObject("btnAddRow.Image"))
                .Build();

            btnEditRow = new ButtonBuilder()
                .Name("btnEditRow")
                .Text("Edit Row")
                .BackgroundColor(Color.FromArgb(31, 38, 62))
                .ContentColor(Color.White)
                .Size(new Size(250, 64))
                .ClickHandler((sender, e) => { btnEditRow_Click(sender, e); })
                .WithImage((Image)resources.GetObject("btnEditRow.Image"))
                .Build();

            btnDeleteRow = new ButtonBuilder()
                .Name("btnDeleteRow")
                .Text("Delete Row")
                .BackgroundColor(Color.FromArgb(31, 38, 62))
                .ContentColor(Color.White)
                .Size(new Size(250, 64))
                .ClickHandler((sender, e) => { btnDeleteRow_Click(sender, e); })
                .WithMouseEnterEventHandler((sender, e) => { btnDelete_MouseEnter(sender, e); })
                .WithMouseLeaveEventHandler((sender, e) => { btnDelete_MouseLeave(sender, e); })
                .WithImage((Image)resources.GetObject("btnDeleteRow.Image"))
                .Build();

            btnLogout = new ButtonBuilder()
                .Name("btnLogout")
                .Text("Logout")
                .BackgroundColor(Color.FromArgb(31, 38, 62))
                .ContentColor(Color.White)
                .Size(new Size(250, 64))
                .ClickHandler((sender, e) => { btnLogout_Click(sender, e); })
                .WithImage((Image)resources.GetObject("btnLogout.Image"))
                .Build();

            panel3.Controls.Add(btnAddRow);
            panel3.Controls.Add(btnEditRow);
            panel3.Controls.Add(btnDeleteRow);
            panel3.Controls.Add(btnLogout);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        private void InitializeDatabase()
        {
            Database database = new Database();
            List<string> databaseNames = database.GetDatabaseNames();

            // Bind the databaseNames list to the ComboBox
            dbCombobox.DataSource = databaseNames;

            if (databaseNames.Count == 0)
            {
                dbCombobox.Text = "Select database";
            }
        }

        private void InitializeGridView(string database)
        {
            DatabaseInfo.Instance.SetDatabaseName(database);
            _sqlServerDao.UpdateConnectionString(DatabaseInfo.Instance.connectionData);

            List<string> tables = DatabaseInfo.Instance.GetAllTablesName();

            // Bind the databaseNames list to the ComboBox
            tableCombobox.DataSource = tables;

            if (tables.Count == 0)
            {
                tableCombobox.Text = "Select tables";
            }
        }

        private void btnDelete_MouseEnter(object sender, EventArgs e)
        {
            btnDeleteRow.BackColor = Color.Red;
        }

        private void btnDelete_MouseLeave(object sender, EventArgs e)
        {
            btnDeleteRow.BackColor = Color.FromArgb(31, 38, 62);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DatabaseInfo.Instance.ResetDatabaseName();
            this.Hide();
            LoginForm login = new LoginForm();
            login.ShowDialog();
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            new HandleForm(this, HandleForm.SaveType.Insert, this.dataGridView.Rows[0], tableCombobox.SelectedItem.ToString(), dbCombobox.SelectedItem.ToString()).Show();
        }

        private void dbCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dbCombobox.SelectedIndex == -1)
                return;

            string selectedDatabase = dbCombobox.SelectedItem.ToString();
            InitializeGridView(selectedDatabase);
        }

        private void btnEditRow_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = this.dataGridView.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                MessageBox.Show("Chưa có dòng nào được chọn!");
                return;
            }
            new HandleForm(this, HandleForm.SaveType.Update, selectedRows[0], tableCombobox.SelectedItem.ToString(), dbCombobox.SelectedItem.ToString()).Show();

        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = this.dataGridView.SelectedRows;
            if (selectedRows.Count <= 0)
            {
                MessageBox.Show("Chưa có dòng nào được chọn");
                return;
            }
            foreach (DataGridViewRow row in selectedRows)
            {
                Dictionary<string, string> rowData = new Dictionary<string, string>();

                // Iterate through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Add the column name (or index) as the key, and the cell's value as the value
                    rowData[cell.OwningColumn.Name] = cell.Value.ToString();
                }


                Console.WriteLine(row.ToString());

                ReloadData();
            }
        }

        private void tableCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableCombobox.SelectedIndex == -1)
                return;

            string selectedTable = tableCombobox.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                DataTable tableData = _sqlServerDao.LoadData(tableName);

                // Bind the data to the DataGridView
                dataGridView.DataSource = tableData;

                // Auto-generate columns and resize them to fit the data
                dataGridView.AutoGenerateColumns = true;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading table data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ReloadData()
        {
            dataGridView.DataSource = _sqlServerDao.LoadData(tableCombobox.SelectedItem.ToString());
        }
    }
}
