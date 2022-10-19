using Microsoft.Reporting.WinForms;
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
    public partial class reportviewerqq : Form
    {
        public string id;
        public string drid;
        public reportviewerqq()
        {
            InitializeComponent();
        }

        private void reportviewerqq_Load(object sender, EventArgs e)
        {
            reloadprint();

        }
        private void reloadprint()
        {
            var pagesetting = new System.Drawing.Printing.PageSettings();
            pagesetting.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 850, 470);
            pagesetting.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            ReportDataSource dr = new ReportDataSource("DataSet1", tblDR());
            ReportDataSource prindr = new ReportDataSource("DataSet2", printDR());
            this.reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(dr);
            this.reportViewer1.LocalReport.DataSources.Add(prindr);
            this.reportViewer1.SetPageSettings(pagesetting);
            this.reportViewer1.RefreshReport();
        }
        private DataTable tblDR()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DataTable dt = new DataTable();
                dbDR.Open();
                SqlCommand cmd = new SqlCommand("SELECT projectname,datetime,ponumber,podate FROM tblDR WHERE id = '" + drid + "'", dbDR);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dbDR.Close();
                return dt;
            }
        }
        private DataTable printDR()
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                DataTable dt = new DataTable();
                itemCode.Open();
                SqlCommand cmd = new SqlCommand("SELECT qty,unit,description FROM printDR", itemCode);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                itemCode.Close();
                return dt;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void label1_Click(object sender, EventArgs e)
        {
          
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
