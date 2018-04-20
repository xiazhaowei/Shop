using ENode.Domain;
using Shop.Domain.Events.Admins;
using System;
using Xia.Common.Extensions;

namespace Shop.Domain.Models.Admins
{
    public class Admin: AggregateRoot<Guid>
    {
        private AdminInfo _info;

        public Admin(Guid id,AdminInfo info):base(id)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new AdminCreatedEvent(info));
        }

        public void Update(AdminEditableInfo info)
        {
            info.CheckNotNull(nameof(info));
            ApplyEvent(new AdminUpdatedEvent(info));
        }

        public void AcceptOperatRecord(string operat,Guid aboutId,string remark)
        {
            ApplyEvent(new NewOperatRecordEvent(new OperatRecordInfo(
                _info.Name, operat, aboutId, remark
                )));
        }

        public void Delete()
        {
            ApplyEvent(new AdminDeletedEvent());
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="password">HASHE</param>
        public void UpdatePassword(string password)
        {
            password.CheckNotNullOrEmpty(nameof(password));
            ApplyEvent(new AdminPasswordUpdatedEvent(password));
        }

        #region Handle
        private void Handle(AdminCreatedEvent evnt)
        {
            _info = evnt.Info;
        }
        private void Handle(AdminUpdatedEvent evnt)
        {
            _info = new AdminInfo(
                evnt.Info.Name,
                evnt.Info.LoginName,
                evnt.Info.Portrait,
                _info.Password,
                evnt.Info.Role,
                evnt.Info.IsLocked);
        }
        private void Handle(AdminPasswordUpdatedEvent evnt)
        {
            _info.Password = evnt.Password;
        }
        private void Handle(NewOperatRecordEvent evnt)
        {

        }
        private void Handle(AdminDeletedEvent evnt)
        {
            _info = null;
        }
        #endregion
    }
}
