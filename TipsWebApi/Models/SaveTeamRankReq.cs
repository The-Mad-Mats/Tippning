namespace TipsWebApi.Models
{
    public class SaveTeamRankReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<TeamRank> TeamRanks { get; set; } = new();
    }

}
