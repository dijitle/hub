using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dijitle.Hub.Models
{
  public class Messages : LinkedList<Message>
  {
    public void AddMessage(string name, string message)
    {
      AddLast(new Message(Last?.Value.Id, name, message));
    }

    public IEnumerable<Message> GetLastMessages(int amount = 25, int page = 1)
    {
      return this.SkipLast((page - 1) * amount).TakeLast(amount);
    }

    public bool IsValid()
    {
      foreach(var m in this)
      {
        if (!m.IsValid())
        {
          return false;
        }
      }

      return true;
    }
  }
}
