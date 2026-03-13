using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWeb.Models
{
    public class RankCompetition
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Deadline { get; set; }
    }
}
