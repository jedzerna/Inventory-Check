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
    public partial class categoryediting : Form
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
        public categoryediting()
        {
            InitializeComponent();
        }

        private void categoryediting_Load(object sender, EventArgs e)
        {
            load();
            if (description.Checked)
            {
                load();
                dataGridView3.Columns["valcategory"].ReadOnly = true;
                dataGridView3.Columns["category"].ReadOnly = false;

                dataGridView1.Columns["subcategory"].ReadOnly = false;
                dataGridView1.Columns["valsubcat"].ReadOnly = true;

                dataGridView2.Columns["valtype"].ReadOnly = true;
                dataGridView2.Columns["type"].ReadOnly = false;
            }
            filter();

            ChangeControlStyles(dataGridView4, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView4.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView4.ColumnHeadersDefaultCellStyle.BackColor;

            ChangeControlStyles(dataGridView3, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;

            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;

            //lo = true;
            dataGridView4.ClearSelection();
            dataGridView3.ClearSelection();
            dataGridView2.ClearSelection(); 
            dataGridView1.ClearSelection();
            //lo = false;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {


            dataGridView3.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView1.Rows.Clear();
            //lo = true;
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable dt = new DataTable();
                string Query = "SELECT value,category FROM tblCategory ORDER BY value ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                dt.Load(myReader);
                otherDB.Close();
                foreach (DataRow row in dt.Rows)
                {
                    int a = dataGridView3.Rows.Add();
                    dataGridView3.Rows[a].Cells["valcategory"].Value = row["value"].ToString();
                    dataGridView3.Rows[a].Cells["category"].Value = row["category"].ToString();
                    dataGridView3.Rows[a].Cells["Column2"].Value = row["value"].ToString();
                    dataGridView3.Rows[a].Cells["Column1"].Value = row["category"].ToString();
                }
            }

            //lo = false;
        }
        private void loadwiththread()
        {
            dataGridView3.BeginInvoke((Action)delegate ()
            {

                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
                dataGridView1.Rows.Clear();
                //lo = true;
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    DataTable dt = new DataTable();
                    string Query = "SELECT value,category FROM tblCategory ORDER BY value ASC";
                    otherDB.Open();
                    SqlCommand cmd = new SqlCommand(Query, otherDB);
                    SqlDataReader myReader = cmd.ExecuteReader();
                    dt.Load(myReader);
                    otherDB.Close();
                    foreach (DataRow row in dt.Rows)
                    {
                        int a = dataGridView3.Rows.Add();
                        dataGridView3.Rows[a].Cells["valcategory"].Value = row["value"].ToString();
                        dataGridView3.Rows[a].Cells["category"].Value = row["category"].ToString();
                        dataGridView3.Rows[a].Cells["Column2"].Value = row["value"].ToString();
                        dataGridView3.Rows[a].Cells["Column1"].Value = row["category"].ToString();
                    }
                }
            });
        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (description.Checked)
            {
                load();
                dataGridView3.Columns["valcategory"].ReadOnly = true;
                dataGridView3.Columns["category"].ReadOnly = false;

                dataGridView1.Columns["subcategory"].ReadOnly = false;
                dataGridView1.Columns["valsubcat"].ReadOnly = true;

                dataGridView2.Columns["valtype"].ReadOnly = true;
                dataGridView2.Columns["type"].ReadOnly = false;

                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
            }

            filter();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            loadcom1();
            dataGridView2.Rows.Clear();
            loadtypedgv();
            filter();
        }
        private void loadcom1()
        {
            //lo = true;
            dataGridView1.Rows.Clear();
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable d11 = new DataTable();
                string Query = "";
                if (description.Checked)
                {
                    Query = "SELECT subcategory,value FROM tblSubCat WHERE catval= @valcategory ORDER BY value ASC";
                }
                else
                {
                    Query = "SELECT subcategory,value FROM tblSubCat WHERE catname= @category ORDER BY value ASC";
                }
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                if (description.Checked)
                {
                    cmd.Parameters.AddWithValue("@valcategory", dataGridView3.CurrentRow.Cells["valcategory"].Value.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@category", dataGridView3.CurrentRow.Cells["category"].Value.ToString());
                }
                SqlDataReader myReader = cmd.ExecuteReader();
                d11.Load(myReader);

                foreach (DataRow row in d11.Rows)
                {
                    int a = dataGridView1.Rows.Add();
                    dataGridView1.Rows[a].Cells["subcategory"].Value = row["subcategory"].ToString();
                    dataGridView1.Rows[a].Cells["valsubcat"].Value = row["value"].ToString().PadLeft(2, '0');
                    dataGridView1.Rows[a].Cells["Column3"].Value = row["subcategory"].ToString();
                    dataGridView1.Rows[a].Cells["Column4"].Value = row["value"].ToString().PadLeft(2, '0');

                }
                otherDB.Close();
            }
            dataGridView2.Rows.Clear();
            loadtypedgv();
            filter();

            //lo = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Rows.Clear();
            loadtypedgv();
        }
        private void loadtypedgv()
        {
            //lo = true;
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable d22 = new DataTable();
                string Query = "";
                if (description.Checked)
                {
                    Query = "SELECT value,type FROM tblType WHERE valcat=@valcat AND valsubcat=@valsubcat ORDER BY value ASC";
                }
                else
                {
                    Query = "SELECT value,type FROM tblType WHERE namecat=@namecat AND namesubcat=@namesubcat ORDER BY value ASC";
                }
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                if (description.Checked)
                {
                    cmd.Parameters.AddWithValue("@valcat", dataGridView3.CurrentRow.Cells["valcategory"].Value.ToString());
                    cmd.Parameters.AddWithValue("@valsubcat", dataGridView1.CurrentRow.Cells["valsubcat"].Value.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@namecat", dataGridView3.CurrentRow.Cells["category"].Value.ToString());
                    cmd.Parameters.AddWithValue("@namesubcat", dataGridView1.CurrentRow.Cells["subcategory"].Value.ToString());
                }
                SqlDataReader myReader = cmd.ExecuteReader();
                d22.Load(myReader);

                foreach (DataRow row in d22.Rows)
                {

                    int a = dataGridView2.Rows.Add();
                    dataGridView2.Rows[a].Cells["type"].Value = row["type"].ToString();
                    dataGridView2.Rows[a].Cells["valtype"].Value = row["value"].ToString().PadLeft(2, '0');
                    dataGridView2.Rows[a].Cells["Column5"].Value = row["type"].ToString();
                    dataGridView2.Rows[a].Cells["Column6"].Value = row["value"].ToString().PadLeft(2, '0');
                }
                otherDB.Close();
            }
            filter();

            //lo = false;
        }

        private void value_CheckedChanged(object sender, EventArgs e)
        {
            if (value.Checked)
            {
                description.Checked = true;
                value.Checked = false;
                MessageBox.Show("This feature still not available, please wait for the next update.","Not Available",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                load();
                dataGridView3.Columns["valcategory"].ReadOnly = false;
                dataGridView3.Columns["category"].ReadOnly = true;

                dataGridView1.Columns["subcategory"].ReadOnly = true;
                dataGridView1.Columns["valsubcat"].ReadOnly = false;

                dataGridView2.Columns["valtype"].ReadOnly = false;
                dataGridView2.Columns["type"].ReadOnly = true;
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
            }
            filter();
        }
        private string filtersearch;
        private void filter()
        {
            string cat = "";
            string sub = "";
            string type = "";
            if (description.Checked)
            {
                cat = dataGridView3.CurrentRow.Cells["valcategory"].Value.ToString();
            }
            else
            {
                cat = dataGridView3.CurrentRow.Cells["category"].Value.ToString();
            }

            if (dataGridView1.Rows.Count != 0)
            {
                if (description.Checked)
                {
                    sub = dataGridView1.CurrentRow.Cells["valsubcat"].Value.ToString().PadRight(2, '0');
                }
                else
                {
                    sub = dataGridView1.CurrentRow.Cells["subcategory"].Value.ToString();
                }
            }

            if (dataGridView2.Rows.Count != 0)
            {
                if (description.Checked)
                {
                    type = dataGridView2.CurrentRow.Cells["valtype"].Value.ToString().PadRight(2,'0');
                }
                else
                {
                    type = dataGridView2.CurrentRow.Cells["type"].Value.ToString();
                }
            }
            if (description.Checked)
            {
                if (sub == "")
                {
                    sub = "00";
                }
                else if (type == "")
                {
                    type = "00";
                }
                filtersearch = cat + sub + type + "-";
                //MessageBox.Show(filtersearch);
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    DataTable dt = new DataTable();
                    codeMaterial.Open();
                    string list = "Select product_code,description,category,subcategory,type from codeMaterial where product_code like '" + filtersearch + "%' order by product_code ASC";
                    SqlCommand command = new SqlCommand(list, codeMaterial);
                    SqlDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    dataGridView4.DataSource = dt;
                    codeMaterial.Close();
                }
                label2.Text = filtersearch;


                if (dataGridView3.Rows.Count == 0)
                {
                    desc1 = "";
                    desc1val = "";
                }
                else
                {
                    desc1 = dataGridView3.CurrentRow.Cells["Column1"].Value.ToString();
                    desc1val = dataGridView3.CurrentRow.Cells["Column2"].Value.ToString();
                }
                if (dataGridView1.Rows.Count == 0)
                {
                    desc2 = "";
                    desc2val = 0;
                }
                else
                {
                    desc2 = dataGridView1.CurrentRow.Cells["Column3"].Value.ToString();
                    desc2val = Convert.ToInt16(dataGridView1.CurrentRow.Cells["Column4"].Value.ToString());
                }
                if (dataGridView2.Rows.Count == 0)
                {
                    desc3 = "";
                    desc3val = 0;
                }
                else
                {
                    desc3 = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
                    desc3val = Convert.ToInt16(dataGridView2.CurrentRow.Cells["Column6"].Value.ToString());
                }
                label3.Text = desc1+", " + desc2 +", "+ desc3;
            }
            else
            {
                if (type == "")
                {
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        DataTable dt = new DataTable();
                        codeMaterial.Open();
                        string list = "Select product_code,description,category,subcategory,type from codeMaterial where category = category and subcategory=@subcategory order by product_code ASC";
                        SqlCommand command = new SqlCommand(list, codeMaterial);
                        command.Parameters.AddWithValue("@category", cat);
                        command.Parameters.AddWithValue("@subcategory", sub);
                        SqlDataReader reader = command.ExecuteReader();
                        dt.Load(reader);
                        dataGridView4.DataSource = dt;
                        codeMaterial.Close();
                    }
                }
                else
                {
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        DataTable dt = new DataTable();
                        codeMaterial.Open();
                        string list = "Select product_code,description,category,subcategory,type from codeMaterial where category = category and subcategory=@subcategory and type=@type order by product_code ASC";
                        SqlCommand command = new SqlCommand(list, codeMaterial);
                        command.Parameters.AddWithValue("@category", cat);
                        command.Parameters.AddWithValue("@subcategory", sub);
                        command.Parameters.AddWithValue("@type", type);
                        SqlDataReader reader = command.ExecuteReader();
                        dt.Load(reader);
                        dataGridView4.DataSource = dt;
                        codeMaterial.Close();
                    }
                }

            }
        }

        string desc1 = "";
        string desc2 = "";
        string desc3 = "";
        string desc1val = "";
        int desc2val = 0;
        int desc3val = 0;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            filter();
        }
        public void loadall()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                dt.Rows.Clear();
                codeMaterial.Open();
                string list = "Select ID,product_code,category,subcategory,type from codeMaterial order by ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                codeMaterial.Close();
            }
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                dtcat.Rows.Clear();
                string Query = "SELECT * FROM tblCategory";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                dtcat.Load(myReader);
                otherDB.Close();
            }
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                dtsub.Rows.Clear();
                string Query = "SELECT * FROM tblSubCat";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                dtsub.Load(myReader);
                otherDB.Close();
            }
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                dttype.Rows.Clear();
                string Query = "SELECT * FROM tblType";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                dttype.Load(myReader);
                otherDB.Close();
            }
        }
        DataTable dt = new DataTable();
        DataTable dtcat = new DataTable();
        DataTable dtsub = new DataTable();
        DataTable dttype = new DataTable();
        Thread thread;
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            loadall();
            if (description.Checked)
            {
                thread =
              new Thread(new ThreadStart(savingdescription));
                thread.Start();

                //savingdescription();
            }
            else
            {

            }
        }
        private void savingdescription()
        {
            guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressIndicator1.Visible = true;
                guna2ProgressIndicator1.Start();
                guna2Button4.Enabled = false;
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = false;
                dataGridView3.Enabled = false;
            });
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update tblCategory set category=@category where category=@category2", otherDB);
                    otherDB.Open();
                    cmd2.Parameters.AddWithValue("@category2", row.Cells["Column1"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@category", row.Cells["category"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    otherDB.Close();
                }

                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update tblSubCat set catname=@catname where catname=@catname2", otherDB);
                    otherDB.Open();
                    cmd2.Parameters.AddWithValue("@catname2", row.Cells["Column1"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@catname", row.Cells["category"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update tblType set namecat=@namecat where namecat=@namecat2", otherDB);
                    otherDB.Open();
                    cmd2.Parameters.AddWithValue("@namecat2", row.Cells["Column1"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@namecat", row.Cells["category"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update codeMaterial set category=@category where category=@category2", codeMaterial);
                    codeMaterial.Open();
                    cmd2.Parameters.AddWithValue("@category2", row.Cells["Column1"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@category", row.Cells["category"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    codeMaterial.Close();
                }
            }

            Thread.Sleep(1000);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update tblSubCat set subcategory=@subcategory where subcategory=@subcategory2", otherDB);
                    otherDB.Open();
                    cmd2.Parameters.AddWithValue("@subcategory2", row.Cells["Column3"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@subcategory", row.Cells["subcategory"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update tblType set namesubcat=@namesubcat where namesubcat=@namesubcat2", otherDB);
                    otherDB.Open();
                    cmd2.Parameters.AddWithValue("@namesubcat2", row.Cells["Column3"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@namesubcat", row.Cells["subcategory"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update codeMaterial set subcategory=@subcategory where subcategory=@subcategory2", codeMaterial);
                    codeMaterial.Open();
                    cmd2.Parameters.AddWithValue("@subcategory2", row.Cells["Column3"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@subcategory", row.Cells["subcategory"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    codeMaterial.Close();
                }
            }
            Thread.Sleep(1000);
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update tblType set type=@type where type=@type2", otherDB);
                    otherDB.Open();
                    cmd2.Parameters.AddWithValue("@type2", row.Cells["Column5"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@type", row.Cells["type"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    SqlCommand cmd2 = new SqlCommand("update codeMaterial set type=@type where type=@type2", codeMaterial);
                    codeMaterial.Open();
                    cmd2.Parameters.AddWithValue("@type2", row.Cells["Column5"].Value.ToString());
                    cmd2.Parameters.AddWithValue("@type", row.Cells["type"].Value.ToString());
                    cmd2.ExecuteNonQuery();
                    codeMaterial.Close();
                }
            }
            Thread.Sleep(1000);
            guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressIndicator1.Stop();
                guna2ProgressIndicator1.Visible = false;
                dataGridView1.Enabled = true;
                dataGridView2.Enabled = true;
                dataGridView3.Enabled = true;
                guna2Button4.Enabled = true;
            });
            loadwiththread();
            //dataGridView4.Rows.Clear();
            MessageBox.Show("Done");
            thread.Abort();

        }
        //bool lo = false;
        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void categoryediting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread != null)
            {
                thread.Abort();
            }
        }
    }
}
