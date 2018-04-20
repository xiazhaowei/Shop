using System;

namespace Shop.Apis.ViewModels
{
    public class ExpressAddressViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
    }
}