using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyEncrypt
{
    /// <summary>
    /// DES AES Blowfish
    ///  对称加密算法的优点是速度快，
    ///  缺点是密钥管理不方便，要求共享密钥。
    /// 可逆对称加密  密钥长度8
    /// </summary>
    public class DesEncrypt
    {
        //设置密钥
        private static byte[] _rgbKey = ASCIIEncoding.ASCII.GetBytes(Constant.DesKey.Substring(0, 8));//取到相关密钥，转成编码，然后存为字节数组
                                                                                                      //string.Substring用于从字符串中截取指定部分并返回一个新的字符串。
        private static byte[] _rgbIV = ASCIIEncoding.ASCII.GetBytes(Constant.DesKey.Insert(0, "w").Substring(0, 8));

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public static string Encrypt(string text)
        {
            //【1】实现加密对象
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();//DESCryptoServiceProvider用于实现DES对称加密算法
            //【2】创建内存流对象，从内存中读取和写入数据
            using (MemoryStream memStream = new MemoryStream())
            {
                //【3】创建加密流对象，使用加密对象的CreateEncryptor方法对数据进行加密
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);//CryptoStream是对流进行包装，将流的信息加密；注意，CryptoStream只负责对信息的处理，不负责保存数据
                                                                                                                                    //dsp.CreateEncryptor对参数进行加密；CryptoStreamMode.Write 写模式
                //【4】创建写入流对象，将数据写入加密流
                StreamWriter sWriter = new StreamWriter(crypStream);
                sWriter.Write(text);
                sWriter.Flush();//将 StreamWriter 内部缓存的数据写入到底层 Stream
                crypStream.FlushFinalBlock();//CryptoStream.FlushFinalBlock方法通知系统后面没有数据，并把没从处理完的数据处理掉。
                memStream.Flush();//数据已经在缓存中了，这么写没有意义
                //【5】将加密后的数据转换为Base64字符串并返回
                return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);//将 MemoryStream 中保存的加密后的二进制数据转换成 Base64 字符串，以便存储、传输或显示。
                                                                                               //MemoryStream.GetBuffer表示得到整个内部缓冲区
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns>解密后的结果</returns>
        public static string Decrypt(string encryptText)
        {
            //【1】实现加密对象，并将加密信息从字符串转换为字节数组
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(encryptText);//Convert.FromBase64String方法将ase64 字符串转换回字节数组。
            //【2】创建内存流对象，从内存中读取和写入数据
            using (MemoryStream memStream = new MemoryStream())
            {
                //【3】创建加密流对象，使用加密对象的CreateDecryptor方法对数据进行解密，并将解密后的数据写入内存流
                CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);//对流进行解密
                crypStream.Write(buffer, 0, buffer.Length);
                crypStream.FlushFinalBlock();
                //【4】将字节数组转换为字符串并返回
                return ASCIIEncoding.UTF8.GetString(memStream.ToArray());//ASCIIEncoding.UTF8.GetString方法将字节数组转换为字符串。
            }
        }
    }
}
