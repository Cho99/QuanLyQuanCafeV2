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

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            load();
        }

        void load()
        {
            dtgvFood.DataSource = foodList;
            loadDateTime();
            LoadListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            loadListFood();
            AddFoodBinding();
            LoadCategory(cbFoodCategory);
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
          
        }
        #region methods
        void loadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
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
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name"));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price"));
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
            if (dtgvFood.SelectedCells.Count > 0) 
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                Category category = CategoryDAO.Instance.GetCategoryByID(id);
                cbFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                foreach(Category item in cbFoodCategory.Items)
                {
                    if(item.ID == category.ID)
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


    }
}
