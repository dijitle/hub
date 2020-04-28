using Dijitle.Hub.Models;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace Dijitle.Hub.Tests
{
  public class Tests
  {
    private Messages _messages;

    [SetUp]
    public void Setup()
    {
      _messages = new Messages();
    }

    [Test]
    public void OneMessageIsValid()
    {
      _messages.AddMessage("Guest 1", "Hello!");
      Assert.That(_messages.IsValid(), Is.True);
    }

    [Test]
    public void TwoMessagesAreValid()
    {
      _messages.AddMessage("Guest 1", "Hello!");
      _messages.AddMessage("Guest 2", "Hello yourself!");

      Assert.That(_messages.IsValid(), Is.True);
    }

    [Test]
    public void MessagesManipulatedNotValid()
    {
      _messages.AddLast(new Message(null, "Guest 1", "Hello!"));

      _messages.Last.Value.MessageContent = "hello!";

      Assert.That(_messages.IsValid(), Is.False);
    }

    [Test]
    public void GetMessageReturnsEmptyIfEmpty()
    {
      Assert.That(_messages.GetLastMessages().ToList(), Is.Empty);
    }

    [Test]
    public void GetMessageReturnsIfLessThan25()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetLastMessages(25, 1).ToList();
      Assert.That(ms, Has.Count.EqualTo(5));
    }

    [Test]
    public void GetMessageReturns3if5Messages()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetLastMessages(3, 1).ToList();

      Assert.That(ms, Has.Count.EqualTo(3));
      Assert.That(ms[0].Name, Is.EqualTo("Guest3"));
      Assert.That(ms[1].Name, Is.EqualTo("Guest4"));
      Assert.That(ms[2].Name, Is.EqualTo("Guest5"));
    }

    [Test]
    public void GetMessageReturns2if5MessagesTake3onpage2()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetLastMessages(3, 2).ToList();

      Assert.That(ms, Has.Count.EqualTo(2));
      Assert.That(ms[0].Name, Is.EqualTo("Guest1"));
      Assert.That(ms[1].Name, Is.EqualTo("Guest2"));
    }

    [Test]
    public void GetMessageReturns0if5MessagesTake3onpage3()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetLastMessages(3, 3).ToList();

      Assert.That(ms, Is.Empty);
    }
  }
}