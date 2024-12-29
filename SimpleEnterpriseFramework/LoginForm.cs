using System;
using System.Collections.Generic;

using System.Drawing;

using System.Windows.Forms;
using SimpleEnterpriseFramework.DBSetting.SQLServer;
using SimpleEnterpriseFramework.DBSetting.Membership.HashPassword;
using SimpleEnterpriseFramework.Builders.UIBuilder;
using SimpleEnterpriseFramework.DBSetting.Membership.CORs;
using SimpleEnterpriseFramework.DBSetting.Membership.CORs.Executor;

namespace SimpleEnterpriseFramework
{
    public partial class LoginForm : BaseForm
    {
        private TextBox usernameTextBox, passwordTextBox;
        private Button loginButton, registerButton;


        public LoginForm(string name) : base(name, "Login Form", new Size(width: 800, height: 480))
        {
            
            InitializeComponent();

            BaseFormBuilder builder = new BaseFormBuilder();
            builder.SetTitle("Login");

            loginButton = new ButtonBuilder()
                .Name("btnLogin")
                .Text("Login")
                .BackgroundColor(Color.Black)
                .ContentColor(Color.White)
                .Size(new Size(140, 45))
                .ClickHandler((sender, e) => login_Click(sender, e))
                .Build();

            registerButton = new ButtonBuilder()
                .Name("btnRegister")
                .Text("Register")
                .BackgroundColor(Color.Black)
                .ContentColor(Color.White)
                .Size(new Size(140, 45))
                .ClickHandler((sender, e) => register_Click(sender, e))
                .Build();

            usernameTextBox = new BasicTextBoxBuilder()
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

            passwordTextBox = new BasicTextBoxBuilder()
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

            builder.AddFormText(usernameTextBox, "Username");
            builder.AddFormText(passwordTextBox, "Password");
            builder.AddButton(loginButton);
            builder.AddButton(registerButton);

            // Create a container panel to center the form
            Panel container = new Panel
            {
                Dock = DockStyle.None,
                Size = new Size(370, 285),
                BackColor = Color.LightGray
            };

            // Center the container within the form
            container.Location = new Point((this.ClientSize.Width - container.Width) / 2,
                                           (this.ClientSize.Height - container.Height) / 2);

            container.Anchor = AnchorStyles.None;

            // Add the built form to the container
            container.Controls.Add(builder.Build());


            //Unhide password
            passwordTextBox.UseSystemPasswordChar = false;

            SuspendLayout();
            this.Controls.Clear();
            this.Controls.Add(container);
            ResumeLayout(false);
        }

        public LoginForm() : this("Login")
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
        // Gọi sự kiện đăng nhập khi người dùng nhấn nút đăng nhập
        public void SetTables(List<string> tables)
        {
 
        }

        private void textUserName_Enter(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "Account" || usernameTextBox.Text == "Empty field")
            {
                usernameTextBox.Text = "";
                usernameTextBox.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textUserName_Leave(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "")
            {
                usernameTextBox.Text = "Account";
                usernameTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void textPassword_Enter(object sender, EventArgs e)
        {
            passwordTextBox.UseSystemPasswordChar = true;
            if (passwordTextBox.Text == "Password" || passwordTextBox.Text == "Empty field")
            {
                passwordTextBox.Text = "";
                passwordTextBox.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textPassword_Leave(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == "")
            {
                passwordTextBox.Text = "Password";
                passwordTextBox.UseSystemPasswordChar = false;
                passwordTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            IMembershipExecutor _executor = new EmptyFieldExecutor(new ValidateMemberExecutor(null, this));

            _executor.Execute(
                new List<string>
                {
                    usernameTextBox.Text, passwordTextBox.Text,
                },
                new List<TextBox>
                {
                    usernameTextBox, passwordTextBox
                }
            );
            //Old code below (commented)

            /*
            if (usernameTextBox.Text != "Account" && passwordTextBox.Text != "Password" && usernameTextBox.Text != "" && passwordTextBox.Text != "" && usernameTextBox.Text != "Empty field" && passwordTextBox.Text != "Empty field")
            {
                // Kết nối với MySQL bằng tk và mk đã setup trên MySQL
                using (var connectionHelper = new SQLServer("Data Source=KIMTRINH\\SQLEXPRESS;Database=simple_enterprise_framework;Integrated Security=True;"))
                {
                    if (connectionHelper.OpenConnection())
                    {
                        string account = usernameTextBox.Text.Trim();
                        string password = passwordTextBox.Text.Trim();

                        try
                        {
                            // Bắt đầu lấy dữ liệu và thực hiện kiểm tra để đăng nhập
                            using (var command = connectionHelper.GetConnection().CreateCommand())
                            {
                                string query = "SELECT Password FROM member WHERE username = @Username";
                                command.CommandText = query;

                                command.Parameters.AddWithValue("@Username", account);

                                // Lấy mật khẩu băm từ cơ sở dữ liệu
                                string hashedPasswordFromDB = command.ExecuteScalar()?.ToString();

                                if (!string.IsNullOrEmpty(hashedPasswordFromDB) && HashPassword.hashPassword(password) == hashedPasswordFromDB)
                                {
                                    MessageBox.Show("Đăng nhập thành công");
                                    HideForm();
                                    MainForm main = new MainForm();
                                    main.ShowDialog();
                                }
                                else
                                {
                                    MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại tài khoản và mật khẩu.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                        }
                        finally
                        {
                            // Đóng kết nối sau khi thực hiện xong
                            connectionHelper.CloseConnection();
                        }
                    }
                }
            } */
        }

        private void register_Click(object sender, EventArgs e)
        {
            HideForm();
            RegisterForm register = new RegisterForm();
            register.ShowDialog();
        }

    }
}
