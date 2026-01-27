namespace TipsWebApi.Models
{
    public class GetGamesReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
    }
}
