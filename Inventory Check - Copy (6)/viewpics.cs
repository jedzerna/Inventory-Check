using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Inventory_Check
{
    public partial class viewpics : Form
    {
        public string id;
        public string num;
   
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
        protected override bool ShowFocusCues => false;
        public viewpics()
        {
            SuspendLayout();
            InitializeComponent();
            pictureBox4.InitialImage = null;
            pictureBox10.InitialImage = null;
            pictureBox11.InitialImage = null;
            ResumeLayout();
        }

        private void viewpics_Load(object sender, EventArgs e)
        {

            guna2ShadowForm1.SetShadowForm(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
        scan obj1 = (scan)Application.OpenForms["scan"];

        out_supply obj3 = (out_supply)Application.OpenForms["out_supply"];
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            if (num == "1")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete this document?", "Delete?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {

                    tblIn.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblInImage WHERE id = '" + id + "'", tblIn))
                    {
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Deleted");
                    tblIn.Close();
                    tblIn.Dispose();
                }
                    obj1.load();
                    this.Close();
                }
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete this document?", "Delete?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                

                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblOutImage WHERE id = '" + id + "'", dbDR))
                        {
                            command.ExecuteNonQuery();
                        }
                        MessageBox.Show("Deleted");
                        dbDR.Close();
                        dbDR.Dispose();
                    }
                    obj1.load2();
                    this.Close();
                }
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox4.Image.Save(saveFileDialog.FileName);
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
