using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class settingsPage : Form
    {
        public settingsPage()
        {
            InitializeComponent();
        }

        private dashboardGLU dash = null;
        public settingsPage(Form callingForm)
        {
            dash = callingForm as dashboardGLU;

            InitializeComponent();
        }
        public string name;
        private void settingsPage_Load(object sender, EventArgs e)
        {
            color();
            timer1.Start();
            if (Properties.Settings.Default.color == "true")
            {
                guna2ToggleSwitch1.Checked = true;
            }
            else
            {
                guna2ToggleSwitch1.Checked = false;
            }
            if (Properties.Settings.Default.popup == "true")
            {
                guna2ToggleSwitch2.Checked = true;
            }
            else
            {
                guna2ToggleSwitch2.Checked = false;
            }
        }
        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                Properties.Settings.Default.color = "true";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.color = "false";
                Properties.Settings.Default.Save();
            }
            color();

            dash.colordw = Properties.Settings.Default.color;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            color();
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);
                guna2Panel1.FillColor = Color.FromArgb(36, 37, 37);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);
                guna2Panel1.FillColor = Color.White;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
            }
        }

        private void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {

            if (guna2ToggleSwitch2.Checked)
            {
                Properties.Settings.Default.popup = "true";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.popup = "false";
                Properties.Settings.Default.Save();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse DBF Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "DBF files (*.dbf)|*.dbf",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
               string path = openFileDialog1.FileName;
                guna2TextBox1.Text = Path.GetFileName(openFileDialog1.FileName);


                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    string sqlTrunc = "TRUNCATE TABLE GLU4";
                    SqlCommand cmd = new SqlCommand(sqlTrunc, otherDB);
                    otherDB.Open();
                    cmd.ExecuteNonQuery();
                    otherDB.Close();


                    DataTable YourResultSet = new DataTable();

                    OleDbConnection yourConnectionHandler = new OleDbConnection(
                        @"Provider=VFPOLEDB.1;Data Source='" + path.Replace(guna2TextBox1.Text, "") + "'");

                    yourConnectionHandler.Open();

                    if (yourConnectionHandler.State == ConnectionState.Open)
                    {
                        string mySQL = "select * from GLU4";  // dbf table name

                        OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                        OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                        DA.Fill(YourResultSet);

                        yourConnectionHandler.Close();
                    }
                    YourResultSet.Columns.Add("createdby");
                    foreach (DataRow row in YourResultSet.Rows)
                    {
                        row["createdby"] = name;
                        row["ACCTCODE"] = row["ACCTCODE"].ToString().Replace("¤","ñ").Replace("¥", "Ñ").Trim();
                        row["ACCTDESC"] = row["ACCTDESC"].ToString().Replace("¤", "ñ").Replace("¥", "Ñ").Trim();
                    }

                    DataTableReader reader = YourResultSet.CreateDataReader();
                    otherDB.Open();  ///this is my connection to the sql server
                    SqlBulkCopy sqlcpy = new SqlBulkCopy(otherDB);
                    sqlcpy.DestinationTableName = "GLU4";  //copy the datatable to the sql table
                    sqlcpy.WriteToServer(YourResultSet);
                    otherDB.Close();
                    reader.Close();

                }
                MessageBox.Show("LOANS IS UPDATED");
            }
        }
    }
}
