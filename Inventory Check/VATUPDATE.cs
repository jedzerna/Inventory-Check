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
    public partial class VATUPDATE : Form
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
        public VATUPDATE()
        {
            InitializeComponent();
        }
        public string form;
        private void VATUPDATE_Load(object sender, EventArgs e)
        {
            if (form == "Form1")
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    otherDB.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tblSettings WHERE description = 'vat'", otherDB);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        guna2TextBox4.Text = (rdr["value"].ToString());
                    }
                    else
                    {
                        guna2TextBox4.Text = "0";
                    }
                    otherDB.Close();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    otherDB.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tblSettings WHERE description = 'tax'", otherDB);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        guna2TextBox1.Text = (rdr["value"].ToString());
                    }
                    else
                    {
                        guna2TextBox1.Text = "0";
                    }
                    otherDB.Close();
                }
            }
            else if (form == "insupply")
            {
                guna2TextBox1.Text = this.In_supply.tax.Replace("%","");
                guna2TextBox4.Text = this.In_supply.vat;
            }
        }
        public string id;
        public string name;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (form == "Form1")
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblSettings set value=@value where description=@description", otherDB);
                    otherDB.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@description", "vat");
                    cmd.Parameters.AddWithValue("@value", guna2TextBox4.Text);
                    cmd.ExecuteNonQuery();
                    otherDB.Close();
                }
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblSettings set value=@value where description=@description", otherDB);
                    otherDB.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@description", "tax");
                    cmd.Parameters.AddWithValue("@value", guna2TextBox1.Text);
                    cmd.ExecuteNonQuery();
                    otherDB.Close();
                }

                this.mainForm.tax = guna2TextBox1.Text + "%";

                MessageBox.Show("Done");
                this.Close();
            }
            else if (form == "insupply")
            {
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblIn set VAT=@VAT,TAX=@TAX where Id=@Id", tblIn);
                    tblIn.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@VAT", guna2TextBox4.Text);
                    cmd.Parameters.AddWithValue("@TAX", guna2TextBox1.Text);
                    cmd.ExecuteNonQuery();
                    tblIn.Close();
                }

                string completepo = "Updating VAT and WHT";
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    insCmd.Parameters.AddWithValue("@operation", completepo);
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblIn.Close();
                }

                this.In_supply.tax = guna2TextBox1.Text;
                this.In_supply.vat = guna2TextBox4.Text;

                MessageBox.Show("Done");
                this.Close();
            }
        }
        private Form1 mainForm = null;
        private In_supply In_supply = null;
        public VATUPDATE(Form callingForm)
        {
                mainForm = callingForm as Form1;
                In_supply = callingForm as In_supply;
                InitializeComponent();
            
        }

        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox4.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox1.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text =="")
            {
                guna2TextBox1.Text = "0.00";
            }
        }

        private void guna2TextBox4_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text == "")
            {
                guna2TextBox4.Text = "0.00";
            }
        }
    }
}
