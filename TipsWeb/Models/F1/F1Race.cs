
namespace TipsWeb.Models.F1
{
    public class F1Race
    {
        public string Track { get; set; } = "";
        public DateTime RaceDate { get; set; }
        public List<F1RaceRows>? RaceRows { get; set; }

    }
}
