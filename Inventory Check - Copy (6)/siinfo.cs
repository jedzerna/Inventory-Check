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
    public partial class siinfo : Form
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
        public string poid;
        public siinfo()
        {
            InitializeComponent();
        }

        private void siinfo_Load(object sender, EventArgs e)
        {

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblSI WHERE poid = '" + poid + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label2.Text = (rdr["SIno"].ToString());
                    label6.Text = (rdr["receivedby"].ToString());
                    label8.Text = (rdr["via"].ToString());
                    label3.Text = (rdr["date"].ToString());
                    label10.Text = (rdr["carno"].ToString());
                    label12.Text = (rdr["forwarder"].ToString());
                }
                tblIn.Close();

            }
            load();
        }
        public string itemid;
        public void load()
        {

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                dataGridView1.Rows.Clear();
                itemCode.Open();

                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("Select description,qty,Id,si,charging from itemCode where icode = '" + itemid + "' order by Id asc", itemCode))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells["invoice"].Value = item["si"];
                    dataGridView1.Rows[n].Cells["charging"].Value = item["charging"];
                    dataGridView1.Rows[n].Cells["description"].Value = item["description"];
                    dataGridView1.Rows[n].Cells["qty"].Value = item["qty"];
                    dataGridView1.Rows[n].Cells["id"].Value = item["Id"];
                }
                itemCode.Close();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
