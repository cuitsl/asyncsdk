using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK;

namespace AsyncAPI3._0Tst
{
    public class EBTEtermService
    {
        private WinSocket socket = new WinSocket(@"guzm", @"guzm", @"7887BAFE56C6F23C6FA1CF3E9B4FC05D");

        public EBTEtermService()
        {
            socket.OnAsynConnect += fff;
            socket.OnBeginConnect += fff;
            socket.OnAsyncDisconnect += fff;
            socket.OnPacketSent += fff;
            socket.OnReadPacket += fff;
            socket.OnValidated += fff;

            socket.Connect(@"127.0.0.1", 350);
        }

        private void ttt(object sender, AsyncEventArgs<eTermApiPacket, eTermApiPacket, WinSocket> e)
        {
            string ss = "";
        }
        private void fff(object sender, EventArgs e)
        {
            string ss = "";
        }

        #region GetRtResult：返回提取PNR的结果
        /// <summary>
        /// 返回提取PNR的结果
        /// </summary>
        /// <param name="strPnr"></param>
        /// <returns></returns>
        public void GetRtResult(string strPnr)
        {
            strPnr = strPnr.Trim();

            try
            {
                socket.SendPacket("RT" + strPnr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

}
