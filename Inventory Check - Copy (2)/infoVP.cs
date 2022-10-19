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
    public partial class infoVP : Form
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
        public infoVP()
        {
            InitializeComponent();
        }
        public string poid;
        private void infoVP_Load(object sender, EventArgs e)
        {
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblVP WHERE poid = '" + poid + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    guna2TextBox2.Text = (rdr["vpno"].ToString());
                    if (rdr["vpamount"].ToString() != "")
                    {
                        guna2TextBox1.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["vpamount"].ToString()));
                    }
                    else
                    {
                        guna2TextBox1.Text = rdr["vpamount"].ToString();
                    }
                    guna2TextBox3.Text = (rdr["cvbank"].ToString());
                    guna2TextBox5.Text = (rdr["cvbankno"].ToString());
                    guna2TextBox4.Text = (rdr["cvno"].ToString());
                    maskedTextBox1.Text = (rdr["cvdateissued"].ToString());
                }
                tblIn.Close();
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
