using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using messanger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Text;

namespace messanger.Controllers
{

    [Route("sse")]
    [ApiController]
    public class SSEController : ControllerBase
    {
        [HttpGet]
        public async Task Get()
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            for(int i = 0; i < 4; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                string message = i.ToString();
                byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(message);
                await Response.Body.WriteAsync(messageBytes, 0, messageBytes.Length);
                await Response.Body.FlushAsync();
            }
        }
    }
}
