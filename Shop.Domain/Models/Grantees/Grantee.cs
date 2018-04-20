using ENode.Domain;
using Shop.Common.Enums;
using Shop.Domain.Events.Grantees;
using System;
using System.Collections.Generic;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Grantees
{
    /// <summary>
    /// 受助人 聚合根
    /// </summary>
    public class Grantee: AggregateRoot<Guid>
    {
        private Guid _publisher;//发布者
        private GranteeInfo _info;//基本信息
        private GranteeStatisticsInfo _granteeStatisticsInfo;//受助情况统计信息
        private IList<Verification> _verifications;//审核内容
        private GranteeStatus _status;//状态
        private IList<Testify> _testifys;//其他人作证
        private IList<MoneyHelp> _moneyHelps;//现金帮助

        public Grantee(Guid id,Guid publisher,GranteeInfo info):base(id)
        {
            ApplyEvent(new GranteeCreatedEvent(publisher,info));
        }


        /// <summary>
        /// 接受现金帮助 更新统计
        /// </summary>
        /// <param name="amount"></param>
        public void AcceptMoneyHelp(MoneyHelp moneyHelp)
        {
            moneyHelp.CheckNotNull(nameof(moneyHelp));
            //添加帮助信息
            ApplyEvent(new AcceptedNewMoneyHelpEvent(moneyHelp));
            //更新统计信息
            ApplyEvent(new GranteeStatisticsInfoChangedEvent(new GranteeStatisticsInfo (
                _granteeStatisticsInfo.Total+moneyHelp.Amount,
                _granteeStatisticsInfo.Goods,
                _granteeStatisticsInfo.Count+1
            )));
        }

        /// <summary>
        /// 申请提现 申请者必须是发布者？
        /// </summary>
        public void ApplyWithDraw()
        {

        }


        /// <summary>
        /// 添加作证人
        /// </summary>
        /// <param name="testify"></param>
        public void AddTestify(Testify testify)
        {
            testify.CheckNotNull(nameof(testify));
            ApplyEvent(new AddTestifyedEvent(testify));
        }
        /// <summary>
        /// 认证
        /// </summary>
        public void Verify()
        {
            ApplyEvent(new GranteeVerifyedEvent());
        }

        /// <summary>
        /// 取消认证 下架 （审核资料投诉不真实等原因，公司考察发现不真实）
        /// </summary>
        public void UnVerify()
        {
            ApplyEvent(new GranteeUnVerifyedEvent());
        }

        /// <summary>
        /// 认证员 添加认证信息 必须借鉴用户上传的认证资料适当鉴别
        /// </summary>
        /// <param name="verification"></param>
        public void AddVerification(Verification verification)
        {
            ApplyEvent(new AddVerificationEvent(verification));
        }

        #region Handle
        private void Handle(GranteeCreatedEvent evnt)
        {
            _publisher = evnt.Publisher;
            _info = evnt.Info;
            _verifications = new List<Verification>();
            _testifys = new List<Testify>();
            _status = GranteeStatus.Placed;
            _moneyHelps = new List<MoneyHelp>();
        }
        private void Handle(AcceptedNewMoneyHelpEvent evnt)
        {
            _moneyHelps.Add(evnt.MoneyHelp);
        }
        private void Handle(GranteeStatisticsInfoChangedEvent evnt)
        {
            _granteeStatisticsInfo = evnt.StatisticsInfo;
        }
        private void Handle(AddTestifyedEvent evnt)
        {
            _testifys.Add(evnt.Testify);
        }
        private void Handle(GranteeVerifyedEvent evnt)
        {
            _status = GranteeStatus.Verifyed;
        }
        private void Handle(AddVerificationEvent evnt)
        {
            _verifications.Add(evnt.Verification);
        }
        private void Handle(GranteeUnVerifyedEvent evnt)
        {
            _status = GranteeStatus.Placed;
        }
        
        #endregion
    }
}
