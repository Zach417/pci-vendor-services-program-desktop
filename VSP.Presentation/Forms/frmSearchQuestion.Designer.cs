﻿namespace VSP.Presentation.Forms
{
    partial class frmSearchQuestion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchQuestion));
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
            this.tabClientSummary = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboAnswer = new System.Windows.Forms.ComboBox();
            this.lblAccountInformationHeader = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.RichTextBox();
            this.pnlSummaryTabHeader = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.tabControlClientDetail = new System.Windows.Forms.TabControl();
            this.btnFeedback = new VSP.Presentation.Forms.FeedbackButton();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel16.SuspendLayout();
            this.tabClientSummary.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlSummaryTabHeader.SuspendLayout();
            this.tabControlClientDetail.SuspendLayout();
            this.SuspendLayout();
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
            this.panel4.Controls.Add(this.btnFeedback);
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
            this.label38.Click += new System.EventHandler(this.CloseForm);
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
            this.label1.Size = new System.Drawing.Size(265, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "Vendor Services Program - Search Question";
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
            // tabClientSummary
            // 
            this.tabClientSummary.BackColor = System.Drawing.Color.Gainsboro;
            this.tabClientSummary.Controls.Add(this.panel2);
            this.tabClientSummary.Controls.Add(this.pnlSummaryTabHeader);
            this.tabClientSummary.Location = new System.Drawing.Point(4, 25);
            this.tabClientSummary.Margin = new System.Windows.Forms.Padding(2);
            this.tabClientSummary.Name = "tabClientSummary";
            this.tabClientSummary.Padding = new System.Windows.Forms.Padding(2);
            this.tabClientSummary.Size = new System.Drawing.Size(981, 600);
            this.tabClientSummary.TabIndex = 0;
            this.tabClientSummary.Text = "Summary";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(218)))), ((int)(((byte)(219)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cboAnswer);
            this.panel2.Controls.Add(this.lblAccountInformationHeader);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.txtSubject);
            this.panel2.Location = new System.Drawing.Point(2, 46);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(968, 545);
            this.panel2.TabIndex = 58;
            // 
            // cboAnswer
            // 
            this.cboAnswer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboAnswer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAnswer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboAnswer.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.cboAnswer.Location = new System.Drawing.Point(136, 59);
            this.cboAnswer.Margin = new System.Windows.Forms.Padding(2);
            this.cboAnswer.Name = "cboAnswer";
            this.cboAnswer.Size = new System.Drawing.Size(814, 21);
            this.cboAnswer.TabIndex = 72;
            // 
            // lblAccountInformationHeader
            // 
            this.lblAccountInformationHeader.AutoSize = true;
            this.lblAccountInformationHeader.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountInformationHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(46)))), ((int)(((byte)(71)))));
            this.lblAccountInformationHeader.Location = new System.Drawing.Point(12, 13);
            this.lblAccountInformationHeader.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAccountInformationHeader.Name = "lblAccountInformationHeader";
            this.lblAccountInformationHeader.Size = new System.Drawing.Size(82, 19);
            this.lblAccountInformationHeader.TabIndex = 40;
            this.lblAccountInformationHeader.Text = "Summary";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(19, 60);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 18);
            this.label3.TabIndex = 52;
            this.label3.Text = "Answer:";
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Arial", 9F);
            this.label24.Location = new System.Drawing.Point(19, 39);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(113, 18);
            this.label24.TabIndex = 52;
            this.label24.Text = "Subject:";
            // 
            // txtSubject
            // 
            this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubject.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSubject.Font = new System.Drawing.Font("Arial", 8F);
            this.txtSubject.ForeColor = System.Drawing.Color.Black;
            this.txtSubject.Location = new System.Drawing.Point(136, 38);
            this.txtSubject.Margin = new System.Windows.Forms.Padding(2);
            this.txtSubject.Multiline = false;
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtSubject.Size = new System.Drawing.Size(814, 19);
            this.txtSubject.TabIndex = 1;
            this.txtSubject.Text = "";
            this.txtSubject.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // pnlSummaryTabHeader
            // 
            this.pnlSummaryTabHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSummaryTabHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSummaryTabHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSummaryTabHeader.Controls.Add(this.btnSave);
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
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.BackgroundImage = global::VSP.Properties.Resources.save;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Gadugi", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(922, 8);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(30, 30);
            this.btnSave.TabIndex = 41;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("High Tower Text", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(-3, 1);
            this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(273, 44);
            this.label23.TabIndex = 0;
            this.label23.Text = "Search Question";
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
            // tabControlClientDetail
            // 
            this.tabControlClientDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlClientDetail.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlClientDetail.Controls.Add(this.tabClientSummary);
            this.tabControlClientDetail.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tabControlClientDetail.Location = new System.Drawing.Point(-6, 0);
            this.tabControlClientDetail.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlClientDetail.Name = "tabControlClientDetail";
            this.tabControlClientDetail.SelectedIndex = 0;
            this.tabControlClientDetail.Size = new System.Drawing.Size(989, 629);
            this.tabControlClientDetail.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlClientDetail.TabIndex = 37;
            // 
            // btnFeedback
            // 
            this.btnFeedback.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFeedback.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFeedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFeedback.Location = new System.Drawing.Point(848, 1);
            this.btnFeedback.Name = "btnFeedback";
            this.btnFeedback.Size = new System.Drawing.Size(118, 18);
            this.btnFeedback.TabIndex = 62;
            this.btnFeedback.Text = "Send Feedback";
            this.btnFeedback.UseVisualStyleBackColor = false;
            // 
            // frmSearchQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(968, 688);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel16);
            this.Controls.Add(this.tabControlClientDetail);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(755, 595);
            this.Name = "frmSearchQuestion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search Question";
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.tabClientSummary.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlSummaryTabHeader.ResumeLayout(false);
            this.pnlSummaryTabHeader.PerformLayout();
            this.tabControlClientDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.Label label25;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TabPage tabClientSummary;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label lblAccountInformationHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label24;
        public System.Windows.Forms.RichTextBox txtSubject;
        private System.Windows.Forms.Panel pnlSummaryTabHeader;
        public System.Windows.Forms.Label label23;
        public System.Windows.Forms.Label label56;
        public System.Windows.Forms.TabControl tabControlClientDetail;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.ComboBox cboAnswer;
        private VSP.Presentation.Forms.FeedbackButton btnFeedback;
    }
}
