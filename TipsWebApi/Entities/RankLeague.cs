using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("rankleague")]
    public class RankLeague
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
        public virtual ICollection<UserRankLeague>? UserRankLeagues { get; set; }
    }
}
