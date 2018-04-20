using ENode.Commanding;
using System;

namespace Shop.Commands.ThirdCurrencys
{
    public class CreateThirdCurrencyCommand:Command<Guid>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ComponyName { get; set; }
        public decimal Conversion { get; set; }
        public bool IsLocked { get; set; }
        public string Remark { get; set; }

        public CreateThirdCurrencyCommand() { }

        public CreateThirdCurrencyCommand(Guid id,
            string name,
            string icon,
            string componeyName,
            decimal conversion,
            bool isLocked,
            string remark):base(id)
        {
            Name = name;
            Icon = icon;
            ComponyName = componeyName;
            Conversion = conversion;
            IsLocked = isLocked;
            Remark = remark;
        }
    }
}
