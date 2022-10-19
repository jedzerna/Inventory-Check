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
    public partial class drhistory : Form
    {
    
        public string id;
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
        public drhistory()
        {
            InitializeComponent();
            pictureBox5.InitialImage = null;
        }

        private void drhistory_Load(object sender, EventArgs e)
        {
            color();
            SuspendLayout();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            load(); 
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //load();
            ResumeLayout();
        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label22.ForeColor = Color.White;
                label24.ForeColor = Color.White;



                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);


                dataGridView1.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;

            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label22.ForeColor = Color.Black;
                label24.ForeColor = Color.Black;


                guna2Panel1.FillColor = Color.FromArgb(115, 1, 1);

                dataGridView1.BackgroundColor = Color.FromArgb(115, 1, 1);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(115, 1, 1);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(115, 1, 1);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;

            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {
           
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
            DataTable dt = new DataTable();
            string list = "Select * from tblDRHistory where id = '" + id + "'";
            SqlCommand command = new SqlCommand(list, dbDR);
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            dataGridView1.DataSource = dt;
            dbDR.Close();
            dbDR.Dispose();
            this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Descending);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
