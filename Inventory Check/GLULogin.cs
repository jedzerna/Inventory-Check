using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace Inventory_Check
{
    public partial class GLULogin : Form
    {


        public GLULogin()
        {

            InitializeComponent();
            pictureBox4.InitialImage = null;
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
        private void GLULogin_Load(object sender, EventArgs e)
        {
            System.Net.NetworkInformation.Ping ping = new Ping();

            PingReply result = ping.Send("www.google.com");

            if (result.Status == IPStatus.Success)
            {
                label4.Text = "Internet Connected";
            }
            else
            {
                //MessageBox.Show("Please make sure you time is updated, Go to Settings > Time&Language > Date & Time> (Make sure the ''Set Time Automatically'' is ON.)");
                label4.Text = "No Connection";
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            //pictureBox5.Image = pictureBox7.Image;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            //pictureBox5.Image = pictureBox8.Image;
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            //pictureBox6.Image = pictureBox7.Image;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            //pictureBox6.Image = pictureBox8.Image;
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBox2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void textBox2_MouseLeave(object sender, EventArgs e)
        {

        }
        private void label20_MouseEnter(object sender, EventArgs e)
        {

        }

        private void label20_MouseLeave(object sender, EventArgs e)
        {

        }
        private void panel2_MouseEnter(object sender, EventArgs e)
        {
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }
        string userid;
        private void login()
        {
            try
            {

                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select username,password from tblAccount where username = '" + guna2TextBox5.Text + "' COLLATE SQL_Latin1_General_CP1_CS_AS  and password = '" + guna2TextBox1.Text + "' COLLATE SQL_Latin1_General_CP1_CS_AS", tblSupplier);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {

                            if (Properties.Settings.Default.popup == "true")
                            {
                                AboutSystem frm = new AboutSystem();
                                frm.ShowDialog();
                            }
                            tblSupplier.Open();
                            String query1 = "SELECT * FROM tblAccount where username like '" + guna2TextBox5.Text.Trim() + "'";
                            SqlCommand cmd2 = new SqlCommand(query1, tblSupplier);
                            SqlDataReader dr1 = cmd2.ExecuteReader();

                            dashboardGLU f = new dashboardGLU();
                            if (dr1.Read())
                            {
                                f.username ="";
                                f.username = guna2TextBox5.Text;
                                f.id = (dr1["id"].ToString());
                                userid = (dr1["id"].ToString());


                            }
                            dr1.Close();
                            DateTime dat = DateTime.Now;
                            string date = dat.ToString("MM/dd/yyyy hh:mm tt");
                            string insStmt = "insert into accounthistory ([userid],[dateandtime],[computername]) values (@userid,@dateandtime,@computername)";
                            SqlCommand insCmd = new SqlCommand(insStmt, tblSupplier);
                            insCmd.Parameters.AddWithValue("@userid", userid.ToString());
                            insCmd.Parameters.AddWithValue("@dateandtime", date.ToString());
                            insCmd.Parameters.AddWithValue("@computername", System.Environment.MachineName.ToString());
                            int affectedRows = insCmd.ExecuteNonQuery();

                            tblSupplier.Close();




                            this.Hide();

                            f.ShowDialog();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            if (tblSupplier != null)
                            {
                                tblSupplier.Dispose();
                            }
                            if (cmd != null)
                            {
                                cmd.Dispose();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Login please check username and password");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void GLULogin_Paint(object sender, PaintEventArgs e)
        {

        }
        public void enforceAdminPrivilegesWorkaround()
        {
            RegistryKey rk;
            string registryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\";

            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    rk = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                }
                else
                {
                    rk = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
                }

                rk = rk.OpenSubKey(registryPath, true);
                MessageBox.Show(rk.ToString());
            }
            catch (System.Security.SecurityException ex)
            {
                MessageBox.Show("Please run as administrator");
                System.Environment.Exit(1);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void GLULogin_Load_1(object sender, EventArgs e)
        {
            color();
            //guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();

            //if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            //{
            //    MessageBox.Show("Program is already running...");
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
            //}



            //System.Net.NetworkInformation.Ping ping = new Ping();

            //PingReply result = ping.Send("www.google.com");

            //if (result.Status == IPStatus.Success)
            //{
            //    label4.Text = "Internet Connected";

            //}
            //else
            //{
            //    //MessageBox.Show("Please make sure you time is updated, Go to Settings > Time&Language > Date & Time> (Make sure the ''Set Time Automatically'' is ON.)");
            //    label4.Text = "No Connection";

            //}

            if (CheckForInternetConnection())
            {
                Isconnected = true;
                label4.Text = "Internet Connected";
            }
            else
            {
                Isconnected = false;
                label4.Text = "No Connection";
            }
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                try
                {

                    //var path = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\GLUConstInc\Jr Stocks Inventory\AppUpdator";
                    ////MessageBox.Show(path + "\AppUpdator.exe");
                    //Process.Start(path + "\\AppUpdator.exe");

                    //MessageBox.Show(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                    string fileName1 = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\InventoryCheck.Vsn";
                   

               
                    FileInfo fi = new FileInfo(fileName1);
                    if (fi.Exists)
                    {

                    }
                    else
                    {
                        string ver = "0.0.0.0";
                        // Create a new file     
                        using (FileStream fs = fi.Create())
                        {
                            Byte[] txt = new UTF8Encoding(true).GetBytes(ver);
                            fs.Write(txt, 0, txt.Length);
                        }
                    }
                    version = "";
                    if (File.Exists(fileName1))
                    {
                        // Read entire text file content in one string    
                        string text = File.ReadAllText(fileName1);
                        //MessageBox.Show(text.ToString().Trim());
                        version = text.ToString().Trim();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "  Please contact service provider!ASAP");
                    this.Close();
                }
                try
                {
                    tblSupplier.Open();
                    String query = "SELECT * FROM updatestat where id = 1";
                    SqlCommand cmd = new SqlCommand(query, tblSupplier);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {

                        if (dr["version"].ToString() == version || dr["version"].ToString() == "0JED")
                        {

                        }
                        else
                        {
                            try
                            {
                                viewupdateonfo a = new viewupdateonfo();

                                this.Hide();
                                a.ShowDialog();
                                this.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Please contact service provider");
                            }
                            //updateupdator();
                            //MessageBox.Show("Updates are available!!!");
                            //var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\AppUpdator";
                            ////MessageBox.Show(path + "\AppUpdator.exe");
                            //Process.Start(path + "\\AppUpdator.exe");

                            //Process[] workers = Process.GetProcessesByName("InventoryCheck");
                            //foreach (Process worker in workers)
                            //{
                            //    worker.Kill();
                            //    worker.WaitForExit();
                            //    worker.Dispose();
                            //}
                        }

                    }
                    dr.Close();
                    tblSupplier.Close();

                }
                catch
                {
                    var a = Environment.NewLine;
                    MessageBox.Show("Server is OFFLINE!!!" +a+a+"Here's the thing you can do...."+a+"1. Please make sure server is turn ON and connected to the LAN properly."
                        +a+"2. Make sure your PC is connected to the LAN."
                        +a+"3. Make sure you are connected to the same network in the server."
                        +a+"4. Please contact the admin for this issue. Thank you.");
                    Application.Exit();
                }
            }

        

            ResumeLayout();

        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);
                guna2TextBox5.IconLeft = My.Resources.Resources.icons8_security_user_male_24px;
                guna2TextBox1.IconLeft = My.Resources.Resources.icons8_grand_master_key_24px;

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label20.ForeColor = Color.White;

                guna2TextBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox1.ForeColor = Color.White;
                guna2TextBox5.ForeColor = Color.White;
                guna2TextBox5.FillColor = Color.FromArgb(34, 35, 35); 

            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                guna2TextBox5.IconLeft = My.Resources.Resources.icons8_username_48px_1;
                guna2TextBox1.IconLeft = My.Resources.Resources.icons8_password_48px;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label20.ForeColor = Color.Black;


                guna2TextBox1.FillColor = Color.White;
                guna2TextBox1.ForeColor = Color.Black;
                guna2TextBox5.FillColor = Color.White;
                guna2TextBox5.ForeColor = Color.Black;


            }
        }
        private void updateupdator()
        {
            string targetPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\AppUpdator";
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
        private string version;
        public static bool Isconnected = false;
        public static bool CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else if (reply.Status == IPStatus.TimedOut)
                {
                    return Isconnected;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void guna2TextBox5_MouseEnter(object sender, EventArgs e)
        {
        }

        private void guna2TextBox5_MouseLeave(object sender, EventArgs e)
        {
        }

        private void guna2TextBox5_Leave(object sender, EventArgs e)
        {
        }

        private void guna2TextBox5_Enter(object sender, EventArgs e)
        {
        }

        private void guna2TextBox5_MouseHover(object sender, EventArgs e)
        {
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            login();
        }

        private void guna2TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
                e.Handled = true;
            }
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
                e.Handled = true;
            }
        }

        private void GLULogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void GLULogin_Shown(object sender, EventArgs e)
        {

        }

        private void GLULogin_Resize(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked)
            {
                guna2TextBox1.PasswordChar = '\0';
            }
            else
            {
                guna2TextBox1.PasswordChar = '●';
            }
        }
    }
}
