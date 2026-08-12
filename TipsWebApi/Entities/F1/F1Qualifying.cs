using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("f1qualifying")]
    public class F1Qualifying
    {
        [Key]
        public int Id { get; set; }
        public string Track { get; set; } = "";
        public DateTime QualiDate { get; set; }
        public int F1SeasonId { get; set; }
        public virtual F1Season? Season { get; set; }
        public virtual ICollection<F1QualifyingRows>? QualifyingRows { get; set; }

    }
}
