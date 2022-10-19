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
    public partial class dashboardGLU : Form
    {
        private string mouse;
        public string username;
        public string id;
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams handleparam = base.CreateParams;
        //        handleparam.ExStyle |= 0x02000000;
        //        return handleparam;
        //    }
        //}
        bool bEnableAntiFlicker = true;
        int intOriginalExStyle = -1;
        protected override CreateParams CreateParams
        {
            get
            {
                if (intOriginalExStyle == -1)
                {
                    intOriginalExStyle = base.CreateParams.ExStyle;
                }
                CreateParams cp = base.CreateParams;
                if (bEnableAntiFlicker)
                {
                    cp.ExStyle |= 0x02000000; //WS_EX_COMPOSITED
                }
                else
                {
                    cp.ExStyle = intOriginalExStyle;
                }
                return cp;
            }
        }
        private void ToggleAntiFlicker(bool Enable)
        {
            bEnableAntiFlicker = Enable;
            this.MaximizeBox = true;
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

        public dashboardGLU()
        {

            InitializeComponent();
            //ResizeRedraw = true;
            //this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
            //              ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
        }

        SqlCommand cmd;
        public void load()
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                try
                {
                    tblSupplier.Open();
                    String query = "SELECT * FROM tblAccount where id like '" + id + "'";
                    cmd = new SqlCommand(query, tblSupplier);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {


                        label1.Text = (dr["fullname"].ToString());
                        label4.Text = (dr["type"].ToString());
                        username = (dr["username"].ToString());
                        //pictureBox4.Image = dr["image"];
                        if (dr["image"] != DBNull.Value)
                        {
                            byte[] img = (byte[])(dr["image"]);
                            MemoryStream mstream = new MemoryStream(img);
                            guna2CirclePictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                        }
                        if (label4.Text == "User")
                        {
                            guna2Button5.Visible = false;
                        }
                        else if (label4.Text == "Developer")
                        {
                            label7.Visible = true;
                            guna2Button5.Visible = true;
                        }
                        else
                        {

                            guna2Button5.Visible = true;
                        }

                    }
                    dr.Close();
                    tblSupplier.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

                    tblSupplier.Open();
                    String query = "SELECT * FROM updatestat where id = 1";
                    cmd = new SqlCommand(query, tblSupplier);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {

                        if (dr["version"].ToString() == version)
                        {
                            pictureBox1.Visible = false;
                            label6.Visible = false;
                            timer1.Stop();
                        }
                        else
                        {
                            pictureBox1.Visible = true;
                            label6.Visible = true;
                        }

                    }
                    dr.Close();
                    tblSupplier.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }
        private string version;
        private void label1_Click(object sender, EventArgs e)
        {
            //Form1 f = new Form1();
            //this.Close();
            //f.Show();

            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            Form1 frm = new Form1();
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dashboardGLU_Load(object sender, EventArgs e)
        {
            //guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            load();

            guna2CirclePictureBox1.InitialImage = null;

            //Image clonedImg = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            //using (var copy = Graphics.FromImage(clonedImg))
            //{
            //    copy.DrawImage(pictureBox1.Image, 0, 0);
            //}
            ////pictureBox1.InitialImage = null;
            //pictureBox1.Image = clonedImg;    

            changefill();
            guna2Button6.FillColor = Color.FromArgb(56, 163, 255);
            dashboard pr = new dashboard();
            pr.name = label1.Text;
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            panel3.Controls.Clear();
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();

            ResumeLayout();
            //this.WindowState = FormWindowState.Maximized;

            color();
            if (Properties.Settings.Default.popup == "true")
            {
                AboutSystem frm = new AboutSystem();
                frm.ShowDialog();
            }
        }
      

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void dashboardGLU_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {


        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            prein pr = new prein();
            pr.TopLevel = false;
            pr.name = label1.Text;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
        }

        private void pictureBox8_MouseEnter(object sender, EventArgs e)
        {
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            if (mouse != "2")
            {
            }


        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            viewitemandproject pr = new viewitemandproject();
            pr.TopLevel = false;
            pr.name = label1.Text;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void pictureBox11_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox11_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_MouseEnter(object sender, EventArgs e)
        {
        }

        private void pictureBox14_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            mouse = "2";
            outlist pr = new outlist();
            pr.TopLevel = false;
            pr.name = label1.Text;
            pr.num = "1";
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
        }

        private void pictureBox17_MouseEnter(object sender, EventArgs e)
        {
        }

        private void pictureBox17_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {

            MessageBox.Show("add");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox20_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox20_MouseLeave(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to log out?", "Log Out?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //(So Loading Bar only shows if the Method takes longer than 1 Second)

                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                GLULogin frm = new GLULogin();

                frm.ShowDialog();
                this.Close();
                //Close the form.(frm_form1)
            }
        }

        private void pictureBox20_MouseEnter_1(object sender, EventArgs e)
        {
        }

        private void pictureBox20_MouseLeave_1(object sender, EventArgs e)
        {

        }


        private void pictureBox24_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
         
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
        }

        private bool needShadow = false;
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox20_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            mouse = "6";

            aAccounts pr = new aAccounts();
            pr.name = label1.Text;
            pr.username = username;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            //guna2Button1.FillColor = Color.FromArgb(58, 58, 58);
            //guna2Button2.FillColor = Color.FromArgb(58, 58, 58);
            //guna2Button3.FillColor = Color.FromArgb(58, 58, 58);
            //guna2Button4.FillColor = Color.FromArgb(58, 58, 58);
            //guna2Button5.FillColor = Color.FromArgb(58, 58, 58);
            //guna2Button6.FillColor = Color.FromArgb(58, 58, 58);
            //guna2Button8.FillColor = Color.FromArgb(41, 41, 41);
            //guna2Button7.FillColor = Color.FromArgb(58, 58, 58);

            //uForm pr = new uForm();
            //pr.username = username;
            //pr.TopLevel = false;
            //panel3.Controls.Add(pr);
            //panel3.AutoScroll = false;
            //pr.Height = panel3.Height;
            //pr.Width = panel3.Width;
            //pr.BringToFront();
            //pr.Show();
            //Cursor.Current = Cursors.Default;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //prein pr = new prein();
            //panel3.Controls.Clear();
            //pr.TopLevel = false;
            //pr.name = label1.Text;
            //panel3.Controls.Add(pr);
            //panel3.AutoScroll = false;
            //pr.BringToFront();
            //pr.Show();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            changefill();
            guna2Button1.FillColor = Color.FromArgb(56, 163, 255);
            panel3.Controls.Clear();
            prein pr = new prein();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.TopLevel = false;
            pr.name = label1.Text;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            changefill();
            guna2Button2.FillColor = Color.FromArgb(56, 163, 255);
            outlist pr = new outlist();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            panel3.Controls.Clear();
            pr.TopLevel = false;
            pr.name = label1.Text;
            pr.num = "1";
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.BringToFront();
            pr.Show();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            changefill();
            guna2Button3.FillColor = Color.FromArgb(56, 163, 255);
            Cursor.Current = Cursors.WaitCursor;
            viewitemandproject pr = new viewitemandproject();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            panel3.Controls.Clear();
            pr.TopLevel = false;
            pr.name = label1.Text;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            changefill();
            guna2Button4.FillColor = Color.FromArgb(56, 163, 255);
            Cursor.Current = Cursors.WaitCursor;
            reportsgen pr = new reportsgen();
            panel3.Controls.Clear();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            //pr.name = label1.Text;
            //pr.username = username;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.name = label1.Text;

            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            changefill();
            guna2Button5.FillColor = Color.FromArgb(56, 163, 255);
            Cursor.Current = Cursors.WaitCursor;
            panel3.Controls.Clear();
            aAccounts pr = new aAccounts();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            //pr.Height = panel3.Height;
            //pr.Width = panel3.Width;
            pr.name = label1.Text;
            pr.username = username;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            changefill();

            uForm pr = new uForm();
            pr.username = username;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            changefill();
            guna2Button6.FillColor = Color.FromArgb(56, 163, 255);
            dashboard pr = new dashboard();
            pr.name = label1.Text;
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            panel3.Controls.Clear();
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
        }

        private void bunifuImageButton8_Click(object sender, EventArgs e)
        {
        }

        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            drsummary pr = new drsummary();
            panel3.Controls.Clear();
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            changefill();

            uForm pr = new uForm();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.username = username;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }
        public string colordw
        {
            get { return label10.Text; }
            set { label10.Text = value; }
        }
        private void guna2Button8_Click(object sender, EventArgs e)
        {
            changefill();
            guna2Button8.FillColor = Color.FromArgb(56, 163, 255);

            Cursor.Current = Cursors.WaitCursor;
            settingsPage pr = new settingsPage(this);
            panel3.Controls.Clear();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            changefill();
            guna2Button7.FillColor = Color.FromArgb(56, 163, 255);
            Cursor.Current = Cursors.WaitCursor;
            supplierlist pr = new supplierlist();
            panel3.Controls.Clear();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.name = label1.Text;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuImageButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
         
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {

        }
        dashboard dash = (dashboard)Application.OpenForms["dashboard"];
        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {

        }
        private void panel3_Resize(object sender, EventArgs e)
        {
        }

        private void dashboardGLU_RegionChanged(object sender, EventArgs e)
        {
         
        }

        private void dashboardGLU_Resize(object sender, EventArgs e)
        {
            //panel3.Refresh();
            //panel3.Update();   
        }

        private void dashboardGLU_ResizeBegin(object sender, EventArgs e)
        {
            ToggleAntiFlicker(true);
        }

        private void guna2CircleButton1_MouseEnter(object sender, EventArgs e)
        {
            //guna2CircleButton1.BackgroundImage = My.Resources.Resources.exitfinal21;
        }

        private void guna2CircleButton1_MouseLeave(object sender, EventArgs e)
        {

            //guna2CircleButton1.BackgroundImage = My.Resources.Resources.exitfinal;
        }

        private void guna2CircleButton3_MouseEnter(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton3_MouseLeave(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton2_MouseEnter(object sender, EventArgs e)
        {
        }

        private void guna2CircleButton2_MouseLeave(object sender, EventArgs e)
        {
        }

        private void guna2CircleButton3_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2CircleButton3_MouseEnter_1(object sender, EventArgs e)
        {
            //guna2CircleButton3.Text = "---";
        }

        private void guna2CircleButton3_MouseLeave_1(object sender, EventArgs e)
        {
            //guna2CircleButton3.Text = "";
        }

        private void label6_Click_1(object sender, EventArgs e)
        {
            try
            {
                viewupdateonfo a = new viewupdateonfo();
                a.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Please contact service provider");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label6.Visible == true)
            {
                if (pictureBox1.Visible == true)
                {
                    pictureBox1.Visible = false;
                }
                else if (pictureBox1.Visible == false)
                {
                    pictureBox1.Visible = true;
                }
            }

        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        bool ws;
        private void guna2CircleButton2_Click_1(object sender, EventArgs e)
        {
            //if (ws == true)
            //{
            //    Width = 1100;
            //    Height = 612;
            //    //guna2Elipse1.BorderRadius = 20;
            //    ws = false;
            //}
            //else
            //{
            //    FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //    Left = Top = 0;
            //    Width = Screen.PrimaryScreen.WorkingArea.Width;
            //    Height = Screen.PrimaryScreen.WorkingArea.Height;
            //    //guna2Elipse1.BorderRadius = 0;

            //    ws = true;
            //}
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    tblSupplier.Open();
                    String query = "SELECT * FROM updatestat where id = 1";
                    cmd = new SqlCommand(query, tblSupplier);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {

                        if (dr["version"].ToString() == version)
                        {
                            pictureBox1.Visible = false;
                            label6.Visible = false;
                            //timer1.Stop();
                        }
                        else
                        {
                            timer1.Start();
                            pictureBox1.Visible = true;
                            label6.Visible = true;
                        }
                    }
                    dr.Close();
                    tblSupplier.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            developer d = new developer();
            d.ShowDialog();
        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pdfviewer a = new pdfviewer();
            a.Show();
        }

        private void dashboardGLU_ResizeEnd(object sender, EventArgs e)
        {
            ToggleAntiFlicker(false);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
           
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            changefill();

            uForm pr = new uForm();
            pr.Height = panel3.Height;
            pr.Width = panel3.Width;
            pr.username = username;
            pr.TopLevel = false;
            panel3.Controls.Add(pr);
            panel3.AutoScroll = false;
            pr.BringToFront();
            pr.Show();
            Cursor.Current = Cursors.Default;
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button9_Click_1(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            Form1 frm = new Form1();
            frm.createdby = label1.Text;
            frm.username = username;
            frm.id = id;
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {


            Cursor.Current = Cursors.WaitCursor;
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            DRForm frm = new DRForm();
            frm.createdby = label1.Text;
            frm.username = username;
            frm.id = id;
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }

        private void label8_Click_2(object sender, EventArgs e)
        {
            AboutSystem frm = new AboutSystem();
            frm.ShowDialog();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {

        }
        private void changefill()
        {
            if (Properties.Settings.Default.color == "true")
            {
                guna2Button1.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button8.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button2.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button3.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button4.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button5.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button6.FillColor = Color.FromArgb(58, 58, 58);
                guna2Button7.FillColor = Color.FromArgb(58, 58, 58);
            }
            else
            {
                guna2Button1.FillColor = Color.Purple;
                guna2Button2.FillColor = Color.Maroon;
                guna2Button3.FillColor = Color.Teal;
                guna2Button4.FillColor = Color.Navy;
                guna2Button5.FillColor = Color.Green;
                guna2Button6.FillColor = Color.OliveDrab;
                guna2Button7.FillColor = Color.FromArgb(128, 64, 0);
                guna2Button8.FillColor = Color.DarkCyan;
            }
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);


                guna2Panel1.FillColor = Color.FromArgb(30, 30, 30);
                guna2Panel2.FillColor = Color.FromArgb(34, 35, 35);
                panel1.BackColor = Color.FromArgb(15, 14, 15);


                label1.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label10.ForeColor = Color.White;

                pictureBox1.Image = My.Resources.Resources.icons8_warning_shield_24px;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);


                panel1.BackColor = Color.FromArgb(243, 243, 243);
                guna2Panel1.FillColor = Color.White;
                guna2Panel2.FillColor = Color.White;


                label1.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;


                pictureBox1.Image = My.Resources.Resources.icons8_warning_shield_48px;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            
        }

        private void label10_TextChanged(object sender, EventArgs e)
        {
            color();
            changefill();
            guna2Button8.FillColor = Color.FromArgb(56, 163, 255);
        }
    }
}
