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
                DialogResult dialogResult = MessageBox.Show("Are you sure to update this SI?", "Completion", MessageBoxButtons.YesNo);
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
                        cmd3.Parameters.AddWithValue("@vpamount", guna2TextBox1.Text);
                        cmd3.Parameters.AddWithValue("@cvbank", guna2TextBox3.Text);
                        cmd3.Parameters.AddWithValue("@cvbankno", guna2TextBox5.Text);
                        cmd3.Parameters.AddWithValue("@cvno", guna2TextBox4.Text);
                        cmd3.Parameters.AddWithValue("@cvdateissued", maskedTextBox1.Text);
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
            guna2TextBox1.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox1.Text));
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
    }
}
