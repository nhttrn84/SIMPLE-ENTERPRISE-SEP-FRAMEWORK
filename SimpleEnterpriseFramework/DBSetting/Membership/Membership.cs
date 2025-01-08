using SimpleEnterpriseFramework.DBSetting.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEnterpriseFramework.DBSetting.Membership
{
    public class Membership
    {
        private AbstractDAO dao;

        public Membership()
        {
            dao = new SQLServerDAO();
            dao.CreateAccountTable();
        }

        public bool Login(string username, string password)
        {
            if (dao.ValidateUser(username, password))
            {
                return true;
            }
            return false;
        }

        public bool Register(string username, string password)
        {
            if (dao.CreateUser(username, password))
            {
                return true;
            }
            return false;
        }

        public bool Logout(string username)
        {
            if (dao.SignOutUser(username))
            {
                return true;
            }
            return false;
        }
    }
}
