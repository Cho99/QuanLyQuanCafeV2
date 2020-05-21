using QuanLyQuanCafe.DAO;
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
        public fAdmin()
        {
            InitializeComponent();
            loadDateTime();
            LoadListByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
          
        }
        #region methods
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
        #endregion

        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            //dtpkFromDate.Value: Ngày bắt đầu
            //dtpkToDate.Value: Ngày kết thúc
            LoadListByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        #endregion

    }
}
