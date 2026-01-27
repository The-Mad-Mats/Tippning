namespace TipsWeb.Models
{
    public class GameUser
    {
        public DateTime DateTime { get; set; } = DateTime.MinValue;
        public string Team1Flag { get; set; } = "";
        public string Team1 { get; set; } = "";
        public string Team2Flag { get; set; } = "";
        public string Team2 { get; set; } = "";
        public int Team1Score { get; set; } = 0;
        public int Team2Score { get; set; } = 0;
    }
}
