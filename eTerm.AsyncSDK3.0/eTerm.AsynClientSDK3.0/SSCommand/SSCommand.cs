using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using eTerm.ASynClientSDK.SSException;
using eTerm.ASynClientSDK.Exception;

namespace eTerm.ASynClientSDK {

    #region SSR FOID 
    /// <summary>
    /// SSR FOID 组
    /// </summary>
    internal struct SSRFOID {
        /// <summary>
        /// Gets or sets the airline.
        /// </summary>
        /// <value>The airline.</value>
        public string airline { get; set; }

        /// <summary>
        /// Gets or sets the passageer.
        /// </summary>
        /// <value>The passageer.</value>
        public BookPassenger Passageer { get; set; }
    } 
    #endregion

    #region SSR FQTV
    /// <summary>
    /// SSR FQTV
    /// </summary>
    internal struct SSRFQTV {
        /// <summary>
        /// Gets or sets the cardno.
        /// </summary>
        /// <value>The cardno.</value>
        public string cardno { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
    }
    #endregion

    /// <summary>
    /// 代表订座客户端
    /// <code>
    /// SSCommand Ss = new SSCommand();
    /// Ss.addAirSeg(new BookAirSeg("MU5415", 'Y', "PVG", "CTU", string.Empty, DateTime.Now.AddDays(20), BookAirSeg.AIRSEGTYPE.AIRSEG_NORMAL));
    /// Ss.addAirSeg(new BookAirSeg("MU5416", 'Y', "CTU", "PVG", string.Empty, DateTime.Now.AddDays(23), BookAirSeg.AIRSEGTYPE.AIRSEG_NORMAL));
    /// Ss.addAdult("胡李俊");
    /// Ss.addAdult("顾宗铭");
    /// Ss.addAdult("张三");
    /// Ss.addAdult("李四");
    /// Ss.addAdult("王五");
    /// Ss.addAdult("麻六");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "胡李俊");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "顾宗铭");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "张三");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "李四");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "王五");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "P6");
    /// Ss.setOffice = "SHA125";
    /// Ss.addContact(new BookContact("SHA", "12345678", "HULIJUN"));
    /// Ss.setTimelimit = DateTime.Now.AddDays(15);
    /// SSResult SR = Ss.Commit() as SSResult;
    /// Console.WriteLine("订票结果：PNR="+SR.getPnr+"\r\n"+SR.OrgCmd);
    /// </code>
    /// <example>
    /// SSCommand Ss = new SSCommand();
    /// Ss.addAirSeg(new BookAirSeg("MU5415", 'Y', "PVG", "CTU", string.Empty, DateTime.Now.AddDays(20), BookAirSeg.AIRSEGTYPE.AIRSEG_NORMAL));
    /// Ss.addAirSeg(new BookAirSeg("MU5416", 'Y', "CTU", "PVG", string.Empty, DateTime.Now.AddDays(23), BookAirSeg.AIRSEGTYPE.AIRSEG_NORMAL));
    /// Ss.addAdult("胡李俊");
    /// Ss.addAdult("顾宗铭");
    /// Ss.addAdult("张三");
    /// Ss.addAdult("李四");
    /// Ss.addAdult("王五");
    /// Ss.addAdult("麻六");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "胡李俊");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "顾宗铭");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "张三");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "李四");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "王五");
    /// Ss.addSSR_FOID("MU", "93747237293729462", "P6");
    /// Ss.setOffice = "SHA125";
    /// Ss.addContact(new BookContact("SHA", "12345678", "HULIJUN"));
    /// Ss.setTimelimit = DateTime.Now.AddDays(15);
    /// SSResult SR = Ss.Commit() as SSResult;
    /// Console.WriteLine("订票结果：PNR="+SR.getPnr+"\r\n"+SR.OrgCmd);
    /// </example>
    /// </summary>
    public class SSCommand:ASynCommand {

        #region 构造函数
        /// <summary>
        /// 使用定义配置项构造连接.
        /// </summary>
        /// <param name="address">服务器地址.</param>
        /// <param name="port">服务器端口.</param>
        /// <param name="userName">授权用户名.</param>
        /// <param name="userPass">授权用户密码.</param>
        /// <param name="groupCode">授权用户分组.</param>
        public SSCommand(string address, int port, string userName, string userPass, string groupCode) {

        }

