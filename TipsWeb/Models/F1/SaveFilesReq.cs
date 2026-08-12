
namespace TipsWeb.Models.F1
{
    public class SaveFilesReq
    {
        public int UserId { get; set; }
        public string Token { get; set; } = "";
        public List<F1Season> Seasons { get; set; }
    }
}
