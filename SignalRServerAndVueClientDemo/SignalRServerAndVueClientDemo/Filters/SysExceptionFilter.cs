using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRServerAndVueClientDemo.Hubs;
using SignalRServerAndVueClientDemo.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServerAndVueClientDemo.Filters
{
    public class SysExceptionFilter : IAsyncExceptionFilter
    {
        readonly IHubContext<ChatHub> _hub;
        ILog _log = LogManager.GetLogger(Startup.Logger.Name, typeof(SysExceptionFilter));
        public SysExceptionFilter(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            string message = ex.Message;
            string url = context.HttpContext?.Request.Path;
            string logMessage = $"错误信息=>【{message}】，【请求地址=>{url}】";
            _log.Error(logMessage);
            var data = ReadHelper.Read();
            await _hub.Clients.All.SendAsync("ReceiveLog", data);
            context.Result = new JsonResult(new { ErrCode = 0, ErrMsg = message, Data = true });
        }
    }
}
