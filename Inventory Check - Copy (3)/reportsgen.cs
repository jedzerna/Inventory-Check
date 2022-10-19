using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Inventory_Check
{
    public partial class reportsgen : Form
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
        public reportsgen()
        {
            InitializeComponent();
            MyPrinter = new LPrinter();
        }

        LPrinter MyPrinter;
        DataTable dt = new DataTable("codeMaterial");
        private void reportsgen_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SuspendLayout();

            ChangeControlStyles(dgv1, ControlStyles.OptimizedDoubleBuffer, true);
            this.dgv1.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dgv1.ColumnHeadersDefaultCellStyle.BackColor;
            dgv1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgv1.RowHeadersVisible = false;


            ChangeControlStyles(dgv4, ControlStyles.OptimizedDoubleBuffer, true);
            this.dgv4.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.dgv4.ColumnHeadersDefaultCellStyle.BackColor;
            dgv4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dgv4.RowHeadersVisible = false;

            dt.Columns.Add("amount", typeof(String));
            loadsupply();



      


            //need to set value to NewColumn column
            // or set it to some other value
        
            ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        public string name = "Jed Zerna";
        private void ChangeControlStyles(Control ctrl, ControlStyles flag, bool value)
        {
            MethodInfo method = ctrl.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
                method.Invoke(ctrl, new object[] { flag, value });
        }
        public void loadsupply()
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
       
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private Thread workerThread = null;

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
            
        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
           
        }
        string lb = "{";
        string rb = "}";
        private void guna2Button4_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2ProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btProjSummary_Click(object sender, EventArgs e)
        {
            if (cmSV.Text == "")
            {
                MessageBox.Show("Please Select Sales Voucher");
            }
            else
            {
                guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressIndicator1.Start();
                    guna2ProgressIndicator1.Visible = true;
                });
                System.Threading.Thread thread =
              new System.Threading.Thread(new System.Threading.ThreadStart(first));
                thread.Start();
            }
        }
        DateTime dfrom;
        DateTime dto;
        DataTable d1t = new DataTable();
        private void first()
        {
            dgv1.BeginInvoke((Action)delegate ()
            {
                dgv4.BeginInvoke((Action)delegate ()
                {
                    dgv1.Rows.Clear();

                    DateTime dat = DateTime.Now;
                    string date = dat.ToString("MM/dd/yyyy");
                    dgv4.Rows.Clear();
                    d1t.Rows.Clear();
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        DateTime from = datetime1.Value;
                        DateTime to = datetime2.Value;
                        dfrom = datetime1.Value;
                        dto = datetime2.Value;

                        dbDR.Open();
                        string list = "SELECT drnumber,datetime,projectcode,projectname,Id,totalamount FROM tblDR WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND sv = '" + cmSV.Text + "' ORDER BY projectcode,drnumber ASC";
                        SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
                        command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                        command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                        command.Fill(d1t);
                        foreach (DataRow item in d1t.Rows)
                        {
                            int c = dgv4.Rows.Add();
                            dgv4.Rows[c].Cells["drnumber"].Value = item["drnumber"].ToString();
                            dgv4.Rows[c].Cells["datetime"].Value = item["datetime"].ToString();
                            dgv4.Rows[c].Cells["projectcode"].Value = item["projectcode"].ToString();
                            dgv4.Rows[c].Cells["projectname"].Value = item["projectname"].ToString();
                            dgv4.Rows[c].Cells["Id"].Value = item["Id"].ToString();
                            dgv4.Rows[c].Cells["totalamount2"].Value = item["totalamount"].ToString();
                        }
                        dbDR.Close();
                        dgv4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                        dgv4.RowHeadersVisible = false;
                    }
                    compute();
                });
            });
        }

        private void compute()
        {
            dgv1.BeginInvoke((Action)delegate ()
            {
                foreach (DataGridViewRow row in dgv4.Rows)
                {
                    RemoveDuplicateRows(d1t, "projectcode");
                }
                foreach (DataRow item in d1t.Rows)
                {
                    int c = dgv1.Rows.Add();
                    dgv1.Rows[c].Cells["cl13"].Value = item["projectcode"].ToString();
                    dgv1.Rows[c].Cells["cl2"].Value = item["projectname"].ToString();
                    decimal sum = 0.00M;
                    foreach (DataGridViewRow row in dgv4.Rows)
                    {
                        if (row.Cells["projectcode"].Value.ToString() == item["projectcode"].ToString())
                        {
                            sum += Convert.ToDecimal(row.Cells["totalamount2"].Value.ToString());
                        }
                    }
                    dgv1.Rows[c].Cells["totalamount"].Value = Math.Round(sum, 2).ToString();
                }
                int a = dgv4.Rows.Count;
                int b = 0;
                foreach (DataGridViewRow rrow in dgv4.Rows)
                {
                    foreach (DataGridViewRow gv1 in dgv1.Rows)
                    {
                        if (gv1.Cells["cl13"].Value.ToString() == rrow.Cells["projectcode"].Value.ToString())
                        {
                            rrow.Cells["subtotal"].Value = gv1.Cells["totalamount"].Value.ToString();
                        }

                    }
                }
                //string pname = "";
                //foreach (DataGridViewRow row in dgv4.Rows)
                //{
                //    if (pname != row.Cells["projectcode"].Value.ToString())
                //    {
                //        foreach (DataGridViewRow gv1 in dgv1.Rows)
                //        {
                //            if (row.Cells["projectcode"].Value.ToString() == gv1.Cells["cl13"].Value.ToString())
                //            {
                //                row.Cells["subtotal"].Value = gv1.Cells["totalamount"].Value.ToString();
                //            }
                //        }
                //        pname = row.Cells["projectcode"].Value.ToString();
                //    }
                //}
                int rr = 1;
                for (int r = 0; r < dgv4.Rows.Count; r++)
                {
                    rr++;
                    if (rr < dgv4.Rows.Count)
                    {
                        if (dgv4.Rows[r].Cells["projectcode"].Value.ToString() == dgv4.Rows[r + 1].Cells["projectcode"].Value.ToString())
                        {
                            dgv4.Rows[r].Cells["subtotal"].Value = "";
                        }
                        else
                        {
                            foreach (DataGridViewRow gv1 in dgv1.Rows)
                            {
                                if (dgv4.Rows[r].Cells["projectcode"].Value.ToString() == gv1.Cells["cl13"].Value.ToString())
                                {
                                    dgv4.Rows[r].Cells["subtotal"].Value = gv1.Cells["totalamount"].Value.ToString();
                                }
                            }
                        }
                    }
                }

            });


            guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressIndicator1.Stop();
                guna2ProgressIndicator1.Visible = false;
            });
            //int rowc = 0;
            //foreach (DataGridViewRow row in dgv4.Rows)
            //{
            //    rowc++;
            //    MessageBox.Show(rowc.ToString());
            //}
            //int rowc = 0;
            //int rowcount = dgv4.Rows.Count;
            //foreach (DataGridViewRow row in dgv4.Rows)
            //{
            //    rowc++;
            //    string sub = "";
            //    if (rowcount > rowc)
            //    {
            //        MessageBox.Show(dgv4.Rows[rowc].Cells["projectcode"].Value.ToString());
            //        if (dgv4.Rows[rowc].Cells["projectcode"].Value.ToString() == row.Cells["projectcode"].Value.ToString())
            //        {


            //            sub = "";

            //        }
            //        else
            //        {
            //            sub = string.Format("{0:#,##0.00}", Convert.ToDecimal(row.Cells["subtotal"].Value.ToString()));
            //        }
            //    }


            //}
        }
        string fronttotal;
        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            foreach (DataRow dtRow in dTable.Rows)
            {
                if (dtRow[colName].ToString() != "")
                {
                    if (hTable.Contains(dtRow[colName]))
                        duplicateList.Add(dtRow);
                    else
                        hTable.Add(dtRow[colName], string.Empty);
                }
            }
            foreach (DataRow dtRow in duplicateList)
                dTable.Rows.Remove(dtRow);

            return dTable;
        }
        //private void copyAlltoClipboard2()
        //{
        //    dgv2.SelectAll();
        //    DataObject dataObj = dgv2.GetClipboardContent();
        //    if (dataObj != null)
        //        Clipboard.SetDataObject(dataObj);
        //}
       

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            printprojsumfront p = new printprojsumfront();
            p.Show();
            projectsumdata b = new projectsumdata();
            b.Show();
        }

        private void btProjsumexcel_Click(object sender, EventArgs e)
        {

            //copyAlltoClipboard2();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void dgv2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
           
        }
        string pcode;
        int pp = 0;
        int nxtp = 51;
        int page = 1;
        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
             guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressIndicator1.Start();
                guna2ProgressIndicator1.Visible = true;
            });
            if (dgv4.Rows.Count == 0)
            {
                MessageBox.Show("No Records Found");
                guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
                {
                    guna2ProgressIndicator1.Stop();
                    guna2ProgressIndicator1.Visible = false;
                });
            }
            else
            {
              
                if (MyPrinter.pd.ShowDialog() == DialogResult.OK)
                {

                    MyPrinter.ps = MyPrinter.pd.PrinterSettings;
                }
                else
                {
                    guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
                    {
                        guna2ProgressIndicator1.Stop();
                        guna2ProgressIndicator1.Visible = false;
                    });
                    return;
                }
                cmsv = cmSV.Text;
                System.Threading.Thread thread =
              new System.Threading.Thread(new System.Threading.ThreadStart(printprojectsummary));
                thread.Start();
            }
            
        }
        string cmsv;
        private void guna2Button7_Click(object sender, EventArgs e)
        {

        }
        private void printing()
        {
            string pname = "";
            int subtotal = 0;
            string subtotalstring = "";
            int rowc = 1;
            int crow = dgv4.Rows.Count;
            int increcount =1;
            Int32 count = 0;
            int rowcount = dgv4.Rows.Count;
            foreach (DataGridViewRow row in dgv4.Rows)
            {
                rowc++;
                //MessageBox.Show(rowc.ToString());
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    if (pname != row.Cells["projectcode"].Value.ToString())
                    {
                        subtotalstring = "";
                        MyPrinter.Print("\r\n");
                        MyPrinter.Print((char)15+ "      " + row.Cells["projectcode"].Value.ToString().PadRight(11, ' ') + "***     " + row.Cells["projectname"].Value.ToString() + "     ***\r\n");
                        pp++;
                        pp++;
                        subtotal = 0;
                        //rowfix = 0;
                        //rowfix1 = 0;
                        foreach (DataGridViewRow rowdgv1 in dgv1.Rows)
                        {
                            if (rowdgv1.Cells["cl13"].Value.ToString() == row.Cells["projectcode"].Value.ToString())
                            {
                                subtotalstring = rowdgv1.Cells["totalamount"].Value.ToString();
                            }
                        }
                        itemCode.Open();
                        SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tblDRitemCode WHERE drid =  '" + row.Cells["Id"].Value.ToString() + "'", itemCode);
                        count = (Int32)cmd.ExecuteScalar();
                        increcount = 1;
                        itemCode.Close();
                    }
                    int rowfix = 0;
                    int rowfix1 = 0;

                    bool difnxtrow = false;
                    if (rowcount > rowc)
                    {
                        if (dgv4.Rows[rowc].Cells["projectcode"].Value.ToString() == row.Cells["projectcode"].Value.ToString())
                        {


                            difnxtrow = false;

                        }
                        else
                        {

                            difnxtrow = true;
                        }
                    }
                    DateTime fromdate = DateTime.Parse(row.Cells["datetime"].Value.ToString());
                    string ffromdate = fromdate.ToString("MM/dd/yyyy");
                    MyPrinter.Print((char)27+ "E"+"        " + ffromdate.PadRight(11, ' ') + "   " + row.Cells["drnumber"].Value.ToString() + (char)27 + "F" +"\r\n");
                    pp++;
                    itemCode.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT qty,unit,description,selling,total,productcode FROM tblDRitemCode WHERE drid =  '" + row.Cells["Id"].Value.ToString() + "'", itemCode);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //itemCode.Open();
                    SqlCommand cmds = new SqlCommand("SELECT COUNT(*) FROM tblDRitemCode WHERE drid =  '" + row.Cells["Id"].Value.ToString() + "'", itemCode);
                    int DBRowcount = (Int32)cmds.ExecuteScalar();
                    //increcount = 1;
                    //itemCode.Close();

                    int dbrow = 0;
                    foreach (DataRow item in dt.Rows)
                    {
                        int jed = 0;
                        dbrow++;
                        string nullval = "";
                        string desc = item["description"].ToString();
                        int chunkSize = 50;
                        int stringLength = desc.Length;
                        increcount++;
                        string total = "";
                        string selling = "";

                        if (item["total"].ToString() == "" || item["total"] == null)
                        {
                            total = "";
                        }
                        else
                        {
                            total = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["total"].ToString()));
                        }

                        if (item["selling"].ToString() == "" || item["selling"] == null)
                        {
                            selling = "";
                        }
                        else
                        {
                            selling = string.Format("{0:#,##0.00}", Convert.ToDecimal(item["selling"].ToString()));
                        }


                        string sub = "";


                        if (dbrow == DBRowcount)
                        {
                            if (row.Cells["subtotal"].Value.ToString() != "")
                            {
                                sub = string.Format("{0:#,##0.00}", Convert.ToDecimal(row.Cells["subtotal"].Value.ToString()));
                            }
                            else
                            {

                                sub = "";
                            }
                        }
                        else
                        {
                            sub = "";
                        }




                        for (int i = 0; i < stringLength; i += chunkSize)
                        {
                            jed++;
                            //rowfix1++;
                            if (i + chunkSize > stringLength) chunkSize = stringLength - i;

                            if (jed == 1)
                            {
                                    MyPrinter.Print((char)15 + "   " + item["qty"].ToString().PadLeft(13, ' ') + " " + item["unit"].ToString().PadRight(7, ' ') + " " + item["productcode"].ToString().PadLeft(13, ' ') + "  " + desc.Substring(i, chunkSize).PadRight(50, ' ') + selling.PadLeft(11, ' ') + total.PadLeft(16, ' ') + sub.PadLeft(20, ' ') + "\r\n");
                            }
                            else
                            {
                                    MyPrinter.Print((char)15 + "   " + nullval.PadLeft(13, ' ') + "    " + nullval.PadRight(7, ' ') + " " + nullval.PadLeft(10, ' ') + "  " + desc.Substring(i, chunkSize).PadRight(50, ' ') + nullval.PadLeft(11, ' ') + nullval.PadLeft(16, ' ') + nullval.PadLeft(20, ' ') + "\r\n");
                            }
                            pp++;
                            if (pp == nxtp)
                            {
                                pp = 0;
                                nxtp = 51;
                                page++;
                                MyPrinter.Print("\x0C");
                                MyPrinter.Print("\r\n");
                                MyPrinter.Print("\r\n");
                                MyPrinter.Print("\r\n");
                                DateTime date = DateTime.Now;
                                string fdate = date.ToString("MM/dd/yyyy");
                                string pagenum = "Page " + page.ToString();
                                string dateprint = "Date printed: " + fdate;
                                MyPrinter.Print((char)15 + "      " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");
                                MyPrinter.Print("\r\n");
                                MyPrinter.Print("\r\n");
                                MyPrinter.Print((char)15 + "                                  Code                                                     Unit Price          Amount            Subtotal" + "\r\n");
                            }
                        }
                    }
                    itemCode.Close();
                    pname = row.Cells["projectcode"].Value.ToString();
                }
            }
            MyPrinter.Print("\r\n");
            pp++;
            MyPrinter.Print("\r\n");
            pp++;
            MyPrinter.Print("\r\n");
            pp++;
            MyPrinter.Print("\r\n");
            pp++;
            if (pp == nxtp)
            {
                pp = 0;
                nxtp = 51;
                page++;
                MyPrinter.Print("\x0C");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                DateTime date = DateTime.Now;
                string fdate = date.ToString("MM/dd/yyyy");
                string pagenum = "Page " + page.ToString();
                string dateprint = "Date printed: " + fdate;
                MyPrinter.Print((char)15 + "      " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                MyPrinter.Print((char)15 + "                                  Code                                                     Unit Price          Amount            Subtotal" + "\r\n");
                MyPrinter.Print("\r\n");
                string blank = "";
                MyPrinter.Print((char)15 + "    " + blank.PadLeft(13, ' ') + " " + blank.PadRight(7, ' ') + " " + blank.PadLeft(13, ' ') + " " + blank.PadRight(37, ' ') + "***     TOTAL AMOUNT     ***   " + string.Format("{0:#,##0.00}", Convert.ToDecimal(fronttotal)).PadLeft(20, ' ') + "\r\n");
            }
            else
            {
                string blank = "";
                MyPrinter.Print((char)15 + "    " + blank.PadLeft(13, ' ') + " " + blank.PadRight(7, ' ') + " " + blank.PadLeft(13, ' ') + " " + blank.PadRight(37, ' ')  + "***     TOTAL AMOUNT     ***" + string.Format("{0:#,##0.00}", Convert.ToDecimal(fronttotal)).PadLeft(20, ' ') + "\r\n");
            }
            guna2ProgressIndicator1.BeginInvoke((Action)delegate ()
            {
                guna2ProgressIndicator1.Stop();
                guna2ProgressIndicator1.Visible = false;
            });
        }
        private void printprojectsummary()
        {

            decimal sum = 0.00M;
            for (int i = 0; i < dgv4.Rows.Count; ++i)
            {
                sum += Convert.ToDecimal(dgv4.Rows[i].Cells["totalamount2"].Value);
            }
            fronttotal = string.Format("{0:#,##0.00}", sum);
            if (!MyPrinter.Open("Reports")) return;
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "                                                     S. UYMATIAO JR. CONSTRUCTION" + "\r\n");
            MyPrinter.Print((char)15 + "                                                            SALES VOUCHER");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");

            
                MyPrinter.Print((char)15 + "             ITEM: " + cmsv.PadRight(21, ' ') + "                                                           SV #" + "\r\n");
          
            DateTime dat = DateTime.Parse(datetime2.Value.ToString());
            string fdat = dat.ToString("MM/dd/yyyy");
            MyPrinter.Print((char)15 + "                                                                                                   DATE: " + fdat.ToString() + "\r\n");
            MyPrinter.Print((char)15 + "             ============================================================================================================" + "\r\n");
            MyPrinter.Print("\r\n");
            DateTime fromdate = DateTime.Parse(datetime1.Value.ToString());
            string ffromdate = fromdate.ToString("MM/dd");
            DateTime todate = DateTime.Parse(datetime2.Value.ToString());
            string ttitle = "";
            string ttodate = todate.ToString("MM/dd"); 
          
                ttitle = "To record Sales of " + cmsv + " period " + ffromdate + "-" + ttodate;
           
            MyPrinter.Print((char)15 + "                                        " + ttitle + "\r\n");
            MyPrinter.Print("\r\n");
            int pp = 0;
            foreach (DataGridViewRow row in dgv1.Rows)
            {
                string pcode = "";
                if (string.IsNullOrEmpty(row.Cells["cl13"].Value as string))
                {
                    pcode = "";
                }
                else
                {
                    pcode = row.Cells["cl13"].Value.ToString();
                }
                string pname = "";
                if (string.IsNullOrEmpty(row.Cells["cl2"].Value as string))
                {
                    pname = "";
                }
                else
                {
                    pname = row.Cells["cl2"].Value.ToString();
                }
                string amount = "";
                if (string.IsNullOrEmpty(row.Cells["totalamount"].Value as string))
                {
                    amount = "";
                }
                else
                {
                    amount = row.Cells["totalamount"].Value.ToString();
                }
                if (pname.Length >= 35)
                {
                    int r = pname.Length - 35;
                    MyPrinter.Print((char)15 + "                               " + pcode.PadRight(13, ' ') + pname.Substring(0, pname.Length - r).PadRight(35, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(amount)).PadLeft(15, ' ') + "\r\n");
                    MyPrinter.Print((char)15 + "                               " + pcode.PadRight(13, ' ') + pname.Substring(35).PadRight(35, ' ') + "\r\n");
                    pp++;
                    pp++;
                }
                else
                {
                    MyPrinter.Print((char)15 + "                               " + pcode.PadRight(13, ' ') + pname.PadRight(40, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(amount)).PadLeft(15, ' ') + "    " + "\r\n");
                    pp++;
                }
            }
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
         
                MyPrinter.Print((char)15 + "                                         " + pcode + " Sales of " + cmsv.PadRight(35, ' ') + fronttotal + "\r\n");
           
                pp = pp + 3;
            int footer = 0;
            if (pp <= 41)
            {
                footer = 41 - pp;
            }
            for (int i = 0; i < footer; i++)
            {
                MyPrinter.Print("\r\n");
            }
            MyPrinter.Print((char)15 + "             ============================================================================================================" + "\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "             Prepared by: " + name.PadRight(23, ' ') + "                                               Checked by:___________________" + "\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "             Entered by:____________________" + "                                                   Approved by:___________________" + "\r\n");
            MyPrinter.Print("\x0C");
            MyPrinter.Close();



            if (!MyPrinter.Open("Summary")) return;

            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            DateTime date = DateTime.Now;
            string fdate = date.ToString("MM/dd/yyyy");
            string pagenum = "Page " + page.ToString();
            string dateprint = "Date printed: " + fdate;
            MyPrinter.Print((char)15 + "      " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");
          
                MyPrinter.Print((char)15 + "      Project Summary Report: " + cmsv + "     for " + ffromdate + " to " + ttodate + "\r\n");
       
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "                                  Code                                                     Unit Price          Amount            Subtotal" + "\r\n");
            //MyPrinter.Print("\r\n");

            printing();
            MyPrinter.Print("\x0C");
            MyPrinter.Close();
            pp = 0;
            nxtp = 51;
        }
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
        private void cmSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Lumber
            //CHB
            //Sand & Gravel
            //Ready - Mixed Conc
            //  Oxygen
            //Direct Sales
            if (cmSV.Text == "Construction Material")
            {
                pcode = "416.04";
            }
            else if (cmSV.Text == "Lumber")
            {
                pcode = "416.01";
            }
            else if (cmSV.Text == "Sand & Gravel")
            {
                pcode = "416.05";
            }
            else if (cmSV.Text == "CHB")
            {
                pcode = "416.02";
            }
            else
            {
                pcode = "000.00";
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {
            //int page = 1;
            //foreach (DataGridViewRow row in dgv4.Rows)
            //{
            //    MessageBox.Show("2");
            //    rowc++;
            //    if (rowc+1 <= crow)
            //    {
            //        MessageBox.Show(dgv4.Rows[rowc].Cells["projectcode"].Value.ToString());
            //    }

            //}
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {


        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ProgressIndicator4_Click(object sender, EventArgs e)
        {

        }
       
        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            DateTime temp;
            if (DateTime.TryParse(guna2DateTimePicker1.Text, out temp))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Invalid Date");
                DateTime d = DateTime.Now;
                guna2DateTimePicker1.Text = d.ToString("MM/dd/yyyy");
                return;
            }
            DateTime tempt;
            if (DateTime.TryParse(guna2DateTimePicker2.Text, out tempt))
            {
                // Yay :)
            }
            else
            {
                MessageBox.Show("Invalid Date");
                DateTime d = DateTime.Now;
                guna2DateTimePicker2.Text = d.ToString("MM/dd/yyyy");
                return;
            }
            printPreviewVP p = new printPreviewVP();
            p.datefrom = guna2DateTimePicker1.Text;
            p.dateto = guna2DateTimePicker2.Text;
            p.ShowDialog();
        }

        private void guna2Button5_Click_2(object sender, EventArgs e)
        {
            try
            {

                string path = Directory.GetCurrentDirectory() + "\\GLU1N.DBF";
                string folderpath = Directory.GetCurrentDirectory();
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                MessageBox.Show(Directory.GetCurrentDirectory() + "\\GLU1N.DBF");
                string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + folderpath + "';Extended Properties=dBase IV";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                using (OleDbCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "CREATE TABLE GLU1N (GG Integer, Changed Double, Name Text)";
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            printViewDRSum p = new printViewDRSum();
            p.datefrom = datetime1.Text;
            p.dateto = datetime2.Text;
            p.sv = cmSV.Text;
            //MessageBox.Show(datetime1.Text);
            //MessageBox.Show(datetime2.Text);
            p.ShowDialog();
        }

        private void guna2DateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button6_Click_1(sender, e);
            }
        }

        private void guna2DateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2Button6_Click_1(sender, e);
            }
        }
    }
}
