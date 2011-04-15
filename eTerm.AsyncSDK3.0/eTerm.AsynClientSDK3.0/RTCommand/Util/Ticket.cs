using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class Ticket
    {
        public Ticket() { }

        private String _PassengerID;
        public String PassengerID
        {
            set { _PassengerID = value; }
            get { return _PassengerID; }
        }

        private String _TicketNo;
        public String TicketNo
        {
            set { _TicketNo = value; }
            get { return _TicketNo; }
        }
    }
}
