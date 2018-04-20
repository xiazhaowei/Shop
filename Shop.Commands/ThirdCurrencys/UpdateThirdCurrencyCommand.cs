using ENode.Commanding;
using System;

namespace Shop.Commands.ThirdCurrencys
{
    public class UpdateThirdCurrencyCommand:Command<Guid>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ComponyName { get; set; }
        public decimal Conversion { get; set; }
        public bool IsLocked { get; set; }
        public string Remark { get; set; }

        public UpdateThirdCurrencyCommand() { }

        public UpdateThirdCurrencyCommand(
            string name,
            string icon,
            string componeyName,
            decimal conversion,
            bool isLocked,
            string remark)
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
