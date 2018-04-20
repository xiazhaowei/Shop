using ENode.Commanding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
using Shop.Api.Models.Request.Users;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.User;
using Shop.Commands.Users;
using Shop.Common.Enums;
using Shop.QueryServices;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class UserController : BaseApiController
    {

        private IUserQueryService _userQueryService;//用户Q端
        private IStoreOrderQueryService _storeOrderQueryService;
        
        /// <summary>
        /// IOC 构造函数注入
        /// </summary>
        /// <param name="commandService"></param>
        /// <param name="conferenceQueryService"></param>
        public UserController(ICommandService commandService, IContextService contextService,
            IUserQueryService userQueryService,
            IStoreOrderQueryService storeOrderQueryService): base(commandService,contextService)
        {
            _userQueryService = userQueryService;
            _storeOrderQueryService = storeOrderQueryService;
        }
        
        
        #region 后台管理接口
        /// <summary>
        /// 用户的推荐用户 2级别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UserInvotes")]
        public BaseApiResponse UserInvotes([FromBody]UserInvotesRequest request)
        {
            request.CheckNotNull(nameof(request));
            int totalUserCount = 0;
            decimal totalStoreOrderAmount = 0M;
            string parentMobile = "无";

            //获取用户信息
            var userInfo = _userQueryService.FindUser(request.Id);
            if(userInfo==null)
            {
                return new BaseApiResponse { Code = 400, Message = "没有找到该用户" };
            }
            if (userInfo.ParentId != Guid.Empty)
            {
                //获取用户的推荐人信息
                var parentInfo = _userQueryService.FindUser(userInfo.ParentId);
                parentMobile = parentInfo.Mobile;
            }

            //递归获取分类包含子类
            Func<QueryServices.Dtos.UserAlis, int, object> getNodeData = null;
            getNodeData = (user, level) => {
                dynamic node = new ExpandoObject();
                node.Id = user.Id;
                node.NickName = user.NickName;
                node.Mobile = user.Mobile;
                node.CreatedOn = user.CreatedOn.GetTimeSpan();
                node.Portrait = user.Portrait;
                node.Role = user.Role.ToDescription();
                node.Invotes = new List<dynamic>();
                totalStoreOrderAmount += UserOrdersTotal(user.Id);
                totalUserCount++;
                var invotes = _userQueryService.UserChildrens(user.Id).OrderByDescending(x => x.CreatedOn);
                foreach (var invote in invotes)
                {
                    node.Invotes.Add(getNodeData(invote, level));
                }
                return node;
            };

            var myInvotes = _userQueryService.UserChildrens(request.Id).OrderByDescending(x => x.CreatedOn);
            List<object> nodes = myInvotes.Select(x => getNodeData(x, 0)).ToList();

            

            return new UserInvotesResponse
            {
                Invotes = nodes,
                TotalStoreOrderAmount= totalStoreOrderAmount,
                TotalUserCount=totalUserCount,
                ParentMobile= parentMobile
            };
        }



        /// <summary>
        /// 检查并切断推荐环
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("BreakInvoteLoop")]
        public async Task<BaseApiResponse> BreakInvoteLoop()
        {
            int breakCount = 0;
            var users = _userQueryService.Users().OrderBy(x => x.CreatedOn);
            //遍历所有用户
            foreach (var user in users)
            {
                var childrens = _userQueryService.UserChildrens(user.Id);
                //遍历用户的孩子
                foreach(var children in childrens)
                {
                    if (children.CreatedOn < user.CreatedOn)
                    {
                        
                        var command = new ClearUserParentCommand
                        {
                            AggregateRootId = children.Id
                        };
                        await ExecuteCommandAsync(command);
                        breakCount++;
                        continue;
                    }
                }
            }

            return new BaseApiResponse {
                Message="操作完成，切断 {0} 个错误推荐".FormatWith(breakCount)
            };
        }
        

        [HttpPost]
        [Authorize]
        [Route("ListPage")]
        public ListPageResponse ListPage([FromBody]ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var users = _userQueryService.UserList();
            var total = users.Count();

            //筛选
            if (request.Role != UserRole.All)
            {
                users = users.Where(x => x.Role == request.Role);
            }
            if (!request.Mobile.IsNullOrEmpty())
            {
                users= users.Where(x=>x.Mobile.Contains(request.Mobile));
            }
            if (!request.NickName.IsNullOrEmpty())
            {
                users = users.Where(x => x.NickName.Contains(request.NickName));
            }
            total = users.Count();

            //分页
            users = users.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);

            return new ListPageResponse
            {
                Total = total,
                Users = users.Select(x => new User
                {
                    Id = x.Id,
                    ParentId= x.ParentId,
                    NickName = x.NickName,
                    Mobile = x.Mobile,
                    Gender = x.Gender,
                    Region = x.Region,
                    Role=x.Role.ToString(),
                    IsLocked=x.IsLocked,
                    IsFreeze=x.IsFreeze
                }).ToList()
            };
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<BaseApiResponse> Edit([FromBody]EditRequest request)
        {
            request.CheckNotNull(nameof(request));
            if (request.NickName.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "昵称长度不得超过20字符." };
            }
            if (!"男,女,保密".IsIncludeItem(request.Gender))
            {
                return new BaseApiResponse { Code = 400, Message = "性别参数错误，非： 男/女/保密" };
            }
            var command = new EditUserCommand(
                request.Id,
                request.NickName,
                request.Gender,
                request.Role);
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "性别设置失败" };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Lock")]
        public async Task<BaseApiResponse> Lock([FromBody]UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new LockUserCommand { AggregateRootId=request.UserId};
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("UnLock")]
        public async Task<BaseApiResponse> UnLock([FromBody]UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UnLockUserCommand { AggregateRootId = request.UserId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        [Route("Freeze")]
        public async Task<BaseApiResponse> Freeze([FromBody]UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new FreezeUserCommand { AggregateRootId = request.UserId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        [Route("UnFreeze")]
        public async Task<BaseApiResponse> UnFreeze([FromBody]UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new UnFreezeUserCommand { AggregateRootId = request.UserId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            return new BaseApiResponse();
        }


        #endregion

        #region 私有方法
        /// <summary>
        /// 计算用户的消费额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private decimal UserOrdersTotal(Guid userId)
        {
            var storeOrders = _storeOrderQueryService.UserStoreOrderAlises(userId).Where(x => x.Status == StoreOrderStatus.Success);
            return storeOrders.Sum(x => x.Total);
        }

        #endregion

    }
}
