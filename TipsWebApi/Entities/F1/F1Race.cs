using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("f1race")]
    public class F1Race
    {
        [Key]
        public int Id { get; set; }
        public string Track { get; set; } = "";
        public DateTime RaceDate { get; set; }
        public int F1SeasonId { get; set; }
        public virtual F1Season? Season { get; set; }
        public virtual ICollection<F1RaceRows>? RaceRows { get; set; }

    }
}
