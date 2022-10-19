using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class salesinvoice : Form
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
        public string poid;
        public string num;
        public string name;
        public salesinvoice()
        {
            InitializeComponent();
        }
        private string si;
        //DateTime f = DateTime.Now.ToString(""); 
        public string f;
        private void salesinvoice_Load(object sender, EventArgs e)
        {
            cal = true;
            guna2DateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            if (num != "1")
            {
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT * FROM tblSI WHERE poid = '" + poid + "'";
                    SqlCommand cmd = new SqlCommand(query, tblIn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        guna2TextBox1.Text = (rdr["SIno"].ToString());
                        si = (rdr["SIno"].ToString());
                        guna2TextBox2.Text = (rdr["receivedby"].ToString());
                        guna2TextBox3.Text = (rdr["via"].ToString());
                        guna2DateTimePicker1.Text = (rdr["date"].ToString());
                        guna2TextBox4.Text = (rdr["carno"].ToString());
                        guna2TextBox5.Text = (rdr["forwarder"].ToString());
                    }
                    tblIn.Close();

                }
                label5.Text = "Update Sales Invoice";
            }
            load();
            if (num == "1")
            {
                dataGridView1.Rows[0].Cells["invoice"].Value = guna2TextBox1.Text;
            }
            cal = false;
        }
        public string itemid;
        public void load()
        {
            if (num == "1")
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    dataGridView1.Rows.Clear();
                    itemCode.Open();

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter a2 = new SqlDataAdapter("Select productcode,mfgcode,description,qty,unit,Id,iitem,stocksleft,cost,selling,discount,total,icode from itemCode where icode = '" + itemid + "' order by Id asc", itemCode))
                        a2.Fill(dt);
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dt;
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells["invoice"].Value = "";
                        dataGridView1.Rows[n].Cells["charging"].Value = "";
                        dataGridView1.Rows[n].Cells["description"].Value = item["description"];
                        dataGridView1.Rows[n].Cells["qty"].Value = item["qty"];
                        dataGridView1.Rows[n].Cells["id"].Value = item["Id"];
                        dataGridView1.Rows[n].Cells["Column1"].Value = item["iitem"];
                        dataGridView1.Rows[n].Cells["Column2"].Value = item["productcode"];
                        dataGridView1.Rows[n].Cells["Column4"].Value = item["unit"];
                        dataGridView1.Rows[n].Cells["Column5"].Value = item["iitem"];
                        dataGridView1.Rows[n].Cells["Column3"].Value = item["cost"];
                        dataGridView1.Rows[n].Cells["Column6"].Value = item["total"];
                        dataGridView1.Rows[n].Cells["Column5"].Value = item["icode"];
                        //dataGridView1.Rows[n].Cells["Column5"].Value = item["icode"];
                    }
                    itemCode.Close();
                    dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                    dataGridView1.RowHeadersVisible = false;
                }
            }

            else
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    dataGridView1.Rows.Clear();
                    itemCode.Open();

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter a2 = new SqlDataAdapter("Select idperitem,productcode,mfgcode,description,qty,unit,Id,iitem,stocksleft,cost,selling,discount,total,icode,charging,si from tblSIitems where poid = '" + poid + "' order by sortingid asc", itemCode))
                        a2.Fill(dt);
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dt;
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells["invoice"].Value = item["si"];
                        dataGridView1.Rows[n].Cells["charging"].Value = item["charging"];
                        dataGridView1.Rows[n].Cells["description"].Value = item["description"];
                        dataGridView1.Rows[n].Cells["qty"].Value = item["qty"];
                        dataGridView1.Rows[n].Cells["id"].Value = item["idperitem"];
                        dataGridView1.Rows[n].Cells["Column2"].Value = item["productcode"];
                        dataGridView1.Rows[n].Cells["Column4"].Value = item["unit"];
                        dataGridView1.Rows[n].Cells["Column1"].Value = item["iitem"];
                        dataGridView1.Rows[n].Cells["Column3"].Value = item["cost"];
                        dataGridView1.Rows[n].Cells["Column6"].Value = item["total"];
                        dataGridView1.Rows[n].Cells["Column5"].Value = item["icode"];
                    }
                    itemCode.Close();
                    dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                    dataGridView1.RowHeadersVisible = false;
                }
                countall();
            }
        }
        private void countall()
        {
            //MessageBox.Show("countall");
            decimal sum = 0.00M;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                decimal qty = 0.00M;
                decimal cost = 0.00M;
                if (row.Cells["qty"].Value == null || row.Cells["qty"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["qty"].Value.ToString()))
                {
                    qty = 0.00M;
                }
                else
                {
                    qty = Convert.ToDecimal(row.Cells["qty"].Value);
                }
                if (row.Cells["Column3"].Value == null || row.Cells["Column3"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Column3"].Value.ToString()))
                {
                    //row.Cells["Column6"].Value = "0.00";
                    cost = 0.00M; 
                }
                else
                {
                    cost = Convert.ToDecimal(row.Cells["Column3"].Value);
                }
                decimal sumperitem = 0.00M;
                sumperitem = qty * cost;
                //MessageBox.Show("1");
                row.Cells["Column6"].Value = string.Format("{0:#,##0.00}", sumperitem);
                sum += sumperitem;
                //MessageBox.Show("2");
            }
            //MessageBox.Show("3");
            label9.Text = string.Format("{0:#,##0.00}", sum);
            //MessageBox.Show("4");
        }
        private void countperitem()
        {
            //MessageBox.Show("countper");
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                decimal qty = 0.00M;
                decimal cost = 0.00M;
                if (row.Cells["qty"].Value == null || row.Cells["qty"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["qty"].Value.ToString()))
                {
                    qty = 0.00M;
                }
                else
                {
                    qty = Convert.ToDecimal(row.Cells["qty"].Value);
                }
                if (row.Cells["Column3"].Value == null || row.Cells["Column3"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Column3"].Value.ToString()))
                {
                    row.Cells["Column6"].Value = "0.00";
                    cost = 0.00M;
                }
                else
                {
                    cost = Convert.ToDecimal(row.Cells["Column6"].Value);
                }
                decimal sumperitem = 0.00M;
                sumperitem = qty * cost;

                row.Cells["Column6"].Value = string.Format("{0:#,##0.00}", sumperitem);
            }
        }
        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "")
            {
                if (si != guna2TextBox1.Text)
                {
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {


                        tblIn.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblSI] WHERE ([SIno] = @SIno)", tblIn);
                        check_User_Name.Parameters.AddWithValue("@SIno", guna2TextBox1.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {
                            MessageBox.Show("SI already exist");
                            return;
                        }
                        tblIn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("SI is empty");
                return;
            }





            if (num == "1")
            {
                if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
                {
                    MessageBox.Show("Please enter all the blank text box!!!");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to add this SI??", "Completion", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string completepo = "Added SI No: " + guna2TextBox1.Text;
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                                " (@name,@date,@operation,@id)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                            insCmd.Parameters.AddWithValue("@name", name);
                            insCmd.Parameters.AddWithValue("@date", f.ToString());
                            insCmd.Parameters.AddWithValue("@operation", completepo);
                            insCmd.Parameters.AddWithValue("@id", poid);
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            SqlCommand cmd3 = new SqlCommand("update tblIn set si=@si,carno=@carno where Id=@Id", tblIn);
                            tblIn.Open();
                            cmd3.Parameters.AddWithValue("@Id", poid);
                            cmd3.Parameters.AddWithValue("@si", guna2TextBox1.Text);
                            //cmd3.Parameters.AddWithValue("@vp", guna2DateTimePicker1.Text);
                            cmd3.Parameters.AddWithValue("@carno", guna2TextBox4.Text);
                            cmd3.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }

                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            string insStmt = "insert into tblVP ([poid], [vpno]) values" +
                                " (@poid,@vpno)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                            insCmd.Parameters.Clear();
                            insCmd.Parameters.AddWithValue("@poid", poid);
                            insCmd.Parameters.AddWithValue("@vpno", "");
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblIn.Close();
                        }

                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            string insStmt = "insert into tblSI ([poid], [SIno], [date], [receivedby], [via], [carno], [forwarder]) values" +
                                " (@poid,@SIno,@date,@receivedby,@via,@carno,@forwarder)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                            insCmd.Parameters.Clear();
                            insCmd.Parameters.AddWithValue("@poid", poid);
                            insCmd.Parameters.AddWithValue("@SIno", guna2TextBox1.Text);
                            insCmd.Parameters.AddWithValue("@date", guna2DateTimePicker1.Text);
                            insCmd.Parameters.AddWithValue("@receivedby", guna2TextBox2.Text);
                            insCmd.Parameters.AddWithValue("@via", guna2TextBox3.Text);
                            insCmd.Parameters.AddWithValue("@carno", guna2TextBox4.Text);
                            insCmd.Parameters.AddWithValue("@forwarder", guna2TextBox5.Text);
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblIn.Close();
                        }
                        int sortid = 0;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells["invoice"].Value == null || row.Cells["invoice"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["invoice"].Value.ToString()))
                            {
                                row.Cells["invoice"].Value = "";
                            }
                            if (row.Cells["charging"].Value == null || row.Cells["charging"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["charging"].Value.ToString()))
                            {
                                row.Cells["charging"].Value = "";
                            }
                            sortid++;
                            decimal qty = 0.00M;
                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                DataTable dt = new DataTable();
                                String query = "SELECT ID,stocksleft FROM codeMaterial WHERE ID = '" + row.Cells["Column1"].Value.ToString() + "'";
                                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                                SqlDataReader rdr = cmd.ExecuteReader();

                                if (rdr.Read())
                                {
                                    qty = Convert.ToDecimal(rdr["stocksleft"].ToString());
                                }
                                else
                                {
                                    qty = 0.00M;
                                }
                                codeMaterial.Close();
                            }
                            qty = qty + Convert.ToDecimal(row.Cells["qty"].Value.ToString());
                            //MessageBox.Show(qty.ToString());
                            //using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            //{
                            //    SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                            //    codeMaterial.Open();
                            //    cmd.Parameters.AddWithValue("@ID", row.Cells["Column1"].Value.ToString());
                            //    cmd.Parameters.AddWithValue("@stocksleft", qty);
                            //    cmd.ExecuteNonQuery();
                            //    codeMaterial.Close();
                            //}

                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date], [peritemid]) values" +
                                    " (@podrid,@type,@itemid,@date,@peritemid)";
                                SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                insCmd.Parameters.Clear();
                                insCmd.Parameters.AddWithValue("@podrid", poid);
                                insCmd.Parameters.AddWithValue("@type", "PO");
                                insCmd.Parameters.AddWithValue("@itemid", row.Cells["Column1"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                insCmd.Parameters.AddWithValue("@peritemid", row.Cells["id"].Value.ToString());
                                int affectedRows = insCmd.ExecuteNonQuery();
                                codeMaterial.Close();
                            }
                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                SqlCommand cmd3 = new SqlCommand("update itemCode set si=@si,charging=@charging where Id=@Id", itemCode);
                                itemCode.Open();
                                cmd3.Parameters.AddWithValue("@Id", row.Cells["Id"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@si", row.Cells["invoice"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@charging", row.Cells["charging"].Value.ToString());
                                cmd3.ExecuteNonQuery();
                                itemCode.Close();
                            }
                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                itemCode.Open();
                                string insStmt = "insert into tblSIitems ([poid], [productcode],[description],[unit],[qty],[iitem],[cost],[total],[si],[charging],[sortingid]) values" +
                                    " (@poid,@productcode,@description,@unit,@qty,@iitem,@cost,@total,@si,@charging,@sortingid)";
                                SqlCommand insCmd = new SqlCommand(insStmt, itemCode);
                                insCmd.Parameters.Clear();
                                insCmd.Parameters.AddWithValue("@poid", poid);
                                insCmd.Parameters.AddWithValue("@productcode", row.Cells["Column2"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@description", row.Cells["description"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@unit", row.Cells["Column4"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@qty", row.Cells["qty"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@iitem", row.Cells["Column1"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@cost", row.Cells["Column3"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@total", row.Cells["Column6"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@si", row.Cells["invoice"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@charging", row.Cells["charging"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@sortingid", sortid);
                                insCmd.Parameters.AddWithValue("@idperitem", row.Cells["Id"].Value.ToString());
                                int affectedRows = insCmd.ExecuteNonQuery();
                                itemCode.Close();

                            }

                        }

                        obj.update();
                        this.Close();
                    }
                }
            }
            else
            {
                if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
                {
                    MessageBox.Show("Please enter all the blank text box!!!");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to update this SI?", "Completion", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string completepo = "Update SI No to: " + guna2TextBox1.Text;
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                                " (@name,@date,@operation,@id)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                            insCmd.Parameters.AddWithValue("@name", name);
                            insCmd.Parameters.AddWithValue("@date", f.ToString());
                            insCmd.Parameters.AddWithValue("@operation", completepo);
                            insCmd.Parameters.AddWithValue("@id", poid);
                            int affectedRows = insCmd.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            SqlCommand cmd3 = new SqlCommand("update tblIn set si=@si,carno=@carno where Id=@Id", tblIn);
                            tblIn.Open();
                            cmd3.Parameters.AddWithValue("@Id", poid);
                            cmd3.Parameters.AddWithValue("@si", guna2TextBox1.Text);
                            //cmd3.Parameters.AddWithValue("@vp", guna2DateTimePicker1.Text);
                            cmd3.Parameters.AddWithValue("@carno", guna2TextBox4.Text);
                            cmd3.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            SqlCommand cmd3 = new SqlCommand("update tblSI set SIno=@SIno,date=@date,receivedby=@receivedby,via=@via,carno=@carno,forwarder=@forwarder where poid=@poid", tblIn);
                            tblIn.Open();
                            cmd3.Parameters.AddWithValue("@poid", poid);
                            cmd3.Parameters.AddWithValue("@SIno", guna2TextBox1.Text);
                            cmd3.Parameters.AddWithValue("@date", guna2DateTimePicker1.Text);
                            cmd3.Parameters.AddWithValue("@receivedby", guna2TextBox2.Text);
                            cmd3.Parameters.AddWithValue("@via", guna2TextBox3.Text);
                            cmd3.Parameters.AddWithValue("@carno", guna2TextBox4.Text);
                            cmd3.Parameters.AddWithValue("@forwarder", guna2TextBox5.Text);
                            cmd3.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }
                        int sort = 0;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells["invoice"].Value == null || row.Cells["invoice"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["invoice"].Value.ToString()))
                            {
                                row.Cells["invoice"].Value = "";
                            }
                            if (row.Cells["charging"].Value == null || row.Cells["charging"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["charging"].Value.ToString()))
                            {
                                row.Cells["charging"].Value = "";
                            }

                            sort++;
                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                SqlCommand cmd3 = new SqlCommand("update itemCode set si=@si,charging=@charging where Id=@Id", itemCode);
                                itemCode.Open();
                                cmd3.Parameters.AddWithValue("@Id", row.Cells["Id"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@si", row.Cells["invoice"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@charging", row.Cells["charging"].Value.ToString());
                                cmd3.ExecuteNonQuery();
                                itemCode.Close();
                            }

                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                SqlCommand cmd3 = new SqlCommand("update tblSIitems set si=@si,charging=@charging,qty=@qty,cost=@cost,total=@total,sortingid=@sortingid where idperitem=@idperitem", itemCode);
                                itemCode.Open();
                                cmd3.Parameters.AddWithValue("@idperitem", row.Cells["Id"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@si", row.Cells["invoice"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@charging", row.Cells["charging"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@qty", row.Cells["qty"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@cost", row.Cells["Column3"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@total", row.Cells["Column6"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@sortingid", sort);
                                cmd3.ExecuteNonQuery();
                                itemCode.Close();
                            }


                            //using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            //{
                            //    itemCode.Open();
                            //    string insStmt = "insert into tblSIitems ([poid], [productcode],[description],[unit],[qty],[iitem],[cost],[total],[si],[charging],[sortingid]) values" +
                            //        " (@poid,@productcode,@description,@unit,@qty,@iitem,@cost,@total,@si,@charging,@sortingid)";
                            //    SqlCommand insCmd = new SqlCommand(insStmt, itemCode);
                            //    insCmd.Parameters.Clear();
                            //    insCmd.Parameters.AddWithValue("@poid", poid);
                            //    insCmd.Parameters.AddWithValue("@productcode", row.Cells["Column2"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@description", row.Cells["description"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@unit", row.Cells["Column4"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@qty", row.Cells["qty"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@iitem", row.Cells["Column1"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@cost", row.Cells["Column3"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@total", row.Cells["Column6"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@si", row.Cells["invoice"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@charging", row.Cells["charging"].Value.ToString());
                            //    insCmd.Parameters.AddWithValue("@sortingid", sortid);
                            //    int affectedRows = insCmd.ExecuteNonQuery();
                            //    itemCode.Close();

                            //}
                        }

                        obj.getinfo();
                        obj.load();
                        MessageBox.Show("Updated");
                        this.Close();
                    }
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            f = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (num == "1")
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Cells["invoice"].Value = guna2TextBox1.Text;
                }
            }
            
        }
        int rowIndex;
        int col;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count >= 1)
                {
                    col = dataGridView1.CurrentCell.ColumnIndex;
                    rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
                    string a = dataGridView1.Rows[rowIndex].Cells["id"].Value.ToString();
                    string b = dataGridView1.Rows[rowIndex].Cells["invoice"].Value.ToString();
                    string c = dataGridView1.Rows[rowIndex].Cells["description"].Value.ToString();
                    string d = dataGridView1.Rows[rowIndex].Cells["qty"].Value.ToString();
                    string ee = dataGridView1.Rows[rowIndex].Cells["charging"].Value.ToString();
                    string f = dataGridView1.Rows[rowIndex].Cells["Column1"].Value.ToString();
                    string aa = dataGridView1.Rows[rowIndex].Cells["Column2"].Value.ToString();
                    string bb = dataGridView1.Rows[rowIndex].Cells["Column4"].Value.ToString();
                    string dd = dataGridView1.Rows[rowIndex].Cells["Column3"].Value.ToString();
                    string eeee = dataGridView1.Rows[rowIndex].Cells["Column6"].Value.ToString();
                    string ff = dataGridView1.Rows[rowIndex].Cells["Column5"].Value.ToString();


                    string g = dataGridView1.Rows[rowIndex - 1].Cells["id"].Value.ToString();
                    string h = dataGridView1.Rows[rowIndex - 1].Cells["invoice"].Value.ToString();
                    string i = dataGridView1.Rows[rowIndex - 1].Cells["description"].Value.ToString();
                    string j = dataGridView1.Rows[rowIndex - 1].Cells["qty"].Value.ToString();
                    string k = dataGridView1.Rows[rowIndex - 1].Cells["charging"].Value.ToString();
                    string l = dataGridView1.Rows[rowIndex - 1].Cells["Column1"].Value.ToString();
                    string gg = dataGridView1.Rows[rowIndex - 1].Cells["Column2"].Value.ToString();
                    string hh = dataGridView1.Rows[rowIndex - 1].Cells["Column4"].Value.ToString();
                    string jj = dataGridView1.Rows[rowIndex - 1].Cells["Column3"].Value.ToString();
                    string kk = dataGridView1.Rows[rowIndex - 1].Cells["Column6"].Value.ToString();
                    string ll = dataGridView1.Rows[rowIndex - 1].Cells["Column5"].Value.ToString();


                    //int a = dataGridView1.Rows.Add();
                    if (rowIndex > 0)
                    {
                        //int aa = dataGridView1.Rows.Add();

                        //dataGridView1.Rows.RemoveAt(rowIndex);
                        dataGridView1.Rows[rowIndex - 1].Cells["id"].Value = a;
                        dataGridView1.Rows[rowIndex - 1].Cells["invoice"].Value = b;
                        dataGridView1.Rows[rowIndex - 1].Cells["description"].Value = c;
                        dataGridView1.Rows[rowIndex - 1].Cells["qty"].Value = d;
                        dataGridView1.Rows[rowIndex - 1].Cells["charging"].Value = ee;
                        dataGridView1.Rows[rowIndex - 1].Cells["Column1"].Value = f;
                        dataGridView1.Rows[rowIndex - 1].Cells["Column2"].Value = aa;
                        dataGridView1.Rows[rowIndex - 1].Cells["Column4"].Value = bb;
                        dataGridView1.Rows[rowIndex - 1].Cells["Column3"].Value = dd;
                        dataGridView1.Rows[rowIndex - 1].Cells["Column6"].Value = eeee;
                        dataGridView1.Rows[rowIndex - 1].Cells["Column5"].Value = ff;


                        dataGridView1.Rows[rowIndex].Cells["id"].Value = g;
                        dataGridView1.Rows[rowIndex].Cells["invoice"].Value = h;
                        dataGridView1.Rows[rowIndex].Cells["description"].Value = i;
                        dataGridView1.Rows[rowIndex].Cells["qty"].Value = j;
                        dataGridView1.Rows[rowIndex].Cells["charging"].Value = k;
                        dataGridView1.Rows[rowIndex].Cells["Column1"].Value = l;
                        dataGridView1.Rows[rowIndex].Cells["Column2"].Value = gg;
                        dataGridView1.Rows[rowIndex].Cells["Column4"].Value = hh;
                        dataGridView1.Rows[rowIndex].Cells["Column3"].Value = jj;
                        dataGridView1.Rows[rowIndex].Cells["Column6"].Value = kk;
                        dataGridView1.Rows[rowIndex].Cells["Column5"].Value = ll;


                        dataGridView1.ClearSelection();
                        this.dataGridView1.Rows[rowIndex - 1].Cells[col].Selected = true;
                    }
                }
            }
            catch
            {

            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count >= 1)
                {
                    rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
                    col = dataGridView1.CurrentCell.ColumnIndex;
                    string a = dataGridView1.Rows[rowIndex].Cells["id"].Value.ToString();
                    string b = dataGridView1.Rows[rowIndex].Cells["invoice"].Value.ToString();
                    string c = dataGridView1.Rows[rowIndex].Cells["description"].Value.ToString();
                    string d = dataGridView1.Rows[rowIndex].Cells["qty"].Value.ToString();
                    string ee = dataGridView1.Rows[rowIndex].Cells["charging"].Value.ToString();
                    string f = dataGridView1.Rows[rowIndex].Cells["Column1"].Value.ToString();
                    string aa = dataGridView1.Rows[rowIndex].Cells["Column2"].Value.ToString();
                    string bb = dataGridView1.Rows[rowIndex].Cells["Column4"].Value.ToString();
                    string dd = dataGridView1.Rows[rowIndex].Cells["Column3"].Value.ToString();
                    string eeee = dataGridView1.Rows[rowIndex].Cells["Column6"].Value.ToString();
                    string ff = dataGridView1.Rows[rowIndex].Cells["Column5"].Value.ToString();


                    string g = dataGridView1.Rows[rowIndex + 1].Cells["id"].Value.ToString();
                    string h = dataGridView1.Rows[rowIndex + 1].Cells["invoice"].Value.ToString();
                    string i = dataGridView1.Rows[rowIndex + 1].Cells["description"].Value.ToString();
                    string j = dataGridView1.Rows[rowIndex + 1].Cells["qty"].Value.ToString();
                    string k = dataGridView1.Rows[rowIndex + 1].Cells["charging"].Value.ToString();
                    string l = dataGridView1.Rows[rowIndex + 1].Cells["Column1"].Value.ToString();
                    string gg = dataGridView1.Rows[rowIndex + 1].Cells["Column2"].Value.ToString();
                    string hh = dataGridView1.Rows[rowIndex + 1].Cells["Column4"].Value.ToString();
                    string jj = dataGridView1.Rows[rowIndex + 1].Cells["Column3"].Value.ToString();
                    string kk = dataGridView1.Rows[rowIndex + 1].Cells["Column6"].Value.ToString();
                    string ll = dataGridView1.Rows[rowIndex + 1].Cells["Column5"].Value.ToString();


                    //int a = dataGridView1.Rows.Add();
                    if (rowIndex < dataGridView1.Rows.Count - 1)
                    {
                        //int aa = dataGridView1.Rows.Add();

                        //dataGridView1.Rows.RemoveAt(rowIndex);
                        dataGridView1.Rows[rowIndex + 1].Cells["id"].Value = a;
                        dataGridView1.Rows[rowIndex + 1].Cells["invoice"].Value = b;
                        dataGridView1.Rows[rowIndex + 1].Cells["description"].Value = c;
                        dataGridView1.Rows[rowIndex + 1].Cells["qty"].Value = d;
                        dataGridView1.Rows[rowIndex + 1].Cells["charging"].Value = ee;
                        dataGridView1.Rows[rowIndex + 1].Cells["Column1"].Value = f;
                        dataGridView1.Rows[rowIndex + 1].Cells["Column2"].Value = aa;
                        dataGridView1.Rows[rowIndex + 1].Cells["Column4"].Value = bb;
                        dataGridView1.Rows[rowIndex + 1].Cells["Column3"].Value = dd;
                        dataGridView1.Rows[rowIndex + 1].Cells["Column6"].Value = eeee;
                        dataGridView1.Rows[rowIndex + 1].Cells["Column5"].Value = ff;


                        dataGridView1.Rows[rowIndex].Cells["id"].Value = g;
                        dataGridView1.Rows[rowIndex].Cells["invoice"].Value = h;
                        dataGridView1.Rows[rowIndex].Cells["description"].Value = i;
                        dataGridView1.Rows[rowIndex].Cells["qty"].Value = j;
                        dataGridView1.Rows[rowIndex].Cells["charging"].Value = k;
                        dataGridView1.Rows[rowIndex].Cells["Column1"].Value = l;
                        dataGridView1.Rows[rowIndex].Cells["Column2"].Value = gg;
                        dataGridView1.Rows[rowIndex].Cells["Column4"].Value = hh;
                        dataGridView1.Rows[rowIndex].Cells["Column3"].Value = jj;
                        dataGridView1.Rows[rowIndex].Cells["Column6"].Value = kk;
                        dataGridView1.Rows[rowIndex].Cells["Column5"].Value = ll;

                        dataGridView1.ClearSelection();
                        this.dataGridView1.Rows[rowIndex + 1].Cells[col].Selected = true;
                    }
                }
            }
            catch
            {

            }
        }
        bool cal = false;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cal == false)
            {
                if (dataGridView1.Rows.Count != 0)
                {

                    decimal qty = 0.00M;
                    decimal cost = 0.00M;
                    //MessageBox.Show("value 1");
                    if (dataGridView1.CurrentRow.Cells["qty"].Value == null || dataGridView1.CurrentRow.Cells["qty"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dataGridView1.CurrentRow.Cells["qty"].Value.ToString()))
                    {
                        dataGridView1.CurrentRow.Cells["qty"].Value = "0.00";
                        qty = 0.00M;
                    }
                    else
                    {
                        qty = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["qty"].Value);
                    }
                    if (dataGridView1.CurrentRow.Cells["Column3"].Value == null || dataGridView1.CurrentRow.Cells["Column3"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dataGridView1.CurrentRow.Cells["Column3"].Value.ToString()))
                    {
                        dataGridView1.CurrentRow.Cells["Column3"].Value = "0.00";
                        cost = 0.00M;
                    }
                    else
                    {
                        cost = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Column3"].Value);
                    }
                    decimal sumperitem = 0.00M;
                    sumperitem = qty * cost;
                    dataGridView1.CurrentRow.Cells["Column6"].Value = string.Format("{0:#,##0.00}", sumperitem);

                    countall();
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //try
            //{
            //    if (e.ColumnIndex == 5 && e.RowIndex != this.dataGridView1.NewRowIndex)
            //    {
            //        if (dataGridView1.Rows[e.RowIndex].Cells["qty"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["qty"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells["qty"].Value.ToString()))
            //        {
            //            dataGridView1.Rows[e.RowIndex].Cells["qty"].Value = "0.00";
            //        }
            //        else
            //        {
            //            double var1 = 0.00;
            //            var1 = double.Parse(e.Value.ToString());
            //            e.Value = var1.ToString("N2");
            //        }
            //    }
            //    else if (e.ColumnIndex == 6 && e.RowIndex != this.dataGridView1.NewRowIndex)
            //    {
            //        if (dataGridView1.Rows[e.RowIndex].Cells["Column3"].Value == null || dataGridView1.Rows[e.RowIndex].Cells["Column3"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells["Column3"].Value.ToString()))
            //        {
            //            dataGridView1.Rows[e.RowIndex].Cells["Column3"].Value = "0.00";
            //        }
            //        else
            //        {
            //            double var1 = 0.00;
            //            var1 = double.Parse(e.Value.ToString());
            //            e.Value = var1.ToString("N2");
            //        }
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Formating");
            //}
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar)
     && !char.IsDigit(e.KeyChar)
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

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //MessageBox.Show("keypress");
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 6)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView1.Rows[1].Cells[0].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[1].Cells[1].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[1].Cells[3].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[1].Cells[4].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[1].Cells[5].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[1].Cells[6].Value.ToString());
            //using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            //{
            //    itemCode.Open();

            //    DataTable dt = new DataTable();
            //    using (SqlDataAdapter a2 = new SqlDataAdapter("Select * from itemCode order by Id asc", itemCode))
            //        a2.Fill(dt);
            //    BindingSource bs = new BindingSource();
            //    bs.DataSource = dt;
            //    int sortid = 0;
            //    itemCode.Close();
            //    foreach (DataRow item in dt.Rows)
            //    {

            //        if (item["poid"] != DBNull.Value)
            //        {
            //            sortid++;
            //            itemCode.Open();
            //            string insStmt = "insert into tblSIitems ([poid], [productcode],[description],[unit],[qty],[iitem],[cost],[total],[si],[charging],[sortingid],[idperitem]) values" +
            //                " (@poid,@productcode,@description,@unit,@qty,@iitem,@cost,@total,@si,@charging,@sortingid,@idperitem)";
            //            SqlCommand insCmd = new SqlCommand(insStmt, itemCode);
            //            insCmd.Parameters.Clear();
            //            insCmd.Parameters.AddWithValue("@poid", item["poid"].ToString());
            //            insCmd.Parameters.AddWithValue("@productcode", item["productcode"].ToString());
            //            insCmd.Parameters.AddWithValue("@description", item["description"].ToString());
            //            insCmd.Parameters.AddWithValue("@unit", item["unit"].ToString());
            //            insCmd.Parameters.AddWithValue("@qty", item["qty"].ToString());
            //            insCmd.Parameters.AddWithValue("@iitem", item["iitem"].ToString());
            //            insCmd.Parameters.AddWithValue("@cost", item["cost"].ToString());
            //            insCmd.Parameters.AddWithValue("@total", item["total"].ToString());
            //            insCmd.Parameters.AddWithValue("@si", item["si"].ToString());
            //            insCmd.Parameters.AddWithValue("@charging", item["charging"].ToString());
            //            insCmd.Parameters.AddWithValue("@sortingid", sortid);
            //            insCmd.Parameters.AddWithValue("@idperitem", item["Id"].ToString());
            //            int affectedRows = insCmd.ExecuteNonQuery();
            //            itemCode.Close();
            //        }




            //    }
            //    MessageBox.Show("Done");
            //}
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
