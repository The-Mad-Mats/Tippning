using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("userrankleague")]
    public class UserRankLeague
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RankLeagueId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual RankLeague? RankLeague { get; set; } = null!;
    }
}
