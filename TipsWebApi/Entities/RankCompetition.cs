using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("rankcompetition")]
    public class RankCompetition
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Deadline { get; set; }
        public virtual ICollection<RankLeague>? RankLeagues { get; set; }
        public virtual ICollection<TeamRank>? TeamRanks { get; set; }

    }
}
