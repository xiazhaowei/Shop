namespace Shop.Api.Models.Request.Users
{
    public class AddUserGiftRequest
    {
        public GiftInfo GiftInfo { get; set; }
        public ExpressAddressInfo ExpressAddressInfo { get; set; }
    }

    public class GiftInfo
    {
        public string Name { get; set; }
        public string Size { get; set; }
    }

    public class ExpressAddressInfo
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
    }
}