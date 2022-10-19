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
    public partial class printPreviewVP : Form
    {
        public printPreviewVP()
        {
            InitializeComponent();
            MyPrinter = new LPrinter();
        }
        LPrinter MyPrinter;
        public string datefrom;
        public string dateto;
        DataTable tblsi = new DataTable();
        private void printPreviewVP_Load(object sender, EventArgs e)
        {
            //tblsi.Rows.Clear();

            tblsi.Columns.Add("VPno", typeof(System.String));
            tblsi.Columns.Add("SIdate", typeof(System.String));
            tblsi.Columns.Add("SIvanno", typeof(System.String));
            tblsi.Columns.Add("POSupplier", typeof(System.String));
            tblsi.Columns.Add("POid", typeof(System.String));

            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                DateTime from = DateTime.Parse(datefrom);
                DateTime to = DateTime.Parse(dateto);

                DataTable si = new DataTable();
                decimal total = 0.00M;
                tblIn.Open();
                string list = "SELECT * FROM tblSI WHERE date BETWEEN @date1 AND @date2";
                SqlDataAdapter command = new SqlDataAdapter(list, tblIn);
                command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                command.Fill(si);
                tblIn.Close();
                foreach (DataRow item in si.Rows)
                {

                    tblIn.Open();
                    SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [tblIn] WHERE ([Id] = @Id)", tblIn);
                    check_User_Name.Parameters.AddWithValue("@Id", item["poid"].ToString());
                    int UserExist = (int)check_User_Name.ExecuteScalar();

                    if (UserExist > 0)
                    {
                        tblIn.Close();

                        string VPno = "";
                        string SIdate = "";
                        string SIvanno = "";
                        string POSupplier = "";
                        string POid = "";

                        SIdate = item["date"].ToString();
                        SIvanno = item["carno"].ToString();

                        tblIn.Open();
                        String query = "SELECT * FROM tblVP WHERE poid = '" + item["poid"].ToString() + "'";
                        SqlCommand cmd = new SqlCommand(query, tblIn);
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int val = 0;
                        if (rdr.Read())
                        {
                            VPno = (rdr["vpno"].ToString());
                        }
                        POid = item["poid"].ToString();
                        tblIn.Close();
                        tblIn.Open();
                        String query1 = "SELECT suppliername FROM tblIn WHERE Id = '" + item["poid"].ToString() + "'";
                        SqlCommand cmd1 = new SqlCommand(query1, tblIn);
                        SqlDataReader rdr1 = cmd1.ExecuteReader();
                        if (rdr1.Read())
                        {
                            POSupplier = (rdr1["suppliername"].ToString());
                        }
                        tblIn.Close();

                        tblsi.Rows.Add(new object[] { VPno, SIdate, SIvanno, POSupplier, POid });
                        tblsi.AcceptChanges();
                    }
                    else
                    {
                        tblIn.Close();

                        tblIn.Open();
                        using (SqlCommand commanda = new SqlCommand("DELETE FROM tblSI WHERE id = '" + item["id"].ToString() + "'", tblIn))
                        {
                            commanda.ExecuteNonQuery();
                        }
                        tblIn.Close();
                    }

                    

                }
            }
            DataView dv = tblsi.DefaultView;
            dv.Sort = "VPno ASC";
            tblsi.AcceptChanges();
            dataGridView2.DataSource = tblsi;
            dataGridView1.Rows.Clear();
            decimal totalamount = 0.00M;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                dataGridView1.Rows.Add();
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    int a = dataGridView1.Rows.Add();
                    dataGridView1.Rows[a].Cells["vpno"].Value = row.Cells["vpnumber"].Value.ToString();
                    dataGridView1.Rows[a].Cells["supplierinv"].Value = row.Cells["POSupplier"].Value.ToString();
                    dataGridView1.Rows[a].Cells["dateanddescription"].Value = row.Cells["SIdate"].Value.ToString();
                    dataGridView1.Rows[a].Cells["shipperuprice"].Value = row.Cells["SIvanno"].Value.ToString();

                    decimal sum = 0.00M;

                    itemCode.Open();
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter a2 = new SqlDataAdapter("Select si,qty,unit,description,cost,total,charging from tblSIitems where poid = '" + row.Cells["POid"].Value.ToString() + "' order by sortingid asc", itemCode))
                        a2.Fill(dt);
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dt;
                    foreach (DataRow item in dt.Rows)
                    {
                        int n = dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells["dateanddescription"].Value = item["description"].ToString();
                        dataGridView1.Rows[n].Cells["qty"].Value = item["qty"].ToString();
                        dataGridView1.Rows[n].Cells["unit"].Value = item["unit"].ToString();
                        dataGridView1.Rows[n].Cells["shipperuprice"].Value = item["cost"].ToString();
                        dataGridView1.Rows[n].Cells["amount"].Value = item["total"].ToString();
                        dataGridView1.Rows[n].Cells["supplierinv"].Value = item["si"].ToString();
                        dataGridView1.Rows[n].Cells["charging"].Value = item["charging"].ToString();
                        sum += Convert.ToDecimal(item["total"].ToString());
                        totalamount += Convert.ToDecimal(item["total"].ToString());
                    }
                    itemCode.Close();
                    dataGridView1.Rows.Add();
                    int b = dataGridView1.Rows.Add();
                    dataGridView1.Rows[b].Cells["shipperuprice"].Value = "TOTAL:";
                    dataGridView1.Rows[b].Cells["amount"].Value = string.Format("{0:#,##0.00}", sum);
                }
            }
            label2.Text = string.Format("{0:#,##0.00}", totalamount);

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
        private void printHeader()
        {
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + ("   Date printed: " + DateTime.Now.ToString("MM/dd/yyyy")).PadRight(130, ' ') + "p. " + page.ToString() + "\r\n");
            MyPrinter.Print((char)15 + "   VP SUMMARY: for " + DateTime.Parse(datefrom).ToString("MM/dd/yyyy") + "  to  " + DateTime.Parse(dateto).ToString("MM/dd/yyyy") + "\r\n");

            MyPrinter.Print("\r\n");
            MyPrinter.Print((char)15 + "   " + ("VP #").PadRight(10, ' ') + ("SUPPLIER/").PadRight(34, ' ') + ("DATE/").PadRight(33, ' ') + "SHIPPER" + "\r\n");
            MyPrinter.Print((char)15 + "   " + (" ").PadRight(10, ' ') + ("INV #").PadRight(17, ' ') + ("QTY").PadRight(5, ' ') + ("UNIT").PadRight(10, ' ') + ("DESCRIPTION").PadRight(38, ' ') + ("U/PRICE").PadRight(14, ' ') + ("AMOUNT").PadRight(15, ' ') + ("CHARGING").PadRight(15, ' ') + "REF #" + "\r\n");

         
        }
        private void print()
        {
            if (!MyPrinter.Open("VoucherReports")) return;
            //HEADER
            printHeader();
            //header
            printdate();
            MyPrinter.Print("\x0C");
            MyPrinter.Close();
            this.Close();


        }
        int page = 1;

        private void printdate()
        {
            using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
            {
                //decimal total = 0.00M;
                //foreach (DataGridViewRow dgvitem in dataGridView2.Rows)
                //{

                    //checklines();
                    //    MyPrinter.Print((char)15 + "   " + "\r\n");
                    //lines++;
                    decimal totalamount = 0.00M;

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    checklines();
                    MyPrinter.Print((char)15 + "   " + "\r\n");
                    lines++;

                    string vp = "";
                    string supplier = "";
                    string adte = "";
                    string van = "";
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        //int a = dataGridView1.Rows.Add();
                        vp = row.Cells["vpnumber"].Value.ToString();
                        supplier = row.Cells["POSupplier"].Value.ToString();
                        adte = row.Cells["SIdate"].Value.ToString();
                        van = row.Cells["SIvanno"].Value.ToString();

                        checklines();
                        MyPrinter.Print((char)15 + "   " + vp.PadRight(8, ' ') + supplier.PadRight(32, ' ') + adte.PadRight(37, ' ') + van + "\r\n");
                        lines++;
                        decimal sum = 0.00M;

                        itemCode.Open();
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter a2 = new SqlDataAdapter("Select si,qty,unit,description,cost,total,charging from tblSIitems where poid = '" + row.Cells["POid"].Value.ToString() + "' order by sortingid asc", itemCode))
                            a2.Fill(dt);
                        BindingSource bs = new BindingSource();
                        bs.DataSource = dt;
                        foreach (DataRow item in dt.Rows)
                        {
                            string desc = "";
                            string qty = "";
                            string unit = "";
                            string uprice = "";
                            string amount = "";
                            string supplierinv = "";
                            string charging = "";
                            string si = "";
                            string reference = "";
                            //int n = dataGridView1.Rows.Add();
                            desc = item["description"].ToString();
                            qty = item["qty"].ToString();
                            unit = item["unit"].ToString();
                            uprice = item["cost"].ToString();
                            amount = item["total"].ToString();
                            supplierinv = item["si"].ToString();
                            charging = item["charging"].ToString();
                            si = item["si"].ToString();
                            sum += Convert.ToDecimal(item["total"].ToString());
                            totalamount += Convert.ToDecimal(item["total"].ToString());



                            int jed = 0;

                            int chunkSize = 34;
                            int stringLength = desc.Length;
                            for (int i = 0; i < stringLength; i += chunkSize)
                            {
                                jed++;

                                if (i + chunkSize > stringLength) chunkSize = stringLength - i;

                                if (jed == 1)
                                {
                                    checklines();                                                                                                                                                                //string reference = "";
                                    MyPrinter.Print("           "+(char)69 + si.PadRight(12, ' ') + qty.PadLeft(10, ' ') + "  " + unit.PadRight(3, ' ') + "  " + desc.Substring(i, chunkSize).PadRight(34, ' ') + " " + uprice.PadLeft(16, ' ') + " " + amount.PadLeft(19, ' ') + " " + charging.PadRight(14, ' ') + " " + reference.PadRight(6, ' ') + "\r\n");
                                    lines++;
                                }
                                else
                                {
                                    checklines();
                                    MyPrinter.Print((char)15 + "           " + ("").PadRight(12, ' ') + ("").PadLeft(10, ' ') + "  " + ("").PadRight(3, ' ') + "  " + desc.Substring(i, chunkSize).PadRight(34, ' ') + " " + ("").PadLeft(16, ' ') + " " + ("").PadLeft(19, ' ') + " " + ("").PadRight(14, ' ') + " " + ("").PadRight(6, ' ') + "\r\n");
                                    lines++;
                                }
                            }
                        }


                        itemCode.Close();
                        checklines();
                        MyPrinter.Print((char)15 + "   " + "\r\n");
                        lines++;

                        checklines();
                        MyPrinter.Print((char)15 + "   " + ("").PadRight(84, ' ') + "TOTAL:" + string.Format("{0:#,##0.00}", Convert.ToDecimal(Math.Round(sum, 2).ToString())).PadLeft(21, ' ') + "\r\n");
                        lines++;
                    }
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
                MyPrinter.Print("\r\n");
                lines++;
                checklines();
                MyPrinter.Print("\r\n");
                lines++;
                checklines();
                MyPrinter.Print((char)15 + ("").PadRight(60, ' ') + "***  GRAND TOTAL  ***" + string.Format("{0:#,##0.00}", Convert.ToDecimal(totalamount.ToString())).PadLeft(28, ' ') + "\r\n");
                lines++;
                //label2.Text = string.Format("{0:#,##0.00}", totalamount);


                //string vp = "";
                //string shipper = "";
                //string date = "";
                //string via = "";
                //tblIn.Open();
                //String query1 = "SELECT * FROM tblVP WHERE poid = '" + item["poid"].ToString() + "'";
                //SqlCommand cmd1 = new SqlCommand(query1, tblIn);
                //SqlDataReader rdr1 = cmd1.ExecuteReader();
                //if (rdr1.Read())
                //{
                //    vp = rdr1["vpno"].ToString();
                //}
                //tblIn.Close();

                //tblIn.Open();
                //String query2 = "SELECT * FROM tblSI WHERE poid = '" + item["poid"].ToString() + "'";
                //SqlCommand cmd2 = new SqlCommand(query2, tblIn);
                //SqlDataReader rdr2 = cmd2.ExecuteReader();
                //if (rdr2.Read())
                //{
                //    via = rdr2["carno"].ToString();
                //    date = DateTime.Parse(rdr2["date"].ToString()).ToString("MM/dd/yyyy");
                //}
                //tblIn.Close();

                //tblIn.Open();
                //String query3 = "SELECT suppliername,Id FROM tblIn WHERE Id = '" + item["poid"].ToString() + "'";
                //SqlCommand cmd3 = new SqlCommand(query3, tblIn);
                //SqlDataReader rdr3 = cmd3.ExecuteReader();
                //if (rdr3.Read())
                //{
                //    shipper = rdr3["suppliername"].ToString();
                //}
                //tblIn.Close();

                //rdr3.Close();
                //rdr1.Close();
                //rdr2.Close();


                //checklines();
                //MyPrinter.Print((char)15 + "   " + vp.PadRight(8, ' ') + shipper.PadRight(32, ' ') + date.PadRight(37, ' ') + via + "\r\n");
                //lines++;
                //decimal sum = 0.00M;
                //using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                //{
                //    itemCode.Open();
                //    DataTable dt = new DataTable();
                //    using (SqlDataAdapter a2 = new SqlDataAdapter("SELECT * FROM itemCode WHERE poid = '" + item["poid"].ToString() + "'", itemCode))
                //        a2.Fill(dt);
                //    BindingSource bs = new BindingSource();
                //    bs.DataSource = dt;
                //    foreach (DataRow rdr in dt.Rows)
                //    {
                //        string si = "";
                //        string qty = "";
                //        string unit = "";
                //        string desc = "";
                //        string uprice = "";
                //        string amount = "";
                //        string charging = "";
                //        string reference = "";
                //        si = rdr["si"].ToString();
                //        qty = rdr["qty"].ToString();
                //        if (rdr["unit"].ToString().Length > 3)
                //        {
                //            unit = rdr["unit"].ToString().Substring(0,3);
                //        }
                //        else
                //        {
                //            unit = rdr["unit"].ToString();
                //        }
                //        desc = rdr["description"].ToString();
                //        uprice = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["cost"].ToString()));

                //        amount = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["total"].ToString()));
                //        charging = rdr["charging"].ToString();
                //        reference = "";

                //        int jed = 0;

                //        int chunkSize = 34;
                //        int stringLength = desc.Length;
                //        for (int i = 0; i < stringLength; i += chunkSize)
                //        {
                //            jed++;

                //            if (i + chunkSize > stringLength) chunkSize = stringLength - i;

                //            if (jed == 1)
                //            {
                //                checklines();                                                                                                                                                                //string reference = "";
                //                MyPrinter.Print((char)15 + "           " + si.PadRight(12, ' ') + qty.PadLeft(10, ' ')+"  " + unit.PadRight(3, ' ') +"  "+ desc.Substring(i, chunkSize).PadRight(34, ' ') +" "+uprice.PadLeft(16, ' ') + " " +amount.PadLeft(19,' ') + " " +charging.PadRight(14,' ') + " " +reference.PadRight(8,' ')+ "\r\n");
                //                lines++;
                //            }
                //            else
                //            {
                //                checklines();
                //                MyPrinter.Print((char)15 + "           " + ("").PadRight(12, ' ') + ("").PadLeft(10, ' ') + "  " + ("").PadRight(3, ' ') + "  " + desc.Substring(i, chunkSize).PadRight(34, ' ') + " " + ("").PadLeft(16, ' ') + " " + ("").PadLeft(19, ' ') + " " + ("").PadRight(14, ' ') + " " + ("").PadRight(8, ' ') + "\r\n");
                //                lines++;
                //            }
                //        }
                //        sum += Convert.ToDecimal(rdr["total"].ToString());
                //    }
                //    itemCode.Close();
                //}

                //total += sum;

                //checklines();
                //MyPrinter.Print("\r\n");
                //lines++;

                //checklines();
                //MyPrinter.Print((char)15 + "   " + ("").PadRight(84, ' ') + "TOTAL:" + string.Format("{0:#,##0.00}", Convert.ToDecimal(Math.Round(sum, 2).ToString())).PadLeft(21, ' ') + "\r\n");
                //lines++;

                //}

                //checklines();
                //MyPrinter.Print("\r\n");
                //lines++;
                //checklines();
                //MyPrinter.Print("\r\n");
                //lines++; 
                //checklines();
                //MyPrinter.Print("\r\n");
                //lines++; 
                //checklines();
                //MyPrinter.Print("\r\n");
                //lines++; 
                //checklines();
                //MyPrinter.Print("\r\n");
                //lines++;
                //checklines();
                //MyPrinter.Print((char)15 + ("").PadRight(60, ' ')+ "***  GRAND TOTAL  ***"+ string.Format("{0:#,##0.00}", Convert.ToDecimal(total.ToString())).PadLeft(28,' ') + "\r\n");
                //lines++;
                //label2.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(total.ToString()));
            }
        }
        int lines = 0;
        private void checklines()
        {
            if (lines == 50)
            {
                lines = 0;
                page++;
                MyPrinter.Print("\x0C");
                printHeader();
            }
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
