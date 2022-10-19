using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Inventory_Check
{
    public partial class itemsleft : Form
    {
        public string name;

        public itemsleft()
        {
            Cursor.Current = Cursors.WaitCursor;
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
        static void SetDoubleBuffer(Control ctl, bool DoubleBuffered)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, ctl, new object[] { DoubleBuffered });
        }

        private void itemsleft_Load(object sender, EventArgs e)
        {
            color();
            SuspendLayout();
            loadall();
            SetDoubleBuffer(dataGridView1, DoubleBuffered = true);
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            Cursor.Current = Cursors.Default;
            timer2.Start();
            ResumeLayout();
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
            guna2TextBox1.Focus();
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label5.ForeColor = Color.White;

                date.ForeColor = Color.White;
                supp.ForeColor = Color.White;

                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);

                guna2TextBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox1.ForeColor = Color.White;


                dataGridView1.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label5.ForeColor = Color.Black;

                date.ForeColor = Color.Black;
                supp.ForeColor = Color.Black;

                guna2Panel1.FillColor = Color.FromArgb(0, 115, 115);
                guna2TextBox1.FillColor = Color.FromArgb(0, 115, 115);
                guna2TextBox1.ForeColor = Color.White;


                dataGridView1.BackgroundColor = Color.FromArgb(0, 115, 115);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 115, 115);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(0, 115, 115);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
       
       public DataTable dt = new DataTable();
        //public void loadsupply()
        //{
           

        //    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
        //    {
        //        dt.Rows.Clear();
        //        codeMaterial.Open();
        //        string list = "Select top 100 ID,product_code,description,stocksleft,selling,cost,category,subcategory,type from codeMaterial order by ID DESC";
        //        SqlCommand command = new SqlCommand(list, codeMaterial);
        //        SqlDataReader reader = command.ExecuteReader();
        //        dt.Load(reader);
        //        dataGridView1.DataSource = dt;
        //        codeMaterial.Close();
        //    }

        //}
        //DataTable dt = new DataTable();
        public void loadall()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                dt.Rows.Clear();
                codeMaterial.Open();
                string list = "Select ID,product_code,description,stocksleft,selling,cost,category,subcategory,type from codeMaterial order by ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                codeMaterial.Close();
            }
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
        }
        public void refr()
        {
            foreach (DataRow row in dt.Rows)
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {



        }
        void form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //loadsupply();
            dataGridView1.RefreshEdit();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {




        }

        private void button3_Click(object sender, EventArgs e)
        {
            //codeMaterial.Open();
            //SqlCommand maxCommand = new SqlCommand("SELECT max(product_code) from codeMaterial", codeMaterial);
            //Int32 max = (Int32)maxCommand.ExecuteScalar();
            //codeMaterial.Close();
            //textBox1.Text = max.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //string connetionString = null;
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
            //        codeMaterial.Close();
            //        ds.WriteXml(@"C:\Users\Edwin\AppData\Local\GLU\Product.xml");
            //        MessageBox.Show("Done");
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());

            //    }
            //    //InsertDgvIntoForm();
            //    //ExportDgvToXML();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
        
        }
        private void search()
        {

            if (date.Checked == true)
            {

                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("product_code LIKE '{0}%'", guna2TextBox1.Text.Replace("'", "''"));

            }
            else if (supp.Checked == true)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("description LIKE '%{0}%'", guna2TextBox1.Text.Replace("'", "''"));
            }
        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        
            search();

        }

        private void label4_Click(object sender, EventArgs e)
        {
        }
        string id;
        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            itemdetail i = new itemdetail();
            i.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            i.form = "refresh";
            i.productcode = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            i.name = name;
            i.ShowDialog();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Cursor.Current = Cursors.WaitCursor;
                itemdetail i = new itemdetail();
                i.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                i.productcode = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                i.name = name;
                i.form = "refresh";
                i.ShowDialog();
                e.Handled = true;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            allhistory a = new allhistory();
            a.name = name;
            a.ShowDialog();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            loadall();
            guna2TextBox1_TextChanged(sender,e);
        }

        private void date_CheckedChanged(object sender, EventArgs e)
        {

            guna2TextBox1.Focus();
        }

        private void supp_CheckedChanged(object sender, EventArgs e)
        {

            guna2TextBox1.Focus();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            additem a = new additem();
            a.name = name;
            a.number = "1";
            a.forms = "ITEMLEFT";
            a.form = "refreshplus";
            a.ShowDialog();
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
            SetDoubleBuffer(dataGridView1, DoubleBuffered = true);

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
                string listA = "Select ID,product_code,description,stocksleft,selling,cost,category,subcategory,type from codeMaterial WHERE ID BETWEEN '" + idindt + "' AND '" + maxId + "'";
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
                        newRow[6] = rows["category"].ToString();
                        newRow[7] = rows["subcategory"].ToString();
                        newRow[8] = rows["type"].ToString();
                        dt.Rows.InsertAt(newRow, 0);
                        dt.AcceptChanges();
                    }
                }
            }
            thread = null;
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


        private void itemsleft_FormClosing(object sender, FormClosingEventArgs e)
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
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
                    col = dataGridView1.CurrentCell.ColumnIndex;

                    dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;

                    if (rowIndex < dataGridView1.Rows.Count - 1)
                    {
                        dataGridView1.ClearSelection();
                        this.dataGridView1.Rows[rowIndex + 1].Cells[col].Selected = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (dataGridView1.SelectedRows.Count > 0)
                { 
                    rowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
                    col = dataGridView1.CurrentCell.ColumnIndex;
                    if (rowIndex >= 1)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex - 1;
                    }
                    if (rowIndex > 0)
                    {
                        dataGridView1.ClearSelection();
                        this.dataGridView1.Rows[rowIndex - 1].Cells[col].Selected = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    itemdetail i = new itemdetail();
                    i.id = dataGridView1.SelectedCells[0].Value.ToString();
                    //i.id = dataGridView1.CurrentRow.Cells[0].Selected.ToString();
                    //id = dataGridView1.CurrentRow.Cells[0].Selected.ToString();
                    id = dataGridView1.SelectedCells[0].Value.ToString();
                    i.form = "refresh";
                    i.productcode = dataGridView1.SelectedCells[1].Value.ToString();
                    //i.productcode = dataGridView1.CurrentRow.Cells[1].Selected.ToString();
                    i.name = name;
                    i.ShowDialog();
                }
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
