namespace TipsWebApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime GameTime { get; set; } = DateTime.MinValue;
        public string Team1Flag { get; set; } = "";
        public string Team1 { get; set; } = "";
        public string Team2Flag { get; set; } = "";
        public string Team2 { get; set; } = "";
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public int? Team1Result { get; set; }
        public int? Team2Result { get; set; }
        public int? Points { get; set; }

    }
}
