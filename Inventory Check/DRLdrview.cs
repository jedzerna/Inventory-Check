using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Inventory_Check
{
    public partial class DRLdrview : Form
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
        public DRLdrview()
        {
            InitializeComponent();
        }
        private void DRLdrview_Load(object sender, EventArgs e)
        {
            color();
            Cursor.Current = Cursors.WaitCursor;
            SuspendLayout();
            DataGridViewButtonColumn dgvbt = new DataGridViewButtonColumn();

            dgvbt.HeaderText = "";
            dgvbt.Text = "Complete";
            dgvbt.Name = "okay";
            dgvbt.FlatStyle = FlatStyle.Flat;
            dgvbt.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(dgvbt);



            DataGridViewButtonColumn dgvbt1 = new DataGridViewButtonColumn();
            dgvbt1.HeaderText = "";
            dgvbt1.Text = "View";
            dgvbt1.Name = "view";
            dgvbt1.FlatStyle = FlatStyle.Flat;
            dgvbt1.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(dgvbt1);
            loadprein();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                //label2.ForeColor = Color.White;
                //label22.ForeColor = Color.White;
                //label24.ForeColor = Color.White;
                //label3.ForeColor = Color.White;

                guna2ComboBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox1.ForeColor = Color.White;
                guna2ComboBox1.ItemsAppearance.BackColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox1.ItemsAppearance.ForeColor = Color.White;

                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);

                dataGridView2.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;

                guna2DateTimePicker1.FillColor = Color.FromArgb(36, 37, 37);
                guna2DateTimePicker2.FillColor = Color.FromArgb(36, 37, 37);
                guna2DateTimePicker1.ForeColor = Color.White;
                guna2DateTimePicker2.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);


                guna2ComboBox1.FillColor = Color.White;
                guna2ComboBox1.ForeColor = Color.Black;
                guna2ComboBox1.ItemsAppearance.BackColor = Color.White;
                guna2ComboBox1.ItemsAppearance.ForeColor = Color.Black;


                guna2DateTimePicker1.FillColor = Color.White;
                guna2DateTimePicker2.FillColor = Color.White;
                guna2DateTimePicker1.ForeColor = Color.Black;
                guna2DateTimePicker2.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;


                label1.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                //label2.ForeColor = Color.Black;
                //label22.ForeColor = Color.Black;
                //label24.ForeColor = Color.Black;
                //label3.ForeColor = Color.Black;

                guna2Panel1.FillColor = Color.White;


                dataGridView2.BackgroundColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;

            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void loadprein()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {

                dataGridView2.Rows.Clear();


                DataTable dt = new DataTable();
              
                    dbDR.Open();
                    string list = "Select drnumber,datetime,projectname,operation,Id,itemcode,sv,dateentered,projectcode,projectname,sv,totalamount from tblDR WHERE datetime BETWEEN @date1 AND @date2 AND sv = '"+guna2ComboBox1.Text+"' order by drnumber asc";
                    SqlCommand command = new SqlCommand(list, dbDR);
                    command.Parameters.AddWithValue("@date1", DateTime.Parse(guna2DateTimePicker1.Text).ToString("MM/dd/yyyy"));
                    command.Parameters.AddWithValue("@date2", DateTime.Parse(guna2DateTimePicker2.Text).ToString("MM/dd/yyyy"));
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);


                decimal sumdr = 0.00M;
                decimal sumitem = 0.00M;
                int rr = 0;
                foreach (DataRow row in dt.Rows)
                {
                    sumdr += Convert.ToDecimal(row["totalamount"].ToString());
                    rr++;
                    if (rr > 1)
                    {
                        int ca = dataGridView2.Rows.Add();
                        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                        dataGridView2.Rows[ca].Cells["okay"].Style = dataGridViewCellStyle2;
                        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                        dataGridView2.Rows[ca].Cells["view"].Style = dataGridViewCellStyle2;
                    }


                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["id"].Value = row["Id"].ToString();
                    dataGridView2.Rows[a].Cells["Column0"].Value = "DR No "+row["drnumber"].ToString();
                    dataGridView2.Rows[a].Cells["Column1"].Value = "CODE: "+row["projectcode"].ToString()+Environment.NewLine+ "PROJ: " + row["projectname"].ToString() + Environment.NewLine + "DATE: " + DateTime.Parse(row["datetime"].ToString()).ToString("MM/dd/yyyy") + Environment.NewLine + "SV: " + row["sv"].ToString();
                    dataGridView2.Rows[a].Cells["Column6"].Value = row["operation"].ToString();

                    //dataGridView2.Rows[a].Cells["Column0"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    //dataGridView2.Rows[a].Cells["Column1"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    //dataGridView2.Rows[a].Cells["Column6"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);


                    if (row["operation"].ToString() != "Incomplete")
                    {
                        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                        dataGridView2.Rows[a].Cells["okay"].Style = dataGridViewCellStyle2;


                    }else if (row["operation"].ToString() == "Complete")
                    {
                        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                        dataGridView2.Rows[a].Cells["view"].Style = dataGridViewCellStyle2;
                    }
                    


                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();

                        itemCode.Open();
                        DataTable dts = new DataTable();
                        dts.Rows.Clear();
                        //MessageBox.Show(row["itemcode"].ToString());
                        string lists = "Select productcode,description,qty,unit,Id,iitem,cost,selling,total,icode,stored from tblDRitemCode where icode = '" + row["itemcode"].ToString() + "'";
                        SqlCommand commands = new SqlCommand(lists, itemCode);
                        SqlDataReader readers = commands.ExecuteReader();
                        dts.Load(readers);
                        itemCode.Close();
                        decimal sum = 0.00M;
                        foreach (DataRow rows in dts.Rows)
                        {
                            sumitem += Convert.ToDecimal(rows["total"].ToString());
                            int b = dataGridView2.Rows.Add();
                            dataGridView2.Rows[b].Cells["id"].Value = rows["Id"].ToString();
                            dataGridView2.Rows[b].Cells["Column0"].Value = rows["productcode"].ToString();
                            dataGridView2.Rows[b].Cells["Column1"].Value = rows["description"].ToString();
                            dataGridView2.Rows[b].Cells["Column2"].Value = rows["qty"].ToString();
                            dataGridView2.Rows[b].Cells["Column3"].Value = rows["unit"].ToString();
                            dataGridView2.Rows[b].Cells["Column4"].Value = rows["selling"].ToString();
                            dataGridView2.Rows[b].Cells["Column5"].Value = rows["stored"].ToString();
                            dataGridView2.Rows[b].Cells["Column6"].Value = rows["total"].ToString();

                            dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                            dataGridView2.Rows[b].Cells["okay"].Style = dataGridViewCellStyle2;


                            dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                            dataGridView2.Rows[b].Cells["view"].Style = dataGridViewCellStyle2;
                            sum += Convert.ToDecimal(rows["total"].ToString());

                        }
                        int c = dataGridView2.Rows.Add();
                        dataGridView2.Rows[c].Cells["Column5"].Value = "Total Amount:";
                        dataGridView2.Rows[c].Cells["Column6"].Value = string.Format("{0:#,##0.00}", sum);


                        //DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                        dataGridView2.Rows[c].Cells["okay"].Style = dataGridViewCellStyle2;


                        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                        dataGridView2.Rows[c].Cells["view"].Style = dataGridViewCellStyle2;
                    }

                }
                //dataGridView2.DataSource = dt;


                label3.Text = string.Format("{0:#,##0.00}", sumdr);
                label5.Text = string.Format("{0:#,##0.00}", sumitem);

                dbDR.Close();
                dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView2.RowHeadersVisible = false;
            }

        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
          
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["okay"].Index &&
                e.RowIndex >= 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to continue? Once completed it cannot be undone.", "complete this DR?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if (dialogResult == DialogResult.Yes)
                {
                    string itemcode = "";
                    string drnumber = "";
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        DataTable dt = new DataTable();
                        String query = "SELECT * FROM tblDR WHERE id = '" + dataGridView2.CurrentRow.Cells["id"].Value.ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, dbDR);
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            itemcode = (rdr["itemcode"].ToString());
                            drnumber = (rdr["drnumber"].ToString());
                            if (rdr["operation"].ToString() == "Completed")
                            {
                                MessageBox.Show("Record has been completed...please see the details");
                                return;
                            }
                        }
                        dbDR.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        DataTable dts = new DataTable();
                        string lists = "Select productcode,description,qty,unit,Id,iitem,cost,selling,total,icode,stored from tblDRitemCode where icode = '" + itemcode + "'";
                        SqlCommand commands = new SqlCommand(lists, itemCode);
                        SqlDataReader readers = commands.ExecuteReader();
                        dts.Load(readers);
                        itemCode.Close();

                        foreach (DataRow row in dts.Rows)
                        {

                            //MessageBox.Show(row["iitem"].ToString());
                            decimal stocks = 0.00M;
                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                String query1 = "SELECT * FROM codeMaterial WHERE ID = '" + row["iitem"].ToString() + "'";
                                SqlCommand cmd2 = new SqlCommand(query1, codeMaterial);
                                SqlDataReader rdrs = cmd2.ExecuteReader();

                                if (rdrs.Read())
                                {
                                    stocks = Convert.ToDecimal(rdrs["stocksleft"].ToString());
                                }
                                else
                                {
                                    stocks = 0.00M;
                                }
                                rdrs.Close();
                                codeMaterial.Close();
                            }
                            decimal sum2 = 0.00M;

                            sum2 += Convert.ToDecimal(stocks.ToString()) - Convert.ToDecimal(row["qty"].ToString());

                            decimal total = Math.Round(sum2, 2);
                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                SqlCommand cmda = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                                codeMaterial.Open();
                                cmda.Parameters.AddWithValue("@ID", row["iitem"].ToString());
                                cmda.Parameters.AddWithValue("@stocksleft", total);
                                cmda.ExecuteNonQuery();
                                codeMaterial.Close();
                            }


                            string p = "DR";
                            SqlCommand cmd4 = new SqlCommand("update tblDRitemCode set typeofp=@typeofp,drnumber=@drnumber,drid=@drid,stocksleft=@stocksleft where icode=@icode and iitem=@iitem", itemCode);
                            itemCode.Open();
                            cmd4.Parameters.AddWithValue("@icode", itemcode);
                            cmd4.Parameters.AddWithValue("@iitem", row["iitem"].ToString());
                            cmd4.Parameters.AddWithValue("@typeofp", p);
                            cmd4.Parameters.AddWithValue("@drnumber", drnumber);
                            cmd4.Parameters.AddWithValue("@drid", dataGridView2.CurrentRow.Cells["id"].Value.ToString());
                            cmd4.Parameters.AddWithValue("@stocksleft", total);
                            cmd4.ExecuteNonQuery();
                            itemCode.Close();

                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                string insStmta = "insert into tblHistory ([podrid], [type], [itemid], [date], [peritemid]) values" +
                                    " (@podrid,@type,@itemid,@date,@peritemid)";
                                SqlCommand insCmda = new SqlCommand(insStmta, codeMaterial);
                                insCmda.Parameters.Clear();
                                insCmda.Parameters.AddWithValue("@podrid", dataGridView2.CurrentRow.Cells["id"].Value.ToString());
                                insCmda.Parameters.AddWithValue("@type", "DR");
                                insCmda.Parameters.AddWithValue("@itemid", row["iitem"].ToString());
                                insCmda.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                insCmda.Parameters.AddWithValue("@peritemid", row["Id"].ToString());
                                int affectedRowsa = insCmda.ExecuteNonQuery();
                                codeMaterial.Close();
                            }
                        }
                    }
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        string operation = "Completed";
                        SqlCommand cmd3 = new SqlCommand("update tblDR set operation=@operation,purchasecompletedby=@purchasecompletedby where Id=@Id", dbDR);
                        dbDR.Open();
                        cmd3.Parameters.AddWithValue("@Id", dataGridView2.CurrentRow.Cells["id"].Value.ToString());
                        cmd3.Parameters.AddWithValue("@operation", operation);
                        cmd3.Parameters.AddWithValue("@purchasecompletedby", name);
                        cmd3.ExecuteNonQuery();
                        dbDR.Close();

                        string completepo = "Completed D.R";

                        DateTime d = DateTime.Now;

                        dbDR.Open();
                        string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                            " (@name,@date,@operation,@id)";
                        SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@date", d.ToString("yyyy-MM-dd"));
                        insCmd.Parameters.AddWithValue("@operation", completepo);
                        insCmd.Parameters.AddWithValue("@id", dataGridView2.CurrentRow.Cells["id"].Value.ToString());
                        int affectedRows = insCmd.ExecuteNonQuery();
                        dbDR.Close();
                    }


                    MessageBox.Show("D.R. Completed");

                    dataGridView2.CurrentRow.Cells["Column6"].Value = "Completed";

                    DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                    dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                    dataGridView2.CurrentRow.Cells["okay"].Style = dataGridViewCellStyle2;


                    //dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
                    //dataGridView2.CurrentRow.Cells["view"].Style = dataGridViewCellStyle2;
                }
            }

            if (e.ColumnIndex == dataGridView2.Columns["view"].Index &&
               e.RowIndex >= 0)
            {
                string itemcode = "";
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT * FROM tblDR WHERE id = '" + dataGridView2.CurrentRow.Cells["id"].Value.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(query, dbDR);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        itemcode = (rdr["itemcode"].ToString());
                        dbDR.Close();
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                out_supply i = new out_supply(this);
                i.id = dataGridView2.CurrentRow.Cells["id"].Value.ToString();
                i.itemid = itemcode;
                i.name = name;
                i.num = "drldrview";
                i.ShowDialog();
            }
        }
        public string name;

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //foreach (DataGridViewRow row in dataGridView2.Rows)
            //{
            //    if (row.Cells["Column6"].Value.ToString() != "Incomplete")
            //    {
            //        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            //        dataGridViewCellStyle2.Padding = new Padding(0, 0, 100, 0);
            //        row.Cells["okay"].Style = dataGridViewCellStyle2;
            //    }
            //}
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            loadprein();
            guna2Button2.Visible = true;
            Cursor.Current = Cursors.Default;

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
        }

        private void guna2DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (label5.Text != label3.Text)
            {
                MessageBox.Show("Amount is not balance. Please make sure the amount is exact");
            }
            else
            {

            }
        }
        Thread thread;
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.Text == "")
            {
                MessageBox.Show("Please Select Sales Voucher.");
                return;
            }
            if (guna2DateTimePicker1.Value.ToString() == "")
            {
                MessageBox.Show("Please Select A Date.");
                return;
            }
            if (guna2DateTimePicker2.Value.ToString() == "")
            {
                MessageBox.Show("Please Select A Date.");
                return;
            }
            combo = "";
            date1 = "";
            date2 = "";
            combo = guna2ComboBox1.Text;
            date1 = guna2DateTimePicker1.Text;
            date2 = guna2DateTimePicker2.Text;

            thread =
              new Thread(new ThreadStart(export));
            thread.Start();

        }
        string combo;
        string date1;
        string date2;
        private void export()
        {
            label6.BeginInvoke((Action)delegate ()
            {
                label6.Visible = true;
                guna2ProgressBar1.Visible = true;

            });
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {




                DataTable dt = new DataTable();

                DataTable dts = new DataTable();
                dts.Columns.Add("DRNO");
                dts.Columns.Add("TYPE");
                dts.Columns.Add("DATE",typeof(DateTime));
                dts.Columns.Add("PROJCODE");
                dts.Columns.Add("SUBCODE");
                dts.Columns.Add("QTY");
                dts.Columns.Add("UNIT");
                dts.Columns.Add("UPRICE");
                dts.Columns.Add("AMOUNT");
                dts.Columns.Add("PRODUCTCODE");
                dts.Columns.Add("ITEM");
                dbDR.Open();
                string list = "Select drnumber,datetime,projectname,operation,Id,itemcode,sv,dateentered,projectcode,projectname,sv,totalamount from tblDR WHERE datetime BETWEEN @date1 AND @date2 AND sv = '" + combo + "' order by datetime asc";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", DateTime.Parse(date1).ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@date2", DateTime.Parse(date2).ToString("yyyy-MM-dd"));
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                decimal result = 0.00M;
                decimal add2 = 0.00M;
                int rowss;
                Int32 count = dt.Rows.Count;
                rowss = count;
                result = 20 / (decimal)count;
                add2 += 0 + 0;
                dbDR.Close();
                DataTable drdt = new DataTable();
                foreach (DataRow row in dt.Rows)
                {
                    drdt.Rows.Clear();
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        string lists = "Select drnumber,projectcode,qty,unit,selling,total,description,productcode from tblDRitemCode where icode = '" + row["itemcode"].ToString() + "'";
                        SqlCommand commands = new SqlCommand(lists, itemCode);
                        SqlDataReader readers = commands.ExecuteReader();
                        drdt.Load(readers);
                        itemCode.Close();
                        decimal sum = 0.00M;
                        foreach (DataRow rows in drdt.Rows)
                        {
                            string sv = "";
                            string drnumber = "";
                            if (rows["drnumber"].ToString().Trim() == "")
                            {
                                drnumber = row["drnumber"].ToString();
                            }
                            else
                            {
                                drnumber = rows["drnumber"].ToString().Trim();
                            }
                            if (row["sv"].ToString() == "Construction Material")
                            {
                                sv = "CM";
                            }else if (row["sv"].ToString() == "Lumber")
                            {
                                sv = "LB";
                            }
                            else if (row["sv"].ToString() == "CHB")
                            {
                                sv = "CH";
                            }
                            else if (row["sv"].ToString() == "Sand & Gravel")
                            {
                                sv = "SG";
                            }
                            else if (row["sv"].ToString() == "Ready - Mixed Conc")
                            {
                                sv = "RM";
                            }
                            else if (row["sv"].ToString() == "Oxygen")
                            {
                                sv = "OX";
                            }
                            else if (row["sv"].ToString() == "Direct Sales")
                            {
                                sv = "DS";
                            }
                            object[] o = { drnumber, sv, DateTime.Parse(row["datetime"].ToString()).ToString("MM/dd/yyyy"), rows["projectcode"].ToString(), "", rows["qty"].ToString(), rows["unit"].ToString(), rows["selling"].ToString(), rows["total"].ToString(), rows["productcode"].ToString(), rows["description"].ToString()  };
                            dts.Rows.Add(o);
                            dts.AcceptChanges();
                        }
                    }
                    add2 = add2 + result;
                   
                    label6.BeginInvoke((Action)delegate ()
                    {
                        label6.Text = "Gathering... "+Decimal.ToInt32(add2).ToString()+"/100";
                    });
                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value =Decimal.ToInt32(add2);
                    });
                }


                WriteToExcel(dts, @"C:\Users\markj\Desktop\");



                MessageBox.Show("Done");
                thread.Abort();
            }
        }
        private void WriteToExcel(System.Data.DataTable dt, string location)
        {
            //instantiate excel objects (application, workbook, worksheets)
            excel.Application XlObj = new excel.Application();
            XlObj.Visible = false;
            excel._Workbook WbObj = (excel.Workbook)(XlObj.Workbooks.Add(""));
            excel._Worksheet WsObj = (excel.Worksheet)WbObj.ActiveSheet;
            //excel.IRange cells = WbObj.Worksheets[0].Cells;
           
            //run through datatable and assign cells to values of datatable
            try
            {
                decimal result = 0.00M;
                decimal add2 = 0.00M;
                int rowss;
                Int32 count = dt.Rows.Count;
                rowss = count;
                result = 10 / (decimal)count;
                add2 += 0 + 20;

                int row = 1; int col = 1;
                foreach (DataColumn column in dt.Columns)
                {
                    //adding columns

                    WsObj.Cells[row, col].NumberFormat = "@";
                    WsObj.Cells[row, col] = column.ColumnName;
                    col++;


                    add2 = add2 + result;

                    label6.BeginInvoke((Action)delegate ()
                    {
                        label6.Text = "Exporting Columns... " + Decimal.ToInt32(add2).ToString() + "/100";
                    });
                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = Decimal.ToInt32(add2);
                    });
                }
                //reset column and row variables
                col = 1;
                row++;



                decimal aresult = 0.00M;
                decimal aadd2 = 0.00M;
                int arowss;
                Int32 acount = dt.Rows.Count;
                arowss = acount;
                aresult = 70 / (decimal)acount;
                aadd2 += 0 + 30;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //adding data
                    //MessageBox.Show(dt.Columns[i].ColumnName.ToString());
                    
                    foreach (var cell in dt.Rows[i].ItemArray)
                    {
                        WsObj.Cells[row, col].NumberFormat = "@";
                        WsObj.Cells[row, col] = cell;

                        col++;
                    }

                    col = 1;
                    row++;

                    aadd2 = aadd2 + aresult;
                    label6.BeginInvoke((Action)delegate ()
                    {
                        label6.Text = "Exporting rows... " + Decimal.ToInt32(aadd2).ToString() + "/100";
                    });
                    guna2ProgressBar1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressBar1.Value = Decimal.ToInt32(aadd2);
                    });
                }
                label6.BeginInvoke((Action)delegate ()
                {
                    label6.Text = "Exported Successfully... ";
                });

                WbObj.SaveAs(location);
            }
            catch (COMException x)
            {
                //ErrorHandler.Handle(x);
            }
            catch (Exception ex)
            {
                //ErrorHandler.Handle(ex);
            }
            finally
            {
                WbObj.Close();
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        //public void getinfo()
        //{

        //    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
        //    {
        //        var sb = new System.Text.StringBuilder();
        //        dbDR.Open();
        //        DataTable dt = new DataTable();
        //        String query = "SELECT * FROM tblDR WHERE id = '" + id + "'";
        //        SqlCommand cmd = new SqlCommand(query, dbDR);
        //        SqlDataReader rdr = cmd.ExecuteReader();

        //        if (rdr.Read())
        //        {
        //            label1.Text = (rdr["drnumber"].ToString());
        //            guna2TextBox2.Text = (rdr["drnumber"].ToString());
        //            guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
        //            label12.Text = (rdr["projectcode"].ToString());
        //            guna2TextBox1.Text = (rdr["projectcode"].ToString());
        //            label11.Text = (rdr["projectname"].ToString());
        //            label20.Text = DateTime.Parse(rdr["datetime"].ToString()).ToString("MM/dd/yyyy");
        //            label17.Text = (rdr["qty"].ToString());
        //            label18.Text = (rdr["totalitems"].ToString());
        //            guna2TextBox4.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));
        //            amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));
        //            operationcheck = (rdr["operation"].ToString());
        //            label19.Text = (rdr["createdby"].ToString());
        //            label25.Text = (rdr["purchasecompletedby"].ToString());
        //            icode = (rdr["itemcode"].ToString());
        //            label3.Text = (rdr["sv"].ToString());

        //            if (rdr["dateentered"].ToString() != "")
        //            {
        //                label4.Text = DateTime.Parse(rdr["dateentered"].ToString()).ToString("MM/dd/yyyy");
        //            }
        //            if (rdr["ponumber"] == null || rdr["ponumber"].ToString() == "" || rdr["ponumber"] == DBNull.Value || rdr["ponumber"].ToString() == null)
        //            {
        //                label28.ForeColor = Color.LightCoral;
        //                label28.Text = "None";
        //            }
        //            else
        //            {
        //                DateTime d = DateTime.Parse(rdr["podate"].ToString());
        //                label28.ForeColor = Color.White;
        //                label28.Text = (rdr["ponumber"].ToString()) + " / Date: " + d.ToString("MM/dd/yyyy");
        //            }

        //        }
        //        dbDR.Close();
        //    }
        //}
        //    public void load2()
        //{


        //}
    }
}
