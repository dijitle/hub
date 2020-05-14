using Dijitle.Hub.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dijitle.Hub.Hubs
{
  public interface IChat
  {
    Task MessageReceived(Message message);
    Task GetMessages(IEnumerable<Message> messages);
    Task GetRooms();
    Task AddRoom(string name);
    Task RemoveRoom(string name);
  }
}
