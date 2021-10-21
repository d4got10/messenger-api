using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using messanger.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace messanger.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        private UserContext _usersContext;

        public UsersController(UserContext usersContext, ILogger<UsersController> logger)
        {
            _usersContext = usersContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get([FromHeader(Name = "Sender")] string exceptUser)
        {
            var user = await _usersContext.Users.FirstOrDefaultAsync((t) => t.Email == exceptUser);
            var allUsersInfos = await _usersContext.Users
                .Where(t => t != user)
                .Select(t => new UserViewModel(t))
                .ToArrayAsync();

            return JsonConvert.SerializeObject(allUsersInfos);
        }
    }
}
