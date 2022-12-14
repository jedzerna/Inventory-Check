using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Inventory_Check
{
    public partial class out_supply : Form
    {
        public string name;
        private string date;
        private string randomitemcode;
        public string poid;
        //static string connectionstring = ConfigurationManager.ConnectionStrings["GLU.Properties.Settings.GLUConnectionString"].ConnectionString;
        //SqlConnection con = new SqlConnection(connectionstring);


        SqlDataReader rdr;
        public string id;
        public string num;
        public string itemid;
        public string itemid2;
        public string id2;
        private string operationcheck;
        private string icode;
        public out_supply()
        {
            InitializeComponent();

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
        private void out_supply_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            SuspendLayout();
            pictureBox5.InitialImage = null;
            pictureBox14.InitialImage = null;
            pictureBox15.InitialImage = null;
            pictureBox16.InitialImage = null;
            pictureBox18.InitialImage = null;
            if (num == "1")
            {
                Cursor.Current = Cursors.WaitCursor;
                getinfo();
                load();
                loadscan();
                Cursor.Current = Cursors.Default;
            }
            else if (num == "2")
            {
                Cursor.Current = Cursors.WaitCursor;
                getinfo2();
                load2();
                loaddb();
                count();
                random1();
                button5.Visible = true;
                pictureBox16.Visible = false;
                pictureBox18.Visible = false;
                pictureBox11.Visible = false;
                label24.Text = "P.O. to D.R.";

                Cursor.Current = Cursors.Default;
            }
            Application.EnableVisualStyles();


            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView2.RowHeadersVisible = false;

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;
            //load();
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load2()
        {

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                DataTable dt = new DataTable();
                string list = "Select productcode,mfgcode,description,qty,unit,Id,iitem,cost,selling,type,total,icode from itemCode where icode = '" + itemid2 + "'";
                SqlCommand command = new SqlCommand(list, itemCode);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                itemCode.Close();
                this.dataGridView1.Sort(this.dataGridView1.Columns[8], ListSortDirection.Descending);
                itemCode.Dispose();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
            }
        }
        private void getinfo2()
        {

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblIn WHERE id = '" + id2 + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
                    guna2TextBox1.Text = (rdr["projectcode"].ToString());
                    label11.Text = (rdr["projectname"].ToString());
                    label17.Text = (rdr["qty"].ToString());
                    label20.Text = (rdr["datetime"].ToString());
                    label18.Text = (rdr["totalitems"].ToString());
                    guna2NumericUpDown2.Text = (rdr["totalamount"].ToString());
                    operationcheck = (rdr["operation"].ToString());
                    label19.Text = (rdr["createdby"].ToString());
                    //label25.Text = (rdr["purchasecompletedby"].ToString());
                    icode = (rdr["itemcode"].ToString());
                }
                tblIn.Close();
                tblIn.Dispose();

                //loaddb();


                label31.Visible = true;

                guna2TextBox2.Visible = true;
                guna2TextBox1.Visible = true;
                guna2TextBox3.Visible = true;
                pictureBox14.Visible = false;
                label33.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                button1.Visible = false;
                dataGridView1.Columns["Column5"].ReadOnly = false;
                dataGridView1.Columns["Column7"].ReadOnly = false;

                dataGridView1.Columns["Column8"].Visible = true;
            }

        }




        public void getinfo()
        {
          
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                var sb = new System.Text.StringBuilder();
                dbDR.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblDR WHERE id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, dbDR);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label1.Text = (rdr["drnumber"].ToString());
                    guna2TextBox2.Text = (rdr["drnumber"].ToString());
                    textBox2.Text = (rdr["additionalinfo"].ToString());
                    guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
                    label12.Text = (rdr["projectcode"].ToString());
                    guna2TextBox1.Text = (rdr["projectcode"].ToString());
                    label11.Text = (rdr["projectname"].ToString());
                    label20.Text = (rdr["datetime"].ToString());
                    label17.Text = (rdr["qty"].ToString());
                    label18.Text = (rdr["totalitems"].ToString());
                    guna2NumericUpDown2.Text = (rdr["totalamount"].ToString());
                    operationcheck = (rdr["operation"].ToString());
                    label19.Text = (rdr["createdby"].ToString());
                    label25.Text = (rdr["purchasecompletedby"].ToString());
                    icode = (rdr["itemcode"].ToString());
                    label3.Text = (rdr["sv"].ToString());
                    if (rdr["ponumber"] == null || rdr["ponumber"].ToString() == "" || rdr["ponumber"] == DBNull.Value || rdr["ponumber"].ToString() == null)
                    {
                        label28.ForeColor = Color.LightCoral;
                        label28.Text = "None";
                    }
                    else
                    {
                        DateTime d = DateTime.Parse(rdr["podate"].ToString());
                        label28.ForeColor = Color.White;
                        label28.Text = (rdr["ponumber"].ToString()) + " / Date: " + d.ToString("MM/dd/yyyy");
                    }

                }
                dbDR.Close();
                dbDR.Dispose();
                if (operationcheck == "Completed")
                {
                    button1.Visible = false;
                    button4.Visible = false;
                    pictureBox14.Visible = true;
                    //button1.Visible = true;
                }
                else
                {
                    label33.Visible = false;
                    count();
                }
            }
            //if (label27.Text != "")
            //{
            //    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            //    {

            //        tblIn.Open();
            //        String query = "SELECT * FROM tblIn WHERE ponumber = '" + label27.Text + "'";
            //        SqlCommand cmd = new SqlCommand(query, tblIn);
            //        rdr = cmd.ExecuteReader();

            //        if (rdr.Read())
            //        {
            //            idpo = (rdr["Id"].ToString());
            //        }
            //        tblIn.Close();
            //    }
            //}

        }
        public void load()
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {

                itemCode.Open();
                DataTable dt = new DataTable();
                string list = "Select productcode,mfgcode,description,qty,unit,Id,iitem,cost,selling,stored,total,icode from tblDRitemCode where icode = '" + itemid + "' order by Id asc";
                SqlCommand command = new SqlCommand(list, itemCode);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                itemCode.Close();
                //this.dataGridView1.Sort(this.dataGridView1.Columns[8], ListSortDirection.Descending);
                itemCode.Dispose();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
            }
        }
        private void loaddb()
        {

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {

                    codeMaterial.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM codeMaterial WHERE product_code = '" + row.Cells[3].Value + "'", codeMaterial);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        row.Cells[1].Value = item["stocksleft"].ToString();
                    }
                    codeMaterial.Close();
                    codeMaterial.Dispose();
                }
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(@"C:\Users\Edwin\AppData\Local\GLU\Product.xml");
                //XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("Table");
                //foreach (XmlNode node in nodeList)
                //{
                //    if (row.Cells[3].Value.ToString() == node.SelectSingleNode("product_code").InnerText)
                //    {
                //        row.Cells[1].Value = node.SelectSingleNode("stocksleft").InnerText;
                //    }
                //}
            }


        }
        private void count()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[6].Value != null || row.Cells[6].Value.ToString() != "" || row.Cells[6].Value != DBNull.Value || row.Cells[1].Value != null || row.Cells[1].Value.ToString() != "")
                {
                    decimal sum = 0.00M;
                    sum += Convert.ToDecimal(row.Cells[1].Value) - Convert.ToDecimal(row.Cells[6].Value);

                    //if (sum <= -0.01M)
                    //{
                    //    row.Cells[2].Value = "Out of Stock";
                    //    row.Cells[2].Style.ForeColor = Color.LightCoral;
                    //}
                    //else
                    //{
                        row.Cells[2].Value = sum.ToString();
                        row.Cells[2].Style.ForeColor = Color.White;
                    //}
                }
                else if (String.IsNullOrEmpty(row.Cells[6].Value as String))
                {
                    row.Cells[6].Value = "0.00";
                }
                else
                {
                    row.Cells[6].Value = "a";
                }

            }
        }
        public void loadscan()
        {

            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                DataTable dt = new DataTable();
                string list = "SELECT image,id FROM tblOutImage WHERE idsup='" + id + "' ";
                SqlCommand command = new SqlCommand(list, dbDR);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                dbDR.Close();

                this.dataGridView3.Sort(this.dataGridView3.Columns[0], ListSortDirection.Ascending);
                dbDR.Dispose();
                dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView3.RowHeadersVisible = false;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 6)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
             && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            //if (e.KeyChar == '.'
            //&& (sender as TextBox).Text.IndexOf('.') > -1)
            //{
            //    e.Handled = true;
            //}
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[6].Value != null || dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() != "")
                {
                    if (e.Value.ToString() != "")
                    {
                        decimal var1 = decimal.Parse(e.Value.ToString());
                        e.Value = var1.ToString("N2");
                    }
                    else if (String.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[6].Value as String))
                    {
                        e.Value = "0.00";
                    }
                    else
                    {
                        e.Value = "0.00";
                    }
                }
            }
            //if (e.ColumnIndex == 12 && e.RowIndex != this.dataGridView1.NewRowIndex)
            //{
            //    if (dataGridView1.Rows[e.RowIndex].Cells[12].Value != null)
            //    {
            //        double var1 = double.Parse(e.Value.ToString());
            //        e.Value = var1.ToString("N2");
            //    }
            //}
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (operationcheck != "Completed" || num == "2")
            {
                count();
            }

        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Validate the CompanyName entry by disallowing empty strings.
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column5")
            {
                if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText =
                        "Qty must not be empty";
                    e.Cancel = true;
                }
            }

        }
        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.
            dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            scan s = new scan();
            s.idsup = id;
            s.num = "2";
            s.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            guna2TextBox2.Visible = false;
            guna2TextBox1.Visible = false;
            guna2TextBox3.Visible = false;

            button3.Visible = false;
            button2.Visible = false;
            button4.Visible = true;
            button1.Visible = true;
            label31.Visible = false;
            label33.Text = "Change D.R. No.";
            guna2TextBox2.Visible = false;
            label7.Visible = false;
            label32.Visible = false;
            label7.Text = "Exist";
            getinfo();
            load();
            //loaddb();
            dataGridView1.Columns["Column5"].ReadOnly = true;
            dataGridView1.Columns["Column8"].Visible = false;
            dataGridView1.Columns["Column7"].ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SuspendLayout();
            Cursor.Current = Cursors.WaitCursor;
            loaddb();


            label31.Visible = true;

            guna2TextBox2.Visible = true;
            guna2TextBox1.Visible = true;
            guna2TextBox3.Visible = true;

            button3.Visible = true;
            button2.Visible = true;
            button4.Visible = false;
            button1.Visible = false;
            dataGridView1.Columns["Column5"].ReadOnly = false;
            dataGridView1.Columns["Column7"].ReadOnly = false;
            dataGridView1.Columns["Column8"].Visible = true;
            Cursor.Current = Cursors.Default;
            ResumeLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //connectionstring6 = ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString;
            //SqlConnection dbDR = new SqlConnection(connectionstring6);

            //connectionstring3 = ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString;
            //SqlConnection itemCode = new SqlConnection(connectionstring3);
            try
            {
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    string completepo = "Editing D.R information";

                    dbDR.Open();
                    string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@date", date);
                    insCmd.Parameters.AddWithValue("@operation", completepo);
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    dbDR.Close();
                }

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblDR set drnumber=@drnumber,additionalinfo=@additionalinfo,projectcode=@projectcode,projectname=@projectname,qty=@qty,totalitems=@totalitems,totalamount=@totalamount where Id=@Id", dbDR);

                    dbDR.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@additionalinfo", guna2TextBox3.Text);
                    cmd.Parameters.AddWithValue("@projectcode", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@projectname", label11.Text);
                    cmd.Parameters.AddWithValue("@qty", label17.Text);
                    cmd.Parameters.AddWithValue("@totalitems", label18.Text);
                    cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2NumericUpDown2.Text));
                    cmd.ExecuteNonQuery();
                    dbDR.Close();
                    dbDR.Dispose();
                }
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        SqlCommand cmd4 = new SqlCommand("update tblDRitemCode set projectcode=@projectcode,projectname=@projectname,drnumber=@drnumber where icode=@icode", itemCode);
                        ;
                        cmd4.Parameters.AddWithValue("@icode", icode);
                        cmd4.Parameters.AddWithValue("@projectcode", guna2TextBox1.Text);
                        cmd4.Parameters.AddWithValue("@projectname", label11.Text);
                        cmd4.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                        cmd4.ExecuteNonQuery();
                    }
                    itemCode.Close();
                }
                //label1.Text = (rdr["drnumber"].ToString());
                //textBox1.Text = (rdr["drnumber"].ToString());
                //textBox2.Text = (rdr["additionalinfo"].ToString());
                //textBox4.Text = (rdr["additionalinfo"].ToString());
                //label27.Text = (rdr["ponumber"].ToString());
                //textBox5.Text = (rdr["ponumber"].ToString());
                //label29.Text = (rdr["podate"].ToString());
                //textBox6.Text = (rdr["podate"].ToString());
                //label12.Text = (rdr["projectcode"].ToString());
                //textBox3.Text = (rdr["projectcode"].ToString());
                //label11.Text = (rdr["projectname"].ToString());
                //label20.Text = (rdr["datetime"].ToString());
                //label17.Text = (rdr["qty"].ToString());
                //label18.Text = (rdr["totalitems"].ToString());
                //label4.Text = (rdr["totalamount"].ToString());
                //operationcheck = (rdr["operation"].ToString());
                //label19.Text = (rdr["createdby"].ToString());
                //label25.Text = (rdr["purchasecompletedby"].ToString());
                //icode = (rdr["itemcode"].ToString());
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblDRitemCode WHERE Id = '" + dataGridView2.Rows[i].Cells[0].Value + "'", itemCode))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    itemCode.Close();
                }



            }
            catch (SystemException ex)
            {
                MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
            }

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    using (SqlCommand command = new SqlCommand("UPDATE tblDRitemCode SET qty=@qty,unit=@unit,total=@total WHERE Id='" + item.Cells[8].Value + "'", itemCode))
                    {
                        command.Parameters.Clear();
                        //command.Parameters.AddWithValue("@Id",);
                        command.Parameters.AddWithValue("@qty", item.Cells[6].Value);
                        command.Parameters.AddWithValue("@unit", item.Cells[7].Value);
                        command.Parameters.AddWithValue("@total", item.Cells[13].Value);
                        command.ExecuteNonQuery();
                    }
                }
                itemCode.Close();
                itemCode.Dispose();
            }


            label31.Visible = false;
            guna2TextBox2.Visible = false;
            guna2TextBox1.Visible = false;
            guna2TextBox3.Visible = false;

            button3.Visible = false;
            button2.Visible = false;
            button4.Visible = true;
            button1.Visible = true;
           label33.Text = "Change D.R. No.";
            guna2TextBox2.Visible = false;
            label7.Visible = false;
            label32.Visible = false;
            label7.Text = "Exist";
            dataGridView1.Columns["Column5"].ReadOnly = true;
            dataGridView1.Columns["Column8"].Visible = false;
            dataGridView1.Columns["Column7"].ReadOnly = true;

            getinfo();
            load();
            MessageBox.Show("Edited Successfully...");
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }
        private string stocks;
        private string total;
        private string operation = "Completed";
        private string nostocks;
        private void button4_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "" || guna2TextBox1.Text == "No Project Code Found")
            {
                MessageBox.Show("Please enter a valid project code..");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to complete this delivery?", "Completion", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        var sb = new System.Text.StringBuilder();
                        dbDR.Open();
                        DataTable dt = new DataTable();
                        String query = "SELECT * FROM tblDR WHERE id = '" + id + "'";
                        SqlCommand cmd = new SqlCommand(query, dbDR);
                        rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            operationcheck = (rdr["operation"].ToString());
                        }
                        dbDR.Close();
                    }
                    if (operationcheck == "Completed")
                    {
                        MessageBox.Show("This D.R. is completed already...");
                        button1.Visible = false;
                        button4.Visible = false;
                        pictureBox14.Visible = true;
                    }
                    else
                    {

                        loaddb();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (Convert.ToDecimal(row.Cells[2].Value.ToString()) <= -0.01M)
                            {
                                DialogResult dialogResult1 = MessageBox.Show("Some of the items doesn't have enough stocks. Do you want to continue?", "Completion", MessageBoxButtons.YesNo,MessageBoxIcon.Error);
                                if (dialogResult1 == DialogResult.Yes)
                                {
                                    break;
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        String searchValue = "Out of Stock";
                        int rowIndex = -1;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[2].Value.ToString().Equals(searchValue))
                            {
                                nostocks = "No Stocks";
                                MessageBox.Show("Some of the items doesn't have enough stocks. Please update the item to continue this item..");
                                rowIndex = row.Index;
                                break;
                            }
                            else
                            {
                                nostocks = "";
                            }
                        }
                        if (nostocks == "" || nostocks == null)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    codeMaterial.Open();
                                    String query = "SELECT * FROM codeMaterial WHERE ID = '" + dataGridView1.Rows[i].Cells[9].Value.ToString() + "'";
                                    SqlCommand cmd2 = new SqlCommand(query, codeMaterial);
                                    rdr = cmd2.ExecuteReader();

                                    if (rdr.Read())
                                    {
                                        stocks = (rdr["stocksleft"].ToString());
                                    }
                                    else
                                    {
                                        stocks = "0.00";
                                    }
                                    codeMaterial.Close();
                                }
                                decimal sum2 = 0.00M;

                                sum2 = Convert.ToDecimal(stocks.ToString()) - Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);

                                total = Math.Round((decimal)Convert.ToDecimal(sum2), 2).ToString();

                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                                    codeMaterial.Open();
                                    cmd.Parameters.AddWithValue("@ID", dataGridView1.Rows[i].Cells[9].Value.ToString());
                                    cmd.Parameters.AddWithValue("@stocksleft", total);
                                    cmd.ExecuteNonQuery();
                                    codeMaterial.Close();
                                    codeMaterial.Dispose();
                                }

                                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                                {
                                    string p = "DR";
                                    SqlCommand cmd4 = new SqlCommand("update tblDRitemCode set typeofp=@typeofp,drnumber=@drnumber,drid=@drid,stocksleft=@stocksleft where icode=@icode", itemCode);
                                    itemCode.Open();
                                    cmd4.Parameters.AddWithValue("@icode", dataGridView1.Rows[i].Cells[14].Value.ToString());
                                    cmd4.Parameters.AddWithValue("@typeofp", p);
                                    cmd4.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                                    cmd4.Parameters.AddWithValue("@drid", id);
                                    cmd4.Parameters.AddWithValue("@stocksleft", total);
                                    cmd4.ExecuteNonQuery();
                                    itemCode.Close();
                                    itemCode.Dispose();
                                }
                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    codeMaterial.Open();
                                    string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date]) values" +
                                        " (@podrid,@type,@itemid,@date)";
                                    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                    insCmd.Parameters.Clear();
                                    insCmd.Parameters.AddWithValue("@podrid", id);
                                    insCmd.Parameters.AddWithValue("@type", "DR");
                                    insCmd.Parameters.AddWithValue("@itemid", dataGridView1.Rows[i].Cells[9].Value.ToString());
                                    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                    int affectedRows = insCmd.ExecuteNonQuery();
                                    codeMaterial.Close();
                                }
                                stocks = "";
                            }

                            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                SqlCommand cmd3 = new SqlCommand("update tblDR set operation=@operation,purchasecompletedby=@purchasecompletedby where Id=@Id", dbDR);
                                dbDR.Open();
                                cmd3.Parameters.AddWithValue("@Id", id);
                                cmd3.Parameters.AddWithValue("@operation", operation);
                                cmd3.Parameters.AddWithValue("@purchasecompletedby", name);
                                cmd3.ExecuteNonQuery();
                                dbDR.Close();
                            }
                            string completepo = "Completed D.R";
                            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                dbDR.Open();
                                string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                                    " (@name,@date,@operation,@id)";
                                SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                                insCmd.Parameters.AddWithValue("@name", name);
                                insCmd.Parameters.AddWithValue("@date", date);
                                insCmd.Parameters.AddWithValue("@operation", completepo);
                                insCmd.Parameters.AddWithValue("@id", id);
                                int affectedRows = insCmd.ExecuteNonQuery();
                                dbDR.Close();

                                dbDR.Dispose();
                            }

                            //SqlDataAdapter adapter;
                            //DataSet ds = new DataSet();
                            //string sql = null;
                            //using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            //{
                            //    sql = "select * from codeMaterial";
                            //    try
                            //    {
                            //        codeMaterial.Open();
                            //        adapter = new SqlDataAdapter(sql, codeMaterial);
                            //        adapter.Fill(ds);
                            //        ds.WriteXml(@"C:\Users\Edwin\AppData\Local\GLU\Product.xml");
                            //        codeMaterial.Close();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        MessageBox.Show(ex.ToString());

                            //    }
                            //}

                            guna2TextBox2.Visible = false;
                            guna2TextBox1.Visible = false;
                            guna2TextBox3.Visible = false;
                            label31.Visible = false;

                            button3.Visible = false;
                            button2.Visible = false;
                            button4.Visible = true;
                            button1.Visible = true;
                           label33.Text = "Change D.R. No.";
                            guna2TextBox2.Visible = false;
                            label7.Visible = false;
                            label32.Visible = false;
                            label7.Text = "Exist";
                            getinfo();
                            load();
                            dataGridView1.Columns["Column5"].ReadOnly = true;
                            dataGridView1.Columns["Column8"].Visible = false;
                            dataGridView1.Columns["Column7"].ReadOnly = true;
                            MessageBox.Show("Record Updated Successfully");
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (num=="1") 
            {
                if (dataGridView1.Columns["Column8"].ReadOnly == false)
                {
                    var senderGrid = (DataGridView)sender;
                    if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                        e.RowIndex >= 0)
                    {
                        int n = dataGridView2.Rows.Add();
                        dataGridView2.Rows[n].Cells[0].Value = dataGridView1.CurrentRow.Cells[8].Value;
                        DataGridViewRow dgvDelRow = dataGridView1.CurrentRow;
                        dataGridView1.Rows.Remove(dgvDelRow);



                        if (dataGridView1.Rows.Count > 0)
                        {
                            count();

                            int b = dataGridView1.Rows.Count;
                            label18.Text = b.ToString();

                            decimal sum = 0.00M;
                            decimal sum3 = 0.00M;
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value) * 1;

                                label17.Text = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");

                                sum3 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[13].Value) * 1;

                                guna2NumericUpDown2.Text = Math.Round((decimal)Convert.ToDecimal(sum3), 2).ToString("N2");
                            }
                        }
                        else
                        {
                            label18.Text = "0";
                            label17.Text = "0";
                            guna2NumericUpDown2.Text = "0";
                        }
                    }

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            DateTime d = DateTime.Now;
            date = d.ToString("yyyy-MM-dd");
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            drhistory po = new drhistory();
            po.id = id;
            po.ShowDialog();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            viewpics v = new viewpics();
            v.id = dataGridView3.CurrentRow.Cells[1].Value.ToString();
            var data = (Byte[])(dataGridView3.CurrentRow.Cells[0].Value);
            var stream = new MemoryStream(data);
            v.pictureBox4.Image = Image.FromStream(stream);
            v.num = "2";
            v.pictureBox11.Visible = false;

            v.ShowDialog();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
          
        }
        private void getprojectcode()
        {
            //connectionstring5 = ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString;
            //SqlConnection otherDB = new SqlConnection(connectionstring5);
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                DataTable dt = new DataTable();
                String query = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = '" + guna2TextBox1.Text.Replace("'", "''") + "'";
                SqlCommand cmd = new SqlCommand(query, otherDB);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label11.Text = (rdr["ACCTDESC"].ToString());
                    label11.ForeColor = Color.White;
                }
                else
                {
                    label11.Text = "No Project Code Found";
                    label11.ForeColor = Color.FromArgb(255, 128, 128);
                }
                otherDB.Close();
                otherDB.Dispose();
            }
        }
        public string idpo;
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //    if ((bool)button2.Visible == true)
            //    {
            //        if (textBox5.Text != "")
            //        {
            //            //connectionstring = ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString;
            //            //SqlConnection tblIn = new SqlConnection(connectionstring);
            //            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            //            {
            //                tblIn.Open();
            //                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblIn] WHERE ([ponumber] = @ponumber)", tblIn);
            //                check_User_Name.Parameters.AddWithValue("@ponumber", textBox5.Text);
            //                int UserExist = (int)check_User_Name.ExecuteScalar();

            //                if (UserExist > 0)
            //                {
            //                    label3.Text = "P.O Number Exist";
            //                    label3.Visible = true;

            //                    DataTable dt = new DataTable();
            //                    String query = "SELECT * FROM tblIn WHERE ponumber = '" + textBox5.Text + "'";
            //                    SqlCommand cmd = new SqlCommand(query, tblIn);
            //                    rdr = cmd.ExecuteReader();

            //                    if (rdr.Read())
            //                    {
            //                        textBox6.Text = (rdr["datetime"].ToString());
            //                        idpo = (rdr["Id"].ToString());
            //                        textBox6.ReadOnly = true;
            //                    }
            //                }
            //                else
            //                {
            //                    label3.Text = "Exist";
            //                    label3.Visible = false;
            //                    textBox6.ReadOnly = false;
            //                    textBox6.Text = "";
            //                }

            //                tblIn.Close();
            //                tblIn.Dispose();
            //            }
            //        }
            //        else
            //        {
            //            textBox6.Text = "";
            //            textBox6.ReadOnly = false;
            //            idpo = "";
            //        }
            //    }

        }

        private void label13_Click(object sender, EventArgs e)
        {
            viewpo v = new viewpo();
            v.num = "2";
            v.ShowDialog();
        }

        private void label31_Click(object sender, EventArgs e)
        {
            procode p = new procode();
            p.createdby = name;
            p.num = "3";
            p.ShowDialog();
        }

        private void label33_Click(object sender, EventArgs e)
        {
            if (label33.Text == "Click Here to Save")
            {
                if (label7.Text == "D.R. Number Exist")
                {
                    MessageBox.Show("D.R is already used...");
                }
                else
                {
                    save();

                }
            }
            else
            {
                guna2TextBox2.Visible = true;
                label33.Text = "Click Here to Save";
                label32.Visible = true;
            }
        }
        private void save()
        {
            label33.Text = "Change D.R. No.";
            guna2TextBox2.Visible = false;

            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                string completepo = "Editing D.R. from " + label1.Text + " to " + guna2TextBox2.Text;

                dbDR.Open();
                string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                    " (@name,@date,@operation,@id)";
                SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@name", name);
                insCmd.Parameters.AddWithValue("@date", date);
                insCmd.Parameters.AddWithValue("@operation", completepo);
                insCmd.Parameters.AddWithValue("@id", id);
                int affectedRows = insCmd.ExecuteNonQuery();
                dbDR.Close();

                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();

                    SqlCommand cmd4 = new SqlCommand("update tblDRitemCode set drnumber=@drnumber,projectcode=@projectcode,projectname=@projectname where icode=@icode", itemCode);
                    
                    cmd4.Parameters.AddWithValue("@icode", icode);
                    cmd4.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                    cmd4.Parameters.AddWithValue("@projectcode", guna2TextBox1.Text);
                    cmd4.Parameters.AddWithValue("@projectname", label11.Text);
                    cmd4.ExecuteNonQuery();
                    itemCode.Close();
                }
                SqlCommand cmd = new SqlCommand("update tblDR set drnumber=@drnumber where Id=@Id", dbDR);

                dbDR.Open();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                cmd.ExecuteNonQuery();
                dbDR.Close();
                dbDR.Dispose();
                label7.Text = "Exist";
                label7.Visible = false;
                label32.Visible = false;
                //obj.loadprein();
                getinfo();
                MessageBox.Show("Edited Successfully...");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label32_Click(object sender, EventArgs e)
        {
            label33.Text = "Change D.R. No.";
            guna2TextBox2.Visible = false;
            label7.Visible = false;
            label32.Visible = false;
            label7.Text = "Exist";
        }

        private void label34_Click(object sender, EventArgs e)
        {
           
        }

        private void label35_Click(object sender, EventArgs e)
        {
         
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (guna2TextBox1.Text == "" || guna2TextBox1.Text == "No Project Code Found")
            {
                MessageBox.Show("Please enter a valid project code..");
            }
            else if (label7.Text == "D.R. Number Exist")
            {
                MessageBox.Show("Please enter a valid DR Number..");
            }
            else
            {
                loaddb();
                String searchValue = "Out of Stock";
                int rowIndex = -1;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[2].Value.ToString().Equals(searchValue))
                    {
                        nostocks = "No Stocks";
                        MessageBox.Show("Some of the items doesn't have enough stocks. Please update the item to continue or change the quantity..");
                        rowIndex = row.Index;
                        break;
                    }
                    else
                    {
                        nostocks = "";
                    }
                }
                if (nostocks == "" || nostocks == null)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to complete this delivery?", "Completion", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        savedr();
                        MessageBox.Show("Success");
                        this.Close();
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }
        public void random1()
        {
            Random rnd2 = new Random();
            int month1 = rnd2.Next(1, 1000000);  // creates a number between 1 and 12
            int dice1 = rnd2.Next(1, 1000000);   // creates a number between 1 and 6
            int card1 = rnd2.Next(1, 10000000);
            randomitemcode = month1.ToString() + dice1.ToString() + card1.ToString();
        }
        string operation2 = "Incomplete";
        public string po;
        private void savedr()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string insStmt = "insert into tblDR ([drnumber], [additionalinfo], [itemcode], [projectcode], [projectname], [datetime], [operation], [qty], [totalitems], [totalamount], [createdby]) values" +
                 " (@drnumber,@additionalinfo,@itemcode,@projectcode,@projectname,@datetime,@operation,@qty,@totalitems,@totalamount,@createdby)";
                SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                insCmd.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                insCmd.Parameters.AddWithValue("@additionalinfo", guna2TextBox3.Text);
                insCmd.Parameters.AddWithValue("@itemcode", randomitemcode);
                insCmd.Parameters.AddWithValue("@projectcode", guna2TextBox1.Text);
                insCmd.Parameters.AddWithValue("@projectname", label11.Text);
                insCmd.Parameters.AddWithValue("@datetime", date);
                insCmd.Parameters.AddWithValue("@operation", operation2);
                insCmd.Parameters.AddWithValue("@qty", label17.Text);
                insCmd.Parameters.AddWithValue("@totalitems", label18.Text);
                insCmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2NumericUpDown2.Text));
                insCmd.Parameters.AddWithValue("@createdby", name);
                int affectedRows = insCmd.ExecuteNonQuery();
                dbDR.Close();
                dbDR.Dispose();
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    decimal sum = 0.00M;
                    sum = Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value.ToString()) - Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    string insStmt2 = "insert into tblDRitemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[stored],[total],[createdby],[projectcode],[projectname]) values" +
                                  " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@stored,@total,@createdby,@projectcode,@projectname)";

                    SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);

                    insCmd2.Parameters.AddWithValue("@productcode", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@mfgcode", dataGridView1.Rows[i].Cells[4].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@description", dataGridView1.Rows[i].Cells[5].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@unit", dataGridView1.Rows[i].Cells[7].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@qty", dataGridView1.Rows[i].Cells[6].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@icode", randomitemcode);
                    insCmd2.Parameters.AddWithValue("@iitem", dataGridView1.Rows[i].Cells[9].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@stocksleft", sum);
                    insCmd2.Parameters.AddWithValue("@cost", dataGridView1.Rows[i].Cells[10].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@selling", dataGridView1.Rows[i].Cells[11].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@stored", dataGridView1.Rows[i].Cells[12].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@total", dataGridView1.Rows[i].Cells[13].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@createdby", name);
                    insCmd2.Parameters.AddWithValue("@projectcode", guna2TextBox1.Text);
                    insCmd2.Parameters.AddWithValue("@projectname", label11.Text);
                    insCmd2.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                    insCmd2.ExecuteNonQuery();
                    itemCode.Close();
                }
               
            }
        }
        private void pictureBox11_Click_1(object sender, EventArgs e)
        {
            itemlist i = new itemlist();
            i.icode = icode;
            i.name = name;
            i.drnumber = label1.Text;
            i.projectcode = label12.Text;
            i.projectname = label11.Text;
            i.qty = label17.Text;
            i.totalitems = label18.Text;
            i.totalamount = guna2NumericUpDown2.Text;
            i.Id = id;
            i.idpo = idpo;
            i.operation = operationcheck;
            i.sv = label3.Text;
            i.no = "2";
            i.ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                label11.Text = "Please Enter Project Code";
                label11.ForeColor = Color.FromArgb(255, 128, 128);
            }
            else if ((bool)button2.Visible == true || num == "2")
            {
                getprojectcode();
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if ((bool)button2.Visible == true || label33.Text == "Click Here to Save" || num == "2")
            {
                if (guna2TextBox2.Text != "")
                {
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE ([drnumber] = @drnumber)", dbDR);
                        check_User_Name.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {
                            if (guna2TextBox2.Text == label1.Text)
                            {
                                label7.Text = "D.R. is the same";
                                label7.Visible = true;
                            }
                            else
                            {
                                label7.Text = "D.R. Number Exist";
                                label7.Visible = true;
                            }
                        }
                        else
                        {
                            label7.Text = "Exist";
                            label7.Visible = false;
                        }
                        dbDR.Close();
                        dbDR.Dispose();
                    }
                }
                else
                {
                    label7.Text = "Exist";
                    label7.Visible = false;
                }

            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {

                string sqlTrunc = "TRUNCATE TABLE printDR";
                SqlCommand cmd = new SqlCommand(sqlTrunc, itemCode);
                itemCode.Open();
                cmd.ExecuteNonQuery();
                itemCode.Close();

            }
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                   
                        string insStmt2 = "insert into printDR ([productcode],[qty],[unit],[description],[Id]) values" +
                                      " (@productcode,@qty,@unit,@description,@Id)";

                        SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);

                        insCmd2.Parameters.AddWithValue("@productcode", row.Cells["dataGridViewTextBoxColumn4"].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@qty", row.Cells["Column5"].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@unit", row.Cells["Column7"].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@description", row.Cells["dataGridViewTextBoxColumn6"].Value.ToString());
                        insCmd2.Parameters.AddWithValue("@Id", id);
                       
                        insCmd2.ExecuteNonQuery();

                }
                itemCode.Close();
                itemCode.Dispose();
            }
            //print p = new print();
            reportviewerqq p = new reportviewerqq();
            p.id = itemid;
            p.drid = id;
            p.ShowDialog();
        }

        private void label27_Click(object sender, EventArgs e)
        {
            addpoproj a = new addpoproj();
            a.id = id;
            a.name = name;
            a.ShowDialog();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {
            if (guna2NumericUpDown2.Text == "")
            {
                MessageBox.Show("Please add Total Amount");
            }
            else
            {

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblDR set totalamount=@totalamount where Id=@Id", dbDR);

                    dbDR.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@totalamount", guna2NumericUpDown2.Text);
                    cmd.ExecuteNonQuery();
                    dbDR.Close();
                }
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    DateTime date = DateTime.Now;
                    string completepo = "changing Total Amount to " + guna2NumericUpDown2.Text + "";
                    dbDR.Open();
                    string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@date", date.ToString("MM/dd/yyyy HH:mm:ss"));
                    insCmd.Parameters.AddWithValue("@operation", completepo);
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    dbDR.Close();
                }
                MessageBox.Show("Done");

            }
        }
    }
}
