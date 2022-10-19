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
    public partial class category : Form
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
        public category()
        {
            InitializeComponent();
        }

        private void category_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            load();
            loadcom1();
            //max();
        }
        DataTable d = new DataTable();
        public void load()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                d.Rows.Clear();
                string Query = "SELECT value,category FROM tblCategory ORDER BY category ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                d.Load(myReader);
                otherDB.Close();
                guna2ComboBox1.DataSource = d;
                guna2ComboBox1.ValueMember = "value";
                guna2ComboBox1.DisplayMember = "category";
            }
        }
        private int value;
        //private void max()
        //{
        //    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
        //    {
        //        var empcon = new SqlCommand("SELECT max(id) FROM [tblCategory]", otherDB);


        //        otherDB.Open();
        //        Int32 max = (Int32)empcon.ExecuteScalar();
        //        int pl = 0;
        //        pl += max + 1;
        //        value = pl;

        //        otherDB.Close();
        //    }
        //}

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            
        }

        string lb = "{";
        string rb = "}";
        DataTable dt = new DataTable(); 
        Int32 Val = 0;
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(guna2ComboBox1.SelectedValue.ToString());
           
        }
        private void loadcom1()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                DataTable dtsub = new DataTable();
                string Query = "SELECT subcategory,value,catname,catval FROM tblSubCat WHERE catval= '" + guna2ComboBox1.SelectedValue + "' ORDER BY subcategory ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                dtsub.Load(myReader);
                otherDB.Close();
                guna2ComboBox2.DataSource = dtsub;
                guna2ComboBox2.ValueMember = "value";
                guna2ComboBox2.DisplayMember = "subcategory";
            }
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            //loadtype();
        }
        private int cat;
        private int subcat;
        private int type;
        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked == true)
            {
                label7.Visible = true;
                guna2TextBox4.Visible = true;
                label1.Visible = true;
                label2.Visible = false;
                label3.Visible = true;
                label4.Visible = false;

                guna2CheckBox2.Enabled = false;
                guna2CheckBox2.Checked = false;
                guna2CheckBox3.Enabled = false;
                guna2CheckBox3.Checked = false;

                guna2TextBox1.Visible = true;
                guna2TextBox2.Visible = true;
                guna2TextBox3.Visible = true;

                guna2ComboBox2.Visible = false;
                guna2ComboBox3.Visible = false;
                guna2ComboBox1.Visible = false;

            }
            else
            {
                label7.Visible = false;
                guna2TextBox4.Visible = false;
                cat = 0;
                label1.Visible = false;
                label2.Visible = true;
                label3.Visible = false;
                label4.Visible = true;

                guna2CheckBox2.Checked = false;
                guna2CheckBox3.Enabled = true;
                guna2CheckBox3.Checked = false;
                guna2CheckBox2.Enabled = true;

                guna2ComboBox1.Visible = true;
                guna2ComboBox2.Visible = true;
                guna2ComboBox3.Visible = true;

                guna2TextBox1.Visible = false;
                guna2TextBox2.Visible = false;
                guna2TextBox3.Visible = false;
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Enabled == true)
            {
                if (guna2CheckBox2.Checked == true)
                {
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        var empcon = new SqlCommand("SELECT max(value) FROM [tblSubCat] WHERE catval = '" + guna2ComboBox1.SelectedValue + "'", otherDB);


                        subcat = 0;
                        otherDB.Open();
                        Int32 max = (Int32)empcon.ExecuteScalar();
                        subcat = max + 1;
                        otherDB.Close();

                    }
                    label2.Visible = false;
                    label3.Visible = true;

                    guna2TextBox2.Visible = true;
                    guna2TextBox3.Visible = true;

                    guna2ComboBox3.Visible = false;
                    guna2ComboBox2.Visible = false;

                    guna2CheckBox3.Checked = false;
                    guna2CheckBox3.Enabled = false;

                }
                else
                {
                    subcat = 0;
                    label2.Visible = true;
                    label3.Visible = false;

                    guna2ComboBox2.Visible = true;
                    guna2ComboBox3.Visible = true;

                    guna2TextBox2.Visible = false;
                    guna2TextBox3.Visible = false;

                    guna2CheckBox3.Checked = false;
                    guna2CheckBox3.Enabled = true;
                }

            }

        }
        private string stringcat;
        private string valcat;
        private string stringsubcat;
        private int intsubcat;
        private string stringtype;
        private int inttypet;


        private void valuechecker()
        {
            valcat = "";
            stringcat = "";
            stringsubcat = "";
            intsubcat = 0;
            stringtype = "";
            inttypet = 0;
            if (guna2CheckBox1.Checked == true)
            {
                if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
                {
                    MessageBox.Show("Please don't leave any blanks");
                }
                else
                {
                    valcat ="";
                    stringcat = "";
                    stringsubcat = "";
                    intsubcat = 0;
                    stringtype = "";
                    inttypet = 0;

                    valcat = guna2TextBox4.Text;
                    stringcat = guna2TextBox1.Text;
                    stringsubcat = guna2TextBox2.Text;
                    intsubcat = 1;
                    stringtype = guna2TextBox3.Text;
                    inttypet = 1;

                    saveexample();
                    save();
                }
            }
            else
            {
                if (guna2CheckBox2.Checked == true)
                {
                    valcat = "";
                    stringcat = "";
                    stringsubcat = "";
                    intsubcat = 0;
                    stringtype = "";
                    inttypet = 0;
                    valcat = guna2ComboBox1.SelectedValue.ToString();
                    stringcat = guna2ComboBox1.Text;
                    if (guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
                    {
                        MessageBox.Show("Please don't leave any blanks");
                    }
                    else
                    {
                        using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                        {
                            otherDB.Close();
                            var empcon = new SqlCommand("SELECT max(value) FROM [tblSubCat] WHERE ([catval] = @catval)", otherDB);
                            empcon.Parameters.AddWithValue("@catval", guna2ComboBox1.SelectedValue);
                            intsubcat = 0;
                            otherDB.Open();
                            Int32 max = 0;
                            if (empcon.ExecuteScalar() == DBNull.Value || empcon.ExecuteScalar() == null)
                            {
                                max = 0;
                            }
                            else
                            {
                                max = (Int32)empcon.ExecuteScalar();
                            }
                            stringsubcat = guna2TextBox2.Text;
                            intsubcat = max + 1;
                            stringtype = guna2TextBox3.Text;
                            inttypet = 1;
                            otherDB.Close();
                        }
                        saveexample();
                        save();
                    }
                }
                else
                {

                    valcat = "";
                    stringcat = "";
                    stringsubcat = "";
                    intsubcat = 0;
                    stringtype = "";
                    inttypet = 0;
                    valcat = guna2ComboBox1.SelectedValue.ToString();
                    stringcat = guna2ComboBox1.Text;
                    stringsubcat = guna2ComboBox2.Text;
                    intsubcat = (Int32)guna2ComboBox2.SelectedValue;
                    if (guna2TextBox3.Text == "")
                    {
                        MessageBox.Show("Please don't leave any blanks");
                    }
                    else
                    {
                        using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                        {
                            otherDB.Close();
                            var empcon = new SqlCommand("SELECT max(value) FROM [tblType] WHERE ([valcat] = @valcat) AND ([valsubcat] = @valsubcat)", otherDB);
                            empcon.Parameters.AddWithValue("@valcat", guna2ComboBox1.SelectedValue);
                            empcon.Parameters.AddWithValue("@valsubcat", guna2ComboBox2.SelectedValue);
                            inttypet = 0;
                            otherDB.Open();
                            Int32 max = 0;
                            if (empcon.ExecuteScalar() == DBNull.Value || empcon.ExecuteScalar() == null)
                            {
                                max = 0;
                            }
                            else
                            {
                                max = (Int32)empcon.ExecuteScalar();
                            }
                            stringtype = guna2TextBox3.Text;
                            inttypet = max + 1;
                            otherDB.Close();
                        }
                        saveexample();
                        save();
                    }
                }
            }
        }

        private void saveexample()
        {
            string en = Environment.NewLine;
            MessageBox.Show("String Category: " + stringcat + en + "Integer Category: " + valcat + en + "String SubCategory: " + stringsubcat + en + "Integer SubCategory: " + intsubcat + en + "String Type: " + stringtype + en + "Integer Type: " + inttypet);
        }
        additem obj = (additem)Application.OpenForms["additem"];
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked == false && guna2CheckBox2.Checked == false & guna2CheckBox3.Checked == false)
            {
                MessageBox.Show("There is nothing to add...");
            }
            else
            {
                if (guna2CheckBox1.Checked == true)
                {
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblCategory] WHERE ([category] = @category)", otherDB);
                        check_User_Name.Parameters.AddWithValue("@category", guna2TextBox1.Text);
                        int chh = (int)check_User_Name.ExecuteScalar();
                        if (chh == 0)
                        {
                            otherDB.Close();
                        }
                        else
                        {
                            MessageBox.Show("Category already added...");
                            otherDB.Close();
                            return;
                        }

                    }
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblCategory] WHERE ([value] = @value)", otherDB);
                        check_User_Name.Parameters.AddWithValue("@value", guna2TextBox4.Text.ToUpper());
                        int chh = (int)check_User_Name.ExecuteScalar();
                        if (chh == 0)
                        {
                            otherDB.Close();
                        }
                        else
                        {
                            MessageBox.Show("ALPHA Value already added...");
                            otherDB.Close();
                            return;
                        }

                    }
                }

                if (guna2CheckBox2.Checked == true)
                {
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblSubCat] WHERE ([subcategory] = @subcategory) AND ([catval] = @catval)", otherDB);
                        check_User_Name.Parameters.AddWithValue("@subcategory", guna2TextBox2.Text);
                        check_User_Name.Parameters.AddWithValue("@catval", guna2ComboBox1.SelectedValue);
                        int chh = (int)check_User_Name.ExecuteScalar();
                        if (chh == 0)
                        {
                            otherDB.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sub Category already added...");
                            otherDB.Close();

                            return;
                        }

                    }
                }
                if (guna2CheckBox3.Checked == true)
                {
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblType] WHERE ([type] = @type) AND ([valcat] = @valcat) AND ([valsubcat] = @valsubcat)", otherDB);
                        check_User_Name.Parameters.AddWithValue("@type", guna2TextBox3.Text);
                        check_User_Name.Parameters.AddWithValue("@valcat", guna2ComboBox1.SelectedValue);
                        check_User_Name.Parameters.AddWithValue("@valsubcat", guna2ComboBox2.SelectedValue);
                        int chh = (int)check_User_Name.ExecuteScalar();
                        if (chh == 0)
                        {
                            otherDB.Close();
                        }
                        else
                        {
                            MessageBox.Show("Type already added...");
                            otherDB.Close();
                            return;
                        }
                    }
                }
                valuechecker();
                this.Close();
            }
        }
        DataTable dttype = new DataTable();
        private void loadtype()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                dttype.Rows.Clear();
                string Query = "SELECT value,type,valcat,namecat,valsubcat,namesubcat FROM tblType WHERE valcat= '" + guna2ComboBox1.SelectedValue + "' AND valsubcat= '" + guna2ComboBox2.SelectedValue + "' ORDER BY type ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader(); 
                dttype.Load(myReader);
                otherDB.Close();

                guna2ComboBox3.DataSource = dttype;
                guna2ComboBox3.ValueMember = "value";
                guna2ComboBox3.DisplayMember = "type";
            }
        }
        private void save()
        {
            if (guna2CheckBox1.Checked == true)
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    otherDB.Open();
                    string insStmt = "insert into tblCategory ([value], [category]) values (@value,@category)";
                    SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@value", valcat);
                    insCmd.Parameters.AddWithValue("@category", stringcat);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    otherDB.Open();
                    string insStmt = "insert into tblSubCat ([value], [subcategory],[catval], [catname]) values (@value,@subcategory,@catval,@catname)";
                    SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@value", intsubcat);
                    insCmd.Parameters.AddWithValue("@subcategory", stringsubcat);
                    insCmd.Parameters.AddWithValue("@catval", valcat);
                    insCmd.Parameters.AddWithValue("@catname", stringcat);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    otherDB.Close();
                }
            }
            if (guna2CheckBox2.Checked == true)
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                { 
                    otherDB.Open();
                    string insStmt = "insert into tblSubCat ([value], [subcategory],[catval], [catname]) values (@value,@subcategory,@catval,@catname)";
                    SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@value", intsubcat);
                    insCmd.Parameters.AddWithValue("@subcategory", stringsubcat);
                    insCmd.Parameters.AddWithValue("@catval", valcat);
                    insCmd.Parameters.AddWithValue("@catname", stringcat);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    otherDB.Close();
                }
            }
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                string insStmt = "insert into tblType ([value], [type],[valsubcat], [namesubcat],[valcat], [namecat]) values (@value,@type,@valsubcat,@namesubcat,@valcat,@namecat)";
                SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@value", inttypet);
                insCmd.Parameters.AddWithValue("@type", stringtype);
                insCmd.Parameters.AddWithValue("@valsubcat", intsubcat);
                insCmd.Parameters.AddWithValue("@namesubcat", stringsubcat);
                insCmd.Parameters.AddWithValue("@valcat", valcat);
                insCmd.Parameters.AddWithValue("@namecat", stringcat);
                int affectedRows = insCmd.ExecuteNonQuery();
                otherDB.Close();
            }


            load();
            //max();
            obj.loadcombo();
            MessageBox.Show("Done");
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ComboBox1_Validating(object sender, CancelEventArgs e)
        {
         
                loadcom1();
            
        }

        private void guna2ComboBox2_Validating(object sender, CancelEventArgs e)
        {
          
                loadtype();
            
        }

        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked == true)
            {
                guna2TextBox3.Visible = true;
                guna2ComboBox3.Visible = true;
            }
            else
            {
                if (guna2CheckBox3.Checked == true)
                {
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        var empcon = new SqlCommand("SELECT MAX(value) FROM [tblType] WHERE valcat = '" + guna2ComboBox1.SelectedValue + "' AND valsubcat = '" + guna2ComboBox2.SelectedValue + "'", otherDB);

                        type = 0;
                        otherDB.Open();
                        Int32 max = 0;
                        if (empcon.ExecuteScalar() == DBNull.Value || empcon.ExecuteScalar() == null)
                        {
                            max = 0;
                        }
                        else
                        {
                            max = (Int32)empcon.ExecuteScalar();
                        }
                        type = max + 1;
                        otherDB.Close();

                    }
                    guna2TextBox3.Visible = true;
                    guna2ComboBox3.Visible = false;
                }
                else
                {
                    type = 0;
                    guna2TextBox3.Visible = false;
                    guna2ComboBox3.Visible = true;
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            categories c = new categories();
            c.Show();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text.Contains("I") || guna2TextBox4.Text.Contains("i") || guna2TextBox4.Text.Contains("o") || guna2TextBox4.Text.Contains("O"))
            {
                MessageBox.Show("Letter contains I,i,O and o is not accepted. Thank you");
                guna2TextBox4.Text = "";
            }
        }
    }
}
