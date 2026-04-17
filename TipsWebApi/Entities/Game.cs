using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("game")]
    public class Game
    {
        [Key]
        public int Id { get; set; }
        public DateTime GameTime { get; set; } = DateTime.MinValue;
        public string Team1 { get; set; } = "";
        public string Team2 { get; set; } = "";
        public int? Team1Score { get; set; } = 0;
        public int? Team2Score { get; set; } = 0;
        public int CompetitionId { get; set; }

        public virtual ICollection<UserGame>? UserGames { get; set; }
        public virtual Competition Competition { get; set; } = null!;


    }
}
