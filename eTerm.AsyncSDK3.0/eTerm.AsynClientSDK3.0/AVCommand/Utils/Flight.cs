using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class Flight
    {
        public Flight() { }

        private String _FlightID = "";
        public String FlightID
        {
            get { return _FlightID; }
            set { _FlightID = value; }
        }
        private String _Airline;
        public String Airline
        {
            get { return _Airline; }
            set { _Airline = value; }
        }

        private String _FlightNO;
        public String FlightNO
        {
            get { return _FlightNO; }
            set { _FlightNO = value; }
        }

        private FlightCarbin[] _Carbins;
        public FlightCarbin[] Carbins
        {
            get { return _Carbins; }
            set { _Carbins = value; }
        }

        private String _FltDate;
        public String FltDate
        {
            get { return _FltDate; }
            set { _FltDate = value; }
        }

        private String _ArrivalDate;
        public String ArrivalDate
        {
            get { return _ArrivalDate; }
            set { _ArrivalDate = value; }
        }

        private String _DepartureTime;
        public String DepartureTime
        {
            get { return _DepartureTime; }
            set { _DepartureTime = value; }
        }

        private String _ArrivalTime;
        public String ArrivalTime
        {
            get { return _ArrivalTime; }
            set { _ArrivalTime = value; }
        }

        private String _DepartureAirport;
        public String DepartureAirport
        {
            get { return _DepartureAirport; }
            set { _DepartureAirport = value; }
        }

        private String _ArrivalAirport;
        public String ArrivalAirport
        {
            get { return _ArrivalAirport; }
            set { _ArrivalAirport = value; }
        }

        private String _Stop;
        public String Stop
        {
            get { return _Stop; }
            set { _Stop = value; }
        }

        private String _Connect = String.Empty;
        public String Connect
        {
            get { return _Connect; }
            set { _Connect = value; }
        }

        private String _AircraftType = String.Empty;
        public String AircraftType
        {
            get { return _AircraftType; }
            set { _AircraftType = value; }
        }

        private String _Meals = String.Empty;
        public String Meal
        {
            get { return _Meals; }
            set { _Meals = value; }
        }

        private String _FoodType = String.Empty;
        public String FoodType
        {
            get { return _FoodType; }
            set { _FoodType = value; }
        }

        private Boolean _Direct = false;
        public Boolean Direct
        {
            get { return _Direct; }
            set { _Direct = value; }
        }

        private Boolean _CodeShare = false;
        public Boolean CodeShare
        {
            get { return _CodeShare; }
            set { _CodeShare = value; }
        }

        private String _CodeShareFlight = String.Empty;
        public String CodeShareFlight
        {
            get { return _CodeShareFlight; }
            set { _CodeShareFlight = value; }
        }

        private String _E = String.Empty;
        public String E
        {
            get { return _E; }
            set { _E = value; }
        }

        private String _ParentID = String.Empty;
        public String ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }

        private Flight[] _SubFlight = new Flight[] { };
        public Flight[] SubFlight
        {
            get { return _SubFlight; }
            set { _SubFlight = value; }
        }

    }
}
