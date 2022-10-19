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
    public partial class in_supplyReplaceForm : Form
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
        public in_supplyReplaceForm()
        {
            InitializeComponent();
        }
        static void SetDoubleBuffer(Control ctl, bool DoubleBuffered)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, ctl, new object[] { DoubleBuffered });
        }
        private void in_supplyReplaceForm_Load(object sender, EventArgs e)
        {
            loadall();

            SetDoubleBuffer(dataGridView2, DoubleBuffered = true);
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
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
          

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                //dataGridView1.Rows.Clear();
                itemCode.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM itemCode WHERE Id = '" + iteminItemcodeid + "'";
                SqlCommand cmd = new SqlCommand(query, itemCode);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label1.Text = (rdr["productcode"].ToString());
                    guna2TextBox4.Text = (rdr["description"].ToString());
                    olddbitemid = (rdr["iitem"].ToString());

                }
                else
                {
                    MessageBox.Show("Found no such record.");
                    this.Close();
                }
                itemCode.Close();
            }
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                dt.Rows.Clear();
                string list = "Select ID,product_code,description,stocksleft,selling,cost,unit from codeMaterial order by ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                codeMaterial.Close();
            }
        }
        private string olddbitemid;
        private string newdbitemid;
        public string iteminItemcodeid;

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Column1"].Index && e.RowIndex >= 0)
            {
                label4.Text = dataGridView2.CurrentRow.Cells["Column2"].Value.ToString();
                guna2TextBox2.Text = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                newdbitemid = dataGridView2.CurrentRow.Cells["Column5"].Value.ToString();
            }
        }
        //string iditem;
        public string operation;
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string newpcode;
            string newdescription;
            string newstocks;
            string newunit;
            string newcost;

            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                //dataGridView1.Rows.Clear();
                codeMaterial.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM codeMaterial WHERE ID = '" + newdbitemid + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    newpcode = (rdr["product_code"].ToString());
                    newstocks = (rdr["stocksleft"].ToString());
                    newdescription = (rdr["description"].ToString());
                    newunit = (rdr["unit"].ToString());
                    newcost = (rdr["cost"].ToString());
                }
                else
                {
                    MessageBox.Show("Somethings wrong.");
                    return;
                }
                codeMaterial.Close();
            }

            string oldpcode;
            string olddescription;
            string oldstocks;
            string oldunit;
            string oldcost;

            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                //dataGridView1.Rows.Clear();
                codeMaterial.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM codeMaterial WHERE ID = '" + olddbitemid + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    oldpcode = (rdr["product_code"].ToString());
                    oldstocks = (rdr["stocksleft"].ToString());
                    olddescription = (rdr["description"].ToString());
                    oldunit = (rdr["unit"].ToString());
                    oldcost = (rdr["cost"].ToString());
                }
                else
                {
                    MessageBox.Show("Somethings wrong.");
                    return;
                }
                codeMaterial.Close();
            }

            string POpcode;
            string POdescription;
            string POqty;
            string POstocks;
            string POunit;
            string POcost;

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                //dataGridView1.Rows.Clear();
                itemCode.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM itemCode WHERE Id = '" + iteminItemcodeid + "'";
                SqlCommand cmd = new SqlCommand(query, itemCode);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    POpcode = (rdr["productcode"].ToString());
                    POdescription = (rdr["description"].ToString());
                    POqty = (rdr["qty"].ToString());
                    POstocks = (rdr["stocksleft"].ToString());
                    POunit = (rdr["unit"].ToString());
                    POcost = (rdr["cost"].ToString());
                }
                else
                {
                    MessageBox.Show("Somethings wrong.");
                    return;
                }
                itemCode.Close();
            }
            //using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            //{
            //    SqlCommand cmd = new SqlCommand("update itemCode set stocksleft=@stocksleft where Id = '" + iteminItemcodeid + "'", itemCode);
            //    itemCode.Open();
            //    cmd.Parameters.AddWithValue("@stocksleft", label4.Text);
            //    cmd.Parameters.AddWithValue("@stocksleft", label4.Text);
            //    cmd.Parameters.AddWithValue("@stocksleft", label4.Text);
            //    cmd.Parameters.AddWithValue("@stocksleft", label4.Text);
            //    cmd.ExecuteNonQuery();
            //    itemCode.Close();
            //}


            //using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            //{
            //    SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
            //    codeMaterial.Open();
            //    cmd.Parameters.AddWithValue("@ID", id);
            //    cmd.Parameters.AddWithValue("@stocksleft", label4.Text);
            //    cmd.ExecuteNonQuery();
            //    codeMaterial.Close();


            //    string operation = "Deducted stock with the QTY of " + guna2TextBox3.Text;
            //    codeMaterial.Open();
            //    string insStmt = "insert into tblHistory ([itemid], [date], [operation], [product_code], [description], [name],[remarks], [dqty],[stock]) values" +
            //        " (@itemid,@date,@operation,@product_code,@description,@name,@remarks,@dqty,@stock)";
            //    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
            //    insCmd.Parameters.AddWithValue("@itemid", id);
            //    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
            //    insCmd.Parameters.AddWithValue("@operation", operation);
            //    insCmd.Parameters.AddWithValue("@product_code", label2.Text);
            //    insCmd.Parameters.AddWithValue("@description", guna2TextBox2.Text);
            //    insCmd.Parameters.AddWithValue("@name", name);
            //    insCmd.Parameters.AddWithValue("@remarks", guna2TextBox1.Text);
            //    insCmd.Parameters.AddWithValue("@dqty", guna2TextBox3.Text);
            //    insCmd.Parameters.AddWithValue("@stock", label4.Text);
            //    int affectedRows = insCmd.ExecuteNonQuery();
            //    codeMaterial.Close();
            //}




            obj.getinfo();
            obj.load();
            obj.count();
            obj.notbalancae();
        }
        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
    }
}
