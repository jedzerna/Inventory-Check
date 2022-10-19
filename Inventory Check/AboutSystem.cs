using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class AboutSystem : Form
    {
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
        public AboutSystem()
        {
            InitializeComponent();
        }

        private void AboutSystem_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SuspendLayout();
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            string latest = "";
            string version = "";

            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                String query = "SELECT * FROM updatestat where id = 1";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    latest = dr["version"].ToString();
                }
                dr.Close();
                tblSupplier.Close();
            }

            try
            {
                string fileName1 = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\InventoryCheck.Vsn";

                version = "";
                if (File.Exists(fileName1))
                {
                    // Read entire text file content in one string    
                    string text = File.ReadAllText(fileName1);
                    //MessageBox.Show(text.ToString().Trim());
                    version = text.ToString().Trim();
                }

                if (latest != version) 
                {
                    MessageBox.Show("Please update the system to avoid future errors.");
                    viewupdateonfo a = new viewupdateonfo();
                    a.ShowDialog();
                }
                label4.Text = version;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            load();
            dataGridView2.ClearSelection();
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void load()
        {
            //dataGridView2.Rows.Clear();
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("Select top 100 type,description,id from tblUpdatelist order by id desc", tblSupplier))
                    a2.Fill(dt);
                int ww = 0;
                dataGridView2.DataSource = dt;
                //foreach (DataRow item in dt.Rows)
                //{
                //    if (item["type"].ToString() == "version")
                //    {
                //        ww++;
                //        if (ww > 1)
                //        {
                //            dataGridView2.Rows.Add();
                //        }
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Style.ForeColor = Color.Blue;
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //    else if (item["type"].ToString() == "ttitle")
                //    {
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //    else if (item["type"].ToString() == "title")
                //    {
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI Semibold", 10);
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //    else
                //    {
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI", 10);
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //}
                tblSupplier.Close();
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (dataGridView2.Columns[e.ColumnIndex].Name == "dataGridViewTextBoxColumn1")
            //{
            //    if (e.Value.ToString() == "version")
            //    {

            //        if (dataGridView2.Columns[e.ColumnIndex].Name == "dataGridViewTextBoxColumn2")
            //        {

            //            e.CellStyle.ForeColor = Color.Blue;
            //            e.CellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            //        }

            //    }
            //    else
            //    {
            //        e.CellStyle.ForeColor = Color.FromArgb(207, 123, 122);
            //    }
            //}
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == "version")
                {
                    row.Cells["dataGridViewTextBoxColumn2"].Style.ForeColor = Color.Blue;
                    row.Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                }
                else if (row.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == "ttitle")
                {
                    row.Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    row.Cells["dataGridViewTextBoxColumn2"].Style.ForeColor = Color.Black;
                }
                else if (row.Cells["dataGridViewTextBoxColumn1"].Value.ToString() == "title")
                {
                    row.Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI Semibold", 10);
                    row.Cells["dataGridViewTextBoxColumn2"].Style.ForeColor = Color.Black;
                }
                else
                {
                    row.Cells["dataGridViewTextBoxColumn2"].Style.Font = new Font("Segoe UI", 10);
                    row.Cells["dataGridViewTextBoxColumn2"].Style.ForeColor = Color.Black;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            creatorPage a = new creatorPage();
            a.ShowDialog();
        }
    }
}
