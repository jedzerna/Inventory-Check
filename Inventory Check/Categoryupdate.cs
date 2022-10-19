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
    public partial class Categoryupdate : Form
    {
        public string cat;
        public string subcat;
        public string type;

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
        public Categoryupdate()
        {
            InitializeComponent();
        }

        private void Categoryupdate_Load(object sender, EventArgs e)
        {
            load();
            getinfo();
            guna2ComboBox1.Text = cat;
            guna2ComboBox2.Text = subcat;
            guna2ComboBox3.Text = type;
            if (productcode != "" && productcode.Length >= 7)
            {
                productcodeonly = productcode.Substring(productcode.Length - 7).ToString();
            }
            guna2TextBox4.Text = productcode;
            //MessageBox.Show(productcode);
            //MessageBox.Show(productcodeonly);
        }
        private void load()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable d = new DataTable();
                d.Rows.Clear();

                string Query = "SELECT category,value FROM tblCategory ORDER BY category ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    d.Load(myReader);
                    otherDB.Close();
                    guna2ComboBox1.DataSource = d;
                    guna2ComboBox1.ValueMember = "value";
                    guna2ComboBox1.DisplayMember = "category";
                }
                else
                {
                    d.Rows.Clear();
                    guna2ComboBox1.DataSource = null;
                }
            }
        }
        private string productcode;
        private string productcodeonly;
        public void getinfo()
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
                    productcode = (rdr["product_code"].ToString());
                }
                codeMaterial.Close();
            }
        }
        public void loadcombo()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable d = new DataTable();
                d.Rows.Clear();

                string Query = "SELECT category,value FROM tblCategory ORDER BY value ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    d.Load(myReader);
                    otherDB.Close();
                    guna2ComboBox1.DataSource = d;
                    guna2ComboBox1.ValueMember = "value";
                    guna2ComboBox1.DisplayMember = "category";
                }
                else
                {
                    d.Rows.Clear();
                    guna2ComboBox1.DataSource = null;
                }
            }

            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable d = new DataTable();
                d.Rows.Clear();

                string Query = "SELECT subcategory FROM tblCategory WHERE category = '" + guna2ComboBox1.Text.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''") + "'";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    d.Load(myReader);
                    otherDB.Close();
                    guna2ComboBox2.DataSource = d;
                    guna2ComboBox2.ValueMember = "subcategory";
                    guna2ComboBox2.DisplayMember = "subcategory";
                }
                else
                {
                    d.Rows.Clear();
                    guna2ComboBox2.DataSource = null;
                }
            }
        }
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string lb = "{";
        string rb = "}";
        Int32 Val = 0;
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Int32.TryParse(guna2ComboBox1.SelectedValue.ToString(), out Val))
            //{
                loadcom1();
            //}
        }
        private void loadcom1()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable dtsub = new DataTable();
                string Query = "SELECT subcategory,value,catname,catval FROM tblSubCat WHERE catval= '" + guna2ComboBox1.SelectedValue.ToString() + "' ORDER BY subcategory ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    dtsub.Load(myReader);
                    otherDB.Close();
                    guna2ComboBox2.DataSource = dtsub;
                    guna2ComboBox2.ValueMember = "value";
                    guna2ComboBox2.DisplayMember = "subcategory";
                }
                else
                {
                    dtsub.Rows.Clear();
                    guna2ComboBox2.DataSource = null;
                }
            }
        }
        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadtype();
        }
        private void loadtype()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable dttype = new DataTable();
                string Query = "SELECT value,type,valcat,namecat,valsubcat,namesubcat FROM tblType WHERE valcat= '" + guna2ComboBox1.SelectedValue + "' AND valsubcat= '" + guna2ComboBox2.SelectedValue + "' ORDER BY type ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                if (myReader.HasRows)
                {
                    dttype.Load(myReader);
                    otherDB.Close();

                    guna2ComboBox3.DataSource = dttype;
                    guna2ComboBox3.ValueMember = "value";
                    guna2ComboBox3.DisplayMember = "type";
                }
                else
                {
                    dttype.Rows.Clear();
                    guna2ComboBox3.DataSource = null;
                }
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            //if (guna2TextBox2.Text != "")
            //{
            //    Auto();
            //}
        }

        itemdetail obj = (itemdetail)Application.OpenForms["itemdetail"];
        private void save()
        {
            
            DialogResult dialogResult = MessageBox.Show("Are you sure to complete this changes?", "Changes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                getinfo();
                string fprcode;
                string cm1 = "00";
                string cm2 = "0";
                string cm3 = "0";
                if (guna2CheckBox1.Checked == false)
                {
                    if (guna2ComboBox1.SelectedValue == null)
                    {
                        cm1 = "0";
                    }
                    else
                    {
                        cm1 = guna2ComboBox1.SelectedValue.ToString();
                    }
                    if (guna2ComboBox2.SelectedValue == null)
                    {
                        cm2 = "00";
                    }
                    else
                    {
                        cm2 = guna2ComboBox2.SelectedValue.ToString().PadLeft(2, '0');
                    }
                    if (guna2ComboBox3.SelectedValue == null)
                    {
                        cm3 = "00";
                    }
                    else
                    {
                        cm3 = guna2ComboBox3.SelectedValue.ToString().PadLeft(2, '0');
                    }

                    fprcode = cm1 + "" + cm2 + cm3 + "-" + id.PadLeft(6, '0');
                }
                else
                {
                    fprcode = guna2TextBox4.Text;
                }
                //MessageBox.Show(productcode);
                //MessageBox.Show(guna2ComboBox1.SelectedValue.ToString().PadLeft(2, '0') + productcode.Substring(2));
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();

                    SqlCommand cmd4 = new SqlCommand("update tblDRitemCode set productcode=@productcode where iitem= '" + id + "'", itemCode);

                    cmd4.Parameters.AddWithValue("@productcode", fprcode);
                    cmd4.ExecuteNonQuery();

                    itemCode.Close();
                }
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    SqlCommand cmd4 = new SqlCommand("update itemCode set productcode=@productcode where iitem= '" + id + "'", itemCode);
                    cmd4.Parameters.AddWithValue("@productcode", fprcode);
                    cmd4.ExecuteNonQuery();
                    itemCode.Close();
                }
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    SqlCommand cmd4 = new SqlCommand("update tblSIitems set productcode=@productcode where iitem= '" + id + "'", itemCode);
                    cmd4.Parameters.AddWithValue("@productcode", fprcode);
                    cmd4.ExecuteNonQuery();
                    itemCode.Close();
                }
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    SqlCommand cmd4 = new SqlCommand("update printDR set productcode=@productcode where productcode= '" + productcode + "'", itemCode);
                    cmd4.Parameters.AddWithValue("@productcode", fprcode);
                    cmd4.ExecuteNonQuery();
                    itemCode.Close();
                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update codeMaterialHistory set product_code=@product_code where product_code= '" + productcode + "'", codeMaterial);
                    codeMaterial.Open();
                    cmd.Parameters.AddWithValue("@product_code", fprcode);
                    cmd.ExecuteNonQuery();
                    codeMaterial.Close();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {

                    SqlCommand cmd = new SqlCommand("update tblProcessHist set product_code=@product_code where product_code= '" + productcode + "'", otherDB);
                    otherDB.Open();
                    cmd.Parameters.AddWithValue("@product_code", fprcode);
                    cmd.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    SqlCommand cmd = new SqlCommand("update codeMaterial set category=@category,product_code=@product_code, subcategory=@subcategory, type=@type where ID=@ID", codeMaterial);
                    codeMaterial.Open();
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@product_code", fprcode);
                    cmd.Parameters.AddWithValue("@category", guna2ComboBox1.Text);
                    cmd.Parameters.AddWithValue("@subcategory", guna2ComboBox2.Text);
                    cmd.Parameters.AddWithValue("@type", guna2ComboBox3.Text);
                    cmd.ExecuteNonQuery();
                    codeMaterial.Close();

                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    DateTime datenow = DateTime.Now;
                    string fname = datenow.ToString("MM/dd/yyyy");
                    otherDB.Open();
                    string insStmt = "insert into tblProcessHist ([product_code], [operation], [nameby], [prodID], [date]) values" +
                        " (@product_code,@operation,@nameby,@prodID,@date)";
                    SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@product_code", fprcode);
                    insCmd.Parameters.AddWithValue("@operation", "Updating from "+productcode + " to " + fprcode);
                    insCmd.Parameters.AddWithValue("@nameby", name);
                    insCmd.Parameters.AddWithValue("@prodID", id);
                    insCmd.Parameters.AddWithValue("@date", fname);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    otherDB.Close();
                }
            
                obj.getinfo();
                refr();
                MessageBox.Show("Saved", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
        }
        public string form;
        public string name;
        public void refr()
        {
            if (form == "refresh")
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
            else if (form == "form1")
            {
                foreach (DataRow row in form1.dt.Rows)
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
                                row["ID"] = rdr["ID"].ToString();
                                row["product_code"] = rdr["product_code"].ToString();
                                row["mfg_code"] = rdr["mfg_code"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["stocksleft"] = rdr["stocksleft"].ToString();
                                row["description"] = rdr["description"].ToString();
                                row["selling"] = rdr["selling"].ToString();
                                row["cost"] = rdr["cost"].ToString();
                                row["unit"] = rdr["unit"].ToString();
                                row["dept"] = rdr["dept"].ToString();

                            }
                            codeMaterial.Close();
                        }
                        break;

                    }
                }
                foreach (DataRow row in additem.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        string list = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial";
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
                                row["description"] = rdr["description"].ToString();
                                row["ID"] = rdr["ID"].ToString();
                                row["product_code"] = rdr["product_code"].ToString();
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
            else if (form == "refreshplus")
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
                foreach (DataRow row in additem.dt.Rows)
                {
                    if (id == row["ID"].ToString())
                    {
                        string list = "SELECT description,ID,product_code,category,subcategory,type FROM codeMaterial";
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
                                row["description"] = rdr["description"].ToString();
                                row["ID"] = rdr["ID"].ToString();
                                row["product_code"] = rdr["product_code"].ToString();
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
        }
        additem additem = (additem)Application.OpenForms["additem"];
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text == "")
            {
                MessageBox.Show("Please enter a product code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (label21.Text == "Product code already exist")
            {
                return;
            }
            save();
        }
        itemsleft itemsleft = (itemsleft)Application.OpenForms["itemsleft"];
        Form1 form1 = (Form1)Application.OpenForms["Form1"];

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(guna2ComboBox1.SelectedValue.ToString().PadLeft(2,'0')+productcode.Substring(2));
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            categories c = new categories();
            c.Show();
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked == true)
            {
                guna2TextBox4.Text = "";
                guna2TextBox4.ReadOnly = false;
            }
            else
            {
                guna2TextBox4.ReadOnly = true;
                getinfo(); string fprcode;
                string cm1 = "00";
                string cm2 = "0";
                string cm3 = "0";
           
                    if (guna2ComboBox1.SelectedValue == null)
                    {
                        cm1 = "0";
                    }
                    else
                    {
                        cm1 = guna2ComboBox1.SelectedValue.ToString();
                    }
                    if (guna2ComboBox2.SelectedValue == null)
                    {
                        cm2 = "00";
                    }
                    else
                    {
                        cm2 = guna2ComboBox2.SelectedValue.ToString().PadLeft(2, '0');
                    }
                    if (guna2ComboBox3.SelectedValue == null)
                    {
                        cm3 = "00";
                    }
                    else
                    {
                        cm3 = guna2ComboBox3.SelectedValue.ToString().PadLeft(2, '0');
                    }

                    fprcode = cm1 + "" + cm2 + cm3 + id.PadLeft(6, '0');
                guna2TextBox4.Text = fprcode;
            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text != "")
            {
                if (guna2TextBox4.Text != productcode)
                {
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        codeMaterial.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [codeMaterial] WHERE ([product_code] = @product_code)", codeMaterial);
                        check_User_Name.Parameters.AddWithValue("@product_code", guna2TextBox4.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {
                            label21.Text = "Product code already exist";
                            label21.Visible = true;
                        }
                        else
                        {
                            label21.Text = "Exist";
                            label21.Visible = false;
                        }

                        codeMaterial.Close();
                    }
                }
                else
                {
                    label21.Text = "Exist";
                    label21.Visible = false;
                }
            }
        }
    }
}
