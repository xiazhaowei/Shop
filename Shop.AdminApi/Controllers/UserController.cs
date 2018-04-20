using ENode.Commanding;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
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
using System.Web;
using System.Web.Http;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
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
        public BaseApiResponse UserInvotes(UserInvotesRequest request)
        {
            request.CheckNotNull(nameof(request));
            int totalUserCount = 0;
            int directPasserCount = 0;
            int totalPasserCount = 0;
            int directVipPasserCount = 0;
            int totalVipPasserCount = 0;
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
                if (user.Role == UserRole.Passer)
                {
                    totalPasserCount++;
                }
                if (user.Role == UserRole.VipPasser)
                {
                    totalVipPasserCount++;
                }
                var invotes = _userQueryService.UserChildrens(user.Id).OrderByDescending(x => x.CreatedOn);
                foreach (var invote in invotes)
                {
                    node.Invotes.Add(getNodeData(invote, level++));
                }
                return node;
            };

            var myInvotes = _userQueryService.UserChildrens(request.Id).OrderByDescending(x => x.CreatedOn);
            List<object> nodes = myInvotes.Select(x => getNodeData(x, 0)).ToList();
            directPasserCount = myInvotes.Where(x => x.Role == UserRole.Passer).Count();
            directVipPasserCount = myInvotes.Where(x => x.Role == UserRole.VipPasser).Count();
            return new UserInvotesResponse
            {
                Invotes = nodes,
                InvoteStatisticsInfo=new InvoteStatisticsInfo {
                    TotalStoreOrderAmount = totalStoreOrderAmount,
                    TotalUserCount = totalUserCount,
                    DirectPasserCount=directPasserCount,
                    TotalPasserCount = totalPasserCount,
                    DirectVipPasserCount=directVipPasserCount,
                    TotalVipPasserCount = totalVipPasserCount
                },
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
        public ListPageResponse ListPage(ListPageRequest request)
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
        public async Task<BaseApiResponse> Edit(EditRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.Id);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }

            if (request.ParentMobile.IsMobileNumber())
            {
                //设置推荐人
                var parent = _userQueryService.FindUser(request.ParentMobile);
                if (parent == null)
                {
                    return new BaseApiResponse { Code = 400, Message = "没找到该推荐人" };
                }
                if (user.CreatedOn < parent.CreatedOn)
                {
                    return new BaseApiResponse { Code = 400, Message = "推荐人的注册日期似乎比该用户晚" };
                }
                var command = new SetMyParentCommand(parent.Id)
                {
                    AggregateRootId = request.Id
                };
                await ExecuteCommandAsync(command);
            }
            
            if (request.NickName.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "昵称长度不得超过20字符." };
            }
            if (!"男,女,保密".IsIncludeItem(request.Gender))
            {
                return new BaseApiResponse { Code = 400, Message = "性别参数错误，非： 男/女/保密" };
            }

            var command2 = new EditUserCommand(
                request.Id,
                request.NickName,
                request.Gender,
                request.Role);
            var result = await ExecuteCommandAsync(command2);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "编辑用户失败" };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑用户", request.Id, user.Mobile);
            return new BaseApiResponse();
        }
        
        /// <summary>
        /// 设置推荐人
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> SetParent(SetParentRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.Id);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }

            if (!request.Mobile.IsMobileNumber())
            {
                return new BaseApiResponse { Code = 400, Message = "请输入正确的手机号" };
            }
            var parent = _userQueryService.FindUser(request.Mobile);
            if (parent == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该推荐人" };
            }
            
            if (user.CreatedOn < parent.CreatedOn)
            {
                return new BaseApiResponse { Code = 400, Message = "推荐人的注册日期似乎比该用户晚" };
            }

            var command = new SetMyParentCommand(parent.Id)
            {
                AggregateRootId = request.Id
            };

            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(),
                "设置用户的推荐人", 
                request.Id, "设置用户：{0}的推荐人为：{1}".FormatWith(user.Mobile,request.Mobile));
            return new BaseApiResponse();
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Lock(UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.UserId);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到用户" };
            }
            var command = new LockUserCommand { AggregateRootId=request.UserId};
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "锁定用户", request.UserId, user.Mobile);
            return new BaseApiResponse();
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> UnLock(UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.UserId);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到用户" };
            }
            var command = new UnLockUserCommand { AggregateRootId = request.UserId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "解锁用户", request.UserId, user.Mobile);
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Freeze(UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.UserId);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到用户" };
            }
            var command = new FreezeUserCommand { AggregateRootId = request.UserId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "冻结用户", request.UserId, user.Mobile);
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> UnFreeze(UserStateRequest request)
        {
            request.CheckNotNull(nameof(request));
            var user = _userQueryService.FindUser(request.UserId);
            if (user == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到用户" };
            }
            var command = new UnFreezeUserCommand { AggregateRootId = request.UserId };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令执行失败" };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "解冻用户", request.UserId, user.Mobile);
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
