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
    public partial class projectsumdata : Form
    {
        public projectsumdata()
        {
            InitializeComponent();
        }

        private void projectsumdata_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projsumdata.projectsumdata' table. You can move, or remove it, as needed.
            this.projectsumdataTableAdapter.Fill(this.projsumdata.projectsumdata);
            // TODO: This line of code loads data into the 'projsumdata.projectsumdata1' table. You can move, or remove it, as needed.
            this.projectsumdata1TableAdapter.Fill(this.projsumdata.projectsumdata1);
            reloadprint();
        }
        private void reloadprint()
        {
            var pagesetting = new System.Drawing.Printing.PageSettings();
            //pagesetting.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 850, 470);
            pagesetting.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            ReportDataSource dr = new ReportDataSource("projectsumdata", projectsumdataa());
            ReportDataSource prindr = new ReportDataSource("projectsumdata1", projectsumdata1());
            this.reportViewer1.LocalReport.ReportPath = "projectsumdata.rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(dr);
            this.reportViewer1.LocalReport.DataSources.Add(prindr);
            this.reportViewer1.SetPageSettings(pagesetting);
            this.reportViewer1.RefreshReport();
        }
        string item = "fawf";
         private DataTable projectsumdataa()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DataTable dt = new DataTable();
                dbDR.Open();
                SqlCommand cmd = new SqlCommand("SELECT [projectdr],[qty],[unit],[code],[description],[unitprice],[amount],[subtotal] FROM projectsumdata", dbDR);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dbDR.Close();
                return dt;
            }
        }
        private DataTable projectsumdata1()
        {
            using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
            {
                DataTable dt = new DataTable();
                dbDR.Open();
                SqlCommand cmd = new SqlCommand("SELECT [date],[sv],[tota] FROM projectsumdata1", dbDR);
                SqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dbDR.Close();
                return dt;
            }
        }
    }
}
