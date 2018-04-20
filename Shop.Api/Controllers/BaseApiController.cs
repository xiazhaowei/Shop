using ECommon.IO;
using ENode.Commanding;
using Shop.Api.Services;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Xia.Common.Extensions;

namespace Shop.Api.Controllers
{
    public class BaseApiController :ApiController
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
