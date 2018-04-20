using ENode.Commanding;
using Microsoft.AspNet.Identity;
using Shop.AdminApi.Extensions;
using Shop.AdminApi.Services;
using Shop.Api.Models.Request;
using Shop.Api.Models.Request.Admins;
using Shop.Api.Models.Response;
using Shop.Api.Models.Response.Admins;
using Shop.Commands.Admins;
using Shop.QueryServices;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Xia.Common;
using Xia.Common.Extensions;
using Xia.Common.Secutiry;

namespace Shop.AdminApi.Controllers
{
    public class AdminController : BaseApiController
    {
        private IAdminQueryService _adminQueryService;

        public AdminController(ICommandService commandService,IContextService contextService,
            IAdminQueryService adminQueryService) : base(commandService,contextService)
        {
            _adminQueryService = adminQueryService;
        }

        
        [HttpPost]
        public  BaseApiResponse Login(LoginRequest request)
        {
            request.CheckNotNull(nameof(request));
            //默认账户
            if (request.Name == "admin")
            {
                if (request.Password != "wftx123456#")
                {
                    return new BaseApiResponse { Code = 400, Message = "密码不正确，登录不被允许" };
                }
                //登陆
                SignIn(GuidUtil.NewSequentialId().ToString(), "admin", true);
                return new LoginResponse
                {
                    User = new User
                    {
                        Id = GuidUtil.NewSequentialId(),
                        LoginName = "admin",
                        Name = "夏某某",
                        Role = "Admin",
                        Portrait = "https://raw.githubusercontent.com/taylorchen709/markdown-images/master/vueadmin/user.png"
                    }
                };
            }
            var admin = _adminQueryService.Find(request.Name);
            //验证用户
            if (admin == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该账号" };
            }
            //验证账号
            if (admin.IsLocked)
            {
                return new BaseApiResponse { Code = 400, Message = "账号锁定" };
            }
            //验证密码
            if (!PasswordHash.ValidatePassword(request.Password, admin.Password))
            {
                return new BaseApiResponse { Code = 400, Message = "登录密码错误" };
            }
            SignIn(admin.Id.ToString(), admin.Name, true);
            //操作记录
            RecordOperat(admin.Id, "登陆系统",Guid.Empty, "登陆系统");
            return new LoginResponse
            {
                User = new User
                {
                    Id = admin.Id,
                    LoginName = admin.LoginName,
                    Name = admin.Name,
                    Role = admin.Role.ToString(),
                    Portrait = admin.Portrait
                }
            };
        }

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Add(AddAdminRequest request)
        {
            request.CheckNotNull(nameof(request));

            var newadminid = GuidUtil.NewSequentialId();
            var command = new CreateAdminCommand(
                newadminid,
                request.Name,
                request.LoginName,
                request.Portrait,
                PasswordHash.CreateHash(request.Password),
                request.Role,
                request.IsLocked
                );
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "添加管理员", newadminid, "管理员：{0}".FormatWith(request.Name));

