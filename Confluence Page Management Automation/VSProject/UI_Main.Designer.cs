namespace SSAConfluenceHRPageCycle
{
    partial class UI_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnMovePages = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblSettings = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtXMLRPCURL = new System.Windows.Forms.TextBox();
            this.lblSpace = new System.Windows.Forms.Label();
            this.txtMyPerformancePageName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDestinationPageName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpaceKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtArchivePrefix = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTemplateLocation = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnMoveDeptPages = new System.Windows.Forms.Button();
            this.txtExclude = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMyDeptPerformancePageName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtBonusTemplatePageName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtObjectives = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMyDeptPerfTemplateLocation = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkProcessPerformanceAppraisals = new System.Windows.Forms.CheckBox();
            this.chkProcessBonus = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkProcessObjectives = new System.Windows.Forms.CheckBox();
            this.chkCreateObjectives = new System.Windows.Forms.CheckBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.chkArchivePDP = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(13, 11);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(219, 32);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.txtLogin_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(13, 88);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(447, 612);
            this.txtStatus.TabIndex = 1;
            // 
            // btnMovePages
            // 
            this.btnMovePages.Enabled = false;
            this.btnMovePages.Location = new System.Drawing.Point(242, 11);
            this.btnMovePages.Name = "btnMovePages";
            this.btnMovePages.Size = new System.Drawing.Size(218, 71);
            this.btnMovePages.TabIndex = 2;
            this.btnMovePages.Text = "Process Pages in My Performance";
            this.btnMovePages.UseVisualStyleBackColor = true;
            this.btnMovePages.Click += new System.EventHandler(this.txtMovePages_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Enabled = false;
            this.btnLogout.Location = new System.Drawing.Point(13, 50);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(219, 32);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.txtLogout_Click);
            // 
            // lblSettings
            // 
            this.lblSettings.AutoSize = true;
            this.lblSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettings.Location = new System.Drawing.Point(531, 86);
            this.lblSettings.Name = "lblSettings";
            this.lblSettings.Size = new System.Drawing.Size(81, 20);
            this.lblSettings.TabIndex = 4;
            this.lblSettings.Text = "Settings:";
            this.lblSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(468, 118);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "Username:";
            this.toolTip1.SetToolTip(this.lblUsername, "Script will login/run under this account.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(468, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Password:";
            this.toolTip1.SetToolTip(this.label1, "You should know.");
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.SystemColors.Menu;
            this.txtUsername.Location = new System.Drawing.Point(471, 134);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(218, 20);
            this.txtUsername.TabIndex = 7;
            this.txtUsername.Text = "adminuser";
            this.toolTip1.SetToolTip(this.txtUsername, "Script will login/run under this account.");
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(471, 173);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(218, 20);
            this.txtPassword.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtPassword, "You should know.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(692, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Confluence Address (XML-RPC URL):";
            this.toolTip1.SetToolTip(this.label2, "Ensure the remote API is enabled on the confluence instance.  Provide the RPC con" +
        "nection URL in here.");
            // 
            // txtXMLRPCURL
            // 
            this.txtXMLRPCURL.BackColor = System.Drawing.SystemColors.Menu;
            this.txtXMLRPCURL.Location = new System.Drawing.Point(695, 134);
            this.txtXMLRPCURL.Name = "txtXMLRPCURL";
            this.txtXMLRPCURL.Size = new System.Drawing.Size(218, 20);
            this.txtXMLRPCURL.TabIndex = 10;
            this.txtXMLRPCURL.Text = "http://10.0.255.255/rpc/xmlrpc";
            this.toolTip1.SetToolTip(this.txtXMLRPCURL, "Ensure the remote API is enabled on the confluence instance.  Provide the RPC con" +
        "nection URL in here.");
            // 
            // lblSpace
            // 
            this.lblSpace.AutoSize = true;
            this.lblSpace.Location = new System.Drawing.Point(692, 277);
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Size = new System.Drawing.Size(150, 13);
            this.lblSpace.TabIndex = 11;
            this.lblSpace.Text = "\'My Performance\' Page Name:";
            this.toolTip1.SetToolTip(this.lblSpace, "Provide the exact page name of the \'My Performance\' page.  All pages under here w" +
        "ill be processed if \'Process Pages in My Performance\' button is clicked.");
            // 
            // txtMyPerformancePageName
            // 
            this.txtMyPerformancePageName.BackColor = System.Drawing.SystemColors.Menu;
            this.txtMyPerformancePageName.Location = new System.Drawing.Point(696, 293);
            this.txtMyPerformancePageName.Name = "txtMyPerformancePageName";
            this.txtMyPerformancePageName.Size = new System.Drawing.Size(218, 20);
            this.txtMyPerformancePageName.TabIndex = 12;
            this.txtMyPerformancePageName.Text = "My Performance";
            this.toolTip1.SetToolTip(this.txtMyPerformancePageName, "Provide the exact page name of the \'My Performance\' page.  All pages under here w" +
        "ill be processed if \'Process Pages in My Performance\' button is clicked.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(692, 316);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "\'My Performance\' Section Archive Page Name:";
            this.toolTip1.SetToolTip(this.label3, "Not Enabled for this march 2013 version");
            // 
            // txtDestinationPageName
            // 
            this.txtDestinationPageName.BackColor = System.Drawing.SystemColors.Menu;
            this.txtDestinationPageName.Location = new System.Drawing.Point(696, 332);
            this.txtDestinationPageName.Name = "txtDestinationPageName";
            this.txtDestinationPageName.Size = new System.Drawing.Size(218, 20);
            this.txtDestinationPageName.TabIndex = 14;
            this.txtDestinationPageName.TabStop = false;
            this.txtDestinationPageName.Text = "**Archive";
            this.toolTip1.SetToolTip(this.txtDestinationPageName, "This is not enabled for this March 2013 version");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(693, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Space Key:";
            this.toolTip1.SetToolTip(this.label4, "Each \'Space\' in confluence has a \'space key\'.  Provide the key for the space all " +
        "HR stuff resides in.");
            // 
            // txtSpaceKey
            // 
            this.txtSpaceKey.BackColor = System.Drawing.SystemColors.Menu;
            this.txtSpaceKey.Location = new System.Drawing.Point(695, 173);
            this.txtSpaceKey.Name = "txtSpaceKey";
            this.txtSpaceKey.Size = new System.Drawing.Size(217, 20);
            this.txtSpaceKey.TabIndex = 16;
            this.txtSpaceKey.Text = "SPACE";
            this.toolTip1.SetToolTip(this.txtSpaceKey, "Each \'Space\' in confluence has a \'space key\'.  Provide the key for the space all " +
        "HR stuff resides in.");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(468, 316);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(188, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Prefix to be applied to archived pages:";
            this.toolTip1.SetToolTip(this.label5, "Prefix the old performance appraisals with this text.");
            // 
            // txtArchivePrefix
            // 
            this.txtArchivePrefix.Location = new System.Drawing.Point(471, 332);
            this.txtArchivePrefix.Name = "txtArchivePrefix";
            this.txtArchivePrefix.Size = new System.Drawing.Size(218, 20);
            this.txtArchivePrefix.TabIndex = 18;
            this.txtArchivePrefix.Text = "FY 2013 (6 months) - ";
            this.toolTip1.SetToolTip(this.txtArchivePrefix, "Prefix the old performance appraisals with this text.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(468, 277);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "MyPerf Template Location Page Name:";
            this.toolTip1.SetToolTip(this.label6, "Provide exact page name of the new performance appraisal template.  It has to exi" +
        "st somewhere in the same \'space\' as the other HR data.");
            // 
            // txtTemplateLocation
            // 
            this.txtTemplateLocation.Location = new System.Drawing.Point(471, 293);
            this.txtTemplateLocation.Name = "txtTemplateLocation";
            this.txtTemplateLocation.Size = new System.Drawing.Size(218, 20);
            this.txtTemplateLocation.TabIndex = 20;
            this.txtTemplateLocation.Text = "**Template - Employee Self-Appraisal Form";
            this.toolTip1.SetToolTip(this.txtTemplateLocation, "Provide exact page name of the new performance appraisal template.  It has to exi" +
        "st somewhere in the same \'space\' as the other HR data.");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(471, 559);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(187, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Names Excluded - (READ TOOL TIP):";
            this.toolTip1.SetToolTip(this.label7, "These names will be excluded.  Ensure ** always exists unless another \'template d" +
        "elimiter\' is used.  Ensure Archive is listed & ensure FY 20 is listed.");
            // 
            // btnMoveDeptPages
            // 
            this.btnMoveDeptPages.Enabled = false;
            this.btnMoveDeptPages.Location = new System.Drawing.Point(471, 9);
            this.btnMoveDeptPages.Name = "btnMoveDeptPages";
            this.btnMoveDeptPages.Size = new System.Drawing.Size(218, 73);
            this.btnMoveDeptPages.TabIndex = 23;
            this.btnMoveDeptPages.Text = "Process Pages in Departments Performance";
            this.btnMoveDeptPages.UseVisualStyleBackColor = true;
            this.btnMoveDeptPages.Click += new System.EventHandler(this.btnMoveDeptPages_Click);
            // 
            // txtExclude
            // 
            this.txtExclude.Location = new System.Drawing.Point(471, 580);
            this.txtExclude.Multiline = true;
            this.txtExclude.Name = "txtExclude";
            this.txtExclude.Size = new System.Drawing.Size(439, 120);
            this.txtExclude.TabIndex = 22;
            this.txtExclude.Text = "**\r\nFY 20\r\nArchive";
            this.toolTip1.SetToolTip(this.txtExclude, "These names will be excluded.  Ensure ** always exists unless another \'template d" +
        "elimiter\' is used.");
            this.txtExclude.TextChanged += new System.EventHandler(this.txtExclude_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(693, 236);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(210, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "\'My departments performance\' Page Name:";
            this.toolTip1.SetToolTip(this.label8, "Provide the exact page name of the \'My departments performance\' page.  All pages " +
        "under here will be processed if \'Process Pages in My departments performance\' bu" +
        "tton is clicked.");
            // 
            // txtMyDeptPerformancePageName
            // 
            this.txtMyDeptPerformancePageName.BackColor = System.Drawing.SystemColors.Menu;
            this.txtMyDeptPerformancePageName.Location = new System.Drawing.Point(695, 254);
            this.txtMyDeptPerformancePageName.Name = "txtMyDeptPerformancePageName";
            this.txtMyDeptPerformancePageName.Size = new System.Drawing.Size(218, 20);
            this.txtMyDeptPerformancePageName.TabIndex = 25;
            this.txtMyDeptPerformancePageName.Text = "My departments performance";
            this.toolTip1.SetToolTip(this.txtMyDeptPerformancePageName, "Provide the exact page name of the \'My departments performance\' page.  All pages " +
        "under here will be processed if \'Process Pages in My departments performance\' bu" +
        "tton is clicked.");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(692, 365);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Bonus % Template Page Name:";
            this.toolTip1.SetToolTip(this.label9, "Provide the exact page name of the new bonus % template");
            // 
            // txtBonusTemplatePageName
            // 
            this.txtBonusTemplatePageName.Location = new System.Drawing.Point(696, 381);
            this.txtBonusTemplatePageName.Name = "txtBonusTemplatePageName";
            this.txtBonusTemplatePageName.Size = new System.Drawing.Size(218, 20);
            this.txtBonusTemplatePageName.TabIndex = 27;
            this.txtBonusTemplatePageName.Text = "**Template - Bonus % Form";
            this.toolTip1.SetToolTip(this.txtBonusTemplatePageName, "Provide the exact page name of the new bonus % template");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(692, 413);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(166, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Objectives Template Page Name:";
            this.toolTip1.SetToolTip(this.label10, "Provide the exact page name of the new objectives template");
            // 
            // txtObjectives
            // 
            this.txtObjectives.Location = new System.Drawing.Point(696, 426);
            this.txtObjectives.Name = "txtObjectives";
            this.txtObjectives.Size = new System.Drawing.Size(218, 20);
            this.txtObjectives.TabIndex = 29;
            this.txtObjectives.Text = "**Template - Objectives";
            this.toolTip1.SetToolTip(this.txtObjectives, "Provide the exact page name of the new objectives template");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(468, 236);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(215, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "My Departments Perf Template Page Name:";
            this.toolTip1.SetToolTip(this.label11, "Provide exact page name of the new performance appraisal template.  It has to exi" +
        "st somewhere in the same \'space\' as the other HR data.");
            // 
            // txtMyDeptPerfTemplateLocation
            // 
            this.txtMyDeptPerfTemplateLocation.Location = new System.Drawing.Point(471, 254);
            this.txtMyDeptPerfTemplateLocation.Name = "txtMyDeptPerfTemplateLocation";
            this.txtMyDeptPerfTemplateLocation.Size = new System.Drawing.Size(218, 20);
            this.txtMyDeptPerfTemplateLocation.TabIndex = 31;
            this.txtMyDeptPerfTemplateLocation.Text = "**Template - Performance Appraisal Form (end of FY)";
            this.toolTip1.SetToolTip(this.txtMyDeptPerfTemplateLocation, "Provide exact page name of the new performance appraisal template.  It has to exi" +
        "st somewhere in the same \'space\' as the other HR data.");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(713, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(165, 20);
            this.label12.TabIndex = 32;
            this.label12.Text = "Advanced Settings:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkProcessPerformanceAppraisals
            // 
            this.chkProcessPerformanceAppraisals.AutoSize = true;
            this.chkProcessPerformanceAppraisals.Checked = true;
            this.chkProcessPerformanceAppraisals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProcessPerformanceAppraisals.Location = new System.Drawing.Point(471, 216);
            this.chkProcessPerformanceAppraisals.Name = "chkProcessPerformanceAppraisals";
            this.chkProcessPerformanceAppraisals.Size = new System.Drawing.Size(357, 17);
            this.chkProcessPerformanceAppraisals.TabIndex = 33;
            this.chkProcessPerformanceAppraisals.Text = "Replace performance appraisals with template and archive old pages?";
            this.chkProcessPerformanceAppraisals.UseVisualStyleBackColor = true;
            // 
            // chkProcessBonus
            // 
            this.chkProcessBonus.AutoSize = true;
            this.chkProcessBonus.Location = new System.Drawing.Point(474, 381);
            this.chkProcessBonus.Name = "chkProcessBonus";
            this.chkProcessBonus.Size = new System.Drawing.Size(220, 17);
            this.chkProcessBonus.TabIndex = 34;
            this.chkProcessBonus.Text = "Replace \'bonus %\' pages? (No archiving)";
            this.toolTip1.SetToolTip(this.chkProcessBonus, "Do you want to replace the bonus% pages?");
            this.chkProcessBonus.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.Tag = "";
            // 
            // chkProcessObjectives
            // 
            this.chkProcessObjectives.AutoSize = true;
            this.chkProcessObjectives.Location = new System.Drawing.Point(474, 428);
            this.chkProcessObjectives.Name = "chkProcessObjectives";
            this.chkProcessObjectives.Size = new System.Drawing.Size(221, 17);
            this.chkProcessObjectives.TabIndex = 36;
            this.chkProcessObjectives.Text = "Replace \'Objectives\' Pages? (no archive)";
            this.toolTip1.SetToolTip(this.chkProcessObjectives, "Do you want to replace the objectives pages? Note: Existing objectives pages shou" +
        "ld exist");
            this.chkProcessObjectives.UseVisualStyleBackColor = true;
            this.chkProcessObjectives.CheckedChanged += new System.EventHandler(this.chkProcessObjectives_CheckedChanged);
            // 
            // chkCreateObjectives
            // 
            this.chkCreateObjectives.AutoSize = true;
            this.chkCreateObjectives.Location = new System.Drawing.Point(474, 451);
            this.chkCreateObjectives.Name = "chkCreateObjectives";
            this.chkCreateObjectives.Size = new System.Drawing.Size(219, 17);
            this.chkCreateObjectives.TabIndex = 37;
            this.chkCreateObjectives.Text = "Create \'Objectives\' Pages from template?";
            this.toolTip1.SetToolTip(this.chkCreateObjectives, "Do you want to create new objectives pages from the template? Do not use this opt" +
        "ion is objectives pages already exist.  This is an advanced setting.");
            this.chkCreateObjectives.UseVisualStyleBackColor = true;
            this.chkCreateObjectives.CheckedChanged += new System.EventHandler(this.chkCreateObjectives_CheckedChanged);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(700, 9);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(211, 72);
            this.btnExit.TabIndex = 35;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // chkArchivePDP
            // 
            this.chkArchivePDP.AutoSize = true;
            this.chkArchivePDP.Location = new System.Drawing.Point(474, 487);
            this.chkArchivePDP.Name = "chkArchivePDP";
            this.chkArchivePDP.Size = new System.Drawing.Size(215, 17);
            this.chkArchivePDP.TabIndex = 38;
            this.chkArchivePDP.Text = "Archive PDP Page  (No template import)";
            this.chkArchivePDP.UseVisualStyleBackColor = true;
            // 
            // UI_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 713);
            this.Controls.Add(this.chkArchivePDP);
            this.Controls.Add(this.chkCreateObjectives);
            this.Controls.Add(this.chkProcessObjectives);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.chkProcessBonus);
            this.Controls.Add(this.chkProcessPerformanceAppraisals);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtMyDeptPerfTemplateLocation);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtObjectives);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBonusTemplatePageName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtMyDeptPerformancePageName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnMoveDeptPages);
            this.Controls.Add(this.txtExclude);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTemplateLocation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtArchivePrefix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSpaceKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDestinationPageName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMyPerformancePageName);
            this.Controls.Add(this.lblSpace);
            this.Controls.Add(this.txtXMLRPCURL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblSettings);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnMovePages);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnLogin);
            this.Name = "UI_Main";
            this.ShowIcon = false;
            this.Text = "SSA\'s Confluence Performance Management Page Cycle Tool March 2013";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnMovePages;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblSettings;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtXMLRPCURL;
        private System.Windows.Forms.Label lblSpace;
        private System.Windows.Forms.TextBox txtMyPerformancePageName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDestinationPageName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpaceKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtArchivePrefix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTemplateLocation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnMoveDeptPages;
        private System.Windows.Forms.TextBox txtExclude;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMyDeptPerformancePageName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtBonusTemplatePageName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtObjectives;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMyDeptPerfTemplateLocation;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkProcessPerformanceAppraisals;
        private System.Windows.Forms.CheckBox chkProcessBonus;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox chkProcessObjectives;
        private System.Windows.Forms.CheckBox chkCreateObjectives;
        private System.Windows.Forms.CheckBox chkArchivePDP;
    }
}

