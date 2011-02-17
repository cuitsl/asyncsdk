using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Util;
using System.IO;
using eTerm.AsyncSDK.Net;
using System.Net;

namespace ASync.WeatherPlugIn {
    [AfterASynCommand("!GetWeather")]
    public sealed class GetWeather : BaseASyncPlugIn {
        /// <summary>
        /// Executes the plug in.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            using (FileStream fs = new FileStream(@"Weather.BIN", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                BinaryReader br = new BinaryReader(fs);
                byte[] buffer = new byte[fs.Length];
                br.Read(buffer, 0, buffer.Length);
                WeaterCityVersion version = new WeaterCityVersion().DeXmlSerialize(TEACrypter.GetDefaultKey, buffer);
                Match cityName = Regex.Match(Encoding.GetEncoding("gb2312").GetString(SESSION.UnOutPakcet(InPacket)).Trim(), @"([\u4E00-\u9FA5]+)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                try {
                    // Create the query.
                    IEnumerable<WeatherCity> queryCitys =
                        from entity in version.WebCityList
                        where entity.CityName == cityName.Value
                        select entity;
                    foreach(WeatherCity city in queryCitys) {
                        WeatherCity CityCode = version.WebCityList.SingleOrDefault<WeatherCity>(CityDetail => CityDetail.ParentId == city.CityId);
                        if(CityCode==null)continue;
                        List<WeatherResult> WeatherResultLst = getResult(getWeater(CityCode.CityName));
                        if (WeatherResultLst.Count > 0) {
                            StringBuilder sb = new StringBuilder();
                            foreach (WeatherResult WeatherItem in WeatherResultLst) {
                                sb.AppendFormat("{0} {1} {2}摄氏度-{3}摄氏度", WeatherItem.Day, WeatherItem.WeekDay, WeatherItem.MinTemperature, WeatherItem.MaxTemperature);
                                break;
                            }
                            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, sb.ToString()));
                        }
                        else {
                            SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, "无天气信息"));
                        }
                        return;
                    }
                }
                catch (Exception ex) {
                    SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, ex.Message));
                    return;
                }
                br.Close();
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, "无该城市信息"));
            }
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="Html">The HTML.</param>
        /// <returns></returns>
        private List<WeatherResult> getResult(string Html) {
            List<WeatherResult> result = new List<WeatherResult>();
            int indexOf = 0;
            MatchCollection temMatchs = Regex.Matches(Html, @"(高温|低温)\s*<strong>(\d+)<strong>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match dayMatch in Regex.Matches(Html, @"(\d+)日(星期[\u4E00-\u9FA5])", RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
                WeatherResult singleResult = new WeatherResult() { Day=string.Format(@"{0}日",dayMatch.Groups[1].Value), WeekDay=dayMatch.Groups[2].Value };
                singleResult.MinTemperature = temMatchs[indexOf].Groups[2].Value;
                if (temMatchs.Count % 2 == 0 && indexOf == 0) {
                }
                else {
                    singleResult.MaxTemperature = temMatchs[indexOf+1].Groups[2].Value;
                }

                result.Add(singleResult);
                indexOf++;
            }
            return result;
        }

        /// <summary>
        /// Gets the weater.
        /// </summary>
        /// <param name="CityCode">The city code.</param>
        /// <returns></returns>
        private string getWeater(string CityCode) {
            //http://www.weather.com.cn/weather/101250201.shtml
            string result = string.Empty;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(string.Format(@"http://www.weather.com.cn/weather/{0}.shtml", CityCode));
            using (HttpWebResponse myRes = (HttpWebResponse)myReq.GetResponse()) {
                using (Stream stream = myRes.GetResponseStream()) {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    result = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Validates the plug in.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        protected override bool ValidatePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            return Key.AllowIntercept;
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public override string Description {
            get {
                return "获取城市天气 格式:!GetWeather 上海";
            }
        }
    }
}
