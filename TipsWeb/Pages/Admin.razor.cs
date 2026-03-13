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
        private List <Game> games = new();
        private string calculationResult = string.Empty;
        private bool showPopup = false;
        private Game newGame = new();
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
        }
        // ========================================
        // POPUP METHODS
        // ========================================
        private void OpenPopup()
        {
            newGame = new Game
            {
                GameTime = DateTime.Today.AddHours(15),
            };
            showPopup = true;
        }
        private void ClosePopup()
        {
            showPopup = false;
            newGame = new Game();
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
                Date = newGame.GameTime,
                HomeTeam = newGame.Team1,
                AwayTeam = newGame.Team2
            };
            await _proxy.AddGame(game);
            ClosePopup();
            await LoadInitialGames();
        }
    }
}
