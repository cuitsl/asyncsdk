using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace ASync.eTermCommand.Utils {
    /// <summary>
    /// TEA加密码算法
    /// </summary>
    public sealed class TEACrypter
    {
        static MD5 MD5Instance = System.Security.Cryptography.MD5.Create();
        /// <summary>
        /// 当前的明文块
        /// </summary>
        private byte[] plain;
        /// <summary>
        /// 前面的明文块
        /// </summary>
        private byte[] prePlain;
        /// <summary>
        /// 输出的密文或明文
        /// </summary>
        private byte[] outData;
        /// <summary>
        /// 当前加密的密文位置和上一次加密的密文块位置，它们相差8
        /// </summary>
        private int crypt, preCrypt;
        /// <summary>
        /// 当前处理的加密解密块的位置
        /// </summary>
        private int pos;
        /// <summary>
        /// 填充数（长度）
        /// </summary>
        private int padding;
        /// <summary>
        /// 密钥
        /// </summary>
        private byte[] key;
        /// <summary>
        /// 用于加密时，表示当前是否是第一个8字节块，因为加密算法是反馈的,
        /// 但是最开始的8个字节没有反馈可用，所以要标明这种情况。
        /// </summary>
        private bool header = true;
        /// <summary>
        /// 这个表示当前解密开始的位置，之所以要这么一个变量是为了避免当解密到最后时
        /// 后面已经没有数据，这时候就会出错，这个变量就是用来判断这种情况免得出错
        /// </summary>
        private int contextStart;
        /// <summary>
        /// 随机数对象
        /// </summary>
        private static Random random = new Random();
        /// <summary>
        /// 字节输出流
        /// </summary>
        private byte[] baos;
        private int baosPos = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="TEACrypter"/> class.
        /// </summary>
        public TEACrypter()
        {
            baos = new byte[8];
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inData">密文</param>
        /// <param name="offset">密文开始的位置</param>
        /// <param name="len">密文长度</param>
        /// <param name="k">密钥</param>
        /// <returns>明文</returns>
        public byte[] Decrypt(byte[] inData, int offset, int len, byte[] k)
        {
            // 检查密钥
            if (k == null)
                return null;

            crypt = preCrypt = 0;
            this.key = k;
            int count;
            byte[] m = new byte[offset + 8];
            // 因为QQ消息加密之后至少是16字节，并且肯定是8的倍数，这里检查这种情况
            if ((len % 8 != 0) || (len < 16))
            {
                return null;
            }
            // 得到消息的头部，关键是得到真正明文开始的位置，这个信息存在第一个字节里面，所以其用解密得到的第一个字节与7做与
            prePlain = Decipher(inData, offset);
            pos = prePlain[0] & 0x7;
            // 得到真正明文的长度
            count = len - pos - 10;
            //如果明文长度小于0，那肯定是出错了，比如传输错误之类的，返回
            if (count < 0) return null;
            // 这个是临时的preCrypt，和加密时第一个8字节块没有prePlain一样，解密时
            // 第一个8字节块也没有preCrypt，所有这里建一个全0的
            for (int i = offset; i < m.Length; i++)
                m[i] = 0;
            // 通过了上面的代码，密文应该是没有问题了，我们分配输出缓冲区
            outData = new byte[count];
            // 设置preCrypt的位置等于0，注意目前的preCrypt位置是指向m的，因为java没有指针，所以我们在后面要控制当前密文buf的引用
            preCrypt = 0;
            // 当前的密文位置，为什么是8不是0呢？注意前面我们已经解密了头部信息了，现在当然该8了
            crypt = 8;
            // 自然这个也是8
            contextStart = 8;
            // 加1，和加密算法是对应的
            pos++;
            // 开始跳过头部，如果在这个过程中满了8字节，则解密下一块
            // 因为是解密下一块，所以我们有一个语句 m = in，下一块当然有preCrypt了，我们不再用m了
            // 但是如果不满8，这说明了什么？说明了头8个字节的密文是包含了明文信息的，当然还是要用m把明文弄出来
            // 所以，很显然，满了8的话，说明了头8个字节的密文除了一个长度信息有用之外，其他都是无用的填充
            padding = 1;
            while (padding <= 2)
            {
                if (pos < 8)
                {
                    pos++;
                    padding++;
                }
                if (pos == 8)
                {
                    m = inData;
                    if (!Decrypt8Bytes(inData, offset, len)) return null;
                }
            }
            // 这里是解密的重要阶段，这个时候头部的填充都已经跳过了，开始解密
            // 注意如果上面一个while没有满8，这里第一个if里面用的就是原始的m，否则这个m就是in了
            int index = 0;
            while (count != 0)
            {
                if (pos < 8)
                {
                    outData[index] = (byte)(m[offset + preCrypt + pos] ^ prePlain[pos]);
                    index++;
                    count--;
                    pos++;
                }
                if (pos == 8)
                {
                    m = inData;
                    preCrypt = crypt - 8;
                    if (!Decrypt8Bytes(inData, offset, len)) return null;
                }
            }

            // 最后的解密部分，上面一个while已经把明文都解出来了，就剩下尾部的填充了，应该全是0
            // 所以这里有检查是否解密了之后是不是0，如果不是的话那肯定出错了，返回null
            for (padding = 1; padding < 8; padding++)
            {
                if (pos < 8)
                {
                    if ((m[offset + preCrypt + pos] ^ prePlain[pos]) != 0)
                    {
                        return null;
                    }
                    pos++;
                }
                if (pos == 8)
                {
                    m = inData;
                    preCrypt = crypt;
                    if (!Decrypt8Bytes(inData, offset, len)) return null;
                }
            }
            return outData;
        }

        /// <summary>
        /// 需要被解密的密文
        /// </summary>
        /// <param name="inData">密文</param>
        /// <param name="k">密钥</param>
        /// <returns>已解密的消息</returns>
        public byte[] Decrypt(byte[] inData, byte[] k)
        {
            return Decrypt(inData, 0, inData.Length, k);
        }

        /// <summary>
        /// 把字节数组从offset开始的len个字节转换成一个unsigned int，
        /// 	<remark>abu 2008-02-15 14:47 </remark>
        /// </summary>
        /// <param name="inData">字节数组</param>
        /// <param name="offset">从哪里开始转换.</param>
        /// <param name="len">转换长度, 如果len超过8则忽略后面的.</param>
        /// <returns></returns>
        public static uint GetUInt(byte[] inData, int offset, int len) {
            uint ret = 0;
            int end = 0;
            if (len > 8)
                end = offset + 8;
            else
                end = offset + len;
            for (int i = 0; i < end; i++) {
                ret <<= 8;
                ret |= (uint)inData[i];
            }
            return ret;
        }



        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inData">明文字节数组</param>
        /// <param name="offset">开始加密的偏移</param>
        /// <param name="len">加密长度</param>
        /// <param name="k">密钥</param>
        /// <returns>密文字节数组</returns>
        public byte[] Encrypt(byte[] inData, int offset, int len, byte[] k)
        {
            if (k == null)
            {
                return null;
            }
            plain = new byte[8];
            prePlain = new byte[8];
            pos = 1;
            padding = 0;
            crypt = preCrypt = 0;
            this.key = k;
            header = true;

            // 计算头部填充字节数
            pos = (len + 0x0A) % 8;
            if (pos != 0)
                pos = 8 - pos;
            // 计算输出的密文长度
            outData = new byte[len + pos + 10];
            // 这里的操作把pos存到了plain的第一个字节里面
            // 0xF8后面三位是空的，正好留给pos，因为pos是0到7的值，表示文本开始的字节位置
            plain[0] = (byte)((Rand() & 0xF8) | pos);
            // 这里用随机产生的数填充plain[1]到plain[pos]之间的内容
            for (int i = 1; i <= pos; i++)
            {
                plain[i] = (byte)(Rand() & 0xFF);
            }
            pos++;
            // 这个就是prePlain，第一个8字节块当然没有prePlain，所以我们做一个全0的给第一个8字节块
            for (int i = 0; i < 8; i++)
            {
                prePlain[i] = 0x0;
            }
            // 继续填充2个字节的随机数，这个过程中如果满了8字节就加密之
            padding = 1;
            while (padding <= 2)
            {
                if (pos < 8)
                {
                    plain[pos++] = (byte)(Rand() & 0xFF);
                    padding++;
                }
                if (pos == 8)
                {
                    Encrypt8Bytes();
                }
            }
            // 头部填充完了，这里开始填真正的明文了，也是满了8字节就加密，一直到明文读完
            int index = offset;
            while (len > 0)
            {
                if (pos < 8)
                {
                    plain[pos++] = inData[index++];
                    len--;
                }
                if (pos == 8)
                {
                    Encrypt8Bytes();
                }
            }
            // 最后填上0，以保证是8字节的倍数
            padding = 1;
            while (padding <= 7)
            {
                if (pos < 8)
                {
                    plain[pos++] = 0x0;
                    padding++;
                }
                if (pos == 8)
                {
                    Encrypt8Bytes();
                }
            }
            return outData;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inData">需要加密的明文</param>
        /// <param name="k">密钥</param>
        /// <returns>密文字节数组</returns>
        public byte[] Encrypt(byte[] inData, byte[] k)
        {
            return Encrypt(inData, 0, inData.Length, k);
        }
        /// <summary>
        /// 加密一个8字节块
        /// </summary>
        /// <param name="inData">明文字节数组</param>
        /// <returns>密文字节数组</returns>
        private byte[] Encipher(byte[] inData)
        {
            // 迭代次数，16次
            int loop = 0x10;
            // 得到明文和密钥的各个部分
            uint y = GetUInt(inData, 0, 4);
            uint z = GetUInt(inData, 4, 4);
            uint a = GetUInt(key, 0, 4);
            uint b = GetUInt(key, 4, 4);
            uint c = GetUInt(key, 8, 4);
            uint d = GetUInt(key, 12, 4);
            // 这是算法的一些控制变量，为什么delta是0x9E3779B9呢？
            // 这个数是TEA算法的delta，实际是就是(sqr(5) - 1) * 2^31 (根号5，减1，再乘2的31次方)
            uint sum = 0;
            uint delta = 0x9E3779B9;
            //delta &= 0xFFFFFFFFL;
            // 开始迭代了，乱七八糟的，我也看不懂，反正和DES之类的差不多，都是这样倒来倒去
            while (loop-- > 0)
            {
                sum += delta;
                //sum &= 0xFFFFFFFFL;
                y += ((z << 4) + a) ^ (z + sum) ^ ((z >> 5) + b);
                // y &= 0xFFFFFFFFL;
                z += ((y << 4) + c) ^ (y + sum) ^ ((y >> 5) + d);
                //  z &= 0xFFFFFFFFL;
            }
            // 最后，我们输出密文，因为我用的long，所以需要强制转换一下变成int
            baos.Initialize();
            WriteInt(y);
            WriteInt(z);
            return baos;
        }

        /// <summary>
        /// 解密从offset开始的8字节密文
        /// </summary>
        /// <param name="inData">密文字节数组</param>
        /// <param name="offset">密文开始位置.</param>
        /// <returns>明文</returns>
        private byte[] Decipher(byte[] inData, int offset)
        {
            //迭代次数，16次
            int loop = 0x10;
            //得到密文和密钥的各个部分
            uint y = GetUInt(inData, offset, 4);
            uint z = GetUInt(inData, offset + 4, 4);
            uint a = GetUInt(key, 0, 4);
            uint b = GetUInt(key, 4, 4);
            uint c = GetUInt(key, 8, 4);
            uint d = GetUInt(key, 12, 4);
            // 算法的一些控制变量，sum在这里也有数了，这个sum和迭代次数有关系
            // 因为delta是这么多，所以sum如果是这么多的话，迭代的时候减减减，减16次，最后
            // 得到0。反正这就是为了得到和加密时相反顺序的控制变量，这样才能解密呀～～
            uint sum = 0xE3779B90;
            // sum &= 0xFFFFFFFFL;
            uint delta = 0x9E3779B9;
            // delta &= 0xFFFFFFFFL;
            // 迭代开始了， @_@
            while (loop-- > 0)
            {
                z -= ((y << 4) + c) ^ (y + sum) ^ ((y >> 5) + d);
                //  z &= 0xFFFFFFFFL;
                y -= ((z << 4) + a) ^ (z + sum) ^ ((z >> 5) + b);
                // y &= 0xFFFFFFFFL;
                sum -= delta;
                //  sum &= 0xFFFFFFFFL;
            }

            baos.Initialize();
            WriteInt(y);
            WriteInt(z);
            return baos;
        }

        /// <summary>解密
        /// 	<remark>abu 2008-02-16 </remark>
        /// </summary>
        /// <param name="inData">密文</param>
        /// <returns>明文</returns>
        private byte[] Decipher(byte[] inData)
        {
            return Decipher(inData, 0);
        }
        private void WriteInt(uint t)
        {
            if (baosPos == 8)
                baosPos = 0;

            baos[baosPos] = (byte)((t & 0xff000000) >> 24);
            baosPos++;
            baos[baosPos] = (byte)((t & 0x00ff0000) >> 16);
            baosPos++;
            baos[baosPos] = (byte)((t & 0x0000ff00) >> 8);
            baosPos++;
            baos[baosPos] = (byte)(t & 0x000000ff);
            baosPos++;
        }
        /// <summary>
        /// 加密8字节
        /// </summary>
        private void Encrypt8Bytes()
        {
            // 这部分完成我上面所说的 plain ^ preCrypt，注意这里判断了是不是第一个8字节块，如果是的话，那个prePlain就当作preCrypt用
            for (pos = 0; pos < 8; pos++)
            {
                if (header)
                {
                    plain[pos] ^= prePlain[pos];
                }
                else
                    plain[pos] ^= outData[preCrypt + pos];
            }
            // 这个完成我上面说的 f(plain ^ preCrypt)
            byte[] crypted = Encipher(plain);
            // 这个没什么，就是拷贝一下，java不像c，所以我只好这么干，c就不用这一步了 在.NET里面还得测试
            Array.Copy(crypted, 0, outData, crypt, 8);
            // 这个完成了 f(plain ^ preCrypt) ^ prePlain，ok，下面拷贝一下就行了
            for (pos = 0; pos < 8; pos++)
            {
                outData[crypt + pos] ^= prePlain[pos];
            }
            Array.Copy(plain, 0, prePlain, 0, 8);
            // 完成了加密，现在是调整crypt，preCrypt等等东西的时候了
            preCrypt = crypt;
            crypt += 8;
            pos = 0;
            header = false;
        }
        /// <summary>
        /// 解密8个字节
        /// </summary>
        /// <param name="inData">密文字节数组.</param>
        /// <param name="offset">从何处开始解密.</param>
        /// <param name="len">密文的长度.</param>
        /// <returns>true表示解密成功</returns>
        private bool Decrypt8Bytes(byte[] inData, int offset, int len)
        {
            // 这里第一步就是判断后面还有没有数据，没有就返回，如果有，就执行 crypt ^ prePlain
            for (pos = 0; pos < 8; pos++)
            {
                if (contextStart + pos >= len)
                {
                    return true;
                }
                prePlain[pos] ^= inData[offset + crypt + pos];
            }
            // 好，这里执行到了 d(crypt ^ prePlain)
            prePlain = Decipher(prePlain);
            if (prePlain == null)
            {
                return false;
            }
            // 解密完成，最后一步好像没做？ 
            // 这里最后一步放到decrypt里面去做了，因为解密的步骤有点不太一样
            // 调整这些变量的值先
            contextStart += 8;
            crypt += 8;
            pos = 0;
            return true;
        }
        /// <summary>
        /// 这是个随机因子产生器，用来填充头部的，如果为了调试，可以用一个固定值
        /// 随机因子可以使相同的明文每次加密出来的密文都不一样
        /// 	<remark>abu 2008-02-16 </remark>
        /// </summary>
        /// <returns>随机因子</returns>        
        private int Rand()
        {
            return random.Next();
        }

        /// <summary>
        /// MD5加密
        /// 	<remark>abu 2008-02-18 </remark>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] MD5(byte[] data)
        {
            return MD5Instance.ComputeHash(data);
        }
    }
}
