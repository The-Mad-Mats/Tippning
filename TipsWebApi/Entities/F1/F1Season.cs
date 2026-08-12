using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("f1season")]
    public class F1Season
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public virtual ICollection<F1Qualifying>? Qualifyings { get; set; }
        public virtual ICollection<F1Race>? Races { get; set; }

    }
}
