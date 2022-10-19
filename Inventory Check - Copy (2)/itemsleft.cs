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
            //t = new System.Timers.Timer();
            //t.Interval = 1000;
            //t.Elapsed += OnTimeEvent;
            //t.Start();
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

            SuspendLayout();
            //loadsupply();
            loadall();
            //loadxml();

            SetDoubleBuffer(dataGridView1, DoubleBuffered = true);
            //dataGridView1.RowHeadersVisible = false;
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;

            Cursor.Current = Cursors.Default;
            //pictureBox5.InitialImage = null;
            //t.Stop(); 
            //ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            //this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            //dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //load();
            ResumeLayout();
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
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
            //using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            //{
            //    dt.Rows.Clear();
            //    codeMaterial.Open();
            //    string list = "Select ID,product_code,description,stocksleft,selling,cost,category,subcategory,type from codeMaterial order by ID DESC";
            //    SqlCommand command = new SqlCommand(list, codeMaterial);
            //    SqlDataReader reader = command.ExecuteReader();
            //    dt.Load(reader);
            //    dataGridView2.DataSource = dt;
            //    codeMaterial.Close(); 
            //    //this.dataGridView2.Sort(this.dataGridView2.Columns[2], ListSortDirection.Descending);
            //}
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
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
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
    }
}
