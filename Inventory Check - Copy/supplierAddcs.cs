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
    public partial class supplierAddcs : Form
    {
        public supplierAddcs()
        {
            InitializeComponent();
        }
        private void supplierAddcs_Load(object sender, EventArgs e)
        {

        }
        public string name;
        supplierlist obj = (supplierlist)Application.OpenForms["supplierlist"];
        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblSupplier] WHERE ([suppliername] = @suppliername)", tblSupplier);
                check_User_Name.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                int chh = (int)check_User_Name.ExecuteScalar();
                if (chh == 0)
                {
                    tblSupplier.Close();
                    DialogResult dialogResult = MessageBox.Show("Are you sure to save this supplier?", "Save?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        tblSupplier.Open();
                        string insStmt = "insert into tblSupplier ([suppliername],[address],[contactnumber],[telephonenumber],[faxnumber],[createdby],[tin],[contactperson],[bankname],[bankno]) values (@suppliername,@address,@contactnumber,@telephonenumber,@faxnumber,@createdby,@tin,@contactperson,@bankname,@bankno)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblSupplier);
                        insCmd.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                        insCmd.Parameters.AddWithValue("@address", guna2TextBox2.Text);
                        insCmd.Parameters.AddWithValue("@contactnumber", guna2TextBox3.Text);
                        insCmd.Parameters.AddWithValue("@telephonenumber", guna2TextBox4.Text);
                        insCmd.Parameters.AddWithValue("@faxnumber", guna2TextBox5.Text);
                        insCmd.Parameters.AddWithValue("@createdby", name);
                        insCmd.Parameters.AddWithValue("@tin", guna2TextBox6.Text);
                        insCmd.Parameters.AddWithValue("@contactperson", guna2TextBox7.Text);
                        insCmd.Parameters.AddWithValue("@bankname", guna2TextBox9.Text);
                        insCmd.Parameters.AddWithValue("@bankno", guna2TextBox8.Text);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblSupplier.Close();
                        MessageBox.Show("Saved");
                        obj.load();
                    }
                }
                else
                {
                    MessageBox.Show("Supplier already added...");
                    tblSupplier.Close();
                }
            }
        }
    }
}
