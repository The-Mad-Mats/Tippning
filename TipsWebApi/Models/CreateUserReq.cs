
namespace TipsWebApi.Models
{
    public class CreateUserReq
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Team { get; set; } = "";
    }
}
