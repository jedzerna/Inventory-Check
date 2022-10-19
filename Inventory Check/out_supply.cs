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


        //SqlDataReader rdr;
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
        private outlist form1;
        private dashboard dash1;
        private DRLdrview drldrview;
        public out_supply(outlist form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }
        public out_supply(dashboard dash)
        {
            InitializeComponent();
            this.dash1 = dash;
        }
        public out_supply(DRLdrview dash)
        {
            InitializeComponent();
            this.drldrview = dash;
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
            bb = true;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            SuspendLayout();
            pictureBox5.InitialImage = null;
            pictureBox14.InitialImage = null;
            pictureBox15.InitialImage = null;
            if (num == "1")
            {
                Cursor.Current = Cursors.WaitCursor;
                getinfo();
                load();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                getinfo();
                load();
                Cursor.Current = Cursors.Default;
            }
            Application.EnableVisualStyles();
            //if (num == "drldrview")
            //{
            //    guna2Button1.Visible = false;
            //    guna2Button6.Visible = false;
            //    label33.Visible = false;
            //    label38.Visible = false;
            //    label35.Visible = false; 
            //    guna2Button8.Visible = false;
            //}

            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView2.RowHeadersVisible = false;

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;

            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblPending] WHERE type = @type AND docid = @docid", otherDB);
                check_User_Name.Parameters.AddWithValue("@type", "DR");
                check_User_Name.Parameters.AddWithValue("@docid", id);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist > 0)
                {
                    guna2Button11.Visible = true;
                }
                else
                {
                    guna2Button12.Visible = true;
                }
                otherDB.Close();
            }
            //load();
            ResumeLayout();
            bb = false;
            label1.Focus();
            count();
            updatea();
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
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
                    guna2TextBox1.Text = (rdr["projectcode"].ToString());
                    label11.Text = (rdr["projectname"].ToString());
                    label17.Text = (rdr["qty"].ToString());
                    label20.Text = (rdr["datetime"].ToString());
                    label18.Text = (rdr["totalitems"].ToString());
                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));
                    operationcheck = (rdr["operation"].ToString());
                    label19.Text = (rdr["createdby"].ToString());
                    //label25.Text = (rdr["purchasecompletedby"].ToString());
                    icode = (rdr["itemcode"].ToString());
                    if (rdr["operation"].ToString() == "Completed")
                    {
                        guna2Button3.Visible = false;
                    }
                    else
                    {
                        guna2Button3.Visible = true;
                    }
                }

                tblIn.Close();
                tblIn.Dispose();

                //loaddb();


                label31.Visible = true;

                guna2TextBox2.Visible = true;
                guna2TextBox1.Visible = true;
                pictureBox14.Visible = false;
                label33.Visible = false;
                //button3.Visible = false;
                guna2Button7.Visible = false;
                guna2Button8.Visible = false;
                guna2Button1.Visible = false;
                dataGridView1.Columns["Column5"].ReadOnly = false;
                //dataGridView1.Columns["Column7"].ReadOnly = false;
                dataGridView1.Columns["Column6"].ReadOnly = false;

                dataGridView1.Columns["Column8"].Visible = true;
            }

        }


        private string amount;

        public void getinfo()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                var sb = new System.Text.StringBuilder();
                dbDR.Open();
                String query = "SELECT * FROM tblDR WHERE id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, dbDR);
               SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label1.Text = (rdr["drnumber"].ToString());
                    guna2TextBox2.Text = (rdr["drnumber"].ToString());
                    guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
                    label12.Text = (rdr["projectcode"].ToString());
                    guna2TextBox1.Text = (rdr["projectcode"].ToString());
                    label11.Text = (rdr["projectname"].ToString());
                    label20.Text = DateTime.Parse(rdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                    label17.Text = (rdr["qty"].ToString());
                    label18.Text = (rdr["totalitems"].ToString());
                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));
                    amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));
                    operationcheck = (rdr["operation"].ToString());
                    label19.Text = (rdr["createdby"].ToString());
                    label25.Text = (rdr["purchasecompletedby"].ToString());
                    icode = (rdr["itemcode"].ToString());
                    label3.Text = (rdr["sv"].ToString());

                    if (rdr["subcode"].ToString() != "")
                    {
                        label21.Visible = true;
                        label30.Visible = true;
                        label38.Visible = true;
                        label21.Text = rdr["subcode"].ToString() +" || "+ rdr["subname"].ToString();
                        guna2TextBox5.Text = rdr["subcode"].ToString();
                        guna2TextBox6.Text = rdr["subname"].ToString();
                    }

                    if (rdr["dateentered"].ToString() != "")
                    {
                        label4.Text = DateTime.Parse(rdr["dateentered"].ToString()).ToString("MM/dd/yyyy");
                    }
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
                    guna2Button8.Visible = false;
                    pictureBox14.Visible = true;
                    guna2Button3.Visible = false;
                    //button1.Visible = true;
                }
                else
                {
                    label33.Visible = false;
                    guna2Button3.Visible = true;
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

                if (dt.Rows.Count == 0)
                {
                    guna2Button3.Visible = true;
                }
            }

        }
        private void loaddb()
        {

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();
                    String query = "SELECT * FROM codeMaterial WHERE ID = '" + row.Cells["iditem"].Value.ToString() + "'";
                    SqlCommand cmd2 = new SqlCommand(query, codeMaterial);
                   SqlDataReader rdr = cmd2.ExecuteReader();

                    if (rdr.Read())
                    {
                        row.Cells["Column3"].Value = Convert.ToDecimal(rdr["stocksleft"].ToString());
                    }
                    else
                    {
                        row.Cells["Column3"].Value = 0.00M;
                    }
                    codeMaterial.Close();
                }



                if (row.Cells[6].Value != null || row.Cells[6].Value.ToString() != "" || row.Cells[6].Value != DBNull.Value || row.Cells[1].Value != null || row.Cells[1].Value.ToString() != "")
                {
                    decimal sum = 0.00M;
                    sum += Convert.ToDecimal(row.Cells[1].Value) - Convert.ToDecimal(row.Cells[6].Value);
                    row.Cells[2].Value = sum.ToString();
                    row.Cells[2].Style.ForeColor = Color.White;

                }
                else if (String.IsNullOrEmpty(row.Cells[6].Value as String))
                {
                    row.Cells[6].Value = "0.00";
                }
                //using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                //{

                //    codeMaterial.Open();
                //    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM codeMaterial WHERE product_code = '" + row.Cells[3].Value + "'", codeMaterial);
                //    DataTable dt = new DataTable();
                //    da.Fill(dt);
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        row.Cells["Column3"].Value = item["stocksleft"].ToString();
                //    }
                //    codeMaterial.Close();
                //}
            }


        }
        public void count()
        {
            if (bb == false)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    //if (row.Cells[6].Value != null || row.Cells[6].Value.ToString() != "" || row.Cells[6].Value != DBNull.Value || row.Cells[1].Value != null || row.Cells[1].Value.ToString() != "")
                    //{
                    //    decimal sum = 0.00M;
                    //    sum += Convert.ToDecimal(row.Cells[1].Value) - Convert.ToDecimal(row.Cells[6].Value);
                    //    row.Cells[2].Value = sum.ToString();
                    //    row.Cells[2].Style.ForeColor = Color.White;

                    //}
                    //else if (String.IsNullOrEmpty(row.Cells[6].Value as String))
                    //{
                    //    row.Cells[6].Value = "0.00";
                    //}

                    decimal total = 0.00M;
                    total += Convert.ToDecimal(row.Cells["Column5"].Value) * Convert.ToDecimal(row.Cells["Column6"].Value);
                    row.Cells["Column10"].Value = string.Format("{0:#,##0.00}", total);
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    int b = dataGridView1.Rows.Count;
                    label18.Text = b.ToString();

                    decimal sum = 0.00M;
                    decimal sum3 = 0.00M;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        sum += Convert.ToDecimal(row.Cells["Column5"].Value);

                        label17.Text = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");

                        sum3 += Convert.ToDecimal(row.Cells["Column10"].Value.ToString());
                    }
                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", sum3);
                }
                else
                {
                    label18.Text = "0";
                    label17.Text = "0";
                    guna2TextBox4.Text = "0";
                }


            }
        }
        private void updatea()
        {
            if (upda == false)
            {
                if (amount != guna2TextBox4.Text)
                {
                    if (guna2TextBox4.Text != "")
                    {
                        if (id != "")
                        {
                            MessageBox.Show("System has encountered some disbalances. We are now trying to fix it.");
                            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                SqlCommand cmd = new SqlCommand("update tblDR set totalamount=@totalamount,qty=@qty,totalitems=@totalitems where id=@id", dbDR);

                                dbDR.Open();
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
                                cmd.Parameters.AddWithValue("@qty", label17.Text);
                                cmd.Parameters.AddWithValue("@totalitems", label18.Text);
                                cmd.ExecuteNonQuery();
                                dbDR.Close();
                            }
                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                itemCode.Open();
                                foreach (DataGridViewRow item in dataGridView1.Rows)
                                {
                                    using (SqlCommand command = new SqlCommand("UPDATE tblDRitemCode SET qty=@qty,unit=@unit,total=@total,selling=@selling WHERE Id='" + item.Cells[8].Value + "'", itemCode))
                                    {
                                        command.Parameters.Clear();
                                        //command.Parameters.AddWithValue("@Id",);
                                        command.Parameters.AddWithValue("@qty", item.Cells["Column5"].Value);
                                        command.Parameters.AddWithValue("@unit", item.Cells["Column7"].Value);
                                        command.Parameters.AddWithValue("@total", item.Cells["Column10"].Value);
                                        command.Parameters.AddWithValue("@selling", item.Cells["Column6"].Value);
                                        command.ExecuteNonQuery();
                                    }
                                }
                                itemCode.Close();
                            }
                        }
                    }
                }
            }
        }
        public void update()
        {
            if (upda == false)
            {
                if (amount != guna2TextBox4.Text)
                {
                    if (guna2TextBox4.Text != "")
                    {
                        if (id != "")
                        {
                            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                SqlCommand cmd = new SqlCommand("update tblDR set totalamount=@totalamount,qty=@qty,totalitems=@totalitems where id=@id", dbDR);

                                dbDR.Open();
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
                                cmd.Parameters.AddWithValue("@qty", label17.Text);
                                cmd.Parameters.AddWithValue("@totalitems", label18.Text);
                                cmd.ExecuteNonQuery();
                                dbDR.Close();
                            }
                        }
                    }
                }
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
            else if (dataGridView1.CurrentCell.ColumnIndex == 11)
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
            if (e.ColumnIndex == 11 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[11].Value != null || dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString() != "")
                {
                    if (e.Value.ToString() != "")
                    {
                        decimal var1 = decimal.Parse(e.Value.ToString());
                        e.Value = var1.ToString("N2");
                    }
                    else if (String.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[11].Value as String))
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
        public bool bb = true;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (upda == false)
            {
                //if (operationcheck != "Completed")
                //{
                    count();
                //}
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
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Column6")
            {
                if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText =
                        "Selling must not be empty";
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
        }

        private void button3_Click(object sender, EventArgs e)
        {

           
        }

        private void button1_Click(object sender, EventArgs e)
        {

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //connectionstring6 = ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString;
            //SqlConnection dbDR = new SqlConnection(connectionstring6);

            //connectionstring3 = ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString;
            //SqlConnection itemCode = new SqlConnection(connectionstring3);
          
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }
        //private string stocks;
        //private string total;
        private string operation = "Completed";
        private string nostocks;
        private void button4_Click(object sender, EventArgs e)
        {
           
        }
        bool upda = false;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (num == "2" || num == "drldrview")
            {

                if (dataGridView1.Columns["Column8"].Visible == true)
                {
                    var senderGrid = (DataGridView)sender;
                    if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                        e.RowIndex >= 0)
                    {
                        if (operationcheck == "Completed")
                        {
                            inandoutitemsdeletion a = new inandoutitemsdeletion();
                            a.itempoid = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString();
                            a.iitem = dataGridView1.CurrentRow.Cells["iditem"].Value.ToString();
                            a.itemstocks = dataGridView1.CurrentRow.Cells["Column5"].Value.ToString();
                            a.name = name;
                            a.operation = operationcheck;
                            a.poid = id;
                            a.forms = "outsupply";
                            a.ShowDialog();
                        }
                        else
                        {
                            int n = dataGridView2.Rows.Add();
                            dataGridView2.Rows[n].Cells[0].Value = dataGridView1.CurrentRow.Cells[8].Value;
                            DataGridViewRow dgvDelRow = dataGridView1.CurrentRow;
                            dataGridView1.Rows.Remove(dgvDelRow);
                            upda = true;
                            count();
                            upda = false;
                        }


                    }
                    if (num == "drldrview")
                    {
                        DialogResult dialogResult = MessageBox.Show("You are opening View All Page, Do you want to refresh?", "Refresh", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {

                            Cursor.Current = Cursors.WaitCursor;
                            this.drldrview.loadprein();
                            Cursor.Current = Cursors.Default;
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
        
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                SqlDataReader rdr = cmd.ExecuteReader();

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

                if (num == "drldrview")
                {
                    DialogResult dialogResult = MessageBox.Show("You are opening View All Page, Do you want to refresh?", "Refresh", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        this.drldrview.loadprein();
                        Cursor.Current = Cursors.Default;
                    }
                }
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
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
                insCmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
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
            
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                label11.Text = "Please Enter Project Code";
                label11.ForeColor = Color.FromArgb(255, 128, 128);
            }
            else if ((bool)guna2Button7.Visible == true || num == "2")
            {
                getprojectcode();
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if ((bool)guna2Button7.Visible == true || label33.Text == "Click Here to Save" || num == "2")
            {
                if (guna2TextBox2.Text != "")
                {
                    if (bb == false)
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
            MessageBox.Show("This feature still not available, please wait to the next updates.");
            return;
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
            //if (guna2NumericUpDown2.Text == "")
            //{
            //    MessageBox.Show("Please add Total Amount");
            //}
            //else
            //{

            //    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            //    {
            //        SqlCommand cmd = new SqlCommand("update tblDR set totalamount=@totalamount where Id=@Id", dbDR);

            //        dbDR.Open();
            //        cmd.Parameters.Clear();
            //        cmd.Parameters.AddWithValue("@Id", id);
            //        cmd.Parameters.AddWithValue("@totalamount", guna2NumericUpDown2.Text);
            //        cmd.ExecuteNonQuery();
            //        dbDR.Close();
            //    }
            //    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            //    {
            //        DateTime date = DateTime.Now;
            //        string completepo = "changing Total Amount to " + guna2NumericUpDown2.Text + "";
            //        dbDR.Open();
            //        string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
            //            " (@name,@date,@operation,@id)";
            //        SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
            //        insCmd.Parameters.Clear();
            //        insCmd.Parameters.AddWithValue("@name", name);
            //        insCmd.Parameters.AddWithValue("@date", date.ToString("MM/dd/yyyy HH:mm:ss"));
            //        insCmd.Parameters.AddWithValue("@operation", completepo);
            //        insCmd.Parameters.AddWithValue("@id", id);
            //        int affectedRows = insCmd.ExecuteNonQuery();
            //        dbDR.Close();
            //    }
            //    MessageBox.Show("Done");

            //}
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete? Once deleted cannot be undone.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblDRitemCode WHERE id = '" + row.Cells["Column1"].Value.ToString() + "'", itemCode))
                        {
                            command.ExecuteNonQuery();
                        }
                        itemCode.Close();
                    }
                }

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblDR WHERE Id = '" + id + "'", dbDR))
                    {
                        command.ExecuteNonQuery();
                    }
                    dbDR.Close();
                }

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblDRHistory WHERE id = '" + id + "'", dbDR))
                    {
                        command.ExecuteNonQuery();
                    }
                    dbDR.Close();
                }

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblOutImage WHERE idsup = '" + id + "'", dbDR))
                    {
                        command.ExecuteNonQuery();
                    }
                    dbDR.Close();
                }

                //form1.loadprein();
                if (num == "2")
                {
                    foreach (DataRow row in form1.dt.Rows)
                    {
                        if (row["Id"].ToString() == id)
                        {
                            row.Delete();
                            break;
                        }
                    }
                }
                MessageBox.Show("Record Deleted");
                this.Close();
            }
        }

        outlist obj1 = (outlist)Application.OpenForms["outlist"];
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            drhistory po = new drhistory();
            po.id = id;
            po.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            scan s = new scan();
            s.idsup = id;
            s.num = "2";
            s.ShowDialog();
        }

        public string totalamoun
        {
            get { return guna2TextBox4.Text; }
            set { guna2TextBox4.Text = value; }
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            itemlist i = new itemlist(this);
            i.icode = icode;
            i.name = name;
            i.drnumber = label1.Text;
            i.projectcode = label12.Text;
            i.projectname = label11.Text;
            i.qty = label17.Text;
            i.totalitems = label18.Text;
            i.totalamount = guna2TextBox4.Text;
            i.Id = id;
            i.idpo = idpo;
            i.operation = operationcheck;
            i.sv = label3.Text;
            i.no = "2";
            i.ShowDialog();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            if (operationcheck == "Completed")
            {
                guna2TextBox1.Visible = true;
                guna2Button9.Visible = true;
                guna2Button7.Visible = true;
                dataGridView1.Columns["Column8"].Visible = true;
                dataGridView1.Columns["Column6"].ReadOnly = false;
            }
            else
            {
                SuspendLayout();
                Cursor.Current = Cursors.WaitCursor;
                loaddb();


                label31.Visible = true;

                guna2TextBox2.Visible = true;
                guna2TextBox1.Visible = true;

                guna2Button9.Visible = true;
                guna2Button7.Visible = true;
                guna2Button8.Visible = false;
                guna2Button1.Visible = false;
                dataGridView1.Columns["Column5"].ReadOnly = false;
                //dataGridView1.Columns["Column7"].ReadOnly = false;
                dataGridView1.Columns["Column6"].ReadOnly = false;
                dataGridView1.Columns["Column8"].Visible = true;
                Cursor.Current = Cursors.Default;
                ResumeLayout();
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (operationcheck == "Completed")
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE tblDRitemCode SET qty=@qty,unit=@unit,total=@total,selling=@selling WHERE Id='" + item.Cells[8].Value + "'", itemCode))
                        {
                            command.Parameters.Clear();
                            //command.Parameters.AddWithValue("@Id",);
                            command.Parameters.AddWithValue("@qty", item.Cells["Column5"].Value);
                            command.Parameters.AddWithValue("@unit", item.Cells["Column7"].Value);
                            command.Parameters.AddWithValue("@total", item.Cells["Column10"].Value);
                            command.Parameters.AddWithValue("@selling", item.Cells["Column6"].Value);
                            command.ExecuteNonQuery();
                        }
                    }
                    itemCode.Close();
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
                    cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
                    cmd.ExecuteNonQuery();
                    dbDR.Close();
                    dbDR.Dispose();
                }
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    string completepo = "";
                    if (label12.Text == guna2TextBox1.Text)
                    {
                        completepo = "Editing item selling price";
                    }
                    else
                    {
                        completepo = "Editing PROJ from " + label12.Text + " to " + guna2TextBox1.Text;
                    }

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
                MessageBox.Show("Edited Successfully...");
                getinfo();
                guna2TextBox1.Visible = false;
                guna2Button9.Visible = false;
                guna2Button7.Visible = false;
                dataGridView1.Columns["Column8"].Visible = false;
                dataGridView1.Columns["Column6"].ReadOnly = true;
            }
            else
            {
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
                        cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
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
                        using (SqlCommand command = new SqlCommand("UPDATE tblDRitemCode SET qty=@qty,unit=@unit,total=@total,selling=@selling WHERE Id='" + item.Cells[8].Value + "'", itemCode))
                        {
                            command.Parameters.Clear();
                            //command.Parameters.AddWithValue("@Id",);
                            command.Parameters.AddWithValue("@qty", item.Cells["Column5"].Value);
                            command.Parameters.AddWithValue("@unit", item.Cells["Column7"].Value);
                            command.Parameters.AddWithValue("@total", item.Cells["Column10"].Value);
                            command.Parameters.AddWithValue("@selling", item.Cells["Column6"].Value);
                            command.ExecuteNonQuery();
                        }
                    }
                    itemCode.Close();
                }


                label31.Visible = false;
                guna2TextBox2.Visible = false;
                guna2TextBox1.Visible = false;

                guna2Button9.Visible = false;
                guna2Button7.Visible = false;
                guna2Button8.Visible = true;
                guna2Button1.Visible = true;
                label33.Text = "Change D.R. No.";
                guna2TextBox2.Visible = false;
                label7.Visible = false;
                label32.Visible = false;
                label7.Text = "Exist";
                dataGridView1.Columns["Column5"].ReadOnly = true;
                dataGridView1.Columns["Column6"].ReadOnly = true;
                dataGridView1.Columns["Column8"].Visible = false;
                //dataGridView1.Columns["Column7"].ReadOnly = true;

                upda = true;
                load();
                getinfo();
                upda = false;

                label1.Focus();
                MessageBox.Show("Edited Successfully...");
                if (num == "drldrview")
                {
                    DialogResult dialogResult = MessageBox.Show("You are opening View All Page, Do you want to refresh?", "Refresh", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        this.drldrview.loadprein();
                        Cursor.Current = Cursors.Default;
                    }
                }

            }
               
        }

        private void guna2Button8_Click(object sender, EventArgs e)
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
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            operationcheck = (rdr["operation"].ToString());
                        }
                        dbDR.Close();
                    }
                    if (operationcheck == "Completed")
                    {
                        MessageBox.Show("This D.R. is completed already...");
                        guna2Button1.Visible = false;
                        guna2Button8.Visible = false;
                        pictureBox14.Visible = true;
                    }
                    else
                    {

                        loaddb();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells["dataGridViewTextBoxColumn4"].Value.ToString() != "")
                            {

                                if (Convert.ToDecimal(row.Cells[2].Value.ToString()) <= -0.01M)
                                {
                                    DialogResult dialogResult1 = MessageBox.Show("Some of the items doesn't have enough stocks. Do you want to continue?", "Completion", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
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
                        }
                        //String searchValue = "Out of Stock";
                        //int rowIndex = -1;
                        //foreach (DataGridViewRow row in dataGridView1.Rows)
                        //{
                        //    if (row.Cells["dataGridViewTextBoxColumn4"].Value.ToString() != "")
                        //    {
                        //        if (row.Cells[2].Value.ToString().Equals(searchValue))
                        //        {
                        //            nostocks = "No Stocks";
                        //            MessageBox.Show("Some of the items doesn't have enough stocks. Please update the item to continue this item..");
                        //            rowIndex = row.Index;
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            nostocks = "";
                        //        }
                        //    }
                        //}
                        //if (nostocks == "" || nostocks == null)
                        //{
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {

                            //}
                            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            //    {
                            decimal stocks = 0.00M;
                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                String query = "SELECT * FROM codeMaterial WHERE ID = '" + row.Cells["iditem"].Value.ToString() + "'";
                                SqlCommand cmd2 = new SqlCommand(query, codeMaterial);
                                SqlDataReader rdr = cmd2.ExecuteReader();

                                if (rdr.Read())
                                {
                                    stocks = Convert.ToDecimal(rdr["stocksleft"].ToString());
                                }
                                else
                                {
                                    stocks = 0.00M;
                                }
                                codeMaterial.Close();
                            }
                            decimal sum2 = 0.00M;

                            sum2 += Convert.ToDecimal(stocks.ToString()) - Convert.ToDecimal(row.Cells["Column5"].Value);

                           decimal total = Math.Round(sum2, 2);

                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                                codeMaterial.Open();
                                cmd.Parameters.AddWithValue("@ID", row.Cells["iditem"].Value.ToString());
                                cmd.Parameters.AddWithValue("@stocksleft", total);
                                cmd.ExecuteNonQuery();
                                codeMaterial.Close();
                            }

                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                string p = "DR";
                                SqlCommand cmd4 = new SqlCommand("update tblDRitemCode set typeofp=@typeofp,drnumber=@drnumber,drid=@drid,stocksleft=@stocksleft where id=@id and iitem=@iitem", itemCode);
                                itemCode.Open();
                                cmd4.Parameters.AddWithValue("@id", row.Cells["Column1"].Value.ToString());
                                cmd4.Parameters.AddWithValue("@iitem", row.Cells["iditem"].Value.ToString());
                                cmd4.Parameters.AddWithValue("@typeofp", p);
                                cmd4.Parameters.AddWithValue("@drnumber", guna2TextBox2.Text);
                                cmd4.Parameters.AddWithValue("@drid", id);
                                cmd4.Parameters.AddWithValue("@stocksleft", total);
                                cmd4.ExecuteNonQuery();
                                itemCode.Close();
                            }
                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date], [peritemid]) values" +
                                    " (@podrid,@type,@itemid,@date,@peritemid)";
                                SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                insCmd.Parameters.Clear();
                                insCmd.Parameters.AddWithValue("@podrid", id);
                                insCmd.Parameters.AddWithValue("@type", "DR");
                                insCmd.Parameters.AddWithValue("@itemid", row.Cells["iditem"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                insCmd.Parameters.AddWithValue("@peritemid", row.Cells["Column1"].Value.ToString());
                                int affectedRows = insCmd.ExecuteNonQuery();
                                codeMaterial.Close();
                            }
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
                        label31.Visible = false;

                        guna2Button9.Visible = false;
                        guna2Button7.Visible = false;
                        guna2Button8.Visible = true;
                        guna2Button1.Visible = true;
                        label33.Text = "Change D.R. No.";
                        guna2TextBox2.Visible = false;
                        label7.Visible = false;
                        label32.Visible = false;
                        label7.Text = "Exist";
                        getinfo();
                        load();
                        dataGridView1.Columns["Column5"].ReadOnly = true;
                        dataGridView1.Columns["Column6"].ReadOnly = true;
                        dataGridView1.Columns["Column8"].Visible = false;
                        //dataGridView1.Columns["Column7"].ReadOnly = true;
                        MessageBox.Show("Record Updated Successfully");

                        if (num == "drldrview")
                        {
                            DialogResult dialogResults = MessageBox.Show("You are opening View All Page, Do you want to refresh?", "Refresh", MessageBoxButtons.YesNo);
                            if (dialogResults == DialogResult.Yes)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                this.drldrview.loadprein();
                                Cursor.Current = Cursors.Default;
                            }
                        }
                    }
                    //}
                }
            }
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (operationcheck == "Completed")
            {

                guna2TextBox1.Visible = false;
                guna2Button9.Visible = false;
                guna2Button7.Visible = false;
                guna2Button7.Visible = false;
                dataGridView1.Columns["Column8"].Visible = false;
                dataGridView1.Columns["Column6"].ReadOnly = true;
            }
            else
            {

                guna2TextBox2.Visible = false;
                guna2TextBox1.Visible = false;

                guna2Button9.Visible = false;
                guna2Button7.Visible = false;
                guna2Button8.Visible = true;
                guna2Button1.Visible = true;
                label31.Visible = false;
                label33.Text = "Change D.R. No.";
                guna2TextBox2.Visible = false;
                label7.Visible = false;
                label32.Visible = false;
                label7.Text = "Exist";
                upda = true;
                load();
                getinfo();
                upda = false;
                //loaddb();
                dataGridView1.Columns["Column5"].ReadOnly = true;
                dataGridView1.Columns["Column8"].Visible = false;
                //dataGridView1.Columns["Column7"].ReadOnly = true;
                dataGridView1.Columns["Column6"].ReadOnly = true;

                label1.Focus();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (guna2Button10.Text == "Show Remarks")
            {
                guna2Panel4.Visible = true;
                guna2Panel3.Visible = false;
                guna2Button10.Text = "Back";
            }
            else
            {
                guna2Panel4.Visible = false;
                guna2Panel3.Visible = true;
                guna2Button10.Text = "Show Remarks";
            }
        }
        public string form;
        dashboard obj2 = (dashboard)Application.OpenForms["dashboard"];
        private void guna2Button12_Click(object sender, EventArgs e)
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                string insStmt = "insert into tblPending ([type], [docid]) values" +
                    " (@type,@docid)";
                SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@type", "DR");
                insCmd.Parameters.AddWithValue("@docid", id);
                int affectedRows = insCmd.ExecuteNonQuery();
                otherDB.Close();
            }

            if (num == "1")
            {
              
                    int a = dash1.dataGridView4.Rows.Add();
                    dash1.dataGridView4.Rows[a].Cells["one"].Value = label1.Text;
                    dash1.dataGridView4.Rows[a].Cells["two"].Value = label11.Text;
                    dash1.dataGridView4.Rows[a].Cells["three"].Value = label20.Text;
                    dash1.dataGridView4.Rows[a].Cells["four"].Value = guna2TextBox3.Text;
                    dash1.dataGridView4.Rows[a].Cells["five"].Value = itemid;
                    dash1.dataGridView4.Rows[a].Cells["six"].Value = id;
                
            }

            guna2Button12.Visible = false;
            guna2Button11.Visible = true;
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
           
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM tblPending WHERE type = 'DR' AND docid = '" + id + "'", otherDB))
                {
                    command.ExecuteNonQuery();
                }
                otherDB.Close();
            }
            if (num == "1")
            {
                foreach (DataGridViewRow row in dash1.dataGridView4.Rows)
                {
                    if (row.Cells["six"].Value.ToString() == id)
                    {
                        DataGridViewRow dgvDelRow = row;
                        dash1.dataGridView4.Rows.Remove(dgvDelRow);
                    }
                }
            }
            guna2Button12.Visible = true;
            guna2Button11.Visible = false;
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {
            if (label35.Text == "Edit")
            {
                maskedTextBox1.Text = label20.Text;
                guna2Panel5.Visible = true;
                label37.Visible = true;
                label35.Text = "Save";
            }
            else
            {

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    string completepo = "Editing D.R. Date from " + label20.Text + " to " + maskedTextBox1.Text;

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

                    SqlCommand cmd = new SqlCommand("update tblDR set datetime=@datetime where Id=@Id", dbDR);

                    dbDR.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@datetime", maskedTextBox1.Text);
                    cmd.ExecuteNonQuery();
                    dbDR.Close();

                    getinfo();

                    MessageBox.Show("Edited Successfully...");

                }
                guna2Panel5.Visible = false;
                label37.Visible = false;
                label35.Text = "Edit";
            }

        }

        private void label37_Click(object sender, EventArgs e)
        {
            guna2Panel5.Visible = false;
            label37.Visible = false;
            label35.Text = "Edit";
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            getinfo();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dataGridView1.Rows[1].Cells[1].Value.ToString());
            ////MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());

            //DataTable drdt = new DataTable();
            //using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            //{

            //    itemCode.Open();
            //    string list = "Select DISTINCT icode from tblDRitemCode";
            //    SqlCommand command = new SqlCommand(list, itemCode);
            //    SqlDataReader reader = command.ExecuteReader();
            //    drdt.Load(reader);
            //    itemCode.Close();
            //}
            //foreach (DataRow row in drdt.Rows)
            //{
            //    bool has = false;
            //    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            //    {
            //        dbDR.Open();
            //        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE @icode=@icode", dbDR);

            //        check_User_Name.Parameters.AddWithValue("@icode", row["icode"].ToString());
            //        int UserExist = (int)check_User_Name.ExecuteScalar();

            //        if (UserExist > 0)
            //        {
            //            has = true;
            //        }
            //        else
            //        {
            //            has = false;
            //        }
            //        dbDR.Close();
            //    }
            //    if (has == false)
            //    {
            //        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            //        {
            //            itemCode.Open();
            //            using (SqlCommand command = new SqlCommand("DELETE FROM tblDRitemCode WHERE icode = '" + row["icode"].ToString() + "'", itemCode))
            //            {
            //                command.ExecuteNonQuery();
            //            }
            //            itemCode.Close();
            //        }
            //    }
            //}
            //MessageBox.Show("Done");
        }

        private void label38_Click(object sender, EventArgs e)
        {
            if (label38.Text == "Edit")
            {
                guna2TextBox5.Visible = true;
                guna2TextBox6.Visible = true;
                label21.Visible = false;
                label36.Visible = true;
                label38.Text = "Save";
            }
            else
            {
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    string completepo = "Editing D.R. Customer from " + label21.Text + " to " + guna2TextBox5.Text +" || "+ guna2TextBox6.Text;

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

                    SqlCommand cmd = new SqlCommand("update tblDR set subcode=@subcode,subname=@subname where Id=@Id", dbDR);

                    dbDR.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@subcode", guna2TextBox5.Text);
                    cmd.Parameters.AddWithValue("@subname", guna2TextBox6.Text);
                    cmd.ExecuteNonQuery();
                    dbDR.Close();

                    getinfo();

                    MessageBox.Show("Edited Successfully...");

                }


                guna2TextBox5.Visible = false;
                guna2TextBox6.Visible = false;
                label21.Visible = true;
                label36.Visible = false;
                label38.Text = "Edit";
            }
        }

        private void label36_Click(object sender, EventArgs e)
        {
            guna2TextBox5.Visible = false;
            guna2TextBox6.Visible = false;
            label21.Visible = true;
            label36.Visible = false;
            label38.Text = "Edit";
        }

        private void guna2Button13_Click(object sender, EventArgs e)
        {
            customDR a = new customDR(this);
            a.form = "out_supply";
            a.id = id;
            a.createdby = name;
            a.ShowDialog();
        }
    }
}
