namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class InterestPage : Form
    {
        private Container components = null;
        public static OleDbConnection Con;
        private ComboBox CreditDueDateBox;
        private string DateInCreditDueDateBox;
        private ContextMenu DebitGridRowContextMenu;
        private ContextMenu GridRowContextMenu;
        public PGUserControl InterestReportBox;
        private Button IssueCredit;
        private MenuItem RecieveCredit;
        private MenuItem RecieveMenu;
        private MenuItem ReissueMenu;
        private MenuItem ReturnCredit;

        public InterestPage()
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.InterestReportBox.DBCon = Con;
            }
        }

        private void CreditDueDateBox_Enter(object sender, EventArgs e)
        {
            try
            {
                this.CreditDueDateBox.Text = Convert.ToDateTime(this.CreditDueDateBox.Text).ToLongDateString();
            }
            catch (Exception)
            {
            }
        }

        private void CreditDueDateBox_Leave(object sender, EventArgs e)
        {
            try
            {
                this.CreditDueDateBox.Text = Convert.ToDateTime(this.CreditDueDateBox.Text).ToShortDateString();
                this.DateInCreditDueDateBox = this.CreditDueDateBox.Text;
            }
            catch (Exception)
            {
                this.CreditDueDateBox.Text = DateTime.Today.ToLongDateString();
                BusinessLogic.MyMessageBox("Please Enter a valid date. e.g. 1 Nov,2004");
                this.CreditDueDateBox.Focus();
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
            this.InterestReportBox = new PGUserControl();
            this.IssueCredit = new Button();
            this.GridRowContextMenu = new ContextMenu();
            this.ReissueMenu = new MenuItem();
            this.RecieveMenu = new MenuItem();
            this.DebitGridRowContextMenu = new ContextMenu();
            this.RecieveCredit = new MenuItem();
            this.ReturnCredit = new MenuItem();
            base.SuspendLayout();
            this.InterestReportBox.ConfigFile = "";
            this.InterestReportBox.DBCon = null;
            this.InterestReportBox.Location = new Point(8, 8);
            this.InterestReportBox.Name = "InterestReportBox";
            this.InterestReportBox.Size = new Size(0x2b8, 0x278);
            this.InterestReportBox.TabIndex = 0;
            this.InterestReportBox.TotallingEnabled = true;
            this.InterestReportBox.QueryParsed += new QueryParsed_Handler(this.InterestReportBox_QueryParsed);
            this.InterestReportBox.Total_Clicked += new Total_ClickedHandler(this.InterestReportBox_Total_Clicked);
            this.IssueCredit.Location = new Point(0x1c8, 0x260);
            this.IssueCredit.Name = "IssueCredit";
            this.IssueCredit.Size = new Size(0xd8, 0x18);
            this.IssueCredit.TabIndex = 1;
            this.IssueCredit.Text = "Issue New Credit..";
            this.IssueCredit.Click += new EventHandler(this.IssueCredit_Click);
            this.GridRowContextMenu.MenuItems.AddRange(new MenuItem[] { this.ReissueMenu, this.RecieveMenu });
            this.ReissueMenu.Index = 0;
            this.ReissueMenu.Text = "Re-Issue..";
            this.ReissueMenu.Click += new EventHandler(this.ReissueMenu_Click);
            this.RecieveMenu.Index = 1;
            this.RecieveMenu.Text = "Reclaim..";
            this.RecieveMenu.Click += new EventHandler(this.RecieveMenu_Click);
            this.DebitGridRowContextMenu.MenuItems.AddRange(new MenuItem[] { this.RecieveCredit, this.ReturnCredit });
            this.RecieveCredit.Index = 0;
            this.RecieveCredit.Text = "Recieve..";
            this.RecieveCredit.Click += new EventHandler(this.RecieveCredit_Click);
            this.ReturnCredit.Index = 1;
            this.ReturnCredit.Text = "Return..";
            this.ReturnCredit.Click += new EventHandler(this.ReturnCredit_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b8, 0x27d);
            base.Controls.Add(this.IssueCredit);
            base.Controls.Add(this.InterestReportBox);
            base.MaximizeBox = false;
            base.Name = "InterestPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manage Credit";
            base.Load += new EventHandler(this.InterestPage_Load);
            base.ResumeLayout(false);
        }

        private void InterestPage_Load(object sender, EventArgs e)
        {
            this.DateInCreditDueDateBox = DateTime.Today.ToShortDateString();
            foreach (Control control in this.InterestReportBox.QueryPanel.Controls)
            {
                try
                {
                    if (control.Name == "Credit_Domain")
                    {
                        this.CreditDueDateBox = (ComboBox) control;
                        this.CreditDueDateBox.Text = DateTime.Today.ToShortDateString();
                        this.DateInCreditDueDateBox = this.CreditDueDateBox.Text;
                        break;
                    }
                }
                catch (Exception exception)
                {
                    BusinessLogic.MyMessageBox(exception.Message);
                }
            }
            if (this.InterestReportBox.ReportGrid != null)
            {
                this.InterestReportBox.ReportGrid.MouseDown += new MouseEventHandler(this.InterestReportBox_ReportGrid_MouseDown);
                this.InterestReportBox.CriteriaBox.SelectedIndexChanged += new EventHandler(this.InterestReportBox_CriteriaBox_SelectedIndexChanged);
                if (this.CreditDueDateBox != null)
                {
                    this.CreditDueDateBox.Leave += new EventHandler(this.CreditDueDateBox_Leave);
                    this.CreditDueDateBox.Enter += new EventHandler(this.CreditDueDateBox_Enter);
                }
            }
        }

        private void InterestReportBox_CriteriaBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.InterestReportBox.CriteriaBox.SelectedIndex)
            {
                case 0:
                    this.IssueCredit.Text = "Issue New Credit..";
                    break;

                case 1:
                    this.IssueCredit.Text = "Recieve Fresh Credit..";
                    break;
            }
            foreach (Control control in this.InterestReportBox.QueryPanel.Controls)
            {
                try
                {
                    if (control.Name == "Credit_Domain")
                    {
                        this.CreditDueDateBox = (ComboBox) control;
                        this.CreditDueDateBox.Text = this.DateInCreditDueDateBox;
                        break;
                    }
                }
                catch (Exception exception)
                {
                    BusinessLogic.MyMessageBox(exception.Message);
                }
            }
            this.CreditDueDateBox.Leave += new EventHandler(this.CreditDueDateBox_Leave);
            this.CreditDueDateBox.Enter += new EventHandler(this.CreditDueDateBox_Enter);
        }

        private bool InterestReportBox_QueryParsed(string parsedQuery)
        {
            Exception exception;
            DataTable dataTable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(parsedQuery, Con);
            try
            {
                adapter.Fill(dataTable);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                BusinessLogic.MyMessageBox(exception.Message);
                return false;
            }
            try
            {
                dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["Name"] };
            }
            catch (Exception exception2)
            {
                exception = exception2;
                BusinessLogic.MyMessageBox("One or more accounts have more than 1 open instances.The account/s are invalid.");
                return false;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    double carriedInterest = 0.0;
                    DateTime transactionInitDate = Convert.ToDateTime(row["Date Of Transaction"]);
                    DateTime transactionEndDate = Convert.ToDateTime(this.CreditDueDateBox.Text);
                    string creditID = Convert.ToString(row["Credit ID"]);
                    string transactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated = Convert.ToString(row["Transaction Set"]);
                    string cmdText = null;
                    if (this.IssueCredit.Text == "Issue New Credit..")
                    {
                        cmdText = "SELECT Credit From Transactions where SequenceOfInterestApplication=0 and AccountCredited=(Select AccountID from AccountTypes where AccountType='Interest' and AccountSubType='" + row["Name"] + "')";
                    }
                    else
                    {
                        cmdText = "SELECT Credit From Transactions where SequenceOfInterestApplication=0 and AccountDebited=(Select AccountID from AccountTypes where AccountType='Interest' and AccountSubType='" + row["Name"] + "')";
                    }
                    carriedInterest = Convert.ToDouble(new OleDbCommand(cmdText, Con).ExecuteScalar());
                    OleDbDataAdapter adapter2 = new OleDbDataAdapter("SELECT * from Transactions where TransactionSet='" + transactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated + "'", Con);
                    DataTable table2 = new DataTable("Transactions");
                    if (0 == adapter2.Fill(table2))
                    {
                        throw new Exception("The Transaction Set is empty!");
                    }
                    double carriedPrinciple = Convert.ToDouble(table2.Select("SequenceOfInterestApplication=-1 and Len(CreditID) > 1")[0]["Debit"]);
                    string selectCommandText = "SELECT * FROM INTERESTSCHEMA WHERE InterestSchemaID=( Select distinct InterestSchemaID from CreditFlow where CreditID='" + creditID + "') ORDER BY SequenceOfInterestApplication ";
                    DataTable table3 = new DataTable();
                    OleDbDataAdapter adapter3 = new OleDbDataAdapter(selectCommandText, Con);
                    if (0 == adapter3.Fill(table3))
                    {
                        throw new Exception("The Interest Schema is empty!");
                    }
                    double num3 = BusinessLogic.InferAmountReturnableFromInterestSchema(table3, carriedPrinciple, transactionInitDate, transactionEndDate, carriedInterest, creditID, transactionSetIfCreditFlowAndTransactionsInterestRowsToBeUpdated, Con);
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    BusinessLogic.MyMessageBox(exception.Message);
                    return false;
                }
            }
            return true;
        }

        private void InterestReportBox_ReportGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                DataGrid.HitTestInfo info = this.InterestReportBox.ReportGrid.HitTest(e.X, e.Y);
                if (info.Type == DataGrid.HitTestType.RowHeader)
                {
                    this.InterestReportBox.ReportGrid.CurrentRowIndex = info.Row;
                    if (this.IssueCredit.Text == "Issue New Credit..")
                    {
                        this.GridRowContextMenu.Show(this.InterestReportBox.ReportGrid, new Point(e.X, e.Y));
                    }
                    else
                    {
                        this.DebitGridRowContextMenu.Show(this.InterestReportBox.ReportGrid, new Point(e.X, e.Y));
                    }
                }
            }
        }

        private void InterestReportBox_Total_Clicked(int ColumnIndex, double Total)
        {
            BusinessLogic.MyMessageBox(Total.ToString());
        }

        private void IssueCredit_Click(object sender, EventArgs e)
        {
            if (this.IssueCredit.Text == "Issue New Credit..")
            {
                CreditPage.Con = Con;
                new CreditPage().ShowDialog(this);
            }
            else
            {
                DebitPage.Con = Con;
                new DebitPage().ShowDialog(this);
            }
        }

        private void RecieveCredit_Click(object sender, EventArgs e)
        {
            DebitRecievePage.Con = Con;
            DebitRecievePage page = new DebitRecievePage();
            try
            {
                page.CarriedForwardInterest.Text = Convert.ToString((double) (Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 5]) + Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 6])));
                page.CarriedPrinciple.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 4]);
                page.Creditee.Items.Add(Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 0]));
                page.BeginDate.Text = this.CreditDueDateBox.Text;
                page.BeginDate.Enabled = false;
                page.IsSlipIssued.Checked = true;
                page.SlipNumber.Enabled = true;
                page.SlipNumber.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 2]);
                page.CreditIdToBeClosed = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 7]);
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Recieve failed");
                return;
            }
            page.ShowDialog(this);
        }

        private void RecieveMenu_Click(object sender, EventArgs e)
        {
            CreditRecievePage.Con = Con;
            CreditRecievePage page = new CreditRecievePage();
            try
            {
                page.CarriedForwardInterest.Text = Convert.ToString((double) (Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 5]) + Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 6])));
                page.CarriedPrinciple.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 4]);
                page.Creditee.Items.Add(Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 0]));
                page.BeginDate.Text = this.CreditDueDateBox.Text;
                page.BeginDate.Enabled = false;
                page.IsSlipIssued.Checked = true;
                page.SlipNumber.Enabled = true;
                page.SlipNumber.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 2]);
                page.CreditIdToBeClosed = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 7]);
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Reissue failed");
                return;
            }
            page.ShowDialog(this);
        }

        private void ReissueMenu_Click(object sender, EventArgs e)
        {
            CreditReissuePage.Con = Con;
            CreditReissuePage page = new CreditReissuePage();
            try
            {
                page.CarriedForwardInterest.Text = Convert.ToString((double) (Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 5]) + Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 6])));
                page.CarriedPrinciple.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 4]);
                page.Creditee.Items.Add(Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 0]));
                page.BeginDate.Text = this.CreditDueDateBox.Text;
                page.BeginDate.Enabled = false;
                page.IsSlipIssued.Checked = true;
                page.SlipNumber.Enabled = true;
                page.SlipNumber.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 2]);
                page.CreditIdToBeClosed = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 7]);
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Reissue failed");
                return;
            }
            page.ShowDialog(this);
        }

        private void ReturnCredit_Click(object sender, EventArgs e)
        {
            DebitReturnPage.Con = Con;
            DebitReturnPage page = new DebitReturnPage();
            try
            {
                page.CarriedForwardInterest.Text = Convert.ToString((double) (Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 5]) + Convert.ToDouble(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 6])));
                page.CarriedPrinciple.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 4]);
                page.Creditee.Items.Add(Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 0]));
                page.BeginDate.Text = this.CreditDueDateBox.Text;
                page.BeginDate.Enabled = false;
                page.IsSlipIssued.Checked = true;
                page.SlipNumber.Enabled = true;
                page.SlipNumber.Text = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 2]);
                page.CreditIdToBeClosed = Convert.ToString(this.InterestReportBox.ReportGrid[this.InterestReportBox.ReportGrid.CurrentRowIndex, 7]);
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Return failed");
                return;
            }
            page.ShowDialog(this);
        }
    }
}

