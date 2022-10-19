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

using WIA;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace Inventory_Check
{
    public partial class scan : Form
    {
     
        public string idsup;
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
        public scan()
        {
            InitializeComponent();
            pictureBox2.InitialImage = null;
            pictureBox3.InitialImage = null;
            pictureBox5.InitialImage = null;

            pictureBox8.InitialImage = null;
            pictureBox9.InitialImage = null;
            pictureBox10.InitialImage = null;
            pictureBox11.InitialImage = null;
        }
    
        //public Pen cropPen;
        public DashStyle cropDashStyle = DashStyle.DashDot;
        private void scan_Load(object sender, EventArgs e)
        {

            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;
            var deviceManager = new DeviceManager();
                for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                {
                    if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                    {
                        continue;
                    }
                    lstListOfScanner.Items.Add(deviceManager.DeviceInfos[i].Properties["Name"].get_Value());
                }
            if (num == "1")
            {
                load();
            }
            else
            {
                load2();
            }
            Cursor.Current = Cursors.Default; 
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {

                tblIn.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT image,id FROM tblInImage WHERE idsup='" + idsup + "'", tblIn))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = item["id"].ToString();

                    dataGridView1.Rows[n].Cells[2].Value = item["image"];
                }
                tblIn.Close();

                this.dataGridView1.Sort(this.dataGridView1.Columns[4], ListSortDirection.Ascending);

                tblIn.Dispose();


            }
        }
        public void load2()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {

                dbDR.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT image,id FROM tblOutImage WHERE idsup='" + idsup + "'", dbDR))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = item["id"].ToString();

                    dataGridView1.Rows[n].Cells[2].Value = item["image"];
                }
                dbDR.Close();

                this.dataGridView1.Sort(this.dataGridView1.Columns[4], ListSortDirection.Ascending);

                dbDR.Dispose();


            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            //try
            //{
                var deviceManager = new DeviceManager();

                DeviceInfo AvailableScanner = null;
                for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                {
                    if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                    {
                        continue;
                    }
                    AvailableScanner = deviceManager.DeviceInfos[i];
                    break;
                }


                var device = AvailableScanner.Connect();
                var ScannerItem = device.Items[1];

                CommonDialogClass dlg = new CommonDialogClass();
            try
            {
                object scanResult = dlg.ShowTransfer(ScannerItem, WIA.FormatID.wiaFormatJPEG, true);

                if (scanResult != null)
                {

                   
                    ImageFile image = (ImageFile)scanResult;

                 
                }
                else
                {

                }
            }
            catch (COMException d)
            {
                // Display the exception in the console.

                //MessageBox.Show(d.ToString());
                uint errorCode = (uint)d.ErrorCode;

                // Catch 2 of the most common exceptions
                if (errorCode == 0x80210006)
                {
                    MessageBox.Show("The scanner is busy or isn't ready");
                }
                else if (errorCode == 0x80210064)
                {
                    MessageBox.Show("The scanning process has been cancelled.");
                }
            }


            Cursor.Current = Cursors.WaitCursor;
            var imgFile = (ImageFile)ScannerItem.Transfer(FormatID.wiaFormatJPEG);

            var path = @"C:\Scanned\ScanImg.jpg";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            imgFile.SaveFile(path);
            pictureBox2.ImageLocation = path;
            Cursor.Current = Cursors.Default;
            //int resolution = 150;
            //int width_pixel = 1250;
            //int height_pixel = 1700;
            //int color_mode = 1; 
            //AdjustScannerSettings(ScannerItem, resolution, 0, 0, width_pixel, height_pixel, 0, 0, color_mode);



            //}
            //catch (COMException ex)
            //{
            //    uint errorCode = (uint)ex.ErrorCode;

            //    // See the error codes
            //    if (errorCode == 0x80210006)
            //    {
            //        MessageBox.Show("The scanner is busy or isn't ready");
            //    }
            //    else if (errorCode == 0x80210064)
            //    {
            //        MessageBox.Show("The scanning process has been cancelled.");
            //    }
            //    else if (errorCode == 0x8021000C)
            //    {
            //        MessageBox.Show("There is an incorrect setting on the WIA device.");
            //    }
            //    else if (errorCode == 0x80210005)
            //    {
            //        MessageBox.Show("The device is offline. Make sure the device is powered on and connected to the PC.");
            //    }
            //    else if (errorCode == 0x80210001)
            //    {
            //        MessageBox.Show("An unknown error has occurred with the WIA device.");
            //    }
            //}
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        In_supply obj = (In_supply)Application.OpenForms["In_supply"];
        out_supply obj2 = (out_supply)Application.OpenForms["out_supply"];
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (num == "1")
            {
                if (label3.Text == "select")
                {

                    MessageBox.Show("Please select image...");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to save this document?", "Save?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        MemoryStream ms = new MemoryStream();
                        pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                        byte[] img11 = ms.ToArray();

                        using(SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {

                            tblIn.Open();
                            SqlCommand cmd = new SqlCommand("insert into tblInImage (idsup, image,name) values ('" + idsup + "',@images,'"+guna2TextBox3.Text+"')", tblIn);



                            cmd.Parameters.Add("@images", SqlDbType.Image).Value = img11;

                            cmd.ExecuteNonQuery();
                            tblIn.Close();
                            tblIn.Dispose();
                        }
                        load();
                        MessageBox.Show("Saved");
                    }
                }

            }
            else
            {
                if (label3.Text == "select")
                {

                    MessageBox.Show("Please select image...");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure to save this document?", "Save?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        MemoryStream ms = new MemoryStream();
                        pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                        byte[] img11 = ms.ToArray();

                        using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {

                            dbDR.Open();
                            SqlCommand cmd = new SqlCommand("insert into tblOutImage (idsup, image,name) values ('" + idsup + "',@images,'" + guna2TextBox3.Text + "')", dbDR);



                            cmd.Parameters.Add("@images", SqlDbType.Image).Value = img11;

                            cmd.ExecuteNonQuery();
                            dbDR.Close();
                            dbDR.Dispose();
                        }
                        load2();
                        MessageBox.Show("Saved");
                    }
                }
            }
            
        }

        private void dataGridView1_MouseEnter(object sender, EventArgs e)
        {
         
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    Cursor = Cursors.Cross;
            //    cropX = e.X;
            //    cropY = e.Y;
            //    cropPen = new Pen(Color.Black, 1);
            //    cropPen.DashStyle = DashStyle.DashDotDot;
            //}
            //pictureBox2.Refresh();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            //if (pictureBox2.Image == null)
            //    return;
            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    pictureBox2.Refresh();
            //    cropWidth = e.X - cropX;
            //    cropHeight = e.Y - cropY;
            //    pictureBox2.CreateGraphics().DrawRectangle(cropPen, cropX, cropY, cropWidth, cropHeight);
            //}
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //Cursor = Cursors.Default;
            //if (cropWidth < 1)
            //{
            //    return;
            //}
            //Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            ////First we define a rectangle with the help of already calculated points  
            //Bitmap OriginalImage = new Bitmap(pictureBox2.Image, pictureBox2.Width, pictureBox2.Height);
            ////Original image  
            //Bitmap _img = new Bitmap(cropWidth, cropHeight);
            //// for cropinf image  
            //Graphics g = Graphics.FromImage(_img);
            //// create graphics  
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            ////set image attributes  
            //g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
            //pictureBox2.Image = _img;
            //pictureBox2.Width = _img.Width;
            //pictureBox2.Height = _img.Height;
            //pictureBox2Location();
            //pictureBox3.Enabled = false;
            //pictureBox3.BackgroundImageLayout = ImageLayout.Center;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {

        }

        private void lstListOfScanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (lstListOfScanner.SelectedItem != null)
            {
                var deviceManager = new DeviceManager();

                DeviceInfo AvailableScanner = null;
                for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                {
                    if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                    {
                        continue;
                    }
                    AvailableScanner = deviceManager.DeviceInfos[i];
                    break;
                }


                var device = AvailableScanner.Connect();
                var ScannerItem = device.Items[1];

                CommonDialogClass dlg = new CommonDialogClass();
                try
                {
                    object scanResult = dlg.ShowTransfer(ScannerItem, WIA.FormatID.wiaFormatJPEG, true);

                    if (scanResult != null)
                    {


                        ImageFile image = (ImageFile)scanResult;


                    }
                    else
                    {

                    }
                }
                catch (COMException d)
                {
                    // Display the exception in the console.

                    //MessageBox.Show(d.ToString());
                    uint errorCode = (uint)d.ErrorCode;

                    // Catch 2 of the most common exceptions
                    if (errorCode == 0x80210006)
                    {
                        MessageBox.Show("The scanner is busy or isn't ready");
                    }
                    else if (errorCode == 0x80210064)
                    {
                        MessageBox.Show("The scanning process has been cancelled.");
                    }
                }


                Cursor.Current = Cursors.WaitCursor;
                var imgFile = (ImageFile)ScannerItem.Transfer(FormatID.wiaFormatJPEG);

                //var path = @"|DataDirectory|\ScanImg.jpg";
                //if (File.Exists(path))
                //{
                //    File.Delete(path);
                //}
                //imgFile.SaveFile(path);
                //string lastFile = imgFile.ToString();

                var imageBytes = (byte[])imgFile.FileData.get_BinaryData();
                var ms = new MemoryStream(imageBytes);
                var img = Image.FromStream(ms);

                pictureBox2.Image = img;
                label3.Text = "";
                Cursor.Current = Cursors.Default;
            }
            else
            {
                MessageBox.Show("Please select printer...");
               
            }
            }
            catch
            {
                    MessageBox.Show("Somethings wrong, please contact the developer..");
              
            }


        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
           
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg;)|*.jpg; *.jpeg;";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box  
                    pictureBox2.Image = new Bitmap(open.FileName);
                    // image file path  
                    label3.Text = "";
                }
            
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

            //dataGridView1.Columns["Column7"].ReadOnly = false;
            dataGridView1.Columns["Column4"].Visible = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //DialogResult dialogResult = MessageBox.Show("Are you sure to delete this document?", "Delete?", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                //MessageBox.Show(dataGridView1.Rows[1].Cells[0].Value.ToString());
                //MessageBox.Show(dataGridView1.Rows[1].Cells[1].Value.ToString());
                //MessageBox.Show(dataGridView1.Rows[1].Cells[2].Value.ToString());
                //MessageBox.Show(dataGridView1.Rows[1].Cells[3].Value.ToString());
                //MessageBox.Show(dataGridView1.Rows[1].Cells[4].Value.ToString());
                //    tblIn.Open();
                //    using (SqlCommand command = new SqlCommand("DELETE FROM tblInImage WHERE id = '" + dataGridView1.CurrentRow.Cells[4].Value + "'", tblIn))
                //    {
                //        command.ExecuteNonQuery();
                //    }
                //    MessageBox.Show("Deleted");
                //    tblIn.Close();
                //    load();
                if (num == "1")
                {
                    viewpics v = new viewpics();
                    v.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    var data = (Byte[])(dataGridView1.CurrentRow.Cells[2].Value);
                    var stream = new MemoryStream(data);
                    v.pictureBox4.Image = Image.FromStream(stream);
                    v.num = "1";

                    v.ShowDialog();
                }
                else
                {
                    viewpics v = new viewpics();
                    v.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    var data = (Byte[])(dataGridView1.CurrentRow.Cells[2].Value);
                    var stream = new MemoryStream(data);
                    v.pictureBox4.Image = Image.FromStream(stream);
                    v.num = "2";

                    v.ShowDialog();
                }

                //    obj.loadscan();
                //}
            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (label3.Text == "select")
            {

                MessageBox.Show("Please select image...");
            }
            else
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox2.Image.Save(saveFileDialog.FileName);
                    }
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
