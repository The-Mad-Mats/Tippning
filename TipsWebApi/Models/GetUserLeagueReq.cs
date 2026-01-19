namespace TipsWebApi.Models
{
    public class GetUserLeagueReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
    }
}
