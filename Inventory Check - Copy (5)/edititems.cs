using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class edititems : Form
    {
        public string ponumber;
        public string id;
        SqlDataReader rdr;
  
        public edititems()
        {
            InitializeComponent();
            pictureBox4.InitialImage = null;
            pictureBox11.InitialImage = null;
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
        private void edititems_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            load(); 
            loaddb(); 
            //getinfo();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                DataTable dt = new DataTable();
                string list = "Select Id,productcode,description,qty,unit,cost,total from itemCode where ponumber = '" + ponumber.ToString() + "'";
                SqlCommand command = new SqlCommand(list, itemCode);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                itemCode.Close();
                itemCode.Dispose();
                //itemCode.Dispose();
                this.dataGridView2.Sort(this.dataGridView2.Columns[6], ListSortDirection.Ascending);
            }

        }
        private void loaddb()
        {

           
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM codeMaterial WHERE product_code = '" + row.Cells[3].Value + "'", codeMaterial);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        row.Cells[0].Value = item["stocksleft"].ToString();
                    }
                    codeMaterial.Close();
                    codeMaterial.Dispose();
                }
            }
        }
        //private void getinfo()
        //{

        //    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
        //    {
        //        tblIn.Open();
        //        DataTable dt = new DataTable();
        //        String query = "SELECT * FROM tblIn WHERE Id = '" + id + "'";
        //        SqlCommand cmd = new SqlCommand(query, tblIn);
        //        rdr = cmd.ExecuteReader();

        //        if (rdr.Read())
        //        {
        //            textBox9.Text = (rdr["ponumber"].ToString());
        //        }
        //        tblIn.Close();
        //        tblIn.Dispose();
        //    }

        //}
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                textBox9.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn2"].Value.ToString();
                textBox4.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                textBox1.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn3"].Value.ToString();
                label6.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
                textBox2.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn7"].Value.ToString();
                if (Convert.ToString(dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn8"].Value) == string.Empty)
                {
                    textBox3.Text = "0.00";
                }
                else
                {
                    textBox3.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn8"].Value.ToString();
                }
                label7.Text = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString();
                guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["cost"].Value.ToString();



            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(textBox4.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text=="")
            {

                MessageBox.Show("Please enter your desired qty!");
            }
            else
            {
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    string dbstocks;
                    codeMaterial.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT * FROM codeMaterial WHERE product_code = '" + textBox9.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, codeMaterial);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        dbstocks = (rdr["stocksleft"].ToString());

                        codeMaterial.Close();
                        decimal sum = 0.00M;
                        sum += Math.Round(Convert.ToDecimal(textBox4.Text), 2) + Math.Round(Convert.ToDecimal(dbstocks.ToString()), 2);

                        codeMaterial.Open();
                        SqlCommand cmd4 = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where product_code=@product_code", codeMaterial);

                        cmd4.Parameters.AddWithValue("@product_code", textBox9.Text);
                        cmd4.Parameters.AddWithValue("@stocksleft", sum);
                        cmd4.ExecuteNonQuery();
                        codeMaterial.Close();
                    }
                }
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {


                SqlCommand cmd5 = new SqlCommand("update itemCode set qty=@qty,unit=@unit,cost=@cost,total=@total where Id=@Id", itemCode);
                itemCode.Open();
                cmd5.Parameters.AddWithValue("@Id", label7.Text);
                cmd5.Parameters.AddWithValue("@qty", textBox4.Text);
                cmd5.Parameters.AddWithValue("@unit", textBox2.Text);
                    cmd5.Parameters.AddWithValue("@cost", guna2TextBox2.Text);
                    cmd5.Parameters.AddWithValue("@total", label12.Text);
                    cmd5.ExecuteNonQuery();
                itemCode.Close();

                }
                //using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                //{
                //    codeMaterial.Open();
                //    string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date]) values" +
                //        " (@podrid,@type,@itemid,@date)";
                //    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                //    insCmd.Parameters.Clear();
                //    insCmd.Parameters.AddWithValue("@podrid", id);
                //    insCmd.Parameters.AddWithValue("@type", "PO");
                //    insCmd.Parameters.AddWithValue("@itemid", row.Cells["Column1"].Value.ToString());
                //    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                //    int affectedRows = insCmd.ExecuteNonQuery();
                //    codeMaterial.Close();
                //}
                //SqlDataAdapter adapter;
                //DataSet ds = new DataSet();
                //string sql = null;

                //using (SqlConnection codeMaterial2 = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                //{
                //    sql = "select * from codeMaterial";
                //    try
                //    {
                //        codeMaterial2.Open();
                //        adapter = new SqlDataAdapter(sql, codeMaterial2);
                //        adapter.Fill(ds);
                //        ds.WriteXml(@"C:\Users\Edwin\AppData\Local\GLU\Product.xml");
                //        codeMaterial2.Close();
                //        codeMaterial2.Dispose();
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(ex.ToString());

                //    }
                //}

                obj.load();

                this.Close();
            }
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
          
            

          
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {
            //if (textBox9.Text != "" || label6.Text != "")
            //{
            //    decimal sum = 0.00M;
            //    sum += Math.Round(Convert.ToDecimal(textBox4.Text), 2) + Math.Round(Convert.ToDecimal(label6.Text), 2);
            //    label5.Text = Math.Round(sum, 2).ToString();
            //    //label12.Text = Math.Round(sum, 2).ToString();


                if (guna2TextBox2.Text != "" && textBox4.Text != "")
                {
                    decimal sum2 = 0.00M;
                    sum2 += Convert.ToDecimal(textBox4.Text) * Convert.ToDecimal(guna2TextBox2.Text);
                    label12.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(Math.Round(sum2, 2).ToString()));
                }

            //}
        }

        private void textBox4_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(textBox4.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox2.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text != "" && textBox4.Text != "")
            {
                decimal sum2 = 0.00M;
                sum2 += Convert.ToDecimal(textBox4.Text) * Convert.ToDecimal(guna2TextBox2.Text);
                label12.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(Math.Round(sum2, 2).ToString()));
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
