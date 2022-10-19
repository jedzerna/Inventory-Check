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
    public partial class addpoproj : Form
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
        public addpoproj()
        {
            InitializeComponent();
        }

        private void addpoproj_Load(object sender, EventArgs e)
        {
            getinfo();
        }
        string ponumber;
        string podate;
        public void getinfo()
        {

            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                var sb = new System.Text.StringBuilder();
                dbDR.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblDR WHERE id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, dbDR);
               SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (rdr["ponumber"] == null || rdr["ponumber"].ToString() == "" || rdr["ponumber"] == DBNull.Value || rdr["ponumber"].ToString() == null)
                    {
                        ponumber = "";
                        podate = "";
                    }
                    else
                    {
                        ponumber = (rdr["ponumber"].ToString());
                        DateTime d = DateTime.Parse((rdr["podate"].ToString()));
                        podate = d.ToString("MM/dd/yyyy");
                    }

                }
                dbDR.Close();
            }

        }
        string lb = "{";
        string rb = "}";
        out_supply obj = (out_supply)Application.OpenForms["out_supply"];
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (label7.Text == "Exist")
            {
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblDR set ponumber=@ponumber,podate=@podate where id=@id", dbDR);

                    dbDR.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ponumber", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@podate", guna2DateTimePicker1.Value);
                    cmd.ExecuteNonQuery();
                    dbDR.Close();
                }
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    string completepo = "Editing D.R's P.O Proj from " + ponumber + " / Date: " + podate;
                    DateTime dtnow = DateTime.Now;
                    string date = dtnow.ToString("yyyy-MM-dd");

                    dbDR.Open();
                    string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                    insCmd.Parameters.Clear();
                    insCmd.Parameters.AddWithValue("@name", name.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                    insCmd.Parameters.AddWithValue("@date", date);
                    insCmd.Parameters.AddWithValue("@operation", completepo.Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''"));
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    dbDR.Close();
                }
                MessageBox.Show("Save");
                obj.getinfo();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter another P.O. number");
            }
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "")
            {

            }
            else
            {
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {

                    dbDR.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE ([ponumber] = @ponumber)", dbDR);
                    check_User_Name.Parameters.AddWithValue("@ponumber", guna2TextBox2.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        if (guna2TextBox2.Text == ponumber)
                        {
                            label7.Text = "P.O. is the same";
                            label7.Visible = true;
                        }
                        else
                        {
                            label7.Text = "P.O.Number Exist";
                            label7.Visible = true;
                        }
                    }
                    else
                    {
                        label7.Text = "Exist";
                        label7.Visible = false;
                    }
                    dbDR.Close();
                }
            }
           
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
