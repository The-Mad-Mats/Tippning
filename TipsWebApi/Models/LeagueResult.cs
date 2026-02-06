namespace TipsWebApi.Models
{
    public class LeagueResult
    {
        public List<LeagueRow> Rows { get; set; } = new List<LeagueRow>();
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}
