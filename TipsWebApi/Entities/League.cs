using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("league")]
    public class League
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; } = "";
        public virtual ICollection<UserLeague>? UserLeagues { get; set; }
    }
}
