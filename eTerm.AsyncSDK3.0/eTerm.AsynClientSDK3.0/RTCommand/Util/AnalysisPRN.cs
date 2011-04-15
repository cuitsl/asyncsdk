using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace eTerm.ASynClientSDK
{
    internal class AnalysisPRN
    {
        public AnalysisPRN() { }

        private String PNRSTR = string.Empty;
        private String PNR = string.Empty;
        protected string[] szDomesticCityList = { "PEK", "SHA", "CAN", "SZX", "HGH", "NKG", "CTU", "WUH", "TAO", "XIY", "DLC", "HRB", "CKG", "XMN", "TSN", "CZY", "CGO", "NGB", "CSX", "SYX", "KMG", "SHE", "FOC", "TNA", "ZUH", "WNZ", "KWE", "KWL", "LHW", "HAK", "KHN", "HET", "INC", "HFE", "LYA", "NDG", "NNG", "TYN", "SJW", "URC", "CGQ", "LXA", "AOG", "BAV", "BHY", "CIH", "DAT", "DDG", "DOY", "DYG", "HSN", "HYN", "JDZ", "JHG", "JIL", "JJN", "JMU", "JNZ", "JZH", "LJG", "LYG", "LZH", "MDG", "SHP", "SWA", "TXN", "WEF", "WEH", "WHU", "WUS", "XNN", "XUZ", "YNJ", "YNT", "YBP", "YIH", "YIW", "ZHA", "XFN", "BJS", "PVG" };
        public AnalysisPRN(string _PNR, string _PNRSTR)
        {
            PNRSTR = _PNRSTR;
            PNR = _PNR;

            //PNR信息中必然包含PNR号
            if (_PNR.IndexOf(PNR) < 0)
                return;
        }

        public PNRInfo ParsePNR()
        {
            string[] lines = PNRSTR.Split('\r');
            PNRSTR = "";
            foreach (string _s in lines)
            {
                if (_s.Length <= 80)
                    PNRSTR = PNRSTR + "\r" + _s;
                else
                {
                    PNRSTR = PNRSTR + "\r" + _s.Substring(0, 80);
                    PNRSTR = PNRSTR + "\r" + _s.Substring(80);
                }
            }
            PNRInfo pnrInfo = new PNRInfo();
            if (PNR.Trim().Length == 5)
                pnrInfo.PNR = "H" + PNR;
            else
                pnrInfo.PNR = PNR;
            //if(PNRSTR.IndexOf("ELECTRONIC TICKET PNR") >= 0)
            if (PNRSTR.IndexOf("*THIS PNR WAS ENTIRELY CANCELLED*") >= 0)
            {
                pnrInfo.Cancel = true;
                //return pnrInfo;
            }
            if (PNRSTR.IndexOf("**ELECTRONIC TICKET PNR**") >= 0)
                pnrInfo.Isticketed = true;
            string pnrPattern = @"\sH{0,1}" + PNR;
            string[] pnrSeg = Regex.Split(PNRSTR, pnrPattern);
            if (pnrSeg.Length < 2)
            {
                if (PNR.Length == 6)
                {
                    pnrPattern = @"\sH{0,1}" + PNR.Substring(1);
                    pnrSeg = Regex.Split(PNRSTR, pnrPattern);
                    if (pnrSeg.Length < 2)
                        return null;
                }
            }
            String Pattern = @"((\d\.[A-Z]+/[A-Z]+(\s[A-Z]+){0,})|(\d{1,2}\.(([\u4e00-\u9fa5])|([A-Z]))+))+";//([A-Z]{1,}\s)+

            MatchCollection Matches = Regex.Matches(pnrSeg[0], Pattern);
            if (Matches.Count > 0)
            {
                Passenger[] psgList = new Passenger[] { };
                foreach (Match match in Matches)
                {
                    Passenger psg = new Passenger();
                    psg.AgeType = "AD";
                    psg.PID = Convert.ToInt32(match.Value.Split('.')[0]);
                    psg.FullName = match.Value.ToString().Split('.')[1];
                    psg.SurName = match.Value.Split('.')[1].Split('/')[0];
                    if (Regex.IsMatch(psg.FullName, @"[\u4e00-\u9fa5]") && Regex.IsMatch(psg.FullName, @"[\u4e00-\u9fa5]CHD$"))
                    {
                        psg.FullName = Regex.Replace(psg.FullName, @"CHD$", "").Trim();
                        psg.AgeType = "CH";
                    }
                    if (match.Value.Split('.')[1].Split('/').Length == 2)
                    {
                        psg.LastName = match.Value.Split('.')[1].Split('/')[1];
                        if (psg.LastName.Split(' ').Length > 1 && Regex.IsMatch(psg.LastName.Trim(), @"\sCHD$"))
                        {
                            psg.LastName = Regex.Replace(psg.LastName.Trim(), @"\sCHD$", "");
                            psg.AgeType = "CH";
                        }

                        if (psg.LastName.Split(' ').Length > 1 && Regex.IsMatch(psg.LastName.Trim(), @"\sMISS$"))
                        {
                            psg.LastName = Regex.Replace(psg.LastName.Trim(), @"\sMISS$", "");
                            psg.AgeType = "CH";
                        }
                        if (psg.LastName.Split(' ').Length > 1 && Regex.IsMatch(psg.LastName.Trim(), @"\sMSTR$"))
                        {
                            psg.LastName = Regex.Replace(psg.LastName.Trim(), @"\sMSTR$", "");
                            psg.AgeType = "CH";
                        }

                        if (psg.LastName.Split(' ').Length > 1 && Regex.IsMatch(psg.LastName.Trim(), @"\sMS$"))
                        {
                            psg.LastName = Regex.Replace(psg.LastName.Trim(), @"\sMS$", "");
                            psg.Title = "MS";
                        }

                        if (psg.LastName.Split(' ').Length > 1 && Regex.IsMatch(psg.LastName.Trim(), @"\sMR$"))
                        {
                            psg.LastName = Regex.Replace(psg.LastName.Trim(), @"\sMR$", "");
                            psg.Title = "MR";
                        }
                    }
                    else if (match.Value.Split('.')[1].Split('/').Length == 3)
                    {
                        psg.LastName = match.Value.Split('.')[1].Split('/')[1];
                        psg.MiddleName = match.Value.Split('.')[1].Split('/')[2];

                        if (psg.MiddleName.Split(' ').Length > 1 && Regex.IsMatch(psg.MiddleName.Trim(), @"\sCHD$"))
                        {
                            psg.MiddleName = Regex.Replace(psg.MiddleName.Trim(), @"\sCHD$", "");
                            psg.AgeType = "CH";
                        }
                        if (psg.MiddleName.Split(' ').Length > 1 && Regex.IsMatch(psg.MiddleName.Trim(), @"\sMISS$"))
                        {
                            psg.MiddleName = Regex.Replace(psg.MiddleName.Trim(), @"\sMISS$", "");
                            psg.AgeType = "CH";
                        }
                        if (psg.MiddleName.Split(' ').Length > 1 && Regex.IsMatch(psg.MiddleName.Trim(), @"\sMSTR$"))
                        {
                            psg.MiddleName = Regex.Replace(psg.MiddleName.Trim(), @"\sMSTR$", "");
                            psg.AgeType = "CH";
                        }

                        if (psg.MiddleName.Split(' ').Length > 1 && Regex.IsMatch(psg.MiddleName.Trim(), @"\sMS$"))
                        {
                            psg.MiddleName = Regex.Replace(psg.MiddleName.Trim(), @"\sMS$", "");
                            psg.Title = "MS";
                        }
                        if (psg.MiddleName.Split(' ').Length > 1 && Regex.IsMatch(psg.MiddleName.Trim(), @"\sMR$"))
                        {
                            psg.MiddleName = Regex.Replace(psg.MiddleName.Trim(), @"\sMR$", "");
                            psg.Title = "MR";
                        }

                    }

                    Array.Resize(ref psgList, psgList.Length + 1);
                    psgList[psgList.Length - 1] = psg;
                    pnrInfo.PassengerQuantity++;
                    if (psg.AgeType == "AD")
                        pnrInfo.AdultQuantity++;
                    else if (psg.AgeType == "CH")
                        pnrInfo.ChildrenQuantity++;
                }

                Pattern = @"XN/IN/.+/P\d{1,}";
                Matches = Regex.Matches(pnrSeg[0], Pattern);
                if (Matches.Count > 0)
                {
                    foreach (Match m in Matches)
                    {
                        string szInfantName = m.Value.ToString().Replace("XN/IN/", "");
                        szInfantName = Regex.Replace(szInfantName, @"\(\w+\)/P\d", "").Trim();
                        Passenger psg = new Passenger();
                        psg.AgeType = "IN";
                        psg.AdultPID = Convert.ToInt32(m.Value.ToString().Split('/')[m.Value.ToString().Split('/').Length - 1].Substring(1));
                        psg.PID = psgList[psgList.Length - 1].PID + 1;
                        psg.FullName = szInfantName.Replace(" INF", "");
                        psg.SurName = szInfantName.Split('/')[0];
                        if (szInfantName.Split('/').Length == 2)
                        {
                            psg.LastName = szInfantName.Split('/')[1];
                        }
                        else if (szInfantName.Split('/').Length == 3)
                        {
                            psg.LastName = szInfantName.Split('/')[1];
                            psg.MiddleName = szInfantName.Split('/')[2];
                        }
                        Array.Resize(ref psgList, psgList.Length + 1);
                        psgList[psgList.Length - 1] = psg;
                        pnrInfo.PassengerQuantity++;
                        pnrInfo.InfantQuantity++;
                    }
                }
                pnrInfo.PassengerList = psgList;
            }
            else
                return null;

            Pattern = @"SSR DOCS[\s\w]+P/[\s\w/\-\+]+P\d{1,}";
            Matches = Regex.Matches(pnrSeg[1], Pattern);
            if (Matches.Count > 0)
            {
                Pattern = @"P/[\s\w/\-\+]+P\d{1,}";
                foreach (Match match in Matches)
                {
                    string sztmp = Regex.Match(match.Value, Pattern).Value.ToString().Trim();
                    string szPID = sztmp.Split('/')[sztmp.Split('/').Length - 1];
                    foreach (Passenger psg in pnrInfo.PassengerList)
                    {
                        if (psg.PID == Convert.ToInt32(szPID.Substring(1)) && psg.AgeType != "IN" && (sztmp.Split('/')[5] == "F" || sztmp.Split('/')[5] == "M"))
                        {
                            psg.Nationality = sztmp.Split('/')[1];
                            psg.DocumentNo = sztmp.Split('/')[2];
                            psg.IssueCountry = sztmp.Split('/')[3];
                            psg.BirthDay = sztmp.Split('/')[4];
                            psg.Sex = sztmp.Split('/')[5];
                            psg.ExpireDate = sztmp.Split('/')[6];
                            break;
                        }
                        else if (psg.AdultPID == Convert.ToInt32(szPID.Substring(1)) && psg.AgeType == "IN" && (sztmp.Split('/')[5] == "FI" || sztmp.Split('/')[5] == "MI"))
                        {

                            psg.Nationality = sztmp.Split('/')[1];
                            psg.DocumentNo = sztmp.Split('/')[2];
                            psg.IssueCountry = sztmp.Split('/')[3];
                            psg.BirthDay = sztmp.Split('/')[4];
                            psg.Sex = sztmp.Split('/')[5];
                            psg.ExpireDate = sztmp.Split('/')[6];
                            break;
                        }
                    }
                }
            }
            else
            {
                Pattern = @"SSR\s+FOID\s+\w{2}\s+\w{3}\s+NI\w+/P\d{1,}";
                Matches = Regex.Matches(pnrSeg[1], Pattern);
                foreach (Match match in Matches)
                {
                    int pid = Convert.ToInt32(Regex.Match(match.Value.ToString(), @"/P\d{1,}$").Value.ToString().Replace("/P",""));
                    foreach (Passenger psg in pnrInfo.PassengerList)
                    {
                        if (psg.PID == pid)
                        {
                            psg.DocumentNo = Regex.Match(match.Value.ToString(), @"NI\w+/P\d{1,}").Value.ToString().Split('/')[0].Substring(2);
                            psg.Sex = "";
                            psg.DocumentType = "";
                            break;
                        }
                    }
                }
            }
            //Pattern = @"\d{1,}\.SSR\sADTK(\s\w+)+";
            //string szADTK = Regex.Match(pnrSeg[1], Pattern).ToString();
            //if (szADTK != "")
            //{
            //    Pattern = @"\s\d{2}[A-Z]{3]\s";
            //    pnrInfo.ADTK = Regex.Match(szADTK, Pattern).ToString().Trim();
            //}
            Pattern = @"(\d{1,}\.\s+\*?\w{2}\d{1,4}\s+(([A-Z]\s+)|([A-Z]\d\s+))[A-Z]{2}\d{2}[A-Z]{3}((\s\s)|(\d{2}))[A-Z]{6}\s+[A-Z]{2}\d{1}\s+\d{4}\s+\d{4}((\s+)|(\+\d)))(E\s[\-|\w]+){0,1}|(\d{1,}\.\s+\*?\w{2}OPEN\s\w{1,3}\s+([A-Z]{2}\d{2}[A-Z]{3}){0,1}\s+[A-Z]{6})";
            //Pattern = @"(\d{1,}\.\s+\w{2}((\d{1,4})|(OPEN))\s+(([A-Z]\s+)|([A-Z]\d\s+))[A-Z]{2}\d{2}[A-Z]{3}((\s\s)|(\d{2}))[A-Z]{6}\s+[A-Z]{2}\d{1}\s+\d{4}\s+\d{4}((\s+)|(\+\d)))|(\d{1,}\.\s+\w{2}OPEN\s+(([A-Z]\s+)|([A-Z]\d\s+))[A-Z]{2}\d{2}[A-Z]{3}((\s\s)|(\d{2}))[A-Z]{6})";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.Multiline);
            pnrInfo.ItineraryType = "D";
            if (Matches.Count > 0)
            {
                Segment[] segList = new Segment[] { };
                int i = 0;
                foreach (Match match in Matches)
                {
                    Segment seg = new Segment();
                    seg.ContextId = Regex.Match(match.Value.ToString(), @"\d{1,}\.").Value.ToString().Split('.')[0];
                    seg.ID = ++i;
                    string xtmp = Regex.Replace(match.Value.ToString(), @"\d{1,}\.", "").Trim();
                    string szPre = "";
                    if (xtmp[0] == '*')
                    {
                        xtmp = xtmp.Substring(1);
                        szPre = "*";
                    }
                    else
                        szPre = "";

                    seg.Airline = szPre + xtmp.Substring(0, 2);
                    seg.FltNo = xtmp.Substring(2, 5).Trim();
                    seg.Carbin = xtmp.Substring(7, 4).Trim();
                    seg.Day = xtmp.Substring(11, 2).Trim();
                    seg.Date = xtmp.Substring(13, 7).Trim();
                    if (seg.Date.Length == 5)
                    {
                        //seg.Date = seg.Date + DateTime.Now.Year.ToString().Substring(2, 2);
                    }
                    seg.DepartureAirport = xtmp.Substring(20, 3).Trim();
                    seg.ArrivalAirport = xtmp.Substring(23, 3).Trim();
                    if (seg.FltNo != "OPEN")
                    {
                        seg.Reservation = xtmp.Substring(27, 3);
                        seg.DepartureTime = xtmp.Substring(33, 5).Trim();
                        seg.ArrivalTime = xtmp.Substring(38).Trim();
                        seg.ArrivalTime = Regex.Match(seg.ArrivalTime, @"\d{4}(\+\d){0,1}").Value;
                    }
                    else
                    {
                        seg.Reservation = "";
                        seg.DepartureTime = "";
                        seg.ArrivalTime = "";
                    }
                    bool bDomestic1 = false;
                    bool bDomestic2 = false;
                    foreach (string _dcity in szDomesticCityList)
                    {
                        if (_dcity == seg.DepartureAirport)
                        {
                            bDomestic1 = true;
                            break;
                        }
                    }
                    foreach (string _dcity in szDomesticCityList)
                    {
                        if (_dcity == seg.ArrivalAirport)
                        {
                            bDomestic2 = true;
                            break;
                        }
                    }
                    if (bDomestic1 == false || bDomestic2 == false)
                        pnrInfo.ItineraryType = "I";

                    if (Regex.IsMatch(match.Value.ToString(), @"\sE\s[\-|\w]+"))
                    {
                        string _str = Regex.Match(match.Value.ToString(), @"\sE\s[\-|\w]+").Value.ToString().Trim();
                        _str = _str.Split(' ')[1];
                        try
                        {
                            seg.DepartureTerminal = _str.Substring(0, 2).Trim();
                            seg.ArrivalTerminal = _str.Substring(2).Trim();
                        }
                        catch
                        { }
                    }
                    Array.Resize(ref segList, segList.Length + 1);
                    segList[segList.Length - 1] = seg;
                }
                pnrInfo.SegmentList = segList;
            }
            else
                return null;
            Pattern = @"\d{1,2}\.TN/.+/P\d{1,}";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                Ticket[] tktList = new Ticket[] { };
                foreach (Match match in Matches)
                {
                    Ticket tkt = new Ticket();
                    tkt.TicketNo = match.Value.Split('/')[1];
                    tkt.PassengerID = match.Value.Split('/')[2].Replace('\r', ' ').Trim();
                    Array.Resize(ref tktList, tktList.Length + 1);
                    tktList[tktList.Length - 1] = tkt;
                }
                pnrInfo.TN = tktList;
            }
            FNInfo fn = new FNInfo();
            fn.FCNY = "";

            Pattern = @"FCNY\d+\.\d{2}";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                fn.FCNY = Matches[0].Value.Replace("FCNY", "").Trim();
            }
            else
                fn.FCNY = "0";
            Pattern = @"SCNY\d+\.\d{2}";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                fn.SCNY = Matches[0].Value.Replace("SCNY", "").Trim();
            }
            else
                fn.SCNY = "0";
            Pattern = @"/C\d+\.\d{2}";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                fn.C = Matches[0].Value.Replace("/C", "").Trim();
            }
            else
                fn.C = "3";
            Pattern = @"XCNY\d+\.\d{2}";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                fn.XCNY = Matches[0].Value.Replace("XCNY", "").Trim();
            }
            else
                fn.XCNY = "0";
            Pattern = @"\bACNY\d+\.\d{2}\b";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                fn.ACNY = Matches[0].Value.Replace("ACNY", "").Trim();
            }
            else
                fn.ACNY = "0";
            Pattern = @"TCNY\d+\.\d{2}[A-Z][A-Z]";
            Matches = Regex.Matches(pnrSeg[1], Pattern, RegexOptions.IgnoreCase);
            if (Matches.Count > 0)
            {
                string[] TCNY = new string[] { };
                foreach (Match match in Matches)
                {
                    Array.Resize(ref TCNY, TCNY.Length + 1);
                    TCNY[TCNY.Length - 1] = match.Value.Replace("TCNY", "").Trim();
                }
                fn.TCNY = TCNY;
            }
            pnrInfo.FN = fn;

            string pattern = @"TN/(IN/){0,1}\d{3}-\d+(-\d{2}){0,2}/P\d{1,}";
            MatchCollection M = Regex.Matches(PNRSTR, pattern);
            foreach (Match _m in M)
            {
                string szTN = string.Empty;
                pattern = @"P\d{1,}";
                int PID = Convert.ToInt32(Regex.Match(_m.Value.ToString(), pattern).Value.ToString().Replace("P", ""));
                pattern = @"\d{3}-\d+(-\d{2}){0,2}";
                szTN = Regex.Match(_m.Value.ToString(), pattern).Value.ToString();

                foreach (Passenger _p in pnrInfo.PassengerList)
                {
                    if (_p.AgeType != "IN" && _p.PID == PID)
                    {
                        _p.TicketNo = szTN;
                        break;
                    }
                    else if (_p.AgeType == "IN" && _p.AdultPID == PID)
                    {
                        _p.TicketNo = szTN;
                        break;
                    }
                }
            }

            pattern = @"TL/\d{4}/\d{2}[A-Z]{3}";
            pnrInfo.TKTL = Regex.Match(pnrSeg[1], pattern).Value.ToString().Replace("TL/", "");

            //分析出票截止日信息

            pattern = @"SSR\sADTK\s1E\sBY\s[A-Z]{3}\d{2}[A-Z]{3}\d{2}/\d{4}";
            // MU/CA等格式 SSR ADTK 1E BY SHA27JUN10/1635 OR CXL CA 919 Q23JUL
            pnrInfo.ADTK = Regex.Match(pnrSeg[1], pattern).Value.ToString();
            if (pnrInfo.ADTK != "")
            {
                pnrInfo.ADTK = Regex.Match(pnrInfo.ADTK, @"d{2}[A-Z]{3}\d{2}/\d{4}").Value.ToString();
            }
            if (pnrInfo.ADTK == "")
            {
                pattern = @"SSR\sADTK\s1E\sTO\s\w{2}\sBY\s[A-Z]{3}\d{4}/\d{2}[A-Z]{3}";
                // KE等格式 SSR ADTK 1E TO KE BY SHA1955/01JUL OTHERWISE WILL BE XLD 
                pnrInfo.ADTK = Regex.Match(pnrSeg[1], pattern).Value.ToString();
                if (pnrInfo.ADTK != "")
                {
                    pnrInfo.ADTK = Regex.Match(pnrInfo.ADTK, @"\d{4}/\d{2}[A-Z]{3}").Value.ToString();
                    pnrInfo.ADTK = Regex.Match(pnrInfo.ADTK, @"\d{2}[A-Z]{3}").Value.ToString() + "/" + Regex.Match(pnrInfo.ADTK, @"\d{4}").Value.ToString();
                }
            }
            if (pnrInfo.ADTK == "")
            {
                pattern = @"SSR\sADTK\s\w{2}\sTO\s\w{2}\sBY\s\d{2}[A-Z]{3}[\s|\w]+\d{4}";
                //AF 格式 SSR ADTK 1E TO AF BY 20JUN OTHERWISE WILL BE XLD
                pnrInfo.ADTK = Regex.Match(pnrSeg[1], pattern).Value.ToString();
                pnrInfo.ADTK = Regex.Match(pnrInfo.ADTK, @"\d{2}[A-Z]{3}").Value.ToString() + "/" + Regex.Match(pnrInfo.ADTK, @"\d{4}").Value.ToString();
            }

            if (pnrInfo.ADTK == "")
            {
                pattern = @"SSR\sOTHS\s\w+BEFORE\s\d{4}[A-Z]?/\d{2}[A-Z]{3}";
                //SSR OTHS 1E KLM CANCELS IF NO TKT ISSUED BEFORE 0500Z/29JUN 
                pnrInfo.ADTK = Regex.Match(pnrSeg[1], pattern).Value.ToString();
            }

            if (pnrInfo.ADTK == "" || pnrInfo.ADTK.Length< 5)
            {
                pattern = @"SSR\sADTK\s1E\sADV\sTKT\sNBR\sTO\sCX/KA\sBY\s\d{1,2}[A-Z]{3}";
                pnrInfo.ADTK = Regex.Match(pnrSeg[1], pattern).Value.ToString();
                pnrInfo.ADTK = Regex.Match(pnrInfo.ADTK, @"\d{1,2}[A-Z]{3}").Value.ToString();
            }
            pattern = @"RMK\s\w{2}/\w{5,6}";
            pnrInfo.BPNR = Regex.Match(pnrSeg[1], pattern).Value.ToString();
            if (pnrInfo.BPNR.IndexOf("/") > 0)
                pnrInfo.BPNR = pnrInfo.BPNR.Split('/')[1].Trim();

            pattern = @"SSR\sSEAT\s+\w{2}\s+\w{3}\s+[A-Z]{6}\s+\d+\s+[A-Z]\d{2}[A-Z]{3}\d{0,2}\s+\d{1,2}[A-Z]{2}(/P\d{1}){0,1}";
            Matches = Regex.Matches(pnrSeg[1], pattern);
            Seat[] seats = new Seat[] { };
            foreach (Match _m in Matches)
            {
                string[] s = _m.Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                Seat _seat = new Seat();
                _seat.CityPair = s[4];
                _seat.Carbin = s[6].Substring(0, 1);
                _seat.TripDate = s[6].Substring(1);
                _seat.FlightNo = s[2] + s[5];
                _seat.SeatNo = s[7];
                if (s[7].IndexOf("/P") > 0)
                {
                    _seat.PID = Convert.ToInt32(s[7].Split('/')[1].Substring(1));
                    _seat.SeatNo = s[7].Split('/')[0];
                    _seat.SeatNo = _seat.SeatNo.Substring(0, _seat.SeatNo.Length - 1);
                }
                else
                {
                    _seat.PID = 1;
                    _seat.SeatNo = s[7].Split('/')[0];
                    _seat.SeatNo = _seat.SeatNo.Substring(0, _seat.SeatNo.Length - 1);
                }

                Array.Resize(ref seats, seats.Length + 1);
                seats[seats.Length - 1] = _seat;
                //foreach (Segment _smt in pnrInfo.SegmentList)
                //{
                //    if (_smt.DepartureAirport + _smt.ArrivalAirport == s[4] && _smt.Carbin.Substring(0, 1) == s[6].Substring(0, 1))
                //    {
                //        _smt.Seat = s[7];
                //        _smt.Seat = _smt.Seat.Substring(0, _smt.Seat.Length - 1);
                //    }
                //}
            }
            pnrInfo.SEAT = seats;

            pattern = @"SSR\sFQTV\s(\s|\w|/)+\w{2}\d+/P\d";
            Matches = Regex.Matches(pnrSeg[1], pattern);
            foreach (Match _m in Matches)
            {
                string s = Regex.Match(_m.Value.ToString(), @"\w+/P\d{1,}").Value.ToString();
                s = Regex.Replace(s, @"/P\d{1,}", "");
                int p = Convert.ToInt32(Regex.Match(_m.Value.ToString(), @"/P\d").Value.ToString().Replace("/P", ""));
                foreach (Passenger _psg in pnrInfo.PassengerList)
                {
                    if (p == _psg.PID)
                    {
                        _psg.FQTV = s;
                        break;
                    }
                }
            }
            return pnrInfo;
        }
    }
}
