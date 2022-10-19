using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class addvp : Form
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
        public string name;
        public addvp()
        {
            InitializeComponent();
        }
        public string f;

        private void addvp_Load(object sender, EventArgs e)
        {


            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblVP] WHERE ([poid] = @poid)", tblIn);
                check_User_Name.Parameters.AddWithValue("@poid", poid);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist <= 0)
                {
                        string insStmt = "insert into tblVP ([poid], [vpno]) values" +
                            " (@poid,@vpno)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@poid", poid);
                        insCmd.Parameters.AddWithValue("@vpno", "");
                        int affectedRows = insCmd.ExecuteNonQuery();
                }
                tblIn.Close();
            }


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
                    if (rdr["cvdateissued"].ToString()!= "")
                    {
                        maskedTextBox1.Text = DateTime.Parse(rdr["cvdateissued"].ToString()).ToString("MM/dd/yyyy");
                    }
                }
                tblIn.Close();
            }
            Auto();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = DateTime.Parse(maskedTextBox1.Text);
            }
            catch
            {
                MessageBox.Show("Invalid Date");
                return;
            }
            if (guna2TextBox2.Text == "" || guna2TextBox1.Text == "" || guna2TextBox3.Text == "" || guna2TextBox4.Text == "" || guna2TextBox5.Text == "" || maskedTextBox1.Text == "")
            {
                MessageBox.Show("Please enter all the blank text box!!!");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to update this VP?", "Update", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string completepo = "Added VP No: " + guna2TextBox2.Text;
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                            " (@name,@date,@operation,@id)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@date", f.ToString());
                        insCmd.Parameters.AddWithValue("@operation", completepo);
                        insCmd.Parameters.AddWithValue("@id", poid);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblIn.Close();
                        tblIn.Dispose();
                    }
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        SqlCommand cmd3 = new SqlCommand("update tblIn set vp=@vp where Id=@Id", tblIn);
                        tblIn.Open();
                        cmd3.Parameters.AddWithValue("@Id", poid);
                        cmd3.Parameters.AddWithValue("@vp", guna2TextBox2.Text);
                        cmd3.ExecuteNonQuery();
                        tblIn.Close();
                        tblIn.Dispose();
                    }
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        SqlCommand cmd3 = new SqlCommand("update tblVP set vpno=@vpno,vpamount=@vpamount,cvbank=@cvbank,cvbankno=@cvbankno,cvno=@cvno,cvdateissued=@cvdateissued where poid=@poid", tblIn);
                        tblIn.Open();
                        cmd3.Parameters.AddWithValue("@poid", poid);
                        cmd3.Parameters.AddWithValue("@vpno", guna2TextBox2.Text);
                        cmd3.Parameters.AddWithValue("@vpamount", string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox1.Text)));
                        cmd3.Parameters.AddWithValue("@cvbank", guna2TextBox3.Text);
                        cmd3.Parameters.AddWithValue("@cvbankno", guna2TextBox5.Text);
                        cmd3.Parameters.AddWithValue("@cvno", guna2TextBox4.Text);
                        cmd3.Parameters.AddWithValue("@cvdateissued", DateTime.Parse(maskedTextBox1.Text).ToString("MM/dd/yyyy"));
                        cmd3.ExecuteNonQuery();
                        tblIn.Close();
                        tblIn.Dispose();
                    }

                    obj.getinfo();
                    obj.load();
                    MessageBox.Show("Updated");
                    this.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            f = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "")
            {
                guna2TextBox1.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox1.Text));
            }
        }

        private void guna2TextBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {

            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox1.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {

        }
        AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
        AutoCompleteStringCollection coll2 = new AutoCompleteStringCollection();
        public void Auto()

        {
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select distinct cvbankno from tblVP", tblIn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i]["cvbankno"].ToString());
                    }
                }
                tblIn.Close();
                guna2TextBox5.AutoCompleteMode = AutoCompleteMode.Suggest;
                guna2TextBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;
                guna2TextBox5.AutoCompleteCustomSource = coll;
            }

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select distinct cvbank from tblVP", tblIn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll2.Add(dt.Rows[i]["cvbank"].ToString());
                    }
                }
                tblIn.Close();
                guna2TextBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
                guna2TextBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                guna2TextBox3.AutoCompleteCustomSource = coll2;
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"(\\|-|\.)");
            string FormattedDate = rgx.Replace(maskedTextBox1.Text, @"/");

            // Separate the date components as delimited by standard mm/dd/yyyy formatting:
            string[] dateComponents = FormattedDate.Split('/');
            string month = dateComponents[0].Trim(); ;
            string day = dateComponents[1].Trim();
            string year = dateComponents[2].Trim();

            // We require a two-digit month. If there is only one digit, add a leading zero:
            if (month.Length == 1)
            {
                month = "0" + month;
            }

            // We require a two-digit day. If there is only one digit, add a leading zero:

            if (day.Length == 1)
            {
                day = "0" + day;
            }

            // We require a four-digit year. If there are only two digits, add 
            // two digits denoting the current century as leading numerals:
            if (year.Length == 2)
            {
                year = "20" + year;
            }

            // Put the date back together again with proper delimiters, and 
            maskedTextBox1.Text = month + "/" + day + "/" + year;
        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox6_Leave(object sender, EventArgs e)
        {
        }

        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
        
        }
    }
}
