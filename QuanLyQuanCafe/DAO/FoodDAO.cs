using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set => instance = value;
        }

        public FoodDAO() { }

        // Lấy danh sạc thức ăn bằng category
        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();
            string query = "select * from Food where idCategory = " + id + "";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }

        // Lấy danh sách thức ăn
        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "select * from Food ";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        //Thêm thức ăn
        public bool insertFood(string name, int id, float price)
        {
            string query = "Insert Food(name, idCategory, price) values(N'"+name+"',"+id+","+price+")";
            int result =  DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        //Sửa Thức ăn
        public bool updateFood(int id,string name, int idCategory, float price)
        {
            string query = string.Format("update Food set name = N'{1}', idCategory={2} , price = {3}  where id = {0}", id,name,idCategory,price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        // Xóa món ăn
        public bool deleteFood(int id)
        {
            BillinfoDAO.Instance.DeleteBillInfoByFoodID(id);
            string query = string.Format("Delete Food where id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
