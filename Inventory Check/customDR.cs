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
    public partial class customDR : Form
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
        //public customDR()
        //{
        //    InitializeComponent();
        //}
        private DRForm form1;
        private out_supply out_supply;
        public customDR(DRForm form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }
        public customDR(out_supply out_supply)
        {
            InitializeComponent();
            this.out_supply = out_supply;
        }

        private void customDR_Load(object sender, EventArgs e)
        {
            color();

            if (form == "out_supply")
            {
                getinfo();
            }
        }
        public string id;
        private string icode;
        public string createdby;
        private string projectname;
        private string projectcode;
        private string drnumber;
        private string sv;
        private string operationcheck;

        private string typeofp = "DR";
        public void getinfo()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                String query = "SELECT * FROM tblDR WHERE id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, dbDR);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    icode = (rdr["itemcode"].ToString());
                    projectname = (rdr["projectname"].ToString());
                    projectcode = (rdr["projectcode"].ToString());
                    drnumber = (rdr["drnumber"].ToString());
                    sv = (rdr["sv"].ToString());
                    operationcheck = (rdr["operation"].ToString());
                }
                dbDR.Close();
            }

        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label9.ForeColor = Color.White;

                guna2TextBox6.FillColor = Color.FromArgb(15, 14, 15);
                guna2TextBox2.ForeColor = Color.White;


                guna2TextBox2.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox2.ForeColor = Color.White;
                guna2TextBox3.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox3.ForeColor = Color.White;
                guna2TextBox4.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox4.ForeColor = Color.White;
                guna2TextBox5.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox5.ForeColor = Color.White;
                guna2TextBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox1.ForeColor = Color.White;


            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;

                guna2TextBox6.FillColor = Color.FromArgb(243, 243, 243);
                guna2TextBox6.ForeColor = Color.Black;


                guna2TextBox1.FillColor = Color.White;
                guna2TextBox1.ForeColor = Color.Black;
                guna2TextBox2.FillColor = Color.White;
                guna2TextBox2.ForeColor = Color.Black;
                guna2TextBox3.FillColor = Color.White;
                guna2TextBox3.ForeColor = Color.Black;
                guna2TextBox4.FillColor = Color.White;
                guna2TextBox4.ForeColor = Color.Black;
                guna2TextBox5.FillColor = Color.White;
                guna2TextBox5.ForeColor = Color.Black;


            }
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox2.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox3.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox5.Text + ch, out x))
            {
                e.Handled = true;
            }
        }
        private void count()
        {
            decimal sum = 0.00M;
            sum += Convert.ToDecimal(guna2TextBox2.Text) * Convert.ToDecimal(guna2TextBox3.Text);
            guna2TextBox6.Text =  string.Format("{0:#,##0.00}", sum);
        }
        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text != "")
            {
                if (guna2TextBox2.Text != "")
                {
                    count();
                }
                else
                {
                    guna2TextBox6.Text = "0.00";
                }
            }
            else
            {
                guna2TextBox6.Text = "0.00";
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text != "")
            {
                if (guna2TextBox2.Text != "")
                {
                    count();
                }
                else
                {
                    guna2TextBox6.Text = "0.00";
                }
            }
            else
            {
                guna2TextBox6.Text = "0.00";
            }
        }
        public string form;
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "")
            {
                MessageBox.Show("Please fill in all the label");
                return;
            }
            if (guna2TextBox3.Text == "")
            {
                MessageBox.Show("Please fill in all the label");
                return;
            }
            if (guna2TextBox4.Text == "")
            {
                MessageBox.Show("Please fill in all the label");
                return;
            }
            if (guna2TextBox5.Text == "")
            {
                MessageBox.Show("Please fill in all the label");
                return;
            }

            if (form == "out_supply")
            {
                getinfo();
                if (operationcheck == "Completed")
                {
                    drinsertcompleted();
                }
                else
                {
                    drinsertincompleted();
                }

                out_supply.bb = true;
                out_supply.getinfo();
                out_supply.load();
                out_supply.bb = false;
                out_supply.count();
                out_supply.update();

                MessageBox.Show("Inserted");
            }
            else
            {
                int a = form1.dataGridView1.Rows.Add();
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn14"].Value = "";
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn15"].Value = "";
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn16"].Value = guna2TextBox4.Text.Trim();
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn17"].Value = "";
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn18"].Value = guna2TextBox2.Text.Trim();
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn19"].Value = guna2TextBox1.Text.Trim();
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn20"].Value = "0.00";
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn21"].Value = guna2TextBox5.Text.Trim();
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn22"].Value = guna2TextBox3.Text.Trim();
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn23"].Value = "";
                form1.dataGridView1.Rows[a].Cells["dataGridViewTextBoxColumn24"].Value = guna2TextBox6.Text.Trim();
                form1.dataGridView1.Rows[a].Cells["Column1"].Value = "";
                form1.dataGridView1.Rows[a].Cells["Column2"].Value = "custom";
                MessageBox.Show("Added");
            }
            guna2TextBox4.Text = "";
            guna2TextBox2.Text = "0.00";
            guna2TextBox1.Text = "";
            guna2TextBox3.Text = "0.00";
            guna2TextBox5.Text = "0.00";
            guna2TextBox6.Text = "0.00";
        }
        private void drinsertcompleted()
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();

                string insStmt2 = "insert into tblDRitemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[stored],[total],[createdby],[projectcode],[projectname],[drnumber],[sv],[typeofp],[drid]) values" +
                              " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@stored,@total,@createdby,@projectcode,@projectname,@drnumber,@sv,@typeofp,@drid)";

                SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);
                insCmd2.Parameters.AddWithValue("@productcode", "");
                insCmd2.Parameters.AddWithValue("@mfgcode", "");
                insCmd2.Parameters.AddWithValue("@description", guna2TextBox4.Text);
                insCmd2.Parameters.AddWithValue("@unit", guna2TextBox1.Text);
                insCmd2.Parameters.AddWithValue("@qty", guna2TextBox2.Text);
                insCmd2.Parameters.AddWithValue("@icode", icode);
                insCmd2.Parameters.AddWithValue("@iitem", "");
                insCmd2.Parameters.AddWithValue("@stocksleft", "0.00");
                insCmd2.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                insCmd2.Parameters.AddWithValue("@selling", guna2TextBox3.Text);
                insCmd2.Parameters.AddWithValue("@stored", "");
                insCmd2.Parameters.AddWithValue("@total", guna2TextBox6.Text);
                insCmd2.Parameters.AddWithValue("@createdby", createdby);
                insCmd2.Parameters.AddWithValue("@projectcode", projectcode);
                insCmd2.Parameters.AddWithValue("@projectname", projectname);
                insCmd2.Parameters.AddWithValue("@drnumber", drnumber);
                insCmd2.Parameters.AddWithValue("@sv", sv);
                insCmd2.Parameters.AddWithValue("@typeofp", typeofp);
                insCmd2.Parameters.AddWithValue("@drid", id);
                insCmd2.ExecuteNonQuery();
                itemCode.Close();
            }
            string completepo = "Added Custom Item: "+ guna2TextBox4.Text;
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                    " (@name,@date,@operation,@id)";
                SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                insCmd.Parameters.AddWithValue("@name", createdby);
                insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                insCmd.Parameters.AddWithValue("@operation", completepo);
                insCmd.Parameters.AddWithValue("@id", id);
                int affectedRows = insCmd.ExecuteNonQuery();
                dbDR.Close();
            }
        }
        private void drinsertincompleted()
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();

                string insStmt2 = "insert into tblDRitemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[stored],[total],[createdby],[projectcode],[projectname],[drnumber],[sv],[typeofp],[drid]) values" +
                              " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@stored,@total,@createdby,@projectcode,@projectname,@drnumber,@sv,@typeofp,@drid)";

                SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);
                insCmd2.Parameters.AddWithValue("@productcode", "");
                insCmd2.Parameters.AddWithValue("@mfgcode", "");
                insCmd2.Parameters.AddWithValue("@description", guna2TextBox4.Text);
                insCmd2.Parameters.AddWithValue("@unit", guna2TextBox1.Text);
                insCmd2.Parameters.AddWithValue("@qty", guna2TextBox2.Text);
                insCmd2.Parameters.AddWithValue("@icode", icode);
                insCmd2.Parameters.AddWithValue("@iitem", "");
                insCmd2.Parameters.AddWithValue("@stocksleft", "0.00");
                insCmd2.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                insCmd2.Parameters.AddWithValue("@selling", guna2TextBox3.Text);
                insCmd2.Parameters.AddWithValue("@stored", "");
                insCmd2.Parameters.AddWithValue("@total", guna2TextBox6.Text);
                insCmd2.Parameters.AddWithValue("@createdby", createdby);
                insCmd2.Parameters.AddWithValue("@projectcode", projectcode);
                insCmd2.Parameters.AddWithValue("@projectname", projectname);
                insCmd2.Parameters.AddWithValue("@drnumber", drnumber);
                insCmd2.Parameters.AddWithValue("@sv", sv);
                insCmd2.Parameters.AddWithValue("@typeofp", "");
                insCmd2.Parameters.AddWithValue("@drid", "");
                insCmd2.ExecuteNonQuery();
                itemCode.Close();
            }
            string completepo = "Added Custom Item: " + guna2TextBox4.Text;
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                    " (@name,@date,@operation,@id)";
                SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                insCmd.Parameters.AddWithValue("@name", createdby);
                insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                insCmd.Parameters.AddWithValue("@operation", completepo);
                insCmd.Parameters.AddWithValue("@id", id);
                int affectedRows = insCmd.ExecuteNonQuery();
                dbDR.Close();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2TextBox1.Text.Length == 5)
            {
                char ch = e.KeyChar;
                if (ch == (char)Keys.Back)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void guna2TextBox2_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "0.00")
            {
                guna2TextBox2.Text = "";
            }
        }

        private void guna2TextBox2_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "")
            {
                guna2TextBox2.Text = "0.00";
            }
        }

        private void guna2TextBox3_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "0.00")
            {
                guna2TextBox3.Text = "";
            }
        }

        private void guna2TextBox3_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "")
            {
                guna2TextBox3.Text = "0.00";
            }
        }

        private void guna2TextBox5_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text == "0.00")
            {
                guna2TextBox5.Text = "";
            }
        }

        private void guna2TextBox5_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text == "")
            {
                guna2TextBox5.Text = "0.00";
            }
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
