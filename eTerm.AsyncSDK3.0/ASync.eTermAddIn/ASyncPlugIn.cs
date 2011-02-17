using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using eTerm.AsyncSDK;
using eTerm.AsyncSDK.Net;
using eTerm.AsyncSDK.Base;
using eTerm.AsyncSDK.Core;
using ASync.MiddleWare;

namespace ASync.eTermAddIn {
    public partial class ASyncPlugIn : BaseAddIn {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASyncPlugIn"/> class.
        /// </summary>
        public ASyncPlugIn() {
            InitializeComponent();
            this.Load += new EventHandler(
                    delegate(object sender, EventArgs e) {
                        QueryPlugIn();
                    }
                );
        }

        /// <summary>
        /// 查询插件.
        /// </summary>
        private void QueryPlugIn() {
            foreach (FileInfo file in new DirectoryInfo(@".\").GetFiles(@"*.PlugIn", SearchOption.TopDirectoryOnly)) {
                LoadPlugIn(file);
            }
        }

       

        /// <summary>
        /// Loads the plug in.
        /// </summary>
        /// <param name="File">The file.</param>
        private void LoadPlugIn(FileInfo File) {
            Assembly ass;
            try {
                ass = Assembly.LoadFrom(File.FullName);
                foreach (Type t in ass.GetTypes()) {
                    foreach (Type i in t.GetInterfaces()) {
                        if (i.FullName == typeof(IAfterCommand<eTerm443Async, eTerm443Packet>).FullName) {
                            IAfterCommand<eTerm443Async, eTerm443Packet> plugIn = (IAfterCommand<eTerm443Async, eTerm443Packet>)System.Activator.CreateInstance(t);
                            object[] attris = t.GetCustomAttributes(typeof(AfterASynCommandAttribute), true);
                            SinglePlugIn PlugInCtr = new SinglePlugIn() {
                                FullName = t.FullName,
                                Title = plugIn.Description
                            };
                            foreach (AfterASynCommandAttribute att in attris) {
                                PlugInCtr.Append(att.ASynCommand);
                            }
                            this.flowLayoutBack.Controls.Add(PlugInCtr);
                        }
                        else if (i.FullName == typeof(IAfterCommand<eTerm363Session, eTerm363Packet>).FullName) {
                            IAfterCommand<eTerm363Session, eTerm363Packet> plugIn = (IAfterCommand<eTerm363Session, eTerm363Packet>)System.Activator.CreateInstance(t);
                            object[] attris = t.GetCustomAttributes(typeof(AfterASynCommandAttribute), true);
                            SinglePlugIn PlugInCtr = new SinglePlugIn() {
                                FullName = t.FullName,
                                Title = plugIn.Description
                            };
                            foreach (AfterASynCommandAttribute att in attris) {
                                PlugInCtr.Append(att.ASynCommand);
                            }
                            this.flowLayoutInter.Controls.Add(PlugInCtr);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Gets the name of the button.
        /// </summary>
        /// <value>The name of the button.</value>
        public override string ButtonName {
            get { return "系统插件"; }
        }

        ///// <summary>
        ///// Gets the image icon.
        ///// </summary>
        ///// <value>The image icon.</value>
        public override string ImageIcon {
            get { return "Hourglass.png"; }
        }
    }
}
