using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class developer : Form
    {
        public developer()
        {
            InitializeComponent();
        }

        private void developer_Load(object sender, EventArgs e)
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                String query = "SELECT * FROM updatestat where id = 1";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    guna2TextBox2.Text = dr["updatestats"].ToString();
                    guna2TextBox1.Text = dr["version"].ToString();
                }
                dr.Close();
                tblSupplier.Close();
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2HtmlLabel1.Text = guna2TextBox2.Text;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to add this update?" , "Add", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                updateupdator();
                getupdatedfiles();
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update updatestat set updatestats=@updatestats,version=@version where id = 1", tblSupplier);

                    tblSupplier.Open();
                    cmd.Parameters.AddWithValue("@updatestats", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@version", guna2TextBox1.Text);
                    cmd.ExecuteNonQuery();
                    tblSupplier.Close();
                }
                MessageBox.Show("Update Info Added");
            }
        }
        private void updateupdator()
        {
            string sourcePath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AppUpdator";
            string targetPath = @"\\JED-PC\Users\Public\Documents\Debug\AppUpdator";

            string fileName = "AppUpdator.exe";
            string sourceFile = System.IO.Path.Combine(sourcePath, "AppUpdator.exe");
            string destFile = System.IO.Path.Combine(targetPath, "AppUpdator.exe");
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourcePath);
            int count = Directory.GetFiles(sourcePath).Length;
            System.IO.File.Copy(sourceFile, destFile, true);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);


                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);

                }

            }
            else
            {
                MessageBox.Show("Source path does not exist!", "Error");
            }
        }
        private void getupdatedfiles()
        {

            string sourcePath = Path.GetDirectoryName(Application.ExecutablePath);
            string targetPath = @"\\JED-PC\Users\Public\Documents\Debug";

            //MessageBox.Show(targetPath);
            string fileName = "InventoryCheck.exe";
            string sourceFile = System.IO.Path.Combine(sourcePath, "InventoryCheck.exe");
            string destFile = System.IO.Path.Combine(targetPath, "InventoryCheck.exe");
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourcePath);
            int count = Directory.GetFiles(sourcePath).Length;
            System.IO.File.Copy(sourceFile, destFile, true);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);


                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);

                }

            }
            else
            {
                MessageBox.Show("Source path does not exist!", "Error");
            }
        }
    }
}
