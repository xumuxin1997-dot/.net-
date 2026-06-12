using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEncrypt
{
    /// <summary>
    /// https://www.wenjuan.com/s/qEVFbuy/
    /// 能听见我说话的，能看到我桌面的，打个1
    /// 高级班的传统，准备好学习的小伙伴儿，给Eleven刷个专属字母E，然后课程就正式开始了！！！
    /// 
    /// 1：MD5 不可逆加密
    /// 2：Des 对称可逆加密
    /// 3：RSA 非对称可逆加密
    /// 4：数字证书 SSL
    /// 
    /// 加密解密：清晰的思维，大家比聪明！
    /// miyao 密钥
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region MD5
                {
                    //MD5公开的算法，任何语言实现后其实都一样，通用的
                    //不可逆加密:原文--加密--密文，密文无法解密出原文
                    //1 相同原文加密的结果是一样的
                    //2 不同长度的内容加密后加过都是32位
                    //3 原文差别很小，结果差别很大
                    //4 不管文件多大，都能产生32位长度摘要
                    //文件内容有一点改动，结果变化非常大
                    //文件内容不变，名字边了，结果是不变

                    //MD5被破解? 没有的，能够通过标本库进行暴力破解，但是存储肯定是有限的
                    //长度32，每个位置有16种可能  2的4*32次方  2的128次方   不可能全部存储
                    //MD5加盐  应对简单密码破解 123456+ruanmou   MD5再MD5
                    //原文更多，不止的2的128次方，那是不是应该有两个原文，MD5后结果一致？ 肯定的，但是还没发现
                    Console.WriteLine(MD5Encrypt.Encrypt("1"));
                    Console.WriteLine(MD5Encrypt.Encrypt("1"));
                    Console.WriteLine(MD5Encrypt.Encrypt("123456小李"));
                    Console.WriteLine(MD5Encrypt.Encrypt("113456小李"));
                    Console.WriteLine(MD5Encrypt.Encrypt("113456小李113456小李113456小李113456小李113456小李113456小李113456小李"));
                    string md5Abstract1 = MD5Encrypt.AbstractFile(@"D:\ruanmou\online12\20190116Advanced12Course18Homework3\20190116Advanced12Course18Homework3 - 副本.rar");
                    string md5Abstract2 = MD5Encrypt.AbstractFile(@"D:\ruanmou\online12\20190116Advanced12Course18Homework3\20190116Advanced12Course18Homework3.rar");
                    //MD5的用途？
                    //1 防篡改：
                    //发个文档，事先给别人一个MD5，是文档的摘要，
                    //源代码管理器svn--即使电脑断网了，文件有任何改动都能被发现--本地存了一个文件的MD5--文件有更新，就再对比下MD5
                    //急速秒传--扫描文件的MD5--跟已有的文件MD5比对--吻合表示文件已存在不用再上传

                    //2 密码保存，防止看到明文
                    //密码应该只有用户知道----数据库不能存明文---但是又需要验证
                    //MD5加密下原始密码---数据库存密文---下次登录把密码MD5后再比对
                    //密文是可见的，所以要求密码不能太简单，加盐(123456+ruanmou)

                    //3 防止抵赖，数字签名
                    //把一些内容摘要一下，由权威的第三方去保障，将来这个文件就是你做的，不能抵赖
                }
                #endregion


                #region Des
                {
                    //对称可逆加密：加密后能解密回原文，加密key和解密key是一个
                    //加密算法都是公开的，密钥是保密的， 即使拿到密文  你是推算不了密钥  也推算不了原文
                    //加密解密的速度快，问题是密钥的安全
                    string desEn = DesEncrypt.Encrypt("王殃殃");
                    string desDe = DesEncrypt.Decrypt(desEn);
                    string desEn1 = DesEncrypt.Encrypt("张三李四");
                    string desDe1 = DesEncrypt.Decrypt(desEn1);
                }
                #endregion 

                #region Rsa
                //非对称可逆加密：加密后能解密回原文，加密key和解密key不是一个，而是一对
                //算法是公开的，加密key和解密key是不能互相推导的  有了密文，没有解密key，也推导不出原文
                {
                    //加密解密速度不快  安全性好  
                    //公开加密key，保证数据的安全传递
                    //公开解密key，保证数据的不可抵赖
                    //公钥就是公开的key  私钥就是不公开的key
                    //C#内置实现了公钥加密  私钥解密；想换需要用第三方的DLL-BouncyCastle
                    KeyValuePair<string, string> encryptDecrypt = RsaEncrypt.GetKeyPair();
                    string rsaEn1 = RsaEncrypt.Encrypt("net", encryptDecrypt.Key);
                    string rsaDe1 = RsaEncrypt.Decrypt(rsaEn1, encryptDecrypt.Value);
                }
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
