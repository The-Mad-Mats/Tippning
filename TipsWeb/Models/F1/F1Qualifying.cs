
namespace TipsWeb.Models.F1
{
    public class F1Qualifying
    {
        public string Track { get; set; } = "";
        public DateTime QualiDate { get; set; }
        public List<F1QualifyingRows>? QualifyingRows { get; set; }

    }
}
