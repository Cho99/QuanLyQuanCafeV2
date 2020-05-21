﻿using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance {
            get { if (instance == null) instance = new BillDAO(); return instance; } 
            private set => instance = value; 
        }

        private BillDAO() { }
        //Thàng công ra bill ID
        //Thất bại ra -1
        public int GetUnCheckBill(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Bill Where idTable = " + id + " And status = 0");

            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        //Tạo Bill mới
        public void createtBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec createBill @idTable", new object[] { id });
        }
        
        // Thanh Toán => Update lại status của bàn
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "Update Bill set DateCheckOut = GETDATE(), status = 1," + "discount =" + discount + ", totalPrice = " + totalPrice + " where id = " + id + "";
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
           return DataProvider.Instance.ExecuteQuery("exec ThongKeHD @checkIn , @checkOut", new object[] {checkIn, checkOut});
        }

        
        //Lất id Bill lớn nhất
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("Select MAX(id) from Bill");
            }
            catch
            {
                return 1;
            }
          
        }
    }
}
