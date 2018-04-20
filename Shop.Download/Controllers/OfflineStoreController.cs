using Shop.Download.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using Xia.Common.Extensions;

namespace Shop.Download.Controllers
{
    public class OfflineStoreController : Controller
    {
        // GET: OfflineStore
        public ActionResult Index()
        {
            //二维码内容
            string content = string.IsNullOrEmpty(Request.QueryString["id"]) ? "OfflineStorePayeeCode:" : "OfflineStorePayeeCode:" + Request.QueryString["id"];
            //店铺名称
            string storeName = string.IsNullOrEmpty(Request.QueryString["name"]) ? "" : Request.QueryString["name"];
            storeName = storeName.Truncate(9);
            int size = 340;
            int border = 20;
            int left = 130;
            int top = 152;
            //生成二维码图片
            var qrCodeImg = new QrcodeCreater().CreateQRCode(content,
                QRCodeEncoder.ENCODE_MODE.BYTE,
                QRCodeEncoder.ERROR_CORRECTION.M,
                0,
                5,
                size,
                border);

            //读取背景图片
            var sourceImg = Path.Combine(Server.MapPath("~/Contents/Imgs"),  "OfflineStoreQrBg.png");
            if (!System.IO.File.Exists(sourceImg))
            {
                return Content("没有找到模板文件");
            }
            //合成图片
            Image imgBack = Image.FromFile(sourceImg);     //背景图片   
            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics         
            Graphics g = Graphics.FromImage(imgBack);
            //绘制背景图片
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            //绘制二维码
            g.DrawImage(qrCodeImg, left, top, qrCodeImg.Width, qrCodeImg.Height);
            //绘制文字
            Font font = new Font("微软雅黑", 40);
            SolidBrush sbrush = new SolidBrush(Color.White);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            Rectangle stringRect = new Rectangle(92, 528, 410, 48);
            g.DrawString(storeName, font, sbrush, stringRect, stringFormat);

            GC.Collect();
            //输出文件流  
            var ms = new MemoryStream();
            imgBack.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return File(ms.ToArray(), "image/jpeg");
            //回收资源
            //ms.Close(); ms = null;
            //qrCodeImg.Dispose(); qrCodeImg = null;
            //imgBack.Dispose(); imgBack = null;
        }
    }
}