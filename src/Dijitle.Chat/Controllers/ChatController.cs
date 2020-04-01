using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dijitle.Chat.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ChatController : ControllerBase
  {
    [HttpGet("hello")]
    public ActionResult<string> Get()
    {
      return Ok("Hello!");
    }

    [HttpGet("hello2")]
    public ActionResult<string> Get2()
    {
      return Ok("Hello!");
    }
  }
}
