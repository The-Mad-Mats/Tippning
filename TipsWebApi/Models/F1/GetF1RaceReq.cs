namespace TipsWebApi.Models.F1
{
    public class GetF1RaceReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public int SeasonId { get; set; }
        public int RaceId { get; set; }
    }
}
