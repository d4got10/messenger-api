using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace network_technologies.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("weather")]
    public class WeatherController : ControllerBase
    {
        private static readonly Dictionary<string, List<string>> Messages = new Dictionary<string, List<string>>();

        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get([FromQuery(Name = "user")] string user)
        {
            if (string.IsNullOrEmpty(user))
                return ConvertMessages(new List<string>() { "abc", "123" });
            Messages.TryGetValue(user, out var messages);
            return ConvertMessages(messages);
        }

        [HttpPost]
        public string Post([FromQuery(Name = "user")] string user, [FromQuery(Name = "message")] string message)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(message))
                return Get(user);

            if(Messages.ContainsKey(user) == false)
                Messages[user] = new List<string>();

            Messages[user].Add(message);
            return Get(user);
        }


        private string ConvertMessages(List<string> messages)
        {
            return JsonConvert.SerializeObject(new Dictionary<string, string[]>() { { "messages", messages == null ? null : messages.ToArray() } });
        }
    }
}
