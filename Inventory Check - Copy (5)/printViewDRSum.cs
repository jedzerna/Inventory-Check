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
    public partial class printViewDRSum : Form
    {
        public printViewDRSum()
        {
            InitializeComponent();
            MyPrinter = new LPrinter();
        }

        LPrinter MyPrinter;
        public string datefrom;
        public string dateto;
        public string sv;
        DataTable tblsi = new DataTable();
        private void printViewDRSum_Load(object sender, EventArgs e)
        {
            label5.Text = "Summary Report: "+sv+ " for "+ datefrom + " to "+ dateto;
            decimal sum = 0.00M;
            string date = "";
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DateTime from = DateTime.Parse(datefrom);
                DateTime to = DateTime.Parse(dateto);

                dbDR.Open();
                string list = "SELECT drnumber,datetime,projectname FROM tblDR WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND sv = '" + sv + "' ORDER BY datetime,drnumber ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
                command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                command.Fill(tblsi);
                dbDR.Close();
                foreach (DataRow item in tblsi.Rows)
                {
                    if (date != DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy"))
                    {
                        int b = dataGridView1.Rows.Add();
                        dataGridView1.Rows[b].Cells["datedr"].Value = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                    }
                    int a = dataGridView1.Rows.Add();
                    dataGridView1.Rows[a].Cells["datedr"].Value = item["drnumber"].ToString();
                    dataGridView1.Rows[a].Cells["project"].Value = item["projectname"].ToString();

                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        DataTable dt = new DataTable();
                        itemCode.Open();
                        string list1 = "SELECT qty,unit,productcode,description,selling,total,id,drnumber FROM tblDRitemCode WHERE drnumber = '" + item["drnumber"].ToString() + "' ORDER BY id ASC";
                        SqlDataAdapter command1 = new SqlDataAdapter(list1, itemCode);
                        command1.Fill(dt);
                        itemCode.Close();
                        foreach (DataRow item1 in dt.Rows)
                        {

                            int c = dataGridView1.Rows.Add();
                            dataGridView1.Rows[c].Cells["qty"].Value = item1["qty"].ToString();
                            dataGridView1.Rows[c].Cells["unit"].Value = item1["unit"].ToString();
                            dataGridView1.Rows[c].Cells["code"].Value = item1["productcode"].ToString();
                            dataGridView1.Rows[c].Cells["item"].Value = item1["description"].ToString();
                            dataGridView1.Rows[c].Cells["unitprice"].Value = item1["selling"].ToString();
                            dataGridView1.Rows[c].Cells["amount"].Value = item1["total"].ToString();
                            sum += Convert.ToDecimal(item1["total"].ToString());
                        }
                    }


                    label2.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(sum.ToString()));

                    date = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                }
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            if (MyPrinter.pd.ShowDialog() == DialogResult.OK)
            {

                MyPrinter.ps = MyPrinter.pd.PrinterSettings;
            }
            else
            {
                return;
            }
            print();

        }
        int page = 1;
        private void print()
        {
            tblsi.Rows.Clear();
            if (!MyPrinter.Open("DRReports")) return;
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 +(" ").ToString().PadRight(8,' ')+ ("Date printed: " + DateTime.Now.ToString("MM/dd/yyyy")).PadRight(117, ' ') + "p. " + page.ToString() + "\r\n");
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + "Summary Report: "+sv +"     "+ DateTime.Parse(datefrom).ToString("MM/dd/yyyy") + "  to  " + DateTime.Parse(dateto).ToString("MM/dd/yyyy") + "\r\n");

            MyPrinter.Print("\r\n"); 
            checklines();
            MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') +("DATE").PadRight(7,' ')+("DR #").PadRight(13,' ')+("PROJECT").PadRight(14,' ')+("QTY")+" "+("UNIT").PadRight(8,' ')+("CODE").PadRight(10,' ')+("ITEM").PadRight(40,' ')+("UNIT PRICE").PadLeft(15,' ')+("AMOUNT").PadLeft(14, ' ') + "\r\n");
        
            lines++;
            decimal sum = 0.00M;
            string date = "";
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DateTime from = DateTime.Parse(datefrom);
                DateTime to = DateTime.Parse(dateto);

                dbDR.Open();
                string list = "SELECT drnumber,datetime,projectname FROM tblDR WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND sv = '" + sv + "' ORDER BY datetime,drnumber ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
                command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                command.Fill(tblsi);
                dbDR.Close();
                foreach (DataRow item in tblsi.Rows)
                {
                    if (date != DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy"))
                    {
                        //int b = dataGridView1.Rows.Add();
                        //dataGridView1.Rows[b].Cells["datedr"].Value = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                        checklines();
                        MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy") + "\r\n");
                        lines++;
                    }
                    //int a = dataGridView1.Rows.Add();
                    //dataGridView1.Rows[a].Cells["datedr"].Value = item["drnumber"].ToString();
                    //dataGridView1.Rows[a].Cells["project"].Value = item["projectname"].ToString();
                    checklines();
                    MyPrinter.Print((char)15 + item["drnumber"].ToString().ToString().PadLeft(17, ' ') + "  " + item["projectname"].ToString() + "\r\n");
                    lines++;

                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        DataTable dt = new DataTable();
                        itemCode.Open();
                        string list1 = "SELECT qty,unit,productcode,description,selling,total,id,drnumber FROM tblDRitemCode WHERE drnumber = '" + item["drnumber"].ToString() + "' ORDER BY id ASC";
                        SqlDataAdapter command1 = new SqlDataAdapter(list1, itemCode);
                        command1.Fill(dt);
                        itemCode.Close();
                        foreach (DataRow item1 in dt.Rows)
                        {
                            string qty = "";
                            string unit = "";
                            string productcode = "";
                            string desc = "";
                            string uprice = "";
                            string amount = "";

                            qty = Convert.ToDecimal(item1["qty"].ToString()).ToString();
                            if (item1["unit"].ToString().Length > 6)
                            {
                                unit = item1["unit"].ToString().Substring(0, 6);
                            }
                            else
                            {
                                unit = item1["unit"].ToString();
                            }
                            desc = item1["description"].ToString();
                            uprice = string.Format("{0:#,##0.00}", Convert.ToDecimal(item1["selling"].ToString()));

                            amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(item1["total"].ToString()));
                          

                            int jed = 0;

                            int chunkSize = 40;
                            int stringLength = desc.Length;
                            for (int i = 0; i < stringLength; i += chunkSize)
                            {
                                jed++;

                                if (i + chunkSize > stringLength) chunkSize = stringLength - i;

                                if (jed == 1)
                                {
                                    checklines();                                                                                                                                                                //string reference = "";
                                    MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + qty.PadLeft(37, ' ') +" " + unit.PadRight(5, ' ') + " "+productcode.PadRight(11,' ')+" " + desc.Substring(i, chunkSize).PadRight(40, ' ') + " " + uprice.PadLeft(14, ' ') + " " + amount.PadLeft(13, ' ')  + "\r\n");
                                    lines++;
                                }
                                else
                                {
                                    checklines();
                                    //MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + ("").PadLeft(10, ' ') + "  " + ("").PadRight(3, ' ') + "  " + desc.Substring(i, chunkSize).PadRight(34, ' ') + " " + ("").PadLeft(16, ' ') + " " + ("").PadLeft(19, ' ') + " " + "\r\n");
                                    MyPrinter.Print((char)15 + (" ").ToString().PadRight(8, ' ') + (" ").PadLeft(37, ' ') + " " + (" ").PadRight(5, ' ') + " " + (" ").PadRight(11, ' ') + " " + desc.Substring(i, chunkSize).PadRight(40, ' ') + " " + (" ").PadLeft(14, ' ') + " " + (" ").PadLeft(13, ' ') + "\r\n");
                                    lines++;
                                }
                            }


                             
                            //int c = dataGridView1.Rows.Add();
                            //dataGridView1.Rows[c].Cells["qty"].Value = item1["qty"].ToString();
                            //dataGridView1.Rows[c].Cells["unit"].Value = item1["unit"].ToString();
                            //dataGridView1.Rows[c].Cells["code"].Value = item1["productcode"].ToString();
                            //dataGridView1.Rows[c].Cells["item"].Value = item1["description"].ToString();
                            //dataGridView1.Rows[c].Cells["unitprice"].Value = item1["selling"].ToString();
                            //dataGridView1.Rows[c].Cells["amount"].Value = item1["total"].ToString();



                            sum += Convert.ToDecimal(item1["total"].ToString());
                        }
                    }


                    //label2.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(sum.ToString()));

                  
                    date = DateTime.Parse(item["datetime"].ToString()).ToString("MM/dd/yyyy");
                }
                checklines();
                MyPrinter.Print("\r\n");
                lines++;
                checklines();
                MyPrinter.Print("\r\n");
                lines++;
                checklines();
                MyPrinter.Print("\r\n");
                lines++;
                checklines();
                MyPrinter.Print((char)15 + ("*** TOTAL AMOUNT  ***").PadLeft(105, ' ') + "  " + string.Format("{0:#,##0.00}", Convert.ToDecimal(sum.ToString())).PadLeft(22, ' ') + "\r\n");
                lines++;

                MyPrinter.Print("\x0C");
                MyPrinter.Close();
                this.Close();
            }

        }
        int lines = 0;
        private void checklines()
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
