namespace IGain
{
    using PGBusinessLogic;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public class AttributesForm : Form
    {
        private string attributeSchemaXml;
        private DataSet attributeSet;
        public DataGrid AttributesGrid;
        private DataTable attributesTable;
        private Container components = null;
        public static OleDbConnection Con = null;
        public static string srcColumn = null;
        public static string srcCommand = null;
        public static string srcTable = null;
        private Button UpdateAttributes;
        public static string whereQueryPartOfUpdate = null;

        public AttributesForm()
        {
            this.InitializeComponent();
            if (((((Con == null) || (srcCommand == null)) || ((srcCommand.Length < 6) || (srcTable == null))) || (((srcTable.Length < 1) || (srcColumn == null)) || ((srcColumn.Length < 1) || (whereQueryPartOfUpdate == null)))) || (whereQueryPartOfUpdate.Length < 6))
            {
                this.AttributesGrid.Enabled = false;
                this.UpdateAttributes.Enabled = false;
            }
        }

        private void AttributesForm_Load(object sender, EventArgs e)
        {
            try
            {
                OleDbCommand command = new OleDbCommand(srcCommand, Con);
                this.attributeSchemaXml = Convert.ToString(command.ExecuteScalar());
                command.Dispose();
                this.attributeSet = new DataSet("AttributesSet");
                if ((this.attributeSchemaXml == null) || (this.attributeSchemaXml.Length < 1))
                {
                    this.attributesTable = this.attributeSet.Tables.Add("AttributesTable");
                    this.attributesTable.Columns.Add("Attribute Name");
                    this.attributesTable.Columns.Add("Attribute Value");
                    DataRow row = this.attributesTable.NewRow();
                    row["Attribute Name"] = "Enter name Of attribute";
                    row["Attribute Value"] = "Enter attribute value";
                    this.attributesTable.Rows.Add(row);
                    this.AttributesGrid.SetDataBinding(this.attributeSet, this.attributeSet.Tables[0].TableName);
                }
                else
                {
                    this.attributeSet.ReadXml(new XmlTextReader(new StringReader(this.attributeSchemaXml)));
                    this.attributeSchemaXml = null;
                    this.AttributesGrid.SetDataBinding(this.attributeSet, this.attributeSet.Tables[0].TableName);
                }
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                if (this.attributeSet != null)
                {
                    this.attributeSet.Clear();
                    this.attributeSet.Dispose();
                }
                if (this.attributesTable != null)
                {
                    this.attributesTable.Clear();
                    this.attributesTable.Dispose();
                }
                if (this.AttributesGrid.DataSource != null)
                {
                    ((DataSet) this.AttributesGrid.DataSource).Clear();
                    ((DataSet) this.AttributesGrid.DataSource).Dispose();
                }
                this.AttributesGrid.DataBindings.Clear();
                this.AttributesGrid.Dispose();
                base.Events.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.AttributesGrid = new DataGrid();
            this.UpdateAttributes = new Button();
            this.AttributesGrid.BeginInit();
            base.SuspendLayout();
            this.AttributesGrid.CaptionText = "Attributes";
            this.AttributesGrid.DataMember = "";
            this.AttributesGrid.HeaderForeColor = SystemColors.ControlText;
            this.AttributesGrid.Name = "AttributesGrid";
            this.AttributesGrid.PreferredColumnWidth = 150;
            this.AttributesGrid.RowHeadersVisible = false;
            this.AttributesGrid.RowHeaderWidth = 200;
            this.AttributesGrid.Size = new Size(0x138, 240);
            this.AttributesGrid.TabIndex = 9;
            this.UpdateAttributes.Location = new Point(0, 240);
            this.UpdateAttributes.Name = "UpdateAttributes";
            this.UpdateAttributes.Size = new Size(0x138, 0x20);
            this.UpdateAttributes.TabIndex = 10;
            this.UpdateAttributes.Text = "Update Attributes";
            this.UpdateAttributes.Click += new EventHandler(this.UpdateAttributes_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x130, 0x115);
            base.Controls.AddRange(new Control[] { this.UpdateAttributes, this.AttributesGrid });
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AttributesForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Manage Attributes";
            base.Load += new EventHandler(this.AttributesForm_Load);
            this.AttributesGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void UpdateAttributes_Click(object sender, EventArgs e)
        {
            try
            {
                this.attributeSchemaXml = this.attributeSet.GetXml();
                string queryToRun = "UPDATE " + srcTable + " SET " + srcColumn + " ='" + this.attributeSchemaXml + "' " + whereQueryPartOfUpdate;
                Exception exception = BusinessLogic.ModifyStoreHouse(Con, queryToRun, null);
                if (exception != null)
                {
                    throw exception;
                }
            }
            catch (Exception exception2)
            {
                BusinessLogic.MyMessageBox(exception2.Message);
                BusinessLogic.MyMessageBox("Update failed!");
                return;
            }
            BusinessLogic.MyMessageBox("Attributes Updated");
        }
    }
}

