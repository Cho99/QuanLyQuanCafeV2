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
    public partial class fReport : Form
    {
        private DateTime checkIn;
        private DateTime checkOut;
        public static string con = "Data Source=GL-522VJ\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        public fReport()
        {
            InitializeComponent();
        }

        public fReport(DateTime checkIn, DateTime checkOut): this()
        {
            this.checkIn = checkIn;
            this.checkOut = checkOut;
        }

        private void fReport_Load(object sender, EventArgs e)
        {
            CrystalReport1 crystal = new CrystalReport1();
            SqlConnection connect = new SqlConnection(con);
            connect.Open();
            SqlCommand cmd = new SqlCommand("ThongKeHDReport", connect);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@checkIn", SqlDbType.DateTime).Value = checkIn;
            cmd.Parameters.Add("@checkOut", SqlDbType.DateTime).Value = checkOut;
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
