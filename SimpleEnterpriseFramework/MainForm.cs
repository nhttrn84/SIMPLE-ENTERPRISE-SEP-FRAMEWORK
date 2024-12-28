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
            Database database = new Database();
            List<string> databaseNames = database.GetDatabaseNames();
            for (int i = 0; i < databaseNames.Count; i++)
            {
                Console.WriteLine(databaseNames[i]);
            }

            // Bind the databaseNames list to the ComboBox
            dbCombobox.DataSource = databaseNames;

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

        }
    }
}
