using ICSharpCode.TextEditor.Document;
using System.Drawing;
using System.Windows.Forms;
namespace eTerm.ASyncActiveX {
    partial class ASynClient {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserPass = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkIsSsl = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textEditorControlWrapper1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.txtKeyStatus = new System.Windows.Forms.StatusStrip();
            this.txtRowNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtColumnNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtKeyBordStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.txtKeyStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(808, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 10;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtAddress, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPort, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtUserName, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblUserPass, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkIsSsl, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnConnect, 9, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(802, 31);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器：";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(67, 3);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(110, 20);
            this.txtAddress.TabIndex = 1;
            this.txtAddress.Text = "asyncsdk.gicp.net";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "端口号：";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(251, 3);
            this.txtPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(50, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(331, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户名：";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(395, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(74, 20);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Text = "guzm";
            // 
            // lblUserPass
            // 
            this.lblUserPass.AutoSize = true;
            this.lblUserPass.Location = new System.Drawing.Point(475, 0);
            this.lblUserPass.Name = "lblUserPass";
            this.lblUserPass.Size = new System.Drawing.Size(43, 13);
            this.lblUserPass.TabIndex = 0;
            this.lblUserPass.Text = "密码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(539, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(74, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "guzm";
            // 
            // chkIsSsl
            // 
            this.chkIsSsl.AutoSize = true;
            this.chkIsSsl.Location = new System.Drawing.Point(619, 3);
            this.chkIsSsl.Name = "chkIsSsl";
            this.chkIsSsl.Size = new System.Drawing.Size(50, 17);
            this.chkIsSsl.TabIndex = 4;
            this.chkIsSsl.Text = "安全";
            this.chkIsSsl.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.AutoSize = true;
            this.btnConnect.Location = new System.Drawing.Point(683, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(116, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "连接服务器(&C)";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textEditorControlWrapper1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(10, 60);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(808, 378);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "指令区";
            // 
            // textEditorControlWrapper1
            // 
            this.textEditorControlWrapper1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorControlWrapper1.ForeColor = System.Drawing.Color.Lime;
            this.textEditorControlWrapper1.IsReadOnly = false;
            this.textEditorControlWrapper1.Location = new System.Drawing.Point(3, 16);
            this.textEditorControlWrapper1.Name = "textEditorControlWrapper1";
            this.textEditorControlWrapper1.Size = new System.Drawing.Size(802, 359);
            this.textEditorControlWrapper1.TabIndex = 0;
            this.textEditorControlWrapper1.Text = "textEditorControl1";
            // 
            // txtKeyStatus
            // 
            this.txtKeyStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtRowNumber,
            this.txtColumnNumber,
            this.txtKeyBordStatus});
            this.txtKeyStatus.Location = new System.Drawing.Point(10, 416);
            this.txtKeyStatus.Name = "txtKeyStatus";
            this.txtKeyStatus.Size = new System.Drawing.Size(808, 22);
            this.txtKeyStatus.TabIndex = 2;
            // 
            // txtRowNumber
            // 
            this.txtRowNumber.Name = "txtRowNumber";
            this.txtRowNumber.Size = new System.Drawing.Size(0, 17);
            // 
            // txtColumnNumber
            // 
            this.txtColumnNumber.Name = "txtColumnNumber";
            this.txtColumnNumber.Size = new System.Drawing.Size(0, 17);
            // 
            // txtKeyBordStatus
            // 
            this.txtKeyBordStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.txtKeyBordStatus.ForeColor = System.Drawing.Color.Blue;
            this.txtKeyBordStatus.Name = "txtKeyBordStatus";
            this.txtKeyBordStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // ASynClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtKeyStatus);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ASynClient";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(828, 448);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.txtKeyStatus.ResumeLayout(false);
            this.txtKeyStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TextBox txtAddress;
        private Label label2;
        private NumericUpDown txtPort;
        private Label label3;
        private TextBox txtUserName;
        private Label lblUserPass;
        private TextBox txtPassword;
        private Button btnConnect;
        private CheckBox chkIsSsl;
        private StatusStrip txtKeyStatus;
        private ToolStripStatusLabel txtRowNumber;
        private ToolStripStatusLabel txtColumnNumber;
        private ToolStripStatusLabel txtKeyBordStatus;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControlWrapper1;
    }
}
