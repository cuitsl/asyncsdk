using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class Segment
    {
        public Segment() { }

        private Int32 _ID;
        public Int32 ID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        private String _Airline;
        public String Airline
        {
            set { _Airline = value; }
            get { return _Airline; }
        }

        private String _FltNO;
        public String FltNo
        {
            set { _FltNO = value; }
            get { return _FltNO; }
        }

        private String _Carbin;
        public String Carbin
        {
            set { _Carbin = value; }
            get { return _Carbin; }
        }

        private String _Day;
        public String Day
        {
            set { _Day = value; }
            get { return _Day; }
        }

        private String _Date;
        public String Date
        {
            set { _Date = value; }
            get { return _Date; }
        }

        private String _DepartureAirport;
        public String DepartureAirport
        {
            set { _DepartureAirport = value; }
            get { return _DepartureAirport; }
        }

        private String _ArrivalAirport;
        public String ArrivalAirport
        {
            set { _ArrivalAirport = value; }
            get { return _ArrivalAirport; }
        }

        private String _Reservation;
        public String Reservation
        {
            set { _Reservation = value; }
            get { return _Reservation; }
        }

        private String _DepartureTime;
        public String DepartureTime
        {
            set { _DepartureTime = value; }
            get { return _DepartureTime; }
        }

        private String _ArrivalTime;
        public String ArrivalTime
        {
            set { _ArrivalTime = value; }
            get { return _ArrivalTime; }
        }

        private String _Ticket;
        public String Ticket
        {
            set { _Ticket = value; }
            get { return _Ticket; }
        }

        private String _ContextId = String.Empty;
        public String ContextId
        {
            get { return _ContextId; }
            set { _ContextId = value; }
        }

        private String _DepartureTerminal = String.Empty;
        public String DepartureTerminal
        {
            get { return _DepartureTerminal; }
            set { _DepartureTerminal = value; }
        }

        private String _ArrivalTerminal = String.Empty;
        public String ArrivalTerminal
        {
            get { return _ArrivalTerminal; }
            set { _ArrivalTerminal = value; }
        }
    }
}
