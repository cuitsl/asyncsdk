using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class PNRInfo
    {
        public PNRInfo() { }
        private String _PNR;
        public String PNR
        {
            set { _PNR = value; }
            get { return _PNR; }
        }

        private String _BPNR;
        public String BPNR
        {
            set { _BPNR = value; }
            get { return _BPNR; }
        }

        private bool _IsTicketed = false;
        public bool Isticketed
        {
            set { _IsTicketed = value; }
            get { return _IsTicketed; }
        }

        private Passenger[] _PassengerList;
        public Passenger[] PassengerList
        {
            set { _PassengerList = value; }
            get { return _PassengerList; }
        }

        private int _PassengerQuantity = 0;
        public int PassengerQuantity
        {
            get { return _PassengerQuantity; }
            set { _PassengerQuantity = value; }
        }

        private int _AdultQuantity = 0;
        public int AdultQuantity
        {
            get { return _AdultQuantity; }
            set { _AdultQuantity = value; }
        }

        private int _ChildrenQuantity = 0;
        public int ChildrenQuantity
        {
            get { return _ChildrenQuantity; }
            set { _ChildrenQuantity = value; }
        }

        private int _InfantQuantity = 0;
        public int InfantQuantity
        {
            get { return _InfantQuantity; }
            set { _InfantQuantity = value; }
        }

        private Segment[] _SegmentList;
        public Segment[] SegmentList
        {
            set { _SegmentList = value; }
            get { return _SegmentList; }
        }

        private String _Contact;
        public String Contact
        {
            set { _Contact = value; }
            get { return _Contact; }
        }

        private String _UserReserveTime;
        public String UserReserveTime
        {
            set { _UserReserveTime = value; }
            get { return _UserReserveTime; }
        }

        private String _AirlineReserveTime;
        public String AirlineReserveTime
        {
            set { _AirlineReserveTime = value; }
            get { return _AirlineReserveTime; }
        }

        private String[] _Remark;
        public String[] Remark
        {
            set { _Remark = value; }
            get { return _Remark; }
        }

        private String[] _SSR;
        public String[] SSR
        {
            set { _SSR = value; }
            get { return _SSR; }
        }

        private String[] _OSI;
        public String[] OSI
        {
            set { _OSI = value; }
            get { return _OSI; }
        }

        private Ticket[] _TN;
        public Ticket[] TN
        {
            set { _TN = value; }
            get { return _TN; }
        }

        private FNInfo _FN;
        public FNInfo FN
        {
            set { _FN = value; }
            get { return _FN; }
        }

        private String _OfficeNo;
        public String OfficeNo
        {
            set { _OfficeNo = value; }
            get { return _OfficeNo; }
        }

        private bool _Cancel;
        public bool Cancel
        {
            set { _Cancel = value; }
            get { return _Cancel; }
        }

        private bool _ValidFare;
        public bool ValidFare
        {
            set { _ValidFare = value; }
            get { return _ValidFare; }
        }

        private string _ADTK = string.Empty;
        public string ADTK
        {
            get { return _ADTK; }
            set { _ADTK = value; }
        }

        private string _TKTL = string.Empty;
        public string TKTL
        {
            get { return _TKTL; }
            set { _TKTL = value; }
        }

        private Seat[] _SEAT;
        public Seat[] SEAT
        {
            get { return _SEAT; }
            set { _SEAT = value; }
        }

        private String _ItineraryType = String.Empty;
        public String ItineraryType
        {
            get { return _ItineraryType; }
            set { _ItineraryType = value; }
        }
    }
}
