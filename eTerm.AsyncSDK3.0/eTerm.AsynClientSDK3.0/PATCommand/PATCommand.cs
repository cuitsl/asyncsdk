using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using System.Linq;
using System.Threading;
namespace eTerm.ASynClientSDK {

    /// <summary>
    /// 税收获取
    /// </summary>
    public sealed class PATCommand : ASynCommand {

        /// <summary>
        /// Initializes a new instance of the <see cref="PATCommand"/> class.
        /// </summary>
        public PATCommand() : base() { 
            
        }

        /// <summary>
        /// 客户端提交.
        /// </summary>
        /// <returns></returns>
        public ASyncResult Commit(string PnrCode) {
            ASyncResult result = null;
            base.Connect();
            SendStream(string.Format(@"RT:{0}", PnrCode));
            string Result = ConvertResult(GetStream());
            ThreadSleep();
            SendStream("PAT:A");
            IOrderedEnumerable<PATResult> r1 = ParseSFC(ConvertResult(GetStream()));
            ThreadSleep();
            IEnumerator<PATResult> Enumerator = r1.GetEnumerator();
            while (Enumerator.MoveNext()) {
                SendStream(string.Format(@"SFC:{0}", Enumerator.Current.Sequence));
                ThreadSleep();
                SendStream(@"@KI");
                result = ResultAdapter(ConvertResult(GetStream()));
                Dispose();
                return Enumerator.Current;
            }
            Dispose();
            return result;
        }

        /// <summary>
        /// Parses the SFC.
        /// </summary>
        /// <param name="Msg">The MSG.</param>
        /// <returns></returns>
        internal IOrderedEnumerable<PATResult> ParseSFC(string Msg) {
            List<PATResult> Result = new List<PATResult>();
            foreach (Match m in Regex.Matches(Msg, @"(\d+)\s+(([A-Za-z]|\+|\/)+)\s+FARE\:CNY(\d+\.\d+)\s+TAX\:CNY(\d+\.\d+)\s+YQ\:CNY(\d+\.\d+)\s+TOTAL\:(\d+\.\d+)")) {
                Result.Add(new PATResult() { 
                     ASynCmd=Msg,
                      CabinFare=float.Parse(m.Groups[4].Value),
                       CabinString=m.Groups[2].Value,
                        CabinTax=float.Parse( m.Groups[5].Value),
                         CabinTotalFare=float.Parse(m.Groups[7].Value),
                          CabinYQ=float.Parse(m.Groups[6].Value),
                           Sequence=m.Groups[1].Value
                });
            }
            return
                from entry in Result
                orderby entry.CabinTotalFare descending
                select entry
                ;
        }

        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            PATResult Pat = new PATResult();
            return Pat;
        }
    }
}
