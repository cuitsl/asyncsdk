using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Core;
using eTerm.AsyncSDK.Net;

namespace eTerm.AsyncSDK.Base {

    /// <summary>
    /// 后继指令属性拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true,Inherited=true)]
    public sealed class AfterASynCommandAttribute:Attribute {

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterASynCommandAttribute"/> class.
        /// </summary>
        /// <param name="Cmd">The CMD.</param>
        public AfterASynCommandAttribute(string Cmd) {
            ASynCommand = Cmd;
        }

        /// <summary>
        /// 指令处理.
        /// </summary>
        /// <value>The A syn command.</value>
        public string ASynCommand { get; set; }

        /// <summary>
        /// 说明.
        /// </summary>
        /// <value>The attribute description.</value>
        public string AttributeDescription { get; set; }

        /// <summary>
        /// 是否为系统插件（决定是需在UI上呈现给用户查看）.
        /// </summary>
        /// <value><c>true</c> if this instance is system; otherwise, <c>false</c>.</value>
        public bool IsSystem { get; set; }
    }

    /// <summary>
    /// 基类
    /// </summary>
    public abstract class BaseAfterCmd : IAfterCommand<eTerm443Async, eTerm443Packet> {

        /// <summary>
        /// 执行代理
        /// </summary>
        public delegate void InvokePacket(eTerm443Async SESSION, eTerm443Packet InPacket, eTerm443Packet OutPacket,AsyncLicenceKey Key);

        InvokePacket __Invoke;
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAfterCmd"/> class.
        /// </summary>
        protected BaseAfterCmd() {
            __Invoke = new InvokePacket(InvokeBase);
        }

        /// <summary>
        /// Invokes the base.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        private void InvokeBase(eTerm443Async SESSION, eTerm443Packet InPacket, eTerm443Packet OutPacket, AsyncLicenceKey Key) {
            if (!ValidatePlugIn(SESSION, InPacket, OutPacket, Key)) return;
            ExecutePlugIn(SESSION, InPacket, OutPacket,Key);
        }

        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected abstract void ExecutePlugIn(eTerm443Async SESSION, eTerm443Packet InPacket, eTerm443Packet OutPacket, AsyncLicenceKey Key);


        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        /// <param name="Session">The session.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>   
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public IAsyncResult BeginExecute(AsyncCallback callBack, eTerm443Async Session, eTerm443Packet InPacket, eTerm443Packet OutPacket, AsyncLicenceKey Key) {
            try {
                return __Invoke.BeginInvoke(Session, InPacket, OutPacket,Key, callBack, Session);
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        }

        /// <summary>
        /// 验证可用性.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        protected abstract bool ValidatePlugIn(eTerm443Async SESSION, eTerm443Packet InPacket, eTerm443Packet OutPacket, AsyncLicenceKey Key);


        /// <summary>
        /// 结束线程.
        /// </summary>
        /// <param name="iar">The iar.</param>
        public void EndExecute(IAsyncResult iar) {
            if (iar == null) return;
            try {
                __Invoke.EndInvoke(iar);
                iar.AsyncWaitHandle.Close();
            }
            catch {
                // Hide inside method invoking stack 
                //throw e;
            }
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description {
            get {
                return "默认指令说明";
            }
        }

        /// <summary>
        /// 插件开发者信息.
        /// </summary>
        /// <value>The copy right.</value>
        public virtual string CopyRight {
            get {
                return string.Format(@"胡李俊(FORCEN HU) 
邮箱：FORCE0908@GMAIL.COM 
电话：13524008692 
QQ:29742914
MSN:VALON0908@HOTMAIL.COM", "");
            }
        }

    }



    /// <summary>
    /// 基类
    /// </summary>
    public abstract class BaseASyncPlugIn : IAfterCommand<eTerm363Session, eTerm363Packet> {

        /// <summary>
        /// 执行代理
        /// </summary>
        public delegate void InvokePacket(eTerm363Session SESSION, eTerm363Packet InPacket, eTerm363Packet OutPacket, AsyncLicenceKey Key);

        InvokePacket __Invoke;
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAfterCmd"/> class.
        /// </summary>
        protected BaseASyncPlugIn() {
            __Invoke = new InvokePacket(InvokeBase);
        }

        /// <summary>
        /// Invokes the base.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        private void InvokeBase(eTerm363Session SESSION, eTerm363Packet InPacket, eTerm363Packet OutPacket, AsyncLicenceKey Key) {
            if (!ValidatePlugIn(SESSION, InPacket, OutPacket, Key)) {
                SESSION.SendPacket(__eTerm443Packet.BuildSessionPacket(SESSION.SID, SESSION.RID, @"服务器不允许该插件"));
                return;
            }
            ExecutePlugIn(SESSION, InPacket, OutPacket, Key);
        }

        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="SESSION">会话端.</param>
        /// <param name="InPacket">入口数据包.</param>
        /// <param name="OutPacket">出口数据包.</param>
        /// <param name="Key">The key.</param>
        protected abstract void ExecutePlugIn(eTerm363Session SESSION, eTerm363Packet InPacket, eTerm363Packet OutPacket, AsyncLicenceKey Key);


        /// <summary>
        /// 开始线程.
        /// </summary>
        /// <param name="callBack">The call back.</param>
        /// <param name="Session">The session.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public IAsyncResult BeginExecute(AsyncCallback callBack, eTerm363Session Session, eTerm363Packet InPacket, eTerm363Packet OutPacket, AsyncLicenceKey Key) {
            try {
                return __Invoke.BeginInvoke(Session, InPacket, OutPacket, Key, callBack, Session);
            }
            catch (Exception e) {
                // Hide inside method invoking stack 
                throw e;
            }
        }

        /// <summary>
        /// 验证可用性.
        /// </summary>
        /// <param name="SESSION">The SESSION.</param>
        /// <param name="InPacket">The in packet.</param>
        /// <param name="OutPacket">The out packet.</param>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        protected abstract bool ValidatePlugIn(eTerm363Session SESSION, eTerm363Packet InPacket, eTerm363Packet OutPacket, AsyncLicenceKey Key);


        /// <summary>
        /// 结束线程.
        /// </summary>
        /// <param name="iar">The iar.</param>
        public void EndExecute(IAsyncResult iar) {
            if (iar == null) return;
            try {
                __Invoke.EndInvoke(iar);
                iar.AsyncWaitHandle.Close();
            }
            catch {
                // Hide inside method invoking stack 
                //throw e;
            }
        }

        /// <summary>
        /// 指令说明.
        /// </summary>
        /// <value>The description.</value>
        public virtual string Description {
            get {
                return "默认指令说明";
            }
        }

        /// <summary>
        /// 插件开发者信息.
        /// </summary>
        /// <value>The copy right.</value>
        public virtual string CopyRight {
            get {
                return string.Format(@"胡李俊(FORCEN HU) 
邮箱：FORCE0908@GMAIL.COM 
电话：13524008692 
QQ:29742914
MSN:VALON0908@HOTMAIL.COM", "");
            }
        }

    }

}
