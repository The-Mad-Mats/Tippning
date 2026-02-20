namespace TipsWebApi.Models
{
    public class CalcResultReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<GameAdmin> Games { get; set; } = new();
    }
}
