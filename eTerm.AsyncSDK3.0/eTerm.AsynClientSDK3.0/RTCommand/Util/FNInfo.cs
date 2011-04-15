using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class FNInfo
    {
        public FNInfo()
        {
            FCNY = string.Empty;
            SCNY = string.Empty;
        }

        private String _FCNY;
        public String FCNY
        {
            set { _FCNY = value; }
            get { return _FCNY; }
        }

        private String _SCNY;
        public String SCNY
        {
            set { _SCNY = value; }
            get { return _SCNY; }
        }

        private String _C;
        public String C
        {
            set { _C = value; }
            get { return _C; }
        }

        private String _XCNY;
        public String XCNY
        {
            set { _XCNY = value; }
            get { return _XCNY; }
        }

        private String _ACNY;
        public String ACNY
        {
            set { _ACNY = value; }
            get { return _ACNY; }
        }

        private String[] _TCNY;
        public String[] TCNY
        {
            set { _TCNY = value; }
            get { return _TCNY; }
        }
    }
}
