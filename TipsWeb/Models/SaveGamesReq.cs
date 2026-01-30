namespace TipsWeb.Models
{
    public class SaveGamesReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<SaveGame> Games { get; set; } = new();
    }

    public class SaveGame
    {
        public int GameId { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
    }
}
