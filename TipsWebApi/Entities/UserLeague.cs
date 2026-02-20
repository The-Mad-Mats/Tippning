using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("userleague")]
    public class UserLeague
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LeagueId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual League? League { get; set; } = null!;
    }
}
