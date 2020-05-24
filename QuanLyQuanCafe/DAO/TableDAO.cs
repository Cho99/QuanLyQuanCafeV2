using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class TableDAO
    {
        private static TableDAO instance;

        internal static TableDAO Instance {
            get {if(instance == null) instance = new TableDAO()  ;return instance; } 
            private set => instance = value;
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90;

        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("ChuyenBan @idTable1 , @idTable2", new object[] { id1, id2 });
        }

        public void MergeTable(int id1 , int id2)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_MergeTable @firstBill , @secondBill", new object[] { id1, id2 });
        }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("Get_Table");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }

        public DataTable GetListTable()
        {
            return DataProvider.Instance.ExecuteQuery("select * from TableFood");
        }

        //Thêm Bàn ăn
        public bool insertTable(string name)
        {
            string query = "insert TableFood(name) values(N'"+name+"')";
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        //Sửa Bàn ăn
        public bool updateTable(int id, string name)
        {
            string query = string.Format("update TableFood set name = N'{1}' where id = {0}", id, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        // Xóa bàn ăn
        public bool deleteTable(int id)
        {
            string query = string.Format("Delete TableFood where id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
