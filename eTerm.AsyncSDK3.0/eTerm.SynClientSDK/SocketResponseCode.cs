using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eTerm.SynClientSDK.Base {
    public enum SocketResponseCode {
        UserError = 0,
        PassError = 1,
        WelCome = 2,
        LogIn = 3,
        SDKRequest = 4,
        StreamBody = 32,
        SyncEmpty = 33,
        SyncCmdError = 34,
        SyncPlugIn = 35,
        SyncPlugInError = 36,
        ErrorStream = 80,
        ErrorCmd = 81,
        NormalStream = 255,
    }
}
