namespace Shop.Common
{
    public class ExpressAddressInfo
    {
        public string Region { get; private set; }
        public string Address { get; private set; }
        public string Name { get; private set; }
        public string Mobile { get; private set; }
        public string Zip { get; private set; }

        public ExpressAddressInfo(string region, string address, string name, string mobile, string zip)
        {
            Region = region;
            Address = address;
            Name = name;
            Mobile = mobile;
            Zip = zip;
        }
    }
}
