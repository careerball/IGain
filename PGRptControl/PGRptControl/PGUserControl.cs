namespace PGRptControl
{
    using PGBusinessLogic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;

    public class PGUserControl : UserControl
    {
        private OleDbConnection ActiveCon;
        private int AggregatableColumnNumber;
        private MenuItem AggregateMenuItem;
        private string BindableObject = null;
        private XmlDocument cnfgFile;
        private string cnfgFileName;
        private Container components = null;
        private Hashtable ConditionTable;
        private Panel ContainerPanel;
        private string CriteriaID;
        private Hashtable CriteriaTable;
        private Button ExecQuery;
        private ContextMenu GridColumnAggregateMenu;
        private bool IsTotallingEnabled;
        private string MasterTable = null;
        private DataGrid ParentGrid;
        private ComboBox SearchCriteria;
        private Label StatusLabel;

        public event BeforeSearch_ClickedHandler BeforeSearch_Clicked;

        public event DataSetFormed_Handler DataSetFormed;

        public event QueryParsed_Handler QueryParsed;

        public event Search_ClickedHandler Search_Clicked;

        public event Total_ClickedHandler Total_Clicked;

        public PGUserControl()
        {
            this.InitializeComponent();
        }

        private void AggregateMenuItem_Click(object sender, EventArgs e)
        {
            double total = 0.0;
            if (this.AggregatableColumnNumber != -1)
            {
                int count = this.ParentGrid.BindingContext[this.ParentGrid.DataSource, this.ParentGrid.DataMember].Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        double num4 = (this.ParentGrid[i, this.AggregatableColumnNumber] == null) ? 0.0 : Convert.ToDouble(this.ParentGrid[i, this.AggregatableColumnNumber]);
                        total += num4;
                    }
                }
                catch (Exception exception)
                {
                    BusinessLogic.MyMessageBox(exception.Message);
                    return;
                }
            }
            if (this.Total_Clicked != null)
            {
                this.Total_Clicked(this.AggregatableColumnNumber, total);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
                base.Events.Dispose();
                if (this.cnfgFile != null)
                {
                    this.cnfgFile.RemoveAll();
                    this.cnfgFile = null;
                }
                if (this.CriteriaTable != null)
                {
                    this.CriteriaTable.Clear();
                    this.CriteriaTable = null;
                }
                if (this.ConditionTable != null)
                {
                    this.ConditionTable.Clear();
                    this.ConditionTable = null;
                }
                if (!this.ParentGrid.IsDisposed)
                {
                    this.ParentGrid.DataBindings.Clear();
                    if (((DataSet) this.ParentGrid.DataSource) != null)
                    {
                        try
                        {
                            ((DataSet) this.ParentGrid.DataSource).Clear();
                            ((DataSet) this.ParentGrid.DataSource).Dispose();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    this.ParentGrid.DataSource = null;
                    this.ParentGrid.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void ExecQuery_Click(object sender, EventArgs e)
        {
            if (this.BeforeSearch_Clicked == null)
            {
                this.ExecuteQueryAndFillGrid();
            }
            else if (this.BeforeSearch_Clicked())
            {
                this.ExecuteQueryAndFillGrid();
            }
        }

        public void ExecuteQueryAndFillGrid()
        {
            string parsedQuery = null;
            Exception exception;
            DataSet dataSet = new DataSet();
            dataSet.Relations.Clear();
            dataSet.Tables.Clear();
            dataSet.Clear();
            this.ParentGrid.DataBindings.Clear();
            this.ParentGrid.DataSource = null;
            this.ParentGrid.Refresh();
            try
            {
                parsedQuery = this.GenerateQueryFromControl();
            }
            catch (Exception exception1)
            {
                exception = exception1;
                BusinessLogic.MyMessageBox(exception.Message);
                dataSet.Clear();
                dataSet.Dispose();
                return;
            }
            try
            {
                if ((this.QueryParsed != null) && !this.QueryParsed(parsedQuery))
                {
                    dataSet.Clear();
                    dataSet.Dispose();
                    return;
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
            }
            int num = 0;
            this.StatusLabel.Text = "";
            if (this.ActiveCon != null)
            {
                try
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(parsedQuery, this.ActiveCon);
                    num = adapter.Fill(dataSet, this.MasterTable);
                    adapter.Dispose();
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    BusinessLogic.MyMessageBox(exception.Message);
                }
                foreach (XmlNode node in this.cnfgFile.DocumentElement.ChildNodes.Item(this.SearchCriteria.SelectedIndex).SelectSingleNode("ResultSet").SelectNodes("DetailSet"))
                {
                    try
                    {
                        OleDbDataAdapter adapter2 = new OleDbDataAdapter(this.ParseQuery(node.InnerText), this.ActiveCon);
                        adapter2.Fill(dataSet, node.Attributes["TableName"].Value);
                        adapter2.Dispose();
                    }
                    catch (Exception exception4)
                    {
                        exception = exception4;
                        BusinessLogic.MyMessageBox(exception.Message);
                    }
                }
                foreach (XmlNode node2 in this.cnfgFile.DocumentElement.ChildNodes.Item(this.SearchCriteria.SelectedIndex).SelectSingleNode("ResultSet").SelectNodes("Relation"))
                {
                    try
                    {
                        string str3 = node2.Attributes["Master"].Value;
                        string str4 = node2.Attributes["Details"].Value;
                        string str5 = node2.Attributes["BindingColumn"].Value;
                        string relationName = node2.Attributes["Name"].Value;
                        DataColumn parentColumn = dataSet.Tables[str3].Columns[str5];
                        DataColumn childColumn = dataSet.Tables[str4].Columns[str5];
                        DataRelation relation = new DataRelation(relationName, parentColumn, childColumn, false);
                        dataSet.Relations.Add(relation);
                    }
                    catch (Exception exception5)
                    {
                        exception = exception5;
                        BusinessLogic.MyMessageBox(exception.Message);
                    }
                }
                try
                {
                    if (this.DataSetFormed != null)
                    {
                        this.DataSetFormed(dataSet, this.BindableObject);
                    }
                }
                catch (Exception exception6)
                {
                    exception = exception6;
                }
                try
                {
                    this.ParentGrid.SetDataBinding(dataSet, this.BindableObject);
                }
                catch (Exception exception7)
                {
                    exception = exception7;
                    BusinessLogic.MyMessageBox(exception.Message);
                }
            }
            this.StatusLabel.Text = num.ToString() + " rows fetched";
            if (this.Search_Clicked != null)
            {
                this.Search_Clicked((long) num, parsedQuery);
            }
            dataSet.Dispose();
        }

        private string GenerateQueryFromControl()
        {
            string queryToBind = null;
            try
            {
                XmlNode firstChild = this.cnfgFile.DocumentElement.ChildNodes.Item(this.SearchCriteria.SelectedIndex).SelectSingleNode("ResultSet").FirstChild;
                if ((firstChild != null) && (firstChild.Name == "MasterSet"))
                {
                    this.BindableObject = firstChild.ParentNode.Attributes["BindTo"].Value;
                    this.MasterTable = firstChild.Attributes["TableName"].Value;
                    queryToBind = firstChild.InnerText;
                }
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                return null;
            }
            return this.ParseQuery(queryToBind);
        }

        public double getTotal(int colIndex)
        {
            double num = 0.0;
            if (colIndex != -1)
            {
                int count = this.ParentGrid.BindingContext[this.ParentGrid.DataSource, this.ParentGrid.DataMember].Count;
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        double num4 = (this.ParentGrid[i, colIndex] == null) ? 0.0 : Convert.ToDouble(this.ParentGrid[i, colIndex]);
                        num += num4;
                    }
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }
            return num;
        }

        private void InitializeComponent()
        {
            this.ParentGrid = new DataGrid();
            this.GridColumnAggregateMenu = new ContextMenu();
            this.AggregateMenuItem = new MenuItem();
            this.SearchCriteria = new ComboBox();
            this.ContainerPanel = new Panel();
            this.ExecQuery = new Button();
            this.StatusLabel = new Label();
            this.ParentGrid.BeginInit();
            base.SuspendLayout();
            this.ParentGrid.DataMember = "";
            this.ParentGrid.HeaderForeColor = SystemColors.ControlText;
            this.ParentGrid.Location = new Point(0x18, 0xf8);
            this.ParentGrid.Name = "ParentGrid";
            this.ParentGrid.PreferredColumnWidth = 120;
            this.ParentGrid.ReadOnly = true;
            this.ParentGrid.RowHeaderWidth = 70;
            this.ParentGrid.Size = new Size(640, 0x158);
            this.ParentGrid.TabIndex = 1;
            this.ParentGrid.MouseDown += new MouseEventHandler(this.ParentGrid_MouseDown);
            this.GridColumnAggregateMenu.MenuItems.AddRange(new MenuItem[] { this.AggregateMenuItem });
            this.AggregateMenuItem.Index = 0;
            this.AggregateMenuItem.Text = "Total";
            this.AggregateMenuItem.Click += new EventHandler(this.AggregateMenuItem_Click);
            this.SearchCriteria.DropDownStyle = ComboBoxStyle.DropDownList;
            this.SearchCriteria.Location = new Point(0x18, 8);
            this.SearchCriteria.Name = "SearchCriteria";
            this.SearchCriteria.Size = new Size(0x128, 0x15);
            this.SearchCriteria.TabIndex = 3;
            this.SearchCriteria.SelectedIndexChanged += new EventHandler(this.SearchCriteria_SelectedIndexChanged);
            this.ContainerPanel.AutoScroll = true;
            this.ContainerPanel.BackColor = SystemColors.ControlLightLight;
            this.ContainerPanel.BorderStyle = BorderStyle.Fixed3D;
            this.ContainerPanel.Location = new Point(0x20, 0x30);
            this.ContainerPanel.Name = "ContainerPanel";
            this.ContainerPanel.Size = new Size(0x278, 0x98);
            this.ContainerPanel.TabIndex = 4;
            this.ContainerPanel.TabStop = true;
            this.ExecQuery.BackColor = SystemColors.Control;
            this.ExecQuery.Cursor = Cursors.Hand;
            this.ExecQuery.ForeColor = SystemColors.ControlText;
            this.ExecQuery.Location = new Point(0x220, 0xd8);
            this.ExecQuery.Name = "ExecQuery";
            this.ExecQuery.Size = new Size(120, 0x18);
            this.ExecQuery.TabIndex = 5;
            this.ExecQuery.Text = "Search...";
            this.ExecQuery.Click += new EventHandler(this.ExecQuery_Click);
            this.StatusLabel.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.StatusLabel.ForeColor = SystemColors.ControlText;
            this.StatusLabel.Location = new Point(0x18, 600);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new Size(400, 0x10);
            this.StatusLabel.TabIndex = 6;
            base.Controls.Add(this.StatusLabel);
            base.Controls.Add(this.ExecQuery);
            base.Controls.Add(this.ContainerPanel);
            base.Controls.Add(this.SearchCriteria);
            base.Controls.Add(this.ParentGrid);
            base.Name = "PGUserControl";
            base.Size = new Size(0x2b0, 0x278);
            base.Load += new EventHandler(this.PGUserControl_Load);
            this.ParentGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void loadReportQueries()
        {
            this.ContainerPanel.Controls.Clear();
            Label[] labelArray = new Label[this.CriteriaTable.Keys.Count];
            ComboBox[] boxArray = new ComboBox[this.CriteriaTable.Keys.Count];
            ComboBox[] boxArray2 = new ComboBox[this.CriteriaTable.Keys.Count];
            this.ConditionTable.Clear();
            int index = 0;
            foreach (string str in this.CriteriaTable.Keys)
            {
                string str2 = str.Replace(" ", "_");
                labelArray[index] = new Label();
                labelArray[index].Name = str2.ToString();
                labelArray[index].Text = str.ToString();
                labelArray[index].Width = 0x80;
                labelArray[index].BackColor = this.ContainerPanel.BackColor;
                labelArray[index].ForeColor = this.ContainerPanel.ForeColor;
                labelArray[index].TextAlign = ContentAlignment.MiddleCenter;
                labelArray[index].Location = new Point(5, (index * 30) + 15);
                this.ContainerPanel.Controls.Add(labelArray[index]);
                boxArray[index] = new ComboBox();
                boxArray[index].Name = str2 + "_Condition";
                boxArray[index].DropDownStyle = ComboBoxStyle.DropDownList;
                boxArray[index].Width = 200;
                boxArray[index].BackColor = this.ContainerPanel.BackColor;
                boxArray[index].ForeColor = this.ContainerPanel.ForeColor;
                boxArray[index].Location = new Point(150, (index * 30) + 15);
                this.ContainerPanel.Controls.Add(boxArray[index]);
                boxArray[index].SelectedIndexChanged += new EventHandler(this.queryCondition_SelectedIndexChanged);
                boxArray2[index] = new ComboBox();
                boxArray2[index].Name = str2 + "_Domain";
                boxArray2[index].Width = 240;
                boxArray2[index].BackColor = this.ContainerPanel.BackColor;
                boxArray2[index].ForeColor = this.ContainerPanel.ForeColor;
                boxArray2[index].Location = new Point(370, (index * 30) + 15);
                boxArray2[index].DropDownStyle = ComboBoxStyle.Simple;
                boxArray2[index].Height = 0x19;
                boxArray2[index].TabIndex = boxArray[index].TabIndex + 1;
                boxArray2[index].KeyPress += new KeyPressEventHandler(this.queryDomain_KeyPressed);
                this.ContainerPanel.Controls.Add(boxArray2[index]);
                XmlNode node = (XmlNode) this.CriteriaTable[str];
                foreach (XmlNode node2 in node.FirstChild.FirstChild.ChildNodes)
                {
                    if (node2.Attributes.Count == 2)
                    {
                        boxArray[index].Items.Add(node2.Attributes.Item(1).Value);
                        this.ConditionTable[str2 + "_Condition_" + node2.Attributes.Item(1).Value] = node2.FirstChild;
                    }
                    else
                    {
                        boxArray[index].Items.Add("N/A");
                    }
                }
                try
                {
                    if (boxArray[index].Items.Count > 0)
                    {
                        boxArray[index].SelectedIndex = 0;
                    }
                }
                catch (Exception exception)
                {
                    BusinessLogic.MyMessageBox(exception.Message);
                }
                index++;
            }
        }

        private void ParentGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.IsTotallingEnabled && (e.Button.ToString() == "Right"))
            {
                DataGrid.HitTestInfo info = this.ParentGrid.HitTest(e.X, e.Y);
                if (info.Type == DataGrid.HitTestType.ColumnHeader)
                {
                    this.AggregatableColumnNumber = info.Column;
                    this.GridColumnAggregateMenu.Show(this.ParentGrid, new Point(e.X, e.Y));
                }
            }
        }

        private string ParseQuery(string QueryToBind)
        {
            string[] strArray = QueryToBind.Split(new char[] { '{', '}' });
            foreach (string str in strArray)
            {
                string str8;
                if (!this.CriteriaTable.Contains(str))
                {
                    continue;
                }
                XmlNode node = (XmlNode) this.CriteriaTable[str];
                string str2 = null;
                string str3 = null;
                string str4 = null;
                str2 = node.FirstChild.Attributes["Mapsto"].Value;
                string str5 = str.Replace(" ", "_");
                string str6 = null;
                string text = "%";
                foreach (Control control in this.ContainerPanel.Controls)
                {
                    if (((control.GetType().Name == "ComboBox") && (control.Name.Length > 0)) && (control.Name == (str5 + "_Condition")))
                    {
                        str6 = ((ComboBox) control).Text.ToString();
                        ComboBox nextControl = (ComboBox) this.ContainerPanel.GetNextControl(control, true);
                        if (nextControl != null)
                        {
                            text = nextControl.Text;
                            try
                            {
                                text = (text.Length > 0) ? text : "%";
                            }
                            catch (Exception)
                            {
                                text = "%";
                            }
                            break;
                        }
                    }
                }
                XmlNode node2 = null;
                try
                {
                    node2 = (XmlNode) this.ConditionTable[str5 + "_Condition_" + str6];
                }
                catch (Exception exception2)
                {
                    BusinessLogic.MyMessageBox(exception2.Message);
                }
                str3 = " Like ";
                if (node2.Attributes["Key"] != null)
                {
                    if (node2.Attributes["Key"].Value == "%")
                    {
                        str4 = " '" + text + "%' ";
                    }
                    else if (node2.Attributes["Key"].Value == "%%")
                    {
                        str4 = " '%" + text + "%' ";
                    }
                    else if (text != "%")
                    {
                        str3 = " " + node2.ParentNode.Attributes["Value"].Value + " ";
                        str8 = node2.Attributes["Key"].Value.Substring(0, 1);
                        str4 = str8 + text + str8;
                    }
                    else
                    {
                        str4 = " '" + text + "' or " + str2 + " is null ";
                    }
                }
                else if (text != "%")
                {
                    str3 = " " + node2.ParentNode.Attributes["Value"].Value + " ";
                    if (node.FirstChild.Attributes["DataType"].Value == "String")
                    {
                        str4 = " '" + text + "' ";
                    }
                    else
                    {
                        str8 = " ";
                        str4 = str8 + text + str8;
                    }
                }
                else
                {
                    str4 = " '" + text + "' or " + str2 + " is null ";
                }
                QueryToBind = QueryToBind.Replace("{" + str + "}", str2 + str3 + str4);
            }
            return QueryToBind;
        }

        private void PGUserControl_Load(object sender, EventArgs e)
        {
            this.StatusLabel.Location = new Point(this.ContainerPanel.Location.X, (this.ContainerPanel.Location.Y + this.ContainerPanel.Height) + 6);
            this.cnfgFile = new XmlDocument();
            this.CriteriaTable = new Hashtable();
            this.ConditionTable = new Hashtable();
            this.AggregatableColumnNumber = -1;
            if ((this.cnfgFileName == null) || (this.cnfgFileName.Length == 0))
            {
                this.cnfgFileName = Application.StartupPath + @"\" + base.Name + ".xml";
            }
            try
            {
                this.cnfgFile.Load(this.cnfgFileName);
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                base.Enabled = false;
                this.ConfigFile = "";
                return;
            }
            foreach (XmlNode node in this.cnfgFile.DocumentElement.ChildNodes)
            {
                this.SearchCriteria.Items.Add(node.Attributes["Value"].Value);
            }
            if (this.SearchCriteria.Items.Count > 0)
            {
                this.SearchCriteria.SelectedIndex = 0;
            }
        }

        private void queryCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ctl = (ComboBox) sender;
            XmlNode node = (XmlNode) this.ConditionTable[ctl.Name + "_" + ctl.Text];
            ComboBox nextControl = (ComboBox) this.ContainerPanel.GetNextControl(ctl, true);
            if (nextControl == null)
            {
                BusinessLogic.MyMessageBox("Domain Box is null");
            }
            else
            {
                nextControl.Items.Clear();
                string str = node.Attributes["Type"].Value;
                if (str != null)
                {
                    if (!(str == "TextBox"))
                    {
                        if (str == "ComboBox")
                        {
                            nextControl.DropDownStyle = ComboBoxStyle.DropDown;
                            nextControl.Sorted = true;
                        }
                        else if (str == "ListBox")
                        {
                            nextControl.DropDownStyle = ComboBoxStyle.DropDownList;
                            nextControl.Sorted = true;
                        }
                    }
                    else
                    {
                        nextControl.DropDownStyle = ComboBoxStyle.Simple;
                    }
                }
                if (node.HasChildNodes)
                {
                    if (node.FirstChild.Name == "Value")
                    {
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            nextControl.Items.Add(node2.InnerText);
                        }
                    }
                    else if (node.FirstChild.Name == "Query")
                    {
                        try
                        {
                            if (this.ActiveCon != null)
                            {
                                DataSet dataSet = new DataSet();
                                OleDbDataAdapter adapter = new OleDbDataAdapter(node.FirstChild.InnerText, this.ActiveCon);
                                if (adapter != null)
                                {
                                    if ((adapter.Fill(dataSet) > 0) && (dataSet != null))
                                    {
                                        foreach (DataRow row in dataSet.Tables[0].Rows)
                                        {
                                            if (!row.IsNull(0))
                                            {
                                                nextControl.Items.Add(row[0]);
                                            }
                                        }
                                        dataSet.Clear();
                                    }
                                    adapter.Dispose();
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            BusinessLogic.MyMessageBox(exception.Message);
                        }
                    }
                    if ((nextControl.Items.Count > 0) && (nextControl.DropDownStyle == ComboBoxStyle.DropDownList))
                    {
                        nextControl.SelectedIndex = 0;
                    }
                }
            }
        }

        private void queryDomain_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.ExecuteQueryAndFillGrid();
            }
        }

        private void SearchCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            Exception exception;
            try
            {
                if (this.CriteriaID != this.SearchCriteria.Text)
                {
                    try
                    {
                        this.ParentGrid.DataBindings.Clear();
                        this.ParentGrid.DataSource = null;
                        this.ParentGrid.Refresh();
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        BusinessLogic.MyMessageBox(exception.Message);
                    }
                    this.CriteriaID = this.SearchCriteria.Text;
                    this.CriteriaTable.Clear();
                    foreach (XmlNode node in this.cnfgFile.DocumentElement.ChildNodes.Item(this.SearchCriteria.SelectedIndex))
                    {
                        if ((node != null) && (node.Name == "ReportCondition"))
                        {
                            this.CriteriaTable[node.Attributes["ID"].Value] = node;
                        }
                    }
                    this.loadReportQueries();
                    this.ContainerPanel.Focus();
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                BusinessLogic.MyMessageBox(exception.Message);
            }
        }

        [Browsable(true)]
        public string ConfigFile
        {
            get
            {
                return this.cnfgFileName;
            }
            set
            {
                this.cnfgFileName = value;
            }
        }

        [Browsable(false)]
        public ComboBox CriteriaBox
        {
            get
            {
                return this.SearchCriteria;
            }
        }

        [Browsable(false)]
        public OleDbConnection DBCon
        {
            get
            {
                return this.ActiveCon;
            }
            set
            {
                this.ActiveCon = value;
            }
        }

        [Browsable(false)]
        public Panel QueryPanel
        {
            get
            {
                return this.ContainerPanel;
            }
        }

        [Browsable(false)]
        public DataGrid ReportGrid
        {
            get
            {
                return this.ParentGrid;
            }
        }

        [Browsable(false)]
        public Label ResultStatus
        {
            get
            {
                return this.StatusLabel;
            }
        }

        [Browsable(false)]
        public Button SearchButton
        {
            get
            {
                return this.ExecQuery;
            }
        }

        [Browsable(true)]
        public bool TotallingEnabled
        {
            get
            {
                return this.IsTotallingEnabled;
            }
            set
            {
                this.IsTotallingEnabled = value;
            }
        }
    }
}

