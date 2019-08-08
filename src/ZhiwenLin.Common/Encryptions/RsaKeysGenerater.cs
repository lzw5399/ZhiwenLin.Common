using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZhiwenLin.Common.Encryptions
{
    public class RsaKeysGenerater
    {
        private readonly IHostingEnvironment _env;

        public RsaKeysGenerater(IHostingEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// 在当前项目的根目录下生成一个公钥，一个私钥，共两个json文件
        /// 私钥由token provider保存
        /// 公钥由token user保存
        /// </summary>
        private void GenerateAndSaveKey()
        {
            RSAParameters publicKeys, privateKeys;

            // 支持512 1024 2048 4096, 系数越高, 越安全, 但效率也越低
            using (var rsa = new RSACryptoServiceProvider(512))
            {
                try
                {
                    privateKeys = rsa.ExportParameters(true);
                    publicKeys = rsa.ExportParameters(false);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            string dir = _env.ContentRootPath;

            System.IO.File.WriteAllText(
                Path.Combine(dir, "key-private.json"),
                JsonConvert.SerializeObject(privateKeys));

            System.IO.File.WriteAllText(
                Path.Combine(dir, "key-public.json"),
                JsonConvert.SerializeObject(publicKeys));
        }
    }
}
