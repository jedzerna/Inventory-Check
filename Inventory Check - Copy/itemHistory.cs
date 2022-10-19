using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class itemHistory : Form
    {
        public string id;
     
        public itemHistory()
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
        private void itemHistory_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            load();
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
           
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
            DataTable dt = new DataTable();
            string list = "Select product_code,mfg_code,description,stocksleft,cost,selling,type,unit,date,modifiedby,ID from codeMaterialHistory where product_code = '" + id + "'";
            SqlCommand command = new SqlCommand(list, codeMaterial);
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            dataGridView1.DataSource = dt;
            codeMaterial.Close();
            codeMaterial.Dispose();

            }

            this.dataGridView1.Sort(this.dataGridView1.Columns[10], ListSortDirection.Descending);
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
