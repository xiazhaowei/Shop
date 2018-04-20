namespace Shop.Api.Models.Request.Users
{
    public class AddExpressAddressRequest
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
    }
}