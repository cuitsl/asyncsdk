using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace eTerm.ASynClient {
    public partial class BaseForm : Office2007RibbonForm {
        public BaseForm() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        this.Text = string.Format(@"企业管理终端 V2.09382", @"");
                    }
                );
        }
    }
}
