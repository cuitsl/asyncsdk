﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.ASynClientSDK.Base;
using eTerm.ASynClientSDK;
using System.Text.RegularExpressions;
using eTerm.ASynClientSDK.AVException;

namespace eTerm.ASynClientSDK {
    /// <summary>
    /// AV指令对象
    /// <remarks>
    /// 提供简单有效的航班信息实时查询通道 查询指定日期及航线上的可用航班信息支持多种查询参数 结果数据以类AvResult的形式表示
    /// </remarks>
    /// <code>
    /// AVCommand Av = new AVCommand();
    /// SyncResult r = Av.getAvailability("SHA", "CTU", DateTime.Now.AddMonths(1),string.Empty,true);
    /// </code>
    /// <example>
    /// AVCommand Av = new AVCommand();
    /// SyncResult r = Av.getAvailability("SHA", "CTU", DateTime.Now.AddMonths(1),string.Empty,true);
    /// </example>
    /// </summary>
    public sealed class AVCommand : ASyncPNCommand {

        #region 变量定义
        private string queryDate = string.Empty;
        private string __AvCommand = string.Empty;
        AVResult avResult = new AVResult();
        #endregion


        #region 重写
        /// <summary>
        /// 是否还有下页数据（将自动执行“PnCommand”）.
        /// </summary>
        /// <param name="msgBody">当前指令结果.</param>
        /// <returns></returns>
        /// <value><c>true</c> if [exist next page]; otherwise, <c>false</c>.</value>
        protected override bool ExistNextPage(string msgBody) {
            return Regex.IsMatch(msgBody, @"7\+");
        }

        /// <summary>
        /// 异常抛出.
        /// </summary>
        /// <param name="Msg"></param>
        protected override void ThrowExeption(string Msg) {
            if (Regex.IsMatch(Msg, @"\*NO\sROUTING\*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new AVNoRoutingException();
            if (Regex.IsMatch(Msg, @"CITY\sPAIR", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new AVCityPairException();
            if (Regex.IsMatch(Msg, @"\*CITY\*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                throw new AVCityPairException();
        }

        /// <summary>
        /// 生成指令并发送分析(子类必须重写).
        /// </summary>
        /// <param name="SynCmd">eTerm实质指令.</param>
        /// <returns></returns>
        protected override ASyncResult GetSyncResult(string SynCmd) {
            __AvCommand = SynCmd;
            return base.GetSyncResult(SynCmd);
        }

        /// <summary>
        /// 指令结果解析适配器.
        /// </summary>
        /// <param name="Msg">指令结果集合.</param>
        /// <returns></returns>
        protected override ASyncResult ResultAdapter(string Msg) {
            AVResult result = new AVResult();
            foreach (Flight seg in new AnalysisAVH().ParseAVH(this.__AvCommand, Msg).Flights) {
                AvItem AvSegment = new AvItem() { 
                 getAirline=seg.FlightNO,
                  getArritime=seg.ArrivalTime,
                   getCarrier=seg.Airline,
                    getDeptime=seg.DepartureTime,
                     getDstcity=seg.ArrivalAirport,
                      getMeal=seg.Meal.Trim().Length>0,
                       getPlanestyle=seg.AircraftType,
                        getOrgcity=seg.DepartureAirport,
                         getStopnumber=int.Parse( seg.Stop),
                          isCodeShare=seg.CodeShare,
                           getLink=seg.Connect
                };
                foreach (FlightCarbin carbin in seg.Carbins) {
                    AvSegment.getCabins.Add(new AvItemCabinChar() {
                         getCode=carbin.Carbin,
                          getAvalibly=carbin.Number,
                    });
                }
                result.AvSegment.Add(AvSegment);
            }
            return result;
        }
        #endregion
    }
}
