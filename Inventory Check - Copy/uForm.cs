using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Inventory_Check
{
    public partial class uForm : Form
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
        SqlCommand cmd;
        //public string username = "w";
        //private string id = "5";
        public string username;
        private string id;
        static string connectionstring4;
    
        SqlDataReader rdr;
        public uForm()
        {
            InitializeComponent();
            pictureBox5.InitialImage = null;
            guna2CirclePictureBox1.InitialImage = null;
            pictureBox8.InitialImage = null;
            pictureBox9.InitialImage = null;
            pictureBox10.InitialImage = null;
            pictureBox11.InitialImage = null;
            pictureBox12.InitialImage = null;
            pictureBox13.InitialImage = null;
        }

        private void uForm_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            load();
            guna2ShadowPanel2.Visible = false;
            guna2ShadowPanel1.Visible = true;
            ResumeLayout();
            load2();
        }
        public void load2()
        {


            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                DataTable dt = new DataTable();
                string list = "SELECT TOP 50 dateandtime,computername FROM accounthistory WHERE userid = '"+id+"' ORDER BY id DESC";
                SqlCommand command = new SqlCommand(list, tblSupplier);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                tblSupplier.Close();
            }
        }
        public void load3()
        {


            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                DataTable dt = new DataTable();
                string list = "SELECT dateandtime,computername FROM accounthistory WHERE userid = '" + id + "' ORDER BY id DESC";
                SqlCommand command = new SqlCommand(list, tblSupplier);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                tblSupplier.Close();
            }
        }
        private string password;
        public void load()
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblAccount WHERE username = '" + username + "'";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    id = (rdr["id"].ToString());
                    guna2TextBox1.Text = (rdr["username"].ToString());
                    password = (rdr["password"].ToString());
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
                rdr.Close();
                tblSupplier.Close();
                tblSupplier.Dispose();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
          
        }
        dashboardGLU obj = (dashboardGLU)Application.OpenForms["dashboardGLU"];
        private void pictureBox15_Click(object sender, EventArgs e)
        {
           
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void get()
        {
            connectionstring4 = ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString;
            SqlConnection tblSupplier = new SqlConnection(connectionstring4);

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
            rdr.Close();
            tblSupplier.Close();
            tblSupplier.Dispose();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
         
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

        private void pictureBox17_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                label6.Text = "Please Enter Username";
                label6.ForeColor = Color.Maroon;
            }
            else if (username != guna2TextBox1.Text)
            {
                get();
            }
            else if (username == guna2TextBox1.Text)
            {
                label6.Text = "Username is the same";
                label6.ForeColor = Color.LimeGreen;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                SqlCommand check_User_Name1 = new SqlCommand("SELECT COUNT(*) FROM [tblAccount] WHERE ([username] = @username)", tblSupplier);
                check_User_Name1.Parameters.AddWithValue("@username", guna2TextBox1.Text);
                int UserExist1 = (int)check_User_Name1.ExecuteScalar();

                tblSupplier.Close();

                if (UserExist1 >= 1 && label6.Text != "Username is the same")
                {
                    MessageBox.Show("Username exist");
                }
                else
                {
                    if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "")
                    {
                        MessageBox.Show("Please enter a valid username/password");
                    }
                    else
                    {
                        if (guna2TextBox5.Text != password)
                        {
                            MessageBox.Show("Wrong password");
                        }
                        else
                        {
                            DialogResult dialogResult1 = MessageBox.Show("Are you sure to update?", "Update?", MessageBoxButtons.YesNo);
                            if (dialogResult1 == DialogResult.Yes)
                            {

                                MemoryStream ms = new MemoryStream();
                                guna2CirclePictureBox1.Image.Save(ms, guna2CirclePictureBox1.Image.RawFormat);
                                byte[] img11 = ms.ToArray();

                                tblSupplier.Open();

                                cmd = new SqlCommand("update tblAccount set username=@username,password=@password,fullname=@fullname,contactnumber=@contactnumber,image=@image where id=@id", tblSupplier);

                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@username", guna2TextBox1.Text);
                                cmd.Parameters.AddWithValue("@password", guna2TextBox2.Text);
                                cmd.Parameters.AddWithValue("@fullname", guna2TextBox3.Text);
                                cmd.Parameters.AddWithValue("@contactnumber", guna2TextBox4.Text);
                                cmd.Parameters.AddWithValue("@image", SqlDbType.Image).Value = img11;
                                cmd.ExecuteNonQuery();

                                tblSupplier.Close();
                                obj.load();
                                MessageBox.Show("Account Updated");

                            }
                        }
                    }
                }
            }
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
            guna2TextBox2.Text = "";
            guna2TextBox1.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            load();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            guna2ShadowPanel2.Visible = false;
            guna2ShadowPanel1.Visible = true;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            load2();
            guna2ShadowPanel2.Visible = true;
            guna2ShadowPanel1.Visible = false;
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                load3();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                load2();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
