namespace IGain
{
    using PGBusinessLogic;
    using PGCookie;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class NewAccountForm : Form
    {
        private DataSet accountAttributeSet;
        private DataGrid AccountAttributesGrid;
        public PGCookie.PGCookie AccSubType;
        public PGCookie.PGCookie AccType;
        private string attributeSchemaXml;
        private DataTable attributesTable;
        public CheckBox checkBox1;
        private Container components = null;
        public Button CreateNewAccount;
        public static OleDbConnection CurCon;
        public RichTextBox DescBox;
        private DataTable dtAccSubType;
        private DataTable dtAccType;
        private Label label1;
        private Label label2;
        private Label label3;
        public static string ReplacementQueryPart = null;
        public string strItemCategoryID;
        public static string ValidationQuery = null;

        public NewAccountForm()
        {
            this.InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.AccSubType.TextBoxText = null;
                this.AccSubType.SourceDataTable = null;
                this.AccSubType.BoundColumn = null;
                this.AccSubType.CacheList.Visible = false;
                this.AccType.CacheList.Visible = false;
                this.AccSubType.Enabled = false;
                LookupItem.Con = CurCon;
                new LookupItem(this).ShowDialog(this);
            }
            else
            {
                this.AccSubType.Enabled = true;
                this.AccSubType.TextBoxText = null;
                this.AccSubType.SourceDataTable = this.dtAccSubType;
                this.AccSubType.BoundColumn = "AccountSubType";
                this.AccSubType.CacheList.Visible = false;
                this.AccType.CacheList.Visible = false;
            }
        }

