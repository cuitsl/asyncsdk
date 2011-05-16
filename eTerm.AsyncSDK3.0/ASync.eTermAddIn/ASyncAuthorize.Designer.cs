namespace ASync.eTermAddIn {
    partial class ASyncAuthorize {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.txtRemainMinutes = new DevComponents.Editors.DoubleInput();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txtCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.checkBoxX1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkOpenDb = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtMaxSession = new DevComponents.Editors.IntegerInput();
            this.txtMaxASync = new DevComponents.Editors.IntegerInput();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtExpire = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtCompany = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.txtPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxSession)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxASync)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpire)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(554, 374);
            this.tabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Text = "系统授权";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.flowLayoutPanel1);
            this.tabControlPanel1.Controls.Add(this.labelX10);
            this.tabControlPanel1.Controls.Add(this.txtRemainMinutes);
            this.tabControlPanel1.Controls.Add(this.labelX9);
            this.tabControlPanel1.Controls.Add(this.txtCode);
            this.tabControlPanel1.Controls.Add(this.labelX8);
            this.tabControlPanel1.Controls.Add(this.checkBoxX1);
            this.tabControlPanel1.Controls.Add(this.chkOpenDb);
            this.tabControlPanel1.Controls.Add(this.txtMaxSession);
            this.tabControlPanel1.Controls.Add(this.txtMaxASync);
            this.tabControlPanel1.Controls.Add(this.labelX7);
            this.tabControlPanel1.Controls.Add(this.labelX6);
            this.tabControlPanel1.Controls.Add(this.labelX5);
            this.tabControlPanel1.Controls.Add(this.labelX4);
            this.tabControlPanel1.Controls.Add(this.txtExpire);
            this.tabControlPanel1.Controls.Add(this.labelX3);
            this.tabControlPanel1.Controls.Add(this.labelX11);
            this.tabControlPanel1.Controls.Add(this.txtCompany);
            this.tabControlPanel1.Controls.Add(this.labelX2);
            this.tabControlPanel1.Controls.Add(this.buttonX1);
            this.tabControlPanel1.Controls.Add(this.txtPath);
            this.tabControlPanel1.Controls.Add(this.labelX1);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 23);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(554, 351);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // labelX10
            // 
            this.labelX10.AutoSize = true;
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.Class = "";
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.Location = new System.Drawing.Point(521, 210);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(31, 16);
            this.labelX10.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX10.TabIndex = 19;
            this.labelX10.Text = "分钟";
            this.labelX10.Visible = false;
            // 
            // txtRemainMinutes
            // 
            // 
            // 
            // 
            this.txtRemainMinutes.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtRemainMinutes.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRemainMinutes.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtRemainMinutes.Enabled = false;
            this.txtRemainMinutes.Increment = 1;
            this.txtRemainMinutes.IsInputReadOnly = true;
            this.txtRemainMinutes.Location = new System.Drawing.Point(422, 208);
            this.txtRemainMinutes.Name = "txtRemainMinutes";
            this.txtRemainMinutes.ShowUpDown = true;
            this.txtRemainMinutes.Size = new System.Drawing.Size(93, 20);
            this.txtRemainMinutes.TabIndex = 18;
            this.txtRemainMinutes.Visible = false;
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.Class = "";
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Location = new System.Drawing.Point(344, 210);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(68, 16);
            this.labelX9.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX9.TabIndex = 17;
            this.labelX9.Text = "剩余时间：";
            this.labelX9.Visible = false;
            // 
            // txtCode
            // 
            // 
            // 
            // 
            this.txtCode.Border.Class = "TextBoxBorder";
            this.txtCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCode.Location = new System.Drawing.Point(123, 52);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(223, 20);
            this.txtCode.TabIndex = 16;
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.Class = "";
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Location = new System.Drawing.Point(57, 53);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(56, 16);
            this.labelX8.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX8.TabIndex = 15;
            this.labelX8.Text = "机器码：";
            // 
            // checkBoxX1
            // 
            // 
            // 
            // 
            this.checkBoxX1.BackgroundStyle.Class = "";
            this.checkBoxX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.checkBoxX1.Enabled = false;
            this.checkBoxX1.Location = new System.Drawing.Point(123, 260);
            this.checkBoxX1.Name = "checkBoxX1";
            this.checkBoxX1.Size = new System.Drawing.Size(60, 23);
            this.checkBoxX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.checkBoxX1.TabIndex = 13;
            this.checkBoxX1.Text = "是/否";
            // 
            // chkOpenDb
            // 
            // 
            // 
            // 
            this.chkOpenDb.BackgroundStyle.Class = "";
            this.chkOpenDb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkOpenDb.Enabled = false;
            this.chkOpenDb.Location = new System.Drawing.Point(123, 226);
            this.chkOpenDb.Name = "chkOpenDb";
            this.chkOpenDb.Size = new System.Drawing.Size(60, 23);
            this.chkOpenDb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkOpenDb.TabIndex = 13;
            this.chkOpenDb.Text = "是/否";
            // 
            // txtMaxSession
            // 
            // 
            // 
            // 
            this.txtMaxSession.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtMaxSession.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMaxSession.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtMaxSession.Enabled = false;
            this.txtMaxSession.IsInputReadOnly = true;
            this.txtMaxSession.Location = new System.Drawing.Point(123, 194);
            this.txtMaxSession.Name = "txtMaxSession";
            this.txtMaxSession.ShowUpDown = true;
            this.txtMaxSession.Size = new System.Drawing.Size(60, 20);
            this.txtMaxSession.TabIndex = 12;
            // 
            // txtMaxASync
            // 
            // 
            // 
            // 
            this.txtMaxASync.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtMaxASync.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtMaxASync.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtMaxASync.Enabled = false;
            this.txtMaxASync.IsInputReadOnly = true;
            this.txtMaxASync.Location = new System.Drawing.Point(123, 160);
            this.txtMaxASync.Name = "txtMaxASync";
            this.txtMaxASync.ShowUpDown = true;
            this.txtMaxASync.Size = new System.Drawing.Size(60, 20);
            this.txtMaxASync.TabIndex = 11;
            // 
            // labelX7
            // 
            this.labelX7.AutoSize = true;
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(45, 260);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(68, 16);
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX7.TabIndex = 9;
            this.labelX7.Text = "日志支持：";
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.Class = "";
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(9, 226);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(106, 16);
            this.labelX6.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX6.TabIndex = 9;
            this.labelX6.Text = "是否启用数据库：";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(33, 194);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(81, 16);
            this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX5.TabIndex = 8;
            this.labelX5.Text = "最大连接数：";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(33, 160);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(81, 16);
            this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX4.TabIndex = 7;
            this.labelX4.Text = "最大配置数：";
            // 
            // txtExpire
            // 
            // 
            // 
            // 
            this.txtExpire.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtExpire.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtExpire.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.txtExpire.ButtonDropDown.Visible = true;
            this.txtExpire.Enabled = false;
            this.txtExpire.IsInputReadOnly = true;
            this.txtExpire.Location = new System.Drawing.Point(123, 123);
            // 
            // 
            // 
            this.txtExpire.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.txtExpire.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.txtExpire.MonthCalendar.BackgroundStyle.Class = "";
            this.txtExpire.MonthCalendar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtExpire.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.Class = "";
            this.txtExpire.MonthCalendar.CommandsBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtExpire.MonthCalendar.DisplayMonth = new System.DateTime(2011, 1, 1, 0, 0, 0, 0);
            this.txtExpire.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.txtExpire.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.txtExpire.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.txtExpire.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.txtExpire.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.txtExpire.MonthCalendar.NavigationBackgroundStyle.Class = "";
            this.txtExpire.MonthCalendar.NavigationBackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtExpire.MonthCalendar.TodayButtonVisible = true;
            this.txtExpire.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.txtExpire.Name = "txtExpire";
            this.txtExpire.Size = new System.Drawing.Size(110, 20);
            this.txtExpire.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.txtExpire.TabIndex = 6;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(45, 125);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 16);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "到期日期：";
            // 
            // txtCompany
            // 
            // 
            // 
            // 
            this.txtCompany.Border.Class = "TextBoxBorder";
            this.txtCompany.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCompany.Location = new System.Drawing.Point(123, 85);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.ReadOnly = true;
            this.txtCompany.Size = new System.Drawing.Size(223, 20);
            this.txtCompany.TabIndex = 4;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(45, 87);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 16);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "公司名称：";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(353, 14);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlS);
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "选择（&S）";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // txtPath
            // 
            // 
            // 
            // 
            this.txtPath.Border.Class = "TextBoxBorder";
            this.txtPath.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPath.Location = new System.Drawing.Point(123, 18);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(223, 20);
            this.txtPath.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(45, 18);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 16);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "授权文件：";
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "系统授权";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            this.labelX11.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.Class = "";
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Location = new System.Drawing.Point(33, 303);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(79, 16);
            this.labelX11.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX11.TabIndex = 3;
            this.labelX11.Text = "SDK认证串：";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(118, 303);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(432, 34);
            this.flowLayoutPanel1.TabIndex = 20;
            // 
            // ASyncAuthorize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ASyncAuthorize";
            this.Size = new System.Drawing.Size(554, 374);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRemainMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxSession)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxASync)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpire)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPath;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCompany;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput txtExpire;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkOpenDb;
        private DevComponents.Editors.IntegerInput txtMaxSession;
        private DevComponents.Editors.IntegerInput txtMaxASync;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCode;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.Editors.DoubleInput txtRemainMinutes;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxX1;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX11;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
