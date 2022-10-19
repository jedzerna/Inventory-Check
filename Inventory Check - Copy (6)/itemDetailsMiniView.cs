using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class itemDetailsMiniView : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleparam = base.CreateParams;
                handleparam.ExStyle |= 0x02000000;
                return handleparam;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DoubleBuffered = true;
        }
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        public string id;
        public string type;
        public itemDetailsMiniView()
        {
            InitializeComponent();
        }

        private void itemDetailsMiniView_Load(object sender, EventArgs e)
        {


        }
        public void getinfo()
        {

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblIn WHERE Id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                SqlDataReader rdr = cmd.ExecuteReader();

                //if (rdr.Read())
                //{
                //    label1.Text = (rdr["ponumber"].ToString());
                //    label13.Text = (rdr["suppliername"].ToString());
                //    DateTime date = DateTime.Parse(rdr["datetime"].ToString());
                //    label20.Text = date.ToString("MM/dd/yyyy");
                //    guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
                //    label17.Text = (rdr["qty"].ToString());
                //    label18.Text = (rdr["totalitems"].ToString());
                //    guna2NumericUpDown2.Value = Convert.ToDecimal(rdr["totalamount"]);
                //    label19.Text = (rdr["createdby"].ToString());
                //    operationcheck = (rdr["operation"].ToString());
                //    label25.Text = (rdr["purchasecompletedby"].ToString());
                //    icode = (rdr["itemcode"].ToString());
                //    poid = (rdr["Id"].ToString());
                //}
                tblIn.Close();

            }
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblSI WHERE poid = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                SqlDataReader rdr = cmd.ExecuteReader();

                //if (rdr.Read())
                //{
                //    if (rdr["SIno"].ToString() == "" || rdr["SIno"] == null || rdr["SIno"] == DBNull.Value)
                //    {
                //        label9.Text = "None";
                //        label9.ForeColor = Color.LightCoral;
                //        pictureBox4.Visible = false;
                //    }
                //    else
                //    {
                //        label9.Text = (rdr["SIno"].ToString());
                //        label9.ForeColor = Color.White;
                //        pictureBox4.Visible = true;
                //    }
                //}
                //else
                //{
                //    label9.Text = "None";
                //    label9.ForeColor = Color.LightCoral;
                //    pictureBox4.Visible = false;
                //}
                tblIn.Close();

            }
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblVP WHERE poid = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                SqlDataReader rdr = cmd.ExecuteReader();

                //if (rdr.Read())
                //{
                //    if (rdr["vpno"].ToString() == "" || rdr["vpno"] == null || rdr["vpno"] == DBNull.Value)
                //    {
                //        label31.Text = "None";
                //        label29.Text = "0.00";
                //        label31.ForeColor = Color.LightCoral;
                //        pictureBox7.Visible = false;
                //    }
                //    else
                //    {
                //        label31.Text = (rdr["vpno"].ToString());
                //        label29.Text = (rdr["vpamount"].ToString());
                //        label31.ForeColor = Color.White;
                //        pictureBox7.Visible = true;
                //    }
                //}
                //else
                //{
                //    label31.Text = "None";
                //    label29.Text = "0.00";
                //    label31.ForeColor = Color.LightCoral;
                //    pictureBox7.Visible = false;
                //}
                tblIn.Close();

            }

        }
    }
}
