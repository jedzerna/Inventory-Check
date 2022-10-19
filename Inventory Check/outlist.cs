using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class outlist : Form
    {
        public string po;
        public string num;

   
        public string name;
        public outlist()
        {
            InitializeComponent();

            pictureBox5.InitialImage = null;
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
        private void outlist_Load(object sender, EventArgs e)
        {
            color();
            SuspendLayout();
            if (num == "1")
            {
                loadprein();
            }
            else
            {
                guna2ShadowForm1.SetShadowForm(this);
                loadprein2();
            }
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;


            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            //foreach (DataGridViewRow row in dataGridView2.Rows)
            //{
            //    if (row.Cells["dataGridViewTextBoxColumn6"].Value.ToString() == "Incomplete")
            //    {
            //        row.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}
            ResumeLayout();
            if (guna2RadioButton6.Checked)
            {
                dt.DefaultView.Sort = "dateentered DESC";
            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label2.ForeColor = Color.White;
                label22.ForeColor = Color.White;
                label24.ForeColor = Color.White;
                label4.ForeColor = Color.White;

                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);

                dataGridView2.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;

                guna2TextBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox1.ForeColor = Color.White;
                guna2DateTimePicker1.FillColor = Color.FromArgb(34, 35, 35);
                guna2DateTimePicker1.ForeColor = Color.White;

                dr.ForeColor = Color.White;
                date.ForeColor = Color.White;
                supp.ForeColor = Color.White;

                guna2RadioButton1.ForeColor = Color.White;
                guna2RadioButton2.ForeColor = Color.White;
                guna2RadioButton3.ForeColor = Color.White;
                guna2RadioButton4.ForeColor = Color.White;
                guna2RadioButton5.ForeColor = Color.White;
                guna2RadioButton6.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label2.ForeColor = Color.Black;
                label22.ForeColor = Color.Black;
                label24.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;

                guna2Panel1.FillColor = Color.FromArgb(115, 1, 1);

                dataGridView2.BackgroundColor = Color.FromArgb(115, 1, 1);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(115, 1, 1);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(115, 1, 1);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;

                guna2TextBox1.FillColor = Color.FromArgb(115, 1, 1);
                guna2TextBox1.ForeColor = Color.White;
                guna2DateTimePicker1.ForeColor = Color.Black;
                guna2DateTimePicker1.FillColor = Color.White;


                dr.ForeColor = Color.Black;
                date.ForeColor = Color.Black;
                supp.ForeColor = Color.Black;

                guna2RadioButton1.ForeColor = Color.Black;
                guna2RadioButton2.ForeColor = Color.Black;
                guna2RadioButton3.ForeColor = Color.Black;
                guna2RadioButton4.ForeColor = Color.Black;
                guna2RadioButton5.ForeColor = Color.Black;
                guna2RadioButton6.ForeColor = Color.Black;
            }
        }



        public DataTable dt = new DataTable();
        public void loadprein()
        {
            dt.Rows.Clear();
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
              

                dbDR.Open();
                string list = "Select drnumber,datetime,projectcode,projectname,operation,Id,itemcode,sv,dateentered from tblDR";
                SqlCommand command = new SqlCommand(list, dbDR);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dt.DefaultView.Sort = "Id DESC";
                foreach (DataRow dr in dt.Rows)
                {
                    dr["projectname"] = dr["projectcode"].ToString() + " || " + dr["projectname"].ToString();
                }
                dt.AcceptChanges();
                dataGridView2.DataSource = dt;


                //dataGridView2.DataSource = dt;


                dbDR.Close();
                this.dataGridView2.Sort(this.dataGridView2.Columns[4], ListSortDirection.Descending);
                dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView2.RowHeadersVisible = false;
            }

        }
        public void loadprein2()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {


                dbDR.Open();
                DataTable dt = new DataTable();
                string list = "Select drnumber,datetime,projectcode,projectname,operation,Id,itemcode,dateentered from tblDR order by Id desc";
                SqlCommand command = new SqlCommand(list, dbDR);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow dr in dt.Rows)
                {
                    dr["projectname"] = dr["projectcode"].ToString() + " || " + dr["projectname"].ToString();
                }
                dt.AcceptChanges();
                dataGridView2.DataSource = dt;



                this.dataGridView2.Sort(this.dataGridView2.Columns[4], ListSortDirection.Descending);
                dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView2.RowHeadersVisible = false;
                dbDR.Close();
                dbDR.Dispose();
            }

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            out_supply i = new out_supply(this);
            i.id = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
            i.itemid = dataGridView2.CurrentRow.Cells["dataGridViewTextBoxColumn4"].Value.ToString();
            i.name = name;
            i.num = "2";
            i.ShowDialog();

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (num == "1")
            {
                if (dr.Checked == true)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("drnumber LIKE '{0}%'", guna2TextBox1.Text.Replace("'", "''"));
                }
                else if (date.Checked == true)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("datetime LIKE '{0}%'", guna2TextBox1.Text.Replace("'", "''"));
                }
                else if (supp.Checked == true)
                {
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("projectname LIKE '%{0}%'", guna2TextBox1.Text.Replace("'", "''"));
                }
            }
            else
            {
                if (dr.Checked == true)
                {
                    if (guna2TextBox1.Text == "")
                    {
                        loadprein2();
                    }
                    else
                    {
                        string rowFilter = string.Format("drnumber LIKE '{0}%'", guna2TextBox1.Text);
                        rowFilter += string.Format("AND ponumber LIKE '{0}%'", po);
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
                    }
                }
                else if (date.Checked == true)
                {
                    if (guna2TextBox1.Text == "")
                    {
                        loadprein2();
                    }
                    else
                    {
                        string rowFilter = string.Format("datetime LIKE '{0}%'", guna2TextBox1.Text);
                        rowFilter += string.Format("AND ponumber LIKE '{0}%'", po);
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
                    }
                }
                else if (supp.Checked == true)
                {
                    if (guna2TextBox1.Text == "")
                    {
                        loadprein2();
                    }
                    else
                    {
                        string rowFilter = string.Format("projectname LIKE '{0}%'", guna2TextBox1.Text);
                        rowFilter += string.Format("AND ponumber LIKE '{0}%'", po);
                        (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
                    }
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            string list = "Select drnumber,datetime,projectname,operation,Id,itemcode,sv from tblDR";
            if (guna2RadioButton1.Checked)
            {
                dt.DefaultView.Sort = "Id DESC";
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
                dt.DefaultView.Sort = "projectname ASC";
            }
        }

        private void guna2RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton4.Checked)
            {
                dt.DefaultView.Sort = "sv ASC";
            }
        }

        private void guna2RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton5.Checked)
            {
                dt.DefaultView.Sort = "operation ASC";
            }
        }

        private void dr_CheckedChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = null;
            if (dr.Checked == true)
            {
                date.Checked = false;
                supp.Checked = false;
                guna2DateTimePicker1.Visible = false;
                guna2TextBox1.Visible = true;
            }
        }

        private void date_CheckedChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = null;
            if (date.Checked == true)
            {
                dr.Checked = false;
                supp.Checked = false;
                guna2DateTimePicker1.Visible = true;
                guna2TextBox1.Visible = false;
            }
        }

        private void supp_CheckedChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = null;
            if (supp.Checked == true)
            {
                dr.Checked = false;
                date.Checked = false;
                guna2DateTimePicker1.Visible = false;
                guna2TextBox1.Visible = true;
            }
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (date.Checked == true)
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "datetime = '" + guna2DateTimePicker1.Value + "'";
            }
        }

        private void guna2RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton6.Checked)
            {
                dt.DefaultView.Sort = "dateentered DESC";
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Please wait...Press OK to continue");
            DRLdrview b = new DRLdrview();
            b.name = name;
            b.ShowDialog();
        }
    }
}
