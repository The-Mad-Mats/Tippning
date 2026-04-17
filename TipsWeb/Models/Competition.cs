using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWeb.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime Deadline { get; set; }
    }
}
