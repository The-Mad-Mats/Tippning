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
        private int selectedLeague = 0;
        private User user = new();
        private Models.League newLeague = new();
        private Models.League joinLeague = new();
        private bool showCreateLeague = false;
        private bool showJoinLeague = false;

        protected override async Task OnInitializedAsync()
        {
            user = AppState.CurrentUser;
            if (user != null)
            {
                Leagues = await _proxy.GetUserleague(new GetUserLeagueReq { UserId = user.Id, Token = user.Token });
                selectedLeague = Leagues.FirstOrDefault()?.Id ?? 0;
                await OnCategoryChanged();
            }
        }
        private async Task OnCategoryChanged()
        {
            if (selectedLeague == 0)
            {
                LeagueRows = new List<LeagueRow>();
            }
            else
            {
                var leagueId = Leagues.FirstOrDefault(l => l.Id == selectedLeague)?.Id ?? 0;
                LeagueRows = await _proxy.GetLeague(new GetLeagueReq { LeagueId = leagueId, UserId = user.Id, Token = user.Token });
            }
        }

        private async Task JoinLeague()
        {
            OpenPopupJoin();
        }
        private async Task CreateLeague()
        {
            OpenPopupCreate();
        }
        // ========================================
        // POPUP CreateLeague METHODS
        // ========================================
        private void OpenPopupCreate()
        {
            newLeague = new Models.League();
            showCreateLeague = true;
        }
        private void ClosePopupCreate()
        {
            showCreateLeague = false;
            newLeague = new Models.League();
        }
        private async Task SaveLeague()
        {
            if (string.IsNullOrWhiteSpace(newLeague.Name) || string.IsNullOrWhiteSpace(newLeague.Password))
            {
                // You can add validation message here
                return;
            }
            var league = new CreateOrJoinLeageReq
            {
                UserId = AppState.CurrentUser?.Id ?? 0,
                Token = AppState.CurrentUser?.Token ?? "",
                LeagueName = newLeague.Name,
                LeaguePassword = newLeague.Password
            };
            await _proxy.CreateLeague(league);
            ClosePopupCreate();
        }
        // ========================================
        // POPUP JoinLeague METHODS
        // ========================================
        private void OpenPopupJoin()
        {
            newLeague = new Models.League();
            showCreateLeague = true;
        }
        private void ClosePopupJoin()
        {
            showCreateLeague = false;
            newLeague = new Models.League();
        }
        private async Task SaveJoinLeague()
        {
            if (string.IsNullOrWhiteSpace(newLeague.Name) || string.IsNullOrWhiteSpace(newLeague.Password))
            {
                // You can add validation message here
                return;
            }
            var league = new CreateOrJoinLeageReq
            {
                UserId = AppState.CurrentUser?.Id ?? 0,
                Token = AppState.CurrentUser?.Token ?? "",
                LeagueName = joinLeague.Name,
                LeaguePassword = joinLeague.Password
            };
            //await _proxy.AddGame(game);
            ClosePopupCreate();
        }
    }
}