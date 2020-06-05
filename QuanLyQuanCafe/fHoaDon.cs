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
    public partial class fHoaDon : Form
    {
        private int idBill;
        public static string con = "Data Source=GL-522VJ\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        public fHoaDon()
        {
            InitializeComponent();
        }

        public fHoaDon(int idBill): this()
        {
            this.idBill = idBill;
        }

        private void fHoaDon_Load(object sender, EventArgs e)
        {
            CrystalReport2 crystal = new CrystalReport2();
            SqlConnection connect = new SqlConnection(con);
            connect.Open();
            SqlCommand cmd = new SqlCommand("HoaDon", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = idBill;
            cmd.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            cmd.Dispose();
            connect.Close();
            crystal.SetDataSource(ds.Tables[0]);
            crp.ReportSource = crystal;
        }
    }
}
