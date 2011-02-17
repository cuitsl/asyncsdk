namespace ASync.eTermBuilder {
    partial class SerialBuilder {
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
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBuilder = new System.Windows.Forms.Button();
            this.txtMaxTSession = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.txtASyncCount = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.txtConnectString = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProviderName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMinutes = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAllowInter = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAllowDb = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PickerExpire = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxTSession)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtASyncCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.txtMaxTSession);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtASyncCount);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtConnectString);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtProviderName);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.maskedTextBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtMinutes);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.chkAllowInter);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkAllowDb);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.PickerExpire);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCompany);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(649, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "授权文件生成器";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(131, 320);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(404, 20);
            this.txtFileName.TabIndex = 26;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(541, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "文件（&S）";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 320);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "目标文件：";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnBuilder);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 351);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(643, 45);
            this.flowLayoutPanel1.TabIndex = 24;
            // 
            // btnBuilder
            // 
            this.btnBuilder.Location = new System.Drawing.Point(565, 3);
            this.btnBuilder.Name = "btnBuilder";
            this.btnBuilder.Size = new System.Drawing.Size(75, 23);
            this.btnBuilder.TabIndex = 0;
            this.btnBuilder.Text = "生成（&B）";
            this.btnBuilder.UseVisualStyleBackColor = true;
            this.btnBuilder.Click += new System.EventHandler(this.btnBuilder_Click);
            // 
            // txtMaxTSession
            // 
            this.txtMaxTSession.Location = new System.Drawing.Point(131, 294);
            this.txtMaxTSession.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.txtMaxTSession.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMaxTSession.Name = "txtMaxTSession";
            this.txtMaxTSession.Size = new System.Drawing.Size(55, 20);
            this.txtMaxTSession.TabIndex = 23;
            this.txtMaxTSession.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 296);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "最大终端数：";
            // 
            // txtASyncCount
            // 
            this.txtASyncCount.Location = new System.Drawing.Point(131, 269);
            this.txtASyncCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.txtASyncCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtASyncCount.Name = "txtASyncCount";
            this.txtASyncCount.Size = new System.Drawing.Size(55, 20);
            this.txtASyncCount.TabIndex = 21;
            this.txtASyncCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 271);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "最大配置数：";
            // 
            // txtConnectString
            // 
            this.txtConnectString.Location = new System.Drawing.Point(131, 242);
            this.txtConnectString.Name = "txtConnectString";
            this.txtConnectString.Size = new System.Drawing.Size(455, 20);
            this.txtConnectString.TabIndex = 19;
            this.txtConnectString.Text = "Data Source=(local);Initial Catalog=Async;User ID=sa;Password=Password01!";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 245);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "连接字符串：";
            // 
            // txtProviderName
            // 
            this.txtProviderName.Location = new System.Drawing.Point(131, 217);
            this.txtProviderName.Name = "txtProviderName";
            this.txtProviderName.Size = new System.Drawing.Size(200, 20);
            this.txtProviderName.TabIndex = 17;
            this.txtProviderName.Text = "System.Data.SqlClient";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 220);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "ProviderName：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(192, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "条";
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(131, 187);
            this.maskedTextBox1.Mask = "00000";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(55, 20);
            this.maskedTextBox1.TabIndex = 14;
            this.maskedTextBox1.Text = "1000";
            this.maskedTextBox1.ValidatingType = typeof(int);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 187);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "配置流量（最大）：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(216, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "分钟";
            // 
            // txtMinutes
            // 
            this.txtMinutes.Location = new System.Drawing.Point(131, 118);
            this.txtMinutes.Name = "txtMinutes";
            this.txtMinutes.ReadOnly = true;
            this.txtMinutes.Size = new System.Drawing.Size(79, 20);
            this.txtMinutes.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "到期日期：";
            // 
            // chkAllowInter
            // 
            this.chkAllowInter.AutoSize = true;
            this.chkAllowInter.Location = new System.Drawing.Point(131, 164);
            this.chkAllowInter.Name = "chkAllowInter";
            this.chkAllowInter.Size = new System.Drawing.Size(55, 17);
            this.chkAllowInter.TabIndex = 9;
            this.chkAllowInter.Text = "是/否";
            this.chkAllowInter.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "允许指令拦截：";
            // 
            // chkAllowDb
            // 
            this.chkAllowDb.AutoSize = true;
            this.chkAllowDb.Location = new System.Drawing.Point(131, 142);
            this.chkAllowDb.Name = "chkAllowDb";
            this.chkAllowDb.Size = new System.Drawing.Size(55, 17);
            this.chkAllowDb.TabIndex = 7;
            this.chkAllowDb.Text = "是/否";
            this.chkAllowDb.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "允许数据库：";
            // 
            // PickerExpire
            // 
            this.PickerExpire.CustomFormat = "";
            this.PickerExpire.Location = new System.Drawing.Point(131, 90);
            this.PickerExpire.Name = "PickerExpire";
            this.PickerExpire.Size = new System.Drawing.Size(200, 20);
            this.PickerExpire.TabIndex = 5;
            this.PickerExpire.Value = new System.DateTime(2011, 1, 24, 0, 0, 0, 0);
            this.PickerExpire.ValueChanged += new System.EventHandler(this.PickerExpire_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "到期日期：";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(131, 61);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(200, 20);
            this.txtCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "机器码：";
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(131, 34);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(200, 20);
            this.txtCompany.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "使用者：";
            // 
            // SerialBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "SerialBuilder";
            this.Size = new System.Drawing.Size(649, 399);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMaxTSession)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtASyncCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker PickerExpire;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAllowDb;
        private System.Windows.Forms.CheckBox chkAllowInter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMinutes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtProviderName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtConnectString;
        private System.Windows.Forms.NumericUpDown txtASyncCount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown txtMaxTSession;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnBuilder;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
