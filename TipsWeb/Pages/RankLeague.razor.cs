using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class RankLeague
    {
        [Inject] public Proxy Proxy { get; set; }
        private LeagueResult LeagueResult = new();
        private Models.RankLeague rankLeague = new();
        private List<Models.League> Leagues = new List<Models.League> { };
        private int selectedLeague = 0;
        private List<Models.RankCompetition> Competitions = new List<Models.RankCompetition> { };
        private int selectedCompetition = 0;
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
                Competitions = await Proxy.GetRankCompetitions(new GetDefaultReq { UserId = user.Id, Token = user.Token });
                selectedCompetition = Competitions.FirstOrDefault()?.Id ?? 0;
                await OnCompetitionChanged();

            }
        }
        private async Task OnCompetitionChanged()
        {
            if (selectedCompetition == 0)
            {
                Competitions = new List<Models.RankCompetition> { };
            }
            else
            {
                Leagues = await Proxy.GetRankLeagues(new GetDefaultReq { UserId = user.Id, Token = user.Token, CompetitionId = selectedCompetition });
                selectedLeague = Leagues.FirstOrDefault()?.Id ?? 0;
                await OnLeagueChanged();
            }
        }

        private async Task OnLeagueChanged()
        {
            if (selectedLeague == 0)
            {
                rankLeague = new Models.RankLeague();
            }
            else
            {
                var leagueId = Leagues.FirstOrDefault(l => l.Id == selectedLeague)?.Id ?? 0;
                rankLeague = await Proxy.GetRankLeague(new GetLeagueReq { LeagueId = leagueId, UserId = user.Id, Token = user.Token, CompetitionId = selectedCompetition });
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
        private async Task ClosePopupCreate()
        {
            showCreateLeague = false;
            newLeague = new Models.League();
            await OnCompetitionChanged();
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
                LeaguePassword = newLeague.Password,
                CompetitionId = selectedCompetition
            };
            await Proxy.CreateRankLeague(league);
            await ClosePopupCreate();
        }
        // ========================================
        // POPUP JoinLeague METHODS
        // ========================================
        private void OpenPopupJoin()
        {
            newLeague = new Models.League();
            showJoinLeague = true;
        }
        private async Task ClosePopupJoin()
        {
            showJoinLeague = false;
            joinLeague = new Models.League();
            await OnCompetitionChanged();
        }
        private async Task SaveJoinLeague()
        {
            if (string.IsNullOrWhiteSpace(joinLeague.Name) || string.IsNullOrWhiteSpace(joinLeague.Password))
            {
                // You can add validation message here
                return;
            }
            var league = new CreateOrJoinLeageReq
            {
                UserId = AppState.CurrentUser?.Id ?? 0,
                Token = AppState.CurrentUser?.Token ?? "",
                LeagueName = joinLeague.Name,
                LeaguePassword = joinLeague.Password,
                CompetitionId = selectedCompetition
            };
            await Proxy.JoinRankLeague(league);
            await ClosePopupJoin();
        }
    }
}