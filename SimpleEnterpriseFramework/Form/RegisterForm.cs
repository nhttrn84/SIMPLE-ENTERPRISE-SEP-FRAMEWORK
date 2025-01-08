using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleEnterpriseFramework.Builders.UIBuilder;
using SimpleEnterpriseFramework.DBSetting.Membership.CORs.Executor;
using SimpleEnterpriseFramework.DBSetting.Membership.CORs;
using SimpleEnterpriseFramework.DBSetting.Membership.HashPassword;
using SimpleEnterpriseFramework.Interfaces.Authenticate;

namespace SimpleEnterpriseFramework
{
    public partial class RegisterForm : BaseForm, IAuthenticateForm
    {
        private TextBox usernameTextBoxReg, passwordTextBoxReg, confirmPasswordTextBoxReg;
        private Button loginButtonReg, registerButtonReg;
        public event EventHandler SubmitClicked;
        public event EventHandler SwitchClicked;
        public RegisterForm() : this("Register")
        {

        }
        public RegisterForm(string name) : base(name, "Register Form", new Size(width: 800, height: 480))
        {
            InitializeComponent();
            BaseFormBuilder builder = new BaseFormBuilder();
            builder.SetTitle("Register");

            loginButtonReg = new ButtonBuilder()
                .Name("btnLogin")
                .Text("Login")
                .BackgroundColor(Color.Black)
                .ContentColor(Color.White)
                .Size(new Size(140, 45))
                .ClickHandler((sender, e) => login_Click(sender, e))
                .Build();

            registerButtonReg = new ButtonBuilder()
                .Name("btnRegister")
                .Text("Register")
                .BackgroundColor(Color.Black)
                .ContentColor(Color.White)
                .Size(new Size(140, 45))
                .ClickHandler((sender, e) => register_Click(sender, e))
                .Build();

            usernameTextBoxReg = new BasicTextBoxBuilder()
                .Name("usernameTextBox")
                .Text("")
                .TabIndex(9)
                .TabStop(true)
                .ContentColor(SystemColors.InfoText)
                .BorderStyle(BorderStyle.FixedSingle)
                .Size(new Size(306, 20))
                .EnterEventHandler((sender, e) => { textUserName_Enter(sender, e); })
                .LeaveEventHandler((sender, e) => { textUserName_Leave(sender, e); })
                .Build();

            passwordTextBoxReg = new BasicTextBoxBuilder()
                .Name("passwordTextBox")
                .Text("Password")
                .TabIndex(12)
                .TabStop(false)
                .IsPasswordField(true)
                .ContentColor(SystemColors.ScrollBar)
                .BorderStyle(BorderStyle.FixedSingle)
                .Size(new Size(306, 20))
                .EnterEventHandler((sender, e) => { textPassword_Enter(sender, e); })
                .LeaveEventHandler((sender, e) => { textPassword_Leave(sender, e); })
                .Build();

            confirmPasswordTextBoxReg = new BasicTextBoxBuilder()
                .Name("confirmPasswordTextBox")
                .Text("RePassword")
                .TabIndex(12)
                .TabStop(false)
                .IsPasswordField(true)
                .ContentColor(SystemColors.ScrollBar)
                .BorderStyle(BorderStyle.FixedSingle)
                .Size(new Size(306, 20))
                .EnterEventHandler((sender, e) => { textRePassword_Enter(sender, e); })
                .LeaveEventHandler((sender, e) => { textRePassword_Leave(sender, e); })
                .Build();

            builder.AddFormText(usernameTextBoxReg, "Username");
            builder.AddFormText(passwordTextBoxReg, "Password");
            builder.AddFormText(confirmPasswordTextBoxReg, "Confirm Password");
            builder.AddButton(registerButtonReg);
            builder.AddButton(loginButtonReg);
           

            // Create a container panel to center the form
            Panel container = new Panel
            {
                Dock = DockStyle.None,
                Size = new Size(370, 350),
                BackColor = Color.LightGray
            };

            // Center the container within the form
            container.Location = new Point((this.ClientSize.Width - container.Width) / 2,
                                           (this.ClientSize.Height - container.Height) / 2);

            container.Anchor = AnchorStyles.None;

            // Add the built form to the container
            container.Controls.Add(builder.Build());

            //Unhide password
            passwordTextBoxReg.UseSystemPasswordChar = false;
            confirmPasswordTextBoxReg.UseSystemPasswordChar = false;

            SuspendLayout();
            this.Controls.Clear();
            this.Controls.Add(container);
            ResumeLayout(false);
        }
        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        public void ShowForm()
        {
            this.Show();
 
        }

        public void HideForm()
        {
            this.Hide();
 
        }

        public void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
 
        }

        private void textUserName_Enter(object sender, EventArgs e)
        {
            if (usernameTextBoxReg.Text == "Account" || usernameTextBoxReg.Text == "Empty field")
            {
                usernameTextBoxReg.Text = "";
                usernameTextBoxReg.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textUserName_Leave(object sender, EventArgs e)
        {
            if (usernameTextBoxReg.Text == "")
            {
                usernameTextBoxReg.Text = "Account";
                usernameTextBoxReg.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void textPassword_Enter(object sender, EventArgs e)
        {
            passwordTextBoxReg.UseSystemPasswordChar = true;
            if (passwordTextBoxReg.Text == "Password" || passwordTextBoxReg.Text == "Empty field")
            {
                passwordTextBoxReg.Text = "";
                passwordTextBoxReg.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textPassword_Leave(object sender, EventArgs e)
        {
            if (passwordTextBoxReg.Text == "")
            {
                passwordTextBoxReg.Text = "Password";
                passwordTextBoxReg.UseSystemPasswordChar = false;
                passwordTextBoxReg.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void textRePassword_Enter(object sender, EventArgs e)
        {
            confirmPasswordTextBoxReg.UseSystemPasswordChar = true;
            if (confirmPasswordTextBoxReg.Text == "RePassword" || confirmPasswordTextBoxReg.Text == "Empty field" || confirmPasswordTextBoxReg.Text == "Password mismatch")
            {
                confirmPasswordTextBoxReg.Text = "";
                confirmPasswordTextBoxReg.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textRePassword_Leave(object sender, EventArgs e)
        {
            if (confirmPasswordTextBoxReg.Text == "")
            {
                confirmPasswordTextBoxReg.Text = "RePassword";
                confirmPasswordTextBoxReg.UseSystemPasswordChar = false;
                confirmPasswordTextBoxReg.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void OnLoginClicked()
        {
            SwitchClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnRegisterClicked()
        {
            SubmitClicked?.Invoke(this, EventArgs.Empty);
        }

        private void login_Click(object sender, EventArgs e)
        {
            HideForm();
            LoginForm login = new LoginForm();
            login.ShowDialog();
        }

        private void register_Click(object sender, EventArgs e)
        {
            IMembershipExecutor _executor = new EmptyFieldExecutor(
                new PasswordMatchExecutor(new ValidateMemberExecutor(null, this)));

            _executor.Execute(
                new List<string>
                {
                    usernameTextBoxReg.Text, passwordTextBoxReg.Text, confirmPasswordTextBoxReg.Text
                },
                new List<TextBox>
                {
                    usernameTextBoxReg, passwordTextBoxReg, confirmPasswordTextBoxReg
                }
            );
        }

    }
}
