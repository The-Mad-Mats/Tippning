using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("userrank")]
    public class UserRank
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeamRankId { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual TeamRank? TeamRank { get; set; } = null!;
    }
}