        /// <summary>
        /// 使用配置文件配置项构造连接.
        /// </summary>
        public SSCommand() : base() { }
        #endregion

        #region 变量定义
        //成人旅客姓名
        private List<string> __adultList = new List<string>();

        //儿童旅客姓名
        private List<string> __childList = new List<string>();

        //婴儿旅客姓名
        private List<BookInfant> __infantList = new List<BookInfant>();

        /// <summary>
        /// 联系组
        /// </summary>
        private List<BookContact> __bookContactList = new List<BookContact>();

        /// <summary>
        /// 航段集合
        /// </summary>
        private List<BookAirSeg> __airSegList = new List<BookAirSeg>();

        /// <summary>
        /// SSR FOID
        /// </summary>
        private List<SSRFOID> __airSSRFOID = new List<SSRFOID>();

        /// <summary>
        /// SSR FQTV
        /// </summary>
        private List<SSRFQTV> __airSSRFQTV = new List<SSRFQTV>();
        private bool __IsSpecial = false;
        #endregion

        #region 重写部分
        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            SSResult Ss = new SSResult();
            ParsePnr(Ss, Msg);
            return Ss;
        }
        #endregion

        #region 属性定义
        /// <summary>
        /// 是否为特殊预定（TRUE:则会将封口与普通指令分开执行）
        /// </summary>
        //public bool IsSpecial { get { return __IsSpecial; } set { 
        //    this.__IsSpecial = value;
        //} }
        #endregion

