using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("teamrank")]
    public class TeamRank
    {
        [Key]
        public int Id { get; set; }
        public string Team { get; set; } = "";
        public int Rank { get; set; } = 0;
        public int RankCompetitionId { get; set; }
        public virtual ICollection<UserRank>? UserRanks { get; set; }
        public virtual RankCompetition RankCompetition { get; set; } = null!;


    }
}
