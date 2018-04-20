using ECommon.IO;
using ENode.Commanding;
using Shop.AdminApi.Services;
using Shop.Commands.Admins;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Xia.Common.Extensions;

namespace Shop.AdminApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected readonly IContextService _contextService;
        protected ICommandService _commandService;//C端

        public BaseApiController(ICommandService commandService,IContextService contextService)
        {
            _commandService = commandService;
            _contextService = contextService;
        }

        #region 私有方法
        //创建操作记录
        protected void RecordOperat(Guid adminId,string operat,Guid aboutId,string remark)
        {
            var command = new NewOperatRecordCommand(operat, aboutId, remark)
            {
                AggregateRootId = adminId
            };
            ExecuteCommandAsync(command);
        }
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
