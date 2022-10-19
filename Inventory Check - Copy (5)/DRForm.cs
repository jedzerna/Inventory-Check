using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class DRForm : Form
    {
    
        private string randomitemcode;
        private string datenow;
        private string operation = "Incomplete";
        public string createdby;
        public string username;
        public string id;

        public DRForm()
        {
            InitializeComponent();
            pictureBox11.InitialImage = null;
            pictureBox15.InitialImage = null;
            pictureBox16.InitialImage = null;
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
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public string idpo;
        private void DRForm_Load(object sender, EventArgs e)
        {
            color();
            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();

            DateTime d = DateTime.Now;
            maskedTextBox1.Text = d.ToString("MM/dd/yyyy");
            dataGridView1.RowHeadersVisible = false;
            loadall();
            random1();
            checkicode(); 
            guna2ComboBox1.Text = "Item Name";
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;

            Cursor.Current = Cursors.Default;

            guna2TextBox3.Focus();
            ResumeLayout();

        }
        private void color()
        {
            if (Properties.Settings.Default.color == "true")
            {
                this.BackColor = Color.FromArgb(15, 14, 15);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                label12.ForeColor = Color.FromArgb(255, 128, 128);
                label13.ForeColor = Color.White;
                label14.ForeColor = Color.White;
                label15.ForeColor = Color.White;
                label16.ForeColor = Color.White;
                label17.ForeColor = Color.White;
                label18.ForeColor = Color.White;
                label19.ForeColor = Color.White;
                label20.ForeColor = Color.White;
                label21.ForeColor = Color.White;
                label22.ForeColor = Color.White;
                label23.ForeColor = Color.White;

                guna2Panel1.FillColor = Color.FromArgb(34, 35, 35);
                guna2Panel2.FillColor = Color.FromArgb(34, 35, 35);
                guna2Panel3.FillColor = Color.FromArgb(34, 35, 35);


                guna2ComboBox3.FillColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox3.ForeColor = Color.White;
                guna2ComboBox3.ItemsAppearance.BackColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox3.ItemsAppearance.ForeColor = Color.White;


                dataGridView1.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;

                dataGridView3.BackgroundColor = Color.FromArgb(34, 35, 35);
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 35, 35);
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.White;


                maskedTextBox1.BackColor = Color.FromArgb(34, 35, 35);
                maskedTextBox1.ForeColor = Color.White;

                guna2TextBox2.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox2.ForeColor = Color.White;
                guna2TextBox3.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox3.ForeColor = Color.White;
                guna2TextBox4.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox4.ForeColor = Color.White;
                guna2TextBox5.FillColor = Color.FromArgb(34, 35, 35);
                guna2TextBox5.ForeColor = Color.White;


                guna2ComboBox1.FillColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox1.ForeColor = Color.White;
                guna2ComboBox1.ItemsAppearance.BackColor = Color.FromArgb(34, 35, 35);
                guna2ComboBox1.ItemsAppearance.ForeColor = Color.White;

            }
            else
            {
                this.BackColor = Color.FromArgb(243, 243, 243);

                label1.ForeColor = Color.Black;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                label12.ForeColor = Color.FromArgb(255, 128, 128);
                label13.ForeColor = Color.Black;
                label14.ForeColor = Color.Black;
                label15.ForeColor = Color.Black;
                label16.ForeColor = Color.Black;
                label17.ForeColor = Color.Black;
                label18.ForeColor = Color.Black;
                label19.ForeColor = Color.Black;
                label20.ForeColor = Color.Black;
                label21.ForeColor = Color.Black;
                label22.ForeColor = Color.Black;
                label23.ForeColor = Color.Black;

                guna2Panel1.FillColor = Color.White;
                guna2Panel2.FillColor = Color.White;
                guna2Panel3.FillColor = Color.White;


                guna2ComboBox3.FillColor = Color.White;
                guna2ComboBox3.ForeColor = Color.Black;
                guna2ComboBox3.ItemsAppearance.BackColor = Color.White;
                guna2ComboBox3.ItemsAppearance.ForeColor = Color.Black;

                dataGridView1.BackgroundColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;



                dataGridView3.BackgroundColor = Color.White;
                dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView3.RowsDefaultCellStyle.ForeColor = Color.Black;


                maskedTextBox1.BackColor = Color.White;
                maskedTextBox1.ForeColor = Color.Black;

                guna2TextBox2.FillColor = Color.White;
                guna2TextBox2.ForeColor = Color.Black;
                guna2TextBox3.FillColor = Color.White;
                guna2TextBox3.ForeColor = Color.Black;
                guna2TextBox4.FillColor = Color.White;
                guna2TextBox4.ForeColor = Color.Black;
                guna2TextBox5.FillColor = Color.White;
                guna2TextBox5.ForeColor = Color.Black;


                guna2ComboBox1.FillColor = Color.White;
                guna2ComboBox1.ForeColor = Color.Black;
                guna2ComboBox1.ItemsAppearance.BackColor = Color.White;
                guna2ComboBox1.ItemsAppearance.ForeColor = Color.Black;

            }
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void checkicode()
        {
          
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblIn] WHERE ([itemcode] = @itemcode)", tblIn);
            check_User_Name.Parameters.AddWithValue("@itemcode", randomitemcode);
            int UserExist = (int)check_User_Name.ExecuteScalar();
            if (UserExist > 0)
            {
                MessageBox.Show("Sorry for the inconvenience please refresh the page because the system having trouble getting data from the database...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    randomitemcode = "";
            }
            tblIn.Close();
            tblIn.Dispose();

            }
        }
   
        public void loadall()
        {
           
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                DataTable dt = new DataTable();
                string list = "Select product_code,mfg_code,description,ID,stocksleft,cost,selling,dept,unit from codeMaterial order by ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                codeMaterial.Close();
                codeMaterial.Dispose();
                dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView3.RowHeadersVisible = false;
            }
        }
        public void random1()
        {
            Random rnd2 = new Random();
            int month1 = rnd2.Next(1, 1000000);  // creates a number between 1 and 12
            int dice1 = rnd2.Next(1, 1000000);   // creates a number between 1 and 6
            int card1 = rnd2.Next(1, 10000000);
            randomitemcode = month1.ToString() + dice1.ToString() + card1.ToString();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Minimized;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
      
         private void save()
            {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string insStmt = "insert into tblDR ([drnumber], [additionalinfo], [itemcode], [projectcode], [projectname], [datetime], [operation], [qty], [totalitems], [totalamount], [createdby], [sv]) values" +
                 " (@drnumber,@additionalinfo,@itemcode,@projectcode,@projectname,@datetime,@operation,@qty,@totalitems,@totalamount,@createdby,@sv)";
                SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                insCmd.Parameters.AddWithValue("@drnumber", guna2TextBox3.Text);
                insCmd.Parameters.AddWithValue("@additionalinfo", guna2TextBox4.Text);
                insCmd.Parameters.AddWithValue("@itemcode", randomitemcode);
                insCmd.Parameters.AddWithValue("@projectcode", guna2TextBox2.Text);
                insCmd.Parameters.AddWithValue("@projectname", label12.Text);
                insCmd.Parameters.AddWithValue("@datetime", DateTime.Parse(maskedTextBox1.Text));
                insCmd.Parameters.AddWithValue("@operation", operation);
                insCmd.Parameters.AddWithValue("@qty", label17.Text);
                insCmd.Parameters.AddWithValue("@totalitems", label18.Text);
                insCmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(label1.Text));
                insCmd.Parameters.AddWithValue("@createdby", createdby);
                insCmd.Parameters.AddWithValue("@sv", guna2ComboBox3.Text);
                int affectedRows = insCmd.ExecuteNonQuery();
                dbDR.Close();
                dbDR.Dispose();
            }

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string insStmt2 = "insert into tblDRitemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[stored],[total],[createdby],[projectcode],[projectname],[sv" +
                        "]) values" +
                                  " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@stored,@total,@createdby,@projectcode,@projectname,@sv)";

                    SqlCommand insCmd2 = new SqlCommand(insStmt2, itemCode);

                    insCmd2.Parameters.AddWithValue("@productcode", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@mfgcode", dataGridView1.Rows[i].Cells[2].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@description", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@unit", dataGridView1.Rows[i].Cells[6].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@qty", dataGridView1.Rows[i].Cells[5].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@icode", randomitemcode);
                    insCmd2.Parameters.AddWithValue("@iitem", dataGridView1.Rows[i].Cells[4].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@stocksleft", dataGridView1.Rows[i].Cells[7].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@cost", dataGridView1.Rows[i].Cells[8].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@selling", dataGridView1.Rows[i].Cells[9].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@stored", dataGridView1.Rows[i].Cells[10].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@total", dataGridView1.Rows[i].Cells[11].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@createdby", createdby);
                    insCmd2.Parameters.AddWithValue("@projectcode", guna2TextBox2.Text);
                    insCmd2.Parameters.AddWithValue("@projectname", label12.Text);
                    insCmd2.Parameters.AddWithValue("@drnumber", guna2TextBox3.Text);
                    insCmd2.Parameters.AddWithValue("@sv", guna2ComboBox3.Text); 
                    insCmd2.ExecuteNonQuery();
                }
                itemCode.Close();
                itemCode.Dispose();
            }
                //comboBox1.Text = "";
                dataGridView1.Rows.Clear();
            guna2TextBox3.Text = "";
            guna2TextBox2.Text = "";
            guna2TextBox4.Text = "";
            label17.Text = "0";
                label18.Text = "0";
                label1.Text = "0";
            loadall();
            random1();
            checkicode();
            MessageBox.Show("D.R. Saved." + Environment.NewLine + "" + Environment.NewLine + "Note: This D.R hasn't change to the database, to change it just go to D.R History, select the D.R number and click Complete D.R to affect the changes.","Successfully",MessageBoxButtons.OK,MessageBoxIcon.Information);
           
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            datenow = d.ToString("MM/dd/yyyy");
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataTable dt = dataGridView3.DataSource as DataTable;
                DataRow row = dt.NewRow();
                row[0] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn14"].Value.ToString();
                row[1] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn15"].Value.ToString();
                row[2] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn16"].Value.ToString();
                row[3] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn17"].Value.ToString();
                row[4] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn20"].Value.ToString();
                row[5] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn21"].Value.ToString();
                row[6] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn22"].Value.ToString();
                row[7] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn23"].Value.ToString();
                row[8] = dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn19"].Value.ToString();
                dt.Rows.Add(row);
                DataGridViewRow dgvDelRow = dataGridView1.CurrentRow;
                dataGridView1.Rows.Remove(dgvDelRow);
                dt.DefaultView.Sort = "ID DESC";
                if (dataGridView1.Rows.Count > 0)
                {
                    int b = dataGridView1.Rows.Count;
                    label18.Text = b.ToString();

                    decimal sum = 0.00M;
                    decimal sum3 = 0.00M;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        sum += Math.Round(Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value), 2) * 1;

                        label17.Text = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");

                        sum3 += Math.Round(Convert.ToDecimal(dataGridView1.Rows[i].Cells[11].Value), 2) * 1;

                        label1.Text = Math.Round((decimal)Convert.ToDecimal(sum3), 2).ToString("N2");
                    }
                }
                else
                {
                    label18.Text = "0";
                    label17.Text = "0";
                    label1.Text = "0";
                }





            }




        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
            int n = dataGridView1.Rows.Add();

            //int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn14"].Value = dataGridView3.CurrentRow.Cells[0].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn15"].Value = dataGridView3.CurrentRow.Cells[1].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn16"].Value = dataGridView3.CurrentRow.Cells[2].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn17"].Value = dataGridView3.CurrentRow.Cells[3].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn18"].Value = "0.00";
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                DataTable dt = new DataTable();
                String query = "SELECT stocksleft FROM codeMaterial WHERE ID = '" + dataGridView3.CurrentRow.Cells[3].Value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn20"].Value = Math.Round(Convert.ToDecimal(rdr["stocksleft"]), 2).ToString();
                }
                codeMaterial.Close();
            }
            //dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn20"].Value = dataGridView3.CurrentRow.Cells[4].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn21"].Value = dataGridView3.CurrentRow.Cells[5].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn22"].Value = dataGridView3.CurrentRow.Cells[6].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn23"].Value = dataGridView3.CurrentRow.Cells[7].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn19"].Value = dataGridView3.CurrentRow.Cells[8].Value;
            dataGridView1.Rows[n].Cells["dataGridViewTextBoxColumn24"].Value = "0.00";
            dataGridView1.Rows[n].Cells["Column1"].Value = "0.00";

            //dataGridView1.Rows[n].Cells[1].Value = dataGridView3.CurrentRow.Cells[0].Value;
            //dataGridView1.Rows[n].Cells[2].Value = dataGridView3.CurrentRow.Cells[1].Value;
            //dataGridView1.Rows[n].Cells[3].Value = dataGridView3.CurrentRow.Cells[3].Value;
            //dataGridView1.Rows[n].Cells[4].Value = dataGridView3.CurrentRow.Cells[4].Value;
            //dataGridView1.Rows[n].Cells[5].Value = "0";
            //dataGridView1.Rows[n].Cells[6].Value = dataGridView3.CurrentRow.Cells[8].Value;
            //dataGridView1.Rows[n].Cells[7].Value = dataGridView3.CurrentRow.Cells[2].Value;
            //dataGridView1.Rows[n].Cells[8].Value = dataGridView3.CurrentRow.Cells[5].Value;
            //dataGridView1.Rows[n].Cells[9].Value = dataGridView3.CurrentRow.Cells[6].Value;
            //dataGridView1.Rows[n].Cells[10].Value = dataGridView3.CurrentRow.Cells[7].Value;
            //dataGridView1.Rows[n].Cells[11].Value = "0";
            dataGridView3.Rows.Remove(dataGridView3.CurrentRow);
            if (dataGridView1.Rows.Count > 0)
            {
                int b = dataGridView1.Rows.Count;
                label18.Text = b.ToString();
            }
            else
            {
                label18.Text = "0";
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[5].Value != null)
                {
                    decimal var1 = 0.00M;
                    var1 = decimal.Parse(e.Value.ToString());
                    e.Value = var1.ToString("N2");
                }
            }
            if (e.ColumnIndex == 11 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[11].Value != null)
                {
                    decimal var1 = 0.00M;
                    var1 = decimal.Parse(e.Value.ToString());
                    e.Value = var1.ToString("N2");
                }
            }
            if (e.ColumnIndex == 9 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[9].Value != null)
                {
                    decimal var1 = 0.00M;
                    var1 = decimal.Parse(e.Value.ToString());
                    e.Value = var1.ToString("N2");
                }
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
            if (dataGridView1.CurrentCell.ColumnIndex == 9)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar)
     && !char.IsDigit(e.KeyChar)
     && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Rows.Count >= 1)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[9].Value == null || row.Cells[9].Value.ToString() == "")
                    {
                        row.Cells[9].Value = "0.00";
                    }
                }
                //int sum = 0;
                //for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                //{
                //    sum+=Convert.ToInt32(dataGridView2.Rows[i].Cells[5].Value);
                //    label17.Text = sum.ToString();
                //}

                decimal sum = 0.00M;
                decimal sum3 = 0.00M;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);

                    label17.Text = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");

                    sum3 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[11].Value);

                    label1.Text = Math.Round((decimal)Convert.ToDecimal(sum3), 2).ToString("N2");
                }

                decimal sum2 = 0.00M;


                sum2 += Math.Round(Convert.ToDecimal(dataGridView1.CurrentRow.Cells[5].Value), 2) * Math.Round(Convert.ToDecimal(dataGridView1.CurrentRow.Cells[9].Value), 2);

                dataGridView1.CurrentRow.Cells[11].Value = Math.Round((decimal)Convert.ToDecimal(sum2), 2).ToString("N2");

                decimal sum4 = 0.00M;
                sum4 += Math.Round(Convert.ToDecimal(dataGridView1.CurrentRow.Cells[5].Value), 2);

                dataGridView1.CurrentRow.Cells[5].Value = Math.Round((decimal)Convert.ToDecimal(sum4), 2).ToString("N2");

                decimal sum5 = 0.00M;

                if (dataGridView1.CurrentRow.Cells[5].Value.ToString() != "")
                {

                    sum5 += Math.Round(Convert.ToDecimal(dataGridView1.CurrentRow.Cells[7].Value), 2) - Math.Round(Convert.ToDecimal(dataGridView1.CurrentRow.Cells[5].Value), 2);
                    if (sum5 >= 0)
                    {
                        dataGridView1.CurrentRow.Cells[12].Value = Math.Round((decimal)Convert.ToDecimal(sum5), 2).ToString("N2");
                    }
                    else 
                    {
                        dataGridView1.CurrentRow.Cells[12].Value = "Out of Stocks";
                        dataGridView1.CurrentRow.Cells[12].Style.ForeColor = Color.LightCoral;
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        SqlDataReader rdr;
        private void getprojectcode()
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                DataTable dt = new DataTable();
                String query = "SELECT ACCTDESC FROM GLU4 WHERE ACCTCODE = '" + guna2TextBox2.Text.Replace("'", "''") + "'";
                SqlCommand cmd = new SqlCommand(query, otherDB);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (Properties.Settings.Default.color == "true")
                    {
                        label12.Text = (rdr["ACCTDESC"].ToString());
                        label12.ForeColor = Color.White;
                    }
                    else
                    {
                        label12.Text = (rdr["ACCTDESC"].ToString());
                        label12.ForeColor = Color.Black;
                    }
                }
                else
                {
                    label12.Text = "No Project Code Found";
                    label12.ForeColor = Color.FromArgb(255, 128, 128);
                }
                otherDB.Close();
                otherDB.Dispose();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label24_Click(object sender, EventArgs e)
        {
            viewpo v = new viewpo();
            v.num = "1";
            v.ShowDialog();
        }

        public string name
        {
            get { return label12.Text; }
            set { label12.Text = value; }
        }
        public string code
        {
            get { return guna2TextBox2.Text; }
            set { guna2TextBox2.Text = value; }
        }
        private void label13_Click(object sender, EventArgs e)
        {
            procode p = new procode(this);
            p.createdby = createdby;
            p.num = "2";
            p.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
          
                
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void label26_Click(object sender, EventArgs e)
        {
         
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
      
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text == "")
            {
                label12.Text = "";
                label12.ForeColor = Color.FromArgb(255, 128, 128);
            }
            else
            {
                getprojectcode();
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text != "")
            {
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE ([drnumber] = @drnumber)", dbDR);
                    check_User_Name.Parameters.AddWithValue("@drnumber", guna2TextBox3.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        label23.ForeColor = Color.FromArgb(255, 128, 128);
                        label23.Text = "D.R. Number Exist";
                        label23.Visible = true;
                    }
                    else
                    {
                        label23.Visible = true;
                        label23.Text = "D.R. Number Available";
                        label23.ForeColor = Color.LimeGreen;
                    }
                    dbDR.Close();
                }
            }
        }
        private void getlatest()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                var empcon = new SqlCommand("SELECT max(Id) FROM [tblDR]", dbDR);

                dbDR.Open();
                Int32 max = (Int32)empcon.ExecuteScalar();
                int getid = max;
                SqlDataReader rdr;
                DataTable dt = new DataTable();
                String query = "SELECT Id,itemcode FROM tblDR WHERE Id = '" + max.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, dbDR);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    string getID = (rdr["Id"].ToString());
                    string getItemCode = (rdr["itemcode"].ToString());

                    DialogResult dialogResult = MessageBox.Show("Would you like to open it now?", "Open", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        out_supply i = new out_supply();
                        i.id = getID;
                        i.itemid = getItemCode;
                        i.name = createdby;
                        i.num = "1";
                        i.ShowDialog();
                    }
                }
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor; 
            DateTime temp;
            if (DateTime.TryParse(maskedTextBox1.Text, out temp))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Invalid Date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DateTime d = DateTime.Now;
                maskedTextBox1.Text = d.ToString("MM/dd/yyyy");
                return;
            }
            if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("Please select an item..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (guna2TextBox2.Text == "" || label12.Text == "No Project Code Found")
            {
                MessageBox.Show("Please enter valid project code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (guna2ComboBox3.Text == "")
            {
                MessageBox.Show("Please select sales voucher", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (guna2TextBox3.Text != "")
                {

                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        SqlCommand check_User_Name2 = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE ([drnumber] = @drnumber)", dbDR);
                        check_User_Name2.Parameters.AddWithValue("@drnumber", guna2TextBox3.Text);
                        int UserExist2 = (int)check_User_Name2.ExecuteScalar();

                        if (UserExist2 > 0 && guna2TextBox3.Text != "")
                        {

                            label23.ForeColor = Color.FromArgb(255, 128, 128);
                            label23.Text = "D.R. Number Exist";
                            label23.Visible = true;
                        }
                        else
                        {

                            label23.Visible = true;
                            label23.Text = "D.R. Number Available";
                            label23.ForeColor = Color.LimeGreen;

                            using (SqlConnection dbDR2 = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                dbDR2.Open();
                                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE ([drnumber] = @drnumber)", dbDR2);
                                check_User_Name.Parameters.AddWithValue("@drnumber", guna2TextBox3.Text);
                                int UserExist = (int)check_User_Name.ExecuteScalar();

                                if (UserExist > 0)
                                {
                                    dbDR2.Close();
                                    dbDR2.Dispose();
                                    if (guna2TextBox3.Text != "")
                                    {
                                        label23.Text = "D.R. Number Exist";
                                        label23.Visible = true;
                                        MessageBox.Show("D.R. already exist...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        label23.Text = "Exist";
                                        label23.Visible = false;
                                        save();
                                        getlatest();
                                    }
                                }
                                else
                                {

                                    label23.Text = "Exist";
                                    label23.Visible = false;
                                    save();
                                    getlatest();
                                }
                            }
                        }
                        dbDR.Close();
                    }
                }
                else
                {
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblDR] WHERE ([drnumber] = @drnumber)", dbDR);
                        check_User_Name.Parameters.AddWithValue("@drnumber", guna2TextBox3.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {
                            dbDR.Close();
                            dbDR.Dispose();
                            if (guna2TextBox3.Text != "")
                            {
                                label23.Text = "D.R. Number Exist";
                                label23.Visible = true;
                                MessageBox.Show("D.R. already exist...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                label23.Text = "Exist";
                                label23.Visible = false;
                                save();
                                getlatest();
                            }
                        }
                        else
                        {

                            label23.Text = "Exist";
                            label23.Visible = false;
                            save();
                            getlatest();
                        }
                    }
                }

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to cancel?", "Cancel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                guna2TextBox3.Text = "";
                guna2TextBox2.Text = "";
                guna2TextBox4.Text = "";
                label17.Text = "0";
                label18.Text = "0";
                label1.Text = "0";
                random1();
                checkicode();
                Cursor.Current = Cursors.Default;
            }
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.Text == "Item Name")
            {
                (dataGridView3.DataSource as DataTable).DefaultView.RowFilter = string.Format("description LIKE '%{0}%'", guna2TextBox5.Text.Replace("'", "''"));

            }
            else if (guna2ComboBox1.Text == "Product Code")
            {
                (dataGridView3.DataSource as DataTable).DefaultView.RowFilter = string.Format("product_code LIKE '{0}%'", guna2TextBox5.Text.Replace("'", "''"));
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[1].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[2].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[3].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[4].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[5].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[6].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[7].Value.ToString());
            MessageBox.Show(dataGridView1.Rows[0].Cells[8].Value.ToString());
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to exit?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Hide();//Hide the 'current' form, i.e frm_form1 
                            //show another form ( frm_form2 )   
                dashboardGLU frm = new dashboardGLU();
                frm.username = username;
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
        }

        private void DRForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            categories c = new categories();
            c.Show();
        }

        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void guna2TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                guna2TextBox2.Focus();
            }
        }

        private void guna2TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                guna2ComboBox3.Focus();
            }
        }

        private void guna2ComboBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                maskedTextBox1.Focus();
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                guna2TextBox4.Focus();
            }
        }

        private void guna2TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                guna2TextBox5.Focus();
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
