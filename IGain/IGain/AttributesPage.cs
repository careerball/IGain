namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class AttributesPage : Form
    {
        public PGUserControl AttributesReportBox;
        private Container components = null;
        public static OleDbConnection Con = null;
        private Button ManageAttributes;

        public AttributesPage()
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.AttributesReportBox.DBCon = Con;
            }
        }

        private void AttributesReportBox_Search_Clicked(long RowsReturned, string MasterQuery)
        {
            string str = MasterQuery;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                base.Events.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ManageAttributes = new Button();
            this.AttributesReportBox = new PGUserControl();
            base.SuspendLayout();
            this.ManageAttributes.Location = new Point(0x1e8, 600);
            this.ManageAttributes.Name = "ManageAttributes";
            this.ManageAttributes.Size = new Size(0xb8, 0x20);
            this.ManageAttributes.TabIndex = 1;
            this.ManageAttributes.Text = "Attributes..";
            this.ManageAttributes.Click += new EventHandler(this.ManageAttributes_Click);
            this.AttributesReportBox.ConfigFile = "";
            this.AttributesReportBox.DBCon = null;
            this.AttributesReportBox.Name = "AttributesReportBox";
            this.AttributesReportBox.Size = new Size(0x2b0, 600);
            this.AttributesReportBox.TabIndex = 2;
            this.AttributesReportBox.TotallingEnabled = true;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b0, 0x285);
            base.Controls.AddRange(new Control[] { this.AttributesReportBox, this.ManageAttributes });
            base.MaximizeBox = false;
            base.Name = "AttributesPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Configure Attributes";
            base.ResumeLayout(false);
        }

        private void ManageAttributes_Click(object sender, EventArgs e)
        {
            if (AttributesForm.Con == null)
            {
                AttributesForm.Con = Con;
            }
            if (this.AttributesReportBox.ReportGrid.DataSource == null)
            {
                BusinessLogic.MyMessageBox("Please fetch some data into the report first! ");
            }
            else
            {
                string text = this.AttributesReportBox.CriteriaBox.Text;
                int currentRowIndex = this.AttributesReportBox.ReportGrid.CurrentRowIndex;
                int length = text.Length;
                if (text.IndexOf("Stock", 0, length) > 1)
                {
                    string str2 = Convert.ToString(this.AttributesReportBox.ReportGrid[currentRowIndex, 7]);
                    AttributesForm.srcCommand = "SELECT TOP 1 STOCKATTRIBUTES FROM STOCK WHERE ITEMCATEGORYID='" + str2 + "'";
                    AttributesForm.srcColumn = " STOCKATTRIBUTES ";
                    AttributesForm.srcTable = " Stock ";
                    AttributesForm.whereQueryPartOfUpdate = " WHERE ITEMCATEGORYID='" + str2 + "'";
                }
                else
                {
                    Exception exception;
                    if (text.IndexOf("Packings", 0, length) > 1)
                    {
                        string str3;
                        try
                        {
                            str3 = Convert.ToString(this.AttributesReportBox.ReportGrid[currentRowIndex, 10]);
                            if (str3 == null)
                            {
                                throw new Exception("Error! Invalid data!");
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            BusinessLogic.MyMessageBox(exception.Message);
                            return;
                        }
                        AttributesForm.srcCommand = "SELECT TOP 1 PACKINGATTRIBUTES FROM PACKINGS WHERE PACKINGID='" + str3 + "'";
                        AttributesForm.srcColumn = " PACKINGATTRIBUTES ";
                        AttributesForm.srcTable = " Packings ";
                        AttributesForm.whereQueryPartOfUpdate = " WHERE PACKINGID='" + str3 + "'";
                    }
                    else if (text.IndexOf("Sellings", 0, length) > 1)
                    {
                        string str4;
                        try
                        {
                            str4 = Convert.ToString(this.AttributesReportBox.ReportGrid[currentRowIndex, 10]);
                            if (str4 == null)
                            {
                                throw new Exception("Error! Invalid data!");
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            BusinessLogic.MyMessageBox("You can not set or view the attributes for this table.\nPlease navigate back to the parent table and then select attributes.");
                            return;
                        }
                        AttributesForm.srcCommand = "SELECT TOP 1 SELLINGATTRIBUTES FROM SELLINGS WHERE SELLINGID='" + str4 + "'";
                        AttributesForm.srcColumn = " SELLINGATTRIBUTES ";
                        AttributesForm.srcTable = " Sellings ";
                        AttributesForm.whereQueryPartOfUpdate = " WHERE SELLINGID='" + str4 + "'";
                    }
                    else if (text.IndexOf("Purchases", 0, length) > 1)
                    {
                        string str5 = Convert.ToString(this.AttributesReportBox.ReportGrid[currentRowIndex, 11]);
                        AttributesForm.srcCommand = "SELECT TOP 1 TRANSACTIONATTRIBUTES FROM TRANSACTIONS WHERE TRANSACTIONSET='" + str5 + "'";
                        AttributesForm.srcColumn = " TRANSACTIONATTRIBUTES ";
                        AttributesForm.srcTable = " TRANSACTIONS ";
                        AttributesForm.whereQueryPartOfUpdate = " WHERE TRANSACTIONSET='" + str5 + "'";
                    }
                }
                AttributesForm form = new AttributesForm();
                form.AttributesGrid.CaptionText = (AttributesForm.srcTable == " TRANSACTIONS ") ? " Purchases " : AttributesForm.srcTable;
                form.ShowDialog(this);
                form.Dispose();
            }
        }
    }
}

