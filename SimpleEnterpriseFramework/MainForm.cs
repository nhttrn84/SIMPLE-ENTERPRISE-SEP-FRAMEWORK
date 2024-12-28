using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleEnterpriseFramework.DBSetting;
using SimpleEnterpriseFramework.DBSetting.SQLServer;

namespace SimpleEnterpriseFramework
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            List<string> tables = SingletonDatabase.getInstance().GetAllTablesName();
            for (int i = 0; i < tables.Count; i++)
            {
                Console.WriteLine(tables[i]);
            }

            // Bind the databaseNames list to the ComboBox
            dbCombobox.DataSource = tables;

            // Optional: Set a prompt text at the start
            dbCombobox.SelectedIndex = -1; // Clears selection
            dbCombobox.Text = "Select table";
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
            this.Hide();
            LoginForm login = new LoginForm();
            login.ShowDialog();
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            
        }

        private void dbCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dbCombobox.SelectedIndex == -1)
                return;
            Console.WriteLine("hello");

            string selectedTable = dbCombobox.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                // Get the connection string from the SingletonDatabase
                string connString = SingletonDatabase.getInstance().connString;

                // Use SqlServerProcessor to fetch table data
                SqlServerProcessor processor = new SqlServerProcessor(connString);
                string query = $"SELECT * FROM {tableName}";
                DataTable tableData = processor.GetAllData(query);
                Console.WriteLine(tableData.ToString());

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
    }
}
