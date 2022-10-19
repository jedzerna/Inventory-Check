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
    public partial class supplierlist : Form
    {
        public string name;
        public supplierlist()
        {
            InitializeComponent();
        }

        private void supplierlist_Load(object sender, EventArgs e)
        {
            load();
        }
        public void load()
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                DataTable dt = new DataTable();
                string list = "Select Id,suppliername,address from tblSupplier order by suppliername asc";
                SqlCommand command = new SqlCommand(list, tblSupplier);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                tblSupplier.Close();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private string supID;
        private string suppliernameforupdate;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            supID = "";
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            guna2TextBox5.Text = "";
            guna2TextBox6.Text = "";
            guna2TextBox7.Text = "";
            guna2TextBox9.Text = "";
            guna2TextBox8.Text = "";
            suppliernameforupdate = "";
            label6.Text = "Created by: ";

            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {

                tblSupplier.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblSupplier WHERE Id = '" + dataGridView2.CurrentRow.Cells["Id"].Value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    supID = (rdr["Id"].ToString());
                    guna2TextBox1.Text = (rdr["suppliername"].ToString());
                    suppliernameforupdate = (rdr["suppliername"].ToString());
                    guna2TextBox2.Text = (rdr["address"].ToString());
                    guna2TextBox3.Text = (rdr["contactnumber"].ToString());
                    guna2TextBox4.Text = (rdr["telephonenumber"].ToString());
                    guna2TextBox5.Text = (rdr["faxnumber"].ToString());
                    guna2TextBox6.Text = (rdr["tin"].ToString());
                    guna2TextBox7.Text = (rdr["contactperson"].ToString());
                    guna2TextBox9.Text = (rdr["bankname"].ToString());
                    guna2TextBox8.Text = (rdr["bankno"].ToString());
                    label6.Text = "Created by: "+ (rdr["createdby"].ToString());
                }
                tblSupplier.Close();
                tblSupplier.Dispose();
            }
            btnSave.Visible = false;
            btnDelete.Visible = true;
            btnCancel.Visible = true;
            btnEdit.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            supID = "";
            guna2TextBox1.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            guna2TextBox5.Text = "";
            guna2TextBox6.Text = "";
            guna2TextBox7.Text = "";
            guna2TextBox9.Text = "";
            guna2TextBox8.Text = "";
            suppliernameforupdate = "";
            label6.Text = "Created by: ";
            btnSave.Visible = false;
            btnDelete.Visible = false;
            btnCancel.Visible = false;
            btnEdit.Visible = false;
            guna2TextBox1.FillColor = SystemColors.WindowFrame;
            guna2TextBox2.FillColor = SystemColors.WindowFrame;
            guna2TextBox3.FillColor = SystemColors.WindowFrame;
            guna2TextBox4.FillColor = SystemColors.WindowFrame;
            guna2TextBox1.ReadOnly = true;
            guna2TextBox2.ReadOnly = true;
            guna2TextBox3.ReadOnly = true;
            guna2TextBox4.ReadOnly = true;
            guna2TextBox5.ReadOnly = true;
            guna2TextBox5.FillColor = SystemColors.WindowFrame;
            guna2TextBox6.ReadOnly = true;
            guna2TextBox6.FillColor = SystemColors.WindowFrame;
            guna2TextBox7.ReadOnly = true;
            guna2TextBox7.FillColor = SystemColors.WindowFrame;
            guna2TextBox9.ReadOnly = true;
            guna2TextBox9.FillColor = SystemColors.WindowFrame;
            guna2TextBox8.ReadOnly = true;
            guna2TextBox8.FillColor = SystemColors.WindowFrame;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnSave.Visible = true;
            btnDelete.Visible = false;
            btnCancel.Visible = true;
            btnEdit.Visible = false;
            guna2TextBox1.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox2.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox3.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox4.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox5.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox1.ReadOnly = false;
            guna2TextBox2.ReadOnly = false;
            guna2TextBox3.ReadOnly = false;
            guna2TextBox4.ReadOnly = false;
            guna2TextBox5.ReadOnly = false;
            guna2TextBox6.ReadOnly = false;
            guna2TextBox6.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox7.ReadOnly = false;
            guna2TextBox7.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox9.ReadOnly = false;
            guna2TextBox9.FillColor = Color.FromArgb(64, 64, 64);
            guna2TextBox8.ReadOnly = false;
            guna2TextBox8.FillColor = Color.FromArgb(64, 64, 64);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to update this?", "Update?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update tblSupplier set suppliername=@suppliername,address=@address,contactnumber=@contactnumber,telephonenumber=@telephonenumber,faxnumber=@faxnumber,tin=@tin,contactperson=@contactperson,bankname=@bankname,bankno=@bankno where Id=@Id", tblSupplier);


                    tblSupplier.Open();
                    cmd.Parameters.AddWithValue("@Id", supID);
                    cmd.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@address", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@contactnumber", guna2TextBox3.Text);
                    cmd.Parameters.AddWithValue("@telephonenumber", guna2TextBox4.Text);
                    cmd.Parameters.AddWithValue("@faxnumber", guna2TextBox5.Text);
                    cmd.Parameters.AddWithValue("@tin", guna2TextBox6.Text);
                    cmd.Parameters.AddWithValue("@contactperson", guna2TextBox7.Text);
                    cmd.Parameters.AddWithValue("@bankname", guna2TextBox9.Text);
                    cmd.Parameters.AddWithValue("@bankno", guna2TextBox8.Text);
                    cmd.ExecuteNonQuery();
                    tblSupplier.Close();
                    MessageBox.Show("Supplier has been updated.");
                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    SqlCommand cmd3 = new SqlCommand("update tblIn set suppliername=@suppliername where suppliername=@suppliernameid", tblIn);
                    tblIn.Open();
                    cmd3.Parameters.AddWithValue("@suppliernameid", suppliernameforupdate);
                    cmd3.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                    cmd3.ExecuteNonQuery();
                    tblIn.Close();
                }


                load();
                btnCancel_Click(sender, e);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete this?", "Delete?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {

                    tblSupplier.Open();

                    using (SqlCommand command = new SqlCommand("DELETE FROM tblSupplier WHERE Id = '" + supID + "'", tblSupplier))
                    {
                        command.ExecuteNonQuery();
                    }

                    tblSupplier.Close();
                }
                load();
                btnCancel_Click(sender, e);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            supplierAddcs a = new supplierAddcs();
            a.name = name;
            a.ShowDialog();
        }
    }
}
