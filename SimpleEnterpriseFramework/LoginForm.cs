﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using SimpleEnterpriseFramework.DBSetting.Membership;
using SimpleEnterpriseFramework.DBSetting;
using SimpleEnterpriseFramework.DBSetting.SQLServer;
using SimpleEnterpriseFramework.DBSetting.Membership.HashPassword;

namespace SimpleEnterpriseFramework
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void textUserName_Enter(object sender, EventArgs e)
        {
            if (txtUsernameLogin.Text == "Account" || txtUsernameLogin.Text == "! Chưa có dữ liệu")
            {
                txtUsernameLogin.Text = "";
                txtUsernameLogin.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textUserName_Leave(object sender, EventArgs e)
        {
            if (txtUsernameLogin.Text == "")
            {
                txtUsernameLogin.Text = "Account";
                txtUsernameLogin.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void textPassword_Enter(object sender, EventArgs e)
        {
            if (txtPasswordLogin.Text == "Password" || txtPasswordLogin.Text == "! Chưa có dữ liệu")
            {
                txtPasswordLogin.Text = "";
                txtPasswordLogin.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textPassword_Leave(object sender, EventArgs e)
        {
            if (txtPasswordLogin.Text == "")
            {
                txtPasswordLogin.Text = "Password";
                txtPasswordLogin.ForeColor = System.Drawing.SystemColors.ScrollBar;
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (txtUsernameLogin.Text == "Account")
            {
                txtUsernameLogin.Text = "! Chưa có dữ liệu";
                txtUsernameLogin.ForeColor = System.Drawing.Color.Red;
            }
            if (txtPasswordLogin.Text == "Password")
            {
                txtPasswordLogin.Text = "! Chưa có dữ liệu";
                txtPasswordLogin.ForeColor = System.Drawing.Color.Red;
            }
            if (txtUsernameLogin.Text != "Account" && txtPasswordLogin.Text != "Password" && txtUsernameLogin.Text != "" && txtPasswordLogin.Text != "" && txtUsernameLogin.Text != "! Chưa có dữ liệu" && txtPasswordLogin.Text != "! Chưa có dữ liệu")
            {
                // Kết nối với MySQL bằng tk và mk đã setup trên MySQL
                using (var connectionHelper = new SQLServer("Data Source=KIMTRINH\\SQLEXPRESS;Database=simple_enterprise_framework;Integrated Security=True;"))
                {
                    if (connectionHelper.OpenConnection())
                    {
                        string account = txtUsernameLogin.Text.Trim();
                        string password = txtPasswordLogin.Text.Trim();

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
                                    this.Hide();
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
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            RegisterForm register = new RegisterForm();
            register.ShowDialog();
        }
    }
}
