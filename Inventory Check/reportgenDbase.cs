using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class reportgenDbase : Form
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
        public string type;
        public DateTime date1;
        public DateTime date2;
        public string name;
        public string ITEM;
        public string TITLE;
        public string TITLELAST;
        public string CODE;
        public reportgenDbase()
        {
            InitializeComponent();
            MyPrinter = new LPrinter();
        }

        LPrinter MyPrinter;
        private void reportgenDbase_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(date1.ToString());
            if (type == "")
            {
                MessageBox.Show("Type is empty");
                this.Close();
            }
            loaddistinttype();
            if (type == "CMDS")
            {
                guna2CheckBox2.Visible = true;
                guna2CheckBox3.Visible = true;
                guna2CheckBox4.Visible = false;
                guna2CheckBox5.Visible = false;
                guna2Button1.Visible = true;
            }
            else
            {
                guna2Button1.Visible = true;
                guna2CheckBox4.Visible = true;
                guna2CheckBox3.Visible = false;
                guna2CheckBox2.Visible = false;
                guna2CheckBox5.Visible = true;
            }
            if (type == "CMDS")
            {
                ITEM = "Const. Mats.-Direct Sales";
                TITLE = "To record Direct Sales for the period";
                TITLELAST = "CM - Direct Sales";
                CODE = "416.04";
            }
            else if (type == "CHB")
            {
                ITEM = "Concrete Hollow Blocks";
                TITLE = "To record Sales of CHB period";
                /*Sales of */
                TITLELAST = "CHB";
                CODE = "416.02";

            }
            else if (type == "CMCM")
            {

                MessageBox.Show("Type not availble");
                this.Close();
            }
            else if (type == "MRSG")
            {
                ITEM = "S/G-Ready Mixed";
                TITLE = "To record Sales of Ready Mixed Conc. period";
                /*Sales of */
                TITLELAST = "Sand/Gravel";
                CODE = "416.05";
            }

            else if (type == "FOL")
            {
                ITEM = "Oxygen & Accetylene";
                TITLE = "To record Sales of Oxygen/Accetylene period";
                /*Sales of */
                TITLELAST = "Oxygen & Accetylene";
                CODE = "416.07";
            }
            else if (type == "SG")
            {
                ITEM = "Sand/Gravel";
                TITLE = "To record Sales of Sand/Gravel period";
                /*Sales of */
                TITLELAST = "Sand/Gravel";
                CODE = "416.05";
            }
            else if (type == "L")
            {
                ITEM = "Lumber";
                TITLE = "To record Sales of Lumber period";
                /*Sales of */
                TITLELAST = "Lumber";
                CODE = "416.01";
            }
            else if (type == "CM")
            {
                ITEM = "Construction Material";
                TITLE = "To record Sales of Construction Material period";
                /*Sales of */
                TITLELAST = "Construction Material";
                CODE = "416.04";
            }
            else
            {
                MessageBox.Show("Type not availble");
                this.Close();
            }


            label2.Text = ITEM;



        }
        DataTable dtdistincttype = new DataTable();
        public void loaddistinttype()
        {
            //MessageBox.Show(date1.ToString("MM/dd/yyyy"));
            dtdistincttype.Rows.Clear();
            //dt.Rows.Clear();

            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string list = "SELECT DISTINCT PROJCODE FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                SqlDataReader reader = command.ExecuteReader();
                dtdistincttype.Load(reader);
                dbDR.Close();
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("PROJCODE");
            dt.Columns.Add("SUBCODE");
            dt.Columns.Add("NAME");
            foreach (DataRow row in dtdistincttype.Rows)
            {
                object[] o = { row["PROJCODE"].ToString(), "", "" };
                dt.Rows.Add(o);
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    DataTable dtss = new DataTable();
                    dbDR.Open();
                    string list = "SELECT DISTINCT SUBCODE FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 AND PROJCODE = '" + row["PROJCODE"].ToString() + "'";
                    SqlCommand command = new SqlCommand(list, dbDR);
                    command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                    command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                    SqlDataReader reader = command.ExecuteReader();
                    dtss.Load(reader);
                    if (dtss.Rows.Count != 0)
                    {
                        foreach (DataRow dtrow in dtss.Rows)
                        {
                            object[] b = { "", dtrow["SUBCODE"].ToString(), "" };
                            dt.Rows.Add(b);
                        }
                    }
                    dbDR.Close();
                }
            }
            dt.Columns.Add("TOTAL");
            string project = "";
            foreach (DataRow row in dt.Rows)
            {
                if (row["PROJCODE"].ToString() != "")
                {
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        string list = "SELECT SUM(AMOUNT) FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 AND PROJCODE = '" + row["PROJCODE"].ToString() + "'";
                        SqlCommand command = new SqlCommand(list, dbDR);
                        command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));

                        decimal count = (decimal)command.ExecuteScalar();
                        dbDR.Close();
                        row["TOTAL"] = count;
                    }
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + row["PROJCODE"].ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, dbDR);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            row["NAME"] = (rdr["ACCTDESC"].ToString());
                        }
                        dbDR.Close();
                    }
                    row["SUBCODE"] = "";
                    project = row["PROJCODE"].ToString();
                }
                else if (row["SUBCODE"].ToString() != "" && project.Trim() != "")
                {
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        string list = "SELECT SUM(AMOUNT) FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 AND PROJCODE = '" + project + "' AND SUBCODE = '" + row["SUBCODE"].ToString() + "'";
                        SqlCommand command = new SqlCommand(list, dbDR);
                        command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));

                        decimal count = (decimal)command.ExecuteScalar();
                        dbDR.Close();
                        row["TOTAL"] = count;
                    }
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + row["SUBCODE"].ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, dbDR);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            row["NAME"] = (rdr["ACCTDESC"].ToString());
                        }
                        dbDR.Close();
                    }
                    row["PROJCODE"] = "";
                }
            }
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt.Rows[i];
                if (row["PROJCODE"].ToString().Trim() == "" && row["NAME"].ToString().Trim() == "" && row["SUBCODE"].ToString().Trim() == "")
                    row.Delete();
            }
            dt.AcceptChanges();
            foreach (DataRow row in dt.Rows)
            {
                int a = dataGridView1.Rows.Add();
                dataGridView1.Rows[a].Cells["PROJCODE"].Value = row["PROJCODE"].ToString();
                dataGridView1.Rows[a].Cells["SUBCODE"].Value = row["SUBCODE"].ToString();
                dataGridView1.Rows[a].Cells["NAMES"].Value = row["NAME"].ToString();
                dataGridView1.Rows[a].Cells["TOTAL"].Value = row["TOTAL"].ToString();
            }
                //dataGridView1.DataSource = dt;
            label1.Text = dt.Rows.Count.ToString();
        }
        //DataTable dt = new DataTable();
        //private void load()
        //{
        //    dt.Rows.Clear();
        //    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
        //    {
        //        dbDR.Open();
        //        string list = "SELECT * FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DATE,PROJCODE,SUBCODE ASC";
        //        SqlCommand command = new SqlCommand(list, dbDR);
        //        command.Parameters.AddWithValue("@date1", date1.ToString());
        //        command.Parameters.AddWithValue("@date2", date2.ToString());
        //        SqlDataReader reader = command.ExecuteReader();
        //        dt.Load(reader);
        //        dbDR.Close();
        //        dataGridView1.DataSource = dt;
        //    }
        //}
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        string pcode;
        int nxtp = 51;
        int page = 1;
        int pp = 0;
        int rowinpage = 0;
        string titleheader;

        DateTime date = DateTime.Now;
        string fdate;
        int ppage;
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (type == "CMDS")
            {
                if (guna2CheckBox1.Checked == false && guna2CheckBox2.Checked == false && guna2CheckBox3.Checked == false)
                {
                    MessageBox.Show("Please choose what to print.");
                    return;
                }
            }
            else
            {
                if (guna2CheckBox1.Checked == false && guna2CheckBox5.Checked == false && guna2CheckBox4.Checked == false)
                {
                    MessageBox.Show("Please choose what to print.");
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            if (MyPrinter.pd.ShowDialog() == DialogResult.OK)
            {
                MyPrinter.ps = MyPrinter.pd.PrinterSettings;
            }
            else
            {
                return;
            }

            if (type == "CMDS")
            {
                nxtp = 51;
                page = 1;
                pp = 0;
                rowinpage = 0;
                titleheader = "";
                ppage = 0;
                fdate = "";
                if (guna2CheckBox1.Checked)
                {
                    printprojectsummary();
                }
                if (guna2CheckBox2.Checked)
                {
                    printsummary();
                }
                if (guna2CheckBox3.Checked)
                {
                    sumsummary();
                }
                Cursor.Current = Cursors.Default;
            }
            else
            {

                nxtp = 51;
                page = 1;
                pp = 0;
                rowinpage = 0;
                titleheader = "";
                ppage = 0;
                fdate = "";
                if (guna2CheckBox1.Checked)
                {
                    othersprintprojectsummary();
                }
                if (guna2CheckBox4.Checked)
                {
                    otherssumsummary();
                }
                if (guna2CheckBox5.Checked)
                {
                    print();
                }
            }
            this.Close();
        }
        private void printprojectsummaryheader()
        {
            string ffromdate = date1.ToString("MM/dd");
            string ttodate = date2.ToString("MM/dd");
            //string ttodate = todate.ToString("MM/dd");
            MyPrinter.Print("\r\n");

            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "                                                     S. UYMATIAO JR. CONSTRUCTION" + "\r\n");
            MyPrinter.Print((char)15 + "                                                            SALES VOUCHER");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");

            MyPrinter.Print((char)15 + "             ITEM: " + ITEM.PadRight(21, ' ') + "                                                         SV # _____________" + "\r\n");

            DateTime dat = DateTime.Parse(date2.ToString());
            string fdat = dat.ToString("MM/dd/yyyy");
            MyPrinter.Print((char)15 + "                                                                                                   DATE: " + fdat.ToString() + "\r\n");
            MyPrinter.Print((char)15 + "             ============================================================================================================" + "\r\n");
            MyPrinter.Print("\r\n");

            string ttitle = TITLE + " " + ffromdate + "-" + ttodate + ".";
            MyPrinter.Print((char)15 + "                                        " + ttitle + "\r\n");
            MyPrinter.Print("\r\n");

        }
        private void printprojectsummary()
        {

            decimal sum = 0.00M;

            if (!MyPrinter.Open("Reports")) return;
            printprojectsummaryheader();
            string ffromdate = date1.ToString("MM/dd");
            //string ffromdate = fromdate.ToString("MM/dd");
            string ttodate = date2.ToString("MM/dd");
            //string ttodate = todate.ToString("MM/dd");


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string pcode = row.Cells["PROJCODE"].Value.ToString();
                string subcode = row.Cells["SUBCODE"].Value.ToString();
                string pname = row.Cells["NAMES"].Value.ToString();

                string total = row.Cells["TOTAL"].Value.ToString();
                string name = "";
                if (pcode != "")
                {
                    name = pcode;
                    if (pname.Length >= 35)
                    {
                        checklines();
                        int r = pname.Length - 35;
                        MyPrinter.Print((char)15 + "                               " + name.PadRight(13, ' ') + pname.Substring(0, pname.Length - r).PadRight(35, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(40, ' ') + "\r\n");
                        pp++;
                        checklines();
                        MyPrinter.Print((char)15 + "                               " + name.PadRight(13, ' ') + pname.Substring(35).PadRight(35, ' ') + "\r\n");
                       
                        pp++;
                    }
                    else
                    {
                        checklines();
                        MyPrinter.Print((char)15 + "                               " + name.PadRight(13, ' ') + pname.PadRight(40, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(40, ' ') + "    " + "\r\n");
                        pp++;
                    }
                }
                else
                {
                    name = "   "+subcode;
                    if (pname.Length >= 35)
                    {
                        checklines();
                        int r = pname.Length - 35;
                        MyPrinter.Print((char)15 + "                               " + name.PadRight(13, ' ') + pname.Substring(0, pname.Length - r).PadRight(35, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(15, ' ') + "\r\n");
                        pp++;
                        checklines();
                        MyPrinter.Print((char)15 + "                               " + name.PadRight(13, ' ') + pname.Substring(35).PadRight(35, ' ') + "\r\n");
                        
                        pp++;
                    }
                    else
                    {
                        checklines();
                        MyPrinter.Print((char)15 + "                               " + name.PadRight(13, ' ') + pname.PadRight(40, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(15, ' ') + "    " + "\r\n");
                        pp++;
                    }
                }

            }


            checklines();
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string list = "SELECT SUM(AMOUNT) FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));

                decimal count = (decimal)command.ExecuteScalar();
                dbDR.Close();

                MyPrinter.Print((char)15 + "                                        " + CODE + "   Sales of " + TITLELAST.PadRight(35, ' ') + string.Format("{0:#,##0.00}", count) + "\r\n");
                if(type == "CMDS")
                {
                    MyPrinter.Print((char)15 + "                                        " + "     (pls refer to breakdown for entry)" + "\r\n");
                }
            }


            pp = pp + 3;
            int footer = 0;
            if (pp <= 40)
            {
                footer = 40 - pp;
            }
            for (int i = 0; i < footer; i++)
            {
                MyPrinter.Print("\r\n");
            }
            printprojectsummaryfoot();
            MyPrinter.Close();

            pp = 0;
            nxtp = 51;
        }
        private void printprojectsummaryfoot()
        {
            MyPrinter.Print((char)15 + "             ============================================================================================================" + "\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "             Prepared by: " + name.PadRight(23, ' ') + "                                               Checked by:___________________" + "\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "             Entered by:____________________" + "                                                   Approved by:___________________" + "\r\n");
            MyPrinter.Print("\x0C");

        }
        private void checklines()
        {
            if (pp > 40)
            {
                pp = 0;
                MyPrinter.Print("\x0C");
                printprojectsummaryheader();
            }
        }
        private void printsummary()
        {
            if (!MyPrinter.Open("Summary")) return;
            summaryheader();
            sumprint();

            MyPrinter.Print("\x0C");
            MyPrinter.Close();
            ppage = 0;
        }
        private void summaryheader()
        {
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            fdate = date.ToString("MM/dd/yyyy");
            ppage++;
            string pagenum = "Page " + ppage.ToString();
            string dateprint = "Date printed: " +  fdate;
            MyPrinter.Print((char)15 + "            " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");

            MyPrinter.Print((char)15 + "            Summary Report: " + ITEM + "  for  " + date1.ToString("MM/dd/yyyy") + " to " + date2.ToString("MM/dd/yyyy") + "\r\n");

            MyPrinter.Print("\r\n");
            rowinpage++;
            MyPrinter.Print((char)15 + "            DATE   DR #         PROJECT                  QTY  UNIT     CODE            ITEM                   UNIT PRICE       AMOUNT" + "\r\n");

        }
        private void sumchecklines()
        {
            if (rowinpage == 52)
            {
                ppage++;
                rowinpage = 0;
                MyPrinter.Print("\x0C");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                fdate = date.ToString("MM/dd/yyyy");
                string pagenum = "Page " + ppage.ToString();
                string dateprint = "Date printed: " + fdate;
                MyPrinter.Print((char)15 + "            " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");

                MyPrinter.Print("\r\n");
                MyPrinter.Print((char)15 + "            DATE   DR #         PROJECT                  QTY  UNIT     CODE            ITEM                   UNIT PRICE       AMOUNT" + "\r\n");

            }
        }
        private void sumprint()
        {
            DataTable dt = new DataTable();
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string list = "SELECT DISTINCT DRNO,DATE FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DRNO ASC";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dbDR.Close();
            }
            string date = "";
            sumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            decimal totalamount = 0.00M;
            foreach (DataRow row in dt.Rows)
            {
                DataTable sqlrow = new DataTable();
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    string list = "SELECT * FROM tblDBaseDR WHERE DRNO = '" + row["DRNO"].ToString() + "'";
                    SqlCommand command = new SqlCommand(list, dbDR);
                    SqlDataReader reader = command.ExecuteReader();
                    sqlrow.Load(reader);
                    dbDR.Close();
                }
                if (date != row["DATE"].ToString())
                {
                    sumchecklines();
                    MyPrinter.Print((char)15 + "            "+ DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy") + "\r\n");
                    rowinpage++;
                }

                string drno = "";
                foreach (DataRow rows in sqlrow.Rows)
                {
                    totalamount += Convert.ToDecimal(rows["AMOUNT"].ToString());

                    if (drno != row["DRNO"].ToString())
                    {
                        string project = "";
                        string sub = "";
                        using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            dbDR.Open();
                            String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + rows["PROJCODE"].ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, dbDR);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                project = (rdr["ACCTDESC"].ToString());
                            }
                            dbDR.Close();
                        }
                        using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            dbDR.Open();
                            String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + rows["SUBCODE"].ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, dbDR);
                            SqlDataReader rdr = cmd.ExecuteReader();

                            if (rdr.Read())
                            {
                                sub = (rdr["ACCTDESC"].ToString());
                            }
                            dbDR.Close();
                        }

                        const int MaxLength = 33;
                        var name = project;
                        if (name.Length > MaxLength)
                            name = name.Substring(0, MaxLength);



                        sumchecklines();
                        MyPrinter.Print((char)15 + "            " + row["DRNO"].ToString().PadLeft(9, ' ') + "  "+ name.PadRight(33,' ') +" "+ sub+ "\r\n");
                        rowinpage++;
                    }

                    const int maxunit = 5;
                    var subunit = rows["UNIT"].ToString();
                    if (subunit.Length > maxunit)
                        subunit = subunit.Substring(0, maxunit);
                    string amount = rows["AMOUNT"].ToString();
                    string unitp = rows["UPRICE"].ToString();
                    string qty = rows["QTY"].ToString();
                    if (qty == "0")
                    {
                        amount = "";
                        unitp = "";
                    }
                    else
                    {
                        amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(rows["AMOUNT"].ToString()));
                        unitp = string.Format("{0:#,##0.00}", Convert.ToDecimal(rows["UPRICE"].ToString()));
                    }
                    int itemleght = rows["ITEM"].ToString().Length;
                    int de = 0;
                    if (itemleght > 29)
                    {
                        de = itemleght - 29;
                    }
                    int pad = 12 - de;


                    sumchecklines();
                    MyPrinter.Print((char)15 + "               " + string.Format("{0:#,##0.00}", Convert.ToDecimal(rows["QTY"].ToString())).PadLeft(47,' ') + subunit.PadRight(5, ' ')+ "              " + rows["ITEM"].ToString().PadRight(29, ' ') +" "+ unitp.PadLeft(pad, ' ')+" "+ amount.PadLeft(12, ' ') + "\r\n");
                 
                    rowinpage++;
                    drno = row["DRNO"].ToString();
                }
                date = row["DATE"].ToString();

            }
            sumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            sumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            sumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            string emp = " ";
            sumchecklines();
            MyPrinter.Print((char)15 + "               " + emp.PadLeft(72, ' ') + "***  TOTAL AMOUNT  ***" + string.Format("{0:#,##0.00}", totalamount).PadLeft(24, ' ') + "\r\n");
            rowinpage++;
        }

        private void guna2CheckBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void subsummaryheader()
        {
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n"); 
            ppage++;
            fdate = date.ToString("MM/dd/yyyy");
            string pagenum = "Page " + ppage.ToString();
            string dateprint = "Date printed: " + fdate;
            MyPrinter.Print((char)15 + "            " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");

            MyPrinter.Print((char)15 + "            Subcode Summary Report:  " + ITEM + "  for  " + date1.ToString("MM/dd/yyyy") + " to " + date2.ToString("MM/dd/yyyy") + "\r\n");
          
            MyPrinter.Print("\r\n");
            string emp = "";
            MyPrinter.Print((char)15 + "            "+ emp.PadRight(80, ' ') + "UNIT PRICE      AMOUNT      Subtotal" + "\r\n");

        }
        private void sumsummary()
        {
            rowinpage = 0;
            ppage = 0;
            if (!MyPrinter.Open("SubSummary")) return;
            subsummaryheader();
            subcodeprint();



            MyPrinter.Print("\x0C");
            MyPrinter.Close();
            ppage = 0;
        }
        private void subcodeprint()
        {
            DataTable dt = new DataTable();
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string list = "SELECT DISTINCT PROJCODE FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY PROJCODE ASC";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dbDR.Close();
            }
            decimal totalamount = 0.00M;
            foreach (DataRow row in dt.Rows)
            {
                string project = "";
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + row["PROJCODE"].ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, dbDR);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        project = (rdr["ACCTDESC"].ToString());
                    }
                    dbDR.Close();
                }

                subsumchecklines();
                MyPrinter.Print((char)15 + "            " + row["PROJCODE"].ToString().Trim().PadRight(10, ' ') + "***   " + project.Trim().ToUpper() + "\r\n");
                rowinpage++;
                DataTable dt1 = new DataTable();
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    string list = "SELECT DISTINCT SUBCODE FROM tblDBaseDR WHERE PROJCODE = '" + row["PROJCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY SUBCODE ASC";
                    SqlCommand command = new SqlCommand(list, dbDR);
                    command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                    command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                    SqlDataReader reader = command.ExecuteReader();
                    dt1.Load(reader);
                    dbDR.Close();
                }
                foreach (DataRow row1 in dt1.Rows)
                {

                    string sub = "";
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + row1["SUBCODE"].ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, dbDR);
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            sub = (rdr["ACCTDESC"].ToString());
                        }
                        dbDR.Close();
                    }

                    subsumchecklines();
                    MyPrinter.Print((char)15 + "            " + row1["SUBCODE"].ToString().Trim().PadRight(16, ' ') +  sub.Trim().ToUpper() + "\r\n");;
                    rowinpage++;

                    DataTable dt2 = new DataTable();
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        string list = "SELECT DISTINCT DRNO,DATE FROM tblDBaseDR WHERE PROJCODE = '" + row["PROJCODE"].ToString() + "' AND SUBCODE = '" + row1["SUBCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DRNO ASC";
                        SqlCommand command = new SqlCommand(list, dbDR);
                        command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                        SqlDataReader reader = command.ExecuteReader();
                        dt2.Load(reader);
                        dbDR.Close();
                    }
                    Int32 count;
                    int sqlrow = 0;
                    decimal totalpersub = 0.00M;
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        string list = "SELECT COUNT(*) FROM tblDBaseDR WHERE PROJCODE = '" + row["PROJCODE"].ToString() + "' AND SUBCODE = '" + row1["SUBCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2";
                        SqlCommand command = new SqlCommand(list, dbDR);
                        command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                        //SqlDataReader reader = command.ExecuteReader();
                        count = (Int32)command.ExecuteScalar();
                        dbDR.Close();
                    }
                    //MessageBox.Show(count.ToString());
                    foreach (DataRow row2 in dt2.Rows)
                    {
                        subsumchecklines();
                        MyPrinter.Print((char)15 + "              " + DateTime.Parse(row2["DATE"].ToString()).ToString("MM/dd/yyyy") + "    " + row2["DRNO"].ToString().PadRight(5, ' ') + "\r\n");
                        rowinpage++;
                        DataTable dt3 = new DataTable();
                        using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            dbDR.Open();
                            string list = "SELECT QTY,UNIT,CODE,ITEM,UPRICE,AMOUNT FROM tblDBaseDR WHERE DRNO = '" + row2["DRNO"].ToString() + "' AND PROJCODE = '" + row["PROJCODE"].ToString() + "' AND SUBCODE = '" + row1["SUBCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DRNO ASC";
                            SqlCommand command = new SqlCommand(list, dbDR);
                            command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                            command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                            SqlDataReader reader = command.ExecuteReader();
                            dt3.Load(reader);
                            dbDR.Close();
                        }
                        foreach (DataRow row3 in dt3.Rows)
                        {
                            sqlrow++;
                            string amount = row3["AMOUNT"].ToString();
                            string unitp = row3["UPRICE"].ToString();
                            string qty = row3["QTY"].ToString();
                            string subtotal = "";
                            if (qty == "0")
                            {
                                amount = "";
                                unitp = "";
                            }
                            else
                            {
                                amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(row3["AMOUNT"].ToString()));
                                unitp = string.Format("{0:#,##0.00}", Convert.ToDecimal(row3["UPRICE"].ToString()));
                            }
                            int itemleght = row3["ITEM"].ToString().Length;
                            int de = 0;
                            if (itemleght > 29)
                            {
                                de = itemleght - 29;
                            }
                            int pad = 12 - de;
                            totalamount += Convert.ToDecimal(row3["AMOUNT"].ToString());

                            totalpersub += Convert.ToDecimal(row3["AMOUNT"].ToString());
                            if (sqlrow == count)
                            {
                                subtotal = string.Format("{0:#,##0.00}", totalpersub);
                            }
                            subsumchecklines();
                            MyPrinter.Print((char)15 + "            " + string.Format("{0:#,##0.00}", Convert.ToDecimal(row3["QTY"].ToString())).PadLeft(29, ' ') +"   "+ row3["UNIT"].ToString().PadRight(5, ' ') + "             " + row3["ITEM"].ToString().PadRight(29, ' ') + " " + unitp.PadLeft(pad, ' ') + " " + amount.PadLeft(12, ' ') + subtotal.PadLeft(12, ' ') + "\r\n");
                            rowinpage++;
                        }
                    }

                    subsumchecklines();
                    MyPrinter.Print("\r\n");
                    rowinpage++;
                }
            }
            subsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            subsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            subsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            subsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            string emp = "";
            subsumchecklines();
            MyPrinter.Print((char)15 + "               " + emp.PadLeft(68, ' ') + "***  TOTAL AMOUNT  ***" + string.Format("{0:#,##0.00}", totalamount).PadLeft(24, ' ') + "\r\n");
            rowinpage++;
        }
        private void subsumchecklines()
        {
            if (rowinpage == 51)
            {
                ppage++;
                rowinpage = 0;
                MyPrinter.Print("\x0C");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                fdate = date.ToString("MM/dd/yyyy");
                string pagenum = "Page " + ppage.ToString();
                string dateprint = "Date printed: " + fdate;
                MyPrinter.Print((char)15 + "            " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");


                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                string emp = "";
                MyPrinter.Print((char)15 + "            " + emp.PadRight(80, ' ') + "UNIT PRICE      AMOUNT      Subtotal" + "\r\n");
            }
        }



        private void othersprintprojectsummaryheader()
        {
            //MessageBox.Show(date1.ToString());
            //MessageBox.Show(date2.ToString());
            string ffromdate = date1.ToString("MM/dd");
            string ttodate = date2.ToString("MM/dd");
            //string ffromdate = DateTime.Parse(date1.ToString()).ToString("MM/dd");
            //string ttodate = DateTime.Parse(date2.ToString()).ToString("MM/dd");
            //MessageBox.Show("2");
            MyPrinter.Print("\r\n");

            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "                                                       S. UYMATIAO JR. CONSTRUCTION" + "\r\n");
            MyPrinter.Print((char)15 + "                                                              SALES VOUCHER");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");

            MyPrinter.Print((char)15 + "               ITEM: " + ITEM.PadRight(21, ' ') + "                                                         SV # _____________" + "\r\n");

            DateTime dat = DateTime.Parse(date2.ToString());
            string fdat = dat.ToString("MM/dd/yyyy");
            MyPrinter.Print((char)15 + "                                                                                                   DATE: " + fdat.ToString() + "\r\n");
            MyPrinter.Print((char)15 + "               ============================================================================================================" + "\r\n");
            MyPrinter.Print("\r\n");

            string ttitle = TITLE + " " + ffromdate + "-" + ttodate + ".";
            MyPrinter.Print((char)15 + "                                          " + ttitle + "\r\n");
            MyPrinter.Print("\r\n");

        }
        private void othersprintprojectsummary()
        {

            decimal sum = 0.00M;

            if (!MyPrinter.Open("Reports")) return;
            othersprintprojectsummaryheader();
            //DateTime fromdate = DateTime.Parse(date1.ToString());
            //string ffromdate = fromdate.ToString("MM/dd");
            //DateTime todate = DateTime.Parse(date2.ToString());
            //string ttodate = todate.ToString("MM/dd");


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string pcode = row.Cells["PROJCODE"].Value.ToString();
                string subcode = row.Cells["SUBCODE"].Value.ToString();
                string pname = row.Cells["NAMES"].Value.ToString();

                string total = row.Cells["TOTAL"].Value.ToString();
                string name = "";
                if (pcode != "")
                {
                    name = pcode;
                    if (pname.Length >= 35)
                    {
                        checklines();
                        int r = pname.Length - 35;
                        MyPrinter.Print((char)15 + "                                 " + name.PadRight(13, ' ') + pname.Substring(0, pname.Length - r).PadRight(35, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(15, ' ') + "\r\n");
                        pp++;
                        checklines();
                        MyPrinter.Print((char)15 + "                                 " + name.PadRight(13, ' ') + pname.Substring(35).PadRight(35, ' ') + "\r\n");

                        pp++;
                    }
                    else
                    {
                        checklines();
                        MyPrinter.Print((char)15 + "                                 " + name.PadRight(13, ' ') + pname.PadRight(40, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(15, ' ') + "    " + "\r\n");
                        pp++;
                    }
                }
                else
                {
                    name = "   " + subcode;
                    if (pname.Length >= 35)
                    {
                        checklines();
                        int r = pname.Length - 35;
                        MyPrinter.Print((char)15 + "                                 " + name.PadRight(13, ' ') + pname.Substring(0, pname.Length - r).PadRight(35, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(15, ' ') + "\r\n");
                        pp++;
                        checklines();
                        MyPrinter.Print((char)15 + "                                 " + name.PadRight(13, ' ') + pname.Substring(35).PadRight(35, ' ') + "\r\n");

                        pp++;
                    }
                    else
                    {
                        checklines();
                        MyPrinter.Print((char)15 + "                                 " + name.PadRight(13, ' ') + pname.PadRight(40, ' ') + "    " + string.Format("{0:#,##0.00}", Convert.ToDecimal(total)).PadLeft(15, ' ') + "    " + "\r\n");
                        pp++;
                    }
                }

            }


            checklines();
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string list = "SELECT SUM(AMOUNT) FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString());
                command.Parameters.AddWithValue("@date2", date2.ToString());

                decimal count = (decimal)command.ExecuteScalar();
                dbDR.Close();
                //decimal count = 0.00M;
                MyPrinter.Print((char)15 + "                                          " + CODE + "   Sales of " + TITLELAST.PadRight(35, ' ') + string.Format("{0:#,##0.00}", count) + "\r\n");
                if (type == "CMDS")
                {
                    MyPrinter.Print((char)15 + "                                          " + "     (pls refer to breakdown for entry)" + "\r\n");
                }
            }

            pp = pp + 3;
            int footer = 0;
            if (pp <= 40)
            {
                footer = 40 - pp;
            }
            for (int i = 0; i < footer; i++)
            {
                MyPrinter.Print("\r\n");
            }
            otherprintprojectsummaryfoot();
            MyPrinter.Close();

            pp = 0;
            nxtp = 51;
        }
        private void otherprintprojectsummaryfoot()
        {
            MyPrinter.Print((char)15 + "               ============================================================================================================" + "\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "               Prepared by: " + name.PadRight(23, ' ') + "                                               Checked by:___________________" + "\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "               Entered by:____________________" + "                                                   Approved by:___________________" + "\r\n");
            MyPrinter.Print("\x0C");

        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
           
        }
        private void otherssubsummaryheader()
        {
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            ppage++;
            fdate = date.ToString("MM/dd/yyyy");
            string pagenum = "Page " + ppage.ToString();
            string dateprint = "Date printed: " + fdate;
            MyPrinter.Print((char)15 + "            " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");

            MyPrinter.Print((char)15 + "            Subcode Summary Report:  " + ITEM + "  for  " + date1.ToString("MM/dd/yyyy") + " to " + date2.ToString("MM/dd/yyyy") + "\r\n");

            MyPrinter.Print("\r\n");
            string emp = "";
            MyPrinter.Print((char)15 + "            " + emp.PadRight(80, ' ') + "UNIT PRICE      AMOUNT      Subtotal" + "\r\n");

        }
        private void otherssumsummary()
        {
            rowinpage = 0;
            ppage = 0;
            if (!MyPrinter.Open("SubSummary")) return;
            otherssubsummaryheader();
            otherssubcodeprint();



            MyPrinter.Print("\x0C");
            MyPrinter.Close();
            ppage = 0;
        }
        private void otherssubcodeprint()
        {
            DataTable dt = new DataTable();
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                dbDR.Open();
                string list = "SELECT DISTINCT PROJCODE FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY PROJCODE ASC";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dbDR.Close();
            }
            decimal totalamount = 0.00M;
            foreach (DataRow row in dt.Rows)
            {
                string project = "";
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + row["PROJCODE"].ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, dbDR);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        project = (rdr["ACCTDESC"].ToString());
                    }
                    dbDR.Close();
                }

                otherssubsumchecklines();
                MyPrinter.Print((char)15 + "            " + row["PROJCODE"].ToString().Trim().PadRight(10, ' ') + "***   " + project.Trim().ToUpper() + "\r\n");
                rowinpage++;
              

                    DataTable dt2 = new DataTable();
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        string list = "SELECT DISTINCT DRNO,DATE FROM tblDBaseDR WHERE PROJCODE = '" + row["PROJCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DRNO ASC";
                        SqlCommand command = new SqlCommand(list, dbDR);
                        command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                    command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                    SqlDataReader reader = command.ExecuteReader();
                        dt2.Load(reader);
                        dbDR.Close();
                    }
                    Int32 count;
                    int sqlrow = 0;
                    decimal totalpersub = 0.00M;
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {
                        dbDR.Open();
                        string list = "SELECT COUNT(*) FROM tblDBaseDR WHERE PROJCODE = '" + row["PROJCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2";
                        SqlCommand command = new SqlCommand(list, dbDR);
                        command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                    command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                    //SqlDataReader reader = command.ExecuteReader();
                    count = (Int32)command.ExecuteScalar();
                        dbDR.Close();
                    }
                    //MessageBox.Show(count.ToString());
                    foreach (DataRow row2 in dt2.Rows)
                    {
                        otherssubsumchecklines();
                        MyPrinter.Print((char)15 + "              " + DateTime.Parse(row2["DATE"].ToString()).ToString("MM/dd/yyyy") + "    " + row2["DRNO"].ToString().PadRight(5, ' ') + "\r\n");
                        rowinpage++;
                        DataTable dt3 = new DataTable();
                        using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                        {
                            dbDR.Open();
                            string list = "SELECT QTY,UNIT,CODE,ITEM,UPRICE,AMOUNT FROM tblDBaseDR WHERE DRNO = '" + row2["DRNO"].ToString() + "' AND PROJCODE = '" + row["PROJCODE"].ToString() + "' AND TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DRNO ASC";
                            SqlCommand command = new SqlCommand(list, dbDR);
                            command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                        SqlDataReader reader = command.ExecuteReader();
                            dt3.Load(reader);
                            dbDR.Close();
                        }
                        foreach (DataRow row3 in dt3.Rows)
                        {
                            sqlrow++;
                            string amount = row3["AMOUNT"].ToString();
                            string unitp = row3["UPRICE"].ToString();
                            string qty = row3["QTY"].ToString();
                            string subtotal = "";
                            if (qty == "0")
                            {
                                amount = "";
                                unitp = "";
                        }
                        else
                        {
                            amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(amount));
                            unitp = string.Format("{0:#,##0.00}", Convert.ToDecimal(unitp));
                        }
                            int itemleght = row3["ITEM"].ToString().Length;
                            int de = 0;
                            if (itemleght > 29)
                            {
                                de = itemleght - 29;
                            }
                            int pad = 12 - de;
                            totalamount += Convert.ToDecimal(row3["AMOUNT"].ToString());

                            totalpersub += Convert.ToDecimal(row3["AMOUNT"].ToString());
                            if (sqlrow == count)
                            {
                                subtotal = string.Format("{0:#,##0.00}", totalpersub);
                            }
                            otherssubsumchecklines();
                            MyPrinter.Print((char)15 + "            " + string.Format("{0:#,##0.00}", Convert.ToDecimal(row3["QTY"].ToString())).PadLeft(29, ' ') + "   " + row3["UNIT"].ToString().PadRight(5, ' ') + "             " + row3["ITEM"].ToString().PadRight(29, ' ') + " " + unitp.PadLeft(pad, ' ') + " " + amount.PadLeft(12, ' ') + subtotal.PadLeft(12, ' ') + "\r\n");
                            rowinpage++;
                        }
                    }
                    otherssubsumchecklines();
                    MyPrinter.Print("\r\n");
                    rowinpage++;
                
            }
            otherssubsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            otherssubsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            otherssubsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            otherssubsumchecklines();
            MyPrinter.Print("\r\n");
            rowinpage++;
            string emp = "";
            otherssubsumchecklines();
            MyPrinter.Print((char)15 + "               " + emp.PadLeft(68, ' ') + "***  TOTAL AMOUNT  ***" + string.Format("{0:#,##0.00}", totalamount).PadLeft(24, ' ') + "\r\n");
            rowinpage++;
        }
        private void otherssubsumchecklines()
        {
            if (rowinpage == 51)
            {
                ppage++;
                rowinpage = 0;
                MyPrinter.Print("\x0C");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                fdate = date.ToString("MM/dd/yyyy");
                string pagenum = "Page " + ppage.ToString();
                string dateprint = "Date printed: " + fdate;
                MyPrinter.Print((char)15 + "            " + dateprint.PadRight(24, ' ') + pagenum.PadLeft(94, ' ') + "\r\n");


                MyPrinter.Print("\r\n");
                MyPrinter.Print("\r\n");
                string emp = "";
                MyPrinter.Print((char)15 + "            " + emp.PadRight(80, ' ') + "UNIT PRICE      AMOUNT      Subtotal" + "\r\n");
            }
        }

        private void guna2CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            //if (guna2CheckBox5.Checked == true)
            //{
            //    guna2CheckBox5.Checked = false;
            //    MessageBox.Show("This feature still not available. Please wait in the next update. Thank you!");
            //}
        }




        private void print()
        {
            //tblsi.Rows.Clear();
            if (!MyPrinter.Open("DRReports")) return;
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + ("Date printed: " + DateTime.Now.ToString("MM/dd/yyyy")).PadRight(117, ' ') + "p. " + page.ToString() + "\r\n");
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + "Summary Report: " + ITEM + "     " + date1.ToString("MM/dd") + "  to  " + date2.ToString("MM/dd") + "\r\n");

            MyPrinter.Print("\r\n");
            otherschecklines();
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + ("DATE").PadRight(7, ' ') + ("DR #").PadRight(13, ' ') + ("PROJECT").PadRight(14, ' ') + ("QTY") + " " + ("UNIT").PadRight(8, ' ') + ("CODE").PadRight(10, ' ') + ("ITEM").PadRight(40, ' ') + ("UNIT PRICE").PadLeft(15, ' ') + ("AMOUNT").PadLeft(14, ' ') + "\r\n");

            lines++;
            decimal sum = 0.00M;
            string date = "";

            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DataTable dt = new DataTable();
                dbDR.Open();
                string list = "SELECT DISTINCT DRNO,DATE,PROJCODE FROM tblDBaseDR WHERE TYPE = '" + type + "' AND DATE BETWEEN @date1 AND @date2 ORDER BY DATE ASC";
                SqlCommand command = new SqlCommand(list, dbDR);
                command.Parameters.AddWithValue("@date1", date1.ToString("MM/dd/yyyy"));
                command.Parameters.AddWithValue("@date2", date2.ToString("MM/dd/yyyy"));
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                dbDR.Close();

                decimal totalamount = 0.00M;
                foreach (DataRow row in dt.Rows)
                {
                    string project = "";

                    dbDR.Open();
                    String query = "SELECT ACCTDESC FROM tblDbaseGLU4 WHERE ACCTCODE = '" + row["PROJCODE"].ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, dbDR);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        project = (rdr["ACCTDESC"].ToString());
                    }
                    dbDR.Close();

                    if (date != DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy"))
                    {
                        //int b = dataGridView1.Rows.Add();
                        //dataGridView1.Rows[b].Cells["datedr"].Value = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                        otherschecklines();
                        MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy") + "\r\n");
                        lines++;
                    }

                    otherschecklines();
                    MyPrinter.Print((char)15 + row["DRNO"].ToString().PadLeft(17, ' ') + "  " + project + "\r\n");
                    lines++;

                    DataTable dtA = new DataTable();
                    dbDR.Open();
                    string list1 = "SELECT QTY,UNIT,ITEM,UPRICE,AMOUNT,DRNO FROM tblDBaseDR WHERE DRNO = '" + row["DRNO"].ToString() + "'";
                    SqlDataAdapter command1 = new SqlDataAdapter(list1, dbDR);
                    command1.Fill(dtA);
                    dbDR.Close();
                    foreach (DataRow item1 in dtA.Rows)
                    {
                        string qty = "";
                        string unit = "";
                        string productcode = "";
                        string desc = "";
                        string uprice = "";
                        string amount = "";

                        qty = Convert.ToDecimal(item1["QTY"].ToString()).ToString();
                        if (item1["UNIT"].ToString().Length > 6)
                        {
                            unit = item1["UNIT"].ToString().Substring(0, 6);
                        }
                        else
                        {
                            unit = item1["UNIT"].ToString();
                        }
                        desc = item1["ITEM"].ToString();
                        uprice = string.Format("{0:#,##0.00}", Convert.ToDecimal(item1["UPRICE"].ToString()));
                        amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(item1["AMOUNT"].ToString()));

                        int jed = 0;

                        otherschecklines();                                                                                                                                                                  //string reference = "";
                        MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + qty.PadLeft(37, ' ') + " " + unit.PadRight(5, ' ') + " " + productcode.PadRight(11, ' ') + " " + desc.ToString().PadRight(40, ' ') + " " + uprice.PadLeft(14, ' ') + " " + amount.PadLeft(13, ' ') + "\r\n");
                        lines++;

                        sum += Convert.ToDecimal(item1["AMOUNT"].ToString());
                    }
                    date = DateTime.Parse(row["DATE"].ToString()).ToString("MM/dd/yyyy");
                }



                //foreach (DataRow item in tblsi.Rows)
                //{
                //    if (date != DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy"))
                //    {
                //        //int b = dataGridView1.Rows.Add();
                //        //dataGridView1.Rows[b].Cells["datedr"].Value = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                //        otherschecklines();
                //        MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy") + "\r\n");
                //        lines++;
                //    }
                //    //int a = dataGridView1.Rows.Add();
                //    //dataGridView1.Rows[a].Cells["datedr"].Value = item["drnumber"].ToString();
                //    //dataGridView1.Rows[a].Cells["project"].Value = item["projectname"].ToString();
                //    otherschecklines();
                //    MyPrinter.Print((char)15 + item["drnumber"].ToString().ToString().PadLeft(17, ' ') + "  " + item["projectname"].ToString() + "\r\n");
                //    lines++;

                //    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                //    {
                //        DataTable dt = new DataTable();
                //        itemCode.Open();
                //        string list1 = "SELECT qty,unit,productcode,description,selling,total,id,drnumber,icode FROM tblDRitemCode WHERE icode = '" + item["itemcode"].ToString() + "' ORDER BY id ASC";
                //        SqlDataAdapter command1 = new SqlDataAdapter(list1, itemCode);
                //        command1.Fill(dt);
                //        itemCode.Close();
                //        foreach (DataRow item1 in dt.Rows)
                //        {
                //            string qty = "";
                //            string unit = "";
                //            string productcode = "";
                //            string desc = "";
                //            string uprice = "";
                //            string amount = "";

                //            qty = Convert.ToDecimal(item1["qty"].ToString()).ToString();
                //            if (item1["unit"].ToString().Length > 6)
                //            {
                //                unit = item1["unit"].ToString().Substring(0, 6);
                //            }
                //            else
                //            {
                //                unit = item1["unit"].ToString();
                //            }
                //            desc = item1["description"].ToString();
                //            uprice = string.Format("{0:#,##0.00}", Convert.ToDecimal(item1["selling"].ToString()));

                //            amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(item1["total"].ToString()));

                //            int jed = 0;

                //            int chunkSize = 40;
                //            int stringLength = desc.Length;
                //            for (int i = 0; i < stringLength; i += chunkSize)
                //            {
                //                jed++;

                //                if (i + chunkSize > stringLength) chunkSize = stringLength - i;

                //                if (jed == 1)
                //                {
                //                    otherschecklines();                                                                                                                                                                //string reference = "";
                //                    MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + qty.PadLeft(37, ' ') + " " + unit.PadRight(5, ' ') + " " + productcode.PadRight(11, ' ') + " " + desc.Substring(i, chunkSize).PadRight(40, ' ') + " " + uprice.PadLeft(14, ' ') + " " + amount.PadLeft(13, ' ') + "\r\n");
                //                    lines++;
                //                }
                //                else
                //                {
                //                    otherschecklines();
                //                    MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + (" ").PadLeft(37, ' ') + " " + (" ").PadRight(5, ' ') + " " + (" ").PadRight(11, ' ') + " " + desc.Substring(i, chunkSize).PadRight(40, ' ') + " " + (" ").PadLeft(14, ' ') + " " + (" ").PadLeft(13, ' ') + "\r\n");
                //                    lines++;
                //                }
                //            }
                //        }
                //        sum += Convert.ToDecimal(item["totalamount"].ToString());
                //    }


                //    //label2.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(sum.ToString()));


                //    date = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                //}
                otherschecklines();
                MyPrinter.Print("\r\n");
                lines++;
                otherschecklines();
                MyPrinter.Print("\r\n");
                lines++;
                otherschecklines();
                MyPrinter.Print("\r\n");
                lines++;
                otherschecklines();
                MyPrinter.Print((char)15 + ("*** TOTAL AMOUNT  ***").PadLeft(105, ' ') + "  " + string.Format("{0:#,##0.00}", Convert.ToDecimal(sum.ToString())).PadLeft(22, ' ') + "\r\n");
                lines++;

                MyPrinter.Print("\x0C");
                MyPrinter.Close();
                this.Close();
            }

        }
        int lines = 0;
        private void otherschecklines()
        {
            if (lines == 52)
            {
                lines = 0;
                page++;
                MyPrinter.Print("\x0C");
                printHeader();
            }
        }
        private void printHeader()
        {
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + ("Date printed: " + DateTime.Now.ToString("MM/dd/yyyy")).PadRight(117, ' ') + "p. " + page.ToString() + "\r\n");

            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + ("DATE").PadRight(7, ' ') + ("DR #").PadRight(13, ' ') + ("PROJECT").PadRight(14, ' ') + ("QTY") + " " + ("UNIT").PadRight(8, ' ') + ("CODE").PadRight(10, ' ') + ("ITEM").PadRight(40, ' ') + ("UNIT PRICE").PadLeft(15, ' ') + ("AMOUNT").PadLeft(14, ' ') + "\r\n");

        }
    }
}
