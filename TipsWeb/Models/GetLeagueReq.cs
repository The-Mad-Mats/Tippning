namespace TipsWeb.Models
{
    public class GetLeagueReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public int LeagueId { get; set; }
    }
}
