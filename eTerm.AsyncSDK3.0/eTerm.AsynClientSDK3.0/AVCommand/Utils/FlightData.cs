using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class FlightData
    {
        public FlightData() { }

        private String _CMD = String.Empty;
        public String CMD
        {
            get { return _CMD; }
            set { _CMD = value; }
        }

        private String _ResultDate = String.Empty;
        public String ResultDate
        {
            get { return _ResultDate; }
            set { _ResultDate = value; }
        }

        private Flight[] _Flights = new Flight[] { };
        public Flight[] Flights
        {
            get { return _Flights; }
            set { _Flights = value; }
        }

        private String _ErrorMsg = String.Empty;
        public String ErrorMsg
        {
            get { return _ErrorMsg; }
            set { _ErrorMsg = value; }
        }

        private String _OriginalData = String.Empty;
        public String OriginalData
        {
            get { return _OriginalData; }
            set { _OriginalData = value; }
        }
    }
}
