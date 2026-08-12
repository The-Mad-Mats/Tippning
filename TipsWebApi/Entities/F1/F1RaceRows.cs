using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Entities
{
    [Table("f1rrows")]
    public class F1RaceRows
    {
        [Key]
        public int Id { get; set; }
        public string Position { get; set; } = "";
        public string Driver { get; set; } = "";
        public string Team { get; set; } = "";
        public string Grid { get; set; } = "";
        public string Stops { get; set; } = "";
        public string Best { get; set; } = "";
        public string Time { get; set; } = "";
        public string Points { get; set; } = "";
        public string Penalties { get; set; } = "";
        public string PenTime { get; set; } = "";
        public int F1RaceId { get; set; }
        public virtual F1Race? Race { get; set; }
    }
}
