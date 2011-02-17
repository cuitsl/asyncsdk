using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using eTerm.AsyncSDK.Core;
using eTerm.AsyncSDK.Net;

namespace ASyncSDK.Office {
    public partial class frmSender : DevComponents.DotNetBar.Office2007Form {
        private List<eTerm363Session> SessionList;
        public frmSender(List<eTerm363Session> SessionList) {
            InitializeComponent();
            this.SessionList = SessionList;
        }

        private void btnSend_Click(object sender, EventArgs e) {
            foreach (eTerm363Session Session in this.SessionList) {
                Session.SendPacket(__eTerm443Packet.BuildSessionPacket(Session.SID, Session.RID, textBoxX1.Text.Trim()));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}