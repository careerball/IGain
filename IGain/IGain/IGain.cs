namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;

    public class IGain : Form
    {
        private TabPage AccountsTab;
        private TabPage Attributes;
        private IContainer components;
        private OleDbConnection Con;
        private ImageList IGainImgList;
        private TabPage ItemsTab;
        private TabPage ManageCredit;
        private TabPage ManageStock;
        private ToolBar PetroBar;
        private TabControl PetroTab;
        private ToolBarButton PrintButton;
        public static string printTemplate;
        private TabPage Reports;
        private TabPage Sale;
        private StreamReader streamReader;

        public IGain()
        {
            Exception exception;
            this.InitializeComponent();
            this.Con = new OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=StoreHouse;Data Source=BATHLA;");
            try
            {
                this.Con = new OleDbConnection(@"FILE NAME= .\Connection.udl");
            }
            catch (Exception exception1)
            {
                exception = exception1;
                this.Con = new OleDbConnection(@"FILE NAME= C:\Connection.udl");
            }
            try
            {
                this.Con.Open();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                BusinessLogic.MyMessageBox(exception.Message);
                this.PetroTab.Enabled = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.Con != null)
            {
                try
                {
                    this.Con.Close();
                    Process.GetCurrentProcess().Close();
                }
                catch (Exception)
                {
                }
            }
            base.Dispose(disposing);
        }

        private void IGain_Load(object sender, EventArgs e)
        {
            this.PetroTab.Height = base.Height - 0x48;
            this.PetroTab.SelectedIndex = -1;
            this.PetroBar.ButtonSize = new Size(110, 0x2b);
            this.PetroTab.ItemSize = new Size(110, 0x30);
        }

        private void IGain_Resize(object sender, EventArgs e)
        {
            this.PetroTab.Height = base.Height - 0x48;
        }

        private void Init()
        {
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(IGain.IGain));
            this.IGainImgList = new ImageList(this.components);
            this.PetroTab = new TabControl();
            this.AccountsTab = new TabPage();
            this.ItemsTab = new TabPage();
            this.ManageCredit = new TabPage();
            this.ManageStock = new TabPage();
            this.Sale = new TabPage();
            this.Attributes = new TabPage();
            this.Reports = new TabPage();
            this.PrintButton = new ToolBarButton();
            this.PetroBar = new ToolBar();
            this.PetroTab.SuspendLayout();
            base.SuspendLayout();
            this.IGainImgList.ImageSize = new Size(0x20, 0x20);
            this.IGainImgList.ImageStream = (ImageListStreamer) manager.GetObject("IGainImgList.ImageStream");
            this.IGainImgList.TransparentColor = Color.Transparent;
            this.PetroTab.Appearance = TabAppearance.Buttons;
            this.PetroTab.Controls.Add(this.AccountsTab);
            this.PetroTab.Controls.Add(this.ItemsTab);
            this.PetroTab.Controls.Add(this.ManageCredit);
            this.PetroTab.Controls.Add(this.ManageStock);
            this.PetroTab.Controls.Add(this.Sale);
            this.PetroTab.Controls.Add(this.Attributes);
            this.PetroTab.Controls.Add(this.Reports);
            this.PetroTab.Dock = DockStyle.Left;
            this.PetroTab.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.PetroTab.ImageList = this.IGainImgList;
            this.PetroTab.ItemSize = new Size(110, 0x30);
            this.PetroTab.Location = new Point(0, 0x36);
            this.PetroTab.Multiline = true;
            this.PetroTab.Name = "PetroTab";
            this.PetroTab.SelectedIndex = 0;
            this.PetroTab.ShowToolTips = true;
            this.PetroTab.Size = new Size(0x70, 0x1a7);
            this.PetroTab.SizeMode = TabSizeMode.Fixed;
            this.PetroTab.TabIndex = 3;
            this.PetroTab.SelectedIndexChanged += new EventHandler(this.PetroTab_SelectedIndexChanged);
            this.AccountsTab.ImageIndex = 6;
            this.AccountsTab.Location = new Point(4, 0x166);
            this.AccountsTab.Name = "AccountsTab";
            this.AccountsTab.Size = new Size(0x68, 0x3d);
            this.AccountsTab.TabIndex = 0;
            this.AccountsTab.Text = "Accounts";
            this.AccountsTab.ToolTipText = "Manage Accounts";
            this.ItemsTab.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ItemsTab.ImageIndex = 7;
            this.ItemsTab.Location = new Point(4, 0x166);
            this.ItemsTab.Name = "ItemsTab";
            this.ItemsTab.Size = new Size(0x68, 0x3d);
            this.ItemsTab.TabIndex = 1;
            this.ItemsTab.Text = "Items     ";
            this.ItemsTab.ToolTipText = "Manage Items";
            this.ManageCredit.ImageIndex = 3;
            this.ManageCredit.Location = new Point(4, 0x166);
            this.ManageCredit.Name = "ManageCredit";
            this.ManageCredit.Size = new Size(0x68, 0x3d);
            this.ManageCredit.TabIndex = 2;
            this.ManageCredit.Text = "Credit    ";
            this.ManageCredit.ToolTipText = "Manage your credit transactions";
            this.ManageStock.ImageIndex = 5;
            this.ManageStock.Location = new Point(4, 0x166);
            this.ManageStock.Name = "ManageStock";
            this.ManageStock.Size = new Size(0x68, 0x3d);
            this.ManageStock.TabIndex = 3;
            this.ManageStock.Text = "Stock     ";
            this.ManageStock.ToolTipText = "Manage Stock";
            this.Sale.ImageIndex = 0;
            this.Sale.Location = new Point(4, 0x166);
            this.Sale.Name = "Sale";
            this.Sale.Size = new Size(0x68, 0x3d);
            this.Sale.TabIndex = 4;
            this.Sale.Text = "Sale      ";
            this.Sale.ToolTipText = "Manage Sale of items";
            this.Attributes.ImageIndex = 2;
            this.Attributes.Location = new Point(4, 0x166);
            this.Attributes.Name = "Attributes";
            this.Attributes.Size = new Size(0x68, 0x3d);
            this.Attributes.TabIndex = 5;
            this.Attributes.Text = "Attributes";
            this.Attributes.ToolTipText = "Assign attributes to business entities";
            this.Reports.ImageIndex = 4;
            this.Reports.Location = new Point(4, 0x166);
            this.Reports.Name = "Reports";
            this.Reports.Size = new Size(0x68, 0x3d);
            this.Reports.TabIndex = 6;
            this.Reports.Text = "Reports";
            this.Reports.ToolTipText = "View reports and information";
            this.PrintButton.ImageIndex = 1;
            this.PrintButton.Text = " Print";
            this.PrintButton.ToolTipText = "Print Document";
            this.PetroBar.Buttons.AddRange(new ToolBarButton[] { this.PrintButton });
            this.PetroBar.ButtonSize = new Size(110, 0x30);
            this.PetroBar.DropDownArrows = true;
            this.PetroBar.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.PetroBar.ImageList = this.IGainImgList;
            this.PetroBar.Location = new Point(0, 0);
            this.PetroBar.Name = "PetroBar";
            this.PetroBar.ShowToolTips = true;
            this.PetroBar.Size = new Size(760, 0x36);
            this.PetroBar.TabIndex = 1;
            this.PetroBar.TextAlign = ToolBarTextAlign.Right;
            this.PetroBar.ButtonClick += new ToolBarButtonClickEventHandler(this.PetroBar_ButtonClick);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(760, 0x1dd);
            base.Controls.Add(this.PetroTab);
            base.Controls.Add(this.PetroBar);
            this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.IsMdiContainer = true;
            base.Name = "IGain";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "IGain";
            base.WindowState = FormWindowState.Maximized;
            base.Resize += new EventHandler(this.IGain_Resize);
            base.Load += new EventHandler(this.IGain_Load);
            this.PetroTab.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LogIGainInfo(string message, EventLogEntryType eventType)
        {
            EventLog log = new EventLog();
            try
            {
                Exception exception;
                try
                {
                    EventLog.DeleteEventSource("IGain Log");
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    exception.Source = null;
                }
                try
                {
                    if (EventLog.Exists("IGain Log"))
                    {
                        EventLog.Delete("IGain Log");
                    }
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    exception.Source = null;
                }
                EventLog.CreateEventSource("IGain Log", "Application");
                log.Source = "IGain Log";
                if ((log != null) && (log.Source == "IGain Log"))
                {
                    log.WriteEntry(message, eventType);
                }
            }
            catch (Exception exception2)
            {
                exception2.Source = null;
            }
        }

        [MTAThread]
        private static void Main()
        {
            Application.Run(new IGain.IGain());
        }

        private void PetroBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button.Text.ToLower().IndexOf("print") > 0)
            {
                foreach (Form form in base.MdiChildren)
                {
                    if (base.ActiveMdiChild == form)
                    {
                        foreach (Control control in form.Controls)
                        {
                            if (control.GetType().Name.ToLower() == "PGUserControl".ToLower())
                            {
                                try
                                {
                                    PGUserControl control2 = (PGUserControl) control;
                                    if (control2 != null)
                                    {
                                        DataSet dataSource = (DataSet) control2.ReportGrid.DataSource;
                                        if (dataSource != null)
                                        {
                                            PrintColumnSelection selection = new PrintColumnSelection(dataSource);
                                            foreach (DataColumn column in dataSource.Tables[control2.ReportGrid.DataMember].Columns)
                                            {
                                                selection.AllColumnsList.Items.Add(column.ColumnName);
                                            }
                                            selection.ShowDialog(form);
                                        }
                                        break;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    exception.Source = null;
                                    BusinessLogic.MyMessageBox("Invalid data source for printing.\n Can not print!");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PetroTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            GC.Collect();
            switch (this.PetroTab.SelectedIndex)
            {
                case 0:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "AccountsPage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    AccountsPage.Con = this.Con;
                    AccountsPage page = new AccountsPage();
                    page.MdiParent = this;
                    page.Show();
                    break;
                }
                case 1:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "ItemsPage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    ItemsPage.Con = this.Con;
                    ItemsPage page2 = new ItemsPage();
                    page2.MdiParent = this;
                    page2.Show();
                    break;
                }
                case 2:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "InterestPage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    InterestPage.Con = this.Con;
                    InterestPage page3 = new InterestPage();
                    page3.MdiParent = this;
                    page3.Show();
                    break;
                }
                case 3:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "StockPage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    StockPage.Con = this.Con;
                    StockPage page4 = new StockPage();
                    page4.MdiParent = this;
                    page4.Show();
                    break;
                }
                case 4:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "SalePage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    SalePage.Con = this.Con;
                    SalePage page5 = new SalePage();
                    page5.MdiParent = this;
                    page5.Show();
                    break;
                }
                case 5:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "AttributesPage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    AttributesPage.Con = this.Con;
                    AttributesPage page6 = new AttributesPage();
                    page6.MdiParent = this;
                    page6.Show();
                    break;
                }
                case 6:
                {
                    foreach (Form form in base.MdiChildren)
                    {
                        if (form.GetType().Name == "ReportsCollectionPage")
                        {
                            form.WindowState = FormWindowState.Normal;
                            form.Focus();
                            this.PetroTab.SelectedIndex = -1;
                            return;
                        }
                    }
                    ReportsCollectionPage.Con = this.Con;
                    ReportsCollectionPage page7 = new ReportsCollectionPage();
                    page7.MdiParent = this;
                    page7.Show();
                    break;
                }
            }
            this.PetroTab.SelectedIndex = -1;
        }
    }
}

