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
    public partial class AboutSystem : Form
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
        public AboutSystem()
        {
            InitializeComponent();
        }

        private void AboutSystem_Load(object sender, EventArgs e)
        {
            string latest = "";
            string version = "";

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
                    latest = dr["version"].ToString();
                }
                dr.Close();
                tblSupplier.Close();
            }

            try
            {
                string fileName1 = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\InventoryCheck.Vsn";

                version = "";
                if (File.Exists(fileName1))
                {
                    // Read entire text file content in one string    
                    string text = File.ReadAllText(fileName1);
                    //MessageBox.Show(text.ToString().Trim());
                    version = text.ToString().Trim();
                }

                if (latest != version)
                {
                    MessageBox.Show("Please update the system to avoid future errors.");
                    viewupdateonfo a = new viewupdateonfo();
                    a.ShowDialog();
                }
                label4.Text = version;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
