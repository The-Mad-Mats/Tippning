using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("f1qrows")]
    public class F1QualifyingRows
    {
        [Key]
        public int Id { get; set; }
        public string Position { get; set; } = "";
        public string Driver { get; set; } = "";
        public string Team { get; set; } = "";
        public string Best { get; set; } = "";
        public string Gap { get; set; } = "";
        public int F1QualifyingId { get; set; }

        public virtual F1Qualifying? Qualifying { get; set; }
    }
}
