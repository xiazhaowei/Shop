using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Uploader;
using Shop.Apis.Utils.Oss;
using Shop.Common.Enums;
using System.IO;
using System.Net.Http.Headers;
using Xia.Common.Extensions;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class UploaderController:Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private IOssUploader _ossUploader;

        public UploaderController(IHostingEnvironment hostingEnvironment,IOssUploader ossUploader)
        {
            _hostingEnvironment = hostingEnvironment;
            _ossUploader = ossUploader;
        }

        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SingleFileUpload")]
        public BaseApiResponse SingleFileUpload()
        {
            string ossHost = "http://goodsdetails.wftx666.com";
            string ossFileName = string.Empty;
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                var postedFile = httpRequest.Form.Files[0];
                var filename = ContentDispositionHeaderValue
                                        .Parse(postedFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                filename = _hostingEnvironment.ContentRootPath + $@"\Uploads\{filename}";
                var size = postedFile.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    postedFile.CopyTo(fs);
                    fs.Flush();
                }
                //上传到Oss
                ossFileName= _ossUploader.PutObjectFromFile("wftx-goods-img-details","goodsDetails", filename);
            }
            else
            {
                return new BaseApiResponse { Code = 400, Message = "没有文件" };
            }
            return new SingleFileUploadResponse { Url= ossHost + "/"+ossFileName.ToOssStyleUrl(OssImageStyles.GoodsDetailPic.ToDescription()) };
        }
    }
}