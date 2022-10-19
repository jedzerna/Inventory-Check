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
            dataGridView1.Rows[0].Cells["invoice"].Value = guna2TextBox1.Text;
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
                    using (SqlDataAdapter a2 = new SqlDataAdapter("Select description,qty,Id,iitem from itemCode where icode = '" + itemid + "' order by Id asc", itemCode))
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
                    using (SqlDataAdapter a2 = new SqlDataAdapter("Select charging,si,description,qty,Id,iitem from itemCode where icode = '" + itemid + "' order by Id asc", itemCode))
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
                        dataGridView1.Rows[n].Cells["id"].Value = item["Id"];
                        dataGridView1.Rows[n].Cells["Column1"].Value = item["iitem"];
                    }
                    itemCode.Close();
                    dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                    dataGridView1.RowHeadersVisible = false;
                }
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

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
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

                            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                            {
                                SqlCommand cmd3 = new SqlCommand("update itemCode set si=@si,charging=@charging,stocksleft=@stocksleft where Id=@Id", itemCode);
                                itemCode.Open();
                                cmd3.Parameters.AddWithValue("@Id", row.Cells["Id"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@si", row.Cells["invoice"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@charging", row.Cells["charging"].Value.ToString());
                                cmd3.Parameters.AddWithValue("@stocksleft", qty);
                                cmd3.ExecuteNonQuery();
                                itemCode.Close();
                            }
                            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                            {
                                codeMaterial.Open();
                                string insStmt = "insert into tblHistory ([podrid], [type], [itemid], [date]) values" +
                                    " (@podrid,@type,@itemid,@date)";
                                SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                                insCmd.Parameters.Clear();
                                insCmd.Parameters.AddWithValue("@podrid", poid);
                                insCmd.Parameters.AddWithValue("@type", "PO");
                                insCmd.Parameters.AddWithValue("@itemid", row.Cells["Column1"].Value.ToString());
                                insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                                int affectedRows = insCmd.ExecuteNonQuery();
                                codeMaterial.Close();
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
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
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
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Cells["invoice"].Value = guna2TextBox1.Text;
            }
        }
    }
}
