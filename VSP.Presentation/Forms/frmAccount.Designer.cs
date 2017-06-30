namespace VSP.Presentation.Forms
{
	partial class frmAccount
	{
		private System.ComponentModel.IContainer components = null;
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccount));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControlDetail = new System.Windows.Forms.TabControl();
            this.tabSummary = new System.Windows.Forms.TabPage();
            this.pnlSummaryTabHeader = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblAddressHeader = new System.Windows.Forms.Label();
            this.lblContactInformationHeader = new System.Windows.Forms.Label();
            this.lblAccountInformationHeader = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtAddressZip = new System.Windows.Forms.RichTextBox();
            this.txtFax = new System.Windows.Forms.RichTextBox();
            this.txtPrimaryContactEmail = new System.Windows.Forms.RichTextBox();
            this.txtPrimaryContactName = new System.Windows.Forms.RichTextBox();
            this.txtPhone = new System.Windows.Forms.RichTextBox();
            this.txtAddressState = new System.Windows.Forms.RichTextBox();
            this.assetvalue = new System.Windows.Forms.RichTextBox();
            this.txtAddressCity = new System.Windows.Forms.RichTextBox();
            this.txtAddressLine2 = new System.Windows.Forms.RichTextBox();
            this.txtName = new System.Windows.Forms.RichTextBox();
            this.txtAddressLine1 = new System.Windows.Forms.RichTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label25 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label46 = new System.Windows.Forms.Label();
            this.panel16 = new System.Windows.Forms.Panel();
            this.label38 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.tabPlans = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dgvPlans = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.cboPlanViews = new System.Windows.Forms.ComboBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControlDetail.SuspendLayout();
            this.tabSummary.SuspendLayout();
            this.pnlSummaryTabHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel16.SuspendLayout();
            this.tabPlans.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlans)).BeginInit();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlDetail
            // 
            this.tabControlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlDetail.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlDetail.Controls.Add(this.tabSummary);
            this.tabControlDetail.Controls.Add(this.tabPlans);
            this.tabControlDetail.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tabControlDetail.Location = new System.Drawing.Point(-6, 0);
            this.tabControlDetail.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlDetail.Name = "tabControlDetail";
            this.tabControlDetail.SelectedIndex = 0;
            this.tabControlDetail.Size = new System.Drawing.Size(989, 629);
            this.tabControlDetail.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlDetail.TabIndex = 37;
            // 
            // tabSummary
            // 
            this.tabSummary.BackColor = System.Drawing.Color.Gainsboro;
            this.tabSummary.Controls.Add(this.pnlSummaryTabHeader);
            this.tabSummary.Controls.Add(this.panel2);
            this.tabSummary.Location = new System.Drawing.Point(4, 25);
            this.tabSummary.Margin = new System.Windows.Forms.Padding(2);
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.Padding = new System.Windows.Forms.Padding(2);
            this.tabSummary.Size = new System.Drawing.Size(981, 600);
            this.tabSummary.TabIndex = 0;
            this.tabSummary.Text = "Summary";
            // 
            // pnlSummaryTabHeader
            // 
            this.pnlSummaryTabHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSummaryTabHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSummaryTabHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSummaryTabHeader.Controls.Add(this.label23);
            this.pnlSummaryTabHeader.Controls.Add(this.label56);
            this.pnlSummaryTabHeader.Font = new System.Drawing.Font("High Tower Text", 32F);
            this.pnlSummaryTabHeader.Location = new System.Drawing.Point(2, 0);
            this.pnlSummaryTabHeader.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSummaryTabHeader.Name = "pnlSummaryTabHeader";
            this.pnlSummaryTabHeader.Size = new System.Drawing.Size(968, 49);
            this.pnlSummaryTabHeader.TabIndex = 57;
            this.pnlSummaryTabHeader.DoubleClick += new System.EventHandler(this.MaximizeForm);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("High Tower Text", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(-3, 1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(270, 44);
            this.label23.TabIndex = 0;
            this.label23.Text = "Company Name";
            this.label23.TextChanged += new System.EventHandler(this.label23_TextChanged);
            this.label23.DoubleClick += new System.EventHandler(this.MaximizeForm);
            // 
            // label56
            // 
            this.label56.BackColor = System.Drawing.Color.Transparent;
            this.label56.Font = new System.Drawing.Font("Gadugi", 20F);
            this.label56.ForeColor = System.Drawing.Color.Black;
            this.label56.Location = new System.Drawing.Point(8, 46);
            this.label56.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(152, 38);
            this.label56.TabIndex = 40;
            this.label56.Text = "Summary";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(218)))), ((int)(((byte)(219)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblAddressHeader);
            this.panel2.Controls.Add(this.lblContactInformationHeader);
            this.panel2.Controls.Add(this.lblAccountInformationHeader);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.txtAddressZip);
            this.panel2.Controls.Add(this.txtFax);
            this.panel2.Controls.Add(this.txtPrimaryContactEmail);
            this.panel2.Controls.Add(this.txtPrimaryContactName);
            this.panel2.Controls.Add(this.txtPhone);
            this.panel2.Controls.Add(this.txtAddressState);
            this.panel2.Controls.Add(this.assetvalue);
            this.panel2.Controls.Add(this.txtAddressCity);
            this.panel2.Controls.Add(this.txtAddressLine2);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.txtAddressLine1);
            this.panel2.Location = new System.Drawing.Point(2, 46);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(968, 545);
            this.panel2.TabIndex = 58;
            // 
            // lblAddressHeader
            // 
            this.lblAddressHeader.AutoSize = true;
            this.lblAddressHeader.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddressHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(46)))), ((int)(((byte)(71)))));
            this.lblAddressHeader.Location = new System.Drawing.Point(13, 164);
            this.lblAddressHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddressHeader.Name = "lblAddressHeader";
            this.lblAddressHeader.Size = new System.Drawing.Size(69, 19);
            this.lblAddressHeader.TabIndex = 40;
            this.lblAddressHeader.Text = "Address";
            // 
            // lblContactInformationHeader
            // 
            this.lblContactInformationHeader.AutoSize = true;
            this.lblContactInformationHeader.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactInformationHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(46)))), ((int)(((byte)(71)))));
            this.lblContactInformationHeader.Location = new System.Drawing.Point(13, 86);
            this.lblContactInformationHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblContactInformationHeader.Name = "lblContactInformationHeader";
            this.lblContactInformationHeader.Size = new System.Drawing.Size(164, 19);
            this.lblContactInformationHeader.TabIndex = 40;
            this.lblContactInformationHeader.Text = "Contact Information";
            // 
            // lblAccountInformationHeader
            // 
            this.lblAccountInformationHeader.AutoSize = true;
            this.lblAccountInformationHeader.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountInformationHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(46)))), ((int)(((byte)(71)))));
            this.lblAccountInformationHeader.Location = new System.Drawing.Point(12, 13);
            this.lblAccountInformationHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAccountInformationHeader.Name = "lblAccountInformationHeader";
            this.lblAccountInformationHeader.Size = new System.Drawing.Size(168, 19);
            this.lblAccountInformationHeader.TabIndex = 40;
            this.lblAccountInformationHeader.Text = "Account Information";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial", 9F);
            this.label11.Location = new System.Drawing.Point(707, 214);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 16);
            this.label11.TabIndex = 52;
            this.label11.Text = "ZIP:";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 9F);
            this.label13.Location = new System.Drawing.Point(453, 134);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 19);
            this.label13.TabIndex = 52;
            this.label13.Text = "Fax:";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Arial", 9F);
            this.label15.Location = new System.Drawing.Point(452, 113);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 19);
            this.label15.TabIndex = 52;
            this.label15.Text = "Primary Email:";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Arial", 9F);
            this.label12.Location = new System.Drawing.Point(19, 134);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 19);
            this.label12.TabIndex = 52;
            this.label12.Text = "Phone:";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Arial", 9F);
            this.label10.Location = new System.Drawing.Point(453, 214);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 19);
            this.label10.TabIndex = 52;
            this.label10.Text = "State:";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Arial", 9F);
            this.label9.Location = new System.Drawing.Point(453, 192);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 19);
            this.label9.TabIndex = 52;
            this.label9.Text = "City";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Arial", 9F);
            this.label8.Location = new System.Drawing.Point(19, 212);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 19);
            this.label8.TabIndex = 52;
            this.label8.Text = "Line 2:";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Arial", 9F);
            this.label7.Location = new System.Drawing.Point(19, 190);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 19);
            this.label7.TabIndex = 52;
            this.label7.Text = "Line 1:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Arial", 9F);
            this.label4.Location = new System.Drawing.Point(19, 114);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 18);
            this.label4.TabIndex = 52;
            this.label4.Text = "Primary Contact:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(19, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 18);
            this.label3.TabIndex = 52;
            this.label3.Text = "Asset Value:";
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Arial", 9F);
            this.label24.Location = new System.Drawing.Point(19, 39);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(113, 18);
            this.label24.TabIndex = 52;
            this.label24.Text = "Account Name:";
            // 
            // txtAddressZip
            // 
            this.txtAddressZip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddressZip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAddressZip.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAddressZip.Font = new System.Drawing.Font("Arial", 8F);
            this.txtAddressZip.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtAddressZip.Location = new System.Drawing.Point(740, 212);
            this.txtAddressZip.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddressZip.Multiline = false;
            this.txtAddressZip.Name = "txtAddressZip";
            this.txtAddressZip.ReadOnly = true;
            this.txtAddressZip.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtAddressZip.Size = new System.Drawing.Size(210, 19);
            this.txtAddressZip.TabIndex = 13;
            this.txtAddressZip.Text = "";
            // 
            // txtFax
            // 
            this.txtFax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFax.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFax.Font = new System.Drawing.Font("Arial", 8F);
            this.txtFax.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtFax.Location = new System.Drawing.Point(551, 134);
            this.txtFax.Margin = new System.Windows.Forms.Padding(2);
            this.txtFax.Multiline = false;
            this.txtFax.Name = "txtFax";
            this.txtFax.ReadOnly = true;
            this.txtFax.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtFax.Size = new System.Drawing.Size(399, 19);
            this.txtFax.TabIndex = 8;
            this.txtFax.Text = "";
            // 
            // txtPrimaryContactEmail
            // 
            this.txtPrimaryContactEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrimaryContactEmail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPrimaryContactEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPrimaryContactEmail.Font = new System.Drawing.Font("Arial", 8F);
            this.txtPrimaryContactEmail.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPrimaryContactEmail.Location = new System.Drawing.Point(551, 113);
            this.txtPrimaryContactEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtPrimaryContactEmail.Multiline = false;
            this.txtPrimaryContactEmail.Name = "txtPrimaryContactEmail";
            this.txtPrimaryContactEmail.ReadOnly = true;
            this.txtPrimaryContactEmail.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtPrimaryContactEmail.Size = new System.Drawing.Size(399, 19);
            this.txtPrimaryContactEmail.TabIndex = 6;
            this.txtPrimaryContactEmail.Text = "";
            // 
            // txtPrimaryContactName
            // 
            this.txtPrimaryContactName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPrimaryContactName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPrimaryContactName.Font = new System.Drawing.Font("Arial", 8F);
            this.txtPrimaryContactName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPrimaryContactName.Location = new System.Drawing.Point(136, 112);
            this.txtPrimaryContactName.Margin = new System.Windows.Forms.Padding(2);
            this.txtPrimaryContactName.Multiline = false;
            this.txtPrimaryContactName.Name = "txtPrimaryContactName";
            this.txtPrimaryContactName.ReadOnly = true;
            this.txtPrimaryContactName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtPrimaryContactName.Size = new System.Drawing.Size(297, 19);
            this.txtPrimaryContactName.TabIndex = 5;
            this.txtPrimaryContactName.Text = "";
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPhone.Font = new System.Drawing.Font("Arial", 8F);
            this.txtPhone.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPhone.Location = new System.Drawing.Point(136, 133);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(2);
            this.txtPhone.Multiline = false;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.ReadOnly = true;
            this.txtPhone.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtPhone.Size = new System.Drawing.Size(297, 19);
            this.txtPhone.TabIndex = 7;
            this.txtPhone.Text = "";
            // 
            // txtAddressState
            // 
            this.txtAddressState.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAddressState.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAddressState.Font = new System.Drawing.Font("Arial", 8F);
            this.txtAddressState.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtAddressState.Location = new System.Drawing.Point(551, 212);
            this.txtAddressState.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddressState.Multiline = false;
            this.txtAddressState.Name = "txtAddressState";
            this.txtAddressState.ReadOnly = true;
            this.txtAddressState.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtAddressState.Size = new System.Drawing.Size(152, 19);
            this.txtAddressState.TabIndex = 12;
            this.txtAddressState.Text = "";
            // 
            // assetvalue
            // 
            this.assetvalue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetvalue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.assetvalue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.assetvalue.Font = new System.Drawing.Font("Arial", 8F);
            this.assetvalue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.assetvalue.Location = new System.Drawing.Point(136, 59);
            this.assetvalue.Margin = new System.Windows.Forms.Padding(2);
            this.assetvalue.Multiline = false;
            this.assetvalue.Name = "assetvalue";
            this.assetvalue.ReadOnly = true;
            this.assetvalue.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.assetvalue.Size = new System.Drawing.Size(814, 19);
            this.assetvalue.TabIndex = 2;
            this.assetvalue.Text = "";
            // 
            // txtAddressCity
            // 
            this.txtAddressCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddressCity.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAddressCity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAddressCity.Font = new System.Drawing.Font("Arial", 8F);
            this.txtAddressCity.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtAddressCity.Location = new System.Drawing.Point(551, 191);
            this.txtAddressCity.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddressCity.Multiline = false;
            this.txtAddressCity.Name = "txtAddressCity";
            this.txtAddressCity.ReadOnly = true;
            this.txtAddressCity.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtAddressCity.Size = new System.Drawing.Size(399, 19);
            this.txtAddressCity.TabIndex = 10;
            this.txtAddressCity.Text = "";
            // 
            // txtAddressLine2
            // 
            this.txtAddressLine2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAddressLine2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAddressLine2.Font = new System.Drawing.Font("Arial", 8F);
            this.txtAddressLine2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtAddressLine2.Location = new System.Drawing.Point(136, 210);
            this.txtAddressLine2.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddressLine2.Multiline = false;
            this.txtAddressLine2.Name = "txtAddressLine2";
            this.txtAddressLine2.ReadOnly = true;
            this.txtAddressLine2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtAddressLine2.Size = new System.Drawing.Size(297, 19);
            this.txtAddressLine2.TabIndex = 11;
            this.txtAddressLine2.Text = "";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.Font = new System.Drawing.Font("Arial", 8F);
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(136, 38);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtName.Size = new System.Drawing.Size(814, 19);
            this.txtName.TabIndex = 1;
            this.txtName.Text = "";
            // 
            // txtAddressLine1
            // 
            this.txtAddressLine1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAddressLine1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAddressLine1.Font = new System.Drawing.Font("Arial", 8F);
            this.txtAddressLine1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtAddressLine1.Location = new System.Drawing.Point(136, 189);
            this.txtAddressLine1.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddressLine1.Multiline = false;
            this.txtAddressLine1.Name = "txtAddressLine1";
            this.txtAddressLine1.ReadOnly = true;
            this.txtAddressLine1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtAddressLine1.Size = new System.Drawing.Size(297, 19);
            this.txtAddressLine1.TabIndex = 9;
            this.txtAddressLine1.Text = "";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(218)))), ((int)(((byte)(219)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label25);
            this.panel4.Location = new System.Drawing.Point(0, 667);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(968, 21);
            this.panel4.TabIndex = 57;
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(5, 0);
            this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(576, 16);
            this.label25.TabIndex = 40;
            this.label25.Text = "Ready";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label46);
            this.panel3.Location = new System.Drawing.Point(0, 615);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(968, 53);
            this.panel3.TabIndex = 58;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BackColor = System.Drawing.Color.Transparent;
            this.label46.Cursor = System.Windows.Forms.Cursors.Default;
            this.label46.Font = new System.Drawing.Font("Gadugi", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.ForeColor = System.Drawing.Color.Black;
            this.label46.Location = new System.Drawing.Point(12, 15);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(99, 25);
            this.label46.TabIndex = 0;
            this.label46.Text = "Summary";
            this.label46.Click += new System.EventHandler(this.label46_Click);
            this.label46.MouseEnter += new System.EventHandler(this.MenuItem_MouseEnter);
            this.label46.MouseLeave += new System.EventHandler(this.MenuItem_MouseLeave);
            // 
            // panel16
            // 
            this.panel16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(46)))), ((int)(((byte)(71)))));
            this.panel16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel16.Controls.Add(this.label38);
            this.panel16.Controls.Add(this.label1);
            this.panel16.Location = new System.Drawing.Point(0, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(968, 27);
            this.panel16.TabIndex = 60;
            this.panel16.DoubleClick += new System.EventHandler(this.MaximizeForm);
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.White;
            this.label38.Location = new System.Drawing.Point(940, 1);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(25, 25);
            this.label38.TabIndex = 22;
            this.label38.Text = "x";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label38.Click += new System.EventHandler(this.label38_Click);
            this.label38.MouseEnter += new System.EventHandler(this.CloseFormButton_MouseEnter);
            this.label38.MouseLeave += new System.EventHandler(this.CloseFormButton_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gadugi", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "Investment Services Program - Account";
            this.label1.DoubleClick += new System.EventHandler(this.MaximizeForm);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Save";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Save and Close";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Save and New";
            // 
            // tabPlans
            // 
            this.tabPlans.Controls.Add(this.panel6);
            this.tabPlans.Controls.Add(this.panel7);
            this.tabPlans.Location = new System.Drawing.Point(4, 25);
            this.tabPlans.Name = "tabPlans";
            this.tabPlans.Padding = new System.Windows.Forms.Padding(3);
            this.tabPlans.Size = new System.Drawing.Size(981, 600);
            this.tabPlans.TabIndex = 1;
            this.tabPlans.Text = "Plans";
            this.tabPlans.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Font = new System.Drawing.Font("Gadugi", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(117, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Plans";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            this.label2.MouseEnter += new System.EventHandler(this.MenuItem_MouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.MenuItem_MouseLeave);
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(218)))), ((int)(((byte)(219)))));
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.dgvPlans);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.cboPlanViews);
            this.panel6.Location = new System.Drawing.Point(2, 46);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(968, 545);
            this.panel6.TabIndex = 64;
            // 
            // dgvPlans
            // 
            this.dgvPlans.AllowUserToAddRows = false;
            this.dgvPlans.AllowUserToDeleteRows = false;
            this.dgvPlans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPlans.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlans.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPlans.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gadugi", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPlans.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPlans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gadugi", 7.8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPlans.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPlans.EnableHeadersVisualStyles = false;
            this.dgvPlans.Location = new System.Drawing.Point(8, 34);
            this.dgvPlans.Margin = new System.Windows.Forms.Padding(2);
            this.dgvPlans.MultiSelect = false;
            this.dgvPlans.Name = "dgvPlans";
            this.dgvPlans.ReadOnly = true;
            this.dgvPlans.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPlans.RowHeadersVisible = false;
            this.dgvPlans.RowTemplate.Height = 24;
            this.dgvPlans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPlans.ShowEditingIcon = false;
            this.dgvPlans.Size = new System.Drawing.Size(944, 504);
            this.dgvPlans.TabIndex = 69;
            this.dgvPlans.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlans_CellDoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Gadugi", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(5, 9);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 16);
            this.label5.TabIndex = 71;
            this.label5.Text = "Views";
            // 
            // cboPlanViews
            // 
            this.cboPlanViews.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlanViews.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPlanViews.Items.AddRange(new object[] {
            "Active Associated Plans",
            "Inactive Associated Plans"});
            this.cboPlanViews.Location = new System.Drawing.Point(50, 7);
            this.cboPlanViews.Margin = new System.Windows.Forms.Padding(2);
            this.cboPlanViews.Name = "cboPlanViews";
            this.cboPlanViews.Size = new System.Drawing.Size(157, 21);
            this.cboPlanViews.TabIndex = 70;
            this.cboPlanViews.SelectedIndexChanged += new System.EventHandler(this.cboPlanViews_SelectedIndexChanged);
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.BackColor = System.Drawing.SystemColors.Control;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Font = new System.Drawing.Font("High Tower Text", 32F);
            this.panel7.Location = new System.Drawing.Point(2, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(2);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(968, 49);
            this.panel7.TabIndex = 63;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("High Tower Text", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(-3, 1);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 44);
            this.label6.TabIndex = 0;
            this.label6.Text = "Plans";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Gadugi", 20F);
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(8, 46);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(152, 38);
            this.label14.TabIndex = 40;
            this.label14.Text = "Summary";
            // 
            // frmAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(968, 688);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel16);
            this.Controls.Add(this.tabControlDetail);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(755, 595);
            this.Name = "frmAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account";
            this.tabControlDetail.ResumeLayout(false);
            this.tabSummary.ResumeLayout(false);
            this.pnlSummaryTabHeader.ResumeLayout(false);
            this.pnlSummaryTabHeader.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.tabPlans.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlans)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }
		public System.Windows.Forms.RichTextBox txtName;
        private System.Windows.Forms.Label label24;
        public System.Windows.Forms.Label lblContactInformationHeader;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
		public System.Windows.Forms.RichTextBox txtAddressZip;
		public System.Windows.Forms.RichTextBox txtAddressState;
		public System.Windows.Forms.RichTextBox txtAddressCity;
		public System.Windows.Forms.RichTextBox txtAddressLine2;
		public System.Windows.Forms.RichTextBox txtAddressLine1;
		public System.Windows.Forms.RichTextBox txtPhone;
		public System.Windows.Forms.RichTextBox txtPrimaryContactEmail;
        public System.Windows.Forms.RichTextBox txtFax;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		public System.Windows.Forms.Label lblAccountInformationHeader;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabSummary;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
        public System.Windows.Forms.RichTextBox assetvalue;
        public System.Windows.Forms.TabControl tabControlDetail;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Label label25;
        private System.Windows.Forms.Panel pnlSummaryTabHeader;
        public System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.RichTextBox txtPrimaryContactName;
        public System.Windows.Forms.Label lblAddressHeader;
        public System.Windows.Forms.Label label56;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabPlans;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.DataGridView dgvPlans;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cboPlanViews;
        private System.Windows.Forms.Panel panel7;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label14;
	}
}
