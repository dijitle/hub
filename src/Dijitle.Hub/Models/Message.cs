using System;
using System.Security.Cryptography;
using System.Text;

namespace Dijitle.Hub.Models
{
  public class Message
  {
    public Message(string previousMessageId, string name, string message)
    {
      PreviousId = previousMessageId;
      Name = name;
      MessageContent = message;
      MessageSent = DateTime.Now;

      using(var hash = SHA512.Create())
      {
        var bytes = Encoding.UTF8.GetBytes($"{PreviousId}{Name}{MessageContent}{MessageSent.Ticks}");

        Id = string.Concat(Array.ConvertAll(hash.ComputeHash(bytes), x => x.ToString("X2")));
      }
    }

    public string Id { get; set; }
    public string PreviousId { get; set; }
    public string Name { get; set; }
    public string MessageContent { get; set; }
    public DateTime MessageSent { get; set; }

    public bool IsValid()
    {
      using (var hash = SHA512.Create())
      {
        var bytes = Encoding.UTF8.GetBytes($"{PreviousId}{Name}{MessageContent}{MessageSent.Ticks}");

        return Id == string.Concat(Array.ConvertAll(hash.ComputeHash(bytes), x => x.ToString("X2")));
      }
    }
  }
}
