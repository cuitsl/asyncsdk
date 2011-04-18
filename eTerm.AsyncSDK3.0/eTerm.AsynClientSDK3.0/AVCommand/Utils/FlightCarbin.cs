using System;
using System.Collections.Generic;
using System.Text;

namespace eTerm.ASynClientSDK
{
    internal class FlightCarbin
    {
        public FlightCarbin() { }

        private String _Carbin = String.Empty;
        public String Carbin
        {
            get { return _Carbin; }
            set { _Carbin = value; }
        }

        private String _ClassName = String.Empty;
        public String ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        private String _Number = string.Empty;
        public String Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        private Int32 _Fare = 0;
        public Int32 Fare
        {
            get { return _Fare; }
            set { _Fare = value; }
        }

        private String _Discount = String.Empty;
        public String Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }

        private Int32 _Tax = 0;
        public Int32 Tax
        {
            get { return _Tax; }
            set { _Tax = value; }
        }

        private Int32 _YQ = 0;
        public Int32 YQ
        {
            get { return _YQ; }
            set { _YQ = value; }
        }

        private String _AppRule = "";
        public String AppRule
        {
            get { return _AppRule; }
            set { _AppRule = value; }
        }

        private String _SaleRestriction = String.Empty;
        public String SaleRestriction
        {
            get { return _SaleRestriction; }
            set { _SaleRestriction = value; }
        }
    }
}
