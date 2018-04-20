namespace Shop.Domain.Models.Users.UserGifts
{
    public class GiftInfo
    {
        public string Name { get; private set; }
        public string Size { get; private set; }

        public GiftInfo(string name,string size)
        {
            Name = name;
            Size = size;
        }
    }
}
