using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.SynClientSDK.Base;
using SDKConfig = eTerm.SynClientSDK.SdkConfig;
namespace eTerm.SynClientSDK {
    public sealed class SdkSyncClient : ASynCommand {
        public SdkSyncClient() { 
            
        }

        /// <summary>
        /// Gets or sets the CMD stream.
        /// </summary>
        /// <value>The CMD stream.</value>
        public MessageStream CmdStream { get; set; }
        /// <summary>
        /// Gets or sets the expire socket.
        /// </summary>
        /// <value>The expire socket.</value>
        public double ExpireSocket { get; set; }
        /// <summary>
        /// Gets or sets the SDK.
        /// </summary>
        /// <value>The SDK.</value>
        public SdkConfig Sdk { get; private set; }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        /// <returns></returns>
        public new  bool Connect() {
            Sdk = SDKConfig.Current;
            return this.Connect(Sdk.Address, Sdk.Port,Sdk.UserName, Sdk.UserPass,Sdk.GroupCode);
        }

        /// <summary>
        /// Connects the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userPass">The user pass.</param>
        /// <param name="userGroup">The user group.</param>
        /// <returns></returns>
        public bool Connect(string host, int port, string userName, string userPass, string userGroup) {
            base.Connect(host, port, false);
            LogIn(userName, userPass);
            return true;
        }
        /// <summary>
        /// Reads the socket stream.
        /// </summary>
        /// <returns></returns>
        public MessageStream ReadSocketStream() {
            return ReadStream();
        }
        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <returns></returns>
        public MessageStream ReadStream() {
            byte[] buffer = GetStream();
            return new MessageStream(0x00, buffer);
        }
        /// <summary>
        /// Sends the CMD.
        /// </summary>
        /// <param name="Cmd">The CMD.</param>
        public void SendCmd(byte[] Cmd) {
            base.SendStream(Cmd);
        }
        /// <summary>
        /// Sends the CMD.
        /// </summary>
        /// <param name="Cmd">The CMD.</param>
        public void SendCmd(MessageStream Cmd) {
            base.SendStream(Cmd.StreamBody);
        }
        /// <summary>
        /// Sends the CMD.
        /// </summary>
        /// <param name="Code">The code.</param>
        /// <param name="Cmd">The CMD.</param>
        public void SendCmd(byte Code, byte[] Cmd) {
            base.SendStream(Cmd);
        }
        /// <summary>
        /// Sends the CMD.
        /// </summary>
        /// <param name="Code">The code.</param>
        /// <param name="Cmd">The CMD.</param>
        public void SendCmd(SocketResponseCode Code, byte[] Cmd) {
            base.SendStream(Cmd);
        }
    }
}
