using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set => instance = value;
        }

        private AccountDAO() { }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("UpdateUser @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, pass, newPass});
            return result > 0;
        }

        public bool Login(string username, string password)
        {
            string query = "Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, password });
            return result.Rows.Count > 0;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Account where userName = '" + userName + "'");
            foreach(DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select UserName, DisplayName, type from Account");
        }

        //Thêm Accuont
        public bool insertAccuont(string name, string displayName, int type)
        {
            string query = "Insert Account(UserName, DisplayName, Type) values(N'" + name + "',N'" + displayName + "'," + type + ")";
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        //Sửa Accuont
        public bool updateAccuont(string name, string displayName, int type)
        {
            string query = string.Format("update Account set DisplayName= N'{1}' , Type = {2}  where Username = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        // Xóa Accuont
        public bool deleteAccuont(string userName)
        {   
            string query = string.Format("Delete Account where UserName = N'{0}'", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool resetPassWord(string userName)
        {
            string query = string.Format("Update Account set PassWord = '0' where UserName = N'{0}'", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }


    }
}
