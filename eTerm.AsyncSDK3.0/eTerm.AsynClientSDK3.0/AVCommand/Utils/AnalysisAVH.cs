using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace eTerm.ASynClientSDK
{
    internal class AnalysisAVH
    {
        public AnalysisAVH() { }

        private FlightCarbin[] ParseCarbin(string szInfo)
        {
            FlightCarbin[] fcs = new FlightCarbin[] { };
            MatchCollection M = Regex.Matches(szInfo, @"[A-Z]\w\s");
            foreach (Match _m in M)
            {
                FlightCarbin fc = new FlightCarbin();
                fc.Carbin = _m.Value.Trim().Substring(0, 1);
                fc.Number = _m.Value.Trim().Substring(1);
                Array.Resize(ref fcs, fcs.Length + 1);
                fcs[fcs.Length - 1] = fc;
            }
            return fcs;
        }

        private void ParsFlightDetail(Flight flt, string str)
        {
            
            string s = Regex.Match(str, @"\s\d{4}(\+\d)?\s+\d{4}(\+\d)?\s+\w{3}\s+\d(\^|\s)[A-Z]{0,1}\s+(E)?").Value.ToString();
            if (s.Trim().Length > 0)
            { 
                MatchCollection M = Regex.Matches(s,@"\d{4}(\+\d)?");
                if (M.Count > 0)
                    flt.DepartureTime = M[0].Value.ToString();
                if(M.Count>1)
                    flt.ArrivalTime = M[1].Value.ToString();
                flt.AircraftType = Regex.Match(s, @"\s\w{3}\s").Value.ToString().Trim();
                s = Regex.Match(s, @"\s\d(\^)?\s?[A-Z]{0,1}\s+(E)?").Value.ToString();
                flt.Stop = Regex.Match(s, @"\s\d").Value.ToString().Trim();
                flt.Meal = Regex.Match(s, @"\^").Value.ToString();
                M = Regex.Matches(s, @"[A-Z]");
                if (M.Count == 2)
                {
                    flt.FoodType = M[0].Value;
                    flt.E = M[1].Value;
                }
                else if (M.Count == 1)
                {
                    if (M[0].Value.ToString() == "E")
                        flt.E = M[0].Value.ToString();
                    else
                        flt.FoodType = M[0].Value;
                }
            }
        }

        private Flight ParseFlight(string szInfo)
        {
            MatchCollection matches = Regex.Matches(szInfo, @"\b\d(\-|\+){0,1}\s{1,}(\*){0,1}\w{2}\d{2,4}\s+([A-Z]{2}){0,1}(!|\*|#){0,1}\s([A-Z]\w\s){1,}\s+[A-Z]{6}\s+\d{4}(\+\d){0,}\s+\d{4}(\+\d){0,}\s+\w{3}\s+\d(\^|\s)\w{0,1}\s+(E){0,1}\s{0,}(>){0,1}\s+(\w{2}\d{2,4}){0,1}\s+([A-Z]\w\s){0,}");
            foreach (Match _m in matches)
            {
                string a = _m.Value.ToString();
                if (Regex.IsMatch(a, @"\s9C\d{3,4}\s"))
                {
                    szInfo = szInfo.Replace(a, "");
                    break;
                }
            }
            if (szInfo.Trim() == "")
                return null;
            Flight flt = new Flight();
            string str = Regex.Match(szInfo, @"\b\d(\-|\+){0,1}\s+(\*){0,1}\w{2}\d{2,4}").Value;
            str = Regex.Match(str, @"(\*){0,1}\w{2}\d{2,4}").Value;
                            
            if (str.Trim().Length == 0)
            { return null; }
            else
            {
                if (str[0] == '*')
                {
                    flt.CodeShare = true;
                    str = str.Substring(1);
                }
                flt.Airline = str.Substring(0, 2);
                flt.FlightNO = str.Substring(2);
            }
            str = Regex.Match(szInfo, @"\b\d(\-|\+){0,1}\s{1,}(\*){0,1}\w{2}\d{2,4}\s+([A-Z]{2}){0,1}(!|\*|#){0,1}\s([A-Z]\w\s){1,}\s+[A-Z]{6}\s+\d{4}(\+\d){0,}\s+\d{4}(\+\d){0,}\s+\w{3}\s+\d(\^|\s)\w{0,1}\s+(E){0,1}\s{0,}(>){0,1}\s+(\w{2}\d{2,4}){0,1}\s+([A-Z]\w\s){0,}").Value;
            if (str.Trim().Length == 0)
            { return null; }
            FlightCarbin[] fcs = ParseCarbin(str);
            
            flt.Carbins = fcs;
            if (flt.CodeShare)
            {
                flt.CodeShareFlight = Regex.Match(Regex.Match(szInfo, @">\s+\w{2}\d{2,4}").Value, @"\w{2}\d{2,4}").Value;
            }
            flt.DepartureAirport = Regex.Match(str, @"\s[A-Z]{6}\s").Value.Trim().Substring(0, 3);
            flt.ArrivalAirport = Regex.Match(str, @"\s[A-Z]{6}\s").Value.Trim().Substring(3);
            ParsFlightDetail(flt, str);
            str = Regex.Replace(szInfo, @"\b\d(\-|\+){0,1}\s{1,}(\*){0,1}\w{2}\d{2,4}\s+([A-Z]{2}){0,1}(!|\*|#){0,1}\s([A-Z]\w\s){1,}\s+[A-Z]{6}\s+\d{4}(\+\d){0,}\s+\d{4}(\+\d){0,}\s+\w{3}\s+\d(\^|\s)\w{0,1}\s+(E){0,1}\s{0,}(>){0,1}\s+(\w{2}\d{2,4}){0,1}\s+([A-Z]\w\s){0,}", "");
            
            flt.SubFlight = ParseSubFlight(str);
            return flt;
        }

        private Flight[] ParseSubFlight(string szInfo)
        {
            Flight[] flts = new Flight[] { };
            MatchCollection M = Regex.Matches(szInfo, @"(\*){0,1}\w{2}\d{2,4}\s+([A-Z]{2}){0,1}(!|\*|#){0,1}\s([A-Z]\w\s){1,}\s+[A-Z]{3}\s+\d{4}(\+\d){0,}\s+\d{4}(\+\d){0,}\s+\w{3}\s+\d(\^|\s)\w{0,1}\s+(E){0,1}\s{0,}(>){0,1}\s+(\w{2}\d{2,4}){0,1}\s+([A-Z]\w\s){0,}");
            foreach (Match _m in M)
            {
                Flight flt = new Flight();
                string str = Regex.Match(_m.Value, @"(\*){0,1}\w{2}\d{2,4}\s+([A-Z]{2}){0,1}(!|\*|#){0,1}\s([A-Z]\w\s){1,}\s+[A-Z]{3}\s").Value.Trim();

                if (str.Trim().Length == 0)
                { }
                else
                {
                    if (str[0] == '*')
                    {
                        flt.CodeShare = true;
                        str = str.Substring(1);
                    }
                    flt.Airline = Regex.Match(str, @"\w{2}\d{2,4}\s").Value.ToString().Substring(0, 2);
                    flt.FlightNO = Regex.Match(str, @"\w{2}\d{2,4}\s").Value.ToString().Substring(2).Trim();
                }

                flt.DepartureAirport = "";
                flt.ArrivalAirport = Regex.Match(str, @"\s[A-Z]{3}").Value.Trim();
                str = _m.Value.Trim();
                FlightCarbin[] fcs = ParseCarbin(str);
                flt.Carbins = fcs;
                if (flt.CodeShare)
                {
                    flt.CodeShareFlight = Regex.Match(Regex.Match(str, @">\s+\w{2}\d{2,4}").Value, @"\w{2}\d{2,4}").Value;
                }
                Array.Resize(ref flts, flts.Length + 1);
                flts[flts.Length - 1] = flt;
            }
            return flts;
        }

        public FlightData ParseAVH(string szCmd, string szResult)
        {
            FlightData fd = new FlightData();
            if (Regex.IsMatch(szResult, @"\s\d{2}[A-Z]{3}\([A-Z]{3}\)\s+[A-Z]{6}"))
            {
                fd.ResultDate = Regex.Match(Regex.Match(szResult, @"\s\d{2}[A-Z]{3}\([A-Z]{3}\)\s+[A-Z]{6}").Value.ToString().Trim(), @"\d{2}[A-Z]{3}").Value.ToString();
            }
            else
            {
                fd.ErrorMsg = szResult;
                return fd;
            }
            string[] s = szResult.Split('\r');
            szResult = string.Empty;
            foreach (string _s in s)
            {
                if (Regex.IsMatch(_s,@"\s\d{2}[A-Z]{3}\([A-Z]{3}\)\s+[A-Z]{6}"))
                { 
                    
                }
                else
                {
                    if (Regex.IsMatch(_s, @"\b\d(\-|\+){0,1}\s{1,}(\*){0,1}\w{2}\d{2,4}\s+([A-Z]{2}){0,1}(!|\*|#){0,1}\s([A-Z]\w\s){1,}\s+"))
                    {
                        if (szResult.Length == 0)
                            szResult = szResult + " " + _s;
                        else
                            szResult = szResult + "\r\n" + _s;
                    }
                    else
                    {
                        szResult = szResult + " " + _s;
                    }
                }
            }
            Flight[] flts = new Flight[] { };
            szResult = szResult.Trim();
            s =szResult.Split(new string[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries);
            foreach(string _s in s)
            {
                Flight flt = ParseFlight(_s);
                if (flt != null)
                {
                    Array.Resize(ref flts, flts.Length + 1);
                    flts[flts.Length - 1] = flt;
                }
            }
            fd.Flights = flts;
            //fd.ErrorMsg = szResult;
            return fd;
        }
    }
}
