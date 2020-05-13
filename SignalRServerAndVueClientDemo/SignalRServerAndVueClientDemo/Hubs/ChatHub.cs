using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServerAndVueClientDemo.Hubs
{

    public class ChatHub : Hub
    {
        /// <summary>
        /// 给所有客户端发送消息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        /// <summary>
        /// 向调用客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageCaller(string message)
        {
            await Clients.Caller.SendAsync("ReceiveCaller", message);
        }
    }
}
