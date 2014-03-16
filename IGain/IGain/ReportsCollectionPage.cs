namespace IGain
{
    using PGBusinessLogic;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;
    using System.Xml;

    public class ReportsCollectionPage : Form
    {
        private IContainer components;
        public static OleDbConnection Con = null;
        private ImageList reportsImgList;
        private TreeView ReportsNavigation;

        public ReportsCollectionPage()
        {
            this.InitializeComponent();
            if (Con == null)
            {
                this.ReportsNavigation.Enabled = false;
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
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(ReportsCollectionPage));
            this.reportsImgList = new ImageList(this.components);
            this.ReportsNavigation = new TreeView();
            base.SuspendLayout();
            this.reportsImgList.ColorDepth = ColorDepth.Depth8Bit;
            this.reportsImgList.ImageSize = new Size(0x18, 0x18);
            this.reportsImgList.ImageStream = (ImageListStreamer) manager.GetObject("reportsImgList.ImageStream");
            this.reportsImgList.TransparentColor = Color.Transparent;
            this.ReportsNavigation.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.ReportsNavigation.FullRowSelect = true;
            this.ReportsNavigation.ImageList = this.reportsImgList;
            this.ReportsNavigation.Name = "ReportsNavigation";
            this.ReportsNavigation.Size = new Size(0x2b8, 0x288);
            this.ReportsNavigation.TabIndex = 0;
            this.ReportsNavigation.DoubleClick += new EventHandler(this.ReportsNavigation_DoubleClick);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2b0, 0x26d);
            base.Controls.AddRange(new Control[] { this.ReportsNavigation });
            base.MaximizeBox = false;
            base.Name = "ReportsCollectionPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Reports";
            base.Load += new EventHandler(this.ReportsCollectionPage_Load);
            base.ResumeLayout(false);
        }

        private void ReportsCollectionPage_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(@".\ReportDefs\ReportCollection.xml");
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    bool flag = false;
                    foreach (TreeNode node2 in this.ReportsNavigation.Nodes)
                    {
                        if (node2.Text == node.Attributes["Group"].Value)
                        {
                            flag = true;
                            node2.Nodes.Add(node.Attributes["HelpString"].Value).Tag = node.Attributes["Name"].Value;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        this.ReportsNavigation.Nodes.Add(node.Attributes["Group"].Value).Nodes.Add(node.Attributes["HelpString"].Value).Tag = node.Attributes["Name"].Value;
                    }
                }
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
            }
        }

        private void ReportsNavigation_DoubleClick(object sender, EventArgs e)
        {
            if (this.ReportsNavigation.SelectedNode.Parent != null)
            {
                FileInfo info = new FileInfo(@".\ReportDefs\" + this.ReportsNavigation.SelectedNode.Tag + ".xml");
                if (!info.Exists)
                {
                    BusinessLogic.MyMessageBox("The file for this report does not exist!");
                }
                else
                {
                    if (ReportingPage.Con == null)
                    {
                        ReportingPage.Con = Con;
                    }
                    new ReportingPage(Convert.ToString(this.ReportsNavigation.SelectedNode.Tag)).ShowDialog(this);
                }
            }
        }
    }
}

