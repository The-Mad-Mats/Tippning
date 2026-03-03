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
        public virtual ICollection<UserRank>? UserRanks { get; set; }

    }
}
