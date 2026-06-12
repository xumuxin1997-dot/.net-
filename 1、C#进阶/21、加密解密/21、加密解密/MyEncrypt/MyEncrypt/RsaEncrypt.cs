using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
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
    /// RSA ECC
    /// 可逆非对称加密 
    /// 非对称加密算法的优点是密钥管理很方便，缺点是速度慢。
    /// </summary>
    public class RsaEncrypt
    {
        /// <summary>
        /// 获取加密/解密对
        /// 给你一个，是无法推算出另外一个的
        /// 
        /// Encrypt   Decrypt
        /// </summary>
        /// <returns>Encrypt   Decrypt</returns>
        public static KeyValuePair<string, string> GetKeyPair()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();//实现 RSA 非对称加密算法 的类
            string publicKey = RSA.ToXmlString(false);//创建并返回包含当前RSA对象的XML字符串。导出公钥时，参数为 false；导出私钥时，参数为 true。
            string privateKey = RSA.ToXmlString(true);//
            return new KeyValuePair<string, string>(publicKey, privateKey);
        }

        /// <summary>
        /// 加密：内容+加密key
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encryptKey">加密key</param>
        /// <returns></returns>
        public static string Encrypt(string content, string encryptKey)
        {
            //【1】实现加密对象
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(encryptKey);//FromXmlString方法是将密钥导入到RSA对象中。
            //【2】将字符串转换为字节数组
            UnicodeEncoding ByteConverter = new UnicodeEncoding();//UnicodeEncoding类功能：字符串 ⇄ 字节数组
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            //【3】加密字节数组
            byte[] resultBytes = rsa.Encrypt(DataToEncrypt, false);//使用公钥对字符串进行加密
            //【4】将字节数组转换为字符串并返回
            return Convert.ToBase64String(resultBytes);
        }

        /// <summary>
        /// 解密  内容+解密key
        /// </summary>
        /// <param name="content"></param>
        /// <param name="decryptKey">解密key</param>
        /// <returns></returns>
        public static string Decrypt(string content, string decryptKey)
        {
            //【1】将字符串转换为字节数组
            byte[] dataToDecrypt = Convert.FromBase64String(content);
            //【2】实现解密对象
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(decryptKey);//FromXmlString方法是将密钥导入到RSA对象中。
            //【3】解密字节数组
            byte[] resultBytes = RSA.Decrypt(dataToDecrypt, false);//使用 RSA 私钥，将 RSA 加密后的密文还原为原始明文数据
            //【4】将字节数组转换为字符串并返回
            UnicodeEncoding ByteConverter = new UnicodeEncoding();//UnicodeEncoding类用于转换字符串
            return ByteConverter.GetString(resultBytes);
        }


        /// <summary>
        /// 可以合并在一起的，每次产生一组新的密钥
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encryptKey">加密key</param>
        /// <param name="decryptKey">解密key</param>
        /// <returns>加密后结果</returns>
        private static string Encrypt(string content, out string publicKey, out string privateKey)
        {
            //【1】实现加密对象
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            publicKey = rsaProvider.ToXmlString(false);
            privateKey = rsaProvider.ToXmlString(true);

            //【2】将字符串转换为字节数组
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] DataToEncrypt = ByteConverter.GetBytes(content);
            byte[] resultBytes = rsaProvider.Encrypt(DataToEncrypt, false);
            return Convert.ToBase64String(resultBytes);
        }
    }
}
