
namespace Inventory_Check
{
    partial class projectsumdata
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
            this.projsumdata = new Inventory_Check.projsumdata();
            this.projectsumdataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.projectsumdataTableAdapter = new Inventory_Check.projsumdataTableAdapters.projectsumdataTableAdapter();
            this.projectsumdata1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.projectsumdata1TableAdapter = new Inventory_Check.projsumdataTableAdapters.projectsumdata1TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.projsumdata)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsumdataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsumdata1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.projectsumdataBindingSource;
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = this.projectsumdata1BindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Inventory_Check.bin.Debug.projectsumdata.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(800, 450);
            this.reportViewer1.TabIndex = 0;
            // 
            // projsumdata
            // 
            this.projsumdata.DataSetName = "projsumdata";
            this.projsumdata.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // projectsumdataBindingSource
            // 
            this.projectsumdataBindingSource.DataMember = "projectsumdata";
            this.projectsumdataBindingSource.DataSource = this.projsumdata;
            // 
            // projectsumdataTableAdapter
            // 
            this.projectsumdataTableAdapter.ClearBeforeFill = true;
            // 
            // projectsumdata1BindingSource
            // 
            this.projectsumdata1BindingSource.DataMember = "projectsumdata1";
            this.projectsumdata1BindingSource.DataSource = this.projsumdata;
            // 
            // projectsumdata1TableAdapter
            // 
            this.projectsumdata1TableAdapter.ClearBeforeFill = true;
            // 
            // projectsumdata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.reportViewer1);
            this.Name = "projectsumdata";
            this.Text = "projectsumdata";
            this.Load += new System.EventHandler(this.projectsumdata_Load);
            ((System.ComponentModel.ISupportInitialize)(this.projsumdata)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsumdataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsumdata1BindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource projectsumdataBindingSource;
        private projsumdata projsumdata;
        private System.Windows.Forms.BindingSource projectsumdata1BindingSource;
        private projsumdataTableAdapters.projectsumdataTableAdapter projectsumdataTableAdapter;
        private projsumdataTableAdapters.projectsumdata1TableAdapter projectsumdata1TableAdapter;
    }
}