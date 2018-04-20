using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Api.Models.Response.Apps
{
    public class HomeBannerResponse:BaseApiResponse
    {
        public IList<HomeBanner> Banners { get; set; }
    }
}