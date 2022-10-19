using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
            SuspendLayout();
            pictureBox11.InitialImage = null;
            num = "00";
            loadcombo();
            load2();

            load();
            guna2TextBox1.Focus();
            ResumeLayout();


            this.WindowState = FormWindowState.Maximized;

        }

        private void load2()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                dt.Rows.Clear();
                codeMaterial.Open();
                string list = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                codeMaterial.Close();


            }
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
        }
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
                if (guna2TextBox1.Text == "" || guna2NumericUpDown2.Text == "" || guna2NumericUpDown1.Text == "" || guna2NumericUpDown3.Text == "")
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
                        insCmd.Parameters.AddWithValue("@stocksleft", guna2NumericUpDown2.Text);
                        insCmd.Parameters.AddWithValue("@cost", guna2NumericUpDown1.Text);
                        insCmd.Parameters.AddWithValue("@selling", guna2NumericUpDown3.Text);
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
                        insCmd2.Parameters.AddWithValue("@stocksleft", guna2NumericUpDown2.Text);
                        insCmd2.Parameters.AddWithValue("@cost", guna2NumericUpDown1.Text);
                        insCmd2.Parameters.AddWithValue("@selling", guna2NumericUpDown3.Text);
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
                        dt.Rows.Add(ids, guna2TextBox4.Text + ids.ToString().PadLeft(6, '0'), " ", guna2NumericUpDown2.Text, guna2TextBox1.Text, guna2NumericUpDown1.Text, guna2NumericUpDown3.Text, guna2TextBox3.Text, guna2ComboBox3.Text);

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
                        insCmd.Parameters.AddWithValue("@qty", guna2NumericUpDown2.Text);
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
                        insCmd.Parameters.AddWithValue("@aqty", guna2NumericUpDown2.Text);
                        insCmd.Parameters.AddWithValue("@stock", guna2NumericUpDown2.Text);
                        insCmd.Parameters.AddWithValue("@type", "SS");
                        int affectedRows = insCmd.ExecuteNonQuery();
                        codeMaterial.Close();
                    }
                    //dt.Rows.Add(guna2TextBox1.Text);
                    load();
                    load2();
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("Done");
                    guna2NumericUpDown1.Text = "0.00";
                    guna2NumericUpDown3.Text = "0.00";
                    guna2NumericUpDown2.Text = "0.00";
                    //this.Close();
                }
            }
        }
        private void guna2NumericUpDown1_Enter(object sender, EventArgs e)
        {
            if (guna2NumericUpDown1.Text == "0.00" || guna2NumericUpDown1.Text == "0" || guna2NumericUpDown1.Text == "")
            {
                guna2NumericUpDown1.Text = "";
            }
        }

        private void guna2NumericUpDown3_Enter(object sender, EventArgs e)
        {
            if (guna2NumericUpDown3.Text == "0.00" || guna2NumericUpDown3.Text == "0" || guna2NumericUpDown3.Text == "")
            {
                guna2NumericUpDown3.Text = "";
            }
        }

        private void guna2NumericUpDown2_Enter(object sender, EventArgs e)
        {
            if (guna2NumericUpDown2.Text == "0.00" || guna2NumericUpDown2.Text == "0" || guna2NumericUpDown2.Text == "")
            {
                guna2NumericUpDown2.Text = "";
            }
        }

        private void guna2NumericUpDown1_Leave(object sender, EventArgs e)
        {
            if (guna2NumericUpDown1.Text == "")
            {
                guna2NumericUpDown1.Text = "0.00";
            }
        }

        private void guna2NumericUpDown3_Leave(object sender, EventArgs e)
        {
            if (guna2NumericUpDown3.Text == "")
            {
                guna2NumericUpDown3.Text = "0.00";
            }
        }

        private void guna2NumericUpDown2_Leave(object sender, EventArgs e)
        {
            if (guna2NumericUpDown2.Text == "")
            {
                guna2NumericUpDown2.Text = "0.00";
            }
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
    }
}
