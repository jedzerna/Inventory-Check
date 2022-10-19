using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class itemdetail : Form
    {
        private string date;
        public string name;
        public string productcode;

        public string id;
        private string description;



        public itemdetail()
        {
            InitializeComponent();
            pictureBox13.InitialImage = null;
        }
        public string form;
        private void itemdetail_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            getinfo();
            load();
            loadstocks();
            loadincoming();
            ChangeControlStyles(dataGridView3, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView3.RowHeadersVisible = false;


            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;

            if (form == "form1") guna2Button7.Visible = false;
            dataGridView3.ClearSelection();
            dataGridView1.ClearSelection();
            ResumeLayout();
            Cursor.Current = Cursors.Default;

            //guna2Button3.Visible = false;
            //guna2Button4.Visible = true;
            //guna2Button1.Visible = true;
            //guna2Button2.Visible = false;
            //guna2Button5.Visible = false;
            //label4.Visible = false;
            //guna2ComboBox3.Visible = true;
            //guna2TextBox12.Visible = false;

            //label15.Visible = false;

            //guna2ComboBox3.FillColor = SystemColors.WindowFrame;
            //guna2TextBox2.FillColor = SystemColors.WindowFrame;
            //guna2TextBox3.FillColor = SystemColors.WindowFrame;
            //guna2TextBox9.FillColor = SystemColors.WindowFrame;
            //guna2TextBox10.FillColor = SystemColors.WindowFrame;
            //guna2TextBox5.FillColor = SystemColors.WindowFrame;

            //guna2TextBox7.Visible = true;
            //guna2TextBox10.ReadOnly = false;

            //guna2TextBox9.ReadOnly = false;

            //guna2TextBox3.ReadOnly = false;
            //guna2TextBox5.ReadOnly = false;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        DataTable stocks = new DataTable();
        public void loadstocks()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                string list = "Select product_code,description from codeMaterial";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                stocks.Load(reader);
                codeMaterial.Close();
            }
        }
        public void loadincoming()
        {
            DataTable dt = new DataTable();
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                    itemCode.Open();
                    string list = "Select ponumber,qty,cost,total,typeofp from itemCode where iitem = '" + id + "' order by Id desc";
                    SqlCommand command = new SqlCommand(list, itemCode);
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    itemCode.Close();

                dt.Columns.Add("supplier");
                dt.Columns.Add("datetime", typeof(DateTime));
                foreach (DataRow row in dt.Rows)
                {
                    if (row["typeofp"].ToString() == "" || row["typeofp"] == DBNull.Value)
                    {
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            String query = "SELECT suppliername,datetime FROM tblIn WHERE ponumber = '" + row["ponumber"].ToString()+"'";
                            SqlCommand cmd = new SqlCommand(query, tblIn);
                            SqlDataReader rdr = cmd.ExecuteReader();
                            if (rdr.Read())
                            {
                                row["supplier"] = rdr["suppliername"].ToString();
                                row["datetime"] = DateTime.Parse(rdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                            }
                            tblIn.Close();
                        }
                    }
                    else
                    {
                        row.Delete();
                    }
                }
                dt.AcceptChanges();
            }
            dt.DefaultView.Sort = "datetime DESC";
            dataGridView1.DataSource = dt;
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
        string o = "PO";
        string q = "DR";
        public void load()
        {
            DataTable dt = new DataTable();
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                if (guna2CheckBox1.Checked)
                {
                    codeMaterial.Open();
                    string list = "Select * from tblHistory where itemid = '" + id + "' order by id desc";
                    SqlCommand command = new SqlCommand(list, codeMaterial);
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    codeMaterial.Close();
                }
                else
                {
                    codeMaterial.Open();
                    string list = "Select top 300 * from tblHistory where itemid = '" + id + "' order by id desc";
                    SqlCommand command = new SqlCommand(list, codeMaterial);
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    codeMaterial.Close();
                }
            }

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {

                DataTable dttt = new DataTable();
                dttt.Clear();
                dttt.Columns.Add("1",typeof(DateTime));
                dttt.Columns.Add("2");
                dttt.Columns.Add("3");
                dttt.Columns.Add("4");
                dttt.Columns.Add("5");
                dttt.Columns.Add("6");
                dttt.Columns.Add("7");
                dttt.Columns.Add("8");
                dttt.Columns.Add("9");
                dttt.Columns.Add("10");
                dttt.Columns.Add("11");
                foreach (DataRow row in dt.Rows)
                {
                    string ponumber = row["podrid"].ToString();
                    string podr = "";
                    string vp = "";
                    string add = "0.00";
                    string sub = "0.00";
                    string total = "0.00";
                    string selling = "0.00";
                    string cost = "0.00";
                    string totalamount = "0.00";
                    string date = row["date"].ToString();
                    string type = row["type"].ToString();

                    if (row["type"].ToString() == "PO")
                    {

                        itemCode.Open();
                        String query = "SELECT ponumber,qty,stocksleft,cost,total FROM itemCode WHERE poid = '" + ponumber + "' AND iitem = '" + row["itemid"].ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, itemCode);
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            podr = (rdr["ponumber"].ToString());
                            add = (rdr["qty"].ToString());
                            total = (rdr["stocksleft"].ToString());
                            cost = (rdr["cost"].ToString());
                            totalamount = (rdr["total"].ToString());
                        }
                        itemCode.Close();

                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            String vquery = "SELECT vpno FROM tblVP WHERE poid = '" + ponumber + "'";
                            SqlCommand vcmd = new SqlCommand(vquery, tblIn);
                            SqlDataReader vrdr = vcmd.ExecuteReader();
                            if (vrdr.Read())
                            {
                                vp = (vrdr["vpno"].ToString());
                            }
                            tblIn.Close();
                        }

                        object[] o = { date, ponumber, podr, vp, add, sub, total, selling, cost, totalamount, type };
                        dttt.Rows.Add(o);
                    }
                    else if (row["type"].ToString() == "DR")
                    {
                        itemCode.Open();
                        String query = "SELECT drnumber,qty,stocksleft,selling,total FROM tblDRitemCode WHERE drid = '" + ponumber + "' AND iitem = '" + row["itemid"].ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, itemCode);
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            podr = (rdr["drnumber"].ToString());
                            sub = (rdr["qty"].ToString());
                            total = (rdr["stocksleft"].ToString());
                            selling = (rdr["selling"].ToString());
                            totalamount = (rdr["total"].ToString());
                        }
                        itemCode.Close();
                        //vp = (vrdr["vpno"].ToString());
                        //using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        //{
                        //    tblIn.Open();
                        //    String vquery = "SELECT vpno FROM tblVP WHERE poid = '" + ponumber + "'";
                        //    SqlCommand vcmd = new SqlCommand(vquery, tblIn);
                        //    SqlDataReader vrdr = vcmd.ExecuteReader();
                        //    if (vrdr.Read())
                        //    {
                        //        vp = (vrdr["vpno"].ToString());
                        //    }
                        //    tblIn.Close();
                        //}

                        object[] o = { date, ponumber, podr, vp, add, sub, total, selling, cost, totalamount, type };
                        dttt.Rows.Add(o);
                    }
                    else
                    {
                        ponumber = row["id"].ToString();
                        podr = "";
                        vp = "";
                        add = row["aqty"].ToString();
                        sub = row["dqty"].ToString();
                        total = row["stock"].ToString();
                        selling = "0.00";
                        cost = "0.00";
                        totalamount = "0.00";
                        date = row["date"].ToString();
                        if (row["type"].ToString() != "")
                        {
                            type = row["type"].ToString();
                        }
                        else
                        {
                            if (row["aqty"].ToString() == "")
                            {
                                type = "DD";
                            }
                            else
                            {
                                type = "RT";
                            }
                        }

                        object[] o = { date, ponumber, podr, vp, add, sub, total, selling, cost, totalamount, type };
                        dttt.Rows.Add(o);

                    }
                }

                //itemCode.Open();
                //DataTable drdt = new DataTable();
                //string drlist = "Select qty,stocksleft,cost,selling,total,drnumber,typeofp,drid,sv from tblDRitemCode where iitem = '" + id + "' and typeofp = '" + q + "'";
                ////string drlist = "Select drnumber,productcode,description,qty,unit,typeofp,selling,projectcode,projectname from tblDRitemCode where iitem = '" + id + "' and typeofp = '" + q + "'";
                //SqlCommand drcommand = new SqlCommand(drlist, itemCode);
                //SqlDataReader drreader = drcommand.ExecuteReader();
                //drdt.Load(drreader);

                //foreach (DataRow row in drdt.Rows)
                //{
                //    string ponumber = row["drid"].ToString();
                //    string podr = row["drnumber"].ToString();
                //    string vp = "";
                //    string add = "0.00";
                //    string sub = row["qty"].ToString(); 
                //    string total = row["stocksleft"].ToString();
                //    string selling = row["selling"].ToString();
                //    string cost = row["cost"].ToString();
                //    string totalamount = row["total"].ToString();
                //    string date = "";
                //    string type = "DR";
                //    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                //    {
                //        dbDR.Open();
                //        String query = "SELECT datetime,sv FROM tblDR WHERE Id = '" + ponumber + "'";
                //        SqlCommand cmd = new SqlCommand(query, dbDR);
                //        SqlDataReader rdr = cmd.ExecuteReader();
                //        if (rdr.Read())
                //        {
                //            date = (rdr["datetime"].ToString());
                //            vp = (rdr["sv"].ToString());
                //        }
                //        dbDR.Close();
                //    }


                //    object[] o = { date, ponumber, podr, vp, add, sub, total, selling, cost, totalamount, type };
                //    dttt.Rows.Add(o);
                //}


                dttt.DefaultView.Sort = "1 DESC";
                dataGridView3.DataSource = dttt;
            }
            dataGridView3.ClearSelection();
        }
        //private void load2()
        //{
        //    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
        //    {
        //        itemCode.Open();
        //        DataTable dt = new DataTable();
        //        string list = "Select drnumber,productcode,description,qty,unit,typeofp,selling,projectcode,projectname from tblDRitemCode where iitem = '" + id + "' and typeofp = '"+q+"'";
        //        SqlCommand command = new SqlCommand(list, itemCode);
        //        SqlDataReader reader = command.ExecuteReader();
        //        dt.Load(reader);
        //        dataGridView1.DataSource = dt;
        //        reader.Close();
        //        itemCode.Close();
        //        //itemCode.Dispose();
        //        ////(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("typeofp LIKE '{0}%'", o);
        //        ////(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("typeofp LIKE '{0}%'", q);
        //        //string rowFilter = string.Format("typeofp LIKE '{0}%'", q);
        //        ////rowFilter += string.Format("OR typeofp LIKE '{0}%'", q);
        //        //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        //    }
        //}
        SqlDataReader rdr;
        string ccat;
        string cscat;
        string ctype;
        public void getinfo()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                description = "";
                codeMaterial.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    guna2TextBox1.Text = (rdr["product_code"].ToString());
                    guna2TextBox2.Text = (rdr["mfg_code"].ToString());
                    guna2TextBox3.Text = (rdr["description"].ToString());
                    description = (rdr["description"].ToString());
                    guna2TextBox7.Text = (rdr["stocksleft"].ToString());
                    guna2TextBox9.Text = (rdr["cost"].ToString());
                    guna2TextBox10.Text = (rdr["selling"].ToString());
                    guna2TextBox5.Text = (rdr["unit"].ToString());
                    label19.Text = (rdr["createdby"].ToString());
                    guna2TextBox12.Text = (rdr["dept"].ToString());
                    guna2TextBox4.Text = (rdr["remarks"].ToString());
                    ccat = (rdr["category"].ToString());
                    cscat = (rdr["subcategory"].ToString());
                    ctype = (rdr["type"].ToString());
                    var nt = Environment.NewLine;
                    label2.Text = "Category: "+rdr["category"].ToString() + nt+nt
                        +"Sub Category: "+ rdr["subcategory"].ToString()+nt+nt
                        +"Type: " + rdr["type"].ToString();
                }
                else
                {
                    MessageBox.Show("Can't find any data.");
                    this.Close();
                }
                codeMaterial.Close();
                codeMaterial.Dispose();
            }
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

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            date = d.ToString("yyyy/MM/dd HH:mm:ss");
            label13.Text = d.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void label12_Click(object sender, EventArgs e)
        {
            itemHistory i = new itemHistory();
            i.id = guna2TextBox1.Text;
            i.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string Query = "delete from codeMaterial where product_code='" + this.textBox2.Text + "';";

            //    SqlCommand MyCommand2 = new SqlCommand(Query, codeMaterial);
            //    SqlDataReader MyReader2;
            //    codeMaterial.Open();
            //    MyReader2 = MyCommand2.ExecuteReader();
            //    while (MyReader2.Read())
            //    {
            //    }
            //    codeMaterial.Close();



            //    string Query2 = "delete from codeMaterialHistory where product_code='" + this.textBox2.Text + "';"; 
            //    SqlCommand MyCommand1 = new SqlCommand(Query2, codeMaterial);
            //    SqlDataReader MyReader1;
            //    codeMaterial.Open();
            //    MyReader1 = MyCommand1.ExecuteReader();
            //    MessageBox.Show("Data Deleted");
            //    while (MyReader2.Read())
            //    {
            //    }
            //    codeMaterial.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            string senderText = (sender as TextBox).Text;
            string senderName = (sender as TextBox).Name;
            string[] splitByDecimal = senderText.Split('.');
            int cursorPosition = (sender as TextBox).SelectionStart;

            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }


            if (e.KeyChar == '.'
                && senderText.IndexOf('.') > -1)
            {
                e.Handled = true;
            }


            if (!char.IsControl(e.KeyChar)
                && senderText.IndexOf('.') < cursorPosition
                && splitByDecimal.Length > 1
                && splitByDecimal[1].Length == 2)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            string senderText = (sender as TextBox).Text;
            string senderName = (sender as TextBox).Name;
            string[] splitByDecimal = senderText.Split('.');
            int cursorPosition = (sender as TextBox).SelectionStart;

            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }


            if (e.KeyChar == '.'
                && senderText.IndexOf('.') > -1)
            {
                e.Handled = true;
            }


            if (!char.IsControl(e.KeyChar)
                && senderText.IndexOf('.') < cursorPosition
                && splitByDecimal.Length > 1
                && splitByDecimal[1].Length == 2)
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {




            //label19.Visible = false;

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
        
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {




        }


        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox6_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox6_Leave(object sender, EventArgs e)
        {

        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress_1(object sender, KeyPressEventArgs e)
        {


        }

        private void textBox5_Leave(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox7_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox11_TextChanged(object sender, EventArgs e)
        {

        }

        string lb = "{";
        string rb = "}";
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "" || guna2TextBox7.Text == "" || guna2TextBox7.Text == "")
            {
                MessageBox.Show("Please enter all the required labels", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Are you sure to complete this changes?", "Changes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (guna2TextBox10.Text == "")
                {
                    guna2TextBox10.Text = "0.00";
                }
                if (guna2TextBox9.Text == "")
                {
                    guna2TextBox9.Text = "0.00";
                }
                loadstocks();
                //string code = guna2TextBox1.Text.Substring(5);
                //string des = guna2TextBox3.Text.Substring(0, 3).ToUpper();
                //string cat = guna2TextBox1.Text.Substring(0, 2);
                //string finalcode = cat + des + code;
                if (description != guna2TextBox3.Text)
                {
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        codeMaterial.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [codeMaterial] WHERE ([description] = @description COLLATE SQL_Latin1_General_CP1_CS_AS)", codeMaterial);
                        check_User_Name.Parameters.AddWithValue("@description", guna2TextBox3.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {
                            MessageBox.Show("Description Name Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        codeMaterial.Close();
                    }
                }

                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        SqlCommand cmd = new SqlCommand("update codeMaterial set [product_code]=@product_code, [mfg_code]=@mfg_code, [description]=@description, [stocksleft]=@stocksleft, [cost]=@cost, selling=@selling, unit=@unit, remarks=@remarks where ID=@ID", codeMaterial);
                        codeMaterial.Open();
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@product_code", guna2TextBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        cmd.Parameters.AddWithValue("@mfg_code", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        cmd.Parameters.AddWithValue("@description", guna2TextBox3.Text);
                        cmd.Parameters.AddWithValue("@stocksleft", guna2TextBox7.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        cmd.Parameters.AddWithValue("@cost", string.Format("{0:#,##0.00}",guna2TextBox9.Text));
                        cmd.Parameters.AddWithValue("@selling", string.Format("{0:#,##0.00}", guna2TextBox10.Text));
                        cmd.Parameters.AddWithValue("@unit", guna2TextBox5.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        cmd.Parameters.AddWithValue("@remarks", guna2TextBox4.Text);
                        cmd.ExecuteNonQuery();
                        codeMaterial.Close();

                        codeMaterial.Open();
                        string insStmt = "insert into codeMaterialHistory ([product_code], [mfg_code], [description], [stocksleft], [cost], [selling], [unit], [date], [modifiedby], [remarks]) values" +
                            " (@product_code,@mfg_code,@description,@stocksleft,@cost,@selling,@unit,@date,@modifiedby,@remarks)";
                        SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                        insCmd.Parameters.AddWithValue("@product_code", guna2TextBox1.Text);
                        insCmd.Parameters.AddWithValue("@mfg_code", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        insCmd.Parameters.AddWithValue("@description", guna2TextBox3.Text);
                        insCmd.Parameters.AddWithValue("@stocksleft", guna2TextBox7.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        insCmd.Parameters.AddWithValue("@cost", string.Format("{0:#,##0.00}", guna2TextBox9.Text));
                        insCmd.Parameters.AddWithValue("@selling", string.Format("{0:#,##0.00}", guna2TextBox10.Text));
                        insCmd.Parameters.AddWithValue("@unit", guna2TextBox5.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        insCmd.Parameters.AddWithValue("@date", date);
                        insCmd.Parameters.AddWithValue("@modifiedby", name.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                        insCmd.Parameters.AddWithValue("@remarks", guna2TextBox4.Text);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        codeMaterial.Close();

                        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                        {
                            SqlCommand cmd2 = new SqlCommand("update itemCode set productcode=@productcode,mfgcode=@mfgcode, [description]=@description where productcode='" + guna2TextBox1.Text + "'", itemCode);
                            itemCode.Open();
                            cmd2.Parameters.AddWithValue("@productcode", guna2TextBox1.Text);
                            cmd2.Parameters.AddWithValue("@mfgcode", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                            cmd2.Parameters.AddWithValue("@description", guna2TextBox3.Text);
                            cmd2.ExecuteNonQuery();
                            itemCode.Close();

                            SqlCommand cmd3 = new SqlCommand("update tblDRitemCode set  productcode=@productcode,mfgcode=@mfgcode, [description]=@description where productcode='" + guna2TextBox1.Text + "'", itemCode);
                            itemCode.Open();
                            cmd3.Parameters.AddWithValue("@productcode", guna2TextBox1.Text);
                            cmd3.Parameters.AddWithValue("@mfgcode", guna2TextBox2.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                            cmd3.Parameters.AddWithValue("@description", guna2TextBox3.Text);
                            cmd3.ExecuteNonQuery();
                            itemCode.Close();
                        }
                    }
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        DateTime datenow = DateTime.Now;
                        string fname = datenow.ToString("MM/dd/yyyy");
                        otherDB.Open();
                        string insStmt = "insert into tblProcessHist ([product_code], [operation], [nameby], [prodID], [date]) values" +
                            " (@product_code,@operation,@nameby,@prodID,@date)";
                        SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@product_code", guna2TextBox1.Text);
                        insCmd.Parameters.AddWithValue("@operation", "Updating");
                        insCmd.Parameters.AddWithValue("@nameby", name);
                        insCmd.Parameters.AddWithValue("@prodID", id);
                        insCmd.Parameters.AddWithValue("@date", fname);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        otherDB.Close();
                    }
                    getinfo();
                    guna2Button3.Visible = true;

                    guna2Button4.Visible = false;
                    guna2Button1.Visible = false;
                    guna2Button2.Visible = true;
                    guna2Button5.Visible = true;
                    guna2TextBox4.ReadOnly = true;

                    //label15.Visible = false;

                    guna2TextBox3.FillColor = Color.FromArgb(0, 8, 30);
                    guna2TextBox9.FillColor = Color.FromArgb(0, 8, 30);
                    guna2TextBox10.FillColor = Color.FromArgb(0, 8, 30);
                    guna2TextBox5.FillColor = Color.FromArgb(0, 8, 30);

                    guna2TextBox7.Visible = true;

                    guna2TextBox9.ReadOnly = true;
                    guna2TextBox10.ReadOnly = true;

                    guna2TextBox3.ReadOnly = true;
                    guna2TextBox5.ReadOnly = true;
                    refr();
                    Cursor.Current = Cursors.Default;
                

            }
        }
        public void refr()
        {
            if (form == "refresh")
            {
                foreach (DataRow row in itemsleft.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {
                            codeMaterial.Open();
                            DataTable dt = new DataTable();
                            String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                            SqlCommand cmd = new SqlCommand(query, codeMaterial);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                //guna2TextBox1.Text = (rdr["product_code"].ToString());
                                row["product_code"] = rdr["product_code"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["stocksleft"] = rdr["stocksleft"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["selling"] = rdr["selling"].ToString();
                                row["cost"] = rdr["cost"].ToString();
                                row["category"] = rdr["category"].ToString();
                                row["subcategory"] = rdr["subcategory"].ToString();
                                row["type"] = rdr["type"].ToString();
                            }
                            codeMaterial.Close();
                        }
                        break;

                    }
                }
            }
            else if (form == "form1")
            {
                foreach (DataRow row in form1.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {
                            codeMaterial.Open();
                            DataTable dt = new DataTable();
                            String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                            SqlCommand cmd = new SqlCommand(query, codeMaterial);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                //guna2TextBox1.Text = (rdr["product_code"].ToString());
                                row["ID"] = rdr["ID"].ToString();
                                row["product_code"] = rdr["product_code"].ToString();
                                row["mfg_code"] = rdr["mfg_code"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["stocksleft"] = rdr["stocksleft"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["selling"] = rdr["selling"].ToString();
                                row["cost"] = rdr["cost"].ToString();
                                row["unit"] = rdr["unit"].ToString();
                                row["dept"] = rdr["dept"].ToString();

                            }
                            codeMaterial.Close();
                        }
                        break;

                    }
                }
                foreach (DataRow row in additem.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        string list = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial";
                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {
                            codeMaterial.Open();
                            DataTable dt = new DataTable();
                            String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                            SqlCommand cmd = new SqlCommand(query, codeMaterial);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                //guna2TextBox1.Text = (rdr["product_code"].ToString());
                                row["description"] = rdr["description"].ToString();
                                row["ID"] = rdr["ID"].ToString();
                                row["product_code"] = rdr["product_code"].ToString();
                                row["category"] = rdr["category"].ToString();
                                row["subcategory"] = rdr["subcategory"].ToString();
                                row["type"] = rdr["type"].ToString();
                            }
                            codeMaterial.Close();
                        }
                        break;

                    }
                }
            }
            else if (form == "refreshplus")
            {
                foreach (DataRow row in itemsleft.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {
                            codeMaterial.Open();
                            DataTable dt = new DataTable();
                            String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                            SqlCommand cmd = new SqlCommand(query, codeMaterial);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                //guna2TextBox1.Text = (rdr["product_code"].ToString());
                                row["product_code"] = rdr["product_code"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["stocksleft"] = rdr["stocksleft"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["selling"] = rdr["selling"].ToString();
                                row["cost"] = rdr["cost"].ToString();
                                row["category"] = rdr["category"].ToString();
                                row["subcategory"] = rdr["subcategory"].ToString();
                                row["type"] = rdr["type"].ToString();
                            }
                            codeMaterial.Close();
                        }
                        break;

                    }
                }
                foreach (DataRow row in additem.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        string list = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial";
                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {
                            codeMaterial.Open();
                            DataTable dt = new DataTable();
                            String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                            SqlCommand cmd = new SqlCommand(query, codeMaterial);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                //guna2TextBox1.Text = (rdr["product_code"].ToString());
                                row["description"] = rdr["description"].ToString();
                                row["ID"] = rdr["ID"].ToString();
                                row["product_code"] = rdr["product_code"].ToString();
                                row["category"] = rdr["category"].ToString();
                                row["subcategory"] = rdr["subcategory"].ToString();
                                row["type"] = rdr["type"].ToString();
                            }
                            codeMaterial.Close();
                        }
                        break;

                    }
                }
            }
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            guna2Button3.Visible = false;
            guna2Button4.Visible = true;
            guna2Button1.Visible = true;
            guna2Button2.Visible = false;
            guna2Button5.Visible = false;
            guna2TextBox4.ReadOnly = false;
            //label15.Visible = false;

            guna2TextBox3.FillColor = Color.FromArgb(20, 28, 50);
            guna2TextBox9.FillColor = Color.FromArgb(20, 28, 50);
            guna2TextBox10.FillColor = Color.FromArgb(20, 28, 50);
            guna2TextBox5.FillColor = Color.FromArgb(20, 28, 50);


            guna2TextBox7.Visible = true;

            guna2TextBox9.ReadOnly = false;
            guna2TextBox10.ReadOnly = false;

            guna2TextBox3.ReadOnly = false;
            guna2TextBox5.ReadOnly = false;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            getinfo();
            guna2TextBox4.ReadOnly = true;
            guna2Button3.Visible = true;

            guna2Button4.Visible = false;
            guna2Button1.Visible = false;

            guna2Button2.Visible = true;
            guna2Button5.Visible = true;

            //label15.Visible = false;

            guna2TextBox3.FillColor = Color.FromArgb(0, 8, 30);
            guna2TextBox9.FillColor = Color.FromArgb(0, 8, 30);
            guna2TextBox10.FillColor = Color.FromArgb(0, 8, 30);
            guna2TextBox5.FillColor = Color.FromArgb(0, 8, 30);

            guna2TextBox7.Visible = true;

            guna2TextBox9.ReadOnly = true;
            guna2TextBox10.ReadOnly = true;

            guna2TextBox3.ReadOnly = true;
            guna2TextBox5.ReadOnly = true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            addstocks a = new addstocks();
            a.form = "add";
            a.id = id;
            a.name = name;
            a.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            deductstocks a = new deductstocks();
            a.id = id;
            a.form = "minus";
            a.name = name;
            a.ShowDialog();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            itemadddeducthist a = new itemadddeducthist();
            a.id = id;
            a.ShowDialog();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            bodegabreakdown a = new bodegabreakdown();
            a.num = "1";
            a.id = id;
            a.ShowDialog();
        }

        private void label10_Click(object sender, EventArgs e)
        {

            bodegabreakdown a = new bodegabreakdown();
            a.id = id;
            a.ShowDialog();
        }

        private void guna2TextBox10_Enter(object sender, EventArgs e)
        {

        }

        private void guna2NumericUpDown3_Leave(object sender, EventArgs e)
        {
        
        }

        private void guna2NumericUpDown3_Enter(object sender, EventArgs e)
        {
         
        }

        private void guna2NumericUpDown2_Enter(object sender, EventArgs e)
        {
          
        }

        private void guna2NumericUpDown1_Enter(object sender, EventArgs e)
        {
           
        }

        private void guna2NumericUpDown1_Leave(object sender, EventArgs e)
        {
          
        }

        private void guna2NumericUpDown2_Leave(object sender, EventArgs e)
        {
        
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label15_Click(object sender, EventArgs e)
        {
         
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        itemsleft itemsleft = (itemsleft)Application.OpenForms["itemsleft"];
        Form1 form1 = (Form1)Application.OpenForms["Form1"];
        additem additem = (additem)Application.OpenForms["additem"];
        private void guna2Button7_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to DELETE this item?", "Delete?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();

                    using (SqlCommand command = new SqlCommand("DELETE FROM codeMaterial WHERE ID = '" + id + "'", codeMaterial))
                    {
                        command.ExecuteNonQuery();
                    }
                    codeMaterial.Close();

                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();

                    using (SqlCommand command = new SqlCommand("DELETE FROM tblHistory WHERE itemid = '" + id + "'", codeMaterial))
                    {
                        command.ExecuteNonQuery();
                    }
                    codeMaterial.Close();
                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();

                    using (SqlCommand command = new SqlCommand("DELETE FROM codeMaterialHistory WHERE product_code = '" + guna2TextBox1.Text + "'", codeMaterial))
                    {
                        command.ExecuteNonQuery();
                    }
                    codeMaterial.Close();
                }

                if (form == "refresh")
                {
                    foreach (DataRow row in itemsleft.dt.Rows)
                    {
                        if (id == row["ID"].ToString())
                        {
                            row.Delete();
                            break;
                        }
                    }
                    itemsleft.dt.AcceptChanges();
                }
                else if (form == "form1")
                {
                    foreach (DataRow row in form1.dt.Rows)
                    {
                        if (id == row["ID"].ToString())
                        {
                            row.Delete();
                            break;
                        }
                    }
                    form1.dt.AcceptChanges();
                    foreach (DataRow row in additem.dt.Rows)
                    {
                        if (id == row["ID"].ToString())
                        {
                            row.Delete();
                            break;
                        }
                    }
                    additem.dt.AcceptChanges();
                }
                else if (form == "refreshplus")
                {
                    foreach (DataRow row in itemsleft.dt.Rows)
                    {
                        if (id == row["ID"].ToString())
                        {
                            row.Delete();
                            break;
                        }
                    }
                    itemsleft.dt.AcceptChanges();
                    foreach (DataRow row in additem.dt.Rows)
                    {
                        if (id == row["ID"].ToString())
                        {
                            row.Delete();
                            break;
                        }
                    }
                    additem.dt.AcceptChanges();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    DateTime datenow = DateTime.Now;
                    string fname = datenow.ToString("MM/dd/yyyy");
                    otherDB.Open();
                    string insStmt = "insert into tblProcessHist ([product_code], [operation], [nameby], [prodID], [date]) values" +
                        " (@product_code,@operation,@nameby,@prodID,@date)";
                    SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@product_code", guna2TextBox1.Text);
                    insCmd.Parameters.AddWithValue("@operation", "Deleted " + "|| " + guna2TextBox3.Text);
                    insCmd.Parameters.AddWithValue("@nameby", name);
                    insCmd.Parameters.AddWithValue("@prodID", id);
                    insCmd.Parameters.AddWithValue("@date", fname);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    otherDB.Close();
                }
                MessageBox.Show("Deleted");
                this.Close();

            }
        }
        private void guna2TextBox9_TextChanged(object sender, EventArgs e)
        {

        }
        public bool isNumber(char ch, string text)
        {
            bool res = true;
            char decimalChar = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            //check if it´s a decimal separator and if doesn´t already have one in the text string
            if (ch == decimalChar && text.IndexOf(decimalChar) != -1)
            {
                res = false;
                return res;
            }

            //check if it´s a digit, decimal separator and backspace
            if (!Char.IsDigit(ch) && ch != decimalChar && ch != (char)Keys.Back)
                res = false;

            return res;
        }

        private void guna2TextBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
             char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox9.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox10.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void dataGridView3_Leave(object sender, EventArgs e)
        {
            dataGridView3.ClearSelection();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["TYPE"].Value.ToString() == "DR")
                {
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(207, 123, 122);
                }
                else if(row.Cells["TYPE"].Value.ToString() == "PO")
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkTurquoise;
                }
                else if (row.Cells["TYPE"].Value.ToString() == "RT")
                {
                    row.DefaultCellStyle.ForeColor = Color.GreenYellow;
                }
                else if (row.Cells["TYPE"].Value.ToString() == "DD")
                {
                    row.DefaultCellStyle.ForeColor = Color.OrangeRed;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
            }
            
                //if (dataGridView3.Rows[e.RowIndex].Cells["TYPE"].Value.ToString() == "TYPE")
                //{
                //    if (e.Value.ToString() == "DR")
                //    {
                //        e.CellStyle.ForeColor = Color.FromArgb(207, 123, 122);
                //    }
                //    else
                //    {
                //        e.CellStyle.ForeColor = Color.White;
                //    }
                //}
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //itemDetailsMiniView i = new itemDetailsMiniView();
            //i.id = dataGridView3.CurrentRow.Cells["ponumber"].Value.ToString();
            //i.type = dataGridView3.CurrentRow.Cells["TYPE"].Value.ToString();
            if (dataGridView3.CurrentRow.Cells["TYPE"].Value.ToString() == "PO")
            {
                po();
            }
            else if (dataGridView3.CurrentRow.Cells["TYPE"].Value.ToString() == "DR")
            {
                dr();
            }
            else
            {
                elsedata();
            }
            //i.ShowDialog();

        }
        private void po()
        {
            string suppliername = "";
            string remarks = "";
            string ponumber = "";
            string datetime = "";
            string created = "";
            string completedby = "";
            string si = "";
            string vp = "";
            string carno = "";
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                String vquery = "SELECT * FROM tblIn WHERE Id = '" + dataGridView3.CurrentRow.Cells["ponumber"].Value.ToString() + "'";
                SqlCommand vcmd = new SqlCommand(vquery, tblIn);
                SqlDataReader vrdr = vcmd.ExecuteReader();
                if (vrdr.Read())
                {
                    suppliername = (vrdr["suppliername"].ToString());
                    remarks = (vrdr["additionalinfo"].ToString());
                    ponumber = (vrdr["ponumber"].ToString());
                    datetime = DateTime.Parse(vrdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                    created = (vrdr["createdby"].ToString());
                    completedby = (vrdr["purchasecompletedby"].ToString());
                    si = (vrdr["si"].ToString());
                    vp = (vrdr["vp"].ToString());
                    carno = (vrdr["carno"].ToString());
                }
                tblIn.Close();
            }
            var n = Environment.NewLine;
            MessageBox.Show("Supplier Name: "+suppliername+n+"P.O. No.: "+ponumber+n+"Date: "+datetime+n+n+"Created By: "+created+n+"Completed By: "+completedby+n+n+"*Additional Informations*"+n+"S.I.: "+si+n+"V.P. No: "+vp+n+"Car No: "+carno+n+n+"*Remarks*"+n+remarks,"P.O. Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        private void dr()
        {
            string drnumber = "";
            string pcode = "";
            string pname = "";
            string datetime = "";
            string total = "";
            string completedby = "";
            string sv = "";
            string ponumber = "";
            string podate = "";
            string remarks = "";
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                String vquery = "SELECT * FROM tblDR WHERE Id = '" + dataGridView3.CurrentRow.Cells["ponumber"].Value.ToString() + "'";
                SqlCommand vcmd = new SqlCommand(vquery, dbDR);
                SqlDataReader vrdr = vcmd.ExecuteReader();
                if (vrdr.Read())
                {
                    drnumber = (vrdr["drnumber"].ToString());
                    pcode = (vrdr["projectcode"].ToString());
                    pname = (vrdr["projectname"].ToString());
                    datetime = DateTime.Parse(vrdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                    total = (vrdr["totalamount"].ToString());
                    completedby = (vrdr["purchasecompletedby"].ToString());
                    sv = (vrdr["sv"].ToString());
                    ponumber = (vrdr["ponumber"].ToString());
                    podate = (vrdr["podate"].ToString());
                    remarks = (vrdr["additionalinfo"].ToString());
                }
                dbDR.Close();
            }
            var n = Environment.NewLine;
            MessageBox.Show("Project: " + pcode + n + "Name: " + pname  +n +n+ "DR No.: " + drnumber + n + n + "Date: " + datetime + n + "Total Amount: " + total + n+n+"Completed By: " +completedby+ n + n + "*Additional Informations*" + n + "SV: " + sv + n + "Project PO No.: " + ponumber + n + "PO Date: " + podate + n + n + "*Remarks*" + n + remarks, "D.R. Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void elsedata()
        {
            string date = "";
            string operation = "";
            string product_code = "";
            string description = "";
            string name = "";
            string qty = "";
            string stock = "";
            string type = "";
            string remarks = "";
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                String vquery = "SELECT * FROM tblHistory WHERE Id = '" + dataGridView3.CurrentRow.Cells["ponumber"].Value.ToString() + "'";
                SqlCommand vcmd = new SqlCommand(vquery, codeMaterial);
                SqlDataReader vrdr = vcmd.ExecuteReader();
                if (vrdr.Read())
                {
                    operation = (vrdr["operation"].ToString());
                    product_code = (vrdr["product_code"].ToString());
                    date = DateTime.Parse(vrdr["date"].ToString()).ToString("MM/dd/yyyy");
                    description = (vrdr["description"].ToString());
                    name = (vrdr["name"].ToString());
                    if (vrdr["aqty"].ToString() != "")
                    {
                        qty = "+"+(vrdr["aqty"].ToString());
                    }
                    else
                    {
                        qty = "-"+(vrdr["dqty"].ToString());
                    }
                    stock = (vrdr["stock"].ToString());
                    remarks = (vrdr["remarks"].ToString());
                }
                codeMaterial.Close();
            }
            var n = Environment.NewLine;
            MessageBox.Show("Operation: " + operation + n + "Date: " + date + n + "Name: " + name + n + n + "Product Code: " + product_code+ n + "Product Name: " + description + n + n + "Total QTY: " + qty + n + "Total Stocks: " + stock + n + n +"*Remarks*"+n+ remarks, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2TextBox9_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox9.Text == "")
            {
                guna2TextBox9.Text = "0.00";
            }
        }

        private void guna2TextBox10_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox10.Text == "")
            {
                guna2TextBox10.Text = "0.00";
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            load();
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (guna2Button9.Text == "Show Remarks")
            {
                guna2Button9.Text = "Back";
                guna2TextBox4.Visible = true;
                label5.Visible = true;
                label2.Visible = false;
                label6.Visible = false;

            }
            else
            {
                guna2Button9.Text = "Show Remarks";
                guna2TextBox4.Visible = false;
                label5.Visible = false;
                label2.Visible = true;
                label6.Visible = true;
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Categoryupdate c = new Categoryupdate();
            c.id = id;
            c.cat = ccat;
            c.subcat = cscat;
            c.type = ctype;
            c.form = form;
            c.name = name;
            c.ShowDialog();
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
