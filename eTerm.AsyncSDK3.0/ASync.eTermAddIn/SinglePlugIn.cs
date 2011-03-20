using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASync.eTermAddIn {
    public partial class SinglePlugIn : UserControl {
        private FlowLayoutPanel flowLayoutPanel1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblFullName;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
    
        public SinglePlugIn() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        expandablePanel1.TitleText = Title;
                        lblFullName.Text = FullName;
                        
                    }
                );
        }

        /// <summary>
        /// Appends the specified attr.
        /// </summary>
        /// <param name="Attr">The attr.</param>
        public void Append(string Attr) {
            this.listViewEx1.Items.Add(new ListViewItem() {  Text=Attr, ToolTipText=Attr});
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { private get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { private get; set; }

        private void InitializeComponent() {
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.lblFullName = new DevComponents.DotNetBar.LabelX();
            this.listViewEx1 = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.expandablePanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.AutoScroll = true;
            this.expandablePanel1.AutoSize = true;
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.flowLayoutPanel1);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expandablePanel1.ExpandOnTitleClick = true;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 0);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(319, 184);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 0;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "Title Bar";
            this.expandablePanel1.ExpandedChanged += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandablePanel1_ExpandedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.labelX1);
            this.flowLayoutPanel1.Controls.Add(this.lblFullName);
            this.flowLayoutPanel1.Controls.Add(this.listViewEx1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 26);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(319, 158);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(3, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(91, 23);
            this.labelX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "TypeFullName：";
            // 
            // lblFullName
            // 
            // 
            // 
            // 
            this.lblFullName.BackgroundStyle.Class = "";
            this.lblFullName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFullName.Location = new System.Drawing.Point(100, 3);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(216, 23);
            this.lblFullName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblFullName.TabIndex = 7;
            // 
            // listViewEx1
            // 
            // 
            // 
            // 
            this.listViewEx1.Border.Class = "ListViewBorder";
            this.listViewEx1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.flowLayoutPanel1.SetFlowBreak(this.listViewEx1, true);
            this.listViewEx1.Location = new System.Drawing.Point(3, 32);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.ShowItemToolTips = true;
            this.listViewEx1.Size = new System.Drawing.Size(313, 123);
            this.listViewEx1.TabIndex = 8;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            // 
            // SinglePlugIn
            // 
            this.Controls.Add(this.expandablePanel1);
            this.Name = "SinglePlugIn";
            this.Size = new System.Drawing.Size(319, 184);
            this.expandablePanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Handles the ExpandedChanged event of the expandablePanel1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DevComponents.DotNetBar.ExpandedChangeEventArgs"/> instance containing the event data.</param>
        private void expandablePanel1_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e) {
            this.Size = expandablePanel1.Size;
        }
    }
}
