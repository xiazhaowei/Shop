using ECommon.IO;
using ENode.Commanding;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shop.AdminApis.Services;
using System.Threading.Tasks;
using Xia.Common.Extensions;

namespace Shop.AdminApis.Controllers
{
    [EnableCors("AllowSameDomain")]
    public class BaseApiController : Controller
    {
        protected readonly IContextService _contextService;
        protected ICommandService _commandService;//C端

        public BaseApiController(ICommandService commandService,IContextService contextService)
        {
            _commandService = commandService;
            _contextService = contextService;
        }

        #region 私有方法
        protected Task<AsyncTaskResult<CommandResult>> ExecuteCommandAsync(ICommand command, int millisecondsDelay = 50000)
        {
            return _commandService.ExecuteAsync(command, CommandReturnType.CommandExecuted).TimeoutAfter(millisecondsDelay);
        }
        protected CommandResult ExecuteCommand(ICommand command, int millisecondsDelay = 50000)
        {
            return _commandService.Execute(command, millisecondsDelay);
        }
        #endregion
    }
}
