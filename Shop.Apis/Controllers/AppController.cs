using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Apps;
using System;
using System.Linq;

namespace Shop.Apis.Controllers
{
    [Route("[controller]")]
    public class AppController:Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public AppController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("CheckVersion")]
        public BaseApiResponse CheckVersion()
        {
            AppVersion appVersion = null;
            var json = ReadJsonFile("appVersion.json");
            appVersion = DeserializeJsonToObject<AppVersion>(json);
            
            return new AppVersionResponse
            {
                Version=appVersion.Version,
                Content= appVersion.Content
            };
        }

        [HttpPost]
        [Route("HomeBanner")]
        public BaseApiResponse HomeBanner()
        {
            HomeBanners homeBanners = null;
            var json = ReadJsonFile("homeBanners.json");
            homeBanners = DeserializeJsonToObject<HomeBanners>(json);

            return new HomeBannerResponse
            {
                Banners = homeBanners.Banners.Select(x=>new HomeBanner {
                    Img=x.Img,
                    Link=x.Link
                }).ToList()
            };
        }

        #region 私有方法

        
        private string ReadJsonFile(string fileName)
        {
            string json = string.Empty;
            //读取json文件
            var jsonFilePath = _hostingEnvironment.ContentRootPath + $@"/Json/{fileName}";

            System.IO.StreamReader streamReader = null;
            if (!System.IO.File.Exists(jsonFilePath))
            {
                throw new Exception("没有找到json文件");
            }
            try
            {
                streamReader = new System.IO.StreamReader(jsonFilePath);
                json = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                streamReader.Close();
            }

            return json;
        }

        private static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            System.IO.StringReader sr = new System.IO.StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
        #endregion
    }
}