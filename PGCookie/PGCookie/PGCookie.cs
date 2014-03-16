namespace PGCookie
{
    using PGBusinessLogic;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class PGCookie : UserControl
    {
        public ListBox CacheList;
        private Container components = null;
        public TextBox RecField;
        private string SelCol;
        private DataTable srcDT = new DataTable();

        public PGCookie()
        {
            this.InitializeComponent();
        }

        private void CacheList_DoubleClick(object sender, EventArgs e)
        {
            this.RecField.Text = (string) this.CacheList.SelectedItem;
            this.CacheList.Visible = false;
        }

        private void CacheList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.RecField.Text = (string) this.CacheList.SelectedItem;
                this.CacheList.Visible = false;
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
            this.RecField = new TextBox();
            this.CacheList = new ListBox();
            base.SuspendLayout();
            this.RecField.Location = new Point(8, 8);
            this.RecField.Name = "RecField";
            this.RecField.Size = new Size(200, 20);
            this.RecField.TabIndex = 0;
            this.RecField.Text = "";
            this.RecField.KeyDown += new KeyEventHandler(this.RecField_KeyDown);
            this.RecField.TextChanged += new EventHandler(this.RecField_TextChanged);
            this.CacheList.HorizontalScrollbar = true;
            this.CacheList.Location = new Point(8, 0x20);
            this.CacheList.Name = "CacheList";
            this.CacheList.Size = new Size(200, 0xad);
            this.CacheList.Sorted = true;
            this.CacheList.TabIndex = 1;
            this.CacheList.TabStop = false;
            this.CacheList.UseTabStops = false;
            this.CacheList.Visible = false;
            this.CacheList.KeyPress += new KeyPressEventHandler(this.CacheList_KeyPress);
            this.CacheList.DoubleClick += new EventHandler(this.CacheList_DoubleClick);
            base.Controls.AddRange(new Control[] { this.CacheList, this.RecField });
            base.Name = "PGCookie";
            base.Size = new Size(0xd8, 0xd8);
            base.Resize += new EventHandler(this.PGCookie_Resize);
            base.ResumeLayout(false);
        }

        private void PGCookie_Resize(object sender, EventArgs e)
        {
            Exception exception;
            try
            {
                this.RecField.Width = base.Width - 20;
                this.CacheList.Width = base.Width - 20;
            }
            catch (Exception exception1)
            {
                exception = exception1;
            }
            try
            {
                this.CacheList.Height = base.Height - 15;
            }
            catch (Exception exception2)
            {
                exception = exception2;
            }
        }

        private void RecField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.CacheList.Visible = false;
            }
            else if (e.KeyValue == 9)
            {
                this.CacheList.Visible = false;
            }
            else if ((e.KeyValue == 40) && this.CacheList.Visible)
            {
                try
                {
                    this.CacheList.Focus();
                    this.CacheList.Select();
                    this.CacheList.SelectedIndex = 0;
                }
                catch (Exception exception)
                {
                    BusinessLogic.MyMessageBox(exception.Message);
                }
            }
        }

        private void RecField_TextChanged(object sender, EventArgs e)
        {
            if (this.srcDT != null)
            {
                this.CacheList.Items.Clear();
                this.CacheList.Visible = true;
                foreach (DataRow row in this.srcDT.Rows)
                {
                    if (row[this.SelCol].ToString().Length > 0)
                    {
                        if (this.RecField.Text.Length > 0)
                        {
                            if (row[this.SelCol].ToString().ToUpper().StartsWith(this.RecField.Text.ToUpper()))
                            {
                                this.CacheList.Items.Add(row[this.SelCol]);
                            }
                        }
                        else
                        {
                            this.CacheList.Items.Add(row[this.SelCol]);
                        }
                    }
                }
            }
        }

        [Browsable(true)]
        public string BoundColumn
        {
            get
            {
                return this.SelCol;
            }
            set
            {
                this.SelCol = value;
            }
        }

        [Browsable(false)]
        public DataTable SourceDataTable
        {
            get
            {
                return this.srcDT;
            }
            set
            {
                this.srcDT = value;
            }
        }

        [Browsable(true)]
        public string TextBoxText
        {
            get
            {
                return this.RecField.Text;
            }
            set
            {
                this.RecField.Text = value;
            }
        }
    }
}

