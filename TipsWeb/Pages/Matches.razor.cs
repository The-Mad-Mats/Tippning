using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class Matches
    {
        [Inject] public Proxy Proxy { get; set; }
        private User user = new();
        private List<Game> Games { get; set; } = new();
        private List<Models.Competition> Competitions = new List<Models.Competition> { };
        private int selectedCompetition = 0;
        //private DotNetObjectReference<RankTeam> objRef;


        protected override async Task OnInitializedAsync()
        {
            //objRef = DotNetObjectReference.Create(this);
            if (AppState.CurrentUser != null)
            {
                Competitions = await Proxy.GetCompetitions(new GetDefaultReq
                {
                    UserId = AppState.CurrentUser.Id,
                    Token = AppState.CurrentUser.Token
                });

                selectedCompetition = Competitions.FirstOrDefault()?.Id ?? 0;
                await OnCompetitionChanged();
            }
        }

        private async Task OnCompetitionChanged()
        {
            if (selectedCompetition == 0)
            {
                Competitions = new List<Models.Competition> { };
            }
            else
            {
                user = AppState.CurrentUser;
                if (user != null)
                {
                    Games = await Proxy.GetUserGames(new GetDefaultReq 
                    { 
                        UserId = user.Id, 
                        Token = user.Token, 
                        CompetitionId = selectedCompetition 
                    });
                }
            }
        }

        private void Spara()
        {
            var saveGamesReq = new SaveGamesReq
            {
                UserId = user.Id,
                Token = user.Token,
                Games = new List<SaveGame>()
            };
            foreach(var game in Games)
            {
                if (game.Team1Score != null && game.Team2Score != null)
                {
                    var gameResult = new SaveGame
                    {
                        GameId = game.Id,
                        Team1Score = game.Team1Score,
                        Team2Score = game.Team2Score
                    };
                    saveGamesReq.Games.Add(gameResult);
                }
            }
            Proxy.SaveGames(saveGamesReq);
        }
    }
}
