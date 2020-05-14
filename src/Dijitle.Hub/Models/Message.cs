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
      Content = message;
      DateSent = DateTime.Now;

      using(var hash = SHA512.Create())
      {
        var bytes = Encoding.UTF8.GetBytes($"{PreviousId}{Name}{Content}{DateSent.Ticks}");

        Id = string.Concat(Array.ConvertAll(hash.ComputeHash(bytes), x => x.ToString("X2")));
      }
    }

    public string Id { get; }
    public string PreviousId { get; }
    public string Name { get; }
    public string Content { get; private set; }
    public DateTime DateSent { get; }

    public bool IsValid()
    {
      using (var hash = SHA512.Create())
      {
        var bytes = Encoding.UTF8.GetBytes($"{PreviousId}{Name}{Content}{DateSent.Ticks}");

        return Id == string.Concat(Array.ConvertAll(hash.ComputeHash(bytes), x => x.ToString("X2")));
      }
    }
  }
}
