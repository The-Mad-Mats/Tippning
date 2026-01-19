namespace TipsWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Team { get; set; } = "";
        public int Points { get; set; }
        public string Token { get; set; } = "";
    }
}
