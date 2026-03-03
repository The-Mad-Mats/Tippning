namespace TipsWebApi.Models
{
    public class RankLeague
    {
        public List<TeamRank> TeamRanks { get; set; } = new();
        public List<UserTeamRank> UserTeamRanks { get; set; } = new();
    }
}
