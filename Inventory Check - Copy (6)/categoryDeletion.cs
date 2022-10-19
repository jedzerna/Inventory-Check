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
    public partial class categoryDeletion : Form
    {
        public categoryDeletion()
        {
            InitializeComponent();
        }
        public string cat;
        public string subcat;
        public string type;
        public string vcat;
        public string vsubcat;
        public string vtype;

        private void categoryDeletion_Load(object sender, EventArgs e)
        {
            if (cat == "")
            {
                guna2CustomCheckBox1.Enabled = false;
                guna2CustomCheckBox2.Enabled = false;
                guna2CustomCheckBox3.Enabled = false;
            }
            else
            {
                guna2TextBox1.Text = cat;
            }

            if (subcat == "")
            {
                guna2CustomCheckBox2.Enabled = false;
                guna2CustomCheckBox3.Enabled = false;
            }
            else
            {
                guna2TextBox2.Text = subcat;
            }


            if (type == "")
            {
                guna2CustomCheckBox3.Enabled = false;
            }
            else
            {
                guna2TextBox3.Text = type;
            }

            label5.Text = "Value: "+vcat;
            label6.Text = "Value: " + vsubcat;
            label7.Text = "Value: " + vtype;
        }


        private void guna2CustomCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox1.Checked)
            {
                if (guna2CustomCheckBox2.Enabled == true)
                {
                    guna2CustomCheckBox2.Checked = true;
                }
                if (guna2CustomCheckBox3.Enabled == true)
                {
                    guna2CustomCheckBox3.Checked = true;
                }
            }
            else
            {

                if (guna2CustomCheckBox2.Checked == true)
                {
                    guna2CustomCheckBox2.Checked = false;
                }
                if (guna2CustomCheckBox3.Checked == true)
                {
                    guna2CustomCheckBox3.Checked = false;
                }
            }
        }

        private void guna2CustomCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox2.Checked)
            {
                if (guna2CustomCheckBox3.Enabled == true)
                {
                    guna2CustomCheckBox3.Checked = true;
                }
            }
            else
            {

                if (guna2CustomCheckBox1.Checked == true)
                {
                    guna2CustomCheckBox1.Checked = false;
                }
                if (guna2CustomCheckBox3.Checked == true)
                {
                    guna2CustomCheckBox3.Checked = false;
                }

            }
        }

        private void guna2CustomCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox3.Checked == false)
            {
                if (guna2CustomCheckBox1.Checked == true)
                {
                    guna2CustomCheckBox1.Checked = false;
                }
                if (guna2CustomCheckBox2.Checked == true)
                {
                    guna2CustomCheckBox2.Checked = false;
                }
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete? Once deleted cannot be undone.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (dialogResult == DialogResult.Yes)
            {
                if (guna2CustomCheckBox1.Checked == true)
                {
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblCategory WHERE value = '" + vcat + "'", otherDB))
                        {
                            command.ExecuteNonQuery();
                        }
                        otherDB.Close();
                    }
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblSubCat WHERE catval = '" + vcat + "' AND value = '" + vsubcat + "'", otherDB))
                        {
                            command.ExecuteNonQuery();
                        }
                        otherDB.Close();
                    }
                    using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                    {
                        otherDB.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblType WHERE valcat = '" + vcat + "' AND valsubcat = '" + vsubcat + "' AND value = '" + vtype + "'", otherDB))
                        {
                            command.ExecuteNonQuery();
                        }
                        otherDB.Close();
                    }

                }
                else
                {
                    if (guna2CustomCheckBox2.Checked == true)
                    {
                        using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                        {
                            otherDB.Open();
                            using (SqlCommand command = new SqlCommand("DELETE FROM tblSubCat WHERE catval = '" + vcat + "' AND value = '" + vsubcat + "'", otherDB))
                            {
                                command.ExecuteNonQuery();
                            }
                            otherDB.Close();
                        }
                        using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                        {
                            otherDB.Open();
                            using (SqlCommand command = new SqlCommand("DELETE FROM tblType WHERE valcat = '" + vcat + "' AND valsubcat = '" + vsubcat + "' AND value = '" + vtype + "'", otherDB))
                            {
                                command.ExecuteNonQuery();
                            }
                            otherDB.Close();
                        }
                    }
                    else
                    {
                        if (guna2CustomCheckBox2.Checked == true)
                        {
                            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                            {
                                otherDB.Open();
                                using (SqlCommand command = new SqlCommand("DELETE FROM tblType WHERE valcat = '" + vcat + "' AND valsubcat = '" + vsubcat + "' AND value = '" + vtype + "'", otherDB))
                                {
                                    command.ExecuteNonQuery();
                                }
                                otherDB.Close();
                            }
                        }
                    }
                }
                MessageBox.Show("Deleted");
            }
        }
    }
}
