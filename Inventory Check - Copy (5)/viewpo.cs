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
    public partial class viewpo : Form
    {
        public string num;
        static string connectionstring;
        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
  (
      int nLeftRect, // X-coordinate of upper-left corner or padding at start
      int nTopRect,// Y-coordinate of upper-left corner or padding at the top of the textbox
      int nRightRect, // X-coordinate of lower-right corner or Width of the object
      int nBottomRect,// Y-coordinate of lower-right corner or Height of the object
                      //RADIUS, how round do you want it to be?
      int nheightRect, //height of ellipse 
      int nweightRect //width of ellipse
  );
        public viewpo()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            pictureBox4.InitialImage = null;
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
        private void viewpo_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            loadprein();
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            ResumeLayout();
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void loadprein()
        {
            connectionstring = ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString;
            SqlConnection tblIn = new SqlConnection(connectionstring);
            string c = "Completed";
            tblIn.Open();
            DataTable dt = new DataTable();
            string list = "Select ponumber,datetime,suppliername,Id from tblIn where operation = '"+ c +"'";
            SqlCommand command = new SqlCommand(list, tblIn);
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            dataGridView1.DataSource = dt;
            tblIn.Close();
            tblIn.Dispose();
            this.dataGridView1.Sort(this.dataGridView1.Columns[1], ListSortDirection.Descending);


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

        out_supply obj2 = (out_supply)Application.OpenForms["out_supply"];
        DRForm obj1 = (DRForm)Application.OpenForms["DRForm"];
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (num == "2")
            {
                //obj2.guna2TextBox4.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                //obj2.guna2TextBox5.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                //obj2.idpo = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }
            else if (num == "1")
            {
                //obj1.textBox6.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                //obj1.textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();


            }
            this.Close();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {

            this.Close();
        }
    }
}
