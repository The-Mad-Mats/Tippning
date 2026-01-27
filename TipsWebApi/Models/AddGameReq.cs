namespace TipsWebApi.Models
{
    public class AddGameReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public string HomeTeam { get; set; } = "";
        public string AwayTeam { get; set; } = "";
    }
}
