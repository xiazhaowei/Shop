using Xia.Common.Extensions;
using ECommon.IO;
using ENode.Commanding;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shop.Apis.Services;
using Shop.Apis.Helpers;
using Microsoft.AspNetCore.Cors;

namespace Shop.Apis.Controllers
{
    [EnableCors("AllowSameDomain")]
    public class BaseApiController : Controller
    {
        protected readonly IContextService _contextService;
        protected ICommandService _commandService;//C端
        protected ApiSession _apiSession = ApiSession.CreateInstance();

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
