using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace eTerm.ASynClientSDK {
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
            SendStream(createSynCmd());
            if (Regex.IsMatch(@"一次性", ConvertResult(GetStream()), RegexOptions.IgnoreCase| RegexOptions.Multiline))
            {
                ThreadSleep();
                SendStream(createOneOff());
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
            return Result;
        }
    }
}
