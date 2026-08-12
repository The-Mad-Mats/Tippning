
namespace TipsWeb.Models.F1
{
    public class F1RaceRows
    {
        public int Id { get; set; }
        public string Position { get; set; } = "";
        public string Driver { get; set; } = "";
        public string Team { get; set; } = "";
        public string Grid { get; set; } = "";
        public string Stops { get; set; } = "";
        public string Best { get; set; } = "";
        public string Time { get; set; } = "";
        public string Points { get; set; } = "";
        public string Penalties { get; set; } = "";
        public string PenTime { get; set; } = "";
        public bool Expanded { get; set; }

    }
}
