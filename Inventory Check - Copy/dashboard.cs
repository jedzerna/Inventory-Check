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

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;


            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;

            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            dataGridView3.ClearSelection();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
       public DataTable dt = new DataTable();
        public void load()
        {

            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                dt.Rows.Clear();
                otherDB.Open();
                string list = "Select docid from tblPending where type = 'PO' order by id DESC";
                SqlCommand command = new SqlCommand(list, otherDB);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);

                foreach (DataRow row in dt.Rows)
                {
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        String query = "SELECT * FROM tblIn WHERE Id = '" + row["docid"].ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, tblIn);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            row["ponumber"] = (rdr["ponumber"].ToString());
                            row["suppliername"] = (rdr["suppliername"].ToString());
                            row["date"] = DateTime.Parse(rdr["datetime"].ToString()).ToString("MM/dd/yyyy");
                            row["remarks"] = (rdr["additionalinfo"].ToString()); 
                            row["itemcode"] = (rdr["itemcode"].ToString());
                        }


                    }
                }
                otherDB.Close();
                dataGridView3.DataSource = dt;
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
                tblIn.Dispose();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
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
                dbDR.Dispose();
                dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView2.RowHeadersVisible = false;
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
            i.id = dataGridView2.CurrentRow.Cells["Column4"].Value.ToString();
            i.itemid = dataGridView2.CurrentRow.Cells["Column3"].Value.ToString();
            i.name = name;
            i.num = "1";
            i.ShowDialog();
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
            i.id = dataGridView3.CurrentRow.Cells["dataGridViewTextBoxColumn5"].Value.ToString();
            i.itemid = dataGridView3.CurrentRow.Cells["Column5"].Value.ToString();
            i.name = name;


            i.ShowDialog();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
