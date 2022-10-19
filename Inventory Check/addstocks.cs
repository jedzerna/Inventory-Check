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
    public partial class addstocks : Form
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
        public string id;
        public string name;
        public string form;
        private string stocks;
        public addstocks()
        {
            InitializeComponent();
        }

        private void addstocks_Load(object sender, EventArgs e)
        {
            getinfo();
            guna2ComboBox3.Text = "OTHERS";
            guna2TextBox3.Focus();
        }
        private void getinfo()
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
                    label2.Text = (rdr["product_code"].ToString());
                    guna2TextBox2.Text = (rdr["description"].ToString());


                    stocks = Math.Round((decimal)Convert.ToDecimal(rdr["stocksleft"]), 2).ToString("N2");

                    label4.Text = (rdr["unit"].ToString());
                }
                codeMaterial.Close();
                codeMaterial.Dispose();
            }
        
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        itemdetail obj = (itemdetail)Application.OpenForms["itemdetail"];
        itemsleft itemsleft = (itemsleft)Application.OpenForms["itemsleft"];
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "" || guna2TextBox1.Text == "" || guna2TextBox3.Text == "0.00")
            {
                MessageBox.Show("Please don't leave the QTY or Remarks blank");
            }else if(guna2ComboBox3.Text == "")
            {
                MessageBox.Show("Please select a bodega");
            }
            else
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblBodega] WHERE itemid = @itemid AND bodega = @bodega", itemCode);
                    check_User_Name.Parameters.AddWithValue("@itemid", id);
                    check_User_Name.Parameters.AddWithValue("@bodega", guna2ComboBox3.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        itemCode.Close();
                        decimal qty = 0.00M;
                        qty += Math.Round((decimal)Convert.ToDecimal(guna2TextBox3.Text), 2) + Math.Round((decimal)Convert.ToDecimal(label9.Text), 2);

                        SqlCommand cmd = new SqlCommand("update tblBodega set itemid=@itemid,bodega=@bodega,qty=@qty where id=@id", itemCode);

                        itemCode.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", bodegaid);
                        cmd.Parameters.AddWithValue("@itemid", id);
                        cmd.Parameters.AddWithValue("@bodega", guna2ComboBox3.Text);
                        cmd.Parameters.AddWithValue("@qty", qty);
                        cmd.ExecuteNonQuery();
                        itemCode.Close();
                    }
                    else
                    {
                        itemCode.Close();

                        itemCode.Open();
                        string insStmt = "insert into tblBodega ([itemid], [bodega], [qty]) values" +
                            " (@itemid,@bodega,@qty)";
                        SqlCommand insCmd = new SqlCommand(insStmt, itemCode);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@itemid", id);
                        insCmd.Parameters.AddWithValue("@bodega", guna2ComboBox3.Text);
                        insCmd.Parameters.AddWithValue("@qty", guna2TextBox3.Text);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        itemCode.Close();
                    }

                }



                DateTime d = DateTime.Now;
                string date = d.ToString("MM/dd/yyyy");

                getinfo();
                decimal sum = 0.00M;
                sum += Math.Round((decimal)Convert.ToDecimal(stocks), 2) + Math.Round((decimal)Convert.ToDecimal(guna2TextBox3.Text), 2);


                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                    codeMaterial.Open();
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@stocksleft", sum);
                    cmd.ExecuteNonQuery();
                    codeMaterial.Close();

                    string operation = "Added stock with the QTY of " + guna2TextBox3.Text;
                    codeMaterial.Open();
                    string insStmt = "insert into tblHistory ([itemid], [date], [operation], [product_code], [description], [name], [remarks], [aqty], [stock], [type]) values" +
                        " (@itemid,@date,@operation,@product_code,@description,@name,@remarks,@aqty,@stock,@type)";
                    SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                    insCmd.Parameters.AddWithValue("@itemid", id);
                    insCmd.Parameters.AddWithValue("@date", date);
                    insCmd.Parameters.AddWithValue("@operation", operation);
                    insCmd.Parameters.AddWithValue("@product_code", label2.Text);
                    insCmd.Parameters.AddWithValue("@description", guna2TextBox2.Text);
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@remarks", guna2TextBox1.Text);
                    insCmd.Parameters.AddWithValue("@aqty", guna2TextBox3.Text);
                    insCmd.Parameters.AddWithValue("@stock", sum);
                    insCmd.Parameters.AddWithValue("@type", "RT");
                    int affectedRows = insCmd.ExecuteNonQuery();
                    codeMaterial.Close();
                }
                if (form == "add")
                {
                    foreach (DataRow row in itemsleft.dt.Rows)
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
                                    //guna2TextBox1.Text = (rdr["product_code"].ToString());
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
                obj.getinfo();
                obj.load();
                MessageBox.Show("Added!");
                this.Close();
            }

        }
        private string bodegaid;
        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblBodega WHERE itemid = '" + id + "' AND bodega = '" + guna2ComboBox3.Text + "'";
                SqlCommand cmd = new SqlCommand(query, itemCode);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    bodegaid = "";
                    label9.Text = Math.Round((decimal)Convert.ToDecimal(rdr["qty"]), 2).ToString("N2");
                    bodegaid = rdr["id"].ToString();
                }
                else
                {
                    label9.Text = "0.00";
                    bodegaid = "";
                }
                itemCode.Close();
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_Enter(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "" || guna2TextBox3.Text == "0.00")
            {
                guna2TextBox3.Text = "";
            }
        }

        private void guna2TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button2_Click(sender, e);
            }
        }
    }
}