            return new BaseApiResponse();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Edit(EditAdminRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var admin = _adminQueryService.Find(request.Id);
            if (admin == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }

            var command = new UpdateAdminCommand(
                request.Name,
                request.LoginName,
                request.Portrait,
                request.Role,
                request.IsLocked
                )
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
            RecordOperat(currentAdmin.AdminId.ToGuid(), "编辑管理员", request.Id, "管理员：{0}".FormatWith(admin.Name));

            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> Delete(DeleteRequest request)
        {
            request.CheckNotNull(nameof(request));
            //判断
            var admin = _adminQueryService.Find(request.Id);
            if (admin == null)
            {
                return new BaseApiResponse { Code = 400, Message = "没找到该用户" };
            }
            //删除
            var command = new DeleteAdminCommand
            {
                AggregateRootId =request.Id
            };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "删除管理员", request.Id, "管理员：{0}".FormatWith(admin.Name));

            return new BaseApiResponse();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ResetPassword(ResetPasswordRequest request)
        {
            var admin = _adminQueryService.Find(request.Id);
            if (admin == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到账户" };
            }
            var pasword = "Abc123456";
            var passwordHash = PasswordHash.CreateHash(pasword);
            var command = new UpdatePasswordCommand(passwordHash) { AggregateRootId = request.Id };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }

            //添加操作记录
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);
            RecordOperat(currentAdmin.AdminId.ToGuid(), "重置管理员密码", request.Id, "管理员：{0}".FormatWith(admin.Name));

            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        public async Task<BaseApiResponse> ChangePassword(ChangePasswordRequest request)
        {
            var currentAdmin = _contextService.GetCurrentAdmin(HttpContext.Current);

            var admin = _adminQueryService.Find(currentAdmin.AdminId.ToGuid());
            if (admin == null)
            {
                return new BaseApiResponse { Code = 400, Message = "未找到账户" };
            }
            //验证密码
            if (!PasswordHash.ValidatePassword(request.OldPassword, admin.Password))
            {
                return new BaseApiResponse { Code = 400, Message = "原密码错误" };
            }
            if (request.OldPassword.Length > 20)
            {
                return new BaseApiResponse { Code = 400, Message = "密码长度不能大于20字符" };
            }
            if (request.OldPassword.Contains(" "))
            {
                return new BaseApiResponse { Code = 400, Message = "密码不能包含空格." };
            }
            var passwordHash = PasswordHash.CreateHash(request.NewPassword);
            var command = new UpdatePasswordCommand(passwordHash) { AggregateRootId = currentAdmin.AdminId.ToGuid() };
            var result = await ExecuteCommandAsync(command);
            if (!result.IsSuccess())
            {
                return new BaseApiResponse { Code = 400, Message = "命令没有执行成功：{0}".FormatWith(result.GetErrorMessage()) };
            }
            return new BaseApiResponse();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse ListPage(ListPageRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var admins = _adminQueryService.Admins();
            var total = admins.Count();

            admins = admins.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new ListResponse
            {
                Total = total,
                Admins = admins.Select(x => new Admin
                {
                    Id = x.Id,
                    Name = x.Name,
                    LoginName = x.LoginName,
                    Portrait = x.Portrait,
                    Role = x.Role.ToString(),
                    IsLocked = x.IsLocked,
                    CreatedOn = x.CreatedOn
                }).ToList()
            };
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public BaseApiResponse OperatRecords(OperatRecordsRequest request)
        {
            request.CheckNotNull(nameof(request));

            var pageSize = 20;
            var operatRecords = _adminQueryService.OperatRecords();
            var total = operatRecords.Count();
            //筛选
            if(!request.AdminName.IsNullOrEmpty())
            {
                operatRecords = operatRecords.Where(x => x.AdminName.Contains(request.AdminName));
            }
            if (request.Operat!="全部")
            {
                operatRecords = operatRecords.Where(x => x.Operat.Contains(request.Operat));
            }
            if (!request.Remark.IsNullOrEmpty())
            {
                operatRecords = operatRecords.Where(x => x.Remark.Contains(request.Remark));
            }
            //分页
            operatRecords = operatRecords.OrderByDescending(x => x.CreatedOn).Skip(pageSize * (request.Page - 1)).Take(pageSize);
            return new OperatRecordsResponse
            {
                Total = total,
                OperatRecords = operatRecords.Select(x => new OperatRecord
                {
                    Id = x.Id,
                    AdminId=x.AdminId,
                    AboutId=x.AboutId,
                    AdminName = x.AdminName,
                    Operat = x.Operat,
                    Remark = x.Remark,
                    CreatedOn = x.CreatedOn.GetTimeSpan()
                }).ToList()
            };
        }

        #region 私有方法
        private void SignIn(string adminId, string name, bool persistentCookie)
        {
            var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,adminId),
                    new Claim(ClaimTypes.Name,name)
                }, DefaultAuthenticationTypes.ApplicationCookie);

            //specify the context
            HttpContext.Current.GetOwinContext().Authentication.SignIn(identity);
        }
        #endregion
    }
}
