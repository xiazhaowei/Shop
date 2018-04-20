using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Request.Store
{
    public class ApplyStoreRequest
    {
        public string AccessCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }

        public Subject Subject { get; set; }
    }

    /// <summary>
    /// 主体信息
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// 公司名称或个人姓名
        /// </summary>
        public string Name { get; set; }
        //身份证号码或统一社会识别号
        public string Number { get; set; }
        //身份证照片或营业执照
        public string Pic { get; set; }
    }
}