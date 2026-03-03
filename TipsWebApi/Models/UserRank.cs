using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TipsWebApi.Models
{
    public class UserRank
    {
        public int TeamId { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
    }
}
