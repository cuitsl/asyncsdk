using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using eTerm.AsyncSDK;
using ASync.MiddleWare;
using eTerm.AsyncSDK.Util;

namespace ASync.eTermAddIn {
    public partial class ASyncGroup : BaseAddIn {
        public ASyncGroup() {
            InitializeComponent();
            this.lstSession.SelectedIndexChanged += new EventHandler(
                delegate(object lst, EventArgs args){
                    if (lstSession.SelectedItems.Count != 1) {
                        btnDelete.Enabled = false;
                        btnUpdate.Enabled = false;
                        return;
                    }
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                    SDKGroup group = lstSession.SelectedItems[0].Tag as SDKGroup;
                    txtGroupName.Text = group.groupName; 
                }
            );
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        lstSession.Items.Clear();
                        if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null) return;
                        foreach (SDKGroup group in AsyncStackNet.Instance.ASyncSetup.GroupCollection) {
                            lstSession.Items.Add(new ListViewItem() { Text=group.groupName,Tag=group });
                        }
                    }
                );
        }

        /// <summary>
        /// Handles the Click event of the btnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnUpdate_Click(object sender, EventArgs e) {
            try {
                if (AsyncStackNet.Instance.ASyncSetup.GroupCollection == null)
                    AsyncStackNet.Instance.ASyncSetup.GroupCollection = new List<SDKGroup>();
                AsyncStackNet.Instance.ASyncSetup.GroupCollection.Add(new SDKGroup() {
                    groupCode = Guid.NewGuid().ToString(),
                    groupName = txtGroupName.Text.Trim()
                });
                AsyncStackNet.Instance.ASyncSetup.XmlSerialize(AsyncStackNet.Instance.CrypterKey, AsyncStackNet.Instance.ASyncSetupFile);
                MessageBox.Show(string.Format(@"分组已经添加！"), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.OnLoad(EventArgs.Empty);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in this.lstSession.Items) {
                if (!item.Checked) continue;
                AsyncStackNet.Instance.ASyncSetup.GroupCollection.Remove(item.Tag as SDKGroup);
            }
            AsyncStackNet.Instance.ASyncSetup.XmlSerialize(AsyncStackNet.Instance.CrypterKey, AsyncStackNet.Instance.ASyncSetupFile);
            MessageBox.Show(string.Format(@"分组删除成功！"), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.OnLoad(EventArgs.Empty);
        }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "系统分组管理"; }
        }

        /// <summary>
        /// Gets the image icon.
        /// </summary>
        /// <value>The image icon.</value>
        public override string ImageIcon {
            get { return "Hourglass.png"; }
        }
    }
}
