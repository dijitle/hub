using Dijitle.Hub.Models;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace Dijitle.Hub.Tests
{
  public class Tests
  {
    private LinkedList<Message> _messages;

    [SetUp]
    public void Setup()
    {
      _messages = new LinkedList<Message>();
    }

    [Test]
    public void OneMessageIsValid()
    {
      _messages.AddLast(new Message(null, "Guest 1", "Hello!"));
      Assert.That(_messages.Select(m => m.IsValid()), Is.All.True);
    }

    [Test]
    public void TwoMessagesAreValid()
    {
      _messages.AddLast(new Message(null, "Guest 1", "Hello!"));
      _messages.AddLast(new Message(_messages.Last.Value.Id, "Guest 1", "Hello!"));

      Assert.That(_messages.Select(m => m.IsValid()), Is.All.True);
    }

    [Test]
    public void MessagesManipulatedNotValid()
    {
      _messages.AddLast(new Message(null, "Guest 1", "Hello!"));

      _messages.Last.Value.MessageContent = "hello!";

      Assert.That(_messages.Select(m => m.IsValid()), Is.All.False);
    }
  }
}