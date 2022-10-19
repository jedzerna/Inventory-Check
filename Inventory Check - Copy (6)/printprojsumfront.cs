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
    public partial class printprojsumfront : Form
    {
        public printprojsumfront()
        {
            InitializeComponent();
        }

        private void printprojsumfront_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projsummary.tblprojsumfront' table. You can move, or remove it, as needed.
            this.tblprojsumfrontTableAdapter.Fill(this.projsummary.tblprojsumfront);
            // TODO: This line of code loads data into the 'projsummary.tblprojsumfrontdata' table. You can move, or remove it, as needed.
            this.tblprojsumfrontdataTableAdapter.Fill(this.projsummary.tblprojsumfrontdata);
            reloadprint();
        }
        private void reloadprint()
        {
            var pagesetting = new System.Drawing.Printing.PageSettings();
            //pagesetting.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 850, 470);
            pagesetting.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            ReportDataSource dr = new ReportDataSource("tblprojsumfront", tblprojsumfront());
            ReportDataSource prindr = new ReportDataSource("tblprojsumfrontdata", tblprojsumfrontdata());
            this.reportViewer1.LocalReport.ReportPath = "projectsummary.rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(dr);
            this.reportViewer1.LocalReport.DataSources.Add(prindr);
            this.reportViewer1.SetPageSettings(pagesetting);
            this.reportViewer1.RefreshReport();
        }
        string item = "fawf";
        private DataTable tblprojsumfront()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DataTable dt = new DataTable();
                dbDR.Open();
                SqlCommand cmd = new SqlCommand("SELECT item,sv,date,preparedby,enteredby,title FROM tblprojsumfront", dbDR);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dbDR.Close();
                return dt;
            }
        }
        private DataTable tblprojsumfrontdata()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DataTable dt = new DataTable();
                dbDR.Open();
                SqlCommand cmd = new SqlCommand("SELECT code,description,amount FROM tblprojsumfrontdata", dbDR);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd); 
                dbDR.Close();
                return dt;
            }
        }
    }
}
