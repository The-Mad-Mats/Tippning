using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("usergame")]
    public class UserGame
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public int? Points { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual Game? Game { get; set; } = null!;
    }
}
