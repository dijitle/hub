using Dijitle.Hub.Models;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

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
      var m = _messages.AddMessage("Guest 1", "Hello!");

      Assert.That(_messages.IsValid(), Is.True);
      Assert.That(m.IsValid, Is.True);
      Assert.That(m.Name, Is.EqualTo("Guest 1"));
      Assert.That(m.Content, Is.EqualTo("Hello!"));
    }

    [Test]
    public void TwoMessagesAreValid()
    {
      _messages.AddMessage("Guest 1", "Hello!");
      _messages.AddMessage("Guest 2", "Hello yourself!");

      Assert.That(_messages.IsValid(), Is.True);
    }

    [Test]
    public void GetMessageReturnsEmptyIfEmpty()
    {
      Assert.That(_messages.GetMessagesBefore(25, "230948u23904823").ToList(), Is.Empty);
    }

    [Test]
    public void GetMessageReturnsIfLessThan25()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      var m = _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetMessagesBefore(25, m.Id).ToList();
      Assert.That(ms, Has.Count.EqualTo(4));
    }

    [Test]
    public void GetMessageReturns3if5Messages()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      var m = _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetMessagesBefore(3, m.Id).ToList();

      Assert.That(ms, Has.Count.EqualTo(3));
      Assert.That(ms[0].Name, Is.EqualTo("Guest2"));
      Assert.That(ms[1].Name, Is.EqualTo("Guest3"));
      Assert.That(ms[2].Name, Is.EqualTo("Guest4"));
    }

    [Test]
    public void GetMessageReturns2if5MessagesTake3onpage2()
    {
      _messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      var m = _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetMessagesBefore(3, m.Id).ToList();

      Assert.That(ms, Has.Count.EqualTo(2));
      Assert.That(ms[0].Name, Is.EqualTo("Guest1"));
      Assert.That(ms[1].Name, Is.EqualTo("Guest2"));
    }

    [Test]
    public void GetMessageReturns0if5MessagesTake3onpage3()
    {
      var m =_messages.AddMessage("Guest1", "Hello!");
      _messages.AddMessage("Guest2", "Hello!");
      _messages.AddMessage("Guest3", "Hello!");
      _messages.AddMessage("Guest4", "Hello!");
      _messages.AddMessage("Guest5", "Hello!");

      var ms = _messages.GetMessagesBefore(3, m.Id).ToList();

      Assert.That(ms, Is.Empty);
    }
  }
}