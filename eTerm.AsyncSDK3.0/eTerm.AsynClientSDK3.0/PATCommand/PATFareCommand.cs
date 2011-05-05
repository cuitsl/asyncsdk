using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// 税收获取指令
    /// </summary>
    public sealed class PATFareCommand : SSCommand {


        /// <summary>
        /// 是否一次性
        /// </summary>
        private bool IsOneOff = false;

        /// <summary>
        /// 拼装指令.
        /// </summary>
        /// <returns></returns>
        protected override string createSynCmd() {
            return string.Format("{0}"
                , createBookAirSeg()
                //,""//无需旅客信息可PAT
                //, createPassger()
                //, createTimelimit()
                //, createContact()
                //, createSSRFOID()
                //, string.IsNullOrEmpty(this.setEnvelopType) ? "@" : this.setEnvelopType
            );
        }

        /// <summary>
        /// 订座数.
        /// </summary>
        /// <value>The gettkt num.</value>
        protected override int gettktNum {
            get {
                return 1;
            }
        }

        /// <summary>
        /// 客户端提交.
        /// </summary>
        /// <returns></returns>
        public override ASyncResult Commit() {
            ASyncResult Result = null;
            base.Connect();
            string PnrCode=string.Empty;
            SendStream(createSynCmd());
            if (Regex.IsMatch(@"一次性", ConvertResult(GetStream()), RegexOptions.IgnoreCase| RegexOptions.Multiline))
            {
                ThreadSleep();
                addAdult("胡李俊");
                addSSR_FOID("MU", "93747237293729462", "胡李俊");
                this.setTimelimit = airSegList[0].departureTime.AddSeconds(30 * 60);        //30分钟
                addContact(new BookContact("SHA", "12345678", "HULIJUN"));
                PnrCode = (Commit() as SSResult).getPnr;
                if (!string.IsNullOrEmpty(PnrCode)) SendStream(string.Format(@"RT:{0}", PnrCode));
                IsOneOff = true;
            }
            ThreadSleep();
            SendStream(@"PAT:A");
            IEnumerator<PATResult> PAT= new PATCommand().ParseSFC(ConvertResult(GetStream())).GetEnumerator();
            while (PAT.MoveNext()) {
                Dispose();
                return PAT.Current;
            }
            Dispose();
            if (IsOneOff) {
                new CPnrCommand().Commit(PnrCode);
            }
            return Result;
        }
    }
}
