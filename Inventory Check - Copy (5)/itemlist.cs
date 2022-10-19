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

        private void itemlist_Load(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            SuspendLayout();
            loadall();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();

            if(no == "1")
            {
                guna2NumericUpDown3.Visible = false;
                label4.Visible = false;
            }
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void loadall()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                DataTable dt = new DataTable();
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

            if (no == "1")
            {
                bool found = false;
                foreach (DataGridViewRow row in obj.dataGridView1.Rows)
                {
                    if (dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == row.Cells["one"].Value.ToString())
                    {
                        MessageBox.Show("This product already exist");
                        found = true;
                        break; // get out of the loop
                    }

                }
                if (found == false)
                {
                    label1.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString();
                    label2.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn2"].Value.ToString();
                    iitem = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn3"].Value.ToString();
                    label3.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn4"].Value.ToString();
                    //label4.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    guna2NumericUpDown2.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column1"].Value.ToString();
                }
            }
            else
            {
                bool found = false;
                foreach (DataGridViewRow row in obj1.dataGridView1.Rows)
                {
                    if (dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == row.Cells["dataGridViewTextBoxColumn4"].Value.ToString())
                    {
                        MessageBox.Show("This product already exist");
                        found = true;
                        break; // get out of the loop
                    }

                }
                if (found == false)
                {
                    label1.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString();
                    label2.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn2"].Value.ToString();
                    iitem = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn3"].Value.ToString();
                    label3.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn4"].Value.ToString();
                    guna2NumericUpDown2.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                    guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column1"].Value.ToString();
                    label14.Text = dataGridView2.CurrentRow.Cells["bodega"].Value.ToString();
                }
            }
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
                    obj.load();
                    bool found = false;

                    foreach (DataGridViewRow row in obj.dataGridView1.Rows)
                    {
                        if (dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == row.Cells["one"].Value.ToString())
                        {
                            MessageBox.Show("This product already exist");
                            found = true;
                            break; // get out of the loop
                        }

                    }
                    if (found == false)
                    {
                        if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                        {

                            decimal sum1 = 0.00M;
                            sum1 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                            label16.Text = Math.Round(sum1, 2).ToString();
                        }
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
                            insCmd2.Parameters.AddWithValue("@qty", guna2NumericUpDown1.Text);
                            insCmd2.Parameters.AddWithValue("@icode", icode);
                            insCmd2.Parameters.AddWithValue("@iitem", iitem);
                            insCmd2.Parameters.AddWithValue("@stocksleft", label16.Text);
                            insCmd2.Parameters.AddWithValue("@cost", guna2NumericUpDown2.Text);
                            insCmd2.Parameters.AddWithValue("@selling", "0.00");
                            insCmd2.Parameters.AddWithValue("@total", label13.Text);
                            insCmd2.Parameters.AddWithValue("@createdby", name);
                            insCmd2.Parameters.AddWithValue("@ponumber", ponumber);
                            if (operation == "Completed")
                            {
                                string po = "PO";
                                insCmd2.Parameters.AddWithValue("@typeofp", po);

                                insCmd2.Parameters.AddWithValue("@poid", Id);
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
                                    string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date]) values" +
                                        " (@podrid,@type,@itemid,@date)";
                                    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                    insCmd.Parameters.Clear();
                                    insCmd.Parameters.AddWithValue("@podrid", Id);
                                    insCmd.Parameters.AddWithValue("@type", "PO");
                                    insCmd.Parameters.AddWithValue("@itemid", iitem);
                                    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                    int affectedRows = insCmd.ExecuteNonQuery();
                                    codeMaterial.Close();
                                }

                            }
                            else
                            {
                                insCmd2.Parameters.AddWithValue("@typeofp", null);
                                insCmd2.Parameters.AddWithValue("@poid", null);
                            }
                            insCmd2.ExecuteNonQuery();

                            itemCode.Close();


                        }

                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            decimal qty1 = 0.00M;
                            qty1 += Convert.ToDecimal(qty) + Convert.ToDecimal(guna2NumericUpDown1.Text);

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
                        MessageBox.Show("Done");
                        this.Close();
                    }
                    
                }
                else if (no == "2")
                {
                    if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                    {

                        decimal sum1 = 0.00M;
                        sum1 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
                        label16.Text = Math.Round(sum1, 2).ToString();
                    }
                    if (Convert.ToDouble(label16.Text) <0.00)
                    {
                        MessageBox.Show("Out of Stock");
                    }
                    else
                    {
                        obj1.load();
                        bool found = false;

                        foreach (DataGridViewRow row in obj1.dataGridView1.Rows)
                        {
                            if (dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == row.Cells["dataGridViewTextBoxColumn4"].Value.ToString())
                            {
                                MessageBox.Show("This product already exist");
                                found = true;
                                break; // get out of the loop
                            }

                        }
                        if (found == false)
                        {
                            if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                            {

                                decimal sum1 = 0.00M;
                                sum1 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
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
                                SqlParameter qty = insCmd2.Parameters.AddWithValue("@qty", guna2NumericUpDown1.Text);
                                if (guna2NumericUpDown1.Text == null)
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
                                SqlParameter cost = insCmd2.Parameters.AddWithValue("@cost", guna2NumericUpDown2.Text);
                                if (guna2NumericUpDown2.Text == null)
                                {
                                    cost.Value = DBNull.Value;
                                }
                                SqlParameter selling = insCmd2.Parameters.AddWithValue("@selling", guna2NumericUpDown3.Text);
                                if (guna2NumericUpDown3.Text == null)
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
                                if (operation == "Completed")
                                {
                                    string po = "DR";
                                    insCmd2.Parameters.AddWithValue("@typeofp", po);
                                    insCmd2.Parameters.AddWithValue("@drnumber", drnumber);
                                    insCmd2.Parameters.AddWithValue("@drid", Id);

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
                                        string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date]) values" +
                                            " (@podrid,@type,@itemid,@date)";
                                        SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                        insCmd.Parameters.Clear();
                                        insCmd.Parameters.AddWithValue("@podrid", Id);
                                        insCmd.Parameters.AddWithValue("@type", "DR");
                                        insCmd.Parameters.AddWithValue("@itemid", iitem);
                                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                        int affectedRows = insCmd.ExecuteNonQuery();
                                        codeMaterial.Close();
                                    }
                                }
                                else if (operation == "Incomplete")
                                {
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


                            }

                            using (SqlConnection tblDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                decimal qty1 = 0.00M;
                                qty1 += Convert.ToDecimal(qty) + Convert.ToDecimal(guna2NumericUpDown1.Text);

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
                            MessageBox.Show("Done");
                            this.Close();
                        }
                    }

                }
            }
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
            if (guna2NumericUpDown1.Text == "")
            {
                guna2NumericUpDown1.Text = "0.00";
            }
            else
            {
                if (no == "1")
                {
                    if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum3 = 0.00M;
                        sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                        label16.Text = Math.Round(sum3, 2).ToString();
                    }
                }
                else
                {
                    if (guna2NumericUpDown3.Text != "" || guna2NumericUpDown1.Text != "")
                    {
                        decimal sum = 0.00M;
                        sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                        label13.Text = Math.Round(sum, 2).ToString();

                        decimal sum2 = 0.00M;
                        sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
                        label16.Text = Math.Round(sum2, 2).ToString();
                    }
                }
            }
        }

        private void guna2NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (no == "1")
            {
                if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2NumericUpDown3.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (no == "1")
            {
                if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2NumericUpDown3.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void guna2NumericUpDown2_Leave(object sender, EventArgs e)
        {
            if (guna2NumericUpDown2.Text == "")
            {
                guna2NumericUpDown2.Text = "0.00";
            }
            if (no == "1")
            {
                if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2NumericUpDown3.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
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
                if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2NumericUpDown3.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void guna2NumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (no == "1")
            {
                if (guna2NumericUpDown2.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2NumericUpDown3.Text != "" || guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
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
            if (no == "1")
            {
                if (guna2NumericUpDown2.Text != "" && guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown2.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum3 = 0.00M;
                    sum3 += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) + Math.Round(decimal.Parse(label3.Text), 2);
                    label16.Text = Math.Round(sum3, 2).ToString();
                }
            }
            else
            {
                if (guna2NumericUpDown3.Text != "" && guna2NumericUpDown1.Text != "")
                {
                    decimal sum = 0.00M;
                    sum += Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2) * Math.Round(decimal.Parse(guna2NumericUpDown3.Text), 2);



                    label13.Text = Math.Round(sum, 2).ToString();

                    decimal sum2 = 0.00M;
                    sum2 += Math.Round(decimal.Parse(label3.Text), 2) - Math.Round(decimal.Parse(guna2NumericUpDown1.Text), 2);
                    label16.Text = Math.Round(sum2, 2).ToString();
                }
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            search();
        }
        private void search()
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
}
