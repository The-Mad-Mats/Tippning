namespace TipsWeb.Models
{
    public class CreateOrJoinLeageReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public string LeagueName { get; set; } = "";
        public string LeaguePassword { get; set; } = "";
    }
}
