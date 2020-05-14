using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dijitle.Hub.Models
{
  public class Messages : LinkedList<Message>
  {

    public Message AddMessage(string name, string message)
    {
      var m = new Message(Last?.Value.Id, name, message);
      AddLast(m);
      return m;
    }

    public IEnumerable<Message> GetMessagesBefore(int amount, string id)
    {
      return this.Reverse().SkipWhile(m => m.Id != id).Skip(1).Take(amount).Reverse();
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
