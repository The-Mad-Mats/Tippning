namespace TipsWebApi.Models
{
    public class UserTeamRank
    {
        public string UserName { get; set; } = "";
        public string Team { get; set; } = "";
        public int Points { get; set; }
        public List<UserRank> UserRanks { get; set; } = new();

    }
}
