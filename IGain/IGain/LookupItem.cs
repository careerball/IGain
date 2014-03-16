namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class LookupItem : Form
    {
        private Container components = null;
        public static OleDbConnection Con;
        public PGUserControl LookupItemReportBox;
        private NewAccountForm m_naFrm;
        private Button selectItemInfo;

        public LookupItem(NewAccountForm naFrm)
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.LookupItemReportBox.DBCon = Con;
                if (naFrm != null)
                {
                    this.m_naFrm = naFrm;
                }
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
            this.LookupItemReportBox = new PGUserControl();
            this.selectItemInfo = new Button();
            base.SuspendLayout();
            this.LookupItemReportBox.ConfigFile = "";
            this.LookupItemReportBox.DBCon = null;
            this.LookupItemReportBox.Location = new Point(0, 0);
            this.LookupItemReportBox.Name = "LookupItemReportBox";
            this.LookupItemReportBox.Size = new Size(0x2a0, 0x1c8);
            this.LookupItemReportBox.TabIndex = 0;
            this.LookupItemReportBox.TotallingEnabled = false;
            this.selectItemInfo.Location = new Point(0x220, 0x1d0);
            this.selectItemInfo.Name = "selectItemInfo";
            this.selectItemInfo.Size = new Size(0x70, 0x18);
            this.selectItemInfo.TabIndex = 1;
            this.selectItemInfo.Text = "Select";
            this.selectItemInfo.Click += new EventHandler(this.selectItemInfo_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2a0, 0x1ed);
            base.Controls.Add(this.selectItemInfo);
            base.Controls.Add(this.LookupItemReportBox);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "LookupItem";
            base.ShowInTaskbar = false;
            this.Text = "Lookup Item";
            base.ResumeLayout(false);
        }

        private void selectItemInfo_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_naFrm.AccSubType.TextBoxText = Convert.ToString(this.LookupItemReportBox.ReportGrid[this.LookupItemReportBox.ReportGrid.CurrentRowIndex, 0]) + "-" + Convert.ToString(this.LookupItemReportBox.ReportGrid[this.LookupItemReportBox.ReportGrid.CurrentRowIndex, 1]);
                this.m_naFrm.strItemCategoryID = Convert.ToString(this.LookupItemReportBox.ReportGrid[this.LookupItemReportBox.ReportGrid.CurrentRowIndex, 3]);
                base.Close();
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
            }
        }
    }
}

