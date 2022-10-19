using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Inventory_Check
{
    public partial class In_supply : Form
    {
        public string name;
        private string date;
        private string icode;

        SqlDataReader rdr;
        public string id;
        public string itemid;
        private string operationcheck;
        public In_supply()
        {
            InitializeComponent();
            pictureBox5.InitialImage = null;
            pictureBox14.InitialImage = null;
            pictureBox15.InitialImage = null;
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
        public void loadsupply()
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
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
                tblSupplier.Close();
                guna2TextBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                guna2TextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                guna2TextBox2.AutoCompleteCustomSource = coll;
            }
        }
        public bool lo = false;
        protected override bool ShowFocusCues => false;
        private void In_supply_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            lo = true;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            getinfo();
            load();
            dataGridView1.Columns["four"].ReadOnly = true;
            dataGridView1.Columns["five"].ReadOnly = true;
            dataGridView1.Columns["delete"].Visible = false;

            ChangeControlStyles(dataGridView1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView1.RowHeadersVisible = false;

            ChangeControlStyles(dataGridView2, ControlStyles.OptimizedDoubleBuffer, true);
            this.dataGridView2.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dataGridView2.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView2.RowHeadersVisible = false;
            //load();
            Auto();
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblPending] WHERE type = @type AND docid = @docid", otherDB);
                check_User_Name.Parameters.AddWithValue("@type", "PO");
                check_User_Name.Parameters.AddWithValue("@docid", id);
                int UserExist = (int)check_User_Name.ExecuteScalar();

                if (UserExist > 0)
                {
                    guna2Button2.Visible = true;
                }
                else
                {
                    guna2Button1.Visible = true;
                }
                otherDB.Close();
            }
            //MessageBox.Show("load");
            lo = false;
            count();
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void load()
        {

            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                dataGridView1.Rows.Clear();
                itemCode.Close();
                itemCode.Open();

                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("Select productcode,mfgcode,description,qty,unit,Id,iitem,stocksleft,cost,selling,discount,total,icode from itemCode where icode = '" + itemid + "' order by Id asc", itemCode))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells["one"].Value = item["productcode"].ToString();
                    dataGridView1.Rows[n].Cells["two"].Value = item["mfgcode"];
                    dataGridView1.Rows[n].Cells["three"].Value = item["description"];
                    dataGridView1.Rows[n].Cells["four"].Value = item["qty"];
                    dataGridView1.Rows[n].Cells["five"].Value = item["unit"];
                    dataGridView1.Rows[n].Cells["six"].Value = item["Id"];
                    dataGridView1.Rows[n].Cells["seven"].Value = item["icode"];
                    dataGridView1.Rows[n].Cells["eight"].Value = item["iitem"];
                    dataGridView1.Rows[n].Cells["nine"].Value = item["stocksleft"];
                    dataGridView1.Rows[n].Cells["ten"].Value = item["cost"];
                    dataGridView1.Rows[n].Cells["eleven"].Value = item["selling"];
                    dataGridView1.Rows[n].Cells["twelve"].Value = item["discount"];
                    dataGridView1.Rows[n].Cells["thirteen"].Value = item["total"];
                    
                }
                itemCode.Close();
                dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView1.RowHeadersVisible = false;
            }

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                dataGridView4.Rows.Clear();
                tblIn.Open();

                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("Select * from tblDiscounts where ponumber = '" + label1.Text + "' order by id asc", tblIn))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView4.Rows.Add();
                    dataGridView4.Rows[n].Cells["percent"].Value = item["apercent"].ToString();
                    dataGridView4.Rows[n].Cells["amount"].Value = item["amount"];
                    dataGridView4.Rows[n].Cells["disid"].Value = item["id"];
                    dataGridView4.Rows[n].Cells["type"].Value = item["type"];
                    dataGridView4.Rows[n].Cells["remarks1"].Value = item["remarks"];

                }
                tblIn.Close();
                dataGridView4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView4.RowHeadersVisible = false;
            }
            dataGridView1.ClearSelection();
            dataGridView4.ClearSelection();
        }
        public void load2()
        {

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                dataGridView4.Rows.Clear();
                tblIn.Open();

                DataTable dt = new DataTable();
                using (SqlDataAdapter a2 = new SqlDataAdapter("Select * from tblDiscounts where ponumber = '" + label1.Text + "' order by id asc", tblIn))
                    a2.Fill(dt);
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                foreach (DataRow item in dt.Rows)
                {
                    int n = dataGridView4.Rows.Add();
                    if (item["type"].ToString() == "Add Amount")
                    {
                        dataGridView4.Rows[n].Cells["percent"].Value = string.Format("{0:#,##0.00}", item["apercent"].ToString());
                    }
                    else
                    {
                        dataGridView4.Rows[n].Cells["percent"].Value = item["apercent"].ToString();
                    }
                    dataGridView4.Rows[n].Cells["amount"].Value = item["amount"];
                    dataGridView4.Rows[n].Cells["disid"].Value = item["id"];
                    dataGridView4.Rows[n].Cells["type"].Value = item["type"];
                    dataGridView4.Rows[n].Cells["remarks1"].Value = item["remarks"];

                }
                tblIn.Close();
                dataGridView4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dataGridView4.RowHeadersVisible = false;
            }
            dataGridView4.ClearSelection();
        }
        private string poid;
        bool founderror = false;
        public void getinfo()
        {

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                //dataGridView1.Rows.Clear();
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblIn WHERE Id = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    dbamount = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));

                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["totalamount"].ToString()));
                    label1.Text = (rdr["ponumber"].ToString());
                    guna2TextBox1.Text = (rdr["ponumber"].ToString());
                    label13.Text = (rdr["suppliername"].ToString());
                    guna2TextBox2.Text = (rdr["suppliername"].ToString());
                    DateTime date = DateTime.Parse(rdr["datetime"].ToString());
                    label20.Text = date.ToString("MM/dd/yyyy");
                    guna2TextBox3.Text = (rdr["additionalinfo"].ToString());
                    label17.Text = (rdr["qty"].ToString());
                    label18.Text = (rdr["totalitems"].ToString());
                    label19.Text = (rdr["createdby"].ToString());
                    operationcheck = (rdr["operation"].ToString());
                    label25.Text = (rdr["purchasecompletedby"].ToString());
                    icode = (rdr["itemcode"].ToString());
                    poid = (rdr["Id"].ToString());
                    label38.Text = (rdr["VAT"].ToString());
                    label8.Text = (rdr["dateentered"].ToString());
                    if (rdr["TAX"].ToString() == "FALSE")
                    {
                        label41.Text = (rdr["TAX"].ToString());
                    }
                    else if (rdr["TAX"].ToString() != "")
                    {
                        label41.Text = (rdr["TAX"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Found no such record.");
                    founderror = true;
                    this.Close();
                }
                tblIn.Close();
                if (operationcheck == "Completed")
                {
                    label30.Visible = true;
                    guna2Button11.Visible = true;
                    button3.Visible = false;
                    button5.Visible = false;
                    pictureBox14.Visible = true;
                    button1.Visible = true;
                    guna2Button3.Visible = false;
                    //guna2TextBox1.SetBounds(46, 95, 270, 32);
                    //label43.Visible = false;
                    //label46.Visible = false;
                    //label47.Visible = false;
                    guna2Panel4.SetBounds(945, 8, 273, 227);
                }
                else
                {
                    label27.Visible = false;
                    label30.Visible = false;
                    guna2Button11.Visible = false;
                    guna2Button3.Visible = true;
                    //label43.Visible = true;
                    //label46.Visible = true;
                    //label47.Visible = true;
                    guna2Panel4.SetBounds(945, 8, 273, 282);

                }

            }
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblSI WHERE poid = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (rdr["SIno"].ToString() == "" || rdr["SIno"] == null || rdr["SIno"] == DBNull.Value)
                    {
                        label9.Text = "None";
                        label9.ForeColor = Color.LightCoral;
                        pictureBox4.Visible = false;
                    }
                    else
                    {
                        label9.Text = (rdr["SIno"].ToString());
                        label9.ForeColor = Color.White;
                        pictureBox4.Visible = true;
                    }
                }
                else
                {
                    label9.Text = "None";
                    label9.ForeColor = Color.LightCoral;
                    pictureBox4.Visible = false;
                }
                tblIn.Close();

            }
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM tblVP WHERE poid = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, tblIn);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    if (rdr["vpno"].ToString() == "" || rdr["vpno"] == null || rdr["vpno"] == DBNull.Value)
                    {
                        label31.Text = "None";
                        label29.Text = "0.00";
                        label31.ForeColor = Color.LightCoral;
                        pictureBox7.Visible = false;
                    }
                    else
                    {
                        label31.Text = (rdr["vpno"].ToString());
                        label29.Text = "₱ " + string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["vpamount"].ToString()));
                        label31.ForeColor = Color.White;
                        pictureBox7.Visible = true;
                    }
                }
                else
                {
                    label31.Text = "None";
                    label29.Text = "0.00";
                    label31.ForeColor = Color.LightCoral;
                    pictureBox7.Visible = false;
                }
                tblIn.Close();

            }

            if (form == "dashboard")
            {
                foreach (DataRow row in obj1.dt.Rows)
                {
                    if (row["docid"].ToString() == id)
                    {
                        row["ponumber"] = label1.Text;
                        row["suppliername"] = label13.Text;
                        row["date"] = label20.Text;
                        row["remarks"] = guna2TextBox3.Text;
                        break;
                    }
                }
            }

        }
        dashboard obj1 = (dashboard)Application.OpenForms["dashboard"];
      
        private string dbamount;
        public void notbalance()
        {
            if (founderror == false)
            {
                if (lo == false)
                {
                    if (dbamount != guna2TextBox4.Text)
                    {
                        var tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString);
                        try
                        {
                            SqlCommand cmd = new SqlCommand("update tblIn set totalamount=@totalamount,VAT=@VAT,TAX=@TAX,qty=@qty,totalitems=@totalitems where Id=@Id", tblIn);

                            tblIn.Open();
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
                            cmd.Parameters.AddWithValue("@VAT", label38.Text);
                            cmd.Parameters.AddWithValue("@TAX", label41.Text);
                            cmd.Parameters.AddWithValue("@qty", label17.Text);
                            cmd.Parameters.AddWithValue("@totalitems", label18.Text);
                            cmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            tblIn.Close();
                        }

                        if (dataGridView4.Rows.Count != 0)
                        {

                            tblIn.Open();
                            for (int i = 0; i < dataGridView4.Rows.Count; i++)
                            {
                                string insStmt2 = "update tblDiscounts set apercent=@apercent,amount=@amount,type=@type,remarks=@remarks where id=@id";

                                SqlCommand insCmd2 = new SqlCommand(insStmt2, tblIn);

                                insCmd2.Parameters.AddWithValue("@id", dataGridView4.Rows[i].Cells["disid"].Value.ToString()); 
                                if (dataGridView4.Rows[i].Cells["type"].Value.ToString() == "Add Amount")
                                {
                                    insCmd2.Parameters.AddWithValue("@apercent", string.Format("{0:#,##0.00}", dataGridView4.Rows[i].Cells["percent"].Value.ToString()));
                                }
                                else
                                {
                                    insCmd2.Parameters.AddWithValue("@apercent", dataGridView4.Rows[i].Cells["percent"].Value.ToString());
                                }
                                insCmd2.Parameters.AddWithValue("@amount", dataGridView4.Rows[i].Cells["amount"].Value.ToString());
                                insCmd2.Parameters.AddWithValue("@type", dataGridView4.Rows[i].Cells["type"].Value.ToString());
                                insCmd2.Parameters.AddWithValue("@remarks", dataGridView4.Rows[i].Cells["remarks1"].Value.ToString());
                                insCmd2.ExecuteNonQuery();
                            }
                            tblIn.Close();

                        }
                    }
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

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {
          
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


        }
        private string stocks;
        private string total;
        private string operation = "Completed";
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void class11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }
        prein obj = (prein)Application.OpenForms["prein"];

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
           
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            date = d.ToString("yyyy/MM/dd HH:mm:ss");
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns["delete"].Visible == true)
            {
                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    int n = dataGridView2.Rows.Add();
                    dataGridView2.Rows[n].Cells[0].Value = dataGridView1.CurrentRow.Cells["six"].Value;
                    DataGridViewRow dgvDelRow = dataGridView1.CurrentRow;
                    dataGridView1.Rows.Remove(dgvDelRow);

                    if (dataGridView1.Rows.Count > 0)
                    {
                        count();
                    }
                    else
                    {
                        label18.Text = "0";
                        label17.Text = "0";
                        guna2TextBox4.Text = "0.00";
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (button3.Visible == false)
            {
                if (dataGridView1.Rows.Count >= 1)
                {
                    double sum2 = 0.00;
                    sum2 += Convert.ToDouble(dataGridView1.CurrentRow.Cells["four"].Value) * Convert.ToDouble(dataGridView1.CurrentRow.Cells["ten"].Value);
                    dataGridView1.CurrentRow.Cells["thirteen"].Value = Math.Round((double)Convert.ToDouble(sum2), 2).ToString("N2");
                    double sum4 = 0.00;
                    sum4 += Math.Round(Convert.ToDouble(dataGridView1.CurrentRow.Cells["four"].Value), 2);

                    dataGridView1.CurrentRow.Cells[4].Value = Math.Round((double)Convert.ToDouble(sum4), 2).ToString("N2");

                    count();
                }
            }
        }
        public void count()
        {
            if (lo == false)
            {
                //MessageBox.Show("1");
                totalwithdiscountonly = 0.00M;
                //MessageBox.Show(label38.Text.ToString());
                //MessageBox.Show(label41.Text.ToString());
                var stringNumber = label38.Text.Replace("%", "");
                decimal numericValue;
                bool isNumber = decimal.TryParse(stringNumber, out numericValue);

                var stringNumber1 = label41.Text.Replace("%", "");
                decimal numericValue1;
                bool isNumber1 = decimal.TryParse(stringNumber1, out numericValue1);

                //MessageBox.Show("2");
                if (isNumber == true)
                {
                    if (isNumber1 == true)
                    {
                        //MessageBox.Show("3");
                        string svat = label38.Text.Replace("%", "");
                        string stax = label41.Text.Replace("%", "");

                        if (dataGridView1.Rows.Count > 0)
                        {
                            //MessageBox.Show("4");
                            int b = dataGridView1.Rows.Count;
                            label18.Text = b.ToString();
                            decimal sum3 = 0.00M;
                            decimal withtax = 0.00M;
                            decimal withtax2 = 0.00M;
                            decimal finalwithtax = 0.00M;
                            decimal totalsum = 0.00M;
                            decimal sumqty = 0.00M;

                            //MessageBox.Show("5");
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                sumqty += Convert.ToDecimal(dataGridView1.Rows[i].Cells["four"].Value);
                                totalsum += Convert.ToDecimal(dataGridView1.Rows[i].Cells["thirteen"].Value);
                            }
                            //MessageBox.Show("6");
                            label17.Text = Convert.ToDouble(sumqty).ToString();
                            decimal distotal = totalsum;
                            decimal disc = 0.00M;
                            decimal totalwithtax = 0.00M;
                            if (dataGridView4.Rows.Count == 0)
                            {
                                //MessageBox.Show("7");
                                if (label38.Text != "FALSE")
                                {
                                    if (Convert.ToDecimal(svat) > 0.00M)
                                    {
                                        disc = Convert.ToDecimal(stax) / 100;
                                        //totalwithtax = totalsum - (totalsum / Convert.ToDecimal(svat)) * disc;
                                        if (Convert.ToDecimal(svat) == 0.00M)
                                        {
                                            totalwithtax = totalsum - (totalsum) * disc;
                                        }
                                        else
                                        {
                                            totalwithtax = totalsum - (totalsum / Convert.ToDecimal(svat)) * disc;
                                        }
                                        guna2TextBox4.Text = string.Format("{0:#,##0.00}", totalwithtax);
                                    }
                                    else
                                    {
                                        guna2TextBox4.Text = string.Format("{0:#,##0.00}", totalsum);
                                    }

                                    //MessageBox.Show("8");
                                }
                                else
                                {
                                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", totalsum);
                                }
                                //MessageBox.Show("9");
                            }
                            else
                            {
                                //MessageBox.Show("10");
                                if (label38.Text != "FALSE")
                                {
                                    int rowdgv = 0;
                                    foreach (DataGridViewRow row in dataGridView4.Rows)
                                    {
                                        if (row.Cells["type"].Value == null || row.Cells["type"].Value == DBNull.Value || String.IsNullOrWhiteSpace(row.Cells["type"].Value.ToString()))
                                        {
                                            //    // here is your message box...
                                            //    MessageBox.Show("Null");
                                        }

                                        else
                                        {
                                            //MessageBox.Show("11");
                                            if (rowdgv == 0)
                                            {
                                                if (row.Cells["type"].Value.ToString() == "Add Amount")
                                                {
                                                    if (row.Cells["percent"].Value.ToString() != null)
                                                    {
                                                        decimal disc1 = Convert.ToDecimal(row.Cells["percent"].Value.ToString());
                                                        distotal = distotal + disc1;
                                                        row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                        totalwithdiscountonly = distotal;
                                                    }
                                                }
                                                else
                                                {
                                                    if (row.Cells["percent"].Value.ToString() != null)
                                                    {
                                                        decimal disc1 = (Convert.ToDecimal(row.Cells["percent"].Value.ToString().Replace("%", "")) / 100);
                                                        distotal = distotal - (distotal * disc1);
                                                        row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                        totalwithdiscountonly = distotal;
                                                    }
                                                }

                                                //MessageBox.Show("12");
                                            }
                                            else
                                            {
                                                if (row.Cells["type"].Value.ToString() == "Add Amount")
                                                {
                                                    if (row.Cells["percent"].Value.ToString() != null)
                                                    {
                                                        //MessageBox.Show(Convert.ToDecimal(row.Cells["percent"].Value.ToString()).ToString());
                                                        decimal disc1 = Convert.ToDecimal(row.Cells["percent"].Value.ToString());
                                                        distotal = totalwithdiscountonly + disc1;
                                                        row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                        totalwithdiscountonly = distotal;
                                                    }
                                                }
                                                else
                                                {
                                                    if (row.Cells["percent"].Value.ToString() != null)
                                                    {
                                                        decimal disc1 = (Convert.ToDecimal(row.Cells["percent"].Value.ToString().Replace("%", "")) / 100);
                                                        distotal = totalwithdiscountonly - (totalwithdiscountonly * disc1);
                                                        row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                        totalwithdiscountonly = distotal;
                                                    }
                                                }
                                                //MessageBox.Show("13");

                                            }

                                            rowdgv++;
                                        }
                                    }

                                    disc = Convert.ToDecimal(stax) / 100;
                                    if (Convert.ToDecimal(svat) == 0.00M)
                                    {
                                        totalwithtax = distotal - (distotal) * disc;
                                    }
                                    else
                                    {
                                        totalwithtax = distotal - (distotal / Convert.ToDecimal(svat)) * disc;
                                    }

                                    //MessageBox.Show("14");

                                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", totalwithtax);
                                }
                                else
                                {
                                    int rowdgv = 0;
                                    foreach (DataGridViewRow row in dataGridView4.Rows)
                                    {
                                        if (rowdgv == 0)
                                        {
                                            if (row.Cells["type"].Value.ToString() == "Add Amount")
                                            {
                                                if (row.Cells["percent"].Value.ToString() != null)
                                                {
                                                    decimal disc1 = Convert.ToDecimal(row.Cells["percent"].Value.ToString());
                                                    distotal = distotal + disc1;
                                                    row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                    totalwithdiscountonly = distotal;
                                                }
                                            }
                                            else
                                            {
                                                decimal disc1 = (Convert.ToDecimal(row.Cells["percent"].Value.ToString().Replace("%", "")) / 100);
                                                distotal = distotal - (distotal * disc1);
                                                row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                totalwithdiscountonly = distotal;
                                            }
                                        }
                                        else
                                        {
                                            if (row.Cells["type"].Value.ToString() == "Add Amount")
                                            {
                                                if (row.Cells["percent"].Value.ToString() != null)
                                                {
                                                    decimal disc1 = Convert.ToDecimal(row.Cells["percent"].Value.ToString());
                                                    distotal = totalwithdiscountonly + disc1;
                                                    row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                    totalwithdiscountonly = distotal;
                                                }
                                            }
                                            else
                                            {
                                                decimal disc1 = (Convert.ToDecimal(row.Cells["percent"].Value.ToString().Replace("%", "")) / 100);
                                                distotal = totalwithdiscountonly - (totalwithdiscountonly * disc1);
                                                row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
                                                totalwithdiscountonly = distotal;
                                            }
                                        }
                                        rowdgv++;
                                    }

                                    //MessageBox.Show("15");
                                    guna2TextBox4.Text = string.Format("{0:#,##0.00}", distotal);
                                }
                            }

                            if (Convert.ToDecimal(svat) > 0.00M)
                            {
                                decimal tax = (totalsum / Convert.ToDecimal(svat)) * disc;
                                label41.Text = stax;
                            }
                            else
                            {
                                label41.Text = "0.00";
                            }
                            label44.Text = string.Format("{0:#,##0.00}", totalsum);

                            //MessageBox.Show("16");
                            notbalance();
                        }
                    }
                }
            }
        }
        private decimal totalwithdiscountonly = 0.00M;
        //public void count2()
        //{
        //    if (lo == false)
        //    {

        //        if (label38.Text != "")
        //        {
        //            if (label41.Text != "")
        //            {
        //                //MessageBox.Show("s");
        //                string svat = label38.Text.Replace("%", "");
        //                string stax = label41.Text.Replace("%", "");

        //                if (dataGridView1.Rows.Count != 0)
        //                {


        //                    decimal totalsum = 0.00M;
        //                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
        //                    {
        //                        totalsum += Convert.ToDecimal(dataGridView1.Rows[i].Cells["thirteen"].Value);
        //                    }
        //                    decimal distotal = totalsum;
        //                    decimal disc = 0.00M;
        //                    decimal totalwithtax = 0.00M;
        //                    if (dataGridView4.Rows.Count == 0)
        //                    {
        //                        if (label38.Text != "FALSE")
        //                        {
        //                            if (Convert.ToDecimal(svat) > 0.00M)
        //                            {
        //                                disc = Convert.ToDecimal(stax) / 100;
        //                                totalwithtax = totalsum - (totalsum / Convert.ToDecimal(svat)) * disc;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (label38.Text != "FALSE")
        //                        {
        //                            foreach (DataGridViewRow row in dataGridView4.Rows)
        //                            {
        //                                decimal disc1 = (Convert.ToDecimal(row.Cells["percent"].Value.ToString().Replace("%", "")) / 100);
        //                                totalwithtax = totalsum - (totalsum * disc1);
        //                                row.Cells["amount"].Value = string.Format("{0:#,##0.00}", totalwithtax);
        //                            }
        //                            disc = Convert.ToDecimal(stax) / 100;
        //                            totalwithtax = totalwithtax - (totalwithtax / Convert.ToDecimal(svat)) * disc;

        //                            if (dbamount != string.Format("{0:#,##0.00}", totalwithtax))
        //                            {
        //                                notbalance();
        //                            }

        //                        }
        //                        else
        //                        {
        //                            foreach (DataGridViewRow row in dataGridView4.Rows)
        //                            {
        //                                decimal disc1 = (Convert.ToDecimal(row.Cells["percent"].Value.ToString().Replace("%", "")) / 100);
        //                                distotal = distotal - (distotal * disc1);
        //                                row.Cells["amount"].Value = string.Format("{0:#,##0.00}", distotal);
        //                                //MessageBox.Show(distotal.ToString());
        //                            }

        //                            if (dbamount != string.Format("{0:#,##0.00}", distotal))
        //                            {
        //                                notbalance();
        //                            }
        //                            //guna2TextBox4.Text = string.Format("{0:#,##0.00}", distotal);
        //                        }
        //                    }

        //                    if (Convert.ToDecimal(svat) > 0.00M)
        //                    {
        //                        decimal tax = (totalsum / Convert.ToDecimal(svat)) * disc;
        //                        label41.Text = stax;
        //                    }
        //                    else
        //                    {
        //                        label41.Text = "0.00";
        //                    }
        //                    label44.Text = string.Format("{0:#,##0.00}", totalsum);
        //                }
        //                notbalance();
        //            }
        //        }
        //    }
        //}
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
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

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["four"].Value != null)
                {
                    if (e.Value.ToString() != "")
                    {
                        double var1 = double.Parse(e.Value.ToString());
                        e.Value = var1.ToString();
                    }
                    else if (e.Value.ToString() == "0.00")
                    {
                        e.Value = "0.00";
                    }
                    else
                    {
                        e.Value = "0.00";
                    }
                }
            }
            if (e.ColumnIndex == 13 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["thirteen"].Value != null)
                {
                    if (e.Value.ToString() != "")
                    {
                        double var1 = double.Parse(e.Value.ToString());
                        e.Value = var1.ToString();
                    }
                    else if (e.Value.ToString() == "0.00")
                    {
                        e.Value = "0.00";
                    }
                    else
                    {
                        e.Value = "0.00";
                    }
                }
            }
            if (e.ColumnIndex == 10 && e.RowIndex != this.dataGridView1.NewRowIndex)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["ten"].Value != null)
                {
                    if (e.Value.ToString() != "")
                    {
                        double var1 = double.Parse(e.Value.ToString());
                        e.Value = var1.ToString();
                    }
                    else if (e.Value.ToString() == "0.00")
                    {
                        e.Value = "0.00";
                    }
                    else
                    {
                        e.Value = "0.00";
                    }
                }
            }
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "four")
            {
                if (String.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText =
                        "Qty must not be empty";
                    e.Cancel = true;
                }
            }

        }
        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.
            dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
        }
        public string form;
        private void button2_Click_1(object sender, EventArgs e)
        {
           
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
        }
        public void update()
        {
            Cursor.Current = Cursors.WaitCursor;
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                DataTable dt = new DataTable();
                String query9 = "SELECT * FROM tblIn WHERE Id = '" + id + "'";
                SqlCommand cmd9 = new SqlCommand(query9, tblIn);
                rdr = cmd9.ExecuteReader();

                if (rdr.Read())
                {
                    operationcheck = (rdr["operation"].ToString());
                }
                tblIn.Close();
            }
            if (operationcheck == "Completed")
            {
                MessageBox.Show("This P.O. is completed purchase already...");
                button3.Visible = false;
                button5.Visible = false;
                pictureBox14.Visible = true;
                button1.Visible = true;
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    double sum2 = 0.00;
                    bool found = true;
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        codeMaterial.Open();
                        String query = "SELECT * FROM codeMaterial WHERE ID = '" + dataGridView1.Rows[i].Cells["eight"].Value.ToString() + "'";
                        SqlCommand cmd2 = new SqlCommand(query, codeMaterial);
                        rdr = cmd2.ExecuteReader();

                        if (rdr.Read())
                        {
                            stocks = (rdr["stocksleft"].ToString());
                        }
                        else
                        {
                            found = false;
                        }
                        codeMaterial.Close();
                    }
                    if (found == true)
                    {

                        sum2 += Convert.ToDouble(stocks.ToString()) + Convert.ToDouble(dataGridView1.Rows[i].Cells["four"].Value);

                        total = Math.Round((double)Convert.ToDouble(sum2), 2).ToString();


                        using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                        {
                            SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                            codeMaterial.Open();
                            cmd.Parameters.AddWithValue("@ID", dataGridView1.Rows[i].Cells["eight"].Value.ToString());
                            cmd.Parameters.AddWithValue("@stocksleft", total);
                            cmd.ExecuteNonQuery();
                            codeMaterial.Close();
                            codeMaterial.Dispose();
                        }
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        string p = "PO";
                        SqlCommand cmd4 = new SqlCommand("update itemCode set typeofp=@typeofp,poid=@poid where icode=@icode", itemCode);
                        itemCode.Open();
                        cmd4.Parameters.AddWithValue("@icode", dataGridView1.Rows[i].Cells["seven"].Value.ToString());
                        cmd4.Parameters.AddWithValue("@typeofp", p);
                        cmd4.Parameters.AddWithValue("@poid", id);
                        cmd4.ExecuteNonQuery();
                        itemCode.Close();
                        itemCode.Dispose();
                    }

                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    SqlCommand cmd3 = new SqlCommand("update tblIn set operation=@operation,purchasecompletedby=@purchasecompletedby where Id=@Id", tblIn);
                    tblIn.Open();
                    cmd3.Parameters.AddWithValue("@Id", id);
                    cmd3.Parameters.AddWithValue("@operation", operation);
                    cmd3.Parameters.AddWithValue("@purchasecompletedby", name);
                    cmd3.ExecuteNonQuery();
                    tblIn.Close();
                    tblIn.Dispose();
                }
                string completepo = "Completed P.O";
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@date", date);
                    insCmd.Parameters.AddWithValue("@operation", completepo);
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblIn.Close();
                    tblIn.Dispose();
                }

                if (form == "prein")
                {
                    obj.loadprein();
                }

                dataGridView1.Rows.Clear();
                guna2TextBox1.Visible = false;
                guna2TextBox2.Visible = false;
                label7.Text = "Exist";
                label7.Visible = false;
                label27.Text = "Change P.O. No.";
                guna2TextBox1.Visible = false;
                label28.Visible = false;
                lo = true;
                getinfo();
                load();
                lo = false;
                count();
                dataGridView1.Columns["four"].ReadOnly = true;
                dataGridView1.Columns["delete"].Visible = false;
                dataGridView1.Columns["five"].ReadOnly = true;
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Record Updated Successfully");
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void label24_Click(object sender, EventArgs e)
        {
        }

        private void label27_Click(object sender, EventArgs e)
        {

            if (label27.Text == "Click Here to Save")
            {
                if (label7.Text == "P.O Number Exist")
                {
                    MessageBox.Show("P.O is already used...");
                }
                else
                {
                    save();
                }
            }
            else
            {
                guna2TextBox1.Visible = true;
                label27.Text = "Click Here to Save";
                label28.Visible = true;
            }
        }

        private void label28_Click(object sender, EventArgs e)
        {
            label27.Text = "Change P.O. No.";
            guna2TextBox1.Visible = false;
            label7.Visible = false;
            label28.Visible = false;
            label7.Text = "Exist";
        }
        private void save()
        {
            label27.Text = "Change P.O. No.";
            guna2TextBox1.Visible = false;


            string completepo = "Editing P.O. from " + label1.Text + " to " + guna2TextBox1.Text;

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                tblIn.Open();
                string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                    " (@name,@date,@operation,@id)";
                SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@name", name);
                insCmd.Parameters.AddWithValue("@date", date);
                insCmd.Parameters.AddWithValue("@operation", completepo);
                insCmd.Parameters.AddWithValue("@id", id);
                int affectedRows = insCmd.ExecuteNonQuery();
                tblIn.Close();
            }
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();

                SqlCommand cmd4 = new SqlCommand("update itemCode set ponumber=@ponumber where icode=@icode", itemCode);
                ;
                cmd4.Parameters.AddWithValue("@icode", icode);
                cmd4.Parameters.AddWithValue("@ponumber", guna2TextBox1.Text);
                cmd4.ExecuteNonQuery();

                itemCode.Close();
            }
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("update tblIn set ponumber=@ponumber,sortid=@sortid where Id=@Id", tblIn);

                tblIn.Open();
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@ponumber", guna2TextBox1.Text);
                cmd.Parameters.AddWithValue("@sortid", Regex.Replace(guna2TextBox1.Text, @"[a-zA-Z]+", ""));
                cmd.ExecuteNonQuery();
                tblIn.Close();
                tblIn.Dispose();
            }

            label7.Text = "Exist";
            label7.Visible = false;
            label28.Visible = false;
            if (form == "prein")
            {
                obj.loadprein();
            }
            lo = true;
            getinfo(); 
            load();
            lo = false;
            count();
            MessageBox.Show("Edited Successfully...");
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click_1(object sender, EventArgs e)
        {
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if ((bool)button2.Visible == true || label27.Text == "Click Here to Save")
            {
                if (guna2TextBox1.Text != "")
                {

                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblIn] WHERE ([ponumber] = @ponumber)", tblIn);
                        check_User_Name.Parameters.AddWithValue("@ponumber", guna2TextBox1.Text);
                        int UserExist = (int)check_User_Name.ExecuteScalar();

                        if (UserExist > 0)
                        {
                            if (guna2TextBox1.Text == label1.Text)
                            {
                                label7.Text = "P.O is the same";
                                label7.Visible = true;
                            }
                            else
                            {
                                label7.Text = "P.O Number Exist";
                                label7.Visible = true;
                            }
                        }
                        else
                        {
                            label7.Text = "Exist";
                            label7.Visible = false;
                        }
                        tblIn.Close();
                        tblIn.Dispose();
                    }
                }
                else
                {
                    label7.Text = "Exist";
                    label7.Visible = false;
                }
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }

        private void label30_Click(object sender, EventArgs e)
        {
        }

        private void label29_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Visible == false)
            {
                MessageBox.Show("Please add Sales Invoice First.");
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void label4_Click_1(object sender, EventArgs e)
        {
            //if (guna2TextBox4.Text == "")
            //{
            //    MessageBox.Show("Please add Total Amount");
            //}
            //else
            //{
            //    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            //    {
            //        SqlCommand cmd = new SqlCommand("update tblIn set totalamount=@totalamount where Id=@Id", tblIn);
            //        tblIn.Open();
            //        cmd.Parameters.Clear();
            //        cmd.Parameters.AddWithValue("@Id", id);
            //        cmd.Parameters.AddWithValue("@totalamount", guna2TextBox4.Text);
            //        cmd.ExecuteNonQuery();
            //        tblIn.Close();
            //        tblIn.Dispose();
            //    }
            //    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            //    {
            //        DateTime date = DateTime.Now;
            //        string completepo = "changing Total Amount to " + guna2TextBox4.Text + "";
            //        tblIn.Open();
            //        string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
            //            " (@name,@date,@operation,@id)";
            //        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
            //        insCmd.Parameters.Clear();
            //        insCmd.Parameters.AddWithValue("@name", name);
            //        insCmd.Parameters.AddWithValue("@date", date.ToString("MM/dd/yyyy HH:mm:ss"));
            //        insCmd.Parameters.AddWithValue("@operation", completepo);
            //        insCmd.Parameters.AddWithValue("@id", id);
            //        int affectedRows = insCmd.ExecuteNonQuery();
            //        tblIn.Close();
            //    }
            //    MessageBox.Show("Done");
            //}
        }

        private void label29_Click_1(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {
        }
        private void label34_Click(object sender, EventArgs e)
        {
            if (label34.Text == "Edit")
            {
                label36.Visible = true;
                guna2TextBox2.Visible = true;
                label34.Text = "Save";
            }
            else
            {
                label36.Visible = false;
                guna2TextBox2.Visible = false;
                label34.Text = "Edit";

                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    SqlCommand cmd3 = new SqlCommand("update tblIn set suppliername=@suppliername where Id=@Id", tblIn);
                    tblIn.Open();
                    cmd3.Parameters.AddWithValue("@Id", id);
                    cmd3.Parameters.AddWithValue("@suppliername", guna2TextBox2.Text);
                    cmd3.ExecuteNonQuery();
                    tblIn.Close();
                }
                string completepo = "Updating Supplier Name";
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@date", date);
                    insCmd.Parameters.AddWithValue("@operation", completepo);
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblIn.Close();
                }
                MessageBox.Show("Updated");
                if (form == "prein")
                {
                    obj.loadprein();
                }
                lo = true;
                getinfo();
                lo = false;
                count();
            }
        }
        private void label35_Click(object sender, EventArgs e)
        {
            if (label35.Text == "Edit")
            {
                label37.Visible = true;
                guna2Panel2.Visible = true;
                label35.Text = "Save";
            }
            else
            {
                DateTime temp;
                if (DateTime.TryParse(maskedTextBox1.Text, out temp))
                {
                    // Yay :)
                }
                else
                {
                    MessageBox.Show("Invalid Date");
                    DateTime d = DateTime.Now;
                    maskedTextBox1.Text = d.ToString("MM/dd/yyyy");
                    return;
                }
                
                label37.Visible = false;
                guna2Panel2.Visible = false;
                label35.Text = "Edit";

                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    SqlCommand cmd3 = new SqlCommand("update tblIn set datetime=@datetime where Id=@Id", tblIn);
                    tblIn.Open();
                    cmd3.Parameters.AddWithValue("@Id", id);
                    cmd3.Parameters.AddWithValue("@datetime", maskedTextBox1.Text);
                    cmd3.ExecuteNonQuery();
                    tblIn.Close();
                }
                string completepo = "Updating Date";
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                        " (@name,@date,@operation,@id)";
                    SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                    insCmd.Parameters.AddWithValue("@name", name);
                    insCmd.Parameters.AddWithValue("@date", date);
                    insCmd.Parameters.AddWithValue("@operation", completepo);
                    insCmd.Parameters.AddWithValue("@id", id);
                    int affectedRows = insCmd.ExecuteNonQuery();
                    tblIn.Close();
                }
                MessageBox.Show("Updated");
                if (form == "prein")
                {
                    obj.loadprein();
                }
                lo = true;
                getinfo();
                lo = false;
                count();
            }
        }

        private void label37_Click(object sender, EventArgs e)
        {
            label37.Visible = false;
            guna2Panel2.Visible = false;
            label35.Text = "Edit";
        }

        private void label36_Click(object sender, EventArgs e)
        {

            label36.Visible = false;
            guna2TextBox2.Visible = false;
            label34.Text = "Edit";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();
                string insStmt = "insert into tblPending ([type], [docid]) values" +
                    " (@type,@docid)";
                SqlCommand insCmd = new SqlCommand(insStmt, otherDB);
                insCmd.Parameters.Clear();
                insCmd.Parameters.AddWithValue("@type", "PO");
                insCmd.Parameters.AddWithValue("@docid", id);
                int affectedRows = insCmd.ExecuteNonQuery();
                otherDB.Close();
            }
            if (form == "dashboard")
            {
                obj1.load();
                obj1.dt.AcceptChanges();
            }
            guna2Button1.Visible = false;
            guna2Button2.Visible = true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (form == "dashboard")
            {
                foreach (DataRow row in obj1.dt.Rows)
                {
                    if (row["docid"].ToString() == id)
                    {
                        row.Delete();
                        break;
                    }
                }
                obj1.dt.AcceptChanges();

            }
            using (SqlConnection otherDB = new SqlConnection(ConfigurationManager.ConnectionStrings["otherDB"].ConnectionString))
            {
                otherDB.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM tblPending WHERE type = 'PO' AND docid = '" + id + "'", otherDB))
                {
                    command.ExecuteNonQuery();
                }
                otherDB.Close();
            }
            guna2Button2.Visible = false;
            guna2Button1.Visible = true;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete? Once deleted cannot be undone.", "Delete", MessageBoxButtons.YesNo,MessageBoxIcon.Hand);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM itemCode WHERE ponumber = '" + label1.Text + "'", itemCode))
                    {
                        command.ExecuteNonQuery();
                    }
                    itemCode.Close();
                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblVP WHERE poid = '" + id + "'", tblIn))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblIn.Close();
                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblSI WHERE poid = '" + id + "'", tblIn))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblIn.Close();
                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblInHistory WHERE id = '" + id + "'", tblIn))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblIn.Close();
                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    tblIn.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM tblIn WHERE Id = '" + id + "'", tblIn))
                    {
                        command.ExecuteNonQuery();
                    }
                    tblIn.Close();
                }
                if (form == "prein")
                {
                    obj.loadprein();
                }
                MessageBox.Show("Record Deleted");
                this.Close();
            }
        }

        private void dataGridView4_Leave(object sender, EventArgs e)
        {
            dataGridView4.ClearSelection();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void guna2TextBox4_Leave(object sender, EventArgs e)
        {
           guna2TextBox4.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(guna2TextBox4.Text));
        }

        private void guna2TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            decimal x;
            if (ch == (char)Keys.Back)
            {
                e.Handled = false;
            }
            else if (!char.IsDigit(ch) && ch != '.' || !Decimal.TryParse(guna2TextBox4.Text + ch, out x))
            {
                e.Handled = true;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            pocompletionhistory po = new pocompletionhistory();
            po.id = id;
            po.ShowDialog();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (label1.Text == "")
            {
                MessageBox.Show("Sorry but you can't add an attachments if you don't have P.O. Number...");
            }
            else
            {
                scan s = new scan();
                s.idsup = id;
                s.num = "1";
                s.ShowDialog();
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (label38.Text == "" || label41.Text == "")
            {
                MessageBox.Show("Please add VAT and WHT first.");
            }
            else
            {
                Cursor.Current = Cursors.WaitCursor;
                itemlist i = new itemlist();
                i.icode = icode;
                i.name = name;
                i.ponumber = label1.Text;
                i.qty = label17.Text;
                i.totalitems = label18.Text;
                i.totalamount = guna2TextBox4.Text;
                i.Id = id;
                i.operation = operation;
                i.no = "1";
                Cursor.Current = Cursors.Default;
                i.ShowDialog();
            }
        }
        public string tax
        {
            get { return label41.Text; }
            set { label41.Text = value; }
        }
        public string vat
        {
            get { return label38.Text; }
            set { label38.Text = value; }
        }

        private void label43_Click(object sender, EventArgs e)
        {
            VATUPDATE a = new VATUPDATE(this);
            a.form = "insupply";
            a.id = id;
            a.name = name;
            a.ShowDialog();
        }

        

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (guna2Button7.Text == "Show Remarks")
            {
                remarks.Visible = false;
                si.Visible = true;
                guna2Button7.FillColor = Color.DodgerBlue;
                guna2Button7.Text = "Go Back";
                guna2Panel5.Visible = false;
                guna2Button12.FillColor = Color.FromArgb(47, 49, 66);

            }
            else
            {
                remarks.Visible = true;
                si.Visible = false;
                guna2Button7.FillColor = Color.FromArgb(47, 49, 66);
                guna2Button7.Text = "Show Remarks";
                guna2Panel5.Visible = false;
                guna2Button12.FillColor = Color.FromArgb(47, 49, 66);
            }



        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            button4.Visible = true;
            button2.Visible = true;
            button5.Visible = false;
            button3.Visible = false;

            guna2TextBox1.Visible = true;
            guna2TextBox2.Visible = true;
            //dataGridView2.ReadOnly = false;Column7
            dataGridView1.Columns["four"].ReadOnly = false;
            dataGridView1.Columns["eleven"].ReadOnly = false;
            dataGridView1.Columns["five"].ReadOnly = false;
            dataGridView1.Columns["ten"].ReadOnly = false;
            dataGridView1.Columns["delete"].Visible = true;
        }

        private void guna2Button9_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dataGridView1.Update();
            dataGridView1.Refresh();
            if (guna2TextBox2.Text == "")
            {
                MessageBox.Show("Please enter text completely in Supplier...");
            }
            else if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("You can't purchase without an item...");
            }
            else if (label7.Text == "P.O Number Exist")
            {
                MessageBox.Show("Existing P.O Number!!!");
            }
            else
            {
                try
                {
                    count();
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        string completepo = "Editing P.O information";

                        tblIn.Open();
                        string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                            " (@name,@date,@operation,@id)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@date", date);
                        insCmd.Parameters.AddWithValue("@operation", completepo);
                        insCmd.Parameters.AddWithValue("@id", id);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblIn.Close();
                    }
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("update tblIn set ponumber=@ponumber,suppliername=@suppliername,additionalinfo=@additionalinfo,qty=@qty,totalitems=@totalitems,totalamount=@totalamount,sortid=@sortid where Id=@Id", tblIn);

                        tblIn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@ponumber", guna2TextBox1.Text);
                        cmd.Parameters.AddWithValue("@suppliername", guna2TextBox2.Text);
                        cmd.Parameters.AddWithValue("@additionalinfo", guna2TextBox3.Text);
                        cmd.Parameters.AddWithValue("@qty", label17.Text);
                        cmd.Parameters.AddWithValue("@totalitems", label18.Text);
                        cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
                        cmd.Parameters.AddWithValue("@sortid", Regex.Replace(guna2TextBox1.Text, @"[a-zA-Z]+", ""));
                        cmd.ExecuteNonQuery();
                        tblIn.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();

                        SqlCommand cmd4 = new SqlCommand("update itemCode set ponumber=@ponumber where icode=@icode", itemCode);
                        ;
                        cmd4.Parameters.AddWithValue("@icode", icode);
                        cmd4.Parameters.AddWithValue("@ponumber", guna2TextBox1.Text);
                        cmd4.ExecuteNonQuery();

                        itemCode.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            using (SqlCommand command = new SqlCommand("DELETE FROM itemCode WHERE Id = '" + dataGridView2.Rows[i].Cells[0].Value + "'", itemCode))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                        itemCode.Close();
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show(string.Format("An error occurred: {0}", ex.Message));
                }

                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE itemCode SET qty=@qty,unit=@unit,total=@total,selling=@selling,cost=@cost WHERE Id='" + item.Cells[6].Value + "'", itemCode))
                        {
                            command.Parameters.Clear();
                            //command.Parameters.AddWithValue("@Id",);
                            command.Parameters.AddWithValue("@qty", item.Cells["four"].Value);
                            command.Parameters.AddWithValue("@unit", item.Cells["five"].Value);
                            command.Parameters.AddWithValue("@total", item.Cells["thirteen"].Value);
                            command.Parameters.AddWithValue("@selling", item.Cells["eleven"].Value);
                            command.Parameters.AddWithValue("@cost", item.Cells["ten"].Value);
                            command.ExecuteNonQuery();
                        }
                    }
                    itemCode.Close();
                    itemCode.Dispose();
                }
                if (form == "prein")
                {
                    obj.loadprein();
                }

                guna2TextBox1.Visible = false;
                guna2TextBox2.Visible = false;

                button4.Visible = false;
                button2.Visible = false;
                button5.Visible = true;
                button3.Visible = true;
                label27.Text = "Change P.O. No.";
                guna2TextBox1.Visible = false;
                label7.Visible = false;
                label28.Visible = false;
                label7.Text = "Exist";
                dataGridView1.Columns["four"].ReadOnly = true;
                dataGridView1.Columns["delete"].Visible = false;
                dataGridView1.Columns["five"].ReadOnly = true;
                dataGridView1.Columns["ten"].ReadOnly = true;
                lo = true;
                getinfo();
                load();
                lo = false;
                count();
                MessageBox.Show("Edited Successfully...");
            }
            Cursor.Current = Cursors.Default;
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {

            count();
            if (guna2TextBox1.Text == "" || label1.Text == "")
            {
                MessageBox.Show("P.O. number is empty");
                return;
            }
            salesinvoice f = new salesinvoice();
            f.num = "1";
            f.poid = id;
            f.name = name;
            f.itemid = itemid;
            f.ShowDialog();
        }

        private void guna2Button9_Click_2(object sender, EventArgs e)
        {
            if (label38.Text == "" || label41.Text == "")
            {
                MessageBox.Show("Please add VAT and WHT first.");
            }
            else
            {
                edititems r = new edititems();
                r.ponumber = label1.Text;
                r.id = id;
                r.ShowDialog();
            }
        }

        private void guna2Button9_Click_3(object sender, EventArgs e)
        {

            button4.Visible = false;
            button2.Visible = false;
            button5.Visible = true;
            button3.Visible = true;
            label7.Visible = false;
            lo = true;
            getinfo();
            load();
            lo = false;
            count();
            label27.Text = "Change P.O. No.";
            guna2TextBox1.Visible = false;
            label7.Visible = false;
            label28.Visible = false;
            label7.Text = "Exist";
            guna2TextBox1.Visible = false;
            label28.Visible = false;
            guna2TextBox2.Visible = false;
            dataGridView1.Columns["four"].ReadOnly = true;
            dataGridView1.Columns["delete"].Visible = false;
            dataGridView1.Columns["five"].ReadOnly = true;
        }

        private void label44_TextChanged(object sender, EventArgs e)
        {
            count();
        }

        private void label41_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("lbl4Textchange");
            count();
        }

        public string totalamoun
        {
            get { return guna2TextBox4.Text; }
            set { guna2TextBox4.Text = value; }
        }
        private void label46_Click(object sender, EventArgs e)
        {
            if (label38.Text != "")
            {
                AddDiscount a = new AddDiscount(this);
                a.ponumber = label1.Text;
                a.id = id;
                a.form = "insupply";
                a.name = name;

                if (dataGridView4.Rows.Count == 0)
                {
                    a.amountwithouttax = label44.Text;
                    a.ShowDialog();
                }
                else
                {
                    a.amountwithouttax = dataGridView4.Rows[dataGridView4.Rows.Count - 1].Cells["amount"].Value.ToString();
                    a.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Empty Amount");
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (dataGridView4.CurrentRow.Cells["disid"].Value.ToString() == "")
                {
                    DataGridViewRow dgvDelRow = dataGridView4.CurrentRow;
                    dataGridView4.Rows.Remove(dgvDelRow);
                }
                else
                {
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblDiscounts WHERE id = '" + dataGridView4.CurrentRow.Cells["disid"].Value.ToString() + "'", tblIn))
                        {
                            command.ExecuteNonQuery();
                        }
                        tblIn.Close();
                    }
                    string completepo = "Deleted discount of " + dataGridView4.CurrentRow.Cells["percent"].Value.ToString() + " Percent";
                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        tblIn.Open();
                        string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                            " (@name,@date,@operation,@id)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        insCmd.Parameters.AddWithValue("@operation", completepo);
                        insCmd.Parameters.AddWithValue("@id", id);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblIn.Close();
                    }
                    DataGridViewRow dgvDelRow = dataGridView4.CurrentRow;
                    dataGridView4.Rows.Remove(dgvDelRow);
                    count();

                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("update tblIn set totalamount=@totalamount where Id=@Id", tblIn);

                        tblIn.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@totalamount", Convert.ToDecimal(guna2TextBox4.Text));
                        cmd.ExecuteNonQuery();
                        tblIn.Close();
                    }
                }
            }
            

            
        }

        private void label47_Click(object sender, EventArgs e)
        {
            dataGridView4.Columns["dataGridViewButtonColumn1"].Visible = true;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            count();
        }

        private void dataGridView4_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            count();
        }

        private void label38_TextChanged(object sender, EventArgs e)
        {
            count();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            count();
        }

        private void dataGridView4_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            count();
        }

        private void dataGridView4_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            count();
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var n = Environment.NewLine;
            MessageBox.Show("Type: " + dataGridView4.CurrentRow.Cells["type"].Value.ToString() + n + n + "Amount: " + dataGridView4.CurrentRow.Cells["percent"].Value.ToString()+ n + "TOTAL: " + dataGridView4.CurrentRow.Cells["amount"].Value.ToString() + n +n+"Remarks: "+dataGridView4.CurrentRow.Cells["remarks1"].Value.ToString());
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
        }

        private void guna2Button9_Click_4(object sender, EventArgs e)
        {

        }

        private void guna2Button10_Click_1(object sender, EventArgs e)
        {

         
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            if (guna2Button12.Text == "More")
            {
                remarks.Visible = false;
                si.Visible = false;
                guna2Button12.Text = "Back";
                guna2Button7.Text = "Show Remarks";
                guna2Panel5.Visible = true;
                guna2Button7.FillColor = Color.FromArgb(47, 49, 66);
                guna2Button12.FillColor = Color.DodgerBlue;
            }
            else
            {
                remarks.Visible = true;
                si.Visible = false;
                guna2Button12.Text = "More";
                guna2Panel5.Visible = false;
                guna2Button7.FillColor = Color.FromArgb(47, 49, 66);
                guna2Button12.FillColor = Color.FromArgb(47, 49, 66);
            }
        }

        private void remarks_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click_1(object sender, EventArgs e)
        {

            infoVP f = new infoVP();
            f.poid = id;
            f.ShowDialog();
        }

        private void label30_Click_1(object sender, EventArgs e)
        {

            siinfo f = new siinfo();
            f.poid = id;
            f.itemid = itemid;
            f.ShowDialog();
        }

        private void guna2Button9_Click_5(object sender, EventArgs e)
        {
            if (pictureBox4.Visible == false)
            {
                MessageBox.Show("Please add Sales Invoice First.");
            }
            else
            {
                addvp f = new addvp();
                f.poid = id;
                f.name = name;
                f.ShowDialog();
            }
        }

        private void guna2Button11_Click_1(object sender, EventArgs e)
        {

            salesinvoice f = new salesinvoice();
            f.poid = id;
            f.name = name;
            f.itemid = itemid;
            f.ShowDialog();
        }
    }
}
