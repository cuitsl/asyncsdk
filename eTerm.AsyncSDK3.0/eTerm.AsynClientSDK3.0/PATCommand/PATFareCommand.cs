using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace eTerm.ASynClientSDK {
    public sealed class PATFareCommand : SSCommand {
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
            GetStream();
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
