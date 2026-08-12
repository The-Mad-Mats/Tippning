namespace TipsWebApi.Models.F1
{
    public class GetF1TracksReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public int SeasonId { get; set; }
    }
}
