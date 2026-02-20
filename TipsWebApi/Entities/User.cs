using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Team { get; set; } = "";
        public int Points { get; set; }
        public string Token { get; set; } = "";
        public bool Admin { get; set; }
        public DateTime? LastLogin { get; set; } = DateTime.MinValue;
        public virtual ICollection<UserLeague>? UserLeagues { get; set; }
        public virtual ICollection<UserGame>? UserGames{ get; set; }

    }
}
