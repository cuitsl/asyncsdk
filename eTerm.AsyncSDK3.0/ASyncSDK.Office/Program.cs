using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ASyncSDK.Office {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DirectoryInfo SvrPath = new DirectoryInfo(@".\");
            DirectoryInfo UpdateFolder = new DirectoryInfo(string.Format(@"{0}{1}\", SvrPath.FullName, @"SvrUpdate"));
            if (UpdateFolder.Exists) {
                SvrOverrite(SvrPath, UpdateFolder);
            }
            Application.Run(new frmMain());
        }


        /// <summary>
        /// SVRs the overrite.
        /// </summary>
        /// <param name="SvrPath">The SVR path.</param>
        /// <param name="UpdateFolder">The update folder.</param>
        static private void SvrOverrite(DirectoryInfo SvrPath , DirectoryInfo UpdateFolder)
        {
            foreach (FileInfo file in UpdateFolder.GetFiles(@"*.*", SearchOption.AllDirectories)) {
                file.CopyTo(string.Format(@"{0}{1}", SvrPath, file.Name), true);
            }
            UpdateFolder.Delete(true);
        }
    }
}
