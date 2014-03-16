namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class ReportingPage : Form
    {
        private Container components = null;
        public static OleDbConnection Con;
        public PGUserControl MorphReportControl;

        public ReportingPage(string ConfigFileName)
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.MorphReportControl.DBCon = Con;
                this.MorphReportControl.ConfigFile = @".\ReportDefs\" + ConfigFileName + ".xml";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.MorphReportControl = new PGUserControl();
            base.SuspendLayout();
            this.MorphReportControl.ConfigFile = "";
            this.MorphReportControl.DBCon = null;
            this.MorphReportControl.Location = new Point(0, 0);
            this.MorphReportControl.Name = "MorphReportControl";
            this.MorphReportControl.Size = new Size(0x2b0, 0x278);
            this.MorphReportControl.TabIndex = 0;
            this.MorphReportControl.TotallingEnabled = true;
            this.MorphReportControl.Search_Clicked += new Search_ClickedHandler(this.MorphReportControl_Search_Clicked);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b0, 0x26d);
            base.Controls.Add(this.MorphReportControl);
            base.MaximizeBox = false;
            base.Name = "ReportingPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Report";
            base.ResumeLayout(false);
        }

        private void MorphReportControl_Click(object sender, EventArgs e)
        {
            if (this.MorphReportControl.ConfigFile.ToUpper() != @".\REPORTDEFS\ITEMSREPORTBOX.XML")
            {
                this.MorphReportControl.Dispose();
                GC.Collect();
                this.MorphReportControl = null;
                this.MorphReportControl = new PGUserControl();
                this.MorphReportControl.DBCon = Con;
                this.MorphReportControl.ConfigFile = @".\ReportDefs\ItemsReportBox.xml";
                base.Controls.Add(this.MorphReportControl);
                this.MorphReportControl.Show();
            }
        }

        private void MorphReportControl_Search_Clicked(long RowsReturned, string MasterQuery)
        {
            MessageBox.Show(MasterQuery);
        }

        private void MorphReportControl_Total_Clicked(int ColumnIndex, double Total)
        {
            BusinessLogic.MyMessageBox(Convert.ToString(Total));
        }
    }
}

