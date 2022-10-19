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
    public partial class AddDiscount : Form
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
        public AddDiscount()
        {
            InitializeComponent();
        }
        public string amount;
        public string form;
        public string ponumber;
        private Form1 mainForm = null;
        private In_supply insupp = null;
        public AddDiscount(Form callingForm)
        {
            mainForm = callingForm as Form1;
            insupp = callingForm as In_supply;

            InitializeComponent();
        }
        public string amountwithouttax;
        private void AddDiscount_Load(object sender, EventArgs e)
        {
            if (form == "insupply")
            {
                label4.Text = amountwithouttax;
            }
            else
            {
                label4.Text = amountwithouttax;
            }
            //this.mainForm.LabelText = label2.Text;
            guna2TextBox4.Focus();
            guna2ComboBox3.Text = "Discounts";
        }

        private void guna2TextBox4_Leave(object sender, EventArgs e)
        {

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

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text != "")
            {
                if (guna2ComboBox3.Text == "Add Amount")
                {
                    decimal sum = 0.00M;
                    sum = Convert.ToDecimal(label4.Text) + (Convert.ToDecimal(guna2TextBox4.Text));
                    label2.Text = string.Format("{0:#,##0.00}", sum);
                }
                else
                {
                    decimal disc = 0.00M;
                    disc = (Convert.ToDecimal(guna2TextBox4.Text) / 100);
                    decimal sum = 0.00M;
                    sum = Convert.ToDecimal(label4.Text) - (Convert.ToDecimal(label4.Text) * disc);
                    label2.Text = string.Format("{0:#,##0.00}", sum);
                }
            }
        }
        public string id;
        public string name;
        Form1 Form1 = (Form1)Application.OpenForms["Form1"];
        In_supply insup = (In_supply)Application.OpenForms["In_supply"];
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox3.Text == "")
            {
                MessageBox.Show("Select Type");
                return;
            }
            if (guna2TextBox4.Text != "")
            {

                if (form == "insupply")
                {
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        string insStmt2 = "insert into tblDiscounts ([ponumber],[apercent],[amount],[type],[remarks]) values" +
                                      " (@ponumber,@apercent,@amount,@type,@remarks)";

                        SqlCommand insCmd2 = new SqlCommand(insStmt2, tblIn);

                        insCmd2.Parameters.AddWithValue("@ponumber", ponumber);
                        if (guna2ComboBox3.Text == "Add Amount")
                        {
                            insCmd2.Parameters.AddWithValue("@apercent", string.Format("{0:#,##0.00}", guna2TextBox4.Text));
                        }
                        else
                        {
                            insCmd2.Parameters.AddWithValue("@apercent", guna2TextBox4.Text + "%");
                        }
                        insCmd2.Parameters.AddWithValue("@amount", label2.Text);
                        insCmd2.Parameters.AddWithValue("@type", guna2ComboBox3.Text);
                        insCmd2.Parameters.AddWithValue("@remarks", guna2TextBox1.Text);
                        insCmd2.ExecuteNonQuery();
                        tblIn.Close();
                    }
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("update tblIn set totalamount=@totalamount where Id=@Id", tblIn);

                        tblIn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(label2.Text));
                        cmd.ExecuteNonQuery();
                        tblIn.Close();
                    }
                    string completepo = "Adding discount of "+guna2TextBox4.Text+"%";
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
                    insupp.lo = true;
                    insup.load2();
                    insupp.lo = false;
                    insup.count();
                    MessageBox.Show("Added");
                    this.Close();

                }

            }
            else
            {
                MessageBox.Show("Please add an amount");
            }
        }

        private void guna2TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button2_Click(sender, e);
            }
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //            Discounts
            //Others
            if (guna2ComboBox3.Text == "Add Amount")
            {
                label14.Text = "Amount";
            }
            else
            {
                label14.Text = "Percent %";
            }

            if (guna2TextBox4.Text != "")
            {
                if (guna2ComboBox3.Text == "Add Amount")
                {
                    decimal sum = 0.00M;
                    sum = Convert.ToDecimal(label4.Text) + (Convert.ToDecimal(guna2TextBox4.Text));
                    label2.Text = string.Format("{0:#,##0.00}", sum);
                }
                else
                {
                    decimal disc = 0.00M;
                    disc = (Convert.ToDecimal(guna2TextBox4.Text) / 100);
                    decimal sum = 0.00M;
                    sum = Convert.ToDecimal(label4.Text) - (Convert.ToDecimal(label4.Text) * disc);
                    label2.Text = string.Format("{0:#,##0.00}", sum);
                }
            }
        }
    }
}
