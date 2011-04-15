using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class Seat
    {
        private String _CityPair = String.Empty;
        public String CityPair
        {
            get { return _CityPair; }
            set { _CityPair = value; }
        }

        private String _FlightNo = String.Empty;
        public String FlightNo
        {
            get { return _FlightNo; }
            set { _FlightNo = value; }
        }

        private String _Carbin = String.Empty;
        public String Carbin
        {
            get { return _Carbin; }
            set { _Carbin = value; }
        }

        private String _TripDate = String.Empty;
        public String TripDate
        {
            get { return _TripDate; }
            set { _TripDate = value; }
        }

        private String _SeatNo = String.Empty;
        public String SeatNo
        {
            get { return _SeatNo; }
            set { _SeatNo = value; }
        }

        private Int32 _PID = 0;
        public Int32 PID
        {
            get { return _PID; }
            set { _PID = value; }
        }

        public Seat() { }
    }
}
