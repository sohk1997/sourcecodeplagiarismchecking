using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.SignalRHub
{
    public class TestHub : Hub
    {
        public Task TestFunction(string text)
        {
            return Clients.All.SendAsync("OnTestFunction", text);
        }
    }
}
