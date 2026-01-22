using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWeb.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
