using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class showproject : Form
    {
        public string code;
        SqlDataReader rdr;
    
        public showproject()
        {
            InitializeComponent();
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

        private void showproject_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            getinfo();
            load();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView2.RowHeadersVisible = false;
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        private void getinfo()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {

                otherDB.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM GLU4 WHERE ACCTCODE = '" + code + "'";
                SqlCommand cmd = new SqlCommand(query, otherDB);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label15.Text = (rdr["ACCTCODE"].ToString());
                    label2.Text = (rdr["ACCTDESC"].ToString());
                }
                otherDB.Close();
                otherDB.Dispose();
            }
        }
        public void load()
        {

            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                DataTable dt = new DataTable();
                string list = "Select projectcode,projectname,drnumber,datetime,totalamount,operation from tblDR where projectcode = '" + code + "'";
                SqlCommand command = new SqlCommand(list, dbDR);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
                reader.Close();
                dbDR.Close();
                dbDR.Dispose();
                //(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("typeofp LIKE '{0}%'", o);
                //(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Format("typeofp LIKE '{0}%'", q);
                //string rowFilter = string.Format("typeofp LIKE '{0}%'", o);
                //rowFilter += string.Format("OR typeofp LIKE '{0}%'", q);
                //(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = rowFilter;

                decimal sum = 0.00M;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {

                    sum += Math.Round(Convert.ToDecimal(dataGridView2.Rows[i].Cells[4].Value), 2) * 1;

                    label4.Text = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");
                }
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

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
