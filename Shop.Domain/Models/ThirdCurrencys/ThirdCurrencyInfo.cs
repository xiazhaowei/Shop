namespace Shop.Domain.Models.ThirdCurrencys
{
    public class ThirdCurrencyInfo
    {
        //货币名称
        public string Name { get; set; }
        public string Icon { get; set; }
        public string CompanyName { get; set; }
        //导入换算比例 1：1=1，1：0.5=0.5
        public decimal Conversion { get; set; }
        public bool IsLocked { get; set; }
        public string Remark { get; set; }

        public ThirdCurrencyInfo(string name,
            string icon,
            string companyName,
            decimal conversion,
            bool isLocked,
            string remark)
        {
            Name = name;
            Icon = icon;
            CompanyName = companyName;
            Conversion = conversion;
            IsLocked = isLocked;
            Remark = remark;
        }
    }
}
