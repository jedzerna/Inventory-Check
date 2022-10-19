using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class itemlist : Form
    {
        public string num;
        public string no;
        public string sv;
        public string icode;

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
        public itemlist()
        {
            InitializeComponent();
        }

        static void SetDoubleBuffer(Control ctl, bool DoubleBuffered)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, ctl, new object[] { DoubleBuffered });
        }
        private void itemlist_Load(object sender, EventArgs e)
        {
            color();

            Cursor.Current = Cursors.WaitCursor;
            SuspendLayout();
            loadall();
            SetDoubleBuffer(dataGridView2, DoubleBuffered = true);
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();

            if(no == "1")
            {
                guna2TextBox4.Visible = false;
                label4.Visible = false;
            }
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label12.ForeColor = Color.White;
                label13.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                label15.ForeColor = Color.White;
                label16.ForeColor = Color.White;
                label17.ForeColor = Color.White;


                dataGridView2.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;

                guna2TextBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox1.ForeColor = Color.White;
                guna2TextBox2.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox2.ForeColor = Color.White;
                guna2TextBox3.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox3.ForeColor = Color.White;
                guna2TextBox4.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox4.ForeColor = Color.White;
                guna2TextBox5.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox5.ForeColor = Color.White;

                date.ForeColor = Color.White;
                supp.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);


                guna2Panel1.FillColor = Color.White;

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                label12.ForeColor = Color.Black;
                label13.ForeColor = Color.Black;
                label14.ForeColor = Color.Black;
                label15.ForeColor = Color.Black;
                label16.ForeColor = Color.Black;
                label17.ForeColor = Color.Black;

                dataGridView2.BackgroundColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;

                guna2TextBox1.FillColor = Color.White;
                guna2TextBox1.ForeColor = Color.Black; 
                guna2TextBox1.FillColor = Color.White;
                guna2TextBox1.ForeColor = Color.Black;
                guna2TextBox2.FillColor = Color.White;
                guna2TextBox2.ForeColor = Color.Black;
                guna2TextBox3.FillColor = Color.White;
                guna2TextBox3.ForeColor = Color.Black;
                guna2TextBox4.FillColor = Color.White;
                guna2TextBox4.ForeColor = Color.Black;
                guna2TextBox5.FillColor = Color.White;
                guna2TextBox5.ForeColor = Color.Black;


                date.ForeColor = Color.Black;
                supp.ForeColor = Color.Black;
            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        DataTable dt = new DataTable();
        public void loadall()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                dt.Rows.Clear();
                string list = "Select ID,product_code,description,stocksleft,selling,cost,unit,dept from codeMaterial order by ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                codeMaterial.Close();
            }
        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }
        private string iitem;
        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
        out_supply obj1 = (out_supply)Application.OpenForms["out_supply"];
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

           
            // do stuff

        }
        SqlDataReader rdr;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }
        public string idpo;
        public string drnumber;
        public string name;
        public string ponumber;
        public string projectcode;
        public string projectname;
        public string qty;
        public string totalitems;
        public string totalamount;
        public string Id;
        public string operation;
        public string itemid;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (label1.Text == "ABC0000000")
            {
                MessageBox.Show("Please select an item!!!");
            }
            else
            {
                
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {

                    codeMaterial.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT * FROM codeMaterial WHERE product_code = '" + label1.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, codeMaterial);
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        label3.Text = (rdr["stocksleft"].ToString());
                    }
                    codeMaterial.Close();
                    codeMaterial.Dispose();
                }

              
                if (no == "1")
                {
                    //obj.load();
                    bool found = false;

                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter a2 = new SqlDataAdapter("Select productcode from itemCode where icode = '" + itemid + "'", itemCode))
                            a2.Fill(dt);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dt;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (dataGridView2.CurrentRow.Cells["Column2"].Value.ToString() == item["productcode"].ToString())
                            {
                                found = true; 
                                DialogResult dialogResult = MessageBox.Show("You already added this item, Click Yes to continue", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    found = false;
                                }
                                else
                                {
                                    found = true;
                                }
                                break; 
                            }
                          
                        }
                        itemCode.Close();
                    }
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {

                        codeMaterial.Open();
                        DataTable dt = new DataTable();
                        String query = "SELECT * FROM codeMaterial WHERE product_code = '" + label1.Text + "'";
                        SqlCommand cmd = new SqlCommand(query, codeMaterial);
                        rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            label3.Text = (rdr["stocksleft"].ToString());
                        }
                        codeMaterial.Close();
                    }

                    //foreach (DataGridViewRow row in obj.dataGridView1.Rows)
                    //{


                    //}
                    if (found == false)
                    {

                        if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                        {

                            decimal sum1 = 0.00M;
                            sum1 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                            label16.Text = Math.Round(sum1, 2).ToString();
                        }
                        bool completed = false;
                        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                        {
                            itemCode.Open();

                            string insStmt2 = "insert into itemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[total],[createdby],[ponumber],[typeofp],[poid]) values" +
                                          " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@total,@createdby,@ponumber,@typeofp,@poid)";

                            SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);

                            insCmd2.Parameters.AddWithValue("@productcode", label1.Text);
                            insCmd2.Parameters.AddWithValue("@mfgcode", "");
                            insCmd2.Parameters.AddWithValue("@description", label2.Text);
                            insCmd2.Parameters.AddWithValue("@unit", guna2TextBox2.Text);
                            insCmd2.Parameters.AddWithValue("@qty", guna2TextBox3.Text);
                            insCmd2.Parameters.AddWithValue("@icode", icode);
                            insCmd2.Parameters.AddWithValue("@iitem", iitem);
                            insCmd2.Parameters.AddWithValue("@stocksleft", label16.Text);
                            insCmd2.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                            insCmd2.Parameters.AddWithValue("@selling", "0.00");
                            insCmd2.Parameters.AddWithValue("@total", label13.Text);
                            insCmd2.Parameters.AddWithValue("@createdby", name);
                            insCmd2.Parameters.AddWithValue("@ponumber", ponumber);
                            if (operation == "Completed")
                            {
                                completed = true;
                                string po = "PO";
                                insCmd2.Parameters.AddWithValue("@typeofp", po);

                                insCmd2.Parameters.AddWithValue("@poid", Id);

                            }
                            else if (operation == "Incomplete")
                            {
                                completed = false;
                                insCmd2.Parameters.AddWithValue("@typeofp", "");
                                insCmd2.Parameters.AddWithValue("@poid", "");
                            }
                            insCmd2.ExecuteNonQuery();

                            itemCode.Close();

                            if (completed == true)
                            {
                                string peritemid = "";

                                SqlCommand LIST = new SqlCommand("SELECT max(Id) FROM [itemCode] WHERE ponumber = @ponumber", itemCode);
                                LIST.Parameters.AddWithValue("@ponumber",ponumber);
                                itemCode.Open();
                                Int32 max = (Int32)LIST.ExecuteScalar();
                                //MessageBox.Show(max.ToString());
                                itemCode.Close();

                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    SqlCommand cmd4 = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where product_code=@product_code", codeMaterial);
                                    codeMaterial.Open();
                                    cmd4.Parameters.AddWithValue("@product_code", label1.Text);
                                    cmd4.Parameters.AddWithValue("@stocksleft", label16.Text);
                                    cmd4.ExecuteNonQuery();
                                    codeMaterial.Close();
                                }
                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    codeMaterial.Open();
                                    string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date],[peritemid]) values" +
                                        " (@podrid,@type,@itemid,@date,@peritemid)";
                                    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                    insCmd.Parameters.Clear();
                                    insCmd.Parameters.AddWithValue("@podrid", Id);
                                    insCmd.Parameters.AddWithValue("@type", "PO");
                                    insCmd.Parameters.AddWithValue("@itemid", iitem);
                                    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                    insCmd.Parameters.AddWithValue("@peritemid", max);
                                    int affectedRows = insCmd.ExecuteNonQuery();
                                    codeMaterial.Close();
                                }


                                SqlCommand LISTa = new SqlCommand("SELECT max(sortingid) FROM [tblSIitems] WHERE poid = @poid", itemCode);
                                LISTa.Parameters.AddWithValue("@poid", Id);
                                itemCode.Open();
                                Int32 maxa = (Int32)LISTa.ExecuteScalar();
                                itemCode.Close();


                                itemCode.Open();
                                string insStmta = "insert into tblSIitems ([poid], [productcode],[description],[unit],[qty],[iitem],[cost],[total],[si],[charging],[sortingid],[idperitem]) values" +
                                    " (@poid,@productcode,@description,@unit,@qty,@iitem,@cost,@total,@si,@charging,@sortingid,@idperitem)";
                                SqlCommand insCmda = new SqlCommand(insStmta, itemCode);
                                insCmda.Parameters.Clear();
                                insCmda.Parameters.AddWithValue("@poid", Id);
                                insCmda.Parameters.AddWithValue("@productcode", label1.Text);
                                insCmda.Parameters.AddWithValue("@description", label2.Text);
                                insCmda.Parameters.AddWithValue("@unit", guna2TextBox2.Text);
                                insCmda.Parameters.AddWithValue("@qty", guna2TextBox3.Text);
                                insCmda.Parameters.AddWithValue("@iitem", iitem);
                                insCmda.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                                insCmda.Parameters.AddWithValue("@total", label13.Text);
                                insCmda.Parameters.AddWithValue("@si", "");
                                insCmda.Parameters.AddWithValue("@charging", "");
                                insCmda.Parameters.AddWithValue("@sortingid", maxa+1);
                                insCmda.Parameters.AddWithValue("@idperitem", max);
                                int affectedRowsa = insCmda.ExecuteNonQuery();
                                itemCode.Close();


                            }

                        }


                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            decimal qty1 = 0.00M;
                            qty1 += Convert.ToDecimal(qty) + Convert.ToDecimal(guna2TextBox3.Text);

                            decimal totalitems1 = 0.00M;
                            totalitems1 += Convert.ToDecimal(totalitems) + 1;

                            decimal totalamount1 = 0.00M;
                            totalamount1 += Convert.ToDecimal(totalamount) + Convert.ToDecimal(label13.Text);


                            SqlCommand cmd4 = new SqlCommand("update tblIn set qty=@qty,totalitems=@totalitems where Id=@Id", tblIn);
                            tblIn.Open();
                            cmd4.Parameters.AddWithValue("@Id", Id);
                            cmd4.Parameters.AddWithValue("@qty", qty1);
                            cmd4.Parameters.AddWithValue("@totalitems", totalitems1);
                            cmd4.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }

                        DateTime date = DateTime.Now;
                        string completepo = "Adding " + label1.Text + "";

                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                                " (@name,@date,@operation,@id)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                            insCmd.Parameters.Clear();
                            insCmd.Parameters.AddWithValue("@name", name);
                            insCmd.Parameters.AddWithValue("@date", date.ToString("MM/dd/yyyy HH:mm:ss"));
                            insCmd.Parameters.AddWithValue("@operation", completepo);
                            insCmd.Parameters.AddWithValue("@id", Id);
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblIn.Close();
                        }


                        obj.getinfo();
                        obj.load();
                        obj.count();
                        obj.notbalancae();

                        MessageBox.Show("Done");
                        //this.Close();
                    }
                    
                }
                else if (no == "2")
                {
                    if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                    {

                        decimal sum1 = 0.00M;
                        sum1 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                        label16.Text = Math.Round(sum1, 2).ToString();
                    }
                    //bool save = false;
                    if (Convert.ToDouble(label16.Text) < 0.00)
                    {
                        DialogResult dialogResult = MessageBox.Show("Out of stocks, Do you want to continue?.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                    }

                    //if(save == false)
                    //{

                    obj1.load();




                    bool found = false;

                    foreach (DataGridViewRow row in obj1.dataGridView1.Rows)
                    {
                        if (dataGridView2.CurrentRow.Cells["Column2"].Value.ToString() == row.Cells["dataGridViewTextBoxColumn4"].Value.ToString())
                        {
                            found = true;
                            DialogResult dialogResult = MessageBox.Show("You already added this item, Click Yes to continue", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResult == DialogResult.Yes)
                            {
                                found = false;
                            }
                            else
                            {
                                found = true;
                            }
                            break;
                        }

                    }
                    if (found == false)
                    {

                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {

                            codeMaterial.Open();
                            DataTable dt = new DataTable();
                            String query = "SELECT * FROM codeMaterial WHERE product_code = '" + label1.Text + "'";
                            SqlCommand cmd = new SqlCommand(query, codeMaterial);
                            rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                label3.Text = (rdr["stocksleft"].ToString());
                            }
                            codeMaterial.Close();
                        }

                        if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                        {

                            decimal sum1 = 0.00M;
                            sum1 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                            label16.Text = Math.Round(sum1, 2).ToString();
                        }
                        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                        {
                            itemCode.Open();

                            string insStmt2 = "insert into tblDRitemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[total],[createdby],[projectcode],[projectname],[typeofp],[drnumber],[drid],[sv],[stored]) values" +
                                          " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@total,@createdby,@projectcode,@projectname,@typeofp,@drnumber,@drid,@sv,@stored)";

                            SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);

                            SqlParameter productcode = insCmd2.Parameters.AddWithValue("@productcode", label1.Text);
                            if (label1.Text == null)
                            {
                                productcode.Value = DBNull.Value;
                            }
                            SqlParameter mfgcode = insCmd2.Parameters.AddWithValue("@mfgcode", "");
                            if (label1.Text == null)
                            {
                                mfgcode.Value = DBNull.Value;
                            }
                            SqlParameter description = insCmd2.Parameters.AddWithValue("@description", label2.Text);
                            if (label2.Text == null)
                            {
                                description.Value = DBNull.Value;
                            }
                            SqlParameter unit = insCmd2.Parameters.AddWithValue("@unit", guna2TextBox2.Text);
                            if (guna2TextBox2.Text == null)
                            {
                                unit.Value = DBNull.Value;
                            }
                            SqlParameter qty = insCmd2.Parameters.AddWithValue("@qty", guna2TextBox3.Text);
                            if (guna2TextBox3.Text == null)
                            {
                                qty.Value = DBNull.Value;
                            }
                            SqlParameter icode1 = insCmd2.Parameters.AddWithValue("@icode", icode);
                            if (icode == null)
                            {
                                icode1.Value = DBNull.Value;
                            }
                            SqlParameter iitem1 = insCmd2.Parameters.AddWithValue("@iitem", iitem);
                            if (iitem == null)
                            {
                                iitem1.Value = DBNull.Value;
                            }
                            SqlParameter stocksleft = insCmd2.Parameters.AddWithValue("@stocksleft", label16.Text);
                            if (label16.Text == null)
                            {
                                stocksleft.Value = DBNull.Value;
                            }
                            SqlParameter cost = insCmd2.Parameters.AddWithValue("@cost", guna2TextBox5.Text);
                            if (guna2TextBox5.Text == null)
                            {
                                cost.Value = DBNull.Value;
                            }
                            SqlParameter selling = insCmd2.Parameters.AddWithValue("@selling", guna2TextBox4.Text);
                            if (guna2TextBox4.Text == null)
                            {
                                selling.Value = DBNull.Value;
                            }

                            SqlParameter total = insCmd2.Parameters.AddWithValue("@total", label13.Text);
                            if (label13.Text == null)
                            {
                                total.Value = DBNull.Value;
                            }
                            SqlParameter createdby = insCmd2.Parameters.AddWithValue("@createdby", name);
                            if (name == null)
                            {
                                createdby.Value = DBNull.Value;
                            }


                            SqlParameter projectcode1 = insCmd2.Parameters.AddWithValue("@projectcode", projectcode);
                            if (projectcode == null)
                            {
                                projectcode1.Value = DBNull.Value;
                            }

                            SqlParameter projectname1 = insCmd2.Parameters.AddWithValue("@projectname", projectname);
                            if (projectname == null)
                            {
                                projectname1.Value = DBNull.Value;
                            }
                            bool completed = false;
                            if (operation == "Completed")
                            {
                                completed = true;
                                string po = "DR";
                                insCmd2.Parameters.AddWithValue("@typeofp", po);
                                insCmd2.Parameters.AddWithValue("@drnumber", drnumber);
                                insCmd2.Parameters.AddWithValue("@drid", Id);

                            }
                            else if (operation == "Incomplete")
                            {
                                completed = false;
                                insCmd2.Parameters.AddWithValue("@typeofp", DBNull.Value);
                                insCmd2.Parameters.AddWithValue("@drnumber", DBNull.Value);
                                insCmd2.Parameters.AddWithValue("@drid", DBNull.Value);
                            }
                            insCmd2.Parameters.AddWithValue("@sv", sv);
                            SqlParameter stored = insCmd2.Parameters.AddWithValue("@stored", label14.Text);
                            if (label14.Text == null)
                            {
                                stored.Value = DBNull.Value;
                            }
                            insCmd2.ExecuteNonQuery();

                            itemCode.Close();


                            if (completed == true)
                            {
                                string peritemid = "";

                                SqlCommand LIST = new SqlCommand("SELECT max(Id) FROM [tblDRitemCode] WHERE icode = @icode", itemCode);
                                LIST.Parameters.AddWithValue("@icode", icode);
                                itemCode.Open();
                                Int32 max = (Int32)LIST.ExecuteScalar();
                                //MessageBox.Show(max.ToString());
                                itemCode.Close();


                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    SqlCommand cmd4 = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where product_code=@product_code", codeMaterial);
                                    codeMaterial.Open();
                                    cmd4.Parameters.AddWithValue("@product_code", label1.Text);
                                    cmd4.Parameters.AddWithValue("@stocksleft", label16.Text);
                                    cmd4.ExecuteNonQuery();
                                    codeMaterial.Close();
                                }
                                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                                {
                                    codeMaterial.Open();
                                    string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date], [peritemid]) values" +
                                        " (@podrid,@type,@itemid,@date,@peritemid)";
                                    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                    insCmd.Parameters.Clear();
                                    insCmd.Parameters.AddWithValue("@podrid", Id);
                                    insCmd.Parameters.AddWithValue("@type", "DR");
                                    insCmd.Parameters.AddWithValue("@itemid", iitem);
                                    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                    insCmd.Parameters.AddWithValue("@peritemid", max);
                                    int affectedRows = insCmd.ExecuteNonQuery();
                                    codeMaterial.Close();
                                }
                            }
                        }

                        using (SqlConnection tblDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            decimal qty1 = 0.00M;
                            qty1 += Convert.ToDecimal(qty) + Convert.ToDecimal(guna2TextBox3.Text);

                            decimal totalitems1 = 0.00M;
                            totalitems1 += Convert.ToDecimal(totalitems) + 1;

                            decimal totalamount1 = 0.00M;
                            totalamount1 += Convert.ToDecimal(totalamount) + Convert.ToDecimal(label13.Text);


                            SqlCommand cmd4 = new SqlCommand("update tblDR set qty=@qty,totalitems=@totalitems,totalamount=@totalamount where Id=@Id", tblDR);
                            tblDR.Open();
                            cmd4.Parameters.AddWithValue("@Id", Id);
                            cmd4.Parameters.AddWithValue("@qty", qty1);
                            cmd4.Parameters.AddWithValue("@totalitems", totalitems1);
                            cmd4.Parameters.AddWithValue("@totalamount", totalamount1);
                            cmd4.ExecuteNonQuery();
                            tblDR.Close();
                        }

                        DateTime date = DateTime.Now;
                        string completepo = "Adding '" + label1.Text + "'";

                        using (SqlConnection tblDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            tblDR.Open();
                            string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                                " (@name,@date,@operation,@id)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblDR);
                            insCmd.Parameters.Clear();
                            insCmd.Parameters.AddWithValue("@name", name);
                            insCmd.Parameters.AddWithValue("@date", date.ToString("yyyy/MM/dd HH:mm:ss"));
                            insCmd.Parameters.AddWithValue("@operation", completepo);
                            insCmd.Parameters.AddWithValue("@id", Id);
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblDR.Close();
                        }

                        obj1.getinfo();
                        obj1.load();
                        //outsup.load();
                        MessageBox.Show("Done");
                        //this.Close();
                    }
                    //}

                }
                label1.Text = "ABC0000000";
                label16.Text = "0.00";
                label3.Text = "0.00";
                label13.Text = "0.00";
                label14.Text = "";
                label2.Text = "";
                guna2TextBox3.Text = "0.00";
                guna2TextBox2.Text = "";
                guna2TextBox4.Text = "0.00";
                guna2TextBox5.Text = "0.00";
            }
        }
        private out_supply outsup = null;
        public itemlist(Form callingForm)
        {
            outsup = callingForm as out_supply;

            InitializeComponent();
        }

        private void guna2TextBox3_MouseLeave(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_Leave(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2NumericUpDown1_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "")
            {
                guna2TextBox3.Text = "0.00";
            }
            else
            {
                if (no == "1")
                {
                    if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum3 = 0.00M;
                        sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                        label16.Text = Math.Round(sum3, 2).ToString();
                    }
                }
                else
                {
                    if (guna2TextBox4.Text != "" || guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum2 = 0.00M;
                        sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                        label16.Text = Math.Round(sum2, 2).ToString();
                    }
                }
            }
        }

        private void guna2NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void guna2NumericUpDown2_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text == "")
            {
                guna2TextBox5.Text = "0.00";
            }
            if (no == "1")
            {
                if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2TextBox4.Text != "" || guna2TextBox3.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void guna2NumericUpDown3_Leave(object sender, EventArgs e)
        {
            if (no == "1")
            {
                if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2TextBox4.Text != "" || guna2TextBox3.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void guna2NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (no == "1")
            {
                if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2TextBox4.Text != "" || guna2TextBox3.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2NumericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {
           
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "")
            {
                if (date.Checked == true)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("product_code LIKE '{0}%'", guna2TextBox1.Text.Replace("'", "''"));

                }
                else if (supp.Checked == true)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("description LIKE '%{0}%'", guna2TextBox1.Text.Replace("'", "''"));
                }
            }
        }

        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox3.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text != "")
            {

                if (no == "1")
                {
                    if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum3 = 0.00M;
                        sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                        label16.Text = Math.Round(sum3, 2).ToString();
                    }
                }
                else
                {
                    if (guna2TextBox4.Text != "" || guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum2 = 0.00M;
                        sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                        label16.Text = Math.Round(sum2, 2).ToString();
                    }
                }
            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text != "")
            {
                if (no == "1")
                {
                    if (guna2TextBox5.Text != "" && guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum3 = 0.00M;
                        sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                        label16.Text = Math.Round(sum3, 2).ToString();
                    }
                }
                else
                {
                    if (guna2TextBox4.Text != "" && guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum2 = 0.00M;
                        sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                        label16.Text = Math.Round(sum2, 2).ToString();
                    }
                }
            }
        }

        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox4.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text != "")
            {
                if (no == "1")
                {
                    if (guna2TextBox5.Text != "" || guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox5.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum3 = 0.00M;
                        sum3 += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                        label16.Text = Math.Round(sum3, 2).ToString();
                    }
                }
                else
                {
                    if (guna2TextBox4.Text != "" || guna2TextBox3.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2TextBox3.Text), 2) * Math.Round(decimal.Parse(guna2TextBox4.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum2 = 0.00M;
                        sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2TextBox3.Text), 2);
                        label16.Text = Math.Round(sum2, 2).ToString();
                    }
                }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (no == "1")
            {
                bool exist = false;
                foreach (DataGridViewRow row in obj.dataGridView1.Rows)
                {
                    if (dataGridView2.CurrentRow.Cells["Column2"].Value.ToString() == row.Cells["one"].Value.ToString())
                    {
                        exist = true;
                        break;
                    }

                }

                if (exist == true)
                {
                    DialogResult dialogResult = MessageBox.Show("You already added this item, Click Yes to continue", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {

                        label1.Text = dataGridView2.CurrentRow.Cells["Column2"].Value.ToString();
                        label2.Text = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                        iitem = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
                        label3.Text = dataGridView2.CurrentRow.Cells["Column6"].Value.ToString();
                        //label4.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                        guna2TextBox5.Text = dataGridView2.CurrentRow.Cells["Column7"].Value.ToString();
                        guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column9"].Value.ToString();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {

                    label1.Text = dataGridView2.CurrentRow.Cells["Column2"].Value.ToString();
                    label2.Text = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                    iitem = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
                    label3.Text = dataGridView2.CurrentRow.Cells["Column6"].Value.ToString();
                    //label4.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    guna2TextBox5.Text = dataGridView2.CurrentRow.Cells["Column7"].Value.ToString();
                    guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column9"].Value.ToString();
                }


              
                    //label1.Text = dataGridView2.CurrentRow.Cells["Column2"].Value.ToString();
                    //label2.Text = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                    //iitem = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
                    //label3.Text = dataGridView2.CurrentRow.Cells["Column6"].Value.ToString();
                    ////label4.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    //guna2TextBox5.Text = dataGridView2.CurrentRow.Cells["Column7"].Value.ToString();
                    //guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column9"].Value.ToString();
                
            }
            else
            {
                //bool found = false;
                //foreach (DataGridViewRow row in obj1.dataGridView1.Rows)
                //{
                //    if (dataGridView2.CurrentRow.Cells["Column2"].Value.ToString() == row.Cells["dataGridViewTextBoxColumn4"].Value.ToString())
                //    {
                //        MessageBox.Show("This product already exist");
                //        found = true;
                //        break; // get out of the loop
                //    }

                //}

                bool exist = false;
                foreach (DataGridViewRow row in obj1.dataGridView1.Rows)
                {
                    if (dataGridView2.CurrentRow.Cells["Column2"].Value.ToString() == row.Cells["dataGridViewTextBoxColumn4"].Value.ToString())
                    {
                        exist = true;
                        break;
                    }

                }

                if (exist == true)
                {
                    DialogResult dialogResult = MessageBox.Show("You already added this item, Click Yes to continue", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        label1.Text = dataGridView2.CurrentRow.Cells["Column2"].Value.ToString();
                        label2.Text = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                        iitem = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
                        label3.Text = dataGridView2.CurrentRow.Cells["Column6"].Value.ToString();
                        guna2TextBox5.Text = dataGridView2.CurrentRow.Cells["Column7"].Value.ToString();
                        guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column9"].Value.ToString();
                        label14.Text = dataGridView2.CurrentRow.Cells["Column10"].Value.ToString();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    label1.Text = dataGridView2.CurrentRow.Cells["Column2"].Value.ToString();
                    label2.Text = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                    iitem = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
                    label3.Text = dataGridView2.CurrentRow.Cells["Column6"].Value.ToString();
                    guna2TextBox5.Text = dataGridView2.CurrentRow.Cells["Column7"].Value.ToString();
                    guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column9"].Value.ToString();
                    label14.Text = dataGridView2.CurrentRow.Cells["Column10"].Value.ToString();
                }
            }
        }

        private void guna2TextBox3_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "0.00")
            {
                guna2TextBox3.Text = "";
            }
        }

        private void guna2TextBox5_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text == "0.00")
            {
                guna2TextBox5.Text = "";
            }
        }

        private void guna2TextBox4_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text == "0.00")
            {
                guna2TextBox4.Text = "";
            }
        }

        private void guna2TextBox3_Leave_1(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "")
            {
                guna2TextBox3.Text = "0.00";
            }
        }

        private void guna2TextBox5_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox5.Text == "")
            {
                guna2TextBox5.Text = "0.00";
            }
        }

        private void guna2TextBox4_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text == "")
            {
                guna2TextBox4.Text = "0.00";
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
            SuspendLayout();
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
            ResumeLayout();
            thread = null;
        }
        public void addition()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
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
                string listA = "Select ID,product_code,description,stocksleft,selling,cost,unit,dept from codeMaterial WHERE ID BETWEEN '" + idindt + "' AND '" + maxId + "'";
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
                        newRow[0] = rows["ID"].ToString();
                        newRow[1] = rows["product_code"].ToString();
                        newRow[2] = rows["description"].ToString();
                        newRow[3] = rows["stocksleft"].ToString();
                        newRow[4] = rows["selling"].ToString();
                        newRow[5] = rows["cost"].ToString();
                        newRow[6] = rows["unit"].ToString();
                        newRow[7] = rows["dept"].ToString();
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
        }
        Thread thread;
        int rowscount;

        private void itemlist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
                timer2.Stop();
            }
        }

        int rowIndex;
        int col;
        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    rowIndex = dataGridView2.SelectedCells[0].OwningRow.Index;
                    col = dataGridView2.CurrentCell.ColumnIndex;

                    dataGridView2.FirstDisplayedScrollingRowIndex = rowIndex;

                    if (rowIndex < dataGridView2.Rows.Count - 1)
                    {
                        dataGridView2.ClearSelection();
                        this.dataGridView2.Rows[rowIndex + 1].Cells[col].Selected = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    rowIndex = dataGridView2.SelectedCells[0].OwningRow.Index;
                    col = dataGridView2.CurrentCell.ColumnIndex;
                    if (rowIndex >= 1)
                    {
                        dataGridView2.FirstDisplayedScrollingRowIndex = rowIndex - 1;
                    }
                    if (rowIndex > 0)
                    {
                        dataGridView2.ClearSelection();
                        this.dataGridView2.Rows[rowIndex - 1].Cells[col].Selected = true;
                    }
                }
            }
        }
    }
}
