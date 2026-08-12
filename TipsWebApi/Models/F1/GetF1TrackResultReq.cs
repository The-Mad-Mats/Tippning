namespace TipsWebApi.Models.F1
{
    public class GetF1TrackResultReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public int SeasonId { get; set; }
        public string TrackName { get; set; }
    }
}
