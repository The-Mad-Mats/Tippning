using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class League
    {
        [Inject] public Proxy _proxy { get; set; }
        private List<LeagueRow> products = new();
        private List<Models.LeagueRow> LeagueRows = new();
        private List<Models.League> Leagues = new List<Models.League> { };
        private string selectedLeague = "";
        private User user = new();
        protected override async Task OnInitializedAsync()
        {
            user = AppState.CurrentUser;
            if (user != null)
            {
                Leagues = await _proxy.GetUserleague(new GetUserLeagueReq { UserId = user.Id, Token = user.Token });
                selectedLeague = Leagues.FirstOrDefault()?.Name ?? "";
                await OnCategoryChanged();
            }
        }
        private async Task OnCategoryChanged()
        {
            if (string.IsNullOrEmpty(selectedLeague))
            {
                LeagueRows = new List<LeagueRow>();
            }
            else
            {
                var leagueId = Leagues.FirstOrDefault(l => l.Name == selectedLeague)?.Id ?? 0;
                LeagueRows = await _proxy.GetLeague(new GetLeagueReq { LeagueId = leagueId, UserId = user.Id, Token = user.Token });
            }
        }
        //public class LeagueRow
        //{
        //    public int Position {get; set; }
        //    public string Team {get; set; } = string.Empty;
        //    public string Liga {get; set; } = string.Empty;
        //    public string Owner { get; set; } 
        //    public int Points { get; set; }
            
        //}
    }
}