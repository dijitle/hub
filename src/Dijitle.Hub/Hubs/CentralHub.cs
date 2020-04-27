using Dijitle.Hub.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dijitle.Hub.Hubs
{
  public class CentralHub : Microsoft.AspNetCore.SignalR.Hub
  {
    private List<Message> _messages = new List<Message>();

    public async Task Message(string name, string message)
    {
      await Clients.All.SendAsync("message", name, message);
    }
  }
}
