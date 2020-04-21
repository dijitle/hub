using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dijitle.hub.Hubs
{
  public class hubHub : Hub
  {
    public async Task Message(string name, string message)
    {
      await Clients.All.SendAsync("message", name, message);
    }
  }
}
