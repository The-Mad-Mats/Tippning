using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class Admin
    {
        // ========================================
        // FIELDS
        // ========================================
        [Inject] public Proxy _proxy { get; set; }
        private User user = new();
        private List <GameAdmin> games = new();
        private string calculationResult = string.Empty;
        private bool showPopup = false;
        private GameAdmin newGame = new();
        // ========================================
        // LIFECYCLE METHODS
        // ========================================
        protected override async Task OnInitializedAsync()
        {
            user = AppState.CurrentUser;

            await LoadInitialGames();
        }
        // ========================================
        // DATA METHODS
        // ========================================
        private async Task LoadInitialGames()
        {
            if (user != null)
            {
                games = await _proxy.GetGames(new GetGamesReq { UserId = user.Id, Token = user.Token });
            }
        }
        // ========================================
        // EVENT HANDLERS
        // ========================================
        private async Task DoCalculation()
        {
            var selectedGames = games.Where(x => x.Team1Score != null && x.Team2Score != null).ToList();
            if(!selectedGames.Any())
            {
                calculationResult = "No games selected!";
                return;
            }
            var calcResultReq = new CalcResultReq
            {
                UserId = user.Id,
                Token = user.Token,
                Games = selectedGames
            };
            await _proxy.CalculateResul(calcResultReq);
        }
        private void AddGames()
        { 
            OpenPopup();
            //var newId = games.Any() ? games.Max(g => g.Id) + 1 : 1;
            //games.Add(new GameItem
            //{
            //    Id = newId, 
            //    HomeTeam = $"New Game {newId}", 
            //    AwayTeam = "Uncategorized", 
            //    HomeTeamScore = "",
            //    AwayTeamScore = "",
            //    IsSelected = false
            //});
            //calculationResult = $"Added new game with ID {newId}";
        }
        // ========================================
        // POPUP METHODS
        // ========================================
        private void OpenPopup()
        {
            newGame = new GameAdmin
            {
                DateTime = DateTime.Today.AddHours(15),
            };
            showPopup = true;
        }
        private void ClosePopup()
        {
            showPopup = false;
            newGame = new GameAdmin();
        }
        private async Task SaveGame()
        {
            if(string.IsNullOrWhiteSpace(newGame.Team1) || string.IsNullOrWhiteSpace(newGame.Team2))
            {
                // You can add validation message here
                return;
            }
            var game = new AddGameReq
            {
                UserId = AppState.CurrentUser?.Id ?? 0,
                Token = AppState.CurrentUser?.Token ?? "",
                Date = newGame.DateTime,
                HomeTeam = newGame.Team1,
                AwayTeam = newGame.Team2
            };
            await _proxy.AddGame(game);
            ClosePopup();
            await LoadInitialGames();
        }

        // ========================================
        // MODELS
        // ========================================
        //public class GameItem
        //{
        //    public int Id {get; set;}
        //    public DateTime Date { get; set; } = DateTime.Now;
        //    public string HomeTeam { get; set; } = "";
        //    public string AwayTeam { get; set; } = "";
        //    public string HomeTeamScore {get; set;} = "";
        //    public string AwayTeamScore {get; set;} = "";
        //    public bool IsSelected {get; set;}
        //}

    }
}
