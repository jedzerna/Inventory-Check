using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class itemadddeducthist : Form
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
        public string id;
        public itemadddeducthist()
        {
            InitializeComponent();
        }

        private void itemadddeducthist_Load(object sender, EventArgs e)
        {

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; 
            load();
        }
        public void load()
        {
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                DataTable dt = new DataTable();
                string list = "Select itemid,date,operation,product_code,description,name,remarks from tblHistory where itemid = '" + id + "' order by id desc";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView1.DataSource = dt;
                reader.Close();
                codeMaterial.Close();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
