namespace TipsWebApi.Models
{
    public class CalcResultReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<Game> Games { get; set; } = new();
    }
}
