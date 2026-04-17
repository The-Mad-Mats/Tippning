using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("competition")]
    public class Competition
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Deadline { get; set; }
        public virtual ICollection<League>? Leagues { get; set; }
        public virtual ICollection<Game>? Games { get; set; }
        public virtual ICollection<UserCompetition>? UserCompetitions { get; set; }

    }
}
