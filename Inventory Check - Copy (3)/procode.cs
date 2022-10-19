using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class procode : Form
    {

        private string id;

        public string createdby;
        public string num;
        public string username;
  
        public procode()
        {
            InitializeComponent();

            pictureBox4.InitialImage = null;
            pictureBox5.InitialImage = null;
            pictureBox6.InitialImage = null;
            pictureBox8.InitialImage = null;
            pictureBox9.InitialImage = null;
            pictureBox11.InitialImage = null;
            pictureBox13.InitialImage = null;
            pictureBox14.InitialImage = null;
        }
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

        private void procode_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            load();
            comboBox2.Text = "Project Name";
            comboBox2.Region = new Region(new Rectangle(3, 3, comboBox2.Width - 3, comboBox2.Height - 7));
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {
           
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {

                otherDB.Open();
            DataTable dt = new DataTable();
            string list = "Select ACCTCODE,ACCTDESC,id from GLU4";
            SqlCommand command = new SqlCommand(list, otherDB);
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            dataGridView2.DataSource = dt;
            otherDB.Close();
            otherDB.Dispose();
            }
            this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Descending);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            id = dataGridView2.CurrentRow.Cells[2].Value.ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Project Code")
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTCODE LIKE '{0}'", textBox4.Text.Replace("'", "''"));
            }
            else if (comboBox2.Text == "Project Name")
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTDESC LIKE '{0}%'", textBox4.Text.Replace("'", "''"));
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text == ""||textBox2.Text =="")
            //{

            //    MessageBox.Show("Please enter the a project code!");
            //}
            //else
            //{

            //    otherDB.Open();
            //    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [GLU4] WHERE ([ACCTCODE] = @ACCTCODE)", otherDB);
            //    check_User_Name.Parameters.AddWithValue("@ACCTCODE", textBox1.Text);
            //    int UserExist = (int)check_User_Name.ExecuteScalar();


            //    if (UserExist > 0)
            //    {
            //        pictureBox13.Visible = false;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please enter the an existing project code!");



            //    }
            //    otherDB.Close();
            //}

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
          
            //pictureBox13.Visible = true;
        }

        Form1 obj = (Form1)Application.OpenForms["Form1"];
        DRForm obj1 = (DRForm)Application.OpenForms["DRForm"];
        out_supply obj2 = (out_supply)Application.OpenForms["out_supply"];
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (num =="1")
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select project...");
                }
                else
                {
                    //obj.textBox1.Text = textBox1.Text;
                    //obj.label12.Text = textBox2.Text;
                    this.Close();
                }
            }
            else if(num == "2")
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select project...");
                }
                else
                {
                    obj1.guna2TextBox2.Text = textBox1.Text;
                    obj1.label12.Text = textBox2.Text;
                    this.Close();
                }
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select project...");
                }
                else
                {
                    obj2.guna2TextBox1.Text = textBox1.Text;
                    obj2.label11.Text = textBox2.Text;
                    this.Close();
                }
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {

                MessageBox.Show("Please fill in the details!");
            }
            else
            {
             
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    otherDB.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [GLU4] WHERE ([ACCTCODE] = @ACCTCODE)", otherDB);
                    check_User_Name.Parameters.AddWithValue("@ACCTCODE", textBox1.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    SqlCommand check_User_Name2 = new SqlCommand("SELECT COUNT(*) FROM [GLU4] WHERE ([ACCTDESC] = @ACCTDESC)", otherDB);
                    check_User_Name2.Parameters.AddWithValue("@ACCTDESC", textBox2.Text);
                    int UserExist2 = (int)check_User_Name2.ExecuteScalar();

                    if (UserExist > 0 || UserExist2 > 0)
                    {
                        MessageBox.Show("Existing project code or name!");

                    }
                    else
                    {

                        string a = "A";
                        string insStmt = "insert into GLU4 ([ACCTCODE], [ACCTDESC], [COY], [createdby]) values" +
                            " (@ACCTCODE,@ACCTDESC,@COY,@createdby)";
                        SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@ACCTCODE", textBox1.Text);
                        insCmd.Parameters.AddWithValue("@ACCTDESC", textBox2.Text);
                        insCmd.Parameters.AddWithValue("@COY", a);
                        insCmd.Parameters.AddWithValue("@createdby", createdby);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        MessageBox.Show("Saved");
                        load();
                    }

                    otherDB.Close();
                    otherDB.Dispose();
                }
            }
            
           




        }
        SqlDataReader rdr;

        private void getprojectcode()
        {
           
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                DataTable dt = new DataTable();
                String query = "SELECT ACCTDESC,id FROM GLU4 WHERE ACCTCODE = '" + textBox1.Text.Replace("'", "''") + "'";
                SqlCommand cmd = new SqlCommand(query, otherDB);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    textBox2.Text = (rdr["ACCTDESC"].ToString());
                    id = (rdr["id"].ToString());
                }
                else
                {
                    textBox2.Text = "";
                }
                otherDB.Close();
                otherDB.Dispose();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text !="")
            {
                getprojectcode();
            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text == "" || textBox2.Text == "")
            //{

            //    MessageBox.Show("Please fill in the details!");
            //
            //else
            //{

            //    SqlCommand cmd = new SqlCommand("update tblIn set ACCTCODE=@ACCTCODE,ACCTDESC=@ACCTDESC where id=@id", otherDB);

            //    otherDB.Open();
            //    cmd.Parameters.Clear();
            //    cmd.Parameters.AddWithValue("@id", id);
            //    cmd.Parameters.AddWithValue("@ACCTCODE", textBox1.Text);
            //    cmd.Parameters.AddWithValue("@ACCTDESC", textBox2.Text);
            //    cmd.ExecuteNonQuery();
            //    otherDB.Close();
            //    MessageBox.Show("Updated");
            //}
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
