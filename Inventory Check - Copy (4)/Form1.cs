using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class Form1 : Form
    {

        private string randomitemcode;
        private string operation = "Incomplete";
        public string createdby;
        public string username;
        public string id;

        public Form1()
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
        private void A_BRAND_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            SuspendLayout();
            
            DateTime d = DateTime.Now;
            maskedTextBox1.Text = d.ToString("MM/dd/yyyy");
            loadall();
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;

            //button1.TabStop = false;
            //button1.FlatStyle = FlatStyle.Flat;
            //button1.FlatAppearance.BorderSize = 0;
            //bunifuDropdown2.DroppedDown = true;
            //comboBox1.Region = new Region(new Rectangle(3, 3, comboBox1.Width - 3, comboBox1.Height - 7));




            guna2ComboBox1.Text = "Item Name";
            random();
            checkicode();
            Auto();
            //loadsupplier();
            guna2TextBox1.Text = "";
            Cursor.Current = Cursors.Default;
            //MessageBox.Show(dataGridView3.Rows[1].Cells[0].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[1].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[2].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[3].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[4].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[5].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[6].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[7].Value.ToString());
            //MessageBox.Show(dataGridView3.Rows[1].Cells[8].Value.ToString());

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;

            ChangeControlStyles(dataGridView3, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView3.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView3.ColumnHeadersDefaultCellStyle.BackColor;
            //load();
            ResumeLayout();
            guna2TextBox1.Focus();
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
        public void random()
        {
            Random rnd2 = new Random();
            int month1 = rnd2.Next(1, 1000000);  // creates a number between 1 and 12
            int dice1 = rnd2.Next(1, 1000000);   // creates a number between 1 and 6
            int card1 = rnd2.Next(1, 10000000);
            randomitemcode = month1.ToString() + dice1.ToString() + card1.ToString();
        }

        //public void loadsupplier()
        //{

        //    using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
        //    {
        //        DataTable dt = new DataTable(); ;
        //        string Query = "Select * from tblSupplier";
        //        tblSupplier.Open();
        //        SqlCommand cmd = new SqlCommand(Query, tblSupplier);
        //        SqlDataReader myReader = cmd.ExecuteReader();
        //        dt.Load(myReader);
        //        tblSupplier.Close();guna2TextBox1
        //        comboBox1.DataSource = dt;
        //        comboBox1.ValueMember = "suppliername";
        //        comboBox1.DisplayMember = "Name";
        //        comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
        //        comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        //        tblSupplier.Dispose();
        //    }
        //}
        public DataTable dt = new DataTable();
        public void loadall()
        {
            dt.Clear();
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                string list = "Select ID,product_code,mfg_code,stocksleft,description,cost,selling,unit,dept from codeMaterial order by ID DESC";
                SqlCommand command = new SqlCommand(list, codeMaterial);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dataGridView3.DataSource = dt;
                codeMaterial.Close();
                //this.dataGridView3.Sort(this.dataGridView3.Columns[4], ListSortDirection.Descending);
                codeMaterial.Dispose();
                dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView3.RowHeadersVisible = false;
                //this.dataGridView2.Sort(this.dataGridView2.Columns[2], ListSortDirection.Descending);
            }
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void label14_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            label19.Text = d.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void label20_Click(object sender, EventArgs e)
        {

        }
        private void label20_TextChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

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

        private void textBox3_Resize(object sender, EventArgs e)
        {

        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {


        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

        }
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_ContextMenuStripChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        private void getprojectcode()
        {

           
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton23_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton22_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton24_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox2_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void customButton2_Click(object sender, EventArgs e)
        {

        }

        private void customButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void customTextbox12__TextChanged(object sender, EventArgs e)
        {

        }

        private void customTextbox14__TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

         

        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {
           
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {


        }
        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }
        private void save2()
        {

            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                string insStmt = "insert into tblSupplier ([suppliername],[createdby]) values" +
                    " (@suppliername,@createdby)";
                SqlCommand insCmd = new SqlCommand(insStmt, tblSupplier);
                insCmd.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                insCmd.Parameters.AddWithValue("@createdby", createdby);
                int affectedRows = insCmd.ExecuteNonQuery();
                tblSupplier.Close();
                tblSupplier.Dispose();
                save();

            }

            Cursor.Current = Cursors.Default;
        }
        private void save()
        {
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                string insStmt = "insert into tblIn ([suppliername], [additionalinfo], [itemcode], [ponumber], [datetime], [operation], [qty], [totalitems], [totalamount], [createdby], [sortid], [VAT], [TAX], [dateentered]) values" +
                    " (@suppliername,@additionalinfo,@itemcode,@ponumber,@datetime,@operation,@qty,@totalitems,@totalamount,@createdby,@sortid,@VAT,@TAX,@dateentered)";
                SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                insCmd.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                insCmd.Parameters.AddWithValue("@additionalinfo", guna2TextBox3.Text);
                insCmd.Parameters.AddWithValue("@itemcode", randomitemcode);
                insCmd.Parameters.AddWithValue("@ponumber", guna2TextBox2.Text);
              
                    insCmd.Parameters.AddWithValue("@datetime", maskedTextBox1.Text);
             
                insCmd.Parameters.AddWithValue("@operation", operation);
                insCmd.Parameters.AddWithValue("@qty", label17.Text);
                insCmd.Parameters.AddWithValue("@totalitems", label18.Text);
                insCmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox5.Text));
           
                insCmd.Parameters.AddWithValue("@createdby", createdby);
                insCmd.Parameters.AddWithValue("@sortid", Regex.Replace(guna2TextBox2.Text, @"[a-zA-Z]+", ""));
                if(guna2CustomCheckBox1.Checked)
                {
                    insCmd.Parameters.AddWithValue("@VAT", svat);
                    insCmd.Parameters.AddWithValue("@TAX", stax);
                }
                else
                {
                    insCmd.Parameters.AddWithValue("@VAT", "FALSE");
                    insCmd.Parameters.AddWithValue("@TAX", "FALSE");
                }
                insCmd.Parameters.AddWithValue("@dateentered", DateTime.Now.ToString("MM/dd/yyyy"));
                int affectedRows = insCmd.ExecuteNonQuery();
                tblIn.Close();
                tblIn.Dispose();
            }

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string insStmt2 = "insert into itemCode ([productcode],[mfgcode],[description],[unit],[qty],[icode],[iitem],[stocksleft],[cost],[selling],[total],[createdby],[ponumber]) values" +
                                  " (@productcode,@mfgcode,@description,@unit,@qty,@icode,@iitem,@stocksleft,@cost,@selling,@total,@createdby,@ponumber)";

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
                    insCmd2.Parameters.AddWithValue("@total", dataGridView1.Rows[i].Cells[10].Value.ToString());
                    insCmd2.Parameters.AddWithValue("@createdby", createdby);
                    insCmd2.Parameters.AddWithValue("@ponumber", guna2TextBox2.Text);
                    insCmd2.ExecuteNonQuery();
                }
                itemCode.Close();
            }
        
            guna2TextBox1.Text = ""; 
            dataGridView1.Rows.Clear();
            guna2TextBox3.Text = "";
            guna2TextBox2.Text = "";
            label17.Text = "0";
            label18.Text = "0";
            guna2TextBox5.Text = "0.00";
            label12.Text = "0.00";
            MessageBox.Show("P.O saved." + Environment.NewLine + "" + Environment.NewLine + "Note: This P.O hasn't change to the database, to change it just go to P.O History, select the P.O number and click Complete P.O to affect the changes.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[1].Value = dataGridView3.CurrentRow.Cells[1].Value;
            dataGridView1.Rows[n].Cells[2].Value = dataGridView3.CurrentRow.Cells[2].Value;
            dataGridView1.Rows[n].Cells[3].Value = dataGridView3.CurrentRow.Cells[4].Value;
            dataGridView1.Rows[n].Cells[4].Value = dataGridView3.CurrentRow.Cells[0].Value;
            dataGridView1.Rows[n].Cells[5].Value = "0.00";
            dataGridView1.Rows[n].Cells[6].Value = dataGridView3.CurrentRow.Cells[7].Value;
            dataGridView1.Rows[n].Cells[7].Value = "0.00";
            dataGridView1.Rows[n].Cells[8].Value = dataGridView3.CurrentRow.Cells[5].Value;
            dataGridView1.Rows[n].Cells[9].Value = dataGridView3.CurrentRow.Cells[6].Value;
            dataGridView1.Rows[n].Cells[10].Value = "0.00";
            dataGridView1.Rows[n].Cells[11].Value = dataGridView3.CurrentRow.Cells[8].Value;
            //dataGridView1.Rows[n].Cells[12].Value = dataGridView3.CurrentRow.Cells[3].Value;

            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                DataTable dt = new DataTable();
                String query = "SELECT stocksleft FROM codeMaterial WHERE ID = '" + dataGridView3.CurrentRow.Cells[0].Value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    dataGridView1.Rows[n].Cells["Column3"].Value = Math.Round(Convert.ToDecimal(rdr["stocksleft"]), 2).ToString();
                }
                codeMaterial.Close();
            }
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
        
        private void label11_Click(object sender, EventArgs e)
        {
            MessageBox.Show(sum3.ToString());
            MessageBox.Show(withtax.ToString());
            MessageBox.Show(withtax2.ToString());
            MessageBox.Show(finalwithtax.ToString());
        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
             && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
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
            if (dataGridView1.CurrentCell.ColumnIndex == 7)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
            if (dataGridView1.CurrentCell.ColumnIndex == 8)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    //tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }
        decimal sum3 = 0.00M;
        decimal withtax = 0.00M;
        decimal withtax2 = 0.00M;
        decimal finalwithtax = 0.00M;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Rows.Count >= 1)
            {

                count();

            }
        }
        string svat;
        string stax;
        private decimal totalwithdiscountonly = 0.00M;
        public void count()
        {
            totalwithdiscountonly = 0.00M;
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tblSettings WHERE description = 'vat'", otherDB);
               SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    svat = (rdr["value"].ToString());
                }
                else
                {
                    svat = "0";
                }
                otherDB.Close();
            }
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tblSettings WHERE description = 'tax'", otherDB);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    stax = (rdr["value"].ToString());
                }
                else
                {
                    stax = "0";
                }
                otherDB.Close();
            }

            if (dataGridView1.Rows.Count != 0)
            {
                int b = dataGridView1.Rows.Count;
                label18.Text = b.ToString();

                sum3 = 0.00M;
                withtax = 0.00M;
                withtax2 = 0.00M;
                finalwithtax = 0.00M;


                double sum2 = 0.00;
                sum2 += Convert.ToDouble(dataGridView1.CurrentRow.Cells[5].Value) * Convert.ToDouble(dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn21"].Value);


                double sum4 = 0.00;
                sum4 += Convert.ToDouble(dataGridView1.CurrentRow.Cells[5].Value);
                dataGridView1.CurrentRow.Cells[5].Value = sum4.ToString();

                double final = 0.00;
                final += sum2 - sum2 * Convert.ToDouble(dataGridView1.CurrentRow.Cells[7].Value);
                dataGridView1.CurrentRow.Cells[10].Value = Math.Round(final, 2).ToString();


                decimal totalsum = 0.00M;
                decimal sum = 0.00M;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn18"].Value);
                    totalsum += Convert.ToDecimal(dataGridView1.Rows[i].Cells["dataGridViewTextBoxColumn24"].Value);
                }
                label17.Text = Convert.ToDouble(sum).ToString();
                decimal distotal = totalsum;
                decimal disc = 0.00M;
               
                
                    if (guna2CustomCheckBox1.Checked)
                    {
                        disc = Convert.ToDecimal(stax) / 100;
                    distotal = distotal - (distotal / Convert.ToDecimal(svat)) * disc;

                        guna2TextBox5.Text = string.Format("{0:#,##0.00}", distotal);
                    }
                    else
                    {
                        guna2TextBox5.Text = string.Format("{0:#,##0.00}", distotal);
                    }
                


                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["dataGridViewTextBoxColumn21"].Value == null || row.Cells["dataGridViewTextBoxColumn21"].Value.ToString() == "")
                    {
                        row.Cells["dataGridViewTextBoxColumn21"].Value = "0.00";
                    }
                }

                label12.Text = string.Format("{0:#,##0.00}", totalsum);
                decimal tax = (Convert.ToDecimal(label12.Text) / Convert.ToDecimal(svat)) * disc;
                label23.Text = stax +"%" + string.Format("{0:#,##0.00}", tax);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                dt = dataGridView3.DataSource as DataTable;
                DataRow row = dt.NewRow();
                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    codeMaterial.Open();
                    String query = "SELECT * FROM codeMaterial WHERE ID = '" + dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn17"].Value.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(query, codeMaterial);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        //string list = "Select ID,product_code,mfg_code,stocksleft,description,cost,selling,unit,dept from codeMaterial order by ID DESC";

                        //label1.Text = (rdr["ponumber"].ToString());
                        //guna2TextBox1.Text = (rdr["ponumber"].ToString());
                        //label13.Text = (rdr["suppliername"].ToString());

                        row[0] = rdr["ID"].ToString();
                        row[1] = rdr["product_code"].ToString();
                        row[2] = rdr["mfg_code"].ToString();
                        row[3] = rdr["stocksleft"].ToString();
                        row[4] = rdr["description"].ToString();
                        row[5] = rdr["cost"].ToString();
                        row[6] = rdr["selling"].ToString();
                        row[7] = rdr["unit"].ToString();
                        row[8] = rdr["dept"].ToString();


                    }
                    codeMaterial.Close();
                }
                dt.Rows.Add(row);
                DataGridViewRow dgvDelRow = dataGridView1.CurrentRow;
                dataGridView1.Rows.Remove(dgvDelRow);

                dt.DefaultView.Sort = "ID DESC";
                if (dataGridView1.Rows.Count > 0)
                {
                    count();
                }
                else
                {
                    label18.Text = "0";
                    label17.Text = "0";
                    guna2TextBox5.Text = "0.00";
                }

            }
        }

        private void Form1_Leave(object sender, EventArgs e)
        {

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_2(object sender, EventArgs e)
        {
           
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void SaveGridValuesToXml()
        {
            dataGridView1.EndEdit(DataGridViewDataErrorContexts.Commit);

        }
        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            SaveGridValuesToXml();
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 5 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[5].Value != null)
                {
                    double var1 = 0.00;
                    var1 = double.Parse(e.Value.ToString());
                    e.Value = var1.ToString();
                }
            }
            if (e.ColumnIndex == 10 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[10].Value != null)
                {
                    double var1 = 0.00;
                    var1 = double.Parse(e.Value.ToString());
                    e.Value = var1.ToString();
                }
            }
            if (e.ColumnIndex == 8 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn21"].Value != null)
                {
                    double var1 = 0.00;
                    var1 = double.Parse(e.Value.ToString());
                    e.Value = var1.ToString();
                }
            }
            if (e.ColumnIndex == 7 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[7].Value != null)
                {
                    double var1 = 0.00;
                    var1 = double.Parse(e.Value.ToString());
                    e.Value = var1.ToString();
                }
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

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

        private void label13_Click(object sender, EventArgs e)
        {
            procode p = new procode();
            p.createdby = createdby;
            p.num = "1";
            p.ShowDialog();
        }



        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["dataGridViewTextBoxColumn21"].Value == null || row.Cells["dataGridViewTextBoxColumn21"].Value.ToString() == "")
                {
                    row.Cells["dataGridViewTextBoxColumn21"].Value = "0.00";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {
    

        }

        private void label14_Click_1(object sender, EventArgs e)
        {
        
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text != "")
            {

                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblIn] WHERE ([ponumber] = @ponumber)", tblIn);
                    check_User_Name.Parameters.AddWithValue("@ponumber", guna2TextBox2.Text);
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        label21.Text = "P.O Number Exist";
                        label21.Visible = true;
                    }
                    else
                    {
                        label21.Text = "Exist";
                        label21.Visible = false;
                    }

                    tblIn.Close();
                    tblIn.Dispose();
                }
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            additem a = new additem();
            a.name = createdby;
            a.forms = "FORM1";
            a.form = "form1";
            a.ShowDialog();
        }
        private void getlatest()
        {
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                var empcon = new SqlCommand("SELECT max(Id) FROM [tblIn]", tblIn);


                tblIn.Open();
                Int32 max = (Int32)empcon.ExecuteScalar();
               int getid = max;
                SqlDataReader rdr;
                //var empcon2 = new SqlCommand("SELECT Id FROM tblIn WHERE Id = '" + max.ToString() + "'", tblIn);

                DataTable dt = new DataTable();
                String query = "SELECT Id,itemcode FROM tblIn WHERE Id = '" + max.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    string getID = (rdr["Id"].ToString());
                    string getItemCode = (rdr["itemcode"].ToString());

                    DialogResult dialogResult = MessageBox.Show("Would you like to open it now?", "Open", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        In_supply i = new In_supply();

                        i.id = getID;
                        i.itemid = getItemCode;
                        i.name = createdby;


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
            if (guna2TextBox1.Text == "")
            {
                MessageBox.Show("Please enter text completely in Supplier and Project Code...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (guna2TextBox2.Text == "")
            {
                MessageBox.Show("Please enter P.O. Number...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("Please select an item to be purchase..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (label21.Text == "P.O Number Exist")
            {
                MessageBox.Show("Existing P.O Number!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                if (guna2TextBox2.Text != "")
                {

                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        SqlCommand check_User_Name2 = new SqlCommand("SELECT COUNT(*) FROM [tblIn] WHERE ([ponumber] = @ponumber)", tblIn);
                        check_User_Name2.Parameters.AddWithValue("@ponumber", guna2TextBox2.Text);
                        int UserExist2 = (int)check_User_Name2.ExecuteScalar();

                        if (UserExist2 > 0)
                        {
                            tblIn.Close();
                            label21.Text = "P.O Number Exist";
                            label21.Visible = true;


                        }
                        else
                        {
                            label21.Text = "Exist";
                            label21.Visible = false;

                            random();
                            checkicode();
                            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                            {

                                tblSupplier.Open();
                                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblSupplier] WHERE ([suppliername] = @suppliername)", tblSupplier);
                                check_User_Name.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                                int UserExist = (int)check_User_Name.ExecuteScalar();

                                if (UserExist <= 0)
                                {

                                    tblSupplier.Close();
                                    DialogResult dialogResult = MessageBox.Show("Supplier is not existed do you want to add?", "Add", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {

                                        save2();
                                        loadall();
                                        getlatest();
                                    }
                                    else
                                    {
                                        DialogResult dialogResult2 = MessageBox.Show("Do you wish to continue?", "Continue", MessageBoxButtons.YesNo);
                                        if (dialogResult2 == DialogResult.Yes)
                                        {
                                            save();
                                            loadall(); 
                                            getlatest();
                                        }
                                    }

                                }
                                else
                                {

                                    save();
                                    loadall();
                                    getlatest();
                                }
                            }
                            random();
                            checkicode();
                            Auto();
                        }

                    }
                }
                else
                {
                    random();
                    checkicode();
                    using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
                    {

                        tblSupplier.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblSupplier] WHERE ([suppliername] = @suppliername)", tblSupplier);
                        check_User_Name.Parameters.AddWithValue("@suppliername", guna2TextBox1.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist <= 0)
                        {

                            tblSupplier.Close();
                            DialogResult dialogResult = MessageBox.Show("Supplier is none existed do you me to add?", "Add", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {

                                save2();
                                loadall();
                                getlatest();
                            }
                            else
                            {
                                DialogResult dialogResult2 = MessageBox.Show("Do you wish to continue?", "Continue", MessageBoxButtons.YesNo);
                                if (dialogResult2 == DialogResult.Yes)
                                {
                                    save();
                                    loadall();
                                    getlatest();
                                }
                            }

                        }
                        else
                        {

                            save();
                            loadall();
                            getlatest();
                        }
                    }
                    random();
                    checkicode();
                    Auto();
                }



            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to cancel?", "Cancel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                random();
                checkicode();
                guna2TextBox1.Text = "";
                dataGridView1.Rows.Clear();
                guna2TextBox2.Text = "";
                guna2TextBox3.Text = "";
                label17.Text = "0";
                label18.Text = "0";
                guna2TextBox5.Text = "0.00";
                label12.Text = "0.00";
                loadall();
                Cursor.Current = Cursors.Default;
            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.Text == "Item Name")
            {
                (dataGridView3.DataSource as DataTable).DefaultView.RowFilter = string.Format("description LIKE '%{0}%'", guna2TextBox4.Text.Replace("'", "''"));
            }
            else if (guna2ComboBox1.Text == "Product Code")
            {
                (dataGridView3.DataSource as DataTable).DefaultView.RowFilter = string.Format("product_code LIKE '{0}%'", guna2TextBox4.Text.Replace("'", "''"));
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to exit?", "Exit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Hide();
                dashboardGLU frm = new dashboardGLU();
                frm.username = username;
                frm.id = id;
                frm.ShowDialog();
                //Close the form.(frm_form1)
                this.Dispose();
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        
        }
        AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
        public void Auto()

        {
            using (SqlConnection tblSupplier = new SqlConnection(ConfigurationManager.ConnectionStrings["tblSupplier"].ConnectionString))
            {
                tblSupplier.Open();
                SqlDataAdapter da = new SqlDataAdapter("select suppliername from tblSupplier", tblSupplier);

                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)

                {

                    for (int i = 0; i < dt.Rows.Count; i++)

                    {

                        coll.Add(dt.Rows[i]["suppliername"].ToString());

                    }

                }
                else

                {

                    //MessageBox.Show("Name not found");

                }
                tblSupplier.Close();
                guna2TextBox1.AutoCompleteMode = AutoCompleteMode.Suggest;

                guna2TextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

                guna2TextBox1.AutoCompleteCustomSource = coll;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            categories c = new categories();
            c.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView3.Rows[1].Cells[0].Value.ToString());
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
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
                maskedTextBox1.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                guna2TextBox1.Focus();
            }
        }

        private void guna2DateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                guna2TextBox4.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                guna2TextBox2.Focus();
            }
        }

        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {
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
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
        string amountrowdis= "";
        public string LabelText
        {
            get { return amountrowdis; }
            set { amountrowdis = value; }
        }
        string firstcol;
        public string tax
        {
            get { return label23.Text; }
            set { label23.Text = value; }
        }
        public string dgvrow1
        {
           
            get { return firstcol; }
            set { firstcol = value; }
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void guna2NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox5.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void dataGridView2_Leave(object sender, EventArgs e)
        {

        }

        private void dataGridView3_Leave(object sender, EventArgs e)
        {
            dataGridView3.ClearSelection();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void guna2CustomCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox1.Checked)
            {
                label11.Text = "Total Amount (VAT Inclusive)";
                count();
            }
            else
            {
                label11.Text = "Total Amount (VAT Exclusive)";
                count();
            }
        }

        private void label24_Click(object sender, EventArgs e)
        {
            VATUPDATE a = new VATUPDATE(this);
            a.form = "Form1";
            a.ShowDialog();
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label23_TextChanged(object sender, EventArgs e)
        {
            count();
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
