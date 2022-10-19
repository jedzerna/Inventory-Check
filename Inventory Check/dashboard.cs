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
using System.Windows.Forms.DataVisualization.Charting;

namespace Inventory_Check
{
    public partial class dashboard : Form
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
        public dashboard()
        {
            InitializeComponent();
        }
        public string name;
        private void dashboard_Load(object sender, EventArgs e)
        {
            color();
            //chart1.Series["Total DR"].Points.AddXY("Ramesh", "8000");
            //chart1.Series["Total DR"].Points.AddXY("Ankit", "7000");
            //chart1.Series["Total DR"].Points.AddXY("Gurmeet", "10000");
            //chart1.Series["Total DR"].Points.AddXY("Suresh", "8500");

            dt.Columns.Add("date", typeof(DateTime));
            dt.Columns.Add("ponumber");
            dt.Columns.Add("remarks");
            dt.Columns.Add("suppliername");
            dt.Columns.Add("itemcode");
            //chart title  
            load();
            loadsupply();
            loadsupply2();

            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
            dataGridView4.ClearSelection();

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;


            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;

            ChangeControlStyles(dataGridView3, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;


            ChangeControlStyles(dataGridView4, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView4.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView4.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;


        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);
                //this.BackColor = Color.White;

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label22.ForeColor = Color.White;
                label24.ForeColor = Color.White;
                label3.ForeColor = Color.White;


                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);
                guna2Panel3.FillColor = Color.FromArgb(34, 35, 35);
                guna2Panel4.FillColor = Color.FromArgb(34, 35, 35);

                dataGridView1.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;


                dataGridView2.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;



                dataGridView3.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.White;


                label4.ForeColor = Color.White;
                guna2ShadowPanel1.FillColor = Color.FromArgb(34, 35, 35);
                dataGridView4.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView4.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView4.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView4.RowsDefaultCellStyle.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label22.ForeColor = Color.Black;
                label24.ForeColor = Color.Black;
                label3.ForeColor = Color.White;

                label4.ForeColor = Color.White;
                guna2Panel1.FillColor = Color.FromArgb(124, 156, 68);
                guna2Panel4.FillColor = Color.FromArgb(124, 156, 68);
                guna2Panel3.FillColor = Color.FromArgb(124, 156, 68);
                guna2ShadowPanel1.FillColor = Color.FromArgb(124, 156, 68);

                dataGridView1.BackgroundColor = Color.FromArgb(124, 156, 68);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;

                dataGridView2.BackgroundColor = Color.FromArgb(124, 156, 68);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView2.RowsDefaultCellStyle.ForeColor = Color.White;

                dataGridView3.BackgroundColor = Color.FromArgb(124, 156, 68);
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.White;


                dataGridView4.BackgroundColor = Color.FromArgb(124, 156, 68);
                dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView4.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView4.RowsDefaultCellStyle.BackColor = Color.FromArgb(124, 156, 68);
                dataGridView4.RowsDefaultCellStyle.ForeColor = Color.White;
            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
       public DataTable dt = new DataTable();
        public DataTable dr = new DataTable();
        public void load()
        {

            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                dt.Rows.Clear();
                otherDB.Open();
                string list = "Select docid,type from tblPending order by id DESC";
                SqlCommand command = new SqlCommand(list, otherDB);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow row in dt.Rows)
                {
                    if (row["type"].ToString() == "PO")
                    {
                        using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                        {
                            tblIn.Open();
                            String query = "SELECT * FROM tblIn WHERE Id = '" + row["docid"].ToString() + "'";
                            SqlCommand cmd = new SqlCommand(query, tblIn);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                             
                                int a = dataGridView3.Rows.Add();
                                dataGridView3.Rows[a].Cells["aone"].Value = (rdr["ponumber"].ToString());
                                dataGridView3.Rows[a].Cells["atwo"].Value = (rdr["suppliername"].ToString());
                                dataGridView3.Rows[a].Cells["athree"].Value = DateTime.Parse(rdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                                dataGridView3.Rows[a].Cells["afour"].Value = (rdr["additionalinfo"].ToString());
                                dataGridView3.Rows[a].Cells["afive"].Value = (rdr["itemcode"].ToString());
                                dataGridView3.Rows[a].Cells["asix"].Value = (rdr["Id"].ToString());
                            }

                            tblIn.Close();
                        }
                    }
                    else
                    {
                        using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            dbDR.Open();
                            String query = "SELECT * FROM tblDR WHERE Id = '" + row["docid"].ToString() + "'";
                            SqlCommand cmd = new SqlCommand(query, dbDR);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                int a = dataGridView4.Rows.Add();
                                dataGridView4.Rows[a].Cells["one"].Value = (rdr["drnumber"].ToString());
                                dataGridView4.Rows[a].Cells["two"].Value = (rdr["projectname"].ToString());
                                dataGridView4.Rows[a].Cells["three"].Value = DateTime.Parse(rdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                                dataGridView4.Rows[a].Cells["four"].Value = (rdr["additionalinfo"].ToString());
                                dataGridView4.Rows[a].Cells["five"].Value = (rdr["itemcode"].ToString());
                                dataGridView4.Rows[a].Cells["six"].Value = (rdr["Id"].ToString());
                            }
                            dbDR.Close();
                        }
                    }
                }
                otherDB.Close();
                dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView3.RowHeadersVisible = false;
            }
        }
        public void loadsupply()
        {
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                string list = "Select top 3 ponumber,createdby,itemcode,Id from tblIn order by ID DESC";
                SqlCommand command = new SqlCommand(list, tblIn);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                tblIn.Close();
                dataGridView1.ClearSelection();
            }
        }
        public void loadsupply2()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                DataTable dt = new DataTable();
                string list = "Select top 3 drnumber,createdby,itemcode,Id from tblDR order by ID DESC";
                SqlCommand command = new SqlCommand(list, dbDR);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                dbDR.Close();
                dataGridView2.ClearSelection();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void elementHost2_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
      
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            In_supply i = new In_supply();
            i.id = dataGridView1.CurrentRow.Cells["Column2"].Value.ToString();
            i.itemid = dataGridView1.CurrentRow.Cells["Column1"].Value.ToString();
            i.FormClosed += new FormClosedEventHandler(Form2_Closed);
            i.name = name;
            i.ShowDialog();
        }
        private void Form2_Closed(object sender, FormClosedEventArgs e)
        {
            load();

        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            out_supply i = new out_supply();
            if (dataGridView2.CurrentRow.Cells["Column4"].Value.ToString() != "")
            {
                i.id = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
                i.itemid = dataGridView2.CurrentRow.Cells["Column3"].Value.ToString();
                i.name = name;
                i.num = "1";
                i.ShowDialog();
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView3_Leave(object sender, EventArgs e)
        {
            dataGridView3.ClearSelection();
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            In_supply i = new In_supply();
            i.form = "dashboard";
            i.id = dataGridView3.CurrentRow.Cells["asix"].Value.ToString();
            i.itemid = dataGridView3.CurrentRow.Cells["afive"].Value.ToString();
            i.name = name;


            i.ShowDialog();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            out_supply i = new out_supply(this);
            i.id = dataGridView4.CurrentRow.Cells["six"].Value.ToString();
            i.itemid = dataGridView4.CurrentRow.Cells["five"].Value.ToString();
            i.name = name;
            i.form = "dashboard";
            i.num = "1";
            i.ShowDialog();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
