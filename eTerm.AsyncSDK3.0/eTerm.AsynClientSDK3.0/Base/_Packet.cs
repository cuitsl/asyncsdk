using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace eTerm.AsyncSDK.Base {
    /// <summary>
    /// 数据包基类
    /// <remarks>
    ///     为实现通用数据包而定义
    ///     数据包依赖于会话端存在
    /// </remarks>
    /// </summary>
    /// <typeparam name="TSession">会话端类型（依赖类型）.</typeparam>
    public abstract class _Packet<TSession>:IDisposable
        where TSession:_Session,new ()
    {
        BinaryWriter __bw;
        BinaryReader __br;
        private byte[] __OriginalBytes;

        /// <summary>
        /// Initializes a new instance of the <see cref="_Packet&lt;TSession&gt;"/> class.
        /// </summary>
        public _Packet() {
            PacketStream = new MemoryStream();
            __bw = new BinaryWriter(PacketStream);
            __br = new BinaryReader(PacketStream);
        }


        /// <summary>
        /// 数据解码(适用不同类型客户端).
        /// </summary>
        /// <returns></returns>
        public virtual byte[] UnpackPakcet() {
            return this.__OriginalBytes;
        }

        /// <summary>
        /// 数据处理流.
        /// </summary>
        /// <value>The packet stream.</value>
        protected MemoryStream PacketStream { get; set; }

        /// <summary>
        /// 数据包对应的会话端.
        /// </summary>
        /// <value>The session.</value>
        public TSession Session { get; set; }

        /// <summary>
        /// 指令类型(需通过数据流分析而得).
        /// </summary>
        /// <value>The packet command.</value>
        public byte PacketCommand { get;protected set; }

        /// <summary>
        /// 数据包协议版本号(仅供查看).
        /// <remarks>
        ///     基类必须重写
        /// </remarks>
        /// </summary>
        /// <value>The T session version.</value>
        protected abstract byte TSessionVersion { get; }

        /// <summary>
        /// 数据包原始内容.
        /// <remarks>
        ///     子类如需解码或编码需对此属性操作
        /// </remarks>
        /// </summary>
        /// <value>The original bytes.</value>
        public virtual byte[] OriginalBytes { get { 
            if(__OriginalBytes==null)
                __OriginalBytes=this.PacketStream.ToArray();
            return __OriginalBytes;
        
        } set {
                PacketStream.Dispose();
                PacketStream = new MemoryStream(value);
        } }

        /// <summary>
        /// 数据包解码（默认不作操作，返回原始内容）.
        /// </summary>
        /// <returns></returns>
        public virtual byte[] DecryptBytes() {
            return this.OriginalBytes;
        }

        #region 读取
        /// <summary>
        /// 压入Byte数组,并将流中当前位置提升数组长度
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="index">value中从零开始的字节偏移量</param>
        /// <param name="count">要写入当前流的字节数</param>
        public void Put(Byte[] value, int index, int count){
            __bw.Write(value, index, count);
        }

        /// <summary>
        /// 写入单字节.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Put(byte value) {
            __bw.Write(value);
        }

        /// <summary>
        /// 写入字符.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Put(char value) {
            __bw.Write(value);
        }
        #endregion

        #region "读流方法"
        /// <summary>
        /// 读取布尔值,并将流中当前位置提升1
        /// </summary>
        /// <returns></returns>
        public bool GetBoolean() {
            return Get() == 0 ? false : true;
        }

        /// <summary>
        /// 读取Byte值,并将流中当前位置提升1
        /// </summary>
        /// <returns></returns>
        public byte Get() {
            return (byte)PacketStream.ReadByte();
        }
        /// <summary>
        /// 获取指定位置字节.
        /// </summary>
        /// <param name="index">索引号.</param>
        /// <returns></returns>
        public byte Get(int index) {
            int current = (int)this.PacketStream.Position;
            Seek(index, SeekOrigin.Begin);
            byte ret = Get();
            Seek(current, SeekOrigin.Begin);
            return ret;
        }

        /// <summary>
        /// 设置当前流中的位置
        /// </summary>
        /// <param name="offset">相对于origin参数字节偏移量</param>
        /// <param name="origin">System.IO.SeekOrigin类型值,指示用于获取新位置的参考点</param>
        /// <returns></returns>
        public virtual long Seek(int offset, SeekOrigin origin) {
            return this.PacketStream.Seek((long)offset, origin);
        }

        /// <summary>
        /// 读取count长度的Byte数组,并将流中当前位置提升count
        /// </summary>
        /// <param name="count">要从当前流中最多读取的字节数</param>
        /// <returns></returns>
        public byte[] GetByteArray(int count) {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            byte[] buffer = new byte[count];
            int num = PacketStream.Read(buffer, 0, count);
            return buffer;
        }

        /// <summary>
        /// 读取一个Char值,并将流中当前位置提升1
        /// </summary>
        /// <returns></returns>
        public char GetChar() {
            return (char)GetUShort();
        }
        /// <summary>
        /// 获取指令位置字符.
        /// </summary>
        /// <param name="index">索引号.</param>
        /// <returns></returns>
        public char GetChar(int index) {
            int current = (int)PacketStream.Position;
            Seek(index, SeekOrigin.Begin);
            char c = GetChar();
            Seek(current, SeekOrigin.Begin);
            return c;
        }

        /// <summary>
        /// 读取一个short值,并将流中当前位置提升2
        /// </summary>
        /// <returns></returns>
        public ushort GetUShort() {
            ushort ret = (ushort)(Get() << 8 | Get());
            return ret;
        }
        /// <summary>
        /// 从指定位置获取ushort数据.
        /// </summary>
        /// <param name="index">索引号.</param>
        /// <returns></returns>
        public ushort GetUShort(int index) {
            int current = (int)PacketStream.Position;
            Seek(index, SeekOrigin.Begin);
            ushort ret = GetUShort();
            Seek(current, SeekOrigin.Begin);
            return ret;
        }
        /// <summary>
        /// 读取一个Int值,并将流中当前位置提升4
        /// </summary>
        /// <returns></returns>
        public int GetInt() {
            int ret = (int)(Get() << 0x18 | Get() << 0x10 | Get() << 8 | Get());
            return ret;
        }
        /// <summary>
        /// 获取UInt.
        /// </summary>
        /// <returns></returns>
        public uint GetUInt() {
            return (uint)GetInt();
        }

        /// <summary>
        /// 读取一个Long值,并将流中当前位置提升8
        /// </summary>
        /// <returns></returns>
        public long GetLong() {
            uint num1 = (uint)GetInt();
            uint num2 = (uint)GetInt();
            return (long)((num1 << 0x20) | num2);
        }

        #endregion

        /// <summary>
        /// 解包分包.
        /// <remarks>
        ///     可在此处分数据包需要的内容，如：指令类型、指令长度等
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetPacketBodyBytes();

        /// <summary>
        /// 指令序号.
        /// <remarks>
        ///     为线程安全自动递增
        /// </remarks>
        /// </summary>
        /// <value>The sequence.</value>
        public long Sequence { get; set; }

        /// <summary>
        /// 数据包结尾标志字节.
        /// </summary>
        /// <value>The after body.</value>
        public abstract byte[] AfterBody { get; }

        /// <summary>
        /// 数据包收取时间.
        /// </summary>
        /// <value>The packet date time.</value>
        public DateTime? PacketDateTime { get; set; }

        /// <summary>
        /// 分析数据有效长度.
        /// </summary>
        /// <returns></returns>
        public abstract int GetPakcetLength();

        /// <summary>
        /// 分析指令类型.
        /// </summary>
        /// <returns></returns>
        public abstract void GetPacketCommand();

        /// <summary>
        /// 验证数据有效性.
        /// </summary>
        /// <returns></returns>
        public abstract bool ValidatePacket();

        #region IDisposable
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) {
            {
                if (disposing) {
                    // Release managed resources
                    if (PacketStream != null) {
                        __bw.Close();
                        __br.Close();
                        PacketStream.Close();
                        PacketStream.Dispose();
                        PacketStream = null;
                    }
                }
                // Release unmanaged resources
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
