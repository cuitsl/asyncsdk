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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textEditorControlWrapper1 = new eTerm.ASyncActiveX.TextEditorControlWrapper();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(543, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textEditorControlWrapper1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(10, 60);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(624, 371);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "指令区";
            // 
            // textEditorControlWrapper1
            // 
            this.textEditorControlWrapper1.BackColor = System.Drawing.SystemColors.Control;
            this.textEditorControlWrapper1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorControlWrapper1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.textEditorControlWrapper1.IsReadOnly = false;
            this.textEditorControlWrapper1.Location = new System.Drawing.Point(3, 16);
            this.textEditorControlWrapper1.Name = "textEditorControlWrapper1";
            this.textEditorControlWrapper1.SelectedText = "";
            this.textEditorControlWrapper1.SelectionStart = 0;
            this.textEditorControlWrapper1.ShowLineNumbers = false;
            this.textEditorControlWrapper1.ShowMatchingBracket = false;
            this.textEditorControlWrapper1.ShowVRuler = false;
            this.textEditorControlWrapper1.Size = new System.Drawing.Size(618, 352);
            this.textEditorControlWrapper1.TabIndex = 0;
            // 
            // ASynClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ASynClient";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(644, 441);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private TextEditorControlWrapper textEditorControlWrapper1;
        private Button button1;
    }
}
