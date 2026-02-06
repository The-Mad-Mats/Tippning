namespace TipsWebApi.Models
{
    public class UserMatch
    {
        public string Team { get; set; } = "";
        public string Owner { get; set; } = "";
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public int? Points { get; set; }
    }
}
