using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillinfoDAO
    {
        private static BillinfoDAO instance;

        public static BillinfoDAO Instance {
            get {if(instance == null) instance = new BillinfoDAO(); return instance; } 
            private set => instance = value;
        }
        private BillinfoDAO() { }

        public List<Billinfo> GetListBillInfo(int id) 
        {
            List<Billinfo> listBillInfo = new List<Billinfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from BillInfo where idBill = " + id + "");
            
            foreach (DataRow item in data.Rows)
            {
                Billinfo info = new Billinfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void createtBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("exec createBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
