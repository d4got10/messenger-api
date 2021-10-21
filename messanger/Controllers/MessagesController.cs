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
        public async Task<string> Get([FromHeader(Name = "Sender")] string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var user = await _usersContext.Users.FirstOrDefaultAsync((t) => t.Email == username);
            var messages = _messagesContext.Messages
                .Where((t) => t.ReceiverId == user.Id)
                .Select((t) => t.SenderId + ":" + t.Data);

            var t = ConvertMessages(messages);
            return t;
        }

        [HttpPost]
        public async Task<string> Post([FromHeader(Name = "Sender")]string senderUserName,
            [FromHeader(Name = "Receiver")] string receiverUserName,
            [FromHeader(Name = "Message")] string message)
        {
            if (string.IsNullOrEmpty(senderUserName) || string.IsNullOrEmpty(receiverUserName) || string.IsNullOrEmpty(message))
                return "false";

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

                return "true";
            }
            catch (Exception ex)
            {
                return "false";
            }
        }


        private string ConvertMessages(IQueryable<string> messages)
        {
            return JsonConvert.SerializeObject(new Dictionary<string, string[]>() { { "messages", messages == null ? null : messages.ToArray() } });
        }
    }
}
