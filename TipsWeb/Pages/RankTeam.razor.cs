using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class RankTeam : IDisposable
    {
        private DateTime StartTid = new DateTime(2026,3,6,2,30,0);
        private DateTime CurrentTimeStamp = DateTime.Now;
        [Inject] IJSRuntime JS { get; set; }
        [Inject] public Proxy Proxy { get; set; }
        [Inject] public AppState AppState { get; set; }

        private List<TeamRank> teams = new();
        private string message = string.Empty;
        private DotNetObjectReference<RankTeam> objRef;

        protected override async Task OnInitializedAsync()
        {
            objRef = DotNetObjectReference.Create(this);
            await LoadTeams();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Always reinitialize after every render to keep event listeners working
            await JS.InvokeVoidAsync("initializeDragDrop", objRef);
        }

        private async Task LoadTeams()
        {
            if (AppState.CurrentUser != null)
            {
                teams = await Proxy.GetUserRanks(new GetDefaultReq
                {
                    UserId = AppState.CurrentUser.Id,
                    Token = AppState.CurrentUser.Token
                });
            }

            teams = teams.OrderBy(t => t.Rank).ToList();
        }

        [JSInvokable]
        public void MoveTeam(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex || fromIndex < 0 || toIndex < 0)
                return;

            var team = teams[fromIndex];
            teams.RemoveAt(fromIndex);
            teams.Insert(toIndex, team);

            message = string.Empty;
            StateHasChanged();
        }

        private async Task ResetRanking()
        {
            await LoadTeams();
            message = "Ranking reset to default order";
            StateHasChanged();
        }

        private async Task SaveRanking()
        {
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].Rank = i + 1;
            }
            var saveUserRank = new SaveUserRankReq
            {
                UserId = AppState.CurrentUser.Id,
                Token = AppState.CurrentUser.Token,
                TeamRanks = teams.Select(t => new TeamRank
                {
                    Id = t.Id,
                    Team = t.Team,
                    TeamId = t.TeamId,
                    Rank = t.Rank
                }).ToList()
            };

            await Proxy.SaveUserRanks(saveUserRank);
            message = "Tips sparat!";
        }

        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}