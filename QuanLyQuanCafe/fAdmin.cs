using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyQuanCafe
{

    public partial class fAdmin : Form
    {
        public static string con = "Data Source=GL-522VJ\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        BindingSource foodList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            load();
        }

      

        private void fAdmin_Load(object sender, EventArgs e)
        {
          
        }
        #region methods
        void load()
        {
            dtgvFood.DataSource = foodList;
            loadDateTime();
            LoadListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            loadListFood();
            AddFoodBinding();
            LoadCategory(cbFoodCategory);
        }

        void searchFoodByName(string name)
        {
            SqlConnection connect = new SqlConnection(con);
            connect.Open();
            SqlCommand cmd = new SqlCommand("select f.id, f.name, c.id as CategoryID ,c.name as category , f.price from Food as f, FoodCategory as c where f.idCategory = c.id and f.name like N'%"+name+"%'", connect);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            foodList.DataSource = dataSet.Tables[0];
            cmd.Dispose();
            connect.Close();
        }
        void loadListFood()
        {
            //foodList.DataSource = FoodDAO.Instance.GetListFood();
            SqlConnection connect = new SqlConnection(con);
            connect.Open();
            SqlCommand cmd = new SqlCommand("select f.id, f.name, c.id as CategoryID ,c.name as category , f.price from Food as f, FoodCategory as c where f.idCategory = c.id", connect);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            foodList.DataSource = dataSet.Tables[0];
            cmd.Dispose();
            connect.Close();
        }

        // Load thời gian hiển thị của bảng thống kê
        void loadDateTime()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListByDate(DateTime checkIn, DateTime checkOut)
        { 
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price",true ,DataSourceUpdateMode.Never));
            //cbFoodCategory.DataSource.Add( )
        }
        void LoadCategory(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        #endregion

        #region events
        //Thống kê theo ngày
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            //dtpkFromDate.Value: Ngày bắt đầu
            //dtpkToDate.Value: Ngày kết thúc
            LoadListByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        // Xem đồ ăn
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            loadListFood();
        }

        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            
            if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value != null)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                Category category = CategoryDAO.Instance.GetCategoryByID(id);
                cbFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                foreach (Category item in cbFoodCategory.Items)
                {
                    if (item.ID == category.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cbFoodCategory.SelectedIndex = index;
            }
            
        }

        #endregion

        #region events
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.insertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món ăn thành công");
                loadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }

            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.updateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món ăn thành công");
                loadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.deleteFood(id))
            {
                MessageBox.Show("Xóa món ăn thành công");
                loadListFood();
                if(deletetFood != null)
                {
                    deletetFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deletetFood;
        public event EventHandler DeletetFood
        {
            add { deletetFood += value; }
            remove { deletetFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
             searchFoodByName(txbSearchFoodName.Text);
        }

        #endregion

        private void txbSearchFoodName_TextChanged(object sender, EventArgs e)
        {

        }

      
    }
}
