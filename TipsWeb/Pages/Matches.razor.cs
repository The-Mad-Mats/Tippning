using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class Matches
    {
        [Inject] public Proxy Proxy { get; set; }
        private User user = new();

        private List<Game> Games { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            user = AppState.CurrentUser;
            if (user != null)
            {
                Games = await Proxy.GetUserGames(new GetDefaultReq { UserId = user.Id, Token = user.Token });
            }

            //Items.Add(new GameUser
            //{
            //    DateTime = DateTime.Now.AddHours(1),
            //    Team1Flag = "images/sweden.png",
            //    Team1 = "Sverige",
            //    Team2Flag = "images/ukraina.png",
            //    Team2 = "Ukraina",
            //    Team1Score = 0,
            //    Team2Score = 0
            //});

            //Items.Add(new GameUser
            //{
            //    DateTime = DateTime.Now.AddDays(1),
            //    Team1Flag = "images/polen.png",
            //    Team1 = "Polen",
            //    Team2Flag = "images/albanien.png",
            //    Team2 = "Albanien",
            //    Team1Score = 0,
            //    Team2Score = 0
            //});
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
            //Items.Add(new GameUser
            //{
            //    Team1Flag = "images/default.png",
            //    Team1 = $"Item {Items.Count + 1}",
            //    Team2Flag = "images/default.png",
            //    Team2 = "New item",
            //    Team1Score = 0,
            //    Team2Score = 0
            //});
        }

        //public class ItemModel
        //{
        //    public DateTime StartTime = DateTime.MinValue;
        //    public string Team1Flag { get; set; } = "";
        //    public string Team1 { get; set; } = "";
        //    public string Team2Flag { get; set; } = "";
        //    public string Team2 { get; set; } = "";
        //    public int Team1Score { get; set; } = 0;
        //    public int Team2Score { get; set; } = 0;
        //}
    }
}
