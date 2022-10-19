using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Inventory_Check
{
    public partial class prein : Form
    {
        //static string connectionstring = ConfigurationManager.ConnectionStrings["GLU.Properties.Settings.GLUConnectionString"].ConnectionString;
        //SqlConnection con = new SqlConnection(connectionstring);


      
        public string name;
        public prein()
        {
            InitializeComponent(); 
            //this.SetStyle(ControlStyles.DoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            //this.SetStyle(ControlStyles.Opaque, false);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(
            //         ControlStyles.OptimizedDoubleBuffer //important
            //         | ControlStyles.ResizeRedraw
            //         | ControlStyles.Selectable
            //         | ControlStyles.AllPaintingInWmPaint//important
            //         | ControlStyles.UserPaint
            //         | ControlStyles.SupportsTransparentBackColor,
            //         true);

            //this.DoubleBuffered = true;
            pictureBox5.InitialImage = null;
        }

        private void prein_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Sorting", typeof(string));
            //guna2ComboBox2.Text = "P.O Number";
            SuspendLayout();
            guna2RadioButton1.Checked = true;
            //d.panel3.Height = base.Height;

            loadprein();
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;

            //textBox2.BackColor = Color.FromArgb(169, 169, 169);

            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResizeRedraw = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
            ResumeLayout();

        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
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
        DataTable dt = new DataTable();
        public void loadprein()
        {
            dt.Rows.Clear();
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {

                //,sortid
                tblIn.Open();
                string list = "Select ponumber,datetime,suppliername,operation,Id,itemcode,si,vp,carno from tblIn";
                SqlCommand command = new SqlCommand(list, tblIn);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                tblIn.Close();
                tblIn.Dispose();
                dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView2.RowHeadersVisible = false;
            }
            //dataGridView2.Rows[0].Cells["dataGridViewTextBoxColumn1"].Value = "0";
            //MessageBox.Show(Regex.Replace(dataGridView2.Rows[0].Cells["dataGridViewTextBoxColumn1"].Value.ToString(), @"[a-zA-Z]+", ""));
            //foreach (DataGridViewRow row in dataGridView2.Rows)
            //{
            //    //MessageBox.Show(row.Cells["dataGridViewTextBoxColumn1"].Value.ToString());
            //    string w = Regex.Replace(row.Cells["Column1"].Value.ToString(), @"[a-zA-Z]+", "");
            //    row.Cells["Column1"].Value = w;
            //}
            foreach (DataRow row in dt.Rows)
            {
                var input = row["ponumber"].ToString();
                var result = new string(input.Where(c => char.IsDigit(c)).ToArray());
                row["Sorting"] = result;

            }
            dt.DefaultView.Sort = "Sorting DESC";
            dataGridView2.DataSource = dt;
            //this.dataGridView2.Sort(this.dataGridView2.Columns["Column1"], ListSortDirection.Descending);
        }
        //public byte[] imageToByteArray(System.Drawing.Image imageIn)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        //    return ms.ToArray();
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            In_supply i = new In_supply();
            i.form = "prein";
            i.id = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
            i.itemid = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn4"].Value.ToString();
            i.name = name;


            i.ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (po.Checked == true)
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("ponumber LIKE '{0}%'", guna2TextBox1.Text.Replace("'", "''"));
            }
            else if (supp.Checked == true)
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("suppliername LIKE '{0}%'", guna2TextBox1.Text.Replace("'", "''"));
            }

            //String searchValue = guna2TextBox1.Text.Replace("'", "''");
            //int rowIndex = -1;
            //foreach (DataGridViewRow row in dataGridView2.Rows)
            //{
            //    if (row.Cells[0].Value.ToString().Equals(searchValue))
            //    {
            //        rowIndex = row.Index;
            //        break;
            //    }
            //}
        }

        private void label22_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            string list = "Select ponumber,datetime,suppliername,operation,Id,itemcode,si,vp,carno from tblIn";

            if (guna2RadioButton1.Checked)
            {
                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "Sorting DESC";
                }
            }
        }

        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton2.Checked)
            {
                dt.DefaultView.Sort = "datetime DESC";
            }
        }

        private void guna2RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton3.Checked)
            {
                dt.DefaultView.Sort = "suppliername ASC";
            }
        }

        private void guna2RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton4.Checked)
            {
                dt.DefaultView.Sort = "operation DESC";
            }
        }

        private void guna2RadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton8.Checked)
            {
                dt.DefaultView.Sort = "si ASC";
            }
        }

        private void guna2RadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton7.Checked)
            {
                dt.DefaultView.Sort = "vp ASC";
            }
        }

        private void guna2RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton6.Checked)
            {
                dt.DefaultView.Sort = "carno ASC";
            }
        }

        private void prein_Resize(object sender, EventArgs e)
        {
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        //public IEnumerable<Control> GetAll(Control control, Type type = null)
        //{
        //    var controls = control.Controls.Cast<Control>();
        //    check the all value, if true then get all the controls
        //    otherwise get the controls of the specified type
        //    if (type == null)
        //        return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls);
        //    else
        //        return controls.SelectMany(ctrl => GetAll(ctrl, type)).Concat(controls).Where(c => c.GetType() == type);
        //}
        private void prein_SizeChanged(object sender, EventArgs e)
        {
           
            //this.SetStyle(ControlStyles.DoubleBuffer, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            //this.SetStyle(ControlStyles.Opaque, false);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.ResizeRedraw, true);
            //SetStyle(
            //         ControlStyles.OptimizedDoubleBuffer //important
            //         | ControlStyles.ResizeRedraw
            //         | ControlStyles.Selectable
            //         | ControlStyles.AllPaintingInWmPaint//important
            //         | ControlStyles.UserPaint
            //         | ControlStyles.SupportsTransparentBackColor,
            //         true);

            //this.DoubleBuffered = true;
        }

        private void prein_StyleChanged(object sender, EventArgs e)
        {
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void prein_DockChanged(object sender, EventArgs e)
        {

        }

        private void prein_ResizeBegin(object sender, EventArgs e)
        {
            //ToggleAntiFlicker(true);

        }
        //bool bEnableAntiFlicker = true;
        //int intOriginalExStyle = -1;

        private void prein_ResizeEnd(object sender, EventArgs e)
        {
            //ToggleAntiFlicker(false);

        }
        private void ToggleAntiFlicker(bool Enable)
        {
            //bEnableAntiFlicker = Enable;
            //this.MaximizeBox = true;
        }

        private void po_CheckedChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = null;
            //loadprein();
            if (po.Checked == true)
            {
                guna2TextBox1.Visible = true;
                date.Checked = false;
                supp.Checked = false;
                guna2DateTimePicker1.Visible = false;
                guna2TextBox1.Focus();
            }
        }

        private void date_CheckedChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = null;
            //loadprein();
            if (date.Checked == true)
            {
                po.Checked = false;
                supp.Checked = false;
                guna2DateTimePicker1.Visible = true;
                guna2TextBox1.Visible = false;
            }
        }

        private void supp_CheckedChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = null;
            //loadprein();
            if (supp.Checked == true)
            {
                po.Checked = false;
                date.Checked = false;
                guna2DateTimePicker1.Visible = false;
                guna2TextBox1.Visible = true;
                guna2TextBox1.Focus();

            }
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {


           if (date.Checked == true)
            {
                //string[] list = guna2TextBox1.Text.Split('/');
                //DateTime date = new DateTime(Convert.ToInt32(list[2]), Convert.ToInt32(list[1]), Convert.ToInt32(list[0]));

                //DataView dv = new DataView(dt);
                //dv.RowFilter = "Datum = #" + date + "#";
                //dv.RowStateFilter = DataViewRowState.ModifiedCurrent;
                //dv.Sort = "datetime DESC";
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter ="datetime = '"+ guna2DateTimePicker1.Value + "'" ;
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
          
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        //protected override CreateParams CreateParams
        //{
        //}
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //}
        //public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        //{
        //}
    }
}
