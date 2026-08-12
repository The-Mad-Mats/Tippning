
namespace TipsWeb.Models.F1
{
    public class F1QualifyingRows
    {
        public int Id { get; set; }
        public string Position { get; set; } = "";
        public string Driver { get; set; } = "";
        public string Team { get; set; } = "";
        public string Best { get; set; } = "";
        public string Gap { get; set; } = "";
        public string Time => Position == "1" ? Best : Gap; // If position is 1, return Best, otherwise return Gap
    }
}
