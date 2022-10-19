using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Inventory_Check
{
    public partial class aAccounts : Form
    {
        public string name;
        public string username;

        private string id;


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
        public aAccounts()
        {
            SuspendLayout();
            InitializeComponent();
            ResizeRedraw = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            ResumeLayout();
        }
        
        private void aAccounts_Load(object sender, EventArgs e)
        {
            color();
            dashboardGLU d = new dashboardGLU();
            d.panel3.Width = this.Width;
            d.panel3.Height = this.Height;
            //SuspendLayout();
            
            load();
            check = "1"; 
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            guna2Button4.Visible = true;
            guna2TextBox2.Text = "";
            guna2TextBox2.ReadOnly = false;
            guna2TextBox1.Text = "";
            guna2TextBox1.ReadOnly = false;
            guna2TextBox3.ReadOnly = false;
            guna2TextBox4.ReadOnly = false;
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            guna2ComboBox2.Text = "";
            guna2CirclePictureBox1.Image = pictureBox8.Image;
            guna2Button3.Visible = false;
            guna2Button5.Visible = false;
            //ResumeLayout();
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label22.ForeColor = Color.White;
                label24.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;

                guna2Panel1.FillColor = Color.FromArgb(36, 37, 37);

                dataGridView2.BackgroundColor = Color.FromArgb(36, 37, 37);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(36, 37, 37);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(36, 37, 37);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.GridColor = Color.FromArgb(36, 37, 37);

                guna2TextBox1.FillColor = Color.FromArgb(36, 37, 37);
                guna2TextBox1.ForeColor = Color.White;
                guna2TextBox2.FillColor = Color.FromArgb(36, 37, 37);
                guna2TextBox2.ForeColor = Color.White;
                guna2TextBox3.FillColor = Color.FromArgb(36, 37, 37);
                guna2TextBox3.ForeColor = Color.White;
                guna2TextBox4.FillColor = Color.FromArgb(36, 37, 37);
                guna2TextBox4.ForeColor = Color.White;
                guna2ComboBox2.FillColor = Color.FromArgb(36, 37, 37);
                guna2ComboBox2.ForeColor = Color.White;

            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label22.ForeColor = Color.Black;
                label24.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;

                guna2Panel1.FillColor = Color.White;

                dataGridView2.BackgroundColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;
                dataGridView2.GridColor = Color.White;

                guna2TextBox1.FillColor = Color.White;
                guna2TextBox1.ForeColor = Color.Black;
                guna2TextBox2.FillColor = Color.White;
                guna2TextBox2.ForeColor = Color.Black;
                guna2TextBox3.FillColor = Color.White;
                guna2TextBox3.ForeColor = Color.Black;
                guna2TextBox4.FillColor = Color.White;
                guna2TextBox4.ForeColor = Color.Black;
                guna2ComboBox2.FillColor = Color.White;
                guna2ComboBox2.ForeColor = Color.Black;
            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        dashboardGLU obj = (dashboardGLU)Application.OpenForms["dashboardGLU"];
        public void load()
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
            tblSupplier.Open();
            DataTable dt = new DataTable();
            string list = "Select id,username,createdby from tblAccount";
            SqlCommand command = new SqlCommand(list, tblSupplier);
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            dataGridView2.DataSource = dt;
            tblSupplier.Close();
            this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Descending);
            tblSupplier.Close();
            tblSupplier.Dispose();
            }
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            guna2CirclePictureBox1.Image = pictureBox8.Image;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

            guna2CirclePictureBox1.Image = pictureBox9.Image;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

            guna2CirclePictureBox1.Image = pictureBox10.Image;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

            guna2CirclePictureBox1.Image = pictureBox11.Image;
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

            guna2CirclePictureBox1.Image = pictureBox12.Image;
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

            guna2CirclePictureBox1.Image = pictureBox13.Image;
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
         
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            
            
        }
        string check;
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
              
            
         
        }
        SqlDataReader rdr;
        private void getprojectcode()
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
               

                tblSupplier.Open();
                DataTable dt = new DataTable();
                String query = "SELECT username FROM tblAccount WHERE username = '" + guna2TextBox1.Text.Replace("'", "''") + "'";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label6.Text = "Username already use";
                    label6.ForeColor = Color.Maroon;
                }
                else
                {
                    label6.Text = "Username Available";
                    label6.ForeColor = Color.LimeGreen;
                }
                tblSupplier.Close();
                tblSupplier.Dispose();
            }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
          
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
         
        }

        private void pictureBox18_Click_1(object sender, EventArgs e)
        {
           
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                label6.Text = "Please Enter Username";
                label6.ForeColor = Color.Maroon;
            }
            else if (check == "1")
            {
                getprojectcode();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg;)|*.jpg; *.jpeg;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                guna2CirclePictureBox1.Image = new Bitmap(open.FileName);
                // image file path  
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to cancel?", "cancel?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                guna2Button4.Visible = true;
                guna2TextBox2.Text = "";
                guna2TextBox2.ReadOnly = false;
                guna2TextBox1.Text = "";
                guna2TextBox1.ReadOnly = false;
                guna2TextBox3.ReadOnly = false;
                guna2TextBox4.ReadOnly = false;
                guna2TextBox3.Text = "";
                guna2TextBox4.Text = "";
                guna2ComboBox2.Text = "";
                guna2CirclePictureBox1.Image = pictureBox8.Image;
                guna2Button3.Visible = false;
                guna2Button5.Visible = false;


            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to update this account?", "Update?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MemoryStream ms = new MemoryStream();
                guna2CirclePictureBox1.Image.Save(ms, guna2CirclePictureBox1.Image.RawFormat);
                byte[] img11 = ms.ToArray();
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblAccount set fullname=@fullname,contactnumber=@contactnumber,type=@type,image=@image where id=@id", tblSupplier);


                    tblSupplier.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", id);
                    //cmd.Parameters.AddWithValue("@username", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@fullname", guna2TextBox3.Text);
                    cmd.Parameters.AddWithValue("@contactnumber", guna2TextBox4.Text);
                    cmd.Parameters.AddWithValue("@type", guna2ComboBox2.Text);
                    cmd.Parameters.Add("@image", SqlDbType.Image).Value = img11;
                    cmd.ExecuteNonQuery();
                    tblSupplier.Close();
                    tblSupplier.Dispose();
                    MessageBox.Show("Account has been updated.");
                    obj.load();
                    guna2Button4.Visible = true;
                    guna2Button3.Visible = false;
                    guna2Button5.Visible = false;
                    guna2TextBox2.Text = "";
                    guna2TextBox2.ReadOnly = false;
                    guna2TextBox1.Text = "";
                    guna2TextBox1.ReadOnly = false;
                    guna2TextBox3.Text = "";
                    guna2TextBox4.Text = "";
                    guna2ComboBox2.Text = "";
                    guna2CirclePictureBox1.Image = pictureBox8.Image;
                    guna2TextBox3.ReadOnly = false;
                    guna2TextBox4.ReadOnly = false;
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "" || guna2TextBox1.Text == "" || guna2TextBox3.Text == "" || guna2TextBox4.Text == "" || guna2ComboBox2.Text == "")
            {
                MessageBox.Show("Please fill in all the details");
            }
            else if (label6.Text == "Username already use")
            {
                MessageBox.Show("Username is used already...");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to save this account?", "Save?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                    {
                        MemoryStream ms = new MemoryStream();
                        guna2CirclePictureBox1.Image.Save(ms, guna2CirclePictureBox1.Image.RawFormat);
                        byte[] img11 = ms.ToArray();


                        tblSupplier.Open();
                        string insStmt = "insert into tblAccount ([username],[password],[fullname],[contactnumber],[type],[image],[createdby]) values (@username,@password,@fullname,@contactnumber,@type,@image,@createdby)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblSupplier);
                        insCmd.Parameters.AddWithValue("@username", guna2TextBox1.Text);
                        insCmd.Parameters.AddWithValue("@password", guna2TextBox2.Text);
                        insCmd.Parameters.AddWithValue("@fullname", guna2TextBox3.Text);
                        insCmd.Parameters.AddWithValue("@contactnumber", guna2TextBox4.Text);
                        insCmd.Parameters.AddWithValue("@type", guna2ComboBox2.Text);
                        insCmd.Parameters.Add("@image", SqlDbType.Image).Value = img11;
                        insCmd.Parameters.AddWithValue("@createdby", name);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblSupplier.Close();
                        tblSupplier.Dispose();
                        MessageBox.Show("Saved");
                        load();
                        guna2TextBox2.Text = "";
                        guna2TextBox1.Text = "";
                        guna2TextBox3.Text = "";
                        guna2TextBox4.Text = "";
                        guna2ComboBox2.Text = "";
                        guna2CirclePictureBox1.Image = pictureBox8.Image;
                        guna2TextBox3.ReadOnly = false;
                        guna2TextBox4.ReadOnly = false;
                    }
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (username == guna2TextBox1.Text)
            {
                DialogResult dialogResult1 = MessageBox.Show("The account is the same from the user that logged in. Are you sure to continue? The app will close after.", "Delete?", MessageBoxButtons.YesNo);
                if (dialogResult1 == DialogResult.Yes)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to delete this account?", "Delete?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                        {

                            tblSupplier.Open();

                            using (SqlCommand command = new SqlCommand("DELETE FROM tblAccount WHERE username = '" + guna2TextBox1.Text + "'", tblSupplier))
                            {
                                command.ExecuteNonQuery();
                            }

                            tblSupplier.Close();
                            tblSupplier.Dispose();
                        }
                        Application.Exit();
                    }
                }
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete this account?", "Delete?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                    {

                        tblSupplier.Open();

                        using (SqlCommand command = new SqlCommand("DELETE FROM tblAccount WHERE username = '" + guna2TextBox1.Text + "'", tblSupplier))
                        {
                            command.ExecuteNonQuery();
                        }
                        tblSupplier.Close();
                        tblSupplier.Dispose();
                        MessageBox.Show("Account has been deleted.");
                        guna2Button4.Visible = true;
                        guna2Button3.Visible = false;
                        guna2Button5.Visible = false;
                        guna2TextBox2.Text = "";
                        guna2TextBox2.ReadOnly = false;
                        guna2TextBox1.Text = "";
                        guna2TextBox1.ReadOnly = false;
                        guna2TextBox3.Text = "";
                        guna2TextBox4.Text = "";
                        guna2ComboBox2.Text = "";
                        guna2CirclePictureBox1.Image = pictureBox8.Image;
                        guna2TextBox3.ReadOnly = false;
                        guna2TextBox4.ReadOnly = false;
                    }
                }
            }

        }

        private void aAccounts_SizeChanged(object sender, EventArgs e)
        {

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void aAccounts_StyleChanged(object sender, EventArgs e)
        {

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                guna2TextBox2.PasswordChar = '\0';
            }
            else
            {
                guna2TextBox2.PasswordChar = '●';
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            check = "2";
            guna2Button4.Visible = false;
            guna2Button3.Visible = true;
            guna2Button5.Visible = true;
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {

                tblSupplier.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblAccount WHERE id = '" + dataGridView2.CurrentRow.Cells[0].Value + "'";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    id = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    guna2TextBox1.Text = (rdr["username"].ToString());
                    guna2TextBox2.Text = (rdr["password"].ToString());
                    guna2TextBox3.Text = (rdr["fullname"].ToString());
                    guna2TextBox4.Text = (rdr["contactnumber"].ToString());
                    guna2ComboBox2.Text = (rdr["type"].ToString());
                    if (rdr["image"] != DBNull.Value)
                    {
                        byte[] img = (byte[])(rdr["image"]);
                        MemoryStream mstream = new MemoryStream(img);
                        guna2CirclePictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                    }
                }
                tblSupplier.Close();
                tblSupplier.Dispose();
            }

            check = "2";
            guna2TextBox2.ReadOnly = true;
            guna2TextBox1.ReadOnly = true;
            guna2TextBox3.ReadOnly = true;
            guna2TextBox4.ReadOnly = true;
            MessageBox.Show("You can't edit the username and password. Only the owner of the account can.");

        }
    }
}
