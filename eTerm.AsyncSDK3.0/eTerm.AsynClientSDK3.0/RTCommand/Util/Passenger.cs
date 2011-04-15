using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class Passenger
    {
        public Passenger() { }

        private Int32 _PID;
        public Int32 PID
        {
            set { _PID = value; }
            get { return _PID; }
        }

        private String _FullName = string.Empty;
        public String FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        private String _SurName = string.Empty;
        public String SurName
        {
            set { _SurName = value; }
            get { return _SurName; }
        }

        private String _MiddleName = string.Empty;
        public String MiddleName
        {
            set { _MiddleName = value; }
            get { return _MiddleName; }
        }

        private String _LastName = string.Empty;
        public String LastName
        {
            set { _LastName = value; }
            get { return _LastName; }
        }

        private String _Title = string.Empty;
        public String Title
        {
            set { _Title = value; }
            get { return _Title; }
        }

        private String _Sex = string.Empty;
        public String Sex
        {
            set { _Sex = value; }
            get { return _Sex; }
        }

        private String _BirthDay = string.Empty;
        public String BirthDay
        {
            set { _BirthDay = value; }
            get { return _BirthDay; }
        }

        private String _Nationality = string.Empty;
        public String Nationality
        {
            set { _Nationality = value; }
            get { return _Nationality; }
        }

        private String _DocumentType = string.Empty;
        public String DocumentType
        {
            set { _DocumentType = value; }
            get { return _DocumentType; }
        }

        private String _IssueCountry = string.Empty;
        public String IssueCountry
        {
            set { _IssueCountry = value; }
            get { return _IssueCountry; }
        }

        private String _DocumentNo = string.Empty;
        public String DocumentNo
        {
            set { _DocumentNo = value; }
            get { return _DocumentNo; }
        }

        private String _ExpireDate = string.Empty;
        public String ExpireDate
        {
            set { _ExpireDate = value; }
            get { return _ExpireDate; }
        }

        private String _TicketNo = String.Empty;
        public String TicketNo
        {
            set { _TicketNo = value; }
            get { return _TicketNo; }
        }

        private String _AgeType = string.Empty;
        public String AgeType
        {
            set { _AgeType = value; }
            get { return _AgeType; }
        }

        private int _AdultPID = 0;
        public int  AdultPID
        {
            set { _AdultPID = value; }
            get { return _AdultPID; }
        }

        private String _FQTV = string.Empty;
        public String FQTV
        {
            get { return _FQTV; }
            set { _FQTV = value; }
        }
    }
}
