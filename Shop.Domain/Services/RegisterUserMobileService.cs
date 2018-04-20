using ECommon.Components;
using System;
using System.Data;
using Shop.Domain.Models.Users;
using Shop.Domain.Repositories;

namespace Shop.Domain.Services
{
    [Component]
    public class RegisterUserMobileService
    {
        private readonly IUserMobileIndexRepository _userMobileIndexRepository;

        /// <summary>
        /// 构造函数注入 IUserMobileIndexRepository的实现着
        /// </summary>
        /// <param name="userMobileIndexRepository"></param>
        public RegisterUserMobileService(IUserMobileIndexRepository userMobileIndexRepository)
        {
            _userMobileIndexRepository = userMobileIndexRepository;
        }

        /// <summary>
        /// 注册手机号
        /// </summary>
        /// <param name="indexId"></param>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        public void RegisterMobile(string indexId, Guid userId, string mobile)
        {
            var mobileIndex = _userMobileIndexRepository.FindMobileIndex(mobile);
            if (mobileIndex == null)
            {
                _userMobileIndexRepository.Add(new UserMobileIndex(indexId, userId, mobile));
            }
            else if (mobileIndex.IndexId != indexId)
            {
                throw new DuplicateNameException("The chosen user mobile is already taken.");
            }
        }
    }
}
