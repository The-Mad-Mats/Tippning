using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Models
{
    public class TeamRank
    {
        public int Id { get; set; }
        public string Team { get; set; } = "";
        public int TeamId { get; set; }
        public int Rank { get; set; } = 0;

    }
}
