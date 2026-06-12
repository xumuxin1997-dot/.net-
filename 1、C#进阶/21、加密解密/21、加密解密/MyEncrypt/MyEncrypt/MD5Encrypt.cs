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
    /// 不可逆加密
    /// 1 防止被篡改
    /// 2 防止明文存储
    /// 3 防止抵赖，数字签名
    /// </summary>
    public class MD5Encrypt
    {
        #region MD5
        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同,
        /// 使用的UTF8编码
        /// </summary>
        /// <param name="source">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public static string Encrypt(string source, int length = 32)//默认参数
        {
            //【1】检查输入是否为空
            if (string.IsNullOrEmpty(source))
            { return string.Empty; }
            //【2】创建MD5哈希算法对象和字符串构建器对象，MD5哈希算法对象用于加密，字符串构建器对象构建对象用于构建加密后的字符串
            HashAlgorithm provider = CryptoConfig.CreateFromName("MD5") as HashAlgorithm;//根据字符串名称创建一个哈希算法对象
                                                                                         //HashAlgorithm类是所有哈希算法的抽象基类，提供了计算哈希值的方法。
                                                                                         //CryptoConfig类是加密算法工厂，提供了根据算法名称创建加密算法对象的方法。
                                                                                         //CryptoConfig.CreateFromName方法是根据算法名称字符串，动态创建对应的密码学对象实例。
            StringBuilder sb = new StringBuilder();//StringBuilder类用于动态构建字符串
                                                   //字符串本质是不可修改的，每一次修改都会创建一个新的字符串对象。
                                                   //StringBuilder类可以在同一块可变缓冲区内修改字符串，避免频繁创建新对象

            //【3】将字符串转换为字节数组，并计算哈希值
            byte[] bytes = Encoding.UTF8.GetBytes(source);//这里需要区别编码的
            byte[] hashValue = provider.ComputeHash(bytes);//HashAlgorithm.ComputeHash方法是对输入数据进行哈希计算，生成固定长度的Hash值
            
            //【4】根据长度选择输出的MD5字符串形式
            //问题：为什么MD5加密结果有16位和32位两种形式？为什么4-12位、0-16位、0-最后？
            switch (length)
            {
                case 16://16位密文是32位密文的9到24位字符
                    for (int i = 4; i < 12; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));//把 MD5 计算出来的 byte[] 转成十六进制字符串。
                                                               //ToString("x2")是将byte转换为两位十六进制字符串，x表示十六进制，2表示每个byte占两位。
                                                               //32 位 MD5 中截取中间 8 个字节（第 5～12 个字节），为什么这样做：这是历史遗留“约定俗成”
                    }
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            //【5】返回加密后的字符串
            return sb.ToString();
        }
        #endregion MD5

        #region MD5摘要
        /// <summary>
        /// 获取文件的MD5摘要
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string AbstractFile(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open))
            {
                return AbstractFile(file);
            }
        }
        /// <summary>
        /// 根据stream获取文件摘要
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string AbstractFile(Stream stream)
        {
            //【1】创建MD5哈希算法实例和字符串构件器实例
            MD5 md5 = new MD5CryptoServiceProvider();//创建一个 MD5 哈希算法实例，用于计算数据的 MD5 摘要
                                                     //MD5类定义了 MD5 算法的标准接口，抽象类
                                                     //MD5CryptoServiceProvider类负责执行MD5算法的计算。
            StringBuilder sb = new StringBuilder();
            //【2】计算输入流的MD5摘要
            byte[] retVal = md5.ComputeHash(stream);
            //【3】将MD5摘要转换为十六进制字符串
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            //【4】返回MD5摘要字符串
            return sb.ToString();
        }
        #endregion
    }
}
