using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class viewupdateonfo : Form
    {
        //int location_x;
        //int location_y;
        public viewupdateonfo()
        {
            InitializeComponent();
            //location_x = guna2HtmlLabel1.Location.X;
            //location_y = guna2HtmlLabel1.Location.Y;
        }
        private void viewupdateonfo_Load(object sender, EventArgs e)
        {
         
            //this.guna2HtmlLabel1.GetHtml();
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                String query = "SELECT * FROM updatestat where id = 1";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    guna2HtmlLabel1.Text = dr["updatestats"].ToString();
                    label4.Text = dr["version"].ToString();
                }
                dr.Close();
                tblSupplier.Close();
            }

            //guna2HtmlLabel1.Controls.Add(guna2VScrollBar1);
        }


        private void update()
        {
            try
            {
                ////updateupdator();

                string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\AppUpdator";
                Process.Start(path + "\\AppUpdator.exe");

                Process[] workers = Process.GetProcessesByName("InventoryCheck");
                foreach (Process worker in workers)
                {
                    worker.Kill();
                    worker.WaitForExit();
                    worker.Dispose();
                }
                viewupdateonfo a = new viewupdateonfo();
                a.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Please contact service provider");
            }
        }
        private void updateupdator()
        {
            string targetPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AppUpdator";

            //string targetPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\AppUpdator";
            string sourcePath = @"\\JED-PC\Users\Public\Documents\Debug\AppUpdator";

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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to update?"+Environment.NewLine+ Environment.NewLine + "The app will close when updating...", "Update", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                update();
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            //Panel 02: Allow Auto Size to Panel. Then set the mode in which auto size works
     
        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
    
        }

        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
     
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
     
        }
    }
}
