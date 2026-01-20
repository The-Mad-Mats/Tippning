namespace TipsWebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Team { get; set; } = "";
        public int Points { get; set; }
        public string Token { get; set; } = "";
        public bool Admin { get; set; }
        public virtual ICollection<UserLeague>? UserLeagues { get; set; }

    }
}
