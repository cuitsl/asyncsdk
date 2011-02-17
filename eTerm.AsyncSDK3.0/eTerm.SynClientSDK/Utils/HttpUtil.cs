using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using System.IO;

namespace eTerm.SynClientSDK.Utils {
    /// <summary>
    /// HTTP请求
    /// </summary>
    public static class HttpUtil {
        public static string Referer = "http://passport.csdn.net";

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <returns></returns>
        public static string GetHtml(string URL) {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;
            wrp = wrt.GetResponse();
            return new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <param name="cookie">The cookie.</param>
        /// <returns></returns>
        public static string GetHtml(string URL, out string cookie) {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;

            wrp = wrt.GetResponse();

            string html = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            cookie = wrp.Headers.Get("Set-Cookie");
            return html;
        }
        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public static string GetHtml(string URL, string postData, string cookie, out string header) {
            return GetHtml("http://passport.csdn.net", URL, postData, cookie, out header);
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="URL">The URL.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public static string GetHtml(string server, string URL, string postData, string cookie, out string header) {
            byte[] byteRequest = Encoding.Default.GetBytes(postData);
            return GetHtml(server, URL, byteRequest, cookie, out header);
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="URL">The URL.</param>
        /// <param name="byteRequest">The byte request.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public static string GetHtml(string server, string URL, byte[] byteRequest, string cookie, out string header) {
            byte[] bytes = GetHtmlByBytes(server, URL, byteRequest, cookie, out header);
            Stream getStream = new MemoryStream(bytes);
            StreamReader streamReader = new StreamReader(getStream, Encoding.UTF8);
            string getString = streamReader.ReadToEnd();
            streamReader.Close();
            getStream.Close();
            return getString;
        }


        /// <summary>
        /// Gets the HTML by bytes.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="URL">The URL.</param>
        /// <param name="byteRequest">The byte request.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public static byte[] GetHtmlByBytes(string server, string URL, byte[] byteRequest, string cookie, out string header) {
            long contentLength;
            HttpWebRequest httpWebRequest;
            HttpWebResponse webResponse;
            Stream getStream;

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);

            httpWebRequest.CookieContainer = co;

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //httpWebRequest.Accept =
            //    "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            //httpWebRequest.Referer = "http://passport.csdn.net";
            //httpWebRequest.UserAgent =
            //    "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentLength = byteRequest.Length;
            Stream stream;
            stream = httpWebRequest.GetRequestStream();
            stream.Write(byteRequest, 0, byteRequest.Length);
            stream.Close();
            webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            header = webResponse.Headers.ToString();
            getStream = webResponse.GetResponseStream();
            contentLength = webResponse.ContentLength;

            byte[] outBytes = new byte[contentLength];
            outBytes = ReadFully(getStream);
            getStream.Close();
            return outBytes;
        }


        /// <summary>
        /// Reads the fully.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream stream) {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream()) {
                while (true) {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public static string GetHtml(string URL, string cookie, out string header) {
            return GetHtml(URL, cookie, out header, HttpUtil.Referer);
        }


        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public static string GetHtml(string URL, string cookie, out string header, string server) {
            HttpWebRequest httpWebRequest;
            HttpWebResponse webResponse;
            Stream getStream;
            StreamReader streamReader;
            string getString = "";
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Referer =HttpUtil.Referer;
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);
            httpWebRequest.CookieContainer = co;
            //httpWebRequest.UserAgent =
            //    "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "GET";
            webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            header = webResponse.Headers.ToString();
            getStream = webResponse.GetResponseStream();
            streamReader = new StreamReader(getStream, Encoding.UTF8);
            getString = streamReader.ReadToEnd();

            streamReader.Close();
            getStream.Close();
            return getString;
        }


        #region --stream--
        /// <summary>
        /// Gets the stream by bytes.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="URL">The URL.</param>
        /// <param name="byteRequest">The byte request.</param>
        /// <param name="cookie">The cookie.</param>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        public static Stream GetStreamByBytes(string server, string URL, byte[] byteRequest, string cookie,
                                              out string header) {
            Stream stream = new MemoryStream(GetHtmlByBytes(server, URL, byteRequest, cookie, out header));
            return stream;
        }
        #endregion

    }
}
