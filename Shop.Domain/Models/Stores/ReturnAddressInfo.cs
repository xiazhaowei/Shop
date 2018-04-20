namespace Shop.Domain.Models.Stores
{
    public class ReturnAddressInfo
    {
        public string Name { get;private set; }
        public string Address { get;private set; }
        public string Mobile { get;private set; }

        public ReturnAddressInfo(string name,string mobile,string address)
        {
            Name = name;
            Mobile = mobile;
            Address = address;
        }
    }
}
