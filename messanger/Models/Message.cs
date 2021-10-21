using System;

namespace messanger.Models
{
    public class Message
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public string Data { get; set; }
    }
}
