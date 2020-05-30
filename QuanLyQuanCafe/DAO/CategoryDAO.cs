using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance {
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set => instance = value; 
        }

        public CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> listItem = new List<Category>();

            string query = "select * from FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listItem.Add(category);
            }

            return listItem;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "select * from FoodCategory Where id =" + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }

            return category;
        }

        // Lấy danh sách Category
        public DataTable GetCategory()
        {
            return DataProvider.Instance.ExecuteQuery("select * from FoodCategory");
        }

        //Thêm Bàn ăn
        public bool insertCategory(string name)
        {
            string query = "insert FoodCategory(name) values(N'" + name + "')";
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        //Sửa Bàn ăn
        public bool updateCategory(int id, string name)
        {
            string query = string.Format("update FoodCategory set name = N'{1}' where id = {0}", id, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        // Xóa bàn ăn
        public bool deleteCategory(int id)
        {
            string query = string.Format("Delete FoodCategory where id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
