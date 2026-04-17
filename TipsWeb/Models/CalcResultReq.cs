namespace TipsWeb.Models
{
    public class CalcResultReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<Game> Games { get; set; } = new();
        public int CompetitionId { get; set; }
    }
}
