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
    public partial class developer : Form
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
        public developer()
        {
            InitializeComponent();
        }

        private void developer_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SuspendLayout();
            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;

            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                String query = "SELECT * FROM updatestat where id = 1";
                SqlCommand cmd = new SqlCommand(query, tblSupplier);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    //guna2TextBox2.Text = dr["updatestats"].ToString();
                    guna2TextBox1.Text = dr["version"].ToString();
                }
                dr.Close();
                tblSupplier.Close();
            }
            load();
            dataGridView2.ClearSelection();
            ResumeLayout();
            Cursor.Current = Cursors.Default;

        }
        private void load()
        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("Select top 100 description,type,id from tblUpdatelist order by id desc", tblSupplier))
                    a2.Fill(dt);
                dataGridView2.DataSource = dt;
                //int ww = 0;
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
                //    }
                //    else if (item["type"].ToString() == "ttitle")
                //    {
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //    else if (item["type"].ToString() == "title")
                //    {
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //    else
                //    {
                //        int a = dataGridView2.Rows.Add();
                //        dataGridView2.Rows[a].Cells["dataGridViewTextBoxColumn2"].Value = item["description"].ToString();
                //        dataGridView2.Rows[a].Cells["Column3"].Value = item["id"].ToString();
                //    }
                //}
                tblSupplier.Close();
            }
        }
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            //guna2HtmlLabel1.Text = guna2TextBox2.Text;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            version = "";
            title = "";
            description = "";
            num = 0;
            num2 = 0;
            //DialogResult dialogResult = MessageBox.Show("Are you sure to add this update?" , "Add", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    updateupdator();
            //    getupdatedfiles();
            //    using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            //    {
            //        SqlCommand cmd = new SqlCommand("update updatestat set version=@version where id = 1", tblSupplier);

            //        tblSupplier.Open();
            //        //cmd.Parameters.AddWithValue("@updatestats", guna2TextBox2.Text);
            //        cmd.Parameters.AddWithValue("@version", guna2TextBox1.Text);
            //        cmd.ExecuteNonQuery();
            //        tblSupplier.Close();
            //    }
            //    MessageBox.Show("Update Info Added");
            //}
        }
        private void updateupdator()
        {
            string sourcePath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AppUpdator";
            string targetPath = @"\\JED-PC\Users\Public\Documents\Debug\AppUpdator";

            string fileName = "AppUpdator.exe";
            string sourceFile = System.IO.Path.Combine(sourcePath, "AppUpdator.exe");
            string destFile = System.IO.Path.Combine(targetPath, "AppUpdator.exe");
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourcePath);
            int count = Directory.GetFiles(sourcePath).Length;
            System.IO.File.Copy(sourceFile, destFile, true);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);


                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);

                }

            }
            else
            {
                MessageBox.Show("Source path does not exist!", "Error");
            }
        }
        private void getupdatedfiles()
        {

            string sourcePath = Path.GetDirectoryName(Application.ExecutablePath);
            string targetPath = @"\\JED-PC\Users\Public\Documents\Debug";

            //MessageBox.Show(targetPath);
            string fileName = "InventoryCheck.exe";
            string sourceFile = System.IO.Path.Combine(sourcePath, "InventoryCheck.exe");
            string destFile = System.IO.Path.Combine(targetPath, "InventoryCheck.exe");
            System.IO.Directory.CreateDirectory(targetPath);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sourcePath);
            int count = Directory.GetFiles(sourcePath).Length;
            System.IO.File.Copy(sourceFile, destFile, true);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);


                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);

                }

            }
            else
            {
                MessageBox.Show("Source path does not exist!", "Error");
            }
        }
        private string version = "";
        private string title = "";
        private string description = "";
        int num = 0;
        int num2 = 0;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "" && guna2TextBox1.Text == "" && guna2TextBox5.Text == "")
            {
                MessageBox.Show("Please enter all the required details");
            }
            else
            {
                if (version != guna2TextBox1.Text)
                {
                    num = 0;
                    if (version != "")
                    {
                        dataGridView1.Rows.Add();
                    }
                    int a = dataGridView1.Rows.Add();
                    version = guna2TextBox1.Text;
                    dataGridView1.Rows[a].Cells["Column1"].Value = "version";
                    dataGridView1.Rows[a].Cells["Column2"].Value = "v" + guna2TextBox1.Text.Trim();
                    dataGridView1.Rows[a].Cells["Column2"].Style.ForeColor = Color.Blue;
                    dataGridView1.Rows[a].Cells["Column2"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);

                    int d = dataGridView1.Rows.Add();
                    dataGridView1.Rows[d].Cells["Column1"].Value = "ttitle";
                    dataGridView1.Rows[d].Cells["Column2"].Value = guna2TextBox5.Text.Trim();
                    dataGridView1.Rows[d].Cells["Column2"].Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                }



                num++;
                int b = dataGridView1.Rows.Add();
                dataGridView1.Rows[b].Cells["Column1"].Value = "title";
                dataGridView1.Rows[b].Cells["Column2"].Value = "   "+num.ToString()+". " + guna2TextBox3.Text.Trim();
                dataGridView1.Rows[b].Cells["Column2"].Style.Font = new Font("Segoe UI Semibold", 10);

                if (guna2TextBox4.Text!="")
                {
                    int c = dataGridView1.Rows.Add();
                    dataGridView1.Rows[c].Cells["Column1"].Value = "desc";
                    dataGridView1.Rows[c].Cells["Column2"].Value ="      " +guna2TextBox4.Text.Trim();
                    dataGridView1.Rows[c].Cells["Column2"].Style.Font = new Font("Segoe UI", 10);
                }
                dataGridView1.ClearSelection();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to add this update?", "Add", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                updateupdator();
                getupdatedfiles();
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update updatestat set version=@version,type=@type where id = 1", tblSupplier);

                    tblSupplier.Open();
                    //cmd.Parameters.AddWithValue("@updatestats", guna2TextBox2.Text);
                    cmd.Parameters.AddWithValue("@version", guna2TextBox1.Text);
                    if (guna2CheckBox1.Checked)
                    {
                        cmd.Parameters.AddWithValue("@type", "true");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@type", "false");
                    }  
                    cmd.ExecuteNonQuery();
                    tblSupplier.Close();
                }
                using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("id");
                    dt.Columns.Add("type");
                    dt.Columns.Add("description");
                    int r = dataGridView1.Rows.Count;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["Column1"].Value == null || row.Cells["Column1"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["Column1"].Value.ToString()))
                        {

                        }
                        else
                        {
                            r--;
                            DataRow _ravi = dt.NewRow();
                            _ravi["id"] = r;
                            _ravi["type"] = row.Cells["Column1"].Value.ToString();
                            _ravi["description"] = row.Cells["Column2"].Value.ToString();
                            dt.Rows.Add(_ravi);
                        }
                    }
                    dt.DefaultView.Sort = "id ASC";

                    DataView dv = dt.DefaultView;
                    dv.Sort = "id ASC";
                    DataTable sortedDT = dv.ToTable();

                    foreach (DataRow dtrow in sortedDT.Rows)
                    {
                        tblSupplier.Open();
                        string insStmt = "insert into tblUpdatelist ([type], [description]) values" +
                            " (@type,@description)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblSupplier);
                        insCmd.Parameters.AddWithValue("@type", dtrow["type"].ToString().Trim());
                        insCmd.Parameters.AddWithValue("@description", dtrow["description"].ToString().Trim());
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblSupplier.Close();
                    }
                }
                load();
                dataGridView1.Rows.Clear();
                version = "";
                title = "";
                description = "";
                num = 0;
                num2 = 0;
                MessageBox.Show("Update Info Added");
            }


            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            if (dataGridView2.CurrentRow.Cells["Column3"].Value == null || dataGridView2.CurrentRow.Cells["Column3"].Value == DBNull.Value || String.IsNullOrWhiteSpace(dataGridView2.CurrentRow.Cells["Column3"].Value.ToString()))
            {
                DataGridViewRow dgvDelRow = dataGridView2.CurrentRow;
                dataGridView2.Rows.Remove(dgvDelRow);
            }
            else
            {

                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                    {
                        tblSupplier.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblUpdatelist WHERE id = '" + dataGridView2.CurrentRow.Cells["Column3"].Value.ToString() + "'", tblSupplier))
                        {
                            command.ExecuteNonQuery();
                        }
                        tblSupplier.Close();
                    }
                    DataGridViewRow dgvDelRow = dataGridView2.CurrentRow;
                    dataGridView2.Rows.Remove(dgvDelRow);
                }

            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
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
    }
}
