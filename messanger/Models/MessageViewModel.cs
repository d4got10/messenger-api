namespace messanger.Models
{
    public class MessageViewModel
    {
        public string Email { get; set; }
        public string Message { get; set; }

        public MessageViewModel(string email, string message)
        {
            Email = email;
            Message = message;
        }
    }
}
