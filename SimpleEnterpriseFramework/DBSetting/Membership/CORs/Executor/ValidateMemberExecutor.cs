using SimpleEnterpriseFramework.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using SimpleEnterpriseFramework.Interfaces.Authenticate;

namespace SimpleEnterpriseFramework.DBSetting.Membership.CORs.Executor
{
    public class ValidateMemberExecutor : IMembershipExecutor
    {
        IMembershipExecutor _next;
        Form currentScreen;

        public ValidateMemberExecutor(IMembershipExecutor next, Form currentScreen)
        {
            _next = next;
            this.currentScreen = currentScreen;
        }

        public void Execute(List<string> boxValue, List<TextBox> boxInstance)
        {
            //Register logic here:
            if (boxValue.Count > 2)
            {
                // Thực hiện đăng ký tài khoản
                Membership p = new Membership();
                if (p.Register(boxValue[0], boxValue[1]))
                {
                    MessageBox.Show("Đăng ký thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    currentScreen.Hide();
                    IoCContainer.Register<IAuthenticateForm, LoginForm>();
                    IAuthenticateForm login = IoCContainer.Resolve<IAuthenticateForm>();
                    login.ShowForm();
                }
                else
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //Login logic here:
            else
            {
                Membership p = new Membership();
                if (p.Login(boxValue[0], boxValue[1]))
                {
                    MessageBox.Show("Đăng nhập thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Dissapear login form
                    currentScreen.Hide();
                    MainForm main = new MainForm("main form");
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //go next
            if (_next != null)
            {
                _next.Execute(boxValue, boxInstance);
            }
            
        }

        public void SetNext(IMembershipExecutor executor)
        {
            _next = executor;
        }
    }
}
