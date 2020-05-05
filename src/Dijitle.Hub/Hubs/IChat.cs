using System.Threading.Tasks;

namespace Dijitle.Hub.Hubs
{
  public interface IChat
  {
    Task MessageReceived(string name, string message);
    Task GetMessages(int amount, int page);
    Task GetRooms();
    Task AddRoom(string name);
    Task RemoveRoom(string name);
  }
}
