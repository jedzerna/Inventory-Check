using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class categories : Form
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
        public categories()
        {
            InitializeComponent();
        }

        private void categories_Load(object sender, EventArgs e)
        {
            color();
            load();
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;

            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;

            ChangeControlStyles(dataGridView3, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;


        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;

                guna2CheckBox1.ForeColor = Color.White;
                guna2CheckBox2.ForeColor = Color.White;
                guna2CheckBox3.ForeColor = Color.White;

                dataGridView1.BackgroundColor = Color.FromArgb(15, 14, 15);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;

                dataGridView2.BackgroundColor = Color.FromArgb(15, 14, 15);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;

                dataGridView3.BackgroundColor = Color.FromArgb(15, 14, 15);
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(15, 14, 15);
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.White;

            }
            else
            {
                this.BackColor = Color.White;

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;

                guna2CheckBox1.ForeColor = Color.Black;
                guna2CheckBox2.ForeColor = Color.Black;
                guna2CheckBox3.ForeColor = Color.Black;


                dataGridView1.BackgroundColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;

                dataGridView2.BackgroundColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.Black;

                dataGridView3.BackgroundColor = Color.White;
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.Black;
            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        DataTable d = new DataTable();
        DataTable d11 = new DataTable();
        DataTable d22 = new DataTable();



        DataTable cd = new DataTable();
        DataTable cd11 = new DataTable();
        DataTable cd22 = new DataTable();
        private bool check = false;
        public void load()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                d.Rows.Clear();
                string Query = "SELECT value,category FROM tblCategory ORDER BY value ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                d.Load(myReader);
                otherDB.Close();
            }
            dataGridView1.DataSource = d;
            UpdateFont();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void loadcom1()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                d11.Rows.Clear();
                string Query = "SELECT subcategory,value,catval FROM tblSubCat WHERE catval= '" + dataGridView1.CurrentRow.Cells["Column1"].Value.ToString()+ "' ORDER BY value ASC";
                otherDB.Open();
                SqlCommand cmd = new SqlCommand(Query, otherDB);
                SqlDataReader myReader = cmd.ExecuteReader();
                d11.Load(myReader);
                cd11 = d11.Clone();
                cd11.Columns["subcategory"].DataType = typeof(string);
                cd11.Columns["value"].DataType = typeof(string);
                cd11.Columns["catval"].DataType = typeof(string);
                foreach (DataRow row in d11.Rows)
                {
                    cd11.ImportRow(row);
                }
                foreach (DataRow row in cd11.Rows)
                {
                    row["value"] = row["value"].ToString().PadLeft(2, '0');
                }
                otherDB.Close();
            }
            dataGridView2.DataSource = cd11; 
            UpdateFont();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void loadtypedgv()
        {
            if (dataGridView2.Rows.Count != 0)
            {
                using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
                {
                    d22.Rows.Clear();
                    string Query = "SELECT value,type FROM tblType WHERE valcat= '" + dataGridView2.CurrentRow.Cells["catval"].Value.ToString() + "' AND valsubcat= '" + dataGridView2.CurrentRow.Cells["Column3"].Value.ToString() + "' ORDER BY value ASC";
                    otherDB.Open();
                    SqlCommand cmd = new SqlCommand(Query, otherDB);
                    SqlDataReader myReader = cmd.ExecuteReader();
                    d22.Load(myReader);
                    cd22 = d22.Clone();
                    cd22.Columns["value"].DataType = typeof(string);
                    cd22.Columns["type"].DataType = typeof(string);
                    foreach (DataRow row in d22.Rows)
                    {
                        cd22.ImportRow(row);
                    }
                    foreach (DataRow row in cd22.Rows)
                    {
                        row["value"] = row["value"].ToString().PadLeft(2, '0');
                    }
                    otherDB.Close();
                }
                dataGridView3.DataSource = cd22;
                UpdateFont();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                d.DefaultView.Sort = "category ASC";
            }
            else
            {
                d.DefaultView.Sort = "value ASC";
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count != 0)
            {
                if (guna2CheckBox2.Checked)
                {
                    cd11.DefaultView.Sort = "subcategory ASC";
                }
                else
                {
                    cd11.DefaultView.Sort = "value ASC";
                }
            }
        }

        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count != 0)
            {
                if (guna2CheckBox3.Checked)
                {
                    cd22.DefaultView.Sort = "type ASC";
                }
                else
                {
                    cd22.DefaultView.Sort = "value ASC";
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            loadcom1();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            loadtypedgv();
        }

        private void dataGridView3_SizeChanged(object sender, EventArgs e)
        {
            //DataGridViewTextBoxCell tb = sender as DataGridViewTextBoxCell;
            //if (tb.Size.Height < 10) return;
            //if (tb == null) return;
            //if (tb.Value.ToString() == "") return;
            //SizeF stringSize;

            //// create a graphics object for this form
            //using (Graphics gfx = this.CreateGraphics())
            //{
            //    // Get the size given the string and the font
            //    stringSize = gfx.MeasureString(tb.Value.ToString(), tb.Style.Font);
            //    //test how many rows
            //    int rows = (int)((double)tb.Size.Height / (stringSize.Height));
            //    if (rows == 0)
            //        return;
            //    double areaAvailable = rows * stringSize.Height * tb.Size.Width;
            //    double areaRequired = stringSize.Width * stringSize.Height * 1.1;

            //    if (areaAvailable / areaRequired > 1.3)
            //    {
            //        while (areaAvailable / areaRequired > 1.3)
            //        {
            //            tb.Style.Font = new Font(tb.Style.Font.FontFamily, tb.Style.Font.Size * 1.1F);
            //            stringSize = gfx.MeasureString(tb.Value.ToString(), tb.Style.Font);
            //            areaRequired = stringSize.Width * stringSize.Height * 1.1;
            //        }
            //    }
            //    else
            //    {
            //        while (areaRequired * 1.3 > areaAvailable)
            //        {
            //            tb.Style.Font = new Font(tb.Style.Font.FontFamily, tb.Style.Font.Size / 1.1F);
            //            stringSize = gfx.MeasureString(tb.Value.ToString(), tb.Style.Font);
            //            areaRequired = stringSize.Width * stringSize.Height * 1.1;
            //        }
            //    }
            //}
        }

        private void categories_ResizeBegin(object sender, EventArgs e)
        {
            //if (this.Width >= 912)
            //{
            //    Int32 w = this.Width;
            //    Int32 fw = w + 552;
            //    Int32 fw1 = fw - 912;

            //    dataGridView3.SetBounds(fw1, 81, 332, 422);
            //    //dataGridView3.Location.X(fw);
            //}
        }

        private void dataGridView2_Resize(object sender, EventArgs e)
        {
            //if (dataGridView2.Width <= 420)
            //{
            //    int h = 29 + 12 + 246 + dataGridView2.Width;
            //    dataGridView3.SetBounds(h, 81, 332, 422);
            //}
            //else
            //{

            //    //dataGridView2.SetBounds(h, 81, 332, 422);
            //}
            
        }

        private void categories_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
            int t = this.Width / 2;
            int t1 = t - 100;
            label3.SetBounds(t1, 9, 201, 25);

            int l1 = 29 + 246 + 6;
            int w1 = this.Width - 324;
            int w2 = w1 / 2;
            dataGridView2.SetBounds(l1, 81, w2, dataGridView2.Height);
            int gl = l1 + dataGridView2.Width;
            int gl1 = gl - 134;
            guna2CheckBox2.SetBounds(gl1, 63, 134, 17);

            int dl1 = 29 + 246 + 6 + dataGridView2.Width + 6;
            int dw1 = this.Width - 324;
            int dw2 = w1 / 2;


            label2.SetBounds(dl1, 63, 42, 15);
            dataGridView3.SetBounds(dl1, 81, dw2, dataGridView3.Height);
            //int fon = 10;

            //Change cell font

            UpdateFont();
            ResumeLayout();
        }
        private void UpdateFont()
        {
            //Change cell font
            if (this.Width >= 950)
            {
                foreach (DataGridViewRow c in dataGridView1.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 12);
                }
                foreach (DataGridViewRow c in dataGridView2.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 12);
                }
                foreach (DataGridViewRow c in dataGridView3.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 12);
                }
            }
            else if (this.Width >= 1000)
            {

                foreach (DataGridViewRow c in dataGridView1.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 15);
                }
                foreach (DataGridViewRow c in dataGridView2.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 15);
                }
                foreach (DataGridViewRow c in dataGridView3.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 15);
                }
            }
            else if(this.Width <= 912)
            {
                foreach (DataGridViewRow c in dataGridView1.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 10);
                }
                foreach (DataGridViewRow c in dataGridView2.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 10);
                }
                foreach (DataGridViewRow c in dataGridView3.Rows)
                {
                    c.DefaultCellStyle.Font = new Font("Abitare Sans 350", 10);
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Hide();//Hide the 'current' form, i.e frm_form1 
                        //show another form ( frm_form2 )   
            categoryediting frm = new categoryediting();
            frm.ShowDialog();
            //Close the form.(frm_form1)
            this.Dispose();
        }
    }
}
