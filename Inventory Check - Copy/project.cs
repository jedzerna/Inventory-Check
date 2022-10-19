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
    public partial class project : Form
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
        public project()
        {
            SuspendLayout();
            InitializeComponent();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        private void project_Load(object sender, EventArgs e)
        {
            load();
        }
        public void load()
        {
          
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
            DataTable dt = new DataTable();
            string list = "Select ACCTCODE,ACCTDESC,id,createdby from GLU4";
            SqlCommand command = new SqlCommand(list, otherDB);
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            dataGridView2.DataSource = dt;
            otherDB.Close();
            otherDB.Dispose();

            }
            this.dataGridView2.Sort(this.dataGridView2.Columns[0], ListSortDirection.Ascending);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            showproject s = new showproject();
            s.code = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            s.ShowDialog();
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (ACCTCODE.Checked == true)
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTCODE LIKE '{0}%'", guna2TextBox2.Text.Replace("'", "''"));
            }
            else if (ACCTDESC.Checked == true)
            {
                (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("ACCTDESC LIKE '%{0}%'", guna2TextBox2.Text.Replace("'", "''"));
            }
        }
    }
}
