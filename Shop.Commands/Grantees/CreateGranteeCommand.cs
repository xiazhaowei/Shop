using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Grantees
{
    public class CreateGranteeCommand : Command<Guid>
    {
        public string Section { get; private set; }
        public Guid Publisher { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        /// <summary>
        /// 项目内容和项目图片禁止透露任何联系方式和银行卡等收款信息，包含但不限于手机号码、座机号
        /// 微信号、支付宝号、银行卡号。违反上述规定，项目验证和提现申请均不予通过
        /// </summary>
        public IList<string> Pics { get; set; }
        public decimal Max { get; set; }
        public int Days { get; set; }

        public CreateGranteeCommand() { }
        public CreateGranteeCommand(Guid id,
            Guid publisher,
            string section,
            string title,
            string description,
            IList<string> pics,
            decimal max,
            int days=30):base(id)
        {
            Publisher = publisher;
            Section = section;
            Title = title;
            Description = description;
            Pics = pics;
            Max = max;
            Days = days;
        }
    }
}
