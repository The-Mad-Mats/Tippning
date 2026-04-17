using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("usercompetition")]
    public class UserCompetition
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompetitionId { get; set; }
        public int Points { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Competition Competition { get; set; } = null!;
    }
}
