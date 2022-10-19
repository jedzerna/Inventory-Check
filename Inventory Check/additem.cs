using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class additem : Form
    {
        public string name;
        public string number;
        private string date;
        public string form;
        public DataTable dt = new DataTable();

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

        public additem()
        {
            InitializeComponent();
        }

        private void additem_Load(object sender, EventArgs e)
        {
            color();
            SuspendLayout();
            pictureBox11.InitialImage = null;
            num = "00";
            loadcombo();
            load2();

            load();
            timer2.Start();
            guna2TextBox1.Focus();
            ResumeLayout();

            //int max = Convert.ToInt32(dt.AsEnumerable().Max(row => row["column_Name"]));
            this.WindowState = FormWindowState.Maximized;

            //DataView view = dt.DefaultView;
            //view.Sort = "ID";
            //DataTable sortedTable = view.ToTable();
            //int min = sortedTable.Rows[0].Field<int>("ID");
            //int max = sortedTable.Rows[sortedTable.Rows.Count - 1].Field<int>("ID");

            //MessageBox.Show(min.ToString());
            //MessageBox.Show(max.ToString());
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label12.ForeColor = Color.White;
                label13.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                label16.ForeColor = Color.White;
                label21.ForeColor = Color.White;
                label22.ForeColor = Color.White;

                guna2ShadowPanel1.FillColor = Color.FromArgb(34, 35, 35);

                dataGridView3.BackgroundColor = Color.FromArgb(15, 14, 15);
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.White;

                guna2TextBox1.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox1.ForeColor = Color.White;
                guna2TextBox2.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox2.ForeColor = Color.White;
                guna2TextBox3.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox3.ForeColor = Color.White;
                guna2TextBox4.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox4.ForeColor = Color.White;



                guna2TextBox5.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox5.ForeColor = Color.White;
                guna2TextBox7.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox7.ForeColor = Color.White;
                guna2TextBox6.FillColor = Color.FromArgb(56, 56, 57);
                guna2TextBox6.ForeColor = Color.White;

                guna2CheckBox1.ForeColor = Color.White;


                guna2ComboBox1.FillColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox1.BorderColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox1.ForeColor = Color.White;
                guna2ComboBox1.ItemsAppearance.BackColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox1.ItemsAppearance.ForeColor = Color.White;
                guna2ComboBox2.FillColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox2.BorderColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox2.ForeColor = Color.White;
                guna2ComboBox2.ItemsAppearance.BackColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox2.ItemsAppearance.ForeColor = Color.White;
                guna2ComboBox3.FillColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox3.BorderColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox3.ForeColor = Color.White;
                guna2ComboBox3.ItemsAppearance.BackColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox3.ItemsAppearance.ForeColor = Color.White;
                guna2ComboBox4.FillColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox4.BorderColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox4.ForeColor = Color.White;
                guna2ComboBox4.ItemsAppearance.BackColor = Color.FromArgb(56, 56, 57);
                guna2ComboBox4.ItemsAppearance.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                label12.ForeColor = Color.Black;
                label13.ForeColor = Color.Black;
                label14.ForeColor = Color.Black;
                label16.ForeColor = Color.Black;
                label21.ForeColor = Color.Black;
                label22.ForeColor = Color.Black;

                guna2ShadowPanel1.FillColor = Color.White;

                dataGridView3.BackgroundColor = Color.FromArgb(243, 243, 243);
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 243, 243);
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(243, 243, 243);
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.Black;


                guna2TextBox1.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox1.ForeColor = Color.Black;
                guna2TextBox2.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox2.ForeColor = Color.Black;
                guna2TextBox3.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox3.ForeColor = Color.Black;
                guna2TextBox4.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox4.ForeColor = Color.Black;



                guna2TextBox5.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox5.ForeColor = Color.Black;
                guna2TextBox7.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox7.ForeColor = Color.Black;
                guna2TextBox6.FillColor = Color.FromArgb(226, 226, 227);
                guna2TextBox6.ForeColor = Color.Black;

                guna2CheckBox1.ForeColor = Color.Black;


                guna2ComboBox1.FillColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox1.BorderColor = Color.White;
                guna2ComboBox1.ForeColor = Color.Black;
                guna2ComboBox1.ItemsAppearance.BackColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox1.ItemsAppearance.ForeColor = Color.Black;
                guna2ComboBox2.FillColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox2.BorderColor = Color.White;
                guna2ComboBox2.ForeColor = Color.Black;
                guna2ComboBox2.ItemsAppearance.BackColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox2.ItemsAppearance.ForeColor = Color.Black;
                guna2ComboBox3.FillColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox3.BorderColor = Color.White;
                guna2ComboBox3.ForeColor = Color.Black;
                guna2ComboBox3.ItemsAppearance.BackColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox3.ItemsAppearance.ForeColor = Color.Black;
                guna2ComboBox4.FillColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox4.BorderColor = Color.White;
                guna2ComboBox4.ForeColor = Color.Black;
                guna2ComboBox4.ItemsAppearance.BackColor = Color.FromArgb(226, 226, 227);
                guna2ComboBox4.ItemsAppearance.ForeColor = Color.Black;
            }
        }
        private void load2()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                dt.Rows.Clear();
                codeMaterial.Open();
                string list = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial ORDER BY ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                codeMaterial.Close();

            }
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
        }
        int rowscount;
        string lb = "{";
        string rb = "}";
        private string num;
        //int pl = 0;
        private int id = 0;
        //public void random()
        //{
        //    Random rnd2 = new Random();
        //    int month1 = rnd2.Next(1, 9999999);  
        //    randoml = month1.ToString() ;
        //}
        //string randoml;
        public void load()
        {
            if (guna2CheckBox1.Checked == true)
            {
                guna2TextBox4.ReadOnly = false;
            }
            else
            {
                guna2TextBox4.ReadOnly = true;
                id = 0;

                //MessageBox.Show(pl.ToString());
                if (guna2ComboBox1.Text == "")
                {
                    guna2TextBox4.Text = "0000";
                    label2.Text = "0000";

                }
                else
                {
                    if (guna2ComboBox2.Text == "")
                    {
                        guna2TextBox4.Text = guna2ComboBox1.SelectedValue + "0000";
                        label2.Text = guna2ComboBox1.SelectedValue + "0000";

                    }
                    else
                    {
                        if (guna2ComboBox4.Text == "")
                        {
                            guna2TextBox4.Text = guna2ComboBox1.SelectedValue.ToString() + guna2ComboBox2.SelectedValue.ToString().PadLeft(2, '0') + "00";
                            label2.Text = guna2ComboBox1.SelectedValue.ToString() + guna2ComboBox2.SelectedValue.ToString().PadLeft(2, '0') + "00";

                        }
                        else
                        {
                            guna2TextBox4.Text = guna2ComboBox1.SelectedValue.ToString() + guna2ComboBox2.SelectedValue.ToString().PadLeft(2, '0') + guna2ComboBox4.SelectedValue.ToString().PadLeft(2, '0');
                            label2.Text = guna2ComboBox1.SelectedValue.ToString() + guna2ComboBox2.SelectedValue.ToString().PadLeft(2, '0') + guna2ComboBox4.SelectedValue.ToString().PadLeft(2, '0');

                        }
                    }

                }

            }


    }
    public void loadcombo()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable d = new DataTable();
                string Query = "SELECT category,value FROM tblCategory ORDER BY category ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    d.Load(myReader);
                }
                otherDB.Close();
                guna2ComboBox1.DataSource = d;
                guna2ComboBox1.ValueMember = "value";
                guna2ComboBox1.DisplayMember = "category";
            }
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {

        }

        itemsleft obj = (itemsleft)Application.OpenForms["itemsleft"];
        Form1 obj1 = (Form1)Application.OpenForms["Form1"];
        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {


        }

        private void pictureBox14_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox14_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
             && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            date = d.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadsubtype();
            load();
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadtype();
            load();
            if (guna2ComboBox2.SelectedValue != null || guna2ComboBox2.Text != "")
            {
                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            category c = new category();
            c.ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView3.DataSource as DataTable).DefaultView.RowFilter = string.Format("description LIKE '%{0}%'", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
        }

        private void guna2NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        public string forms;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text == "")
            {
                MessageBox.Show("Please enter a product code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (label3.Text == "Product code already exist")
            {
                return;
            }
            if (guna2ComboBox1.Text == "")
            {
                MessageBox.Show("Please select category of the item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (guna2ComboBox3.Text == "")
            {
                MessageBox.Show("Please select where to store the item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                load();
                load2();
                //DataRow[] foundAuthors = dt.Select("description = '" + guna2TextBox1.Text + "'");
                //if (foundAuthors.Length != 0)
                //{
                //    label21.Text = "Item Name Exist";
                //    label21.Visible = true;
                //}
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [codeMaterial] WHERE ([description] = @description)", codeMaterial);
                    check_User_Name.Parameters.AddWithValue("@description", guna2TextBox1.Text.Trim());
                    int UserExist = (int)check_User_Name.ExecuteScalar();
                    if (UserExist > 0)
                    {
                        label21.Text = "Item Name Exist";
                        label21.Visible = true;
                        label21.ForeColor = Color.Maroon;
                        return;
                    }
                    codeMaterial.Close();
                }

                label21.Text = "Exist";
                label21.Visible = false;
                if (guna2TextBox1.Text == "" || guna2TextBox7.Text == "" || guna2TextBox5.Text == "" || guna2TextBox6.Text == "")
                {
                    MessageBox.Show("Please fill in all the required details that has asterisk", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (guna2TextBox1.Text == "" && guna2TextBox1.Text.Length >= 3)
                {
                    MessageBox.Show("Please enter description with 3 minimum letters...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (label21.Text == "Item Name Exist")
                {
                    MessageBox.Show("Please enter another item name...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int ids = 0;
                    string fpcode = "";
                    Cursor.Current = Cursors.WaitCursor;
                    //load();
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        codeMaterial.Open();
                        string insStmt = "insert into codeMaterial ([product_code], [mfg_code], [description], [stocksleft], [cost], [selling], [type], [unit], [createdby], [dept], [category], [subcategory], [remarks]) values" +
                            " (@product_code,@mfg_code,@description,@stocksleft,@cost,@selling,@type,@unit,@createdby,@dept,@category,@subcategory,@remarks)";
                        SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@product_code", guna2TextBox4.Text);
                        insCmd.Parameters.AddWithValue("@mfg_code", "");
                        insCmd.Parameters.AddWithValue("@description", guna2TextBox1.Text.Trim());
                        insCmd.Parameters.AddWithValue("@stocksleft", guna2TextBox7.Text);
                        insCmd.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                        insCmd.Parameters.AddWithValue("@selling", guna2TextBox6.Text);
                        insCmd.Parameters.AddWithValue("@type", guna2ComboBox4.Text);
                        insCmd.Parameters.AddWithValue("@unit", guna2TextBox3.Text);
                        insCmd.Parameters.AddWithValue("@createdby", name);
                        insCmd.Parameters.AddWithValue("@dept", guna2ComboBox3.Text);
                        insCmd.Parameters.AddWithValue("@category", guna2ComboBox1.Text);
                        insCmd.Parameters.AddWithValue("@subcategory", guna2ComboBox2.Text);
                        insCmd.Parameters.AddWithValue("@remarks", guna2TextBox2.Text);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        codeMaterial.Close();


                        var empcon = new SqlCommand("SELECT max(ID) FROM [codeMaterial]", codeMaterial);
                        codeMaterial.Open();
                        Int32 max = (Int32)empcon.ExecuteScalar();
                        ids = max;
                        codeMaterial.Close();

                         fpcode = guna2TextBox4.Text +"-"+ ids.ToString().PadLeft(6, '0');

                        SqlCommand cmd = new SqlCommand("update codeMaterial set product_code=@product_code where ID= '" + ids.ToString() + "'", codeMaterial);
                        codeMaterial.Open();
                        cmd.Parameters.AddWithValue("@product_code", fpcode);
                        cmd.ExecuteNonQuery();
                        codeMaterial.Close();


                        codeMaterial.Open();
                        string insStmt2 = "insert into codeMaterialHistory ([product_code], [mfg_code], [description], [stocksleft], [cost], [selling], [type], [unit], [date], [modifiedby], [remarks]) values" +
                            " (@product_code,@mfg_code,@description,@stocksleft,@cost,@selling,@type,@unit,@date,@modifiedby,@remarks)";
                        SqlCommand insCmd2 = new SqlCommand(insStmt2, codeMaterial);
                        insCmd2.Parameters.AddWithValue("@product_code", fpcode);
                        insCmd2.Parameters.AddWithValue("@mfg_code", "");
                        insCmd2.Parameters.AddWithValue("@description", "Added " + guna2TextBox1.Text.Trim());
                        insCmd2.Parameters.AddWithValue("@stocksleft", guna2TextBox7.Text);
                        insCmd2.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                        insCmd2.Parameters.AddWithValue("@selling", guna2TextBox6.Text);
                        insCmd2.Parameters.AddWithValue("@type", guna2ComboBox4.Text);
                        insCmd2.Parameters.AddWithValue("@unit", guna2TextBox3.Text);
                        insCmd2.Parameters.AddWithValue("@date", date);
                        insCmd2.Parameters.AddWithValue("@modifiedby", name);
                        insCmd2.Parameters.AddWithValue("@remarks", guna2TextBox2.Text);
                        int affectedRows2 = insCmd2.ExecuteNonQuery();
                        codeMaterial.Close();

                    }
                    if (forms == "ITEMLEFT")
                    {
                        obj.loadall();
                    }
                    if (forms == "FORM1")
                    {

                        DataTable dt = obj1.dataGridView3.DataSource as DataTable;
                        dt.Rows.Add(ids, guna2TextBox4.Text + ids.ToString().PadLeft(6, '0'), " ", guna2TextBox7.Text, guna2TextBox1.Text, guna2TextBox5.Text, guna2TextBox6.Text, guna2TextBox3.Text, guna2ComboBox3.Text);

                        dt.DefaultView.Sort = "ID DESC";

                    }
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        DateTime datenow = DateTime.Now;
                        string fname = datenow.ToString("MM/dd/yyyy");
                        //string fpcode = guna2TextBox4.Text + ids.ToString();
                        otherDB.Open();
                        string insStmt = "insert into tblProcessHist ([product_code], [operation], [nameby], [prodID], [date]) values" +
                            " (@product_code,@operation,@nameby,@prodID,@date)";
                        SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@product_code", fpcode);
                        insCmd.Parameters.AddWithValue("@operation", "Adding");
                        insCmd.Parameters.AddWithValue("@nameby", name);
                        insCmd.Parameters.AddWithValue("@prodID", ids);
                        insCmd.Parameters.AddWithValue("@date", fname);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        otherDB.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        string insStmt = "insert into tblBodega ([itemid], [bodega], [qty]) values" +
                            " (@itemid,@bodega,@qty)";
                        SqlCommand insCmd = new SqlCommand(insStmt, itemCode);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@itemid", ids);
                        insCmd.Parameters.AddWithValue("@bodega", guna2ComboBox3.Text);
                        insCmd.Parameters.AddWithValue("@qty", guna2TextBox7.Text);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        itemCode.Close();
                    }
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        string operation = "Starting Stock";
                        codeMaterial.Open();
                        string insStmt = "insert into tblHistory ([itemid], [date], [operation], [product_code], [description], [name], [remarks], [aqty], [stock], [type]) values" +
                            " (@itemid,@date,@operation,@product_code,@description,@name,@remarks,@aqty,@stock,@type)";
                        SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                        insCmd.Parameters.AddWithValue("@itemid", ids);
                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                        insCmd.Parameters.AddWithValue("@operation", operation);
                        insCmd.Parameters.AddWithValue("@product_code", fpcode);
                        insCmd.Parameters.AddWithValue("@description", guna2TextBox1.Text.Trim());
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@remarks", guna2TextBox2.Text);
                        insCmd.Parameters.AddWithValue("@aqty", guna2TextBox7.Text);
                        insCmd.Parameters.AddWithValue("@stock", guna2TextBox7.Text);
                        insCmd.Parameters.AddWithValue("@type", "SS");
                        int affectedRows = insCmd.ExecuteNonQuery();
                        codeMaterial.Close();
                    }
                    //dt.Rows.Add(guna2TextBox1.Text);
                    load();
                    load2();
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Done");
                    guna2TextBox5.Text = "0.00";
                    guna2TextBox6.Text = "0.00";
                    guna2TextBox7.Text = "0.00";
                    //this.Close();
                }
            }
        }
        private void guna2NumericUpDown1_Enter(object sender, EventArgs e)
        {
          
        }

        private void guna2NumericUpDown3_Enter(object sender, EventArgs e)
        {
         
        }

        private void guna2NumericUpDown2_Enter(object sender, EventArgs e)
        {
          
        }

        private void guna2NumericUpDown1_Leave(object sender, EventArgs e)
        {
          
        }

        private void guna2NumericUpDown3_Leave(object sender, EventArgs e)
        {
        
        }

        private void guna2NumericUpDown2_Leave(object sender, EventArgs e)
        {
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            itemdetail i = new itemdetail();
            i.id = dataGridView3.CurrentRow.Cells["Column1"].Value.ToString();
            i.productcode = dataGridView3.CurrentRow.Cells["Column2"].Value.ToString();
            i.name = name;
            i.form = form;
            i.ShowDialog();
        }

        private void additem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void guna2ComboBox1_Validating(object sender, CancelEventArgs e)
        {

        }
        private void loadsubtype()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable dtsub = new DataTable();
                string Query = "SELECT subcategory,value,catname,catval FROM tblSubCat WHERE catval= '" + guna2ComboBox1.SelectedValue + "' ORDER BY subcategory ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    dtsub.Load(myReader);
                    guna2ComboBox2.DataSource = dtsub;
                    guna2ComboBox2.ValueMember = "value";
                    guna2ComboBox2.DisplayMember = "subcategory";
                }
                otherDB.Close();
            }
        }
        private void loadtype()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable dttype = new DataTable();
                string Query = "SELECT value,type,valcat,namecat,valsubcat,namesubcat FROM tblType WHERE valcat= '" + guna2ComboBox1.SelectedValue + "' AND valsubcat= '" + guna2ComboBox2.SelectedValue + "' ORDER BY type ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    dttype.Load(myReader);
                    guna2ComboBox4.DataSource = dttype;
                    guna2ComboBox4.ValueMember = "value";
                    guna2ComboBox4.DisplayMember = "type";
                }
                otherDB.Close();

            }
        }

        private void guna2ComboBox2_Validating(object sender, CancelEventArgs e)
        {

        }

        private void guna2ComboBox4_Validating(object sender, CancelEventArgs e)
        {

        }

        private void guna2ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            load();
            if (guna2ComboBox4.SelectedValue != null || guna2ComboBox4.Text != "")
            {
                pictureBox2.Visible = true;
            }
            else
            {
                pictureBox2.Visible = false;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            categories c = new categories();
            c.Show();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            guna2ComboBox2.SelectedIndex = -1;
            //guna2ComboBox2.SelectedValue = null;
            guna2ComboBox4.SelectedIndex = -1;
            //guna2ComboBox4.SelectedValue = null;
            guna2ComboBox4.DataSource = null;
            load();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            guna2ComboBox4.SelectedIndex = -1;
            //guna2ComboBox4.SelectedValue = null;
            load();
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            load();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text != "")
            {

                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [codeMaterial] WHERE ([product_code] = @product_code)", codeMaterial);
                    check_User_Name.Parameters.AddWithValue("@product_code", guna2TextBox4.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        label3.Text = "Product code already exist";
                        label3.Visible = true;
                    }
                    else
                    {
                        label3.Text = "Exist";
                        label3.Visible = false;
                    }

                    codeMaterial.Close();
                }
            }
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                guna2Button2_Click(sender, e);
                guna2TextBox1.Focus();
            }
        }

        private void guna2TextBox5_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text == "0.00" || guna2TextBox5.Text == "0" || guna2TextBox5.Text == "")
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

        private void guna2TextBox6_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox6.Text == "0.00" || guna2TextBox6.Text == "0" || guna2TextBox6.Text == "")
            {
                guna2TextBox6.Text = "";
            }
        }

        private void guna2TextBox6_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox6.Text == "")
            {
                guna2TextBox6.Text = "0.00";
            }
        }

        private void guna2TextBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox6.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox7_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox7.Text == "0.00" || guna2TextBox7.Text == "0" || guna2TextBox7.Text == "")
            {
                guna2TextBox7.Text = "";
            }
        }

        private void guna2TextBox7_Leave(object sender, EventArgs e)
        {

            if (guna2TextBox7.Text == "")
            {
                guna2TextBox7.Text = "0.00";
            }
        }

        private void guna2TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox7.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                guna2Button2_Click(sender, e);
                guna2TextBox1.Focus();
            }
        }

        private void guna2TextBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                guna2Button2_Click(sender, e);
                guna2TextBox1.Focus();
            }
        }

        private void guna2TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                guna2Button2_Click(sender, e);
                guna2TextBox1.Focus();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread =
              new Thread(new ThreadStart(checker));
                thread.Start();
            }
        }
        public void checker()
        {
            rowscount = dt.Rows.Count;
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();

                string list = "SELECT COUNT(*) FROM codeMaterial";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                Int32 count = (Int32)command.ExecuteScalar();

                codeMaterial.Close();
                if (count > rowscount)
                {
                    //MessageBox.Show("not equal");
                    addition();

                }
                else if (count < rowscount)
                {
                    deletion();
                }
            }
            thread = null;
        }
        public void addition()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                //MessageBox.Show("not equal");

                DataRow row = dt.Rows[0];
                string idindt = row["ID"].ToString();

                codeMaterial.Open();
                //MessageBox.Show(idindt);

                string listb = "SELECT MAX(ID) FROM codeMaterial";
                SqlCommand commandv = new SqlCommand(listb, codeMaterial);
                int maxId = Convert.ToInt32(commandv.ExecuteScalar());
                codeMaterial.Close();

                DataTable dat = new DataTable();
                codeMaterial.Open();
                string listA = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial WHERE ID BETWEEN '" + idindt + "' AND '" + maxId + "'";
                SqlCommand commandA = new SqlCommand(listA, codeMaterial);
                SqlDataReader readerA = commandA.ExecuteReader();
                dat.Load(readerA);
                codeMaterial.Close();
                int check = 0;
                foreach (DataRow rows in dat.Rows)
                {
                    check++;
                    if (check != 1)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow[0] = rows["description"].ToString();
                        newRow[1] = rows["ID"].ToString();
                        newRow[2] = rows["product_code"].ToString();
                        newRow[3] = rows["category"].ToString();
                        newRow[4] = rows["subcategory"].ToString();
                        newRow[5] = rows["type"].ToString();
                        dt.Rows.InsertAt(newRow, 0);
                        dt.AcceptChanges();
                    }
                }
            }
        }
        public void deletion()
        {
            foreach (DataRow rows in dt.Rows)
            {
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [codeMaterial] WHERE ([ID] = @ID)", codeMaterial);
                    check_User_Name.Parameters.AddWithValue("@ID", rows["ID"].ToString());
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {

                    }
                    else
                    {
                        rows.Delete();
                    }

                    codeMaterial.Close();
                }
            }
            dt.AcceptChanges();
            thread = null;
        }
        Thread thread;

        private void additem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }
    }
}
