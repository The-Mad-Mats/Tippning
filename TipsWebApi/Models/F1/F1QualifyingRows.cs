using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Models.F1
{
    public class F1QualifyingRows
    {
        public int Id { get; set; }
        public string Position { get; set; } = "";
        public string Driver { get; set; } = "";
        public string Team { get; set; } = "";
        public string Best { get; set; } = "";
        public string Gap { get; set; } = "";
    }
}
