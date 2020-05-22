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
        BindingSource accountList = new BindingSource();

        public Account loginAccount;
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
            // Load Lại dữ liệu khi thay đổi
            dtgvFood.DataSource = foodList;
            dtgvAcount.DataSource = accountList;

            //Phần load dành cho bên Thống kê
            loadDateTime();
            LoadListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            //Phần load dành cho bên food
            loadListFood();
            AddFoodBinding();
            LoadCategory(cbFoodCategory);

            //Phần load dành cho bên Accuont
            loadListAccount();
            AddAcountBinding();
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
        
        void AddAcountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAcount.DataSource, "UserName", true , DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAcount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            txbType.DataBindings.Add(new Binding("Text", dtgvAcount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        // Lấy dữ liệu tài khoản
        void loadListAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if(AccountDAO.Instance.insertAccuont(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
            loadListAccount();
        }

        void UpdateAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.updateAccuont(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
            loadListAccount();
        }

        void DeleteAccount(string userName)
        {
            if(loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng không xóa tài khoản đang đăng nhập ^^!");
                return;
            }
            if (AccountDAO.Instance.deleteAccuont(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
            loadListAccount();
        }

        void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.resetPassWord(userName))
            {
                MessageBox.Show("Reset Mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Lỗi");
            }
        }

        // lấy dữ liệu món ăn
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


        private void btnShowAcount_Click(object sender, EventArgs e)
        {
            loadListAccount();
        }

        private void btnAddAcount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = Convert.ToInt32(txbType.Text);
            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAcount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            DeleteAccount(userName);
        }

        private void btnEditAcount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = Convert.ToInt32(txbType.Text);
            UpdateAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            ResetPassword(userName);
        }

        #endregion


        private void txbSearchFoodName_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
