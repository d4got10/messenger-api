using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using messanger.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace network_technologies.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;

        private MessagesContext _messagesContext;
        private UserContext _usersContext;

        public MessagesController(UserContext usersContext, MessagesContext messagesContext, ILogger<MessagesController> logger)
        {
            _usersContext = usersContext;
            _messagesContext = messagesContext;
            _logger = logger;
        }


        [HttpGet]
        public async Task<string> Get([FromHeader(Name = "Sender")] string username, [FromHeader(Name = "Receiver")] string fromUsername)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var user = await _usersContext.Users.FirstOrDefaultAsync((t) => t.Email == username);
            var sender = await _usersContext.Users.FirstOrDefaultAsync((t) => t.Email == fromUsername);

            var messages = _messagesContext.Messages
                .Where((t) => t.ReceiverId == user.Id && t.SenderId == sender.Id || t.ReceiverId == sender.Id && t.SenderId == user.Id)
                .OrderBy((t) => t.Date)
                .Select((t) => new MessageViewModel(t.SenderId == user.Id ? user.Email : sender.Email, t.Data))
                .Select((t) => JsonConvert.SerializeObject(t));

            var t = ConvertMessages(messages);
            return t;
        }


        [HttpPost]
        public async Task<StatusCodeResult> Post([FromHeader(Name = "Sender")]string senderUserName,
            [FromHeader(Name = "Receiver")] string receiverUserName,
            [FromBody] string message)
        {
            if (string.IsNullOrEmpty(senderUserName) || string.IsNullOrEmpty(receiverUserName) || string.IsNullOrEmpty(message))
                return StatusCode(400);

            try
            {
                var sender = await _usersContext.Users.FirstOrDefaultAsync((t) => t.Email == senderUserName);
                var receiver = await _usersContext.Users.FirstOrDefaultAsync((t) => t.Email == receiverUserName);
                _messagesContext.Add(
                    new Message
                    {
                        SenderId = sender.Id,
                        ReceiverId = receiver.Id,
                        Data = message,
                        Date = DateTime.Now
                    });
                await _messagesContext.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }


        private string ConvertMessages(IQueryable<string> messages)
        {
            return JsonConvert.SerializeObject(new Dictionary<string, string[]>() { { "messages", messages == null ? null : messages.ToArray() } });
        }
    }
}
