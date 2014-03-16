namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class AccountsPage : Form
    {
        public PGUserControl AccountsReportBox;
        private Container components = null;
        public static OleDbConnection Con;
        private Button CreateNewAccount;
        private MenuItem DeleteRow;
        private MenuItem EditRow;
        private ContextMenu GridRowContextMenu;

        public AccountsPage()
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.AccountsReportBox.DBCon = Con;
                if (this.AccountsReportBox.ReportGrid != null)
                {
                    this.AccountsReportBox.ReportGrid.MouseDown += new MouseEventHandler(this.AccountsReportBox_ReportGrid_MouseDown);
                }
                this.AccountsReportBox.BeforeSearch_Clicked += new BeforeSearch_ClickedHandler(this.AccountsReportBox_BeforeSearchClicked);
            }
        }

        private void AccountsPage_Load(object sender, EventArgs e)
        {
        }

        private bool AccountsReportBox_BeforeSearchClicked()
        {
            return true;
        }

        private void AccountsReportBox_ReportGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                DataGrid.HitTestInfo info = this.AccountsReportBox.ReportGrid.HitTest(e.X, e.Y);
                if (info.Type == DataGrid.HitTestType.RowHeader)
                {
                    this.AccountsReportBox.ReportGrid.CurrentRowIndex = info.Row;
                    this.GridRowContextMenu.Show(this.AccountsReportBox.ReportGrid, new Point(e.X, e.Y));
                }
            }
        }

        private void AccountsReportBox_Total_Clicked(int ColumnIndex, double Total)
        {
            BusinessLogic.MyMessageBox(Total.ToString());
        }

        private void CreateNewAccount_Click(object sender, EventArgs e)
        {
            NewAccountForm.CurCon = Con;
            NewAccountForm form = new NewAccountForm();
            form.strItemCategoryID = null;
            form.ShowDialog(this);
        }

        private void DeleteRow_Click(object sender, EventArgs e)
        {
            string str = null;
            string str2 = null;
            string str3 = null;
            string queryToValidate = null;
            Exception exception;
            try
            {
                str = (string) this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 0];
            }
            catch (Exception exception1)
            {
                exception = exception1;
                BusinessLogic.MyMessageBox(exception.Message);
                return;
            }
            try
            {
                str2 = (string) this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 1];
            }
            catch (Exception exception3)
            {
                exception = exception3;
                BusinessLogic.MyMessageBox(exception.Message);
                return;
            }
            str3 = " WHERE AccountType='" + str + "' and AccountSubType='" + str2 + "' ";
            queryToValidate = " SELECT Count(*) from Accounts where AccountID=(Select AccountID from AccountTypes " + str3 + ")";
            string queryToRun = "Delete from Accounttypes" + str3;
            if (BusinessLogic.MyMessageBox("Are you sure you want to remove this account?", "Remove?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                try
                {
                    Exception exception2 = BusinessLogic.ModifyStoreHouse(Con, queryToRun, queryToValidate);
                    if (exception2 != null)
                    {
                        BusinessLogic.MyMessageBox(exception2.Message);
                    }
                    else
                    {
                        BusinessLogic.MyMessageBox("Account Removed");
                    }
                }
                catch (Exception exception4)
                {
                    exception = exception4;
                    BusinessLogic.MyMessageBox(exception.Message);
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

        private void EditRow_Click(object sender, EventArgs e)
        {
            string str = null;
            string str2 = null;
            string str3 = null;
            Exception exception;
            try
            {
                str = (this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 0] == DBNull.Value) ? "" : ((string) this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 0]);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                str = "";
            }
            try
            {
                str2 = (this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 1] == DBNull.Value) ? "" : ((string) this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 1]);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                str2 = "";
            }
            try
            {
                str3 = (this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 2] == DBNull.Value) ? "A description for this account" : ((string) this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 2]);
            }
            catch (Exception exception3)
            {
                exception = exception3;
                str3 = "A description for this account";
            }
            NewAccountForm.CurCon = Con;
            NewAccountForm.ReplacementQueryPart = " WHERE AccountType='" + str + "' and AccountSubType='" + str2 + "' ";
            NewAccountForm.ValidationQuery = null;
            NewAccountForm form = new NewAccountForm();
            form.strItemCategoryID = null;
            form.AccType.TextBoxText = str;
            form.AccSubType.TextBoxText = str2;
            form.DescBox.Text = str3;
            form.Text = "Update account";
            form.CreateNewAccount.Text = "Update";
            form.checkBox1.Visible = false;
            if ((this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 3] != DBNull.Value) && Convert.ToBoolean(this.AccountsReportBox.ReportGrid[this.AccountsReportBox.ReportGrid.CurrentRowIndex, 3]))
            {
                form.AccSubType.Enabled = false;
            }
            form.ShowDialog(this);
        }

        private void InitializeComponent()
        {
            this.AccountsReportBox = new PGUserControl();
            this.CreateNewAccount = new Button();
            this.GridRowContextMenu = new ContextMenu();
            this.EditRow = new MenuItem();
            this.DeleteRow = new MenuItem();
            base.SuspendLayout();
            this.AccountsReportBox.BackColor = SystemColors.Control;
            this.AccountsReportBox.ConfigFile = "";
            this.AccountsReportBox.DBCon = null;
            this.AccountsReportBox.Name = "AccountsReportBox";
            this.AccountsReportBox.Size = new Size(0x2b0, 0x278);
            this.AccountsReportBox.TabIndex = 0;
            this.AccountsReportBox.TotallingEnabled = true;
            this.AccountsReportBox.Total_Clicked += new Total_ClickedHandler(this.AccountsReportBox_Total_Clicked);
            this.CreateNewAccount.Location = new Point(0x1d0, 600);
            this.CreateNewAccount.Name = "CreateNewAccount";
            this.CreateNewAccount.Size = new Size(200, 0x17);
            this.CreateNewAccount.TabIndex = 1;
            this.CreateNewAccount.Text = "Create New Account...";
            this.CreateNewAccount.Click += new EventHandler(this.CreateNewAccount_Click);
            this.GridRowContextMenu.MenuItems.AddRange(new MenuItem[] { this.EditRow, this.DeleteRow });
            this.EditRow.Index = 0;
            this.EditRow.Text = "Edit..";
            this.EditRow.Click += new EventHandler(this.EditRow_Click);
            this.DeleteRow.Index = 1;
            this.DeleteRow.Text = "Delete";
            this.DeleteRow.Click += new EventHandler(this.DeleteRow_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b8, 0x27d);
            base.Controls.AddRange(new Control[] { this.CreateNewAccount, this.AccountsReportBox });
            base.MaximizeBox = false;
            base.Name = "AccountsPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manage Accounts";
            base.Load += new EventHandler(this.AccountsPage_Load);
            base.ResumeLayout(false);
        }
    }
}

