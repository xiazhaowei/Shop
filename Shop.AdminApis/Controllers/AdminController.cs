using ENode.Commanding;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Extensions;
using Shop.AdminApis.Services;
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
using Xia.Common;
using Xia.Common.Extensions;
using Xia.Common.Secutiry;

namespace Shop.AdminApis.Controllers
{
    [Route("[controller]")]
    public class AdminController : BaseApiController
    {
        private IAdminQueryService _adminQueryService;

        public AdminController(ICommandService commandService,IContextService contextService,
            IAdminQueryService adminQueryService) : base(commandService,contextService)
        {
            _adminQueryService = adminQueryService;
        }

        
        [HttpPost]
        [Route("Login")]
        public async Task<BaseApiResponse> Login([FromBody]LoginRequest request)
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
                await SignInAsync(GuidUtil.NewSequentialId().ToString(), "admin", true);
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
            await SignInAsync(admin.Id.ToString(), admin.Name, true);
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
        [Route("Add")]
        public async Task<BaseApiResponse> Add([FromBody]AddAdminRequest request)
        {
            request.CheckNotNull(nameof(request));

            var command = new CreateAdminCommand(
                GuidUtil.NewSequentialId(),
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
            return new BaseApiResponse();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public async Task<BaseApiResponse> Edit([FromBody]EditAdminRequest request)
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
            return new BaseApiResponse();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public async Task<BaseApiResponse> Delete([FromBody]DeleteRequest request)
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
            return new BaseApiResponse();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ResetPassword")]
        public async Task<BaseApiResponse> ResetPassword([FromBody]ResetPasswordRequest request)
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
            return new BaseApiResponse();
        }

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<BaseApiResponse> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            var admin = _adminQueryService.Find(request.Id);
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
            var command = new UpdatePasswordCommand(passwordHash) { AggregateRootId = request.Id };
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
        [Route("ListPage")]
        public BaseApiResponse ListPage([FromBody]ListPageRequest request)
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

        #region 私有方法
        private Task SignInAsync(string adminId, string name, bool persistentCookie)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, adminId),
                new Claim(ClaimTypes.Name,name)
            }, CookieAuthenticationDefaults.AuthenticationScheme));

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = persistentCookie
            };
            if (persistentCookie)
            {
                authenticationProperties.ExpiresUtc = DateTime.UtcNow.AddDays(365);
            }
            else
            {
                authenticationProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);
            }

            return HttpContext.SignInAsync(user, authenticationProperties);
        }
        #endregion
    }
}
