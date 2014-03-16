namespace IGain
{
    using PGBusinessLogic;
    using PGRptControl;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class SalePage : Form
    {
        private Container components = null;
        public static OleDbConnection Con = null;
        private DataTable dtSell;
        private Button MoveToSellGrid;
        private TextBox NumberOfBoxes;
        private long numberOfRows;
        private DataRow rowSell;
        public PGUserControl SalesReportBox;
        private Button Sell;
        private DataGrid SellingGrid;
        private Button updateBoxes;

        public SalePage()
        {
            this.InitializeComponent();
            if (Con != null)
            {
                this.SalesReportBox.DBCon = Con;
            }
            else
            {
                this.SalesReportBox.Enabled = false;
                BusinessLogic.MyMessageBox("Connection is null! Can not continue.");
                return;
            }
            try
            {
                this.FormSellDataTable();
            }
            catch (Exception exception)
            {
                this.SalesReportBox.Enabled = false;
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Sell Table can no be formed! Can not continue.");
                return;
            }
            this.SalesReportBox.ReportGrid.CurrentCellChanged += new EventHandler(this.ReportGrid_CurrentCellChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FormSellDataTable()
        {
            this.dtSell = new DataTable("SellTable");
            this.dtSell.Columns.Add("Selling Rate", System.Type.GetType("System.Decimal"));
            this.dtSell.Columns.Add("Total Qty Sold", System.Type.GetType("System.Decimal"));
            this.dtSell.Columns.Add("Boxes Sold", System.Type.GetType("System.Int32"));
            this.dtSell.Columns.Add("Free Qty Sold", System.Type.GetType("System.Decimal"));
            this.dtSell.Columns.Add("Selling Date", System.Type.GetType("System.DateTime"));
            this.dtSell.Columns.Add("Selling Slip");
            this.dtSell.Columns.Add("Transaction Amount", System.Type.GetType("System.Decimal"));
            this.dtSell.AcceptChanges();
            this.rowSell = this.dtSell.NewRow();
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(SalePage));
            this.SalesReportBox = new PGUserControl();
            this.Sell = new Button();
            this.SellingGrid = new DataGrid();
            this.MoveToSellGrid = new Button();
            this.updateBoxes = new Button();
            this.NumberOfBoxes = new TextBox();
            this.SellingGrid.BeginInit();
            base.SuspendLayout();
            this.SalesReportBox.ConfigFile = "";
            this.SalesReportBox.DBCon = null;
            this.SalesReportBox.Location = new Point(0, 0);
            this.SalesReportBox.Name = "SalesReportBox";
            this.SalesReportBox.Size = new Size(0x2b8, 0x1b0);
            this.SalesReportBox.TabIndex = 0;
            this.SalesReportBox.TotallingEnabled = true;
            this.SalesReportBox.Search_Clicked += new Search_ClickedHandler(this.SalesReportBox_Search_Clicked);
            this.SalesReportBox.QueryParsed += new QueryParsed_Handler(this.SalesReportBox_QueryParsed);
            this.SalesReportBox.Total_Clicked += new Total_ClickedHandler(this.SalesReportBox_Total_Clicked);
            this.Sell.Enabled = false;
            this.Sell.Location = new Point(560, 0x240);
            this.Sell.Name = "Sell";
            this.Sell.Size = new Size(0x68, 0x18);
            this.Sell.TabIndex = 2;
            this.Sell.Text = "Sell";
            this.Sell.Click += new EventHandler(this.Sell_Click);
            this.SellingGrid.CaptionText = "Sell Items";
            this.SellingGrid.DataMember = "";
            this.SellingGrid.HeaderForeColor = SystemColors.ControlText;
            this.SellingGrid.Location = new Point(0x18, 480);
            this.SellingGrid.Name = "SellingGrid";
            this.SellingGrid.PreferredColumnWidth = 120;
            this.SellingGrid.Size = new Size(640, 0x58);
            this.SellingGrid.TabIndex = 3;
            this.SellingGrid.CurrentCellChanged += new EventHandler(this.SellingGrid_CurrentCellChanged);
            this.MoveToSellGrid.Enabled = false;
            this.MoveToSellGrid.Image = (Image) manager.GetObject("MoveToSellGrid.Image");
            this.MoveToSellGrid.Location = new Point(320, 440);
            this.MoveToSellGrid.Name = "MoveToSellGrid";
            this.MoveToSellGrid.Size = new Size(0x20, 0x20);
            this.MoveToSellGrid.TabIndex = 4;
            this.MoveToSellGrid.Click += new EventHandler(this.MoveToSellGrid_Click);
            this.updateBoxes.Location = new Point(560, 0x1c0);
            this.updateBoxes.Name = "updateBoxes";
            this.updateBoxes.Size = new Size(0x60, 0x18);
            this.updateBoxes.TabIndex = 5;
            this.updateBoxes.Text = "Update Boxes";
            this.updateBoxes.Visible = false;
            this.updateBoxes.Click += new EventHandler(this.updateBoxes_Click);
            this.NumberOfBoxes.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.NumberOfBoxes.ForeColor = SystemColors.HotTrack;
            this.NumberOfBoxes.Location = new Point(0x1d0, 0x1c0);
            this.NumberOfBoxes.Name = "NumberOfBoxes";
            this.NumberOfBoxes.Size = new Size(80, 0x15);
            this.NumberOfBoxes.TabIndex = 6;
            this.NumberOfBoxes.Text = "";
            this.NumberOfBoxes.Visible = false;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b8, 0x25d);
            base.Controls.Add(this.NumberOfBoxes);
            base.Controls.Add(this.updateBoxes);
            base.Controls.Add(this.MoveToSellGrid);
            base.Controls.Add(this.SellingGrid);
            base.Controls.Add(this.Sell);
            base.Controls.Add(this.SalesReportBox);
            base.MaximizeBox = false;
            base.Name = "SalePage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manage Sales";
            this.SellingGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void MoveToSellGrid_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateSalesData();
                this.ResetSellGridData();
                this.MoveToSellGrid.Enabled = false;
                this.Sell.Enabled = true;
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Importing of package for sale failed!");
            }
        }

        private void ReportGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.SalesReportBox.ReportGrid.CurrentCell.ColumnNumber == 10)
            {
                this.updateBoxes.Visible = true;
                this.NumberOfBoxes.Visible = true;
                try
                {
                    this.NumberOfBoxes.Text = Convert.ToString(this.SalesReportBox.ReportGrid[this.SalesReportBox.ReportGrid.CurrentRowIndex, 10]);
                }
                catch (Exception exception)
                {
                    exception.Source = null;
                }
            }
            else
            {
                this.updateBoxes.Visible = false;
                this.NumberOfBoxes.Visible = false;
            }
        }

        private void ResetSellGridData()
        {
            this.dtSell.Rows.Clear();
            this.dtSell.Clear();
            this.rowSell["Selling Rate"] = "0.0";
            this.rowSell["Total Qty Sold"] = "0.0";
            this.rowSell["Boxes Sold"] = "0";
            this.rowSell["Free Qty Sold"] = "0.0";
            this.rowSell["Selling Date"] = DateTime.Now.ToLongDateString().Substring(DateTime.Now.ToLongDateString().IndexOf(',') + 1);
            this.rowSell["Selling Slip"] = "Enter selling slip";
            this.rowSell["Transaction Amount"] = "0.0";
            this.dtSell.Rows.Add(this.rowSell);
            this.dtSell.AcceptChanges();
            if (this.SellingGrid.DataSource == null)
            {
                this.SellingGrid.DataSource = this.dtSell;
            }
            CurrencyManager manager = (CurrencyManager) this.SellingGrid.BindingContext[this.SellingGrid.DataSource, this.SellingGrid.DataMember];
            DataView list = (DataView) manager.List;
            list.AllowNew = false;
        }

        private bool SalesReportBox_QueryParsed(string parsedQuery)
        {
            return true;
        }

        private void SalesReportBox_Search_Clicked(long RowsReturned, string MasterQuery)
        {
            this.numberOfRows = RowsReturned;
            try
            {
                if (this.SellingGrid.DataSource != null)
                {
                    this.SellingGrid.DataBindings.Clear();
                    ((DataTable) this.SellingGrid.DataSource).Clear();
                    this.SellingGrid.DataSource = null;
                }
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
            }
            if (RowsReturned > 0L)
            {
                this.MoveToSellGrid.Enabled = true;
            }
            else
            {
                this.MoveToSellGrid.Enabled = false;
            }
        }

        private void SalesReportBox_Total_Clicked(int ColumnIndex, double Total)
        {
            BusinessLogic.MyMessageBox(Convert.ToString(Total));
        }

        private void Sell_Click(object sender, EventArgs e)
        {
            Exception exception2;
            try
            {
                this.ValidateSalesData();
                if (Convert.ToDecimal(this.SellingGrid[0, 0]) < 0M)
                {
                    throw new Exception("Rate can not be less than 0!");
                }
                if (Convert.ToDouble(this.SellingGrid[0, 1]) > this.SalesReportBox.getTotal(2))
                {
                    throw new Exception("There is not enough packed quantity to be sold!");
                }
                if ((Convert.ToUInt32(this.SellingGrid[0, 2]) > Convert.ToUInt32(this.SalesReportBox.ReportGrid[0, 10])) && (BusinessLogic.MyMessageBox("The number of boxes being sold is more than available.\nAre you sure you want to continue?", "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                {
                    throw new Exception("Can not continue due to insufficient boxes for sale!");
                }
                if (Convert.ToDecimal(this.SellingGrid[0, 3]) < 0M)
                {
                    throw new Exception("Free Qty can not be less than 0!");
                }
                string str = Convert.ToDateTime(this.SellingGrid[0, 4]).ToLongDateString();
                str = str.Substring(str.IndexOf(",") + 1);
                AppSettingsReader reader = new AppSettingsReader();
                string str2 = Convert.ToString(reader.GetValue("DateDelimiter", typeof(string)));
                object obj2 = new OleDbCommand("SELECT DISTINCT SELLINGSLIP FROM SELLINGS WHERE SELLINGSLIP='" + Convert.ToString(this.SellingGrid[0, 5]) + "' AND SELLINGDATE =" + str2 + str + str2 + " AND NOT  (SELLINGSLIP IS  NULL OR  LEN(SELLINGSLIP)=0)", Con).ExecuteScalar();
                if ((obj2 != DBNull.Value) && (Convert.ToString(obj2).Length >= 1))
                {
                    throw new Exception("The Selling Slip already exists for this date.\nPlease try again and enter a different Selling Slip");
                }
                string[] queriesToRun = new string[1L + (2L * this.numberOfRows)];
                long nextTransactionID = BusinessLogic.GetNextTransactionID(Con);
                if (nextTransactionID < 0L)
                {
                    throw new Exception("Error! TransactionID can not be less than 1");
                }
                string str3 = Guid.NewGuid().ToString();
                string str4 = Guid.NewGuid().ToString();
                queriesToRun[0] = "INSERT INTO SELLINGS(SELLINGID,PACKINGID,BOXESSOLD,FREEQUANTITYSOLD,SELLINGDATE,TOTALQUANTITYSOLD,TRANSACTIONSET,SELLINGATTRIBUTES,SELLINGRATE,SELLINGSLIP) VALUES ('" + str4 + "','" + Convert.ToString(this.SalesReportBox.ReportGrid[0, 9]) + "'," + Convert.ToString(this.SellingGrid[0, 2]) + "," + Convert.ToString(this.SellingGrid[0, 3]) + ",'" + str + "'," + Convert.ToString(this.SellingGrid[0, 1]) + ",'" + str3 + "',null," + Convert.ToString(this.SellingGrid[0, 0]) + ",'" + Convert.ToString(this.SellingGrid[0, 5]) + "')";
                double num2 = this.SalesReportBox.getTotal(2);
                for (int i = 1; i <= this.numberOfRows; i++)
                {
                    object obj3 = new OleDbCommand("SELECT ACCOUNTID FROM ACCOUNTTYPES WHERE ACCOUNTTYPE='SALE' AND ITEMCATEGORYID='" + Convert.ToString(this.SalesReportBox.ReportGrid[i - 1, 5]) + "'", Con).ExecuteScalar();
                    if (obj3 == DBNull.Value)
                    {
                        throw new Exception("Error! AccountID can not be null");
                    }
                    string str5 = Convert.ToString(obj3);
                    queriesToRun[i] = "UPDATE PACKINGS SET TOTALQUANTITYPACKED=" + Convert.ToString((double) (Math.Abs((double) (num2 - Convert.ToDouble(this.SellingGrid[0, 1]))) * (Convert.ToDouble(this.SalesReportBox.ReportGrid[i - 1, 2]) / num2))) + " ,ISDISPOSED=" + ((Convert.ToDouble(this.SellingGrid[0, 1]) >= this.SalesReportBox.getTotal(2)) ? "-1" : "0") + " ,TRANSACTIONSETIFDISPOSED= '" + str3 + "',BOXESFORMED=" + ((Convert.ToUInt32(this.SellingGrid[0, 2]) >= Convert.ToUInt32(this.SalesReportBox.ReportGrid[0, 10])) ? "0" : Convert.ToString((uint) (Convert.ToUInt32(this.SalesReportBox.ReportGrid[0, 10]) - Convert.ToUInt32(this.SellingGrid[0, 2])))) + " WHERE PACKINGID='" + Convert.ToString(this.SalesReportBox.ReportGrid[0, 9]) + "' AND ITEMCATEGORYID='" + Convert.ToString(this.SalesReportBox.ReportGrid[i - 1, 5]) + "'";
                    queriesToRun[(int) ((IntPtr) (this.numberOfRows + i))] = "INSERT INTO TRANSACTIONS(TRANSACTIONID,ACCOUNTDEBITED,ACCOUNTCREDITED,DEBIT,CREDIT,STOCKAFFECTED,ITEMCATEGORYID,STOCKCOUNTER,RUNNINGRATE,DATEOFTRANSACTION,SLIPISSUED,SLIPNUMBER,TRANSACTIONSET,ISCREDITABLE,CREDITID,SEQUENCEOFINTERESTAPPLICATION,CHESTSAPPENDED,FREEQUANTITYAPPENDED,TRANSACTIONATTRIBUTES) VALUES ('" + Convert.ToString(nextTransactionID) + "','Cash-Cash','" + str5 + "'," + Convert.ToString((double) ((Convert.ToDouble(this.SalesReportBox.ReportGrid[i - 1, 2]) / num2) * Convert.ToDouble(this.SellingGrid[0, 6]))) + "," + Convert.ToString((double) ((Convert.ToDouble(this.SalesReportBox.ReportGrid[i - 1, 2]) / num2) * Convert.ToDouble(this.SellingGrid[0, 6]))) + ",0,null,0.0,0.0,'" + str + "',-1,'" + Convert.ToString(this.SellingGrid[0, 5]) + "','" + str3 + "',0,null,0,0,0,null)";
                    nextTransactionID += 1L;
                }
                Exception exception = BusinessLogic.PerformMultipleQueriesWithoutValidation(Con, queriesToRun);
                if (exception != null)
                {
                    throw exception;
                }
            }
            catch (Exception exception1)
            {
                exception2 = exception1;
                BusinessLogic.MyMessageBox(exception2.Message);
                BusinessLogic.MyMessageBox("Package can not be sold!");
                return;
            }
            try
            {
                this.SalesReportBox.ExecuteQueryAndFillGrid();
                this.ResetSellGridData();
                this.MoveToSellGrid.Enabled = true;
            }
            catch (Exception exception3)
            {
                exception2 = exception3;
                BusinessLogic.MyMessageBox("You are advsised to re-query the packed data before proceeding!");
            }
            BusinessLogic.MyMessageBox("Package Sold");
        }

        private void SellingGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.SellingGrid.CurrentCell.ColumnNumber == 6)
            {
                this.SellingGrid[this.SellingGrid.CurrentCell] = Convert.ToDecimal(this.SellingGrid[this.SellingGrid.CurrentCell]) + (Convert.ToDecimal(this.SellingGrid[0, 0]) * Convert.ToDecimal(this.SellingGrid[0, 1]));
            }
        }

        private void updateBoxes_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateSalesData();
                string queryToRun = string.Concat(new object[] { "UPDATE PACKINGS SET BOXESFORMED=", Convert.ToInt32(this.NumberOfBoxes.Text), " WHERE PACKINGID='", Convert.ToString(this.SalesReportBox.ReportGrid[0, 9]), "'" });
                Exception exception = BusinessLogic.ModifyStoreHouse(Con, queryToRun, null);
                if (exception != null)
                {
                    throw exception;
                }
                BusinessLogic.MyMessageBox("Number of boxes updated");
                this.SalesReportBox.ExecuteQueryAndFillGrid();
            }
            catch (Exception exception2)
            {
                BusinessLogic.MyMessageBox(exception2.Message);
                BusinessLogic.MyMessageBox("Update of boxes failed!");
            }
        }

        private void ValidateSalesData()
        {
            DataRow[] rowArray = ((DataSet) this.SalesReportBox.ReportGrid.DataSource).Tables["Packings"].Select("PackID is not null");
            if ((rowArray == null) || (rowArray.Length == 0))
            {
                throw new Exception("No rows to validate");
            }
            string str = Convert.ToString(rowArray[0]["PackID"]);
            foreach (DataRow row in rowArray)
            {
                if (Convert.ToString(row["PackID"]) != str)
                {
                    throw new Exception("There are items which belong to different packings.\nPlease refine your search to filter out items which belong to distinct packings!");
                }
            }
            int num = Convert.ToInt32(new OleDbCommand("SELECT COUNT(*) FROM PACKINGS WHERE ISDISPOSED =0 AND PACKINGID='" + str + "'", Con).ExecuteScalar());
            if (num == 0)
            {
                throw new Exception("This packing may be already sold!");
            }
            if (num != rowArray.Length)
            {
                throw new Exception("There are some items in this packing that are missing! \n Please refine your query to get the exhaustive list of items for this packing.");
            }
        }
    }
}

