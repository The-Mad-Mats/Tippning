

namespace TipsWebApi.Models.F1
{
    public class F1Season
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<F1Qualifying> Qualifyings { get; set; }
        public List<F1Race> Races { get; set; }
    }
}