        #region 分析结果
        /// <summary>
        /// Parses the PNR.
        /// </summary>
        /// <param name="Ss">The ss.</param>
        /// <param name="Msg">The MSG.</param>
        private void ParsePnr(SSResult Ss, string Msg) {
            if (Regex.IsMatch(Msg, @"\s*(\w{6})\s+(\-)+\s*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                Ss.getPnr = Regex.Match(Msg, @"\s*(\w{6})\s+(\-)+\s*", RegexOptions.Multiline | RegexOptions.IgnoreCase).Groups[1].Value;
        }
        #endregion

        #region 参数设置
        /// <summary>
        /// 设置封口方式。.
        /// <code>
        /// Ss.setEnvelopType = "@KI";
        /// Ss.setEnvelopType = "@";
        /// </code>
        /// </summary>
        /// <value>The type of the set envelop.</value>
        public string setEnvelopType { set; protected get; }

        /// <summary>
        /// 设置团名.
        /// </summary>
        /// <value>The name of the set group.</value>
        public string setGroupName { set; protected get; }

        /// <summary>
        /// 设置团体票标识.
        /// </summary>
        /// <value><c>true</c> if [set group ticket]; otherwise, <c>false</c>.</value>
        public bool setGroupTicket { set;protected get; }

        /// <summary>
        /// 获取团体票标识。 .
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is group ticket; otherwise, <c>false</c>.
        /// </value>
        public bool isGroupTicket { get { return this.setGroupTicket; } }

        /// <summary>
        /// 设置责任组Office Code 如setOffice("PEK099"),为将所生成的PNR责任组设置为PEK099.
        /// </summary>
        /// <value>The set office.</value>
        public string setOffice { set;protected get; }

        /// <summary>
        /// 设置团体pnr的人数.
        /// </summary>
        /// <value>The set passenger number.</value>
        public int setPassengerNumber { set; protected get; }

        /// <summary>
        /// 设置出票时限.
        /// </summary>
        /// <value>The set timelimit.</value>
        public DateTime? setTimelimit { set; protected get; }

        /// <summary>
        /// 订座数.
        /// </summary>
        /// <value>The gettkt num.</value>
        protected virtual int gettktNum { get { return this.__adultList.Count + this.__childList.Count; } }
        #endregion

        #region 方法列表
        /// <summary>
        /// 添加SSR FOID 组.
        /// </summary>
        /// <example>
        /// Ss.addSSR_FOID("MU", "93747237293729462", "P1");
        /// Ss.addSSR_FOID("MU", "93747237293729462", "胡李俊");
        /// </example>
        /// <param name="airline">航空公司两字代码.</param>
        /// <param name="idtype">身份证件类型 NI身份证，CC信用卡，PP护照.</param>
        /// <param name="id">对应的身份证件号码.</param>
        /// <param name="name">对应旅客姓名.</param>
        public void addSSR_FOID(string airline, string idtype, string id, string name) {
            this.__airSSRFOID.Add(new SSRFOID() { airline = airline, Passageer = BookPassenger.createAdult(name, idtype, id) });
        }
        
        /// <summary>
        /// 添加SSR FOID 组(默认使用：NI).
        /// </summary>
        /// <param name="airline">航空公司两字代码.</param>
        /// <param name="id">对应的身份证件号码.</param>
        /// <param name="name">对应旅客姓名.</param>
        public void addSSR_FOID(string airline, string id, string name) {
            addSSR_FOID(airline, "NI", id,this.__adultList.Contains( name)?string.Format("P{0}",this.__adultList.IndexOf(name)+1):name);
        }

        /// <summary>
        /// 添加常旅客组.
        /// </summary>
        /// <param name="cardno">常旅客卡号，必须带航空公司代码！如 CA123456789.</param>
        /// <param name="name">对应旅客姓名 .</param>
        public void addSSR_FQTV(String cardno,
                        String name) {
            this.__airSSRFQTV.Add(new SSRFQTV() { cardno = cardno, name = name });
        }


        /// <summary>
        /// 添加成人旅客姓名.
        /// <code>
        /// 如addAdult("ZHANG/QIANG")或addAdult("孙家浩")，输入英文名称ZHANG/QIANG或中文名称孙家浩的姓名 
        /// </code>
        /// </summary>
        /// <param name="name">The name.</param>
        public void addAdult(string name) {
            this.__adultList.Add( name);
        }

        /// <summary>
        /// 添加普通航段.
        /// <code>
        /// 如addAirSegNormal("CA1301",'Y',"PEK","CAN","NN",1,"2002-10-20")，申请订取CA1301航班,Y舱,20OCT,北京到广州的一个座位
        /// </code>
        /// </summary>
        /// <param name="airseg">The airseg.</param>
        public void addAirSeg(BookAirSeg airseg) {
            this.__airSegList.Add(airseg);
        }

        /// <summary>
        /// 添加儿童姓名.
        /// </summary>
        /// <param name="name">The name.</param>
        public void addChild(string name) {
            this.__childList.Add(name);
        }

        /// <summary>
        /// 添加联系组.
        /// <code>
        /// addContact("66017755-2509"),旅客联系电话为66017755-2509 
        /// </code>
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void addContact(BookContact contact) {
            this.__bookContactList.Add(contact);
        }

        /// <summary>
        /// 添加婴儿组.
        /// </summary>
        /// <param name="infant">The infant.</param>
        public void addInfant(BookInfant infant) {
            infant.getcarrierName=this.__adultList.Contains(infant.getcarrierName)?string.Format(@"P{0}",this.__adultList.IndexOf(infant.getcarrierName)+1):infant.getcarrierName;
            this.__infantList.Add(infant);
        }

        /// <summary>
        /// 添加婴儿组.
        /// </summary>
        /// <param name="birth">The birth.</param>
        /// <param name="carrierName">Name of the carrier.</param>
        /// <param name="name">The name.</param>
        public void addInfant(DateTime birth, String carrierName, String name) {
            addInfant(new BookInfant() {getbirth=birth,getcarrierName=carrierName,getname=name });
        }

        /// <summary>
        /// 结果错误检查及异常抛出.
        /// </summary>
        /// <param name="Msg"></param>
        protected override void ThrowExeption(string Msg) {
            Msg = Msg.Trim();
            if (Regex.IsMatch(Msg, @"\*DATE\*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new DateFormatException();
            if (Regex.IsMatch(Msg, @"\*ACTION\s*CODE\s*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new SSActionCodeException();
            if (Regex.IsMatch(Msg, @"^AIRLINE", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new SSAirlineException();
            if (Regex.IsMatch(Msg, @"^FLIGHT NUMBER", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new SSFltNumberException();
            if (Regex.IsMatch(Msg, @"超出字库GB2312", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new SSInvalidCharException();
            if (Regex.IsMatch(Msg, @"^NAME\s+LENGTH", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                throw new SSNameLengthException();
            if (Regex.IsMatch(Msg, @"^OFFICE", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new SSOfficeException();
        }

        /// <summary>
        /// 客户端提交.
        /// </summary>
        /// <returns></returns>
        public virtual ASyncResult Commit() {
            if (this.setTimelimit == null)
                throw new SSTicketMissingException();
            if (this.__bookContactList.Count <= 0)
                throw new SSContactMissingException();
            if (this.__adultList.Count <= 0 && this.__childList.Count <= 0 && this.__infantList.Count <= 0)
                throw new SSNameMissingException();
            if (this.setTimelimit >= this.__airSegList[0].departureTime)
                throw new SSTktDateException();
            ASyncResult result= GetSyncResult(this.createSynCmd());
            //if (this.IsSpecial)
            //    result = GetSyncResult((string.IsNullOrEmpty(this.setEnvelopType) ? "@" : this.setEnvelopType));
            return result;
        }
        #endregion

        #region 拼装指令
        /// <summary>
        /// 拼装指令.
        /// </summary>
        /// <returns></returns>
        protected virtual string createSynCmd() {
            //return "SS:CA1537/Y/1OCT/PEKNKGNN3/1225 1400";
            return string.Format("{0}{1}{2}{3}{4}\r{5}"
                , createBookAirSeg()
                , createPassger()
                , createTimelimit()
                , createContact()
                , createSSRFOID()
                , string.IsNullOrEmpty(this.setEnvelopType)?"@":this.setEnvelopType
                );
        }

        /// <summary>
        /// 拼装航段令组.
        /// </summary>
        /// <returns></returns>
        protected virtual string createBookAirSeg() {
            StringBuilder sb = new StringBuilder();
            foreach (BookAirSeg airSeg in this.__airSegList) {
                sb.AppendFormat("SS:{0} {1} {2} {3}{4} {5}{6}\r"
                    , airSeg.getairNo
                    ,airSeg.getfltClass.ToString()
                    , string.Format("{0}{1}{2}", airSeg.departureTime.Day.ToString("D2"), getDateString(airSeg.departureTime), airSeg.departureTime.ToString("yy"))
                    ,airSeg.getorgCity
                    ,airSeg.getdesCity
                    ,string.IsNullOrEmpty( airSeg.getactionCode)?"NN":airSeg.getactionCode
                    ,this.gettktNum
                    );
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates the passger.
        /// </summary>
        /// <returns></returns>
        protected virtual string createPassger() {
            StringBuilder sb = new StringBuilder("NM");
            foreach (string p in this.__adultList) {
                sb.AppendFormat("1{0}", p);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates the timelimit.
        /// </summary>
        /// <returns></returns>
        protected virtual string createTimelimit() {
            return string.Format("\rTKTL/{0}/{1}/{2}"
                ,this.setTimelimit.Value.ToString("hhmm")
                , string.Format("{0}{1}{2}", this.setTimelimit.Value.Day.ToString("D2"), getDateString(this.setTimelimit.Value), this.setTimelimit.Value.ToString("yy"))
                ,this.setOffice
                );
        }

        /// <summary>
        /// Creates the SSRFOID.
        /// </summary>
        /// <returns></returns>
        protected virtual string createSSRFOID() {
            StringBuilder sb = new StringBuilder();
            foreach (SSRFOID foid in this.__airSSRFOID) {
                sb.AppendFormat("\rSSR FOID {0} HK/{1}{2}/{3}", foid.airline, foid.Passageer.IdType, foid.Passageer.Id, foid.Passageer.Name);        
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates the contact.
        /// </summary>
        /// <returns></returns>
        protected virtual string createContact() {
            StringBuilder sb = new StringBuilder();
            foreach (BookContact c in this.__bookContactList) {
                sb.AppendFormat("\rCT:{0} {1} {2}", c.getcity, c.psgrName, c.getcontact);
                if (!string.IsNullOrEmpty(c.getcontact))
                    sb.AppendFormat("\rRMK MP {0}/P1", c.getcontact);
            }
            return sb.ToString();
        }
        #endregion
    }
}
