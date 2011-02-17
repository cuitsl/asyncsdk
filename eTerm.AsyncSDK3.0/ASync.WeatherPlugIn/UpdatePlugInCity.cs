using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;
using System.Net;
using System.IO;
using eTerm.AsyncSDK.Net;
using System.Text.RegularExpressions;
using eTerm.AsyncSDK.Util;

namespace ASync.WeatherPlugIn {
    [AfterASynCommand("!weathercity")]
    public sealed class UpdatePlugInCity : BaseASyncPlugIn {
        /// <summary>
        /// Executes the plug in.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        protected override void ExecutePlugIn(eTerm.AsyncSDK.Core.eTerm363Session SESSION, eTerm.AsyncSDK.Core.eTerm363Packet InPacket, eTerm.AsyncSDK.Core.eTerm363Packet OutPacket, eTerm.AsyncSDK.AsyncLicenceKey Key) {
            try {
                List<WeatherCity> collection = new List<WeatherCity>();
                WeatherCity RootCity = new WeatherCity() { CityId = string.Empty, CityName = string.Empty };
                ReadCity(RootCity,collection);
                new WeaterCityVersion() { VersionDate=DateTime.Now, WebCityList=collection }.XmlSerialize(TEACrypter.GetDefaultKey, new FileInfo(@"Weather.BIN").FullName);
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, "城市更新成功"));
            }
            catch(Exception ex) {
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, ex.Message));
            }
        }

        /// <summary>
        /// Reads the city.
        /// </summary>
        /// <param name="ParentCity">The parent city.</param>
        private void ReadCity(WeatherCity ParentCity, List<WeatherCity> collection) {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(string.Format(@"http://www.weather.com.cn/data/list3/city{0}.xml", ParentCity.CityId));
            using (HttpWebResponse myRes = (HttpWebResponse)myReq.GetResponse()) {
                using (Stream stream = myRes.GetResponseStream()) {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    //250401|衡阳,250402|衡山,250403|衡东,250404|祁东,250405|衡阳县,250406|常宁,250407|衡南,250408|耒阳,250409|南岳
                    foreach (Match m in Regex.Matches(sr.ReadToEnd(), @"([0-9]+)\|([\u4E00-\u9FA5 0-9]+)", RegexOptions.IgnoreCase)) {
                        WeatherCity subCity = new WeatherCity() { ParentId=ParentCity.CityId, CityId = m.Groups[1].Value, CityName = m.Groups[2].Value, CityPinYin = Cn2PyUtil.FullConvert(m.Groups[2].Value) };
                        if (!Regex.IsMatch(subCity.CityName, @"^[0-9]+$")) {
                            ReadCity(subCity, collection);
                        }
                        collection.Add(subCity);
                    }
                    sr.Dispose();
                }
            }
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
                return "更新天气预报城市列表（从中国天气网更新）";
            }
        }
    }
}
