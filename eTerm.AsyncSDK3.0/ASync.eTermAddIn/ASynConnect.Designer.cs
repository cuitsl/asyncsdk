namespace ASync.eTermAddIn {
    partial class ASynConnect {
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
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnSessionEdit = new DevComponents.DotNetBar.ButtonItem();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.btnDelete = new DevComponents.DotNetBar.ButtonItem();
            this.itemContainer2 = new DevComponents.DotNetBar.ItemContainer();
            this.btnInsert = new DevComponents.DotNetBar.ButtonItem();
            this.itemContainer4 = new DevComponents.DotNetBar.ItemContainer();
            this.btnDispose = new DevComponents.DotNetBar.ButtonItem();
            this.lstSession = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.PanelSession = new DevComponents.DotNetBar.TabControlPanel();
            this.ipLocalIp = new DevComponents.Editors.IpAddressInput();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radAddress = new System.Windows.Forms.RadioButton();
            this.radPassword = new System.Windows.Forms.RadioButton();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.btnClearTraffic = new DevComponents.DotNetBar.ButtonX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSingleSave = new DevComponents.DotNetBar.ButtonX();
            this.comboTree1 = new DevComponents.DotNetBar.Controls.ComboTree();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.txtOfficeCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.chkIsSsl = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.txtSIText = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.chkAutoSi = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkIsOpen = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.integerInput2 = new DevComponents.Editors.IntegerInput();
            this.txtPort = new DevComponents.Editors.IntegerInput();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtSessionName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxX2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.txtAddress = new DevComponents.DotNetBar.Controls.TextBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.PanelSession.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipLocalIp)).BeginInit();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.integerInput2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.PanelSession);
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(811, 509);
            this.tabControl1.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document;
            this.tabControl1.TabIndex = 1;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Tabs.Add(this.tabItem2);
            this.tabControl1.SelectedTabChanged += new DevComponents.DotNetBar.TabStrip.SelectedTabChangedEventHandler(this.tabControl1_SelectedTabChanged);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.contextMenuBar1);
            this.tabControlPanel1.Controls.Add(this.lstSession);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 23);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(811, 486);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            this.tabControlPanel1.Text = "用户管理";
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.contextMenuBar1.Location = new System.Drawing.Point(115, 85);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 27);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 1;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.AutoExpandOnClick = true;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnSessionEdit,
            this.itemContainer1,
            this.btnDelete,
            this.itemContainer2,
            this.btnInsert,
            this.itemContainer4,
            this.btnDispose});
            this.buttonItem1.Text = "配置管理";
            // 
            // btnSessionEdit
            // 
            this.btnSessionEdit.Enabled = false;
            this.btnSessionEdit.Image = global::ASync.eTermAddIn.Properties.Resources.Pencil3;
            this.btnSessionEdit.Name = "btnSessionEdit";
            this.btnSessionEdit.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlE);
            this.btnSessionEdit.Text = "修改配置（&E）";
            this.btnSessionEdit.Click += new System.EventHandler(this.btnSessionEdit_Click);
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.Class = "";
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.Text = "-";
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = global::ASync.eTermAddIn.Properties.Resources.DeleteRed;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlD);
            this.btnDelete.Text = "删除配置（&D）";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // itemContainer2
            // 
            // 
            // 
            // 
            this.itemContainer2.BackgroundStyle.Class = "";
            this.itemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer2.Name = "itemContainer2";
            this.itemContainer2.Text = "-";
            // 
            // btnInsert
            // 
            this.btnInsert.Image = global::ASync.eTermAddIn.Properties.Resources.User1;
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlN);
            this.btnInsert.Text = "新增配置（&N）";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // itemContainer4
            // 
            // 
            // 
            // 
            this.itemContainer4.BackgroundStyle.Class = "";
            this.itemContainer4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer4.Name = "itemContainer4";
            // 
            // btnDispose
            // 
            this.btnDispose.Enabled = false;
            this.btnDispose.Image = global::ASync.eTermAddIn.Properties.Resources.Compass;
            this.btnDispose.Name = "btnDispose";
            this.btnDispose.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlO);
            this.btnDispose.Text = "打开连接（&O）";
            this.btnDispose.Click += new System.EventHandler(this.btnDispose_Click);
            // 
            // lstSession
            // 
            // 
            // 
            // 
            this.lstSession.Border.Class = "ListViewBorder";
            this.lstSession.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstSession.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader8,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader7,
            this.columnHeader5,
            this.columnHeader6});
            this.contextMenuBar1.SetContextMenuEx(this.lstSession, this.buttonItem1);
            this.lstSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSession.FullRowSelect = true;
            this.lstSession.Location = new System.Drawing.Point(1, 1);
            this.lstSession.Name = "lstSession";
            this.lstSession.Size = new System.Drawing.Size(809, 484);
            this.lstSession.TabIndex = 0;
            this.lstSession.UseCompatibleStateImageBehavior = false;
            this.lstSession.View = System.Windows.Forms.View.Details;
            this.lstSession.SelectedIndexChanged += new System.EventHandler(this.lstSession_SelectedIndexChanged);
            this.lstSession.DoubleClick += new System.EventHandler(this.lstSession_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 0;
            this.columnHeader1.Text = "登录帐号";
            this.columnHeader1.Width = 71;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 1;
            this.columnHeader2.Text = "主机地址";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 122;
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 2;
            this.columnHeader3.Text = "是否安全连接";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 90;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 3;
            this.columnHeader4.Text = "是否启用";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 73;
            // 
            // columnHeader7
            // 
            this.columnHeader7.DisplayIndex = 4;
            this.columnHeader7.Text = "SI";
            this.columnHeader7.Width = 85;
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 5;
            this.columnHeader5.Text = "OfficeCode";
            this.columnHeader5.Width = 76;
            // 
            // columnHeader6
            // 
            this.columnHeader6.DisplayIndex = 6;
            this.columnHeader6.Text = "所属分组";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.DisplayIndex = 7;
            this.columnHeader8.Text = "认证方式";
            this.columnHeader8.Width = 95;
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "配置管理";
            // 
            // PanelSession
            // 
            this.PanelSession.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.PanelSession.Controls.Add(this.txtAddress);
            this.PanelSession.Controls.Add(this.textBoxX2);
            this.PanelSession.Controls.Add(this.textBoxX1);
            this.PanelSession.Controls.Add(this.ipLocalIp);
            this.PanelSession.Controls.Add(this.labelX15);
            this.PanelSession.Controls.Add(this.panel1);
            this.PanelSession.Controls.Add(this.labelX14);
            this.PanelSession.Controls.Add(this.btnClearTraffic);
            this.PanelSession.Controls.Add(this.labelX13);
            this.PanelSession.Controls.Add(this.labelX12);
            this.PanelSession.Controls.Add(this.flowLayoutPanel1);
            this.PanelSession.Controls.Add(this.comboTree1);
            this.PanelSession.Controls.Add(this.labelX11);
            this.PanelSession.Controls.Add(this.comboBoxEx1);
            this.PanelSession.Controls.Add(this.labelX7);
            this.PanelSession.Controls.Add(this.txtOfficeCode);
            this.PanelSession.Controls.Add(this.labelX6);
            this.PanelSession.Controls.Add(this.chkIsSsl);
            this.PanelSession.Controls.Add(this.labelX10);
            this.PanelSession.Controls.Add(this.labelX9);
            this.PanelSession.Controls.Add(this.labelX8);
            this.PanelSession.Controls.Add(this.txtSIText);
            this.PanelSession.Controls.Add(this.labelX5);
            this.PanelSession.Controls.Add(this.chkAutoSi);
            this.PanelSession.Controls.Add(this.chkIsOpen);
            this.PanelSession.Controls.Add(this.labelX4);
            this.PanelSession.Controls.Add(this.integerInput2);
            this.PanelSession.Controls.Add(this.txtPort);
            this.PanelSession.Controls.Add(this.labelX3);
            this.PanelSession.Controls.Add(this.txtPassword);
            this.PanelSession.Controls.Add(this.labelX2);
            this.PanelSession.Controls.Add(this.txtSessionName);
            this.PanelSession.Controls.Add(this.labelX1);
            this.PanelSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSession.Enabled = false;
            this.PanelSession.Location = new System.Drawing.Point(0, 23);
            this.PanelSession.Name = "PanelSession";
            this.PanelSession.Padding = new System.Windows.Forms.Padding(1);
            this.PanelSession.Size = new System.Drawing.Size(811, 486);
            this.PanelSession.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.PanelSession.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.PanelSession.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.PanelSession.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.PanelSession.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.PanelSession.Style.GradientAngle = 90;
            this.PanelSession.TabIndex = 2;
            this.PanelSession.TabItem = this.tabItem2;
            // 
            // ipLocalIp
            // 
            this.ipLocalIp.AutoOverwrite = true;
            // 
            // 
            // 
            this.ipLocalIp.BackgroundStyle.Class = "DateTimeInputBackground";
            this.ipLocalIp.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ipLocalIp.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.ipLocalIp.ButtonFreeText.Visible = true;
            this.ipLocalIp.Location = new System.Drawing.Point(96, 51);
            this.ipLocalIp.Name = "ipLocalIp";
            this.ipLocalIp.Size = new System.Drawing.Size(137, 20);
            this.ipLocalIp.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ipLocalIp.TabIndex = 33;
            this.ipLocalIp.Visible = false;
            this.ipLocalIp.WatermarkText = "认证本地IP";
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX15.BackgroundStyle.Class = "";
            this.labelX15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX15.Location = new System.Drawing.Point(16, 53);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(79, 16);
            this.labelX15.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX15.TabIndex = 32;
            this.labelX15.Text = "本地绑定IP：";
            this.labelX15.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.radAddress);
            this.panel1.Controls.Add(this.radPassword);
            this.panel1.Location = new System.Drawing.Point(96, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 36);
            this.panel1.TabIndex = 31;
            // 
            // radAddress
            // 
            this.radAddress.AutoSize = true;
            this.radAddress.Location = new System.Drawing.Point(96, 11);
            this.radAddress.Name = "radAddress";
            this.radAddress.Size = new System.Drawing.Size(73, 17);
            this.radAddress.TabIndex = 1;
            this.radAddress.Text = "地址认证";
            this.radAddress.UseVisualStyleBackColor = true;
            this.radAddress.CheckedChanged += new System.EventHandler(this.radAddress_CheckedChanged);
            // 
            // radPassword
            // 
            this.radPassword.AutoSize = true;
            this.radPassword.Checked = true;
            this.radPassword.Location = new System.Drawing.Point(6, 11);
            this.radPassword.Name = "radPassword";
            this.radPassword.Size = new System.Drawing.Size(73, 17);
            this.radPassword.TabIndex = 0;
            this.radPassword.TabStop = true;
            this.radPassword.Text = "用户认证";
            this.radPassword.UseVisualStyleBackColor = true;
            this.radPassword.CheckedChanged += new System.EventHandler(this.radPassword_CheckedChanged);
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.Class = "";
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.Location = new System.Drawing.Point(27, 20);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(68, 16);
            this.labelX14.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX14.TabIndex = 30;
            this.labelX14.Text = "认证方式：";
            // 
            // btnClearTraffic
            // 
            this.btnClearTraffic.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClearTraffic.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnClearTraffic.Location = new System.Drawing.Point(436, 237);
            this.btnClearTraffic.Name = "btnClearTraffic";
            this.btnClearTraffic.Size = new System.Drawing.Size(78, 23);
            this.btnClearTraffic.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnClearTraffic.TabIndex = 29;
            this.btnClearTraffic.Text = "清除（&D）";
            this.btnClearTraffic.Click += new System.EventHandler(this.btnClearTraffic_Click);
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.Class = "";
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Location = new System.Drawing.Point(378, 170);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(56, 16);
            this.labelX13.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX13.TabIndex = 28;
            this.labelX13.Text = "月流量：";
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            this.labelX12.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.Class = "";
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Location = new System.Drawing.Point(380, 134);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(54, 16);
            this.labelX12.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX12.TabIndex = 28;
            this.labelX12.Text = "自动SI：";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.btnSingleSave);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1, 440);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(809, 45);
            this.flowLayoutPanel1.TabIndex = 27;
            // 
            // btnSingleSave
            // 
            this.btnSingleSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSingleSave.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnSingleSave.Location = new System.Drawing.Point(713, 3);
            this.btnSingleSave.Name = "btnSingleSave";
            this.btnSingleSave.Size = new System.Drawing.Size(93, 23);
            this.btnSingleSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSingleSave.TabIndex = 11;
            this.btnSingleSave.Text = "保存配置（&U）";
            this.btnSingleSave.Click += new System.EventHandler(this.btnSingleSave_Click);
            // 
            // comboTree1
            // 
            this.comboTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.comboTree1.BackgroundStyle.Class = "TextBoxBorder";
            this.comboTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.comboTree1.ButtonDropDown.Visible = true;
            this.comboTree1.Location = new System.Drawing.Point(96, 237);
            this.comboTree1.Name = "comboTree1";
            this.comboTree1.Size = new System.Drawing.Size(334, 23);
            this.comboTree1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboTree1.TabIndex = 26;
            this.comboTree1.WatermarkText = "当前月份流理不可清理";
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
            this.labelX11.Location = new System.Drawing.Point(28, 237);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(68, 16);
            this.labelX11.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX11.TabIndex = 25;
            this.labelX11.Text = "指令流量：";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Text";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 14;
            this.comboBoxEx1.Location = new System.Drawing.Point(96, 209);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(142, 20);
            this.comboBoxEx1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxEx1.TabIndex = 24;
            this.comboBoxEx1.WatermarkText = "授权可用方可用";
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
            this.labelX7.Location = new System.Drawing.Point(27, 209);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(68, 16);
            this.labelX7.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX7.TabIndex = 23;
            this.labelX7.Text = "所属分组：";
            // 
            // txtOfficeCode
            // 
            // 
            // 
            // 
            this.txtOfficeCode.Border.Class = "TextBoxBorder";
            this.txtOfficeCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtOfficeCode.Location = new System.Drawing.Point(96, 170);
            this.txtOfficeCode.MaxLength = 16;
            this.txtOfficeCode.Name = "txtOfficeCode";
            this.txtOfficeCode.Size = new System.Drawing.Size(142, 20);
            this.txtOfficeCode.TabIndex = 22;
            this.txtOfficeCode.WatermarkText = "配置Office号";
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
            this.labelX6.Location = new System.Drawing.Point(20, 170);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(75, 16);
            this.labelX6.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX6.TabIndex = 21;
            this.labelX6.Text = "OfficeCode：";
            // 
            // chkIsSsl
            // 
            this.chkIsSsl.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsSsl.BackgroundStyle.Class = "";
            this.chkIsSsl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsSsl.Location = new System.Drawing.Point(96, 104);
            this.chkIsSsl.Name = "chkIsSsl";
            this.chkIsSsl.Size = new System.Drawing.Size(58, 23);
            this.chkIsSsl.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkIsSsl.TabIndex = 19;
            this.chkIsSsl.Text = "是/否";
            // 
            // labelX10
            // 
            this.labelX10.AutoSize = true;
            this.labelX10.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.Class = "";
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.Location = new System.Drawing.Point(239, 137);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(38, 16);
            this.labelX10.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX10.TabIndex = 18;
            this.labelX10.Text = "RID：";
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.Class = "";
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Location = new System.Drawing.Point(239, 111);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(38, 16);
            this.labelX9.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX9.TabIndex = 18;
            this.labelX9.Text = "SID：";
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
            this.labelX8.Location = new System.Drawing.Point(27, 107);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(68, 16);
            this.labelX8.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX8.TabIndex = 18;
            this.labelX8.Text = "安全连接：";
            // 
            // txtSIText
            // 
            // 
            // 
            // 
            this.txtSIText.Border.Class = "TextBoxBorder";
            this.txtSIText.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSIText.Location = new System.Drawing.Point(96, 271);
            this.txtSIText.Multiline = true;
            this.txtSIText.Name = "txtSIText";
            this.txtSIText.Size = new System.Drawing.Size(492, 79);
            this.txtSIText.TabIndex = 9;
            this.txtSIText.WatermarkText = "区分用户用途";
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
            this.labelX5.Location = new System.Drawing.Point(65, 271);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(29, 16);
            this.labelX5.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX5.TabIndex = 8;
            this.labelX5.Text = "SI：";
            // 
            // chkAutoSi
            // 
            this.chkAutoSi.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkAutoSi.BackgroundStyle.Class = "";
            this.chkAutoSi.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkAutoSi.Location = new System.Drawing.Point(440, 134);
            this.chkAutoSi.Name = "chkAutoSi";
            this.chkAutoSi.Size = new System.Drawing.Size(58, 23);
            this.chkAutoSi.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkAutoSi.TabIndex = 7;
            this.chkAutoSi.Text = "是/否";
            // 
            // chkIsOpen
            // 
            this.chkIsOpen.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkIsOpen.BackgroundStyle.Class = "";
            this.chkIsOpen.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkIsOpen.Location = new System.Drawing.Point(96, 134);
            this.chkIsOpen.Name = "chkIsOpen";
            this.chkIsOpen.Size = new System.Drawing.Size(58, 23);
            this.chkIsOpen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkIsOpen.TabIndex = 7;
            this.chkIsOpen.Text = "是/否";
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
            this.labelX4.Location = new System.Drawing.Point(27, 137);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(68, 16);
            this.labelX4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "配置状态：";
            // 
            // integerInput2
            // 
            // 
            // 
            // 
            this.integerInput2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.integerInput2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.integerInput2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.integerInput2.Location = new System.Drawing.Point(438, 170);
            this.integerInput2.MaxValue = 100000;
            this.integerInput2.MinValue = 0;
            this.integerInput2.Name = "integerInput2";
            this.integerInput2.ShowUpDown = true;
            this.integerInput2.Size = new System.Drawing.Size(87, 20);
            this.integerInput2.TabIndex = 5;
            this.integerInput2.WatermarkText = "月可用指令数";
            // 
            // txtPort
            // 
            // 
            // 
            // 
            this.txtPort.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtPort.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPort.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtPort.Location = new System.Drawing.Point(260, 79);
            this.txtPort.MaxValue = 10000;
            this.txtPort.MinValue = 2;
            this.txtPort.Name = "txtPort";
            this.txtPort.ShowUpDown = true;
            this.txtPort.Size = new System.Drawing.Size(49, 20);
            this.txtPort.TabIndex = 5;
            this.txtPort.Value = 350;
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
            this.labelX3.Location = new System.Drawing.Point(15, 79);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(81, 16);
            this.labelX3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "服务器地址：";
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.Border.Class = "TextBoxBorder";
            this.txtPassword.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtPassword.Location = new System.Drawing.Point(329, 53);
            this.txtPassword.MaxLength = 16;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(142, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.WatermarkText = "配置密码";
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
            this.labelX2.Location = new System.Drawing.Point(260, 55);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 16);
            this.labelX2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "配置密码：";
            // 
            // txtSessionName
            // 
            // 
            // 
            // 
            this.txtSessionName.Border.Class = "TextBoxBorder";
            this.txtSessionName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSessionName.Location = new System.Drawing.Point(96, 51);
            this.txtSessionName.MaxLength = 16;
            this.txtSessionName.Name = "txtSessionName";
            this.txtSessionName.Size = new System.Drawing.Size(142, 20);
            this.txtSessionName.TabIndex = 1;
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
            this.labelX1.Location = new System.Drawing.Point(27, 52);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 16);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "配置帐号：";
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.PanelSession;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "配置信息修改";
            // 
            // textBoxX1
            // 
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.Location = new System.Drawing.Point(271, 111);
            this.textBoxX1.MaxLength = 2;
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(38, 20);
            this.textBoxX1.TabIndex = 34;
            this.textBoxX1.WatermarkText = "SID";
            // 
            // textBoxX2
            // 
            // 
            // 
            // 
            this.textBoxX2.Border.Class = "TextBoxBorder";
            this.textBoxX2.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX2.Location = new System.Drawing.Point(271, 137);
            this.textBoxX2.MaxLength = 2;
            this.textBoxX2.Name = "textBoxX2";
            this.textBoxX2.Size = new System.Drawing.Size(38, 20);
            this.textBoxX2.TabIndex = 34;
            this.textBoxX2.WatermarkText = "RID";
            // 
            // columnHeader9
            // 
            this.columnHeader9.DisplayIndex = 8;
            this.columnHeader9.Text = "编号";
            // 
            // txtAddress
            // 
            // 
            // 
            // 
            this.txtAddress.Border.Class = "TextBoxBorder";
            this.txtAddress.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtAddress.Location = new System.Drawing.Point(96, 78);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(142, 20);
            this.txtAddress.TabIndex = 35;
            this.txtAddress.WatermarkText = "域名或IP";
            // 
            // ASynConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "ASynConnect";
            this.Size = new System.Drawing.Size(811, 509);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.PanelSession.ResumeLayout(false);
            this.PanelSession.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ipLocalIp)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.integerInput2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.TabControl tabControl1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private DevComponents.DotNetBar.TabControlPanel PanelSession;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private DevComponents.DotNetBar.Controls.ListViewEx lstSession;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem btnSessionEdit;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.ButtonItem btnDelete;
        private DevComponents.DotNetBar.ItemContainer itemContainer2;
        private DevComponents.DotNetBar.ButtonItem btnInsert;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSessionName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassword;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.IntegerInput txtPort;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsOpen;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSIText;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsSsl;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.ItemContainer itemContainer4;
        private DevComponents.DotNetBar.ButtonItem btnDispose;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOfficeCode;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.Editors.IntegerInput integerInput2;
        private DevComponents.DotNetBar.Controls.ComboTree comboTree1;
        private DevComponents.DotNetBar.LabelX labelX11;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevComponents.DotNetBar.ButtonX btnSingleSave;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAutoSi;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.ButtonX btnClearTraffic;
        private DevComponents.DotNetBar.LabelX labelX14;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radAddress;
        private System.Windows.Forms.RadioButton radPassword;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.Editors.IpAddressInput ipLocalIp;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX2;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtAddress;

    }
}
