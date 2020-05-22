using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyQuanCafe.DTO.Menu;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount {
            get => loginAccount;
            set { loginAccount = value; ChangeAccount(loginAccount.Type); } 
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadCategory();
            LoadTable();
            loadCbTable(cbSwitchTable);
        }

        #region Method
        // Thay Đổi tài khoản
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;                  
                }
                flpTable.Controls.Add(btn);
            }
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodByCategory(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void loadCbTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion
        
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (Menu item in listBillInfo)
            {
                ListViewItem listView = new ListViewItem(item.FoodName.ToString());
                listView.SubItems.Add(item.Count.ToString());
                listView.SubItems.Add(item.Price.ToString());
                listView.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                //Đổ dữ liệu vào bảng lsvBill
                lsvBill.Items.Add(listView);
            }
            //Xét loại tiền tệ
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;

            txt_TotalPrice.Text = totalPrice.ToString("c" , culture);
        }

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int tableId = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableId);
        }

        private void cbCategory_SelectedIndexChange(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if(cb.SelectedItem == null)
            {
                return;
            }
            // Lấy giá trị category từ Combobox cb_Catergoy
            Category selected = cb.SelectedItem as Category;
            id = selected.ID; //Lấy id từ giá trị Category được chọn
            LoadFoodByCategory(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if(table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }
            int idBill = BillDAO.Instance.GetUnCheckBill(table.ID);

            int idFood = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                BillDAO.Instance.createtBill(table.ID);
                //BillDAO.Instance.GetMaxIDBill() : Hàm GetMaxIDBill() là dùng để lấy id lớn nhất của bảng Bill vừa mới tạo
                BillinfoDAO.Instance.createtBillInfo(BillDAO.Instance.GetMaxIDBill(), idFood, count);
            } else
            {
                BillinfoDAO.Instance.createtBillInfo(idBill, idFood, count);
            }
            ShowBill(table.ID);
            LoadTable();
        }


        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAcountProfile f = new fAcountProfile(LoginAccount);
            f.ShowDialog();
        }
        

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertFood += F_InsertFood;
            f.DeletetFood += F_DeletetFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodByCategory((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_DeletetFood(object sender, EventArgs e)
        {
            LoadFoodByCategory((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodByCategory((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void fTableManager_Load(object sender, EventArgs e)
        {

        }

        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBill(table.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txt_TotalPrice.Text.Split(',')[0]); // Tổng tiền chưa giảm giá
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;//Tổng tiền sau khi đã tính với giảm giá


            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có muốn thanh toán hóa đơn cho bàn {0}\n Tổng tiền - (Tổng tiền/100) x Giảm giá \n= {1} - ({1}/100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice ), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount, (float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            string nameTable1 = (lsvBill.Tag as Table).Name;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            string nameTable2 = (cbSwitchTable.SelectedItem as Table).Name;

            if (MessageBox.Show(string.Format("Bạn có thiệt sự muốn chuyển bàn {0} qua bàn {1}", nameTable1, nameTable2), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }           
        }
        #endregion
    }
}
