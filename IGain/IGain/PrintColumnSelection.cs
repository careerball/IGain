namespace IGain
{
    using AxSHDocVw;
    using PGBusinessLogic;
    using SHDocVw;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    public class PrintColumnSelection : Form
    {
        public ListBox AllColumnsList;
        private Container components = null;
        private DataSet dsSrc;
        private Button EditPrint;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private object missing;
        private object objZoomValue;
        private Button PPreview;
        public ListBox PrintTemplateList;
        public ListBox SelectedColumnList;
        private AxWebBrowser templateBrowser;

        public PrintColumnSelection(DataSet paramDS)
        {
            this.InitializeComponent();
            this.objZoomValue = 1;
            this.missing = System.Type.Missing;
            try
            {
                DirectoryInfo info = new DirectoryInfo(@".\Print Templates");
                if (info == null)
                {
                    throw new Exception("The print-template directory is not accessible!");
                }
                foreach (FileInfo info2 in info.GetFiles("*.html"))
                {
                    this.PrintTemplateList.Items.Add(info2.Name);
                }
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("There was an error enumerating print templates!");
            }
            if (paramDS != null)
            {
                this.dsSrc = paramDS;
            }
            else
            {
                BusinessLogic.MyMessageBox("Error retrieving data source!");
                foreach (Control control in base.Controls)
                {
                    control.Enabled = false;
                }
            }
        }

        private void AllColumnsList_DoubleClick(object sender, EventArgs e)
        {
            this.SelectedColumnList.Items.Add(this.AllColumnsList.SelectedItem.ToString());
        }

        private string ApplyTemplate(XmlDocument doc)
        {
            if (this.templateBrowser.Document != null)
            {
                object target = this.templateBrowser.Document.GetType().InvokeMember("body", BindingFlags.GetProperty, null, this.templateBrowser.Document, null);
                if (target != null)
                {
                    string str = Convert.ToString(target.GetType().InvokeMember("outerHTML", BindingFlags.GetProperty, null, target, null));
                    if (str != null)
                    {
                        string oldValue = new AppSettingsReader().GetValue("ZoomValue", typeof(string)).ToString();
                        if (oldValue != null)
                        {
                            str = str.Replace(oldValue, "100%");
                        }
                        int index = str.ToUpper().IndexOf("<DATA>");
                        int num2 = str.ToUpper().IndexOf("</DATA>");
                        if ((index < 1) || (num2 < 1))
                        {
                            return doc.OuterXml;
                        }
                        string str3 = str.Substring(index, (num2 - index) + 7);
                        return str.Replace(str3, doc.OuterXml);
                    }
                }
            }
            return doc.OuterXml;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.templateBrowser.Dispose();
                Marshal.ReleaseComObject(this.templateBrowser);
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditPrint_Click(object sender, EventArgs e)
        {
            if (this.PrintTemplateList.SelectedItem == null)
            {
                BusinessLogic.MyMessageBox("Please first select a template for printing from the [Choose Template] section");
            }
            else
            {
                TextWriter writer = null;
                try
                {
                    if (this.SelectedColumnList.Items.Count < 1)
                    {
                        throw new Exception("No columns selected!");
                    }
                    XmlDocument doc = new XmlDocument();
                    int num = 100 / (this.SelectedColumnList.Items.Count + 1);
                    doc.LoadXml("<center><table border=\"1\" width=\"" + Convert.ToString((int) (num * this.SelectedColumnList.Items.Count)) + "%\"></table></center>");
                    XmlNode node = doc.DocumentElement.FirstChild.AppendChild(doc.CreateElement("tr"));
                    foreach (object obj2 in this.SelectedColumnList.Items)
                    {
                        XmlNode node2 = node.AppendChild(doc.CreateElement("td"));
                        XmlNode node3 = doc.CreateNode(XmlNodeType.Attribute, "width", null);
                        node3.Value = Convert.ToString(num) + "%";
                        node2.Attributes.SetNamedItem(node3);
                        node2.AppendChild(doc.CreateElement("b")).InnerText = Convert.ToString(obj2);
                    }
                    foreach (DataRow row in this.dsSrc.Tables[0].Rows)
                    {
                        XmlNode node5 = doc.DocumentElement.FirstChild.AppendChild(doc.CreateElement("tr"));
                        foreach (object obj2 in this.SelectedColumnList.Items)
                        {
                            XmlNode node6 = node5.AppendChild(doc.CreateElement("td"));
                            XmlNode node7 = doc.CreateNode(XmlNodeType.Attribute, "width", null);
                            node7.Value = Convert.ToString(num) + "%";
                            node6.Attributes.SetNamedItem(node7);
                            node6.InnerText = Convert.ToString(row[Convert.ToString(obj2)]);
                        }
                    }
                    string str2 = this.ApplyTemplate(doc);
                    doc.RemoveAll();
                    writer = new StreamWriter(@".\PrintCache\PrintDocument.html", false, Encoding.ASCII);
                    writer.Write(str2);
                    writer.Close();
                    writer = null;
                    Process.Start("iexplore.exe", "\"" + Application.StartupPath + "\\PrintCache\\PrintDocument.html\"");
                }
                catch (Exception exception)
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer = null;
                    }
                    BusinessLogic.MyMessageBox(exception.Message);
                    BusinessLogic.MyMessageBox("Document for printing could not be formed!");
                }
            }
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(PrintColumnSelection));
            this.label1 = new Label();
            this.AllColumnsList = new ListBox();
            this.SelectedColumnList = new ListBox();
            this.label2 = new Label();
            this.PPreview = new Button();
            this.EditPrint = new Button();
            this.PrintTemplateList = new ListBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.templateBrowser = new AxWebBrowser();
            this.templateBrowser.BeginInit();
            base.SuspendLayout();
            this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(120, 0x18);
            this.label1.TabIndex = 0;
            this.label1.Text = "         All Columns";
            this.AllColumnsList.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.AllColumnsList.ItemHeight = 14;
            this.AllColumnsList.Location = new Point(8, 0x20);
            this.AllColumnsList.Name = "AllColumnsList";
            this.AllColumnsList.Size = new Size(0x88, 0x1a8);
            this.AllColumnsList.TabIndex = 1;
            this.AllColumnsList.DoubleClick += new EventHandler(this.AllColumnsList_DoubleClick);
            this.SelectedColumnList.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.SelectedColumnList.ItemHeight = 14;
            this.SelectedColumnList.Location = new Point(0x98, 0x20);
            this.SelectedColumnList.Name = "SelectedColumnList";
            this.SelectedColumnList.Size = new Size(0x88, 0x1a8);
            this.SelectedColumnList.TabIndex = 3;
            this.SelectedColumnList.DoubleClick += new EventHandler(this.SelectedColumnList_DoubleClick);
            this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(160, 8);
            this.label2.Name = "label2";
            this.label2.Size = new Size(120, 0x18);
            this.label2.TabIndex = 2;
            this.label2.Text = "   Selected Columns";
            this.PPreview.Location = new Point(0x98, 0x1d0);
            this.PPreview.Name = "PPreview";
            this.PPreview.Size = new Size(0x90, 0x20);
            this.PPreview.TabIndex = 4;
            this.PPreview.Text = "Print Preview..";
            this.PPreview.Click += new EventHandler(this.PPreview_Click);
            this.EditPrint.Location = new Point(8, 0x1d0);
            this.EditPrint.Name = "EditPrint";
            this.EditPrint.Size = new Size(0x88, 0x20);
            this.EditPrint.TabIndex = 5;
            this.EditPrint.Text = "Preview";
            this.EditPrint.Click += new EventHandler(this.EditPrint_Click);
            this.PrintTemplateList.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.PrintTemplateList.ItemHeight = 14;
            this.PrintTemplateList.Location = new Point(0x300, 0x20);
            this.PrintTemplateList.Name = "PrintTemplateList";
            this.PrintTemplateList.Size = new Size(120, 0x1a8);
            this.PrintTemplateList.TabIndex = 6;
            this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold);
            this.label3.Location = new Point(0x300, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x88, 0x10);
            this.label3.TabIndex = 7;
            this.label3.Text = "Choose Template..";
            this.label4.Font = new Font("Arial", 8.25f, FontStyle.Bold);
            this.label4.Location = new Point(320, 8);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x98, 0x10);
            this.label4.TabIndex = 9;
            this.label4.Text = "Print template preview";
            this.templateBrowser.Enabled = true;
            this.templateBrowser.Location = new Point(0x130, 0x20);
            this.templateBrowser.OcxState = (AxHost.State) manager.GetObject("templateBrowser.OcxState");
            this.templateBrowser.Size = new Size(0x1c8, 0x1a8);
            this.templateBrowser.TabIndex = 10;
            this.templateBrowser.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.templateBrowser_DocumentComplete);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x388, 0x1f5);
            base.Controls.Add(this.templateBrowser);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.PrintTemplateList);
            base.Controls.Add(this.EditPrint);
            base.Controls.Add(this.PPreview);
            base.Controls.Add(this.SelectedColumnList);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.AllColumnsList);
            base.Controls.Add(this.label1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PrintColumnSelection";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Select Columns";
            base.Load += new EventHandler(this.PrintColumnSelection_Load);
            this.templateBrowser.EndInit();
            base.ResumeLayout(false);
        }

        private void PPreview_Click(object sender, EventArgs e)
        {
            if (this.PrintTemplateList.SelectedItem == null)
            {
                BusinessLogic.MyMessageBox("Please first select a template for printing from the [Choose Template] section");
            }
            else
            {
                TextWriter writer = null;
                try
                {
                    if (this.SelectedColumnList.Items.Count < 1)
                    {
                        throw new Exception("No columns selected!");
                    }
                    XmlDocument doc = new XmlDocument();
                    int num = 100 / (this.SelectedColumnList.Items.Count + 1);
                    doc.LoadXml("<center><table border=\"1\" width=\"" + Convert.ToString((int) (num * this.SelectedColumnList.Items.Count)) + "%\"></table></center>");
                    XmlNode node = doc.DocumentElement.FirstChild.AppendChild(doc.CreateElement("tr"));
                    foreach (object obj2 in this.SelectedColumnList.Items)
                    {
                        XmlNode node2 = node.AppendChild(doc.CreateElement("td"));
                        XmlNode node3 = doc.CreateNode(XmlNodeType.Attribute, "width", null);
                        node3.Value = Convert.ToString(num) + "%";
                        node2.Attributes.SetNamedItem(node3);
                        node2.AppendChild(doc.CreateElement("b")).InnerText = Convert.ToString(obj2);
                    }
                    foreach (DataRow row in this.dsSrc.Tables[0].Rows)
                    {
                        XmlNode node5 = doc.DocumentElement.FirstChild.AppendChild(doc.CreateElement("tr"));
                        foreach (object obj2 in this.SelectedColumnList.Items)
                        {
                            XmlNode node6 = node5.AppendChild(doc.CreateElement("td"));
                            XmlNode node7 = doc.CreateNode(XmlNodeType.Attribute, "width", null);
                            node7.Value = Convert.ToString(num) + "%";
                            node6.Attributes.SetNamedItem(node7);
                            node6.InnerText = Convert.ToString(row[Convert.ToString(obj2)]);
                        }
                    }
                    string str2 = this.ApplyTemplate(doc);
                    doc.RemoveAll();
                    writer = new StreamWriter(@".\PrintCache\PrintDocument.html", false, Encoding.ASCII);
                    writer.Write(str2);
                    writer.WriteLine("<script language=\"javascript\">");
                    writer.WriteLine("\tvar WebBrowser='<OBJECT ID=WebBrowser1 WIDTH=0 HEIGHT=0 CLASSID=CLSID:8856F961-340A-11D0-A96B-00C04FD705A2></OBJECT>';");
                    writer.WriteLine("\tdocument.write(WebBrowser);");
                    writer.WriteLine("\tWebBrowser1.ExecWB(7,0);");
                    writer.WriteLine("</script>");
                    writer.Close();
                    writer = null;
                    Process.Start("iexplore.exe", "\"" + Application.StartupPath + "\\PrintCache\\PrintDocument.html\"");
                }
                catch (Exception exception)
                {
                    if (writer != null)
                    {
                        writer.Close();
                        writer = null;
                    }
                    BusinessLogic.MyMessageBox(exception.Message);
                    BusinessLogic.MyMessageBox("Document for printing could not be formed!");
                }
            }
        }

        private void PrintColumnSelection_Load(object sender, EventArgs e)
        {
            this.PrintTemplateList.SelectedIndexChanged += new EventHandler(this.PrintTemplateList_SelectedIndexChanged);
        }

        private void PrintTemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            object missing = this.missing;
            this.templateBrowser.Navigate(Application.StartupPath + @"\Print Templates\" + Convert.ToString(this.PrintTemplateList.SelectedItem), ref this.missing, ref missing, ref this.missing, ref this.missing);
        }

        private void SelectedColumnList_DoubleClick(object sender, EventArgs e)
        {
            this.SelectedColumnList.Items.RemoveAt(this.SelectedColumnList.SelectedIndex);
        }

        private void templateBrowser_DocumentComplete(object sender, DWebBrowserEvents2_DocumentCompleteEvent e)
        {
            try
            {
                if (this.templateBrowser.Document != null)
                {
                    object target = this.templateBrowser.Document.GetType().InvokeMember("body", BindingFlags.GetProperty, null, this.templateBrowser.Document, null);
                    if (target != null)
                    {
                        object obj3 = target.GetType().InvokeMember("style", BindingFlags.GetProperty, null, target, null);
                        if (obj3 != null)
                        {
                            AppSettingsReader reader = new AppSettingsReader();
                            object[] args = new object[] { Convert.ToString(reader.GetValue("ZoomValue", typeof(string))) };
                            obj3.GetType().InvokeMember("zoom", BindingFlags.SetProperty, null, obj3, args);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("Error in zooming! setting to default zoom.");
                if (Convert.ToUInt32(this.objZoomValue) == 1)
                {
                    try
                    {
                        this.templateBrowser.ExecWB(SHDocVw.OLECMDID.OLECMDID_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref this.objZoomValue, ref this.missing);
                    }
                    catch (Exception)
                    {
                    }
                    this.objZoomValue = 0;
                }
            }
        }
    }
}

