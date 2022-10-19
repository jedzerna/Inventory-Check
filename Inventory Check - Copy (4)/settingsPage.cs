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
                this.BackColor = Color.FromArgb(45, 44, 45);
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
    }
}
