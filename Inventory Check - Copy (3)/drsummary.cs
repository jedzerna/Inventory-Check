using System;
using System.Collections;
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
    public partial class drsummary : Form
    {
        public drsummary()
        {
            InitializeComponent();
        }

        private void drsummary_Load(object sender, EventArgs e)
        {

        }
        string lb = "{";
        string rb = "}";
        private void drs()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DateTime from = datetime1.Value;
                DateTime to = datetime2.Value;

                dbDR.Open();
                DataTable dt = new DataTable();
                string list = "SELECT DISTINCT projectcode,projectname FROM tblDR WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND SV = '" + cmSV.Text + "' ORDER BY projectname ASC";
                SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
                command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                command.Fill(dt);
                foreach (DataRow item in dt.Rows)
                {
                    int c = dgv1.Rows.Add();
                    dgv1.Rows[c].Cells["Column13"].Value = item["projectcode"].ToString();
                    dgv1.Rows[c].Cells["Column2"].Value = item["projectname"].ToString();
                }
                dbDR.Close();
                dgv1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                dgv1.RowHeadersVisible = false;
            }
            foreach (DataGridViewRow row in dgv1.Rows)
            {
                DataTable dt = new DataTable();
                DateTime from = datetime1.Value;
                DateTime to = datetime2.Value;

                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    dbDR.Open();
                    string list = "SELECT totalamount FROM tblDR WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND projectcode = '" + row.Cells["Column13"].Value.ToString() + "' AND sv = '" + cmSV.Text + "'";

                    SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
                    command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                    command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                    command.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {

                        int c = dgv3.Rows.Add();
                        dgv3.Rows[c].Cells["totalamount1"].Value = item["totalamount"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                        //dataGridView2.Rows[c].Cells["five"].Value = item["unit"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                        //dataGridView2.Rows[c].Cells["six"].Value = item["description"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                        //dataGridView2.Rows[c].Cells["seven"].Value = item["selling"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                        //dataGridView2.Rows[c].Cells["eight"].Value = item["total"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");

                    }
                    dbDR.Close();
                }
                decimal sum = 0.00M;
                //decimal sum3 = 0.00M;
                for (int i = 0; i < dgv3.Rows.Count; i++)
                {
                    if (dgv3.Rows[i].Cells["totalamount1"].Value.ToString() != "")
                    {
                        sum += Math.Round(Convert.ToDecimal(dgv3.Rows[i].Cells["totalamount1"].Value), 2) * 1;
                    }

                }
                row.Cells["totalamount"].Value = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");
                dgv3.Rows.Clear();
                dt.Rows.Clear();
            }
            
            //using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            //{
            //    DateTime from = guna2DateTimePicker1.Value;
            //    DateTime to = guna2DateTimePicker2.Value;
            //    foreach (DataGridViewRow row in dataGridView1.Rows)
            //    {
            //        dbDR.Open();
            //        DataTable dt = new DataTable();
            //        string list = "Select projectname,datetime,totalamount from tblDR where datetime BETWEEN @date1 and @date2 and operation = 'Completed' and projectcode = '"+row.Cells["Column13"].Value.ToString()+"' ORDER BY projectname ASC";
            //        SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
            //        command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
            //        command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
            //        command.Fill(dt);
            //        dataGridView3.DataSource = dt;
            //        dbDR.Close();
            //        dataGridView3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            //        dataGridView3.RowHeadersVisible = false;
            //    }
            //}
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (cmSV.Text == "")
            {
                MessageBox.Show("Please Select Sales Voucher");
            }
            else
            {
                System.Threading.Thread thread =
              new System.Threading.Thread(new System.Threading.ThreadStart(first));
                thread.Start();


            }

        }
        private void first()
        {
            dgv1.BeginInvoke((Action)delegate ()
            {
                dgv2.BeginInvoke((Action)delegate ()
                {
                    dgv3.BeginInvoke((Action)delegate ()
                    {
                        dgv4.BeginInvoke((Action)delegate ()
                        {

                            dgv1.Rows.Clear();
                            dgv3.Rows.Clear();
                            drs();

                            dtLocalC.Rows.Clear();
                            dtLocalC.Columns.Clear();
                            dgv2.DataSource = null;
                            dgv2.Rows.Clear();
                            DateTime dat = DateTime.Now;
                            string date = dat.ToString("MM/dd/yyyy");
                            dgv4.Rows.Clear();

                            int add1 = dgv2.Rows.Add();
                            dgv2.Rows[add1].Cells["one"].Value = "Date Printed: " + date;
                            dgv2.Rows[add1].Cells["four"].Value = "";
                            dgv2.Rows[add1].Cells["five"].Value = "";
                            dgv2.Rows[add1].Cells["productcode"].Value = "";
                            dgv2.Rows[add1].Cells["six"].Value = "";
                            dgv2.Rows[add1].Cells["seven"].Value = "";
                            dgv2.Rows[add1].Cells["eight"].Value = "";
                            dgv2.Rows[add1].Cells["nine"].Value = "";

                            int add12 = dgv2.Rows.Add();
                            dgv2.Rows[add12].Cells["one"].Value = "Project Summary Report: " + cmSV.Text + "          for " + datetime1.Text + " for " + datetime2.Text;
                            dgv2.Rows[add12].Cells["four"].Value = "";
                            dgv2.Rows[add12].Cells["five"].Value = "";
                            dgv2.Rows[add12].Cells["productcode"].Value = "";
                            dgv2.Rows[add12].Cells["six"].Value = "";
                            dgv2.Rows[add12].Cells["seven"].Value = "";
                            dgv2.Rows[add12].Cells["eight"].Value = "";
                            dgv2.Rows[add12].Cells["nine"].Value = "";

                            int add3 = dgv2.Rows.Add();
                            dgv2.Rows[add3].Cells["one"].Value = "";
                            dgv2.Rows[add3].Cells["four"].Value = "";
                            dgv2.Rows[add3].Cells["five"].Value = "";
                            dgv2.Rows[add3].Cells["productcode"].Value = "";
                            dgv2.Rows[add3].Cells["six"].Value = "";
                            dgv2.Rows[add3].Cells["seven"].Value = "";
                            dgv2.Rows[add3].Cells["eight"].Value = "";
                            dgv2.Rows[add3].Cells["nine"].Value = "";

                            int add = dgv2.Rows.Add();
                            dgv2.Rows[add].Cells["one"].Value = "";
                            dgv2.Rows[add].Cells["four"].Value = "";
                            dgv2.Rows[add].Cells["five"].Value = "";
                            dgv2.Rows[add].Cells["productcode"].Value = "Code";
                            dgv2.Rows[add].Cells["six"].Value = "";
                            dgv2.Rows[add].Cells["seven"].Value = "Unit Price";
                            dgv2.Rows[add].Cells["eight"].Value = "Amount";
                            dgv2.Rows[add].Cells["nine"].Value = "Subtotal";


                            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                            {
                                DateTime from = datetime1.Value;
                                DateTime to = datetime2.Value;

                                dbDR.Open();
                                DataTable dt = new DataTable();
                                string list = "SELECT drnumber,datetime,projectcode,projectname,Id,totalamount FROM tblDR WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND sv = '" + cmSV.Text + "' ORDER BY projectname ASC";
                                SqlDataAdapter command = new SqlDataAdapter(list, dbDR);
                                command.SelectCommand.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                                command.SelectCommand.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));
                                command.Fill(dt);
                                foreach (DataRow item in dt.Rows)
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
                });
            });
        }

        DataTable dtLocalC = new DataTable();
        decimal sum = 0.00M;
        string project;
        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        private string projectcode1;
        private string projectcode2;
        private Int32 ids;
        private void compute()
        {
            dgv1.BeginInvoke((Action)delegate ()
            {
                dgv2.BeginInvoke((Action)delegate ()
                {
                    dgv3.BeginInvoke((Action)delegate ()
                    {
                        dgv4.BeginInvoke((Action)delegate ()
                        {
                            if (dgv4.Rows.Count == 0)
                            {
                                MessageBox.Show("No Records Found");
                            }
                            else
                            {
                                projectcode1 = dgv4.Rows[0].Cells["projectcode"].Value.ToString();
                                projectcode2 = "sa";
                                foreach (DataGridViewRow row in dgv4.Rows)
                                {
                                    if (row.Cells["projectcode"].Value.ToString() == projectcode2)
                                    {
                                        projectcode2 = row.Cells["projectcode"].Value.ToString();
                                    }
                                    else
                                    {
                                        int aaaa = dgv2.Rows.Add();
                                        dgv2.Rows[aaaa].Cells["one"].Value = "";
                                        projectcode2 = row.Cells["projectcode"].Value.ToString();
                                    }

                                    int a = dgv2.Rows.Add();
                                    dgv2.Rows[a].Cells["one"].Value = row.Cells["projectcode"].Value.ToString() + "  ***  ";
                                    dgv2.Rows[a].Cells["four"].Value = row.Cells["projectname"].Value.ToString();
                                    dgv2.Rows[a].Cells["eight"].Value = "";

                                    DateTime dtgrid = DateTime.Parse(row.Cells["datetime"].Value.ToString());
                                    string date = dtgrid.ToString("MM/dd/yyyy");
                                    int b = dgv2.Rows.Add();
                                    dgv2.Rows[b].Cells["one"].Value = "   " + date + Environment.NewLine+ "    " + row.Cells["drnumber"].Value.ToString();
                                    //dgv2.Rows[b].Cells["four"].Value =row.Cells["drnumber"].Value.ToString();
                                    dgv2.Rows[b].Cells["eight"].Value = "";

                                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                                    {
                                        itemCode.Open();
                                        SqlDataAdapter da = new SqlDataAdapter("SELECT qty,unit,description,selling,total,productcode FROM tblDRitemCode WHERE drid =  '" + row.Cells["Id"].Value.ToString() + "'", itemCode);
                                        DataTable dt = new DataTable();
                                        da.Fill(dt);
                                        foreach (DataRow item in dt.Rows)
                                        {

                                            int c = dgv2.Rows.Add();
                                            dgv2.Rows[c].Cells["four"].Value = item["qty"].ToString()+"  "+ item["unit"].ToString().PadLeft(8)+"     "+ item["productcode"].ToString();
                                            //dgv2.Rows[c].Cells["five"].Value =item["unit"].ToString();
                                            //dgv2.Rows[c].Cells["productcode"].Value = item["productcode"].ToString();
                                            dgv2.Rows[c].Cells["six"].Value = item["description"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                                            dgv2.Rows[c].Cells["seven"].Value = item["selling"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                                            dgv2.Rows[c].Cells["eight"].Value = item["total"].ToString().Replace("[", lb).Replace("]", rb).Replace("​*", "[*​]").Replace("%", "[%]").Replace("'", "''");
                                            //dgv2.Rows[c].Cells["productcode"].Value = "            ";



                                        }
                                        itemCode.Close();
                                    }
                                    if (row.Cells["projectcode"].Value.ToString() != projectcode1)
                                    {
                                        foreach (DataGridViewRow dgv in dgv1.Rows)
                                        {
                                            if (dgv.Cells["Column13"].Value.ToString() == row.Cells["projectcode"].Value.ToString())
                                            {
                                                int co = dgv2.Rows.Count;
                                                dgv2.Rows[co - 1].Cells["nine"].Value = dgv.Cells["totalamount"].Value.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        projectcode1 = row.Cells["projectcode"].Value.ToString();
                                    }
                                }
                                ids = 0;

                                string val = dgv1.Rows[0].Cells["Column13"].Value.ToString();

                                foreach (DataGridViewRow dgv4 in dgv4.Rows)
                                {

                                    if (val == dgv4.Cells["projectcode"].Value.ToString())
                                    {
                                        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                                        {
                                            var empcon = new SqlCommand("SELECT COUNT(drid) FROM [tblDRitemCode] WHERE drid = '" + dgv4.Cells["Id"].Value.ToString() + "'", itemCode);

                                            itemCode.Open();
                                            Int32 max = (Int32)empcon.ExecuteScalar();
                                            ids = ids + max;
                                            itemCode.Close();
                                        }
                                    }
                                }
                                ids = ids + 4;
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {
                                    DateTime from = datetime1.Value;
                                    DateTime to = datetime2.Value;

                                    var empcon = new SqlCommand("SELECT COUNT(Id) FROM [tblDR] WHERE datetime BETWEEN @date1 AND @date2 AND operation = 'Completed' AND sv = '" + cmSV.Text + "' AND projectcode = '" + val + "'", dbDR);

                                    empcon.Parameters.AddWithValue("@date1", Convert.ToDateTime(from.ToString("yyyy-MM-dd")));
                                    empcon.Parameters.AddWithValue("@date2", Convert.ToDateTime(to.ToString("yyyy-MM-dd")));

                                    dbDR.Open();
                                    Int32 max = (Int32)empcon.ExecuteScalar();
                                    ids = ids + (3 * max) - max;
                                    dbDR.Close();
                                }
                                dgv2.Rows[ids].Cells["nine"].Value = dgv1.Rows[0].Cells["totalamount"].Value.ToString();
                             
                                foreach (DataGridViewRow row in dgv2.Rows)
                                {
                                    if (row.Cells["one"].Value == null || row.Cells["one"].Value.ToString() == "")
                                    {
                                        row.Cells["one"].Value = "";
                                    }
                                    if (row.Cells["four"].Value == null || row.Cells["four"].Value.ToString() == "")
                                    {
                                        row.Cells["four"].Value = "";
                                    }
                                    if (row.Cells["five"].Value == null || row.Cells["five"].Value.ToString() == "")
                                    {
                                        row.Cells["five"].Value = "";
                                    }
                                    if (row.Cells["six"].Value == null || row.Cells["six"].Value.ToString() == "")
                                    {
                                        row.Cells["six"].Value = "";
                                    }
                                    if (row.Cells["seven"].Value == null || row.Cells["seven"].Value.ToString() == "")
                                    {
                                        row.Cells["seven"].Value = "";
                                    }
                                    if (row.Cells["eight"].Value == null || row.Cells["eight"].Value.ToString() == "")
                                    {
                                        row.Cells["eight"].Value = "";
                                    }
                                    if (row.Cells["nine"].Value == null || row.Cells["nine"].Value.ToString() == "")
                                    {
                                        row.Cells["nine"].Value = "";
                                    }
                                    if (row.Cells["productcode"].Value == null || row.Cells["productcode"].Value.ToString() == "")
                                    {
                                        row.Cells["productcode"].Value = "";
                                    }
                                }

                                dtLocalC.Columns.Add("one");
                                dtLocalC.Columns.Add("four");
                                dtLocalC.Columns.Add("five");
                                dtLocalC.Columns.Add("productcode");
                                dtLocalC.Columns.Add("six");
                                dtLocalC.Columns.Add("seven");
                                dtLocalC.Columns.Add("eight");
                                dtLocalC.Columns.Add("nine");


                                DataRow drLocal = null;
                                foreach (DataGridViewRow dr in dgv2.Rows)
                                {
                                    drLocal = dtLocalC.NewRow();
                                    drLocal["one"] = dr.Cells["one"].Value.ToString();
                                    drLocal["four"] = dr.Cells["four"].Value.ToString();
                                    drLocal["five"] = dr.Cells["five"].Value.ToString();
                                    drLocal["productcode"] = dr.Cells["productcode"].Value.ToString();
                                    drLocal["six"] = dr.Cells["six"].Value.ToString();
                                    drLocal["seven"] = dr.Cells["seven"].Value.ToString();
                                    drLocal["eight"] = dr.Cells["eight"].Value.ToString();
                                    drLocal["nine"] = dr.Cells["nine"].Value.ToString();
                                    dtLocalC.Rows.Add(drLocal);
                                }

                                RemoveDuplicateRows(dtLocalC, "one");
                                dgv2.Rows.Clear();
                                //dataGridView2.DataSource = dtLocalC;
                                foreach (DataRow item in dtLocalC.Rows)
                                {

                                    int c = dgv2.Rows.Add();
                                    dgv2.Rows[c].Cells["one"].Value = item["one"].ToString();
                                    dgv2.Rows[c].Cells["four"].Value = item["four"].ToString();
                                    dgv2.Rows[c].Cells["five"].Value = item["five"].ToString();
                                    dgv2.Rows[c].Cells["productcode"].Value = item["productcode"].ToString();
                                    dgv2.Rows[c].Cells["six"].Value = item["six"].ToString();
                                    dgv2.Rows[c].Cells["seven"].Value = item["seven"].ToString();
                                    dgv2.Rows[c].Cells["eight"].Value = item["eight"].ToString();
                                    dgv2.Rows[c].Cells["nine"].Value = item["nine"].ToString();

                                }

                                //double sum3 = 0.00;
                                //for (int i = 0; i < dgv1.Rows.Count; i++)
                                //{
                                //    sum3 += Convert.ToDouble(dgv1.Rows[i].Cells["totalamount"].Value) * 1;
                                //}
                                //int add = dgv1.Rows.Add();
                                //dgv1.Rows[add].Cells["totalamount"].Value = "";
                                //int add1 = dgv1.Rows.Add();
                                //dgv1.Rows[add1].Cells["totalamount"].Value = "";
                                //int ta = dgv1.Rows.Add();
                                //if (cmSV.Text == "Construction Material")
                                //{
                                //    sv1 = "Error 101";
                                //}
                                //else
                                //{
                                //    sv1 = "Error 102";
                                //}
                                //dgv1.Rows[ta].Cells["Column2"].Value = sv1.ToString() + " Sales of " + cmSV.Text;
                                //dgv1.Rows[ta].Cells["totalamount"].Value = Math.Round((double)Convert.ToDouble(sum3), 2).ToString("N2");

                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {

                                    string sqlTrunc = "TRUNCATE TABLE tblprojsumfront";
                                    SqlCommand cmd = new SqlCommand(sqlTrunc, dbDR);
                                    dbDR.Open();
                                    cmd.ExecuteNonQuery();
                                    dbDR.Close();

                                }
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {

                                    string sqlTrunc = "TRUNCATE TABLE tblprojsumfrontdata";
                                    SqlCommand cmd = new SqlCommand(sqlTrunc, dbDR);
                                    dbDR.Open();
                                    cmd.ExecuteNonQuery();
                                    dbDR.Close();

                                }
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {

                                    string sqlTrunc = "TRUNCATE TABLE projectsumdata1";
                                    SqlCommand cmd = new SqlCommand(sqlTrunc, dbDR);
                                    dbDR.Open();
                                    cmd.ExecuteNonQuery();
                                    dbDR.Close();

                                }
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {

                                    string sqlTrunc = "TRUNCATE TABLE projectsumdata";
                                    SqlCommand cmd = new SqlCommand(sqlTrunc, dbDR);
                                    dbDR.Open();
                                    cmd.ExecuteNonQuery();
                                    dbDR.Close();

                                }

                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {
                                    dbDR.Open();
                                    foreach (DataGridViewRow row in dgv1.Rows)
                                    {

                                        if (row.Cells["Column13"].Value.ToString() == "" || row.Cells["Column13"].Value == null)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            string insStmt2 = "insert into tblprojsumfrontdata ([code],[description],[amount]) values" +
                                                      " (@code,@description,@amount)";

                                            SqlCommand insCmd2 = new SqlCommand(insStmt2, dbDR);

                                            insCmd2.Parameters.AddWithValue("@code", row.Cells["Column13"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@description", row.Cells["Column2"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@amount", row.Cells["totalamount"].Value.ToString());

                                            insCmd2.ExecuteNonQuery();
                                        }


                                    }
                                    dbDR.Close();
                                }
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {
                                    int rowadd = 0;
                                    dbDR.Open();
                                    foreach (DataGridViewRow row in dgv2.Rows)
                                    {

                                        rowadd = 1 + rowadd;
                                        if (rowadd >= 6)
                                        {
                                            string insStmt2 = "insert into projectsumdata ([projectdr],[qty],[unit],[code],[description],[unitprice],[amount],[subtotal]) values" +
                                                         " (@projectdr,@qty,@unit,@code,@description,@unitprice,@amount,@subtotal)";

                                            SqlCommand insCmd2 = new SqlCommand(insStmt2, dbDR);

                                            insCmd2.Parameters.AddWithValue("@projectdr", row.Cells["one"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@qty", row.Cells["four"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@unit", row.Cells["five"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@code", row.Cells["productcode"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@description", row.Cells["six"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@unitprice", row.Cells["seven"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@amount", row.Cells["eight"].Value.ToString());
                                            insCmd2.Parameters.AddWithValue("@subtotal", row.Cells["nine"].Value.ToString());

                                            insCmd2.ExecuteNonQuery();

                                        }

                                    }
                                    dbDR.Close();
                                }
                                int dg2 = dgv1.Rows.Count;
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {
                                    dbDR.Open();
                                    string insStmt2 = "insert into projectsumdata1 ([date],[sv],[tota]) values" +
                                              " (@date,@sv,@tota)";
                                    SqlCommand insCmd2 = new SqlCommand(insStmt2, dbDR);
                                    DateTime dat = DateTime.Now;
                                    string td = dat.ToString("MM/dd/yyyy");
                                    insCmd2.Parameters.AddWithValue("@date", td.ToString());
                                    insCmd2.Parameters.AddWithValue("@sv", cmSV.Text);
                                    insCmd2.Parameters.AddWithValue("@tota", dgv1.Rows[dg2 - 1].Cells["totalamount"].Value.ToString());

                                    insCmd2.ExecuteNonQuery();
                                    dbDR.Close();
                                }

                                double sum4 = 0.00;
                                for (int i = 0; i < dgv1.Rows.Count; i++)
                                {
                                if (dgv1.Rows[i].Cells["totalamount"].Value != null || dgv1.Rows[i].Cells["totalamount"].Value.ToString()!= "")
                                    {
                                        sum4 += Convert.ToDouble(dgv1.Rows[i].Cells["totalamount"].Value) * 1;
                                    }
                                }
                                int add11 = dgv1.Rows.Add();
                                dgv1.Rows[add11].Cells["totalamount"].Value = "";
                                int add12 = dgv1.Rows.Add();
                                dgv1.Rows[add12].Cells["totalamount"].Value = "";
                                int ta1 = dgv1.Rows.Add();
                                if (cmSV.Text == "Construction Material")
                                {
                                    sv1 = "416.04";
                                }
                                else if (cmSV.Text == "Lumber")
                                {
                                    sv1 = "416.01";
                                }
                                else if (cmSV.Text == "CHB")
                                {
                                    sv1 = "416.02";
                                }
                                else if (cmSV.Text == "Sand & Gravel")
                                {
                                    sv1 = "416.05";
                                }
                                else if (cmSV.Text == "Ready - Mixed Conc")
                                {
                                    sv1 = "410.05";
                                }
                                else if (cmSV.Text == "Oxygen")
                                {
                                    sv1 = "416.07";
                                }
                                else if (cmSV.Text == "Direct Sales")
                                {
                                    sv1 = "110.03";
                                }
                                dgv1.Rows[ta1].Cells["Column2"].Value = sv1.ToString() + " Sales of " + cmSV.Text;
                                dgv1.Rows[ta1].Cells["totalamount"].Value = Math.Round((double)Convert.ToDouble(sum4), 2).ToString("N2");
                                int dg = dgv1.Rows.Count;
                                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                                {
                                    dbDR.Open();
                                    string insStmt2 = "insert into tblprojsumfront ([item],[sv],[date],[preparedby],[enteredby],[title],[totaldesc],[totalamount]) values" +
                                              " (@item,@sv,@date,@preparedby,@enteredby,@title,@totaldesc,@totalamount)";
                                    SqlCommand insCmd2 = new SqlCommand(insStmt2, dbDR);
                                    DateTime dat = DateTime.Now;
                                    string td = dat.ToString("MM/dd/yyyy");
                                    insCmd2.Parameters.AddWithValue("@item", cmSV.Text);
                                    insCmd2.Parameters.AddWithValue("@sv", "");
                                    insCmd2.Parameters.AddWithValue("@date", td.ToString());
                                    insCmd2.Parameters.AddWithValue("@preparedby", "Jed R. Zerna");
                                    insCmd2.Parameters.AddWithValue("@enteredby", "Jed R. Zerna");
                                    insCmd2.Parameters.AddWithValue("@title", "To record Sales of " + cmSV.Text + " period " + datetime1.Text + "-" + datetime2.Text);
                                    insCmd2.Parameters.AddWithValue("@totaldesc", dgv1.Rows[dg - 1].Cells["Column2"].Value.ToString());
                                    insCmd2.Parameters.AddWithValue("@totalamount", dgv1.Rows[dg - 1].Cells["totalamount"].Value.ToString());

                                    insCmd2.ExecuteNonQuery();
                                    dbDR.Close();
                                }
                                //copyAlltoClipboard1();
                                //Microsoft.Office.Interop.Excel.Application xlexcel;
                                //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                                //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                                //object misValue = System.Reflection.Missing.Value;
                                //xlexcel = new Microsoft.Office.Interop.Excel.Application();
                                //xlexcel.Visible = true;
                                //xlWorkBook = xlexcel.Workbooks.Add(misValue);
                                //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                                //Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
                                //CR.Select();
                                //xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                            }
                        });
                    });
                });
            });
        }
        string sv1;
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
        private void copyAlltoClipboard()
        {
            dgv2.SelectAll();
            DataObject dataObj = dgv2.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            printprojsumfront p = new printprojsumfront();
            p.Show();
            projectsumdata b = new projectsumdata();
            b.Show();
            //copyAlltoClipboard1();
            //Microsoft.Office.Interop.Excel.Application xlexcel;
            //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //object misValue = System.Reflection.Missing.Value;
            //xlexcel = new Microsoft.Office.Interop.Excel.Application();
            //xlexcel.Visible = true;
            //xlWorkBook = xlexcel.Workbooks.Add(misValue);
            //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            //CR.Select();
            //xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }
        private void copyAlltoClipboard1()
        {
            dgv2.SelectAll();
            DataObject dataObj = dgv2.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
    }
}
