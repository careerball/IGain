namespace IGain
{
    using PGBusinessLogic;
    using System;
    using System.ComponentModel;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class CreditReissuePage : Form
    {
        public TextBox BeginDate;
        public TextBox CarriedForwardInterest;
        public TextBox CarriedPrinciple;
        private Container components = null;
        public static OleDbConnection Con = null;
        public ComboBox Creditee;
        public string CreditIdToBeClosed;
        private ComboBox InterestType1;
        private ComboBox InterestType2;
        private ComboBox InterestType3;
        public CheckBox IsSlipIssued;
        private Button IssueCredit;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button MoveCFI;
        private Button MoveCFP;
        private ComboBox Per1;
        private ComboBox Per2;
        private ComboBox Per3;
        private TextBox Principle;
        private TextBox Rate1;
        private TextBox Rate2;
        private TextBox Rate3;
        public TextBox SlipNumber;
        private Label Term1;
        private Label Term2;
        private ComboBox TermDuration1;
        private ComboBox TermDuration2;
        private ComboBox TermDuration3;
        private CheckBox ThereAfter1;
        private CheckBox ThereAfter2;

        public CreditReissuePage()
        {
            this.InitializeComponent();
        }

        private void BeginDate_Enter(object sender, EventArgs e)
        {
            try
            {
                this.BeginDate.Text = DateTime.Parse(this.BeginDate.Text).ToLongDateString();
            }
            catch (Exception)
            {
            }
        }

        private void BeginDate_Leave(object sender, EventArgs e)
        {
            try
            {
                this.BeginDate.Text = DateTime.Parse(this.BeginDate.Text).ToShortDateString();
            }
            catch (Exception)
            {
                this.BeginDate.Text = DateTime.Today.ToLongDateString();
                this.BeginDate.Focus();
                BusinessLogic.MyMessageBox("Please enter a valid date format \n e.g. 1 Jan,2006");
            }
        }

        private void CarriedForwardInterest_Leave(object sender, EventArgs e)
        {
            try
            {
                this.CarriedForwardInterest.Text = Convert.ToString(Convert.ToDouble(this.CarriedForwardInterest.Text));
            }
            catch (Exception)
            {
                this.CarriedForwardInterest.Text = "";
                BusinessLogic.MyMessageBox("Please enter a numeric value only");
                this.CarriedForwardInterest.Focus();
            }
        }

        private void CarriedPrinciple_Leave(object sender, EventArgs e)
        {
            try
            {
                double num = Convert.ToDouble(this.CarriedPrinciple.Text);
            }
            catch (Exception)
            {
                this.CarriedPrinciple.Text = "";
                this.CarriedPrinciple.Focus();
                BusinessLogic.MyMessageBox("Please enter a numeric value only!");
            }
        }

        private void CreditPage_Load(object sender, EventArgs e)
        {
            if (Con == null)
            {
                BusinessLogic.MyMessageBox("Connection is null.Can not proceed!", "Error!");
                this.IssueCredit.Enabled = false;
            }
            else
            {
                if (this.Creditee.Items.Count > 0)
                {
                    this.Creditee.SelectedIndex = 0;
                }
                this.Per1.SelectedIndex = 0;
                this.InterestType1.SelectedIndex = 0;
                this.Rate1.Text = "12";
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
            ResourceManager manager = new ResourceManager(typeof(CreditReissuePage));
            this.label1 = new Label();
            this.label2 = new Label();
            this.Principle = new TextBox();
            this.label5 = new Label();
            this.Per1 = new ComboBox();
            this.label4 = new Label();
            this.Rate1 = new TextBox();
            this.label3 = new Label();
            this.InterestType1 = new ComboBox();
            this.label7 = new Label();
            this.Term1 = new Label();
            this.ThereAfter1 = new CheckBox();
            this.TermDuration1 = new ComboBox();
            this.TermDuration2 = new ComboBox();
            this.ThereAfter2 = new CheckBox();
            this.Term2 = new Label();
            this.label10 = new Label();
            this.InterestType2 = new ComboBox();
            this.label11 = new Label();
            this.Per2 = new ComboBox();
            this.label12 = new Label();
            this.Rate2 = new TextBox();
            this.label13 = new Label();
            this.TermDuration3 = new ComboBox();
            this.label15 = new Label();
            this.InterestType3 = new ComboBox();
            this.label16 = new Label();
            this.Per3 = new ComboBox();
            this.label17 = new Label();
            this.Rate3 = new TextBox();
            this.label18 = new Label();
            this.IsSlipIssued = new CheckBox();
            this.SlipNumber = new TextBox();
            this.label8 = new Label();
            this.BeginDate = new TextBox();
            this.IssueCredit = new Button();
            this.Creditee = new ComboBox();
            this.label9 = new Label();
            this.CarriedForwardInterest = new TextBox();
            this.label6 = new Label();
            this.CarriedPrinciple = new TextBox();
            this.label14 = new Label();
            this.MoveCFI = new Button();
            this.MoveCFP = new Button();
            base.SuspendLayout();
            this.label1.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(8, 0x98);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Issue";
            this.label2.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0xb0, 0x98);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x70, 0x18);
            this.label2.TabIndex = 2;
            this.label2.Text = "an amount of Rs.";
            this.Principle.Location = new Point(0x128, 0x98);
            this.Principle.Name = "Principle";
            this.Principle.Size = new Size(0x60, 20);
            this.Principle.TabIndex = 2;
            this.Principle.Text = "0";
            this.Principle.Leave += new EventHandler(this.Principle_Leave);
            this.label5.Font = new Font("Arial", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0x18, 0x18);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0xf8, 0x20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Credit Re-Issue Form :";
            this.Per1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Per1.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Per1.Items.AddRange(new object[] { "Year", "Month" });
            this.Per1.Location = new Point(0x90, 0xd0);
            this.Per1.Name = "Per1";
            this.Per1.Size = new Size(0x48, 0x17);
            this.Per1.TabIndex = 5;
            this.Per1.SelectedIndexChanged += new EventHandler(this.Per1_SelectedIndexChanged);
            this.label4.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x68, 0xd0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(40, 0x17);
            this.label4.TabIndex = 11;
            this.label4.Text = "% per";
            this.Rate1.Location = new Point(0x38, 0xd0);
            this.Rate1.Name = "Rate1";
            this.Rate1.Size = new Size(40, 20);
            this.Rate1.TabIndex = 4;
            this.Rate1.Text = "";
            this.Rate1.Leave += new EventHandler(this.Rate_Leave);
            this.label3.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x20, 0xd0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x10, 0x10);
            this.label3.TabIndex = 9;
            this.label3.Text = "@";
            this.InterestType1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.InterestType1.Items.AddRange(new object[] { "Simple Interest", "Compound Interest" });
            this.InterestType1.Location = new Point(0x108, 0xd0);
            this.InterestType1.Name = "InterestType1";
            this.InterestType1.Size = new Size(0x80, 0x15);
            this.InterestType1.TabIndex = 6;
            this.label7.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label7.Location = new Point(0xe8, 0xd0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x18, 0x10);
            this.label7.TabIndex = 15;
            this.label7.Text = "on";
            this.Term1.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Term1.Location = new Point(0x220, 0xd0);
            this.Term1.Name = "Term1";
            this.Term1.Size = new Size(0x30, 0x17);
            this.Term1.TabIndex = 0x11;
            this.ThereAfter1.Enabled = false;
            this.ThereAfter1.Location = new Point(600, 0xd0);
            this.ThereAfter1.Name = "ThereAfter1";
            this.ThereAfter1.Size = new Size(80, 0x18);
            this.ThereAfter1.TabIndex = 8;
            this.ThereAfter1.Text = "Thereafter";
            this.ThereAfter1.CheckedChanged += new EventHandler(this.ThereAfter1_CheckedChanged);
            this.TermDuration1.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TermDuration1.Items.AddRange(new object[] { "Ever" });
            this.TermDuration1.Location = new Point(440, 0xd0);
            this.TermDuration1.Name = "TermDuration1";
            this.TermDuration1.Size = new Size(0x60, 0x17);
            this.TermDuration1.TabIndex = 7;
            this.TermDuration1.Leave += new EventHandler(this.TermDuration1_Leave);
            this.TermDuration1.Enter += new EventHandler(this.TermDuration1_Enter);
            this.TermDuration2.Enabled = false;
            this.TermDuration2.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TermDuration2.Items.AddRange(new object[] { "Ever" });
            this.TermDuration2.Location = new Point(440, 0x110);
            this.TermDuration2.Name = "TermDuration2";
            this.TermDuration2.Size = new Size(0x60, 0x17);
            this.TermDuration2.TabIndex = 12;
            this.TermDuration2.Leave += new EventHandler(this.TermDuration2_Leave);
            this.TermDuration2.Enter += new EventHandler(this.TermDuration2_Enter);
            this.ThereAfter2.Enabled = false;
            this.ThereAfter2.Location = new Point(600, 0x110);
            this.ThereAfter2.Name = "ThereAfter2";
            this.ThereAfter2.Size = new Size(80, 0x18);
            this.ThereAfter2.TabIndex = 13;
            this.ThereAfter2.Text = "Thereafter";
            this.ThereAfter2.CheckedChanged += new EventHandler(this.ThereAfter2_CheckedChanged);
            this.Term2.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Term2.Location = new Point(0x220, 0x110);
            this.Term2.Name = "Term2";
            this.Term2.Size = new Size(0x30, 0x17);
            this.Term2.TabIndex = 0x1b;
            this.label10.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label10.Location = new Point(240, 0x110);
            this.label10.Name = "label10";
            this.label10.Size = new Size(20, 0x10);
            this.label10.TabIndex = 0x1a;
            this.label10.Text = "on";
            this.InterestType2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.InterestType2.Enabled = false;
            this.InterestType2.Items.AddRange(new object[] { "Simple Interest", "Compound Interest" });
            this.InterestType2.Location = new Point(0x108, 0x110);
            this.InterestType2.Name = "InterestType2";
            this.InterestType2.Size = new Size(0x80, 0x15);
            this.InterestType2.TabIndex = 11;
            this.label11.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label11.Location = new Point(0x188, 0x110);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x30, 0x10);
            this.label11.TabIndex = 0x18;
            this.label11.Text = " For";
            this.Per2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Per2.Enabled = false;
            this.Per2.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Per2.Items.AddRange(new object[] { "Year", "Month" });
            this.Per2.Location = new Point(0x98, 0x110);
            this.Per2.Name = "Per2";
            this.Per2.Size = new Size(0x44, 0x17);
            this.Per2.TabIndex = 10;
            this.Per2.SelectedIndexChanged += new EventHandler(this.Per2_SelectedIndexChanged);
            this.label12.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label12.Location = new Point(0x68, 0x110);
            this.label12.Name = "label12";
            this.label12.Size = new Size(40, 0x17);
            this.label12.TabIndex = 0x16;
            this.label12.Text = "% per";
            this.Rate2.Enabled = false;
            this.Rate2.Location = new Point(0x38, 0x110);
            this.Rate2.Name = "Rate2";
            this.Rate2.Size = new Size(40, 20);
            this.Rate2.TabIndex = 9;
            this.Rate2.Text = "";
            this.Rate2.Leave += new EventHandler(this.Rate_Leave);
            this.label13.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label13.Location = new Point(0x20, 0x110);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x10, 0x10);
            this.label13.TabIndex = 20;
            this.label13.Text = "@";
            this.TermDuration3.DropDownStyle = ComboBoxStyle.DropDownList;
            this.TermDuration3.Enabled = false;
            this.TermDuration3.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.TermDuration3.Items.AddRange(new object[] { "Ever" });
            this.TermDuration3.Location = new Point(0x1b0, 0x158);
            this.TermDuration3.Name = "TermDuration3";
            this.TermDuration3.Size = new Size(0x60, 0x17);
            this.TermDuration3.TabIndex = 0x11;
            this.label15.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label15.Location = new Point(240, 0x158);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x18, 0x10);
            this.label15.TabIndex = 0x24;
            this.label15.Text = "on";
            this.InterestType3.DropDownStyle = ComboBoxStyle.DropDownList;
            this.InterestType3.Enabled = false;
            this.InterestType3.Items.AddRange(new object[] { "Simple Interest", "Compound Interest" });
            this.InterestType3.Location = new Point(0x108, 0x158);
            this.InterestType3.Name = "InterestType3";
            this.InterestType3.Size = new Size(0x80, 0x15);
            this.InterestType3.TabIndex = 0x10;
            this.label16.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label16.Location = new Point(0x188, 0x158);
            this.label16.Name = "label16";
            this.label16.Size = new Size(40, 0x10);
            this.label16.TabIndex = 0x22;
            this.label16.Text = " For";
            this.Per3.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Per3.Enabled = false;
            this.Per3.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Per3.Items.AddRange(new object[] { "Year", "Month" });
            this.Per3.Location = new Point(0x98, 0x158);
            this.Per3.Name = "Per3";
            this.Per3.Size = new Size(0x48, 0x17);
            this.Per3.TabIndex = 15;
            this.Per3.SelectedIndexChanged += new EventHandler(this.Per3_SelectedIndexChanged);
            this.label17.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label17.Location = new Point(0x70, 0x158);
            this.label17.Name = "label17";
            this.label17.Size = new Size(40, 0x17);
            this.label17.TabIndex = 0x20;
            this.label17.Text = "% per";
            this.Rate3.Enabled = false;
            this.Rate3.Location = new Point(0x38, 0x158);
            this.Rate3.Name = "Rate3";
            this.Rate3.Size = new Size(40, 20);
            this.Rate3.TabIndex = 14;
            this.Rate3.Text = "";
            this.Rate3.Leave += new EventHandler(this.Rate_Leave);
            this.label18.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label18.Location = new Point(0x20, 0x158);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x10, 0x10);
            this.label18.TabIndex = 30;
            this.label18.Text = "@";
            this.IsSlipIssued.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.IsSlipIssued.Location = new Point(0x20, 0x1a8);
            this.IsSlipIssued.Name = "IsSlipIssued";
            this.IsSlipIssued.Size = new Size(0x60, 0x18);
            this.IsSlipIssued.TabIndex = 0x12;
            this.IsSlipIssued.Text = "Issue Slip";
            this.IsSlipIssued.CheckedChanged += new EventHandler(this.IsSlipIssued_CheckedChanged);
            this.SlipNumber.Location = new Point(160, 0x1a8);
            this.SlipNumber.Name = "SlipNumber";
            this.SlipNumber.Size = new Size(0xa8, 20);
            this.SlipNumber.TabIndex = 0x13;
            this.SlipNumber.Text = "Enter Slip Number here";
            this.SlipNumber.Visible = false;
            this.label8.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label8.Location = new Point(0x198, 0x98);
            this.label8.Name = "label8";
            this.label8.Size = new Size(40, 0x18);
            this.label8.TabIndex = 0x2a;
            this.label8.Text = "as on";
            this.BeginDate.Location = new Point(0x1c8, 0x98);
            this.BeginDate.Name = "BeginDate";
            this.BeginDate.Size = new Size(0x90, 20);
            this.BeginDate.TabIndex = 3;
            this.BeginDate.Text = "";
            this.BeginDate.Leave += new EventHandler(this.BeginDate_Leave);
            this.BeginDate.Enter += new EventHandler(this.BeginDate_Enter);
            this.IssueCredit.Font = new Font("Arial", 11.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.IssueCredit.Location = new Point(0x1a0, 0x1a8);
            this.IssueCredit.Name = "IssueCredit";
            this.IssueCredit.Size = new Size(0xb8, 0x20);
            this.IssueCredit.TabIndex = 20;
            this.IssueCredit.Text = "Issue";
            this.IssueCredit.Click += new EventHandler(this.IssueCredit_Click);
            this.Creditee.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Creditee.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Creditee.Location = new Point(0x38, 0x98);
            this.Creditee.Name = "Creditee";
            this.Creditee.Size = new Size(0x70, 0x17);
            this.Creditee.TabIndex = 1;
            this.label9.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label9.Location = new Point(120, 80);
            this.label9.Name = "label9";
            this.label9.Size = new Size(120, 0x10);
            this.label9.TabIndex = 0x2b;
            this.label9.Text = "Carried Forward  :-";
            this.CarriedForwardInterest.Enabled = false;
            this.CarriedForwardInterest.Location = new Point(0x100, 80);
            this.CarriedForwardInterest.Name = "CarriedForwardInterest";
            this.CarriedForwardInterest.Size = new Size(0x48, 20);
            this.CarriedForwardInterest.TabIndex = 0x2c;
            this.CarriedForwardInterest.Text = "0";
            this.CarriedForwardInterest.Leave += new EventHandler(this.CarriedForwardInterest_Leave);
            this.label6.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label6.Location = new Point(0x188, 0xd0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x30, 0x10);
            this.label6.TabIndex = 13;
            this.label6.Text = " For";
            this.CarriedPrinciple.Enabled = false;
            this.CarriedPrinciple.Location = new Point(0x158, 80);
            this.CarriedPrinciple.Name = "CarriedPrinciple";
            this.CarriedPrinciple.Size = new Size(0x48, 20);
            this.CarriedPrinciple.TabIndex = 0x2d;
            this.CarriedPrinciple.Text = "0";
            this.CarriedPrinciple.Leave += new EventHandler(this.CarriedPrinciple_Leave);
            this.label14.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label14.Location = new Point(0x1a8, 80);
            this.label14.Name = "label14";
            this.label14.Size = new Size(120, 0x10);
            this.label14.TabIndex = 0x2e;
            this.label14.Text = "- : Carried Principle";
            this.MoveCFI.Image = (Image) manager.GetObject("MoveCFI.Image");
            this.MoveCFI.Location = new Point(0x130, 0x70);
            this.MoveCFI.Name = "MoveCFI";
            this.MoveCFI.Size = new Size(0x18, 0x20);
            this.MoveCFI.TabIndex = 0x2f;
            this.MoveCFI.Click += new EventHandler(this.MoveCFI_Click);
            this.MoveCFP.Image = (Image) manager.GetObject("MoveCFP.Image");
            this.MoveCFP.Location = new Point(0x158, 0x70);
            this.MoveCFP.Name = "MoveCFP";
            this.MoveCFP.Size = new Size(0x18, 0x20);
            this.MoveCFP.TabIndex = 0x30;
            this.MoveCFP.Click += new EventHandler(this.MoveCFP_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(680, 0x1e5);
            base.Controls.Add(this.MoveCFP);
            base.Controls.Add(this.MoveCFI);
            base.Controls.Add(this.label14);
            base.Controls.Add(this.CarriedPrinciple);
            base.Controls.Add(this.CarriedForwardInterest);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.Creditee);
            base.Controls.Add(this.IssueCredit);
            base.Controls.Add(this.BeginDate);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.SlipNumber);
            base.Controls.Add(this.IsSlipIssued);
            base.Controls.Add(this.TermDuration3);
            base.Controls.Add(this.label15);
            base.Controls.Add(this.InterestType3);
            base.Controls.Add(this.label16);
            base.Controls.Add(this.Per3);
            base.Controls.Add(this.label17);
            base.Controls.Add(this.Rate3);
            base.Controls.Add(this.label18);
            base.Controls.Add(this.TermDuration2);
            base.Controls.Add(this.ThereAfter2);
            base.Controls.Add(this.Term2);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.InterestType2);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.Per2);
            base.Controls.Add(this.label12);
            base.Controls.Add(this.Rate2);
            base.Controls.Add(this.label13);
            base.Controls.Add(this.TermDuration1);
            base.Controls.Add(this.ThereAfter1);
            base.Controls.Add(this.Term1);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.InterestType1);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.Per1);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.Rate1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.Principle);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.MaximizeBox = false;
            base.Name = "CreditReissuePage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Re-Issue Credit";
            base.Load += new EventHandler(this.CreditPage_Load);
            base.ResumeLayout(false);
        }

        private void IsSlipIssued_CheckedChanged(object sender, EventArgs e)
        {
            this.SlipNumber.Visible = this.IsSlipIssued.Checked;
        }

        private void IssueCredit_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateData();
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message, "Error!");
                return;
            }
            if ((this.MoveCFP.Tag != null) || (DialogResult.No != BusinessLogic.MyMessageBox("You have not adjusted previous principle to the new one.\n Are you sure you want to proceed?", "Proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
            {
                this.PerformCreditIssueTrans();
            }
        }

        private void MoveCFI_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.MoveCFI.Tag != null)
                {
                    this.Principle.Text = Convert.ToString((double) (Convert.ToDouble(this.Principle.Text) - Convert.ToDouble(this.MoveCFI.Tag)));
                    this.CarriedForwardInterest.Text = Convert.ToString(this.MoveCFI.Tag);
                    this.MoveCFI.Tag = null;
                }
                else
                {
                    this.MoveCFI.Tag = Convert.ToDouble(this.CarriedForwardInterest.Text);
                    this.CarriedForwardInterest.Text = "0";
                    this.Principle.Text = Convert.ToString((double) (Convert.ToDouble(this.Principle.Text) + Convert.ToDouble(this.MoveCFI.Tag)));
                }
                this.MoveCFI.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                this.MoveCFI.Refresh();
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("An error has occured while adding Carry forward amounts!");
                this.MoveCFI.Enabled = false;
            }
        }

        private void MoveCFP_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.MoveCFP.Tag != null)
                {
                    this.Principle.Text = Convert.ToString((double) (Convert.ToDouble(this.Principle.Text) - Convert.ToDouble(this.MoveCFP.Tag)));
                    this.CarriedPrinciple.Text = Convert.ToString(this.MoveCFP.Tag);
                    this.MoveCFP.Tag = null;
                }
                else
                {
                    this.MoveCFP.Tag = Convert.ToDouble(this.CarriedPrinciple.Text);
                    this.CarriedPrinciple.Text = "0";
                    this.Principle.Text = Convert.ToString((double) (Convert.ToDouble(this.Principle.Text) + Convert.ToDouble(this.MoveCFP.Tag)));
                }
                this.MoveCFP.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                this.MoveCFP.Refresh();
            }
            catch (Exception exception)
            {
                BusinessLogic.MyMessageBox(exception.Message);
                BusinessLogic.MyMessageBox("An error has occured while adding Carry forward amounts!");
                this.MoveCFP.Enabled = false;
            }
        }

        private void Per1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Rate1.Text = "";
            if (this.TermDuration1.Text != "Ever")
            {
                try
                {
                    this.TermDuration1.Text = Convert.ToDateTime(this.TermDuration1.Text).ToShortDateString();
                }
                catch (Exception)
                {
                    try
                    {
                        if ((this.Term1.Text == "Months") && (this.Per1.Text == "Year"))
                        {
                            this.TermDuration1.Text = Convert.ToString((double) (((double) Convert.ToUInt32(this.TermDuration1.Text)) / 12.0));
                        }
                        else if ((this.Term1.Text == "Years") && (this.Per1.Text == "Month"))
                        {
                            this.TermDuration1.Text = Convert.ToString(Convert.ToUInt32((decimal) (Convert.ToDecimal(this.TermDuration1.Text) * 12M)));
                        }
                    }
                    catch (Exception)
                    {
                        BusinessLogic.MyMessageBox("Please Enter only numeric or decimal values");
                        this.TermDuration1.Text = "";
                        this.TermDuration1.Focus();
                    }
                }
            }
            this.Term1.Text = this.Per1.Text + "s";
        }

        private void Per2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Rate2.Text = "";
            if (this.TermDuration2.Text != "Ever")
            {
                try
                {
                    this.TermDuration2.Text = Convert.ToDateTime(this.TermDuration2.Text).ToShortDateString();
                }
                catch (Exception)
                {
                    try
                    {
                        if ((this.Term2.Text == "Months") && (this.Per2.Text == "Year"))
                        {
                            this.TermDuration2.Text = Convert.ToString((double) (((double) Convert.ToUInt32(this.TermDuration2.Text)) / 12.0));
                        }
                        else if ((this.Term2.Text == "Years") && (this.Per2.Text == "Month"))
                        {
                            this.TermDuration2.Text = Convert.ToString(Convert.ToUInt32((decimal) (Convert.ToDecimal(this.TermDuration2.Text) * 12M)));
                        }
                    }
                    catch (Exception)
                    {
                        BusinessLogic.MyMessageBox("Please Enter only numeric or decimal values");
                        this.TermDuration2.Text = "";
                        this.TermDuration2.Focus();
                    }
                }
            }
            this.Term2.Text = this.Per2.Text + "s";
        }

        private void Per3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Rate3.Text = "";
        }

        private void PerformCreditIssueTrans()
        {
            Exception exception2;
            try
            {
                string[] strArray;
                string text;
                string str12;
                int num2;
                DateTime time;
                if (Con == null)
                {
                    throw new Exception("Connection is null.Can not proceed");
                }
                OleDbCommand command = new OleDbCommand("SELECT MAX(transactionid) from transactions", Con);
                string str = null;
                object obj2 = command.ExecuteScalar();
                if (obj2 == DBNull.Value)
                {
                    str = "1";
                }
                else
                {
                    str = Convert.ToString((uint) (Convert.ToUInt32(obj2) + 1));
                }
                string cmdText = "Select AccountID from accounttypes where AccountType='Credit' and AccountSubType='" + this.Creditee.Text + "'";
                string str3 = "Select AccountID from accounttypes where AccountType='Interest' and AccountSubType='" + this.Creditee.Text + "'";
                string str4 = this.IsSlipIssued.Checked ? this.SlipNumber.Text : "";
                OleDbCommand command2 = new OleDbCommand(cmdText, Con);
                string str5 = Convert.ToString(command2.ExecuteScalar());
                command2.CommandText = str3;
                string str6 = Convert.ToString(command2.ExecuteScalar());
                uint num = Convert.ToUInt32(new OleDbCommand("Select Count(*) from Transactions where SequenceOfInterestApplication=0 and AccountCredited=  (Select AccountID from Accounttypes where AccountType='Interest' and AccountSubType='" + this.Creditee.Text + "')", Con).ExecuteScalar());
                if (num == 0)
                {
                    string queryToRun = "INSERT INTO Transactions (SequenceOfInterestApplication,TransactionID,AccountDebited,AccountCredited,Debit,Credit,StockAffected,ItemCategoryID,StockCounter,RunningRate,DateOfTransaction,SlipIssued,SlipNumber,TransactionSet,IsCreditable,CreditID) values ('0','" + str + "','" + str5 + "','" + str6 + "','" + this.CarriedForwardInterest.Text + "','" + this.CarriedForwardInterest.Text + "','0','','0.0','0.0','" + this.BeginDate.Text + "','0','','" + Guid.NewGuid().ToString() + "','0','')";
                    Exception exception = BusinessLogic.ModifyStoreHouse(Con, queryToRun, null);
                    if (exception != null)
                    {
                        throw exception;
                    }
                    str = Convert.ToString((uint) (Convert.ToUInt32(str) + 1));
                }
                else if (num > 1)
                {
                    throw new Exception("The Carry Forward account has duplicate entries.Can not proceed!");
                }
                string str8 = Guid.NewGuid().ToString();
                string str9 = Guid.NewGuid().ToString();
                string str10 = Guid.NewGuid().ToString();
                if (this.ThereAfter2.Checked)
                {
                    strArray = new string[3];
                    text = this.BeginDate.Text;
                    str12 = null;
                    if (this.TermDuration1.Text != "Ever")
                    {
                        try
                        {
                            str12 = Convert.ToDateTime(this.TermDuration1.Text).ToShortDateString();
                        }
                        catch (Exception exception1)
                        {
                            exception2 = exception1;
                            str12 = (this.Per1.Text == "Month") ? Convert.ToDateTime(text).AddMonths(Convert.ToInt32(this.TermDuration1.Text)).ToShortDateString() : Convert.ToDateTime(text).AddMonths(Convert.ToInt32((decimal) (Convert.ToDecimal(this.TermDuration1.Text) * 12M))).ToShortDateString();
                        }
                    }
                    else
                    {
                        time = new DateTime(0xbb8, 1, 1);
                        str12 = time.ToShortDateString();
                    }
                    strArray[0] = "INSERT INTO InterestSchema (Per,IsCompound,SequenceOfInterestApplication,InterestSchemaID,Rate,StartDate,EndDate) values ('" + this.Per1.Text + "','" + ((this.InterestType1.Text == "Simple Interest") ? "0" : "1") + "','1','" + str8 + "','" + this.Rate1.Text + "','" + text + "','" + str12 + "') ";
                    text = Convert.ToDateTime(str12).AddDays(1.0).ToShortDateString();
                    str12 = null;
                    if (this.TermDuration2.Text != "Ever")
                    {
                        try
                        {
                            str12 = Convert.ToDateTime(this.TermDuration2.Text).ToShortDateString();
                        }
                        catch (Exception exception4)
                        {
                            exception2 = exception4;
                            str12 = (this.Per2.Text == "Month") ? Convert.ToDateTime(text).AddMonths(Convert.ToInt32(this.TermDuration2.Text)).ToShortDateString() : Convert.ToDateTime(text).AddMonths(Convert.ToInt32((decimal) (Convert.ToDecimal(this.TermDuration2.Text) * 12M))).ToShortDateString();
                        }
                    }
                    else
                    {
                        time = new DateTime(0xfa0, 1, 1);
                        str12 = time.ToShortDateString();
                    }
                    strArray[1] = "INSERT INTO InterestSchema (Per,IsCompound,SequenceOfInterestApplication,InterestSchemaID,Rate,StartDate,EndDate) values ('" + this.Per2.Text + "','" + ((this.InterestType2.Text == "Simple Interest") ? "0" : "1") + "','2','" + str8 + "','" + this.Rate2.Text + "','" + text + "','" + str12 + "') ";
                    text = Convert.ToDateTime(str12).AddDays(1.0).ToShortDateString();
                    time = new DateTime(0x1388, 1, 1);
                    str12 = time.ToShortDateString();
                    strArray[2] = "INSERT INTO InterestSchema (Per,IsCompound,SequenceOfInterestApplication,InterestSchemaID,Rate,StartDate,EndDate) values ('" + this.Per3.Text + "','" + ((this.InterestType3.Text == "Simple Interest") ? "0" : "1") + "','3','" + str8 + "','" + this.Rate3.Text + "','" + text + "','" + str12 + "') ";
                }
                else if (this.ThereAfter1.Checked)
                {
                    strArray = new string[2];
                    text = this.BeginDate.Text;
                    str12 = null;
                    if (this.TermDuration1.Text != "Ever")
                    {
                        try
                        {
                            str12 = Convert.ToDateTime(this.TermDuration1.Text).ToShortDateString();
                        }
                        catch (Exception exception5)
                        {
                            exception2 = exception5;
                            str12 = (this.Per1.Text == "Month") ? Convert.ToDateTime(text).AddMonths(Convert.ToInt32(this.TermDuration1.Text)).ToShortDateString() : Convert.ToDateTime(text).AddMonths(Convert.ToInt32((decimal) (Convert.ToDecimal(this.TermDuration1.Text) * 12M))).ToShortDateString();
                        }
                    }
                    else
                    {
                        time = new DateTime(0xfa0, 1, 1);
                        str12 = time.ToShortDateString();
                    }
                    strArray[0] = "INSERT INTO InterestSchema (Per,IsCompound,SequenceOfInterestApplication,InterestSchemaID,Rate,StartDate,EndDate) values ('" + this.Per1.Text + "','" + ((this.InterestType1.Text == "Simple Interest") ? "0" : "1") + "','1','" + str8 + "','" + this.Rate1.Text + "','" + text + "','" + str12 + "') ";
                    text = Convert.ToDateTime(str12).AddDays(1.0).ToShortDateString();
                    str12 = null;
                    if (this.TermDuration2.Text != "Ever")
                    {
                        try
                        {
                            str12 = Convert.ToDateTime(this.TermDuration2.Text).ToShortDateString();
                        }
                        catch (Exception exception6)
                        {
                            exception2 = exception6;
                            str12 = (this.Per2.Text == "Month") ? Convert.ToDateTime(text).AddMonths(Convert.ToInt32(this.TermDuration2.Text)).ToShortDateString() : Convert.ToDateTime(text).AddMonths(Convert.ToInt32((decimal) (Convert.ToDecimal(this.TermDuration2.Text) * 12M))).ToShortDateString();
                        }
                    }
                    else
                    {
                        time = new DateTime(0x1388, 1, 1);
                        str12 = time.ToShortDateString();
                    }
                    strArray[1] = "INSERT INTO InterestSchema (Per,IsCompound,SequenceOfInterestApplication,InterestSchemaID,Rate,StartDate,EndDate) values ('" + this.Per2.Text + "','" + ((this.InterestType2.Text == "Simple Interest") ? "0" : "1") + "','2','" + str8 + "','" + this.Rate2.Text + "','" + text + "','" + str12 + "') ";
                }
                else
                {
                    strArray = new string[1];
                    text = this.BeginDate.Text;
                    str12 = null;
                    if (this.TermDuration1.Text != "Ever")
                    {
                        try
                        {
                            str12 = Convert.ToDateTime(this.TermDuration1.Text).ToShortDateString();
                        }
                        catch (Exception exception7)
                        {
                            exception2 = exception7;
                            str12 = (this.Per1.Text == "Month") ? Convert.ToDateTime(text).AddMonths(Convert.ToInt32(this.TermDuration1.Text)).ToShortDateString() : Convert.ToDateTime(text).AddMonths(Convert.ToInt32((decimal) (Convert.ToDecimal(this.TermDuration1.Text) * 12M))).ToShortDateString();
                        }
                    }
                    else
                    {
                        str12 = new DateTime(0x1388, 1, 1).ToShortDateString();
                    }
                    strArray[0] = "INSERT INTO InterestSchema (Per,IsCompound,SequenceOfInterestApplication,InterestSchemaID,Rate,StartDate,EndDate) values ('" + this.Per1.Text + "','" + ((this.InterestType1.Text == "Simple Interest") ? "0" : "1") + "','1','" + str8 + "','" + this.Rate1.Text + "','" + text + "','" + str12 + "') ";
                }
                string str13 = "INSERT INTO CreditFlow(CreditID,TransactionID,InterestSchemaID,InterestDue,CarriedPrinciple,AmountReturnable,BroughtForward,IsClosed) values ('" + str9 + "','" + str + "','" + str8 + "','0','" + this.Principle.Text + "','" + this.Principle.Text + "','" + this.CarriedForwardInterest.Text + "','0')";
                string str14 = "INSERT INTO Transactions (SequenceOfInterestApplication,TransactionID,AccountDebited,AccountCredited,Debit,Credit,StockAffected,ItemCategoryID,StockCounter,RunningRate,DateOfTransaction,SlipIssued,SlipNumber,TransactionSet,IsCreditable,CreditID) values ('-1','" + str + "','" + str5 + "','Cash-Cash','" + this.Principle.Text + "','" + this.Principle.Text + "','0','','0.0','0.0','" + this.BeginDate.Text + "','" + (this.IsSlipIssued.Checked ? "1" : "0") + "','" + str4 + "','" + str10 + "','1','" + str9 + "')";
                string str15 = "UPDATE Transactions set Debit='" + this.CarriedForwardInterest.Text + "',Credit='" + this.CarriedForwardInterest.Text + "' where SequenceOfInterestApplication=0 and AccountCredited= '" + str6 + "'";
                string[] strArray2 = null;
                if (this.ThereAfter2.Checked)
                {
                    strArray2 = new string[3];
                    for (num2 = 1; num2 <= 3; num2++)
                    {
                        strArray2[num2 - 1] = "INSERT INTO Transactions (SequenceOfInterestApplication,TransactionID,AccountDebited,AccountCredited,Debit,Credit,StockAffected,ItemCategoryID,StockCounter,RunningRate,DateOfTransaction,SlipIssued,SlipNumber,TransactionSet,IsCreditable,CreditID) values ('" + num2.ToString() + "','" + Convert.ToString((long) (Convert.ToUInt32(str) + num2)) + "','" + str5 + "','" + str6 + "','0','0','0','','0.0','0.0','" + this.BeginDate.Text + "','0','','" + str10 + "','0','')";
                    }
                }
                else if (this.ThereAfter1.Checked)
                {
                    strArray2 = new string[2];
                    for (num2 = 1; num2 <= 2; num2++)
                    {
                        strArray2[num2 - 1] = "INSERT INTO Transactions (SequenceOfInterestApplication,TransactionID,AccountDebited,AccountCredited,Debit,Credit,StockAffected,ItemCategoryID,StockCounter,RunningRate,DateOfTransaction,SlipIssued,SlipNumber,TransactionSet,IsCreditable,CreditID) values ('" + num2.ToString() + "','" + Convert.ToString((long) (Convert.ToUInt32(str) + num2)) + "','" + str5 + "','" + str6 + "','0','0','0','','0.0','0.0','" + this.BeginDate.Text + "','0','','" + str10 + "','0','')";
                    }
                }
                else
                {
                    strArray2 = new string[1];
                    for (num2 = 1; num2 <= 1; num2++)
                    {
                        strArray2[num2 - 1] = "INSERT INTO Transactions (SequenceOfInterestApplication,TransactionID,AccountDebited,AccountCredited,Debit,Credit,StockAffected,ItemCategoryID,StockCounter,RunningRate,DateOfTransaction,SlipIssued,SlipNumber,TransactionSet,IsCreditable,CreditID) values ('" + num2.ToString() + "','" + Convert.ToString((long) (Convert.ToUInt32(str) + num2)) + "','" + str5 + "','" + str6 + "','0','0','0','','0.0','0.0','" + this.BeginDate.Text + "','0','','" + str10 + "','0','')";
                    }
                }
                string[] strArray3 = new string[] { "INSERT INTO ACCOUNTS(AccountID,Debit,Credit,DateOfTransaction,TransactionID) values('Cash-Cash','0','" + this.Principle.Text + "','" + this.BeginDate.Text + "','" + str + "')", "INSERT INTO ACCOUNTS(AccountID,Debit,Credit,DateOfTransaction,TransactionID) values('" + str5 + "','" + this.Principle.Text + "','0','" + this.BeginDate.Text + "','" + str + "')", "INSERT INTO ACCOUNTS(AccountID,Debit,Credit,DateOfTransaction,TransactionID) values('" + str6 + "','0','0','" + this.BeginDate.Text + "','" + Convert.ToString((uint) (Convert.ToUInt32(str) + 1)) + "')", "INSERT INTO ACCOUNTS(AccountID,Debit,Credit,DateOfTransaction,TransactionID) values('" + str5 + "','0','0','" + this.BeginDate.Text + "','" + Convert.ToString((uint) (Convert.ToUInt32(str) + 1)) + "')" };
                if (this.CreditIdToBeClosed == null)
                {
                    throw new Exception("ID of account to be closed is null.Can not proceed.");
                }
                if (this.CreditIdToBeClosed.Length < 2)
                {
                    throw new Exception("ID of account to be closed is invalid.Can not proceed.");
                }
                string str16 = "Update CreditFlow Set IsClosed='1' where CreditID='" + this.CreditIdToBeClosed + "'";
                int length = strArray.Length;
                string[] queriesToRun = new string[((((length + 1) + 2) + strArray2.Length) + 4) + 1];
                for (num2 = 0; num2 < length; num2++)
                {
                    queriesToRun[num2] = strArray[num2];
                }
                queriesToRun[length] = str13;
                queriesToRun[length + 1] = str14;
                queriesToRun[length + 2] = str15;
                for (num2 = 0; num2 < strArray2.Length; num2++)
                {
                    queriesToRun[(length + 3) + num2] = strArray2[num2];
                }
                for (num2 = 0; num2 < 4; num2++)
                {
                    queriesToRun[((length + 3) + strArray2.Length) + num2] = strArray3[num2];
                }
                queriesToRun[queriesToRun.Length - 1] = str16;
                Exception exception3 = BusinessLogic.PerformMultipleQueriesWithoutValidation(Con, queriesToRun);
                if (exception3 != null)
                {
                    throw exception3;
                }
                BusinessLogic.MyMessageBox("The credit of " + this.Principle.Text + @"\- to " + this.Creditee.Text + " is approved!", "Credit Approved!");
                base.Close();
            }
            catch (Exception exception8)
            {
                exception2 = exception8;
                BusinessLogic.MyMessageBox(exception2.Message);
            }
        }

        private void Principle_Leave(object sender, EventArgs e)
        {
            try
            {
                double num = Convert.ToDouble(this.Principle.Text);
            }
            catch (Exception)
            {
                this.Principle.Text = "";
                this.Principle.Focus();
                BusinessLogic.MyMessageBox("Please enter a numeric value only!");
            }
        }

        private void Rate_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal num = Convert.ToDecimal(((TextBox) sender).Text);
            }
            catch (Exception)
            {
                BusinessLogic.MyMessageBox("Please Enter only numeric or decimal values");
                ((TextBox) sender).Text = "";
                ((TextBox) sender).Focus();
            }
        }

        private void TermDuration1_Enter(object sender, EventArgs e)
        {
            if (!this.ThereAfter1.Enabled)
            {
                this.ThereAfter1.Enabled = true;
            }
            this.Term1.Text = this.Per1.Text + "s";
        }

        private void TermDuration1_Leave(object sender, EventArgs e)
        {
            if (this.TermDuration1.Text != "Ever")
            {
                try
                {
                    this.TermDuration1.Text = Convert.ToDateTime(this.TermDuration1.Text).ToShortDateString();
                    this.label6.Text = "Upto";
                    this.Term1.Text = "";
                }
                catch (Exception)
                {
                    try
                    {
                        decimal num = Convert.ToDecimal(this.TermDuration1.Text);
                        this.label6.Text = "For";
                    }
                    catch (Exception)
                    {
                        BusinessLogic.MyMessageBox("Please Enter only numeric or decimal values");
                        this.TermDuration1.Text = "";
                        this.TermDuration1.Focus();
                    }
                }
            }
        }

        private void TermDuration2_Enter(object sender, EventArgs e)
        {
            if (!this.ThereAfter2.Enabled)
            {
                this.ThereAfter2.Enabled = true;
            }
            this.Term2.Text = this.Per2.Text + "s";
        }

        private void TermDuration2_Leave(object sender, EventArgs e)
        {
            if (this.TermDuration2.Text != "Ever")
            {
                try
                {
                    this.TermDuration2.Text = Convert.ToDateTime(this.TermDuration2.Text).ToShortDateString();
                    this.label11.Text = "Upto";
                    this.Term2.Text = "";
                }
                catch (Exception)
                {
                    try
                    {
                        decimal num = Convert.ToDecimal(this.TermDuration2.Text);
                        this.label11.Text = "For";
                    }
                    catch (Exception)
                    {
                        BusinessLogic.MyMessageBox("Please Enter only numeric or decimal values");
                        this.TermDuration2.Text = "";
                        this.TermDuration2.Focus();
                    }
                }
            }
        }

        private void ThereAfter1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ThereAfter1.Checked)
            {
                this.Rate2.Enabled = true;
                this.Per2.Enabled = true;
                this.InterestType2.Enabled = true;
                this.TermDuration2.Enabled = true;
                this.InterestType2.SelectedIndex = 1;
                this.Per2.SelectedIndex = 0;
                this.Rate2.Text = "12";
            }
            else if (!this.ThereAfter1.Checked)
            {
                this.Rate2.Enabled = false;
                this.Per2.Enabled = false;
                this.InterestType2.Enabled = false;
                this.TermDuration2.Enabled = false;
            }
        }

        private void ThereAfter2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ThereAfter2.Checked)
            {
                this.Rate3.Enabled = true;
                this.Per3.Enabled = true;
                this.InterestType3.Enabled = true;
                this.TermDuration3.Enabled = true;
                this.InterestType3.SelectedIndex = 1;
                this.Per3.SelectedIndex = 0;
                this.TermDuration3.SelectedIndex = 0;
                this.Rate3.Text = "12";
            }
            else if (!this.ThereAfter2.Checked)
            {
                this.Rate3.Enabled = false;
                this.Per3.Enabled = false;
                this.InterestType3.Enabled = false;
                this.TermDuration3.Enabled = false;
            }
        }

        private void ValidateData()
        {
            if (this.Creditee.Text == null)
            {
                throw new Exception("Please select a person to reissue credit to.\nIf the person is not in the selected box,\nfirst add a Credit and an Interest account\nfor the person from the Accounts Tab");
            }
            if (this.Creditee.Text.Length == 0)
            {
                throw new Exception("Please select a person to reissue credit to.\nIf the person is not in the selected box,\nfirst add a Credit and an Interest account\nfor the person from the Accounts Tab");
            }
            bool flag = false;
            foreach (Control control in base.Controls)
            {
                if ((control.Enabled && control.Visible) && ((control.GetType().Name != "Label") && (control.GetType().Name != "Button")))
                {
                    if (control.Text == "Ever")
                    {
                        flag = true;
                    }
                    if (control.Text == null)
                    {
                        control.Focus();
                        throw new Exception("Enter a valid value in the box");
                    }
                    if (control.Text.Length == 0)
                    {
                        control.Focus();
                        throw new Exception("Enter a valid value in the box");
                    }
                }
            }
            if (!flag)
            {
                throw new Exception("Atleat one of the term durations must end with \"Ever\" .");
            }
            if (Convert.ToDecimal(this.Rate1.Text) < 0M)
            {
                this.Rate1.Text = "";
                this.Rate1.Focus();
                throw new Exception("Rate can not be less than 0");
            }
            if (this.Rate2.Enabled && (Convert.ToDecimal(this.Rate2.Text) < 0M))
            {
                this.Rate2.Text = "";
                this.Rate2.Focus();
                throw new Exception("Rate can not be less than 0");
            }
            if (this.Rate3.Enabled && (Convert.ToDecimal(this.Rate3.Text) < 0M))
            {
                this.Rate3.Text = "";
                this.Rate3.Focus();
                throw new Exception("Rate can not be less than 0");
            }
        }
    }
}

