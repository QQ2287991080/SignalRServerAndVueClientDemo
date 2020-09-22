using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServerAndVueClientDemo.Hubs
{
    /// <summary>
    /// https://docs.microsoft.com/zh-cn/aspnet/core/signalr/hubs?view=aspnetcore-3.1
    /// 强类型中心
    /// </summary>
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveMessage(object message);
        Task ReceiveCaller(object message);
        Task ReceiveLog(object data);
    }
}
