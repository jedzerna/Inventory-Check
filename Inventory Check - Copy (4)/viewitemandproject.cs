using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class viewitemandproject : Form
    {
        public string name;

        public viewitemandproject()
        {
            InitializeComponent();
            pictureBox5.InitialImage = null;
        }

        private void viewitemandproject_Load(object sender, EventArgs e)
        {
            color();
            itemsleft pr = new itemsleft();
            pr.Height = panel2.Height;
            pr.Width = panel2.Width;
            pr.TopLevel = false;
            pr.name = name;
            panel2.Controls.Add(pr);
            panel2.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            button5.Visible = false;
            Cursor.Current = Cursors.Default;
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label22.ForeColor = Color.White;
                panel1.BackColor = Color.FromArgb(15, 14, 15);
                panel2.BackColor = Color.FromArgb(15, 14, 15);

            }
            else
            {
                panel1.BackColor = Color.FromArgb(243, 243, 243);
                panel2.BackColor = Color.FromArgb(243, 243, 243);
                this.BackColor = Color.FromArgb(243, 243, 243);

                label1.ForeColor = Color.Black;
                label22.ForeColor = Color.Black;
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
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

        private void button4_Click(object sender, EventArgs e)
        {
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            categories c = new categories();
            c.ShowDialog();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            button5.Visible = true;
            label1.Text = "Projects";
            label22.Text = "Here you can view the project codes and the details each";
            project pr = new project();
            pr.Height = panel2.Height;
            pr.Width = panel2.Width;
            pr.TopLevel = false;
            //pr.name = name;
            panel2.Controls.Add(pr);
            panel2.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            pictureBox5.Image = pictureBox1.Image;
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {

            button4.Visible = true;
            button5.Visible = false;
            label1.Text = "Item Supplies";
            label22.Text = "Purchase Order Previous Transaction";
            itemsleft pr = new itemsleft();
            pr.Height = panel2.Height;
            pr.Width = panel2.Width;
            pr.TopLevel = false;
            pr.name = name;
            panel2.Controls.Add(pr);
            panel2.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            pictureBox5.Image = pictureBox2.Image;
            Cursor.Current = Cursors.Default;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
