
namespace Inventory_Check
{
    partial class printprojsumfront
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.projsummary = new Inventory_Check.projsummary();
            this.tblprojsumfrontBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblprojsumfrontTableAdapter = new Inventory_Check.projsummaryTableAdapters.tblprojsumfrontTableAdapter();
            this.tblprojsumfrontdataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblprojsumfrontdataTableAdapter = new Inventory_Check.projsummaryTableAdapters.tblprojsumfrontdataTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.projsummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblprojsumfrontBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblprojsumfrontdataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "projsum";
            reportDataSource1.Value = this.tblprojsumfrontBindingSource;
            reportDataSource2.Name = "projsumdata";
            reportDataSource2.Value = this.tblprojsumfrontdataBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Inventory_Check.bin.Debug.projectsummary.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1001, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // projsummary
            // 
            this.projsummary.DataSetName = "projsummary";
            this.projsummary.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblprojsumfrontBindingSource
            // 
            this.tblprojsumfrontBindingSource.DataMember = "tblprojsumfront";
            this.tblprojsumfrontBindingSource.DataSource = this.projsummary;
            // 
            // tblprojsumfrontTableAdapter
            // 
            this.tblprojsumfrontTableAdapter.ClearBeforeFill = true;
            // 
            // tblprojsumfrontdataBindingSource
            // 
            this.tblprojsumfrontdataBindingSource.DataMember = "tblprojsumfrontdata";
            this.tblprojsumfrontdataBindingSource.DataSource = this.projsummary;
            // 
            // tblprojsumfrontdataTableAdapter
            // 
            this.tblprojsumfrontdataTableAdapter.ClearBeforeFill = true;
            // 
            // printprojsumfront
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 450);
            this.Controls.Add(this.reportViewer1);
            this.Name = "printprojsumfront";
            this.Text = "printprojsumfront";
            this.Load += new System.EventHandler(this.printprojsumfront_Load);
            ((System.ComponentModel.ISupportInitialize)(this.projsummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblprojsumfrontBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblprojsumfrontdataBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource tblprojsumfrontBindingSource;
        private projsummary projsummary;
        private System.Windows.Forms.BindingSource tblprojsumfrontdataBindingSource;
        private projsummaryTableAdapters.tblprojsumfrontTableAdapter tblprojsumfrontTableAdapter;
        private projsummaryTableAdapters.tblprojsumfrontdataTableAdapter tblprojsumfrontdataTableAdapter;
    }
}