        private void CreateNewAccount_Click(object sender, EventArgs e)
        {
            if (CurCon == null)
            {
                BusinessLogic.MyMessageBox("Connection is invalid");
            }
            else if (this.AccType.TextBoxText.Length == 0)
            {
                BusinessLogic.MyMessageBox("Account Type can not be empty");
                this.AccType.Focus();
            }
            else if (this.AccSubType.TextBoxText.Length == 0)
            {
                BusinessLogic.MyMessageBox("Account SubType can not be empty");
                this.AccSubType.Focus();
            }
            else
            {
                Exception exception2;
                if (this.CreateNewAccount.Text == "Update")
                {
                    string queryToRun = "Update AccountTypes Set AccountType='" + this.AccType.TextBoxText + "', AccountSubType='" + this.AccSubType.TextBoxText + "', AccountDescription='" + this.DescBox.Text + "',AccountAttributes='" + this.accountAttributeSet.GetXml() + "'";
                    if (ReplacementQueryPart == null)
                    {
                        BusinessLogic.MyMessageBox("Query is incomplete.Account not updated");
                    }
                    else if (ReplacementQueryPart.Length < 6)
                    {
                        BusinessLogic.MyMessageBox("Query is incomplete.Account not updated");
                    }
                    else
                    {
                        queryToRun = queryToRun + ReplacementQueryPart;
                        try
                        {
                            Exception exception = BusinessLogic.ModifyStoreHouse(CurCon, queryToRun, ValidationQuery);
                            if (exception != null)
                            {
                                BusinessLogic.MyMessageBox(exception.Message);
                            }
                            else
                            {
                                BusinessLogic.MyMessageBox("Account Updated");
                                base.Close();
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception2 = exception1;
                            BusinessLogic.MyMessageBox(exception2.Message);
                        }
                    }
                }
                else
                {
                    OleDbTransaction transaction;
                    OleDbCommand command = new OleDbCommand();
                    try
                    {
                        transaction = CurCon.BeginTransaction(IsolationLevel.ReadCommitted);
                        command.Connection = CurCon;
                        command.Transaction = transaction;
                    }
                    catch (Exception exception3)
                    {
                        exception2 = exception3;
                        BusinessLogic.MyMessageBox(exception2.Message);
                        return;
                    }
                    try
                    {
                        if ((this.strItemCategoryID == null) || (this.strItemCategoryID.Length < 1))
                        {
                            command.CommandText = "Insert into AccountTypes(AccountID,AccountType,AccountSubType,AccountDescription,AccountAttributes) values('" + this.AccType.TextBoxText + "-" + this.AccSubType.TextBoxText + "','" + this.AccType.TextBoxText + "','" + this.AccSubType.TextBoxText + "','" + this.DescBox.Text + "','" + this.accountAttributeSet.GetXml() + "')";
                        }
                        else
                        {
                            command.CommandText = "Insert into AccountTypes(AccountID,AccountType,AccountSubType,AccountDescription,AccountAttributes,ItemCategoryID) values('" + this.AccType.TextBoxText + "-" + this.AccSubType.TextBoxText + "','" + this.AccType.TextBoxText + "','" + this.AccSubType.TextBoxText + "','" + this.DescBox.Text + "','" + this.accountAttributeSet.GetXml() + "','" + this.strItemCategoryID + "')";
                        }
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        BusinessLogic.MyMessageBox("Account Created Successfully!");
                        base.Close();
                    }
                    catch (Exception exception4)
                    {
                        exception2 = exception4;
                        transaction.Rollback();
                        BusinessLogic.MyMessageBox(exception2.Message);
                        BusinessLogic.MyMessageBox("Account creation failed");
                    }
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
            this.AccType = new PGCookie.PGCookie();
            this.label1 = new Label();
            this.AccSubType = new PGCookie.PGCookie();
            this.label2 = new Label();
            this.label3 = new Label();
            this.DescBox = new RichTextBox();
            this.CreateNewAccount = new Button();
            this.AccountAttributesGrid = new DataGrid();
            this.checkBox1 = new CheckBox();
            this.AccountAttributesGrid.BeginInit();
            base.SuspendLayout();
            this.AccType.BoundColumn = null;
            this.AccType.Location = new Point(40, 40);
            this.AccType.Name = "AccType";
            this.AccType.Size = new Size(0x90, 160);
            this.AccType.TabIndex = 0;
            this.AccType.TextBoxText = "";
            this.label1.Location = new Point(0x30, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(120, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account Type :-";
            this.AccSubType.BoundColumn = null;
            this.AccSubType.Location = new Point(0xe0, 40);
            this.AccSubType.Name = "AccSubType";
            this.AccSubType.Size = new Size(160, 160);
            this.AccSubType.TabIndex = 1;
            this.AccSubType.TextBoxText = "";
            this.label2.Location = new Point(0xd8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x88, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "Account Of :-";
            this.label3.Location = new Point(400, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x68, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description :-";
            this.DescBox.Location = new Point(0x1a0, 40);
            this.DescBox.Name = "DescBox";
            this.DescBox.Size = new Size(0xc0, 0x98);
            this.DescBox.TabIndex = 2;
            this.DescBox.Text = "A description for the account";
            this.CreateNewAccount.Location = new Point(0x198, 0x1a0);
            this.CreateNewAccount.Name = "CreateNewAccount";
            this.CreateNewAccount.Size = new Size(0xc0, 0x20);
            this.CreateNewAccount.TabIndex = 5;
            this.CreateNewAccount.Text = "Create Account";
            this.CreateNewAccount.Click += new EventHandler(this.CreateNewAccount_Click);
            this.AccountAttributesGrid.CaptionText = "Account Attributes";
            this.AccountAttributesGrid.DataMember = "";
            this.AccountAttributesGrid.HeaderForeColor = SystemColors.ControlText;
            this.AccountAttributesGrid.Location = new Point(40, 0xe0);
            this.AccountAttributesGrid.Name = "AccountAttributesGrid";
            this.AccountAttributesGrid.PreferredColumnWidth = 200;
            this.AccountAttributesGrid.RowHeadersVisible = false;
            this.AccountAttributesGrid.RowHeaderWidth = 200;
            this.AccountAttributesGrid.Size = new Size(0x238, 0xb0);
            this.AccountAttributesGrid.TabIndex = 8;
            this.checkBox1.Location = new Point(0x1a0, 200);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(120, 0x10);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Is an Item";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x270, 0x1cd);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.AccountAttributesGrid);
            base.Controls.Add(this.CreateNewAccount);
            base.Controls.Add(this.DescBox);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.AccSubType);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.AccType);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "NewAccountForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "New Account Form";
            base.Load += new EventHandler(this.NewAccountForm_Load);
            this.AccountAttributesGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void NewAccountForm_Load(object sender, EventArgs e)
        {
            if (CurCon != null)
            {
                Exception exception;
                this.dtAccType = new DataTable();
                this.dtAccSubType = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("Select distinct AccountType from AccountTypes", CurCon);
                try
                {
                    adapter.Fill(this.dtAccType);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    BusinessLogic.MyMessageBox(exception.Message);
                    return;
                }
                adapter.SelectCommand = new OleDbCommand("Select distinct AccountSubType from AccountTypes where ItemcategoryID is null or len(ItemCategoryID)< 1 ", CurCon);
                try
                {
                    adapter.Fill(this.dtAccSubType);
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    BusinessLogic.MyMessageBox(exception.Message);
                    return;
                }
                adapter.Dispose();
                this.AccType.SourceDataTable = this.dtAccType;
                this.AccType.BoundColumn = "AccountType";
                this.AccType.CacheList.Visible = false;
                this.AccSubType.SourceDataTable = this.dtAccSubType;
                this.AccSubType.BoundColumn = "AccountSubType";
                this.AccSubType.CacheList.Visible = false;
                try
                {
                    this.attributeSchemaXml = Convert.ToString(new OleDbCommand("Select top 1 AccountAttributes from AccountTypes where AccountType='" + this.AccType.TextBoxText + "' and AccountSubType='" + this.AccSubType.TextBoxText + "' ", CurCon).ExecuteScalar());
                    this.accountAttributeSet = new DataSet();
                    if ((this.attributeSchemaXml == null) || (this.attributeSchemaXml.Length < 1))
                    {
                        this.attributesTable = this.accountAttributeSet.Tables.Add("AttributesTable");
                        this.attributesTable.Columns.Add("Attribute Name");
                        this.attributesTable.Columns.Add("Attribute Value");
                        DataRow row = this.attributesTable.NewRow();
                        row["Attribute Name"] = "Enter name Of attribute";
                        row["Attribute Value"] = "Enter attribute value";
                        this.attributesTable.Rows.Add(row);
                        this.AccountAttributesGrid.SetDataBinding(this.accountAttributeSet, this.accountAttributeSet.Tables[0].TableName);
                    }
                    else
                    {
                        this.accountAttributeSet.ReadXml(new XmlTextReader(new StringReader(this.attributeSchemaXml)));
                        this.AccountAttributesGrid.SetDataBinding(this.accountAttributeSet, this.accountAttributeSet.Tables[0].TableName);
                    }
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    BusinessLogic.MyMessageBox(exception.Message);
                }
            }
        }
    }
}

