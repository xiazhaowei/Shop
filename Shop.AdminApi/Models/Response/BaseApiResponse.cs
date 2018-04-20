namespace Shop.Api.Models.Response
{
    /// <summary>
    /// 接口输出基类
    /// </summary>
    public class BaseApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public BaseApiResponse()
        {
            Code = 200;
            Message = "";
        }
    }
}