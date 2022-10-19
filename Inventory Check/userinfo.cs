using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Inventory_Check
{
    public partial class userinfo : Form
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
        public userinfo()
        {
            InitializeComponent();
        }
        public string id;
        private void userinfo_Load(object sender, EventArgs e)
        {
            color();
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {

                tblSupplier.Open();
                String query = "SELECT * FROM tblAccount where id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    label4.Text = dr["type"].ToString();
                    label1.Text = dr["fullname"].ToString();
                    if (dr["image"] != DBNull.Value)
                    {
                        byte[] img = (byte[])(dr["image"]);
                        MemoryStream mstream = new MemoryStream(img);
                        guna2CirclePictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                    }
                }
            }
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label4.ForeColor = Color.White;

            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);


                label1.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
            }
        }
    }
}
