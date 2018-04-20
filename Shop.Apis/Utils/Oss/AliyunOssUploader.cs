using Aliyun.OSS;
using Aliyun.OSS.Common;
using ECommon.Components;
using System;
using System.IO;
using Xia.Common.Extensions;

namespace Shop.Apis.Utils.Oss
{
    [Component]
    public class AliyunOssUploader: IOssUploader
    {
        static OssClient client = new OssClient(AliyunOssConfig.Endpoint, AliyunOssConfig.AccessKeyId, AliyunOssConfig.AccessKeySecret);

        /// <summary>
        /// 上传对象
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="ossPath"></param>
        /// <param name="fileToUpload"></param>
        /// <returns></returns>
        public  string PutObjectFromFile(string bucketName,string ossPath,string fileToUpload)
        {
            string key =ossPath+"/"+ DateTime.Now.ToSerialNumber() + Path.GetExtension(fileToUpload);
            try
            {
                client.PutObject(bucketName, key, fileToUpload);
            }
            catch (OssException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return key;
        }

        /// <summary>
        /// 追加上传
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileToUpload"></param>
        /// <returns></returns>
        public string SyncAppendObject(string bucketName,string fileToUpload)
        {
            string key = DateTime.Now.ToSerialNumber()+Path.GetExtension(fileToUpload);
            long position = 0;
            try
            {
                var metadata = client.GetObjectMetadata(bucketName, key);
                position = metadata.ContentLength;
            }
            catch (Exception) { }

            try
            {
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var request = new AppendObjectRequest(bucketName, key)
                    {
                        ObjectMetadata = new ObjectMetadata(),
                        Content = fs,
                        Position = position
                    };

                    var result = client.AppendObject(request);
                    position = result.NextAppendPosition;
                }

                // append object by using NextAppendPosition
                using (var fs = File.Open(fileToUpload, FileMode.Open))
                {
                    var request = new AppendObjectRequest(bucketName, key)
                    {
                        ObjectMetadata = new ObjectMetadata(),
                        Content = fs,
                        Position = position
                    };

                    var result = client.AppendObject(request);
                    position = result.NextAppendPosition;
                }
            }
            catch (OssException ex)
            {
            }
            catch (Exception ex)
            {
            }
            return key;
        }
    }
}