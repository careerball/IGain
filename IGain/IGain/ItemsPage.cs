namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class ItemsPage : Form
    {
        private Container components = null;
        public static OleDbConnection Con;
        private Button CreateNewItem;
        private MenuItem DeleteRow;
        private MenuItem EditRow;
        private ContextMenu GridRowContextMenu;
        public PGUserControl ItemsReportBox;

        public ItemsPage()
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.ItemsReportBox.DBCon = Con;
            }
        }

        private void CreateNewItem_Click(object sender, EventArgs e)
        {
            NewItemForm.CurCon = Con;
            new NewItemForm().ShowDialog(this);
        }

        private void DeleteRow_Click(object sender, EventArgs e)
        {
            string str = null;
            string str2 = null;
            string str3 = null;
            string str4 = null;
            string queryToValidate = null;
            Exception exception;
            try
            {
                str = (string) this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 0];
            }
            catch (Exception exception1)
            {
                exception = exception1;
                BusinessLogic.MyMessageBox(exception.Message);
                return;
            }
            try
            {
                str2 = (string) this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 3];
            }
            catch (Exception exception3)
            {
                exception = exception3;
                BusinessLogic.MyMessageBox(exception.Message);
                return;
            }
            try
            {
                str3 = (string) this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 1];
            }
            catch (Exception exception4)
            {
                exception = exception4;
                BusinessLogic.MyMessageBox(exception.Message);
                return;
            }
            str4 = " WHERE ItemCategoryName='" + str + "' and ItemSubCategoryName='" + str3 + "' ";
            queryToValidate = " SELECT Count(*) from Stock where ItemCategoryID=(Select ItemCategoryID from ItemCategories " + str4 + ")";
            string queryToRun = "Delete from ItemCategories" + str4;
            if (BusinessLogic.MyMessageBox("Are you sure you want to remove this item?\nIts stock entries will also be removed if it is an inventory item", "Remove?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                try
                {
                    string str7 = "DELETE FROM STOCK WHERE ITEMCATEGORYID='" + str2 + "'";
                    BusinessLogic.ModifyStoreHouse(Con, str7, null);
                    Exception exception2 = BusinessLogic.ModifyStoreHouse(Con, queryToRun, queryToValidate);
                    if (exception2 != null)
                    {
                        BusinessLogic.MyMessageBox(exception2.Message);
                    }
                    else
                    {
                        BusinessLogic.MyMessageBox("Item Removed");
                    }
                }
                catch (Exception exception5)
                {
                    exception = exception5;
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
            bool flag;
            Exception exception;
            try
            {
                str = (this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 0] == DBNull.Value) ? "" : ((string) this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 0]);
            }
            catch (Exception exception1)
            {
                exception = exception1;
                str = "";
            }
            try
            {
                str2 = (this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 1] == DBNull.Value) ? "" : ((string) this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 1]);
            }
            catch (Exception exception2)
            {
                exception = exception2;
                str2 = "";
            }
            try
            {
                str3 = (this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 3] == DBNull.Value) ? "" : ((string) this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 3]);
            }
            catch (Exception exception3)
            {
                exception = exception3;
                str3 = "";
            }
            try
            {
                flag = Convert.ToBoolean(this.ItemsReportBox.ReportGrid[this.ItemsReportBox.ReportGrid.CurrentRowIndex, 2]);
            }
            catch (Exception exception4)
            {
                exception = exception4;
                flag = true;
            }
            bool flag2 = false;
            try
            {
                flag2 = Convert.ToUInt32(new OleDbCommand("Select count(*) from stock where itemcategoryid='" + str3 + "'", Con).ExecuteScalar()) != 0;
            }
            catch (Exception exception5)
            {
                exception = exception5;
                flag2 = false;
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Stock Information for this item could not be retrieved!");
            }
            NewItemForm.CurCon = Con;
            NewItemForm.ReplacementQueryPart = " WHERE ItemCategoryName='" + str + "' and ItemSubCategoryName='" + str2 + "' ";
            NewItemForm.ValidationQuery = null;
            NewItemForm form = new NewItemForm();
            form.releaseToStock.Checked = flag2;
            if (flag2)
            {
                form.releaseToStock.Enabled = false;
            }
            form.itemCategoryID = str3;
            form.ItemCategory.TextBoxText = str;
            form.ItemSubCategory.TextBoxText = str2;
            form.IsInventoryItem.Checked = flag;
            form.Text = "Update Item";
            form.CreateNewItem.Text = "Update";
            form.ShowDialog(this);
        }

        private void InitializeComponent()
        {
            this.ItemsReportBox = new PGUserControl();
            this.CreateNewItem = new Button();
            this.GridRowContextMenu = new ContextMenu();
            this.EditRow = new MenuItem();
            this.DeleteRow = new MenuItem();
            base.SuspendLayout();
            this.ItemsReportBox.ConfigFile = "";
            this.ItemsReportBox.DBCon = null;
            this.ItemsReportBox.Location = new Point(8, 8);
            this.ItemsReportBox.Name = "ItemsReportBox";
            this.ItemsReportBox.Size = new Size(0x2b0, 0x278);
            this.ItemsReportBox.TabIndex = 0;
            this.ItemsReportBox.TotallingEnabled = false;
            this.CreateNewItem.Location = new Point(480, 0x260);
            this.CreateNewItem.Name = "CreateNewItem";
            this.CreateNewItem.Size = new Size(0xc0, 0x17);
            this.CreateNewItem.TabIndex = 1;
            this.CreateNewItem.Text = "Create New Item...";
            this.CreateNewItem.Click += new EventHandler(this.CreateNewItem_Click);
            this.GridRowContextMenu.MenuItems.AddRange(new MenuItem[] { this.EditRow, this.DeleteRow });
            this.EditRow.Index = 0;
            this.EditRow.Text = "Edit..";
            this.EditRow.Click += new EventHandler(this.EditRow_Click);
            this.DeleteRow.Index = 1;
            this.DeleteRow.Text = "Delete";
            this.DeleteRow.Click += new EventHandler(this.DeleteRow_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b8, 0x27d);
            base.Controls.AddRange(new Control[] { this.CreateNewItem, this.ItemsReportBox });
            base.MaximizeBox = false;
            base.Name = "ItemsPage";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manage Items";
            base.Load += new EventHandler(this.ItemsPage_Load);
            base.ResumeLayout(false);
        }

        private void ItemsPage_Load(object sender, EventArgs e)
        {
            if (this.ItemsReportBox.ReportGrid != null)
            {
                this.ItemsReportBox.ReportGrid.MouseDown += new MouseEventHandler(this.ItemsReportBox_ReportGrid_MouseDown);
            }
        }

        private void ItemsReportBox_ReportGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                DataGrid.HitTestInfo info = this.ItemsReportBox.ReportGrid.HitTest(e.X, e.Y);
                if (info.Type == DataGrid.HitTestType.RowHeader)
                {
                    this.ItemsReportBox.ReportGrid.CurrentRowIndex = info.Row;
                    this.GridRowContextMenu.Show(this.ItemsReportBox.ReportGrid, new Point(e.X, e.Y));
                }
            }
        }
    }
}

