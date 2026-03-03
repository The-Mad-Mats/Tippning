namespace TipsWeb.Models
{
    public class SaveUserRankReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<TeamRank> TeamRanks { get; set; } = new();
    }

}
