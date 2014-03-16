namespace IGain
{
    using Microsoft.VisualBasic;
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class StockPage : Form
    {
        private Button CalcStockInfo;
        private DataGridCell clLastCell;
        private Container components = null;
        public static OleDbConnection Con;
        private DataTable dtPackings;
        private Button Pack;
        private DataGrid PackingsGrid;
        public PGUserControl StockReportBox;
        private Button updateStock;

        public StockPage()
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.StockReportBox.DBCon = Con;
            }
            this.StockReportBox.ReportGrid.CurrentCellChanged += new EventHandler(this.ReportGrid_CurrentCellChanged);
            this.StockReportBox.ReportGrid.DoubleClick += new EventHandler(this.ReportGrid_DoubleClick);
        }

        private void CalcStockInfo_Click(object sender, EventArgs e)
        {
            int currentRowIndex = this.StockReportBox.ReportGrid.CurrentRowIndex;
            try
            {
                this.StockReportBox.ReportGrid[currentRowIndex, 2] = Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 2]) + Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 8]);
                this.StockReportBox.ReportGrid[currentRowIndex, 4] = Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 4]) + Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 10]);
                this.StockReportBox.ReportGrid[currentRowIndex, 5] = Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 5]) + Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 11]);
                this.StockReportBox.ReportGrid[currentRowIndex, 9] = Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 9]) + (Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 3]) * Convert.ToDouble(this.StockReportBox.ReportGrid[currentRowIndex, 8]));
                this.updateStock.Enabled = true;
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("One of the values entered is not compatible with its data type.\n Please recheck the values and re-enter them.");
                for (int i = 8; i < 12; i++)
                {
                    this.StockReportBox.ReportGrid[currentRowIndex, i] = "0";
                }
            }
            this.CalcStockInfo.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FormPackingsDataTable()
        {
            this.dtPackings = new DataTable("PackingsTable");
            this.dtPackings.Columns.Add("Item ID");
            this.dtPackings.Columns.Add("Item Category");
            this.dtPackings.Columns.Add("Item Subcategory");
            this.dtPackings.Columns.Add("Chests packed", System.Type.GetType("System.Int32"));
            this.dtPackings.Columns.Add("Free Qty packed", System.Type.GetType("System.Decimal"));
            this.dtPackings.Columns.Add("Total Qty packed", System.Type.GetType("System.Decimal"));
            this.dtPackings.Columns.Add("Packing Date", System.Type.GetType("System.DateTime"));
            this.dtPackings.Columns.Add("Packing Slip");
            this.dtPackings.AcceptChanges();
        }

        private void InitializeComponent()
        {
            this.StockReportBox = new PGUserControl();
            this.updateStock = new Button();
            this.CalcStockInfo = new Button();
            this.PackingsGrid = new DataGrid();
            this.Pack = new Button();
            this.PackingsGrid.BeginInit();
            base.SuspendLayout();
            this.StockReportBox.ConfigFile = "";
            this.StockReportBox.DBCon = null;
            this.StockReportBox.Name = "StockReportBox";
            this.StockReportBox.Size = new Size(690, 390);
            this.StockReportBox.TabIndex = 0;
            this.StockReportBox.TotallingEnabled = false;
            this.StockReportBox.Search_Clicked += new Search_ClickedHandler(this.StockReportBox_Search_Clicked);
            this.StockReportBox.Total_Clicked += new Total_ClickedHandler(this.StockReportBox_Total_Clicked);
            this.updateStock.Enabled = false;
            this.updateStock.Location = new Point(0x240, 0x1a0);
            this.updateStock.Name = "updateStock";
            this.updateStock.TabIndex = 1;
            this.updateStock.Text = "Update";
            this.updateStock.Click += new EventHandler(this.updateStock_Click);
            this.CalcStockInfo.Enabled = false;
            this.CalcStockInfo.Location = new Point(0x1c0, 0x1a0);
            this.CalcStockInfo.Name = "CalcStockInfo";
            this.CalcStockInfo.TabIndex = 2;
            this.CalcStockInfo.Text = "Calculate";
            this.CalcStockInfo.Click += new EventHandler(this.CalcStockInfo_Click);
            this.PackingsGrid.AllowSorting = false;
            this.PackingsGrid.CaptionText = "Create Packings";
            this.PackingsGrid.DataMember = "";
            this.PackingsGrid.HeaderForeColor = SystemColors.ControlText;
            this.PackingsGrid.Location = new Point(0x20, 0x1c0);
            this.PackingsGrid.Name = "PackingsGrid";
            this.PackingsGrid.PreferredColumnWidth = 120;
            this.PackingsGrid.ReadOnly = true;
            this.PackingsGrid.Size = new Size(0x270, 0x98);
            this.PackingsGrid.TabIndex = 3;
            this.PackingsGrid.DoubleClick += new EventHandler(this.PackingsGrid_DoubleClick);
            this.PackingsGrid.CurrentCellChanged += new EventHandler(this.PackingsGrid_CurrentCellChanged);
            this.Pack.Enabled = false;
            this.Pack.Location = new Point(0x240, 0x260);
            this.Pack.Name = "Pack";
            this.Pack.TabIndex = 4;
            this.Pack.Text = "Pack";
            this.Pack.Click += new EventHandler(this.Pack_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b8, 0x285);
            base.Controls.AddRange(new Control[] { this.Pack, this.PackingsGrid, this.CalcStockInfo, this.updateStock, this.StockReportBox });
            base.MaximizeBox = false;
            base.Name = "StockPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manage Stock";
            this.PackingsGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void Pack_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidatePackingsDataAndPack();
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                return;
            }
            this.clLastCell.ColumnNumber = 0;
            this.dtPackings.Clear();
            this.Pack.Enabled = false;
            BusinessLogic.MyMessageBox("Packed!");
            this.StockReportBox.ExecuteQueryAndFillGrid();
        }

        private void PackingsGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.PackingsGrid.CurrentCell.ColumnNumber < 3)
            {
                this.PackingsGrid.ReadOnly = true;
            }
            else
            {
                this.PackingsGrid.ReadOnly = false;
            }
            try
            {
                if ((this.clLastCell.ColumnNumber == 6) || (this.clLastCell.ColumnNumber == 7))
                {
                    foreach (DataRow row in this.dtPackings.Rows)
                    {
                        row[this.clLastCell.ColumnNumber] = this.PackingsGrid[this.clLastCell];
                    }
                }
                this.clLastCell = this.PackingsGrid.CurrentCell;
                this.dtPackings.AcceptChanges();
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Please Enter valid data!");
            }
        }

        private void PackingsGrid_DoubleClick(object sender, EventArgs e)
        {
            if (this.PackingsGrid.Enabled && this.Pack.Enabled)
            {
                Point position = this.PackingsGrid.PointToClient(Cursor.Position);
                if (this.PackingsGrid.HitTest(position).Type == DataGrid.HitTestType.RowHeader)
                {
                    int currentRowIndex = this.PackingsGrid.CurrentRowIndex;
                    try
                    {
                        this.dtPackings.Rows.RemoveAt(currentRowIndex);
                    }
                    catch (Exception exception)
                    {
                        BusinessLogic.MyMessageBox(exception.Message);
                        return;
                    }
                    if (this.dtPackings.Rows.Count == 0)
                    {
                        this.Pack.Enabled = false;
                    }
                    this.dtPackings.AcceptChanges();
                }
            }
        }

        private void ReportGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (((((this.StockReportBox.ReportGrid.CurrentCell.ColumnNumber == 0) || (this.StockReportBox.ReportGrid.CurrentCell.ColumnNumber == 1)) || ((this.StockReportBox.ReportGrid.CurrentCell.ColumnNumber == 2) || (this.StockReportBox.ReportGrid.CurrentCell.ColumnNumber == 4))) || (this.StockReportBox.ReportGrid.CurrentCell.ColumnNumber == 5)) || (this.StockReportBox.ReportGrid.CurrentCell.ColumnNumber == 6))
            {
                this.StockReportBox.ReportGrid.ReadOnly = true;
            }
            else
            {
                this.StockReportBox.ReportGrid.ReadOnly = false;
            }
        }

        private void ReportGrid_DoubleClick(object sender, EventArgs e)
        {
            if (this.PackingsGrid.Enabled)
            {
                Point position = this.StockReportBox.ReportGrid.PointToClient(Cursor.Position);
                if (this.StockReportBox.ReportGrid.HitTest(position).Type == DataGrid.HitTestType.RowHeader)
                {
                    try
                    {
                        int currentRowIndex = this.StockReportBox.ReportGrid.CurrentRowIndex;
                        if (this.dtPackings == null)
                        {
                            this.FormPackingsDataTable();
                        }
                        if (this.dtPackings == null)
                        {
                            throw new Exception("Error Packing Items.");
                        }
                        DataRow row = this.dtPackings.NewRow();
                        row["Item ID"] = this.StockReportBox.ReportGrid[currentRowIndex, 6];
                        row["Item Category"] = this.StockReportBox.ReportGrid[currentRowIndex, 0];
                        row["Item Subcategory"] = this.StockReportBox.ReportGrid[currentRowIndex, 1];
                        row["Chests packed"] = "0";
                        row["Free Qty packed"] = "0.0";
                        row["Total Qty packed"] = "0.0";
                        row["Packing Date"] = DateTime.Now.ToShortDateString();
                        row["Packing Slip"] = "Enter new packing slip";
                        this.dtPackings.Rows.Add(row);
                        if (this.PackingsGrid.DataSource != this.dtPackings)
                        {
                            this.PackingsGrid.DataSource = this.dtPackings;
                        }
                        this.dtPackings.AcceptChanges();
                        CurrencyManager manager = (CurrencyManager) this.PackingsGrid.BindingContext[this.PackingsGrid.DataSource, this.PackingsGrid.DataMember];
                        DataView list = (DataView) manager.List;
                        list.AllowNew = false;
                        this.PackingsGrid.Refresh();
                        this.Pack.Enabled = true;
                    }
                    catch (Exception exception)
                    {
                        this.PackingsGrid.Enabled = false;
                        this.Pack.Enabled = false;
                        this.StockReportBox.ReportGrid.DoubleClick -= new EventHandler(this.ReportGrid_DoubleClick);
                        BusinessLogic.MyMessageBox(exception.Message);
                        BusinessLogic.MyMessageBox("Error Packing Items.");
                    }
                }
            }
        }

        private void StockReportBox_Search_Clicked(long RowsReturned, string MasterQuery)
        {
            try
            {
                CurrencyManager manager = (CurrencyManager) this.StockReportBox.ReportGrid.BindingContext[this.StockReportBox.ReportGrid.DataSource, this.StockReportBox.ReportGrid.DataMember];
                DataView list = (DataView) manager.List;
                list.AllowNew = false;
                if (RowsReturned < 1L)
                {
                    this.CalcStockInfo.Enabled = false;
                }
                else
                {
                    this.CalcStockInfo.Enabled = true;
                }
                this.updateStock.Enabled = false;
            }
            catch (Exception exception)
            {
                this.StockReportBox.ReportGrid.Enabled = false;
                BusinessLogic.MyMessageBox(exception.Message);
            }
        }

        private void StockReportBox_Total_Clicked(int ColumnIndex, double Total)
        {
            BusinessLogic.MyMessageBox(Convert.ToString(Total));
        }

        private void updateStock_Click(object sender, EventArgs e)
        {
            Exception exception;
            int num2;
            Exception exception2;
            int currentRowIndex = this.StockReportBox.ReportGrid.CurrentRowIndex;
            try
            {
                this.StockReportBox.ReportGrid[currentRowIndex, 7] = Convert.ToDateTime(this.StockReportBox.ReportGrid[currentRowIndex, 7]).ToLongDateString();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                BusinessLogic.MyMessageBox("The date format is invalid in column [Effective Date], row " + Convert.ToString(currentRowIndex) + ".Please correct it to proceed");
                this.updateStock.Enabled = false;
                for (num2 = 8; num2 < 12; num2++)
                {
                    this.StockReportBox.ReportGrid[currentRowIndex, num2] = "0";
                }
                this.CalcStockInfo.Enabled = true;
                return;
            }
            try
            {
                string str = Convert.ToDateTime(this.StockReportBox.ReportGrid[currentRowIndex, 7]).ToLongDateString();
                str = str.Substring(str.IndexOf(",") + 1);
                string str2 = Guid.NewGuid().ToString();
                long nextTransactionID = BusinessLogic.GetNextTransactionID(Con);
                if (nextTransactionID < 0L)
                {
                    throw new Exception("Error! TransactionID can not be less than 1");
                }
                object obj2 = new OleDbCommand("SELECT ACCOUNTID FROM ACCOUNTTYPES WHERE ACCOUNTTYPE='PURCHASE' AND ITEMCATEGORYID='" + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 6]) + "'", Con).ExecuteScalar();
                if (obj2 == DBNull.Value)
                {
                    throw new Exception("Error! AccountID can not be null");
                }
                string str3 = Convert.ToString(obj2);
                string[] queriesToRun = new string[2];
                string str4 = Interaction.InputBox("Enter a qualifier for this update", "Update Qualifier!", "Update12 as on " + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString(), -1, -1);
                if ((str4 == null) || (str4.Length < 1))
                {
                    throw new Exception("You must enter a qualifier for the update.\nThis helps you to track the update later on and modify it's attributes.");
                }
                AppSettingsReader reader = new AppSettingsReader();
                string str5 = Convert.ToString(reader.GetValue("DateDelimiter", typeof(string)));
                if ((str5 == null) || (str5.Length < 1))
                {
                    throw new Exception("Date Delimiter is not defined");
                }
                object obj3 = new OleDbCommand("SELECT DISTINCT SLIPNUMBER FROM TRANSACTIONS WHERE STOCKAFFECTED <> 0 AND SLIPNUMBER='" + str4 + "' AND DATEOFTRANSACTION =" + str5 + str + str5 + " AND NOT  (SLIPNUMBER IS  NULL OR  LEN(SLIPNUMBER)=0)", Con).ExecuteScalar();
                if ((obj3 != DBNull.Value) && (Convert.ToString(obj3).Length >= 1))
                {
                    throw new Exception("The Qualifier already exists for this date.\nPlease try again and enter a different Qualifier");
                }
                queriesToRun[0] = string.Concat(new object[] { "UPDATE STOCK SET STOCKCOUNTER='", Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 2]), "', RUNNINGRATE='", Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 3]), "',CHESTCOUNT='", Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 4]), "', FREEQUANTITY='", Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 5]), "',DATEOFTRANSACTION='", str, "',TRANSACTIONID='", nextTransactionID, "' WHERE ITEMCATEGORYID='", Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 6]), "'" });
                queriesToRun[1] = "INSERT INTO TRANSACTIONS(TRANSACTIONID,ACCOUNTDEBITED,ACCOUNTCREDITED,DEBIT,CREDIT,STOCKAFFECTED,ITEMCATEGORYID,STOCKCOUNTER,RUNNINGRATE,DATEOFTRANSACTION,SLIPISSUED,SLIPNUMBER,TRANSACTIONSET,ISCREDITABLE,CREDITID,SEQUENCEOFINTERESTAPPLICATION,CHESTSAPPENDED,FREEQUANTITYAPPENDED) VALUES (" + Convert.ToString(nextTransactionID) + ",'" + str3 + "','Cash-Cash'," + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 9]) + "," + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 9]) + ",-1,'" + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 6]) + "'," + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 8]) + "," + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 3]) + ",'" + str + "',-1,'" + str4 + "','" + str2 + "',0,null,0," + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 10]) + "," + Convert.ToString(this.StockReportBox.ReportGrid[currentRowIndex, 11]) + ")";
                exception2 = BusinessLogic.PerformMultipleQueriesWithoutValidation(Con, queriesToRun);
                if (exception2 != null)
                {
                    throw exception2;
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Update failed!");
                this.updateStock.Enabled = false;
                this.CalcStockInfo.Enabled = true;
                num2 = 8;
                while (num2 < 12)
                {
                    this.StockReportBox.ReportGrid[currentRowIndex, num2] = "0";
                    num2++;
                }
                return;
            }
            BusinessLogic.MyMessageBox("Stock Information updated for row " + Convert.ToString((int) (currentRowIndex + 1)) + " !");
            this.updateStock.Enabled = false;
            this.CalcStockInfo.Enabled = true;
            for (num2 = 8; num2 < 12; num2++)
            {
                this.StockReportBox.ReportGrid[currentRowIndex, num2] = "0";
            }
            try
            {
                this.StockReportBox.ExecuteQueryAndFillGrid();
            }
            catch (Exception exception4)
            {
                exception2 = exception4;
            }
        }

        private void ValidatePackingsDataAndPack()
        {
            int currentRowIndex = this.PackingsGrid.CurrentRowIndex;
            string str = Convert.ToDateTime(this.PackingsGrid[currentRowIndex, 6]).ToLongDateString();
            str = str.Substring(str.IndexOf(",") + 1);
            AppSettingsReader reader = new AppSettingsReader();
            string str2 = Convert.ToString(reader.GetValue("DateDelimiter", typeof(string)));
            object obj2 = new OleDbCommand("SELECT DISTINCT PACKINGSLIP FROM PACKINGS WHERE PACKINGSLIP='" + Convert.ToString(this.PackingsGrid[currentRowIndex, 7]) + "' AND DATEOFPACKING =" + str2 + str + str2 + " AND NOT  (PACKINGSLIP IS  NULL OR  LEN(PACKINGSLIP)=0)", Con).ExecuteScalar();
            if ((obj2 != DBNull.Value) && (Convert.ToString(obj2).Length >= 1))
            {
                throw new Exception("The Packing Slip already exists for this date.\nPlease try again and enter a different Packing Slip");
            }
            Hashtable hashtable = new Hashtable();
            foreach (DataRow row in this.dtPackings.Rows)
            {
                if (hashtable.Contains(row["Item ID"]))
                {
                    hashtable[row["Item ID"]] = Convert.ToInt32(hashtable[row["Item ID"]]) + Convert.ToInt32(row["Chests Packed"]);
                }
                else
                {
                    hashtable.Add(row["Item ID"], row["Chests Packed"]);
                }
            }
            foreach (object obj3 in hashtable.Keys)
            {
                int num2 = Convert.ToInt32(((DataSet) this.StockReportBox.ReportGrid.DataSource).Tables["Stock"].Select("[Item ID]='" + Convert.ToString(obj3) + "'")[0]["Chests"]);
                if (Convert.ToInt32(hashtable[obj3]) > num2)
                {
                    throw new Exception("Number of chests of item " + Convert.ToString(obj3) + " exceeds than available.\nPlease reduce some chests from packing quantity and proceed.!");
                }
            }
            Hashtable hashtable2 = new Hashtable();
            foreach (DataRow row in this.dtPackings.Rows)
            {
                if (hashtable2.Contains(row["Item ID"]))
                {
                    hashtable2[row["Item ID"]] = Convert.ToDecimal(hashtable2[row["Item ID"]]) + Convert.ToDecimal(row["Free Qty packed"]);
                }
                else
                {
                    hashtable2.Add(row["Item ID"], row["Free Qty Packed"]);
                }
            }
            foreach (object obj3 in hashtable2.Keys)
            {
                decimal num3 = Convert.ToDecimal(((DataSet) this.StockReportBox.ReportGrid.DataSource).Tables["Stock"].Select("[Item ID]='" + Convert.ToString(obj3) + "'")[0]["Free Qty"]);
                if (Convert.ToDecimal(hashtable2[obj3]) > num3)
                {
                    throw new Exception("Free Quantity available for item " + Convert.ToString(obj3) + " exceeds than available.\nPlease reduce some free quantity from packing and proceed.!");
                }
            }
            Hashtable hashtable3 = new Hashtable();
            foreach (DataRow row in this.dtPackings.Rows)
            {
                if (hashtable3.Contains(row["Item ID"]))
                {
                    hashtable3[row["Item ID"]] = Convert.ToDecimal(hashtable3[row["Item ID"]]) + Convert.ToDecimal(row["Total Qty packed"]);
                }
                else
                {
                    hashtable3.Add(row["Item ID"], row["Total Qty Packed"]);
                }
            }
            string[] queriesToRun = new string[2 * hashtable3.Keys.Count];
            string str3 = Guid.NewGuid().ToString();
            int index = 0;
            foreach (object obj3 in hashtable3.Keys)
            {
                decimal num5 = Convert.ToDecimal(((DataSet) this.StockReportBox.ReportGrid.DataSource).Tables["Stock"].Select("[Item ID]='" + Convert.ToString(obj3) + "'")[0]["Stock"]);
                if (Convert.ToDecimal(hashtable3[obj3]) > num5)
                {
                    throw new Exception("Total Quantity available for item " + Convert.ToString(obj3) + " exceeds than available.\nPlease reduce the total quantity from packing and proceed.!");
                }
                queriesToRun[index] = "INSERT INTO PACKINGS(PACKINGID,DATEOFPACKING,PACKINGSLIP,ITEMCATEGORYID,CHESTSPACKED,FREEQUANTITYPACKED,PACKINGATTRIBUTES,TOTALQUANTITYPACKED,ISDISPOSED,TRANSACTIONSETIFDISPOSED) VALUES('" + str3 + "','" + str + "','" + Convert.ToString(this.PackingsGrid[currentRowIndex, 7]) + "','" + Convert.ToString(obj3) + "'," + Convert.ToString(Convert.ToInt32(hashtable[obj3])) + "," + Convert.ToString(Convert.ToDecimal(hashtable2[obj3])) + ",null," + Convert.ToString(Convert.ToDecimal(hashtable3[obj3])) + ",0,null)";
                int num6 = Convert.ToInt32(((DataSet) this.StockReportBox.ReportGrid.DataSource).Tables["Stock"].Select("[Item ID]='" + Convert.ToString(obj3) + "'")[0]["Chests"]) - Convert.ToInt32(hashtable[obj3]);
                decimal num7 = Convert.ToDecimal(((DataSet) this.StockReportBox.ReportGrid.DataSource).Tables["Stock"].Select("[Item ID]='" + Convert.ToString(obj3) + "'")[0]["Free Qty"]) - Convert.ToDecimal(hashtable2[obj3]);
                decimal num8 = num5 - Convert.ToDecimal(hashtable3[obj3]);
                queriesToRun[index + 1] = "UPDATE STOCK SET STOCKCOUNTER=" + Convert.ToString(num8) + ",CHESTCOUNT=" + Convert.ToString(num6) + ",FREEQUANTITY=" + Convert.ToString(num7) + " WHERE ITEMCATEGORYID='" + Convert.ToString(obj3) + "'";
                index += 2;
            }
            if (hashtable2 != null)
            {
                hashtable2.Clear();
            }
            if (hashtable != null)
            {
                hashtable.Clear();
            }
            if (hashtable3 != null)
            {
                hashtable3.Clear();
            }
            Exception exception = BusinessLogic.PerformMultipleQueriesWithoutValidation(Con, queriesToRun);
            if (exception != null)
            {
                throw exception;
            }
        }
    }
}

