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

    public class NewItemForm : Form
    {
        private string attributeSchemaXml;
        private Container components = null;
        public Button CreateNewItem;
        public static OleDbConnection CurCon;
        public CheckBox IsInventoryItem;
        private DataSet itemAttributeSet;
        private DataGrid ItemAttributesGrid;
        public PGCookie.PGCookie ItemCategory;
        public string itemCategoryID;
        private DataTable itemsTable;
        public PGCookie.PGCookie ItemSubCategory;
        private Label label1;
        private Label label2;
        public CheckBox releaseToStock;
        public static string ReplacementQueryPart = null;
        private string strItemCategory;
        private string strItemSubCategory;
        public static string ValidationQuery = null;

        public NewItemForm()
        {
            this.InitializeComponent();
        }

        private void CreateNewItem_Click(object sender, EventArgs e)
        {
            if (CurCon == null)
            {
                BusinessLogic.MyMessageBox("Connection is invalid");
            }
            else if (this.ItemCategory.TextBoxText.Length == 0)
            {
                BusinessLogic.MyMessageBox("Item Category can not be empty");
                this.ItemCategory.Focus();
            }
            else if (this.ItemSubCategory.TextBoxText.Length == 0)
            {
                BusinessLogic.MyMessageBox("Item SubCategory can not be empty");
                this.ItemSubCategory.Focus();
            }
            else
            {
                Exception exception2;
                if (this.CreateNewItem.Text == "Update")
                {
                    string str = "Update ItemCategories Set ItemCategoryName='" + this.ItemCategory.TextBoxText + "', ItemSubCategoryName='" + this.ItemSubCategory.TextBoxText + "', IsInventory='" + (this.IsInventoryItem.Checked ? "1" : "0") + "',ItemAttributes='" + this.itemAttributeSet.GetXml() + "'";
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
                        str = str + ReplacementQueryPart;
                        try
                        {
                            string[] strArray;
                            if (!(!this.releaseToStock.Checked || this.IsInventoryItem.Checked))
                            {
                                throw new Exception("Stock entries can not be created for non-inventory items!");
                            }
                            if ((this.releaseToStock.Checked && this.IsInventoryItem.Checked) && this.releaseToStock.Enabled)
                            {
                                strArray = new string[2];
                                strArray[1] = "INSERT INTO STOCK(ITEMCATEGORYID,STOCKCOUNTER,RUNNINGRATE,CHESTCOUNT,FREEQUANTITY) VALUES('" + this.ItemCategory.TextBoxText + "-" + this.ItemSubCategory.TextBoxText + "',0.0,0.0,0,0.0)";
                            }
                            else if (!((this.releaseToStock.Checked || this.IsInventoryItem.Checked) || this.releaseToStock.Enabled))
                            {
                                strArray = new string[2];
                                strArray[1] = "DELETE FROM STOCK WHERE ITEMCATEGORYID='" + this.itemCategoryID + "'";
                            }
                            else
                            {
                                strArray = new string[1];
                            }
                            strArray[0] = str;
                            Exception exception = BusinessLogic.PerformMultipleQueriesWithoutValidation(CurCon, strArray);
                            if (exception != null)
                            {
                                BusinessLogic.MyMessageBox(exception.Message);
                            }
                            else
                            {
                                BusinessLogic.MyMessageBox("Item Updated");
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
                    OleDbCommand command2 = new OleDbCommand();
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
                        command.CommandText = "Insert into ItemCategories(ItemCategoryID,ItemCategoryName,ItemSubCategoryName,IsInventory,ItemAttributes) values('" + this.ItemCategory.TextBoxText + "-" + this.ItemSubCategory.TextBoxText + "','" + this.ItemCategory.TextBoxText + "','" + this.ItemSubCategory.TextBoxText + "','" + (this.IsInventoryItem.Checked ? "1" : "0") + "','" + this.itemAttributeSet.GetXml() + "')";
                        command.ExecuteNonQuery();
                        if (this.releaseToStock.Checked && this.IsInventoryItem.Checked)
                        {
                            command2.CommandText = "INSERT INTO STOCK(ITEMCATEGORYID,STOCKCOUNTER,RUNNINGRATE,CHESTCOUNT,FREEQUANTITY) VALUES('" + this.ItemCategory.TextBoxText + "-" + this.ItemSubCategory.TextBoxText + "',0.0,0.0,0,0.0)";
                            command2.ExecuteNonQuery();
                        }
                        else if (!(!this.releaseToStock.Checked || this.IsInventoryItem.Checked))
                        {
                            throw new Exception("Stock entries can not be created for non-inventory items!");
                        }
                        transaction.Commit();
                        BusinessLogic.MyMessageBox("Item Created Successfully!");
                    }
                    catch (Exception exception4)
                    {
                        exception2 = exception4;
                        transaction.Rollback();
                        BusinessLogic.MyMessageBox(exception2.Message);
                        BusinessLogic.MyMessageBox("Item creation failed");
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
            this.ItemCategory = new PGCookie.PGCookie();
            this.ItemSubCategory = new PGCookie.PGCookie();
            this.IsInventoryItem = new CheckBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.CreateNewItem = new Button();
            this.ItemAttributesGrid = new DataGrid();
            this.releaseToStock = new CheckBox();
            this.ItemAttributesGrid.BeginInit();
            base.SuspendLayout();
            this.ItemCategory.BoundColumn = "ItemCategoryName";
            this.ItemCategory.Location = new Point(40, 0x38);
            this.ItemCategory.Name = "ItemCategory";
            this.ItemCategory.Size = new Size(0xb8, 0xb0);
            this.ItemCategory.TabIndex = 0;
            this.ItemCategory.TextBoxText = "";
            this.ItemSubCategory.BoundColumn = "ItemSubCategoryName";
            this.ItemSubCategory.Location = new Point(0x100, 0x38);
            this.ItemSubCategory.Name = "ItemSubCategory";
            this.ItemSubCategory.Size = new Size(0xb0, 0xb0);
            this.ItemSubCategory.TabIndex = 1;
            this.ItemSubCategory.TextBoxText = "";
            this.IsInventoryItem.Location = new Point(480, 0x40);
            this.IsInventoryItem.Name = "IsInventoryItem";
            this.IsInventoryItem.Size = new Size(120, 0x18);
            this.IsInventoryItem.TabIndex = 2;
            this.IsInventoryItem.Text = "Inventory Item ?";
            this.IsInventoryItem.CheckedChanged += new EventHandler(this.IsInventoryItem_CheckedChanged);
            this.label1.Location = new Point(0x30, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(160, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "Item Category :-";
            this.label2.Location = new Point(0x100, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(160, 0x18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Item Subcategory :-";
            this.CreateNewItem.Location = new Point(440, 440);
            this.CreateNewItem.Name = "CreateNewItem";
            this.CreateNewItem.Size = new Size(160, 0x18);
            this.CreateNewItem.TabIndex = 5;
            this.CreateNewItem.Text = "Create Item";
            this.CreateNewItem.Click += new EventHandler(this.CreateNewItem_Click);
            this.ItemAttributesGrid.CaptionText = "Item Attributes";
            this.ItemAttributesGrid.DataMember = "";
            this.ItemAttributesGrid.HeaderForeColor = SystemColors.ControlText;
            this.ItemAttributesGrid.Location = new Point(0x10, 0x100);
            this.ItemAttributesGrid.Name = "ItemAttributesGrid";
            this.ItemAttributesGrid.PreferredColumnWidth = 200;
            this.ItemAttributesGrid.RowHeadersVisible = false;
            this.ItemAttributesGrid.RowHeaderWidth = 200;
            this.ItemAttributesGrid.Size = new Size(0x238, 0xb0);
            this.ItemAttributesGrid.TabIndex = 9;
            this.releaseToStock.Location = new Point(480, 0x80);
            this.releaseToStock.Name = "releaseToStock";
            this.releaseToStock.Size = new Size(0x70, 0x18);
            this.releaseToStock.TabIndex = 10;
            this.releaseToStock.Text = "Release to stock";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x260, 0x1e5);
            base.Controls.Add(this.releaseToStock);
            base.Controls.Add(this.ItemAttributesGrid);
            base.Controls.Add(this.CreateNewItem);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.IsInventoryItem);
            base.Controls.Add(this.ItemSubCategory);
            base.Controls.Add(this.ItemCategory);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "NewItemForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "New Item Form";
            base.Load += new EventHandler(this.NewItemForm_Load);
            this.ItemAttributesGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void IsInventoryItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.IsInventoryItem.Checked)
            {
                this.releaseToStock.Checked = false;
            }
            else if (!this.releaseToStock.Enabled)
            {
                this.releaseToStock.Checked = true;
            }
        }

        private void NewItemForm_Load(object sender, EventArgs e)
        {
            if (CurCon != null)
            {
                Exception exception;
                this.strItemSubCategory = this.ItemSubCategory.TextBoxText;
                this.strItemCategory = this.ItemCategory.TextBoxText;
                DataTable dataTable = new DataTable();
                DataTable table2 = new DataTable();
                string selectCommandText = "SELECT DISTINCT ItemCategoryName FROM ItemCategories where Len(ItemCategoryName)>0 union \r\n\t\t\t\tSELECT DISTINCT ItemSubCategoryName FROM ItemCategories where Len(ItemSubCategoryName)>0";
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommandText, CurCon);
                try
                {
                    adapter.Fill(dataTable);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    BusinessLogic.MyMessageBox(exception.Message);
                    return;
                }
                string cmdText = "SELECT DISTINCT ItemSubCategoryName FROM ItemCategories where Len(ItemSubCategoryName)>0 union \r\n\t\t\t\tSELECT DISTINCT ItemCategoryName FROM ItemCategories where Len(ItemCategoryName)>0";
                adapter.SelectCommand = new OleDbCommand(cmdText, CurCon);
                try
                {
                    adapter.Fill(table2);
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    BusinessLogic.MyMessageBox(exception.Message);
                    return;
                }
                adapter.Dispose();
                this.ItemCategory.SourceDataTable = dataTable;
                this.ItemCategory.BoundColumn = "ItemCategoryName";
                this.ItemCategory.CacheList.Visible = false;
                this.ItemSubCategory.SourceDataTable = table2;
                this.ItemSubCategory.BoundColumn = "ItemSubCategoryName";
                this.ItemSubCategory.CacheList.Visible = false;
                try
                {
                    this.attributeSchemaXml = Convert.ToString(new OleDbCommand("Select top 1 ItemAttributes from ItemCategories where ItemCategoryName='" + this.ItemCategory.TextBoxText + "' and ItemSubCategoryName='" + this.ItemSubCategory.TextBoxText + "' ", CurCon).ExecuteScalar());
                    this.itemAttributeSet = new DataSet();
                    if ((this.attributeSchemaXml == null) || (this.attributeSchemaXml.Length < 1))
                    {
                        this.itemsTable = this.itemAttributeSet.Tables.Add("AttributesTable");
                        this.itemsTable.Columns.Add("Attribute Name");
                        this.itemsTable.Columns.Add("Attribute Value");
                        DataRow row = this.itemsTable.NewRow();
                        row["Attribute Name"] = "Enter name Of attribute";
                        row["Attribute Value"] = "Enter attribute value";
                        this.itemsTable.Rows.Add(row);
                        this.ItemAttributesGrid.SetDataBinding(this.itemAttributeSet, this.itemAttributeSet.Tables[0].TableName);
                    }
                    else
                    {
                        this.itemAttributeSet.ReadXml(new XmlTextReader(new StringReader(this.attributeSchemaXml)));
                        this.ItemAttributesGrid.SetDataBinding(this.itemAttributeSet, this.itemAttributeSet.Tables[0].TableName);
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

