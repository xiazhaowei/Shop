using ENode.Commanding;
using System;
using System.Collections.Generic;

namespace Shop.Commands.Grantees
{
    public class AddVerificationCommand:Command<Guid>
    {
        /// <summary>
        /// 审核标题  患者：张三
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 证明资料
        /// </summary>
        public IList<string> Pics { get; private set; }
        /// <summary>
        /// 已审核的项目  身份证已审核；诊断证明已提交；诊断医院已审核
        /// </summary>
        public IList<string> VerifiedNames { get; private set; }

        public AddVerificationCommand() { }
        public AddVerificationCommand(string title,IList<string>pics,IList<string>verifiedNames)
        {
            Title = title;
            Pics = pics;
            VerifiedNames = verifiedNames;
        }
    }
}
