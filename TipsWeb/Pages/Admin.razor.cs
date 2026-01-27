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
        private List <GameAdmin> games = new();
        private string calculationResult = string.Empty;
        private bool showPopup = false;
        private GameAdmin newGame = new();
        // ========================================
        // LIFECYCLE METHODS
        // ========================================
        protected override async Task OnInitializedAsync()
        {
            await LoadInitialGames();
        }
        // ========================================
        // DATA METHODS
        // ========================================
        private async Task LoadInitialGames()
        {
            games = await _proxy.GetGames(new GetGamesReq { UserId = AppState.CurrentUser.Id, Token = AppState.CurrentUser.Token });
            //games = new List<GameAdmin>
            //{
            //    new()
            //    {
            //        Id = 1,
            //        HomeTeam = "Football Match",
            //        AwayTeam = "Sports",
            //        HomeTeamScore = "",
            //        AwayTeamScore    = "", 
            //        IsSelected = false
            //    },
            //    new()
            //    {
            //        Id = 2,
            //        HomeTeam = "Basketball Game",
            //        AwayTeam = "Sports",
            //        HomeTeamScore = "",
            //        AwayTeamScore = "",
            //        IsSelected = false
            //    },
            //    new()
            //    {
            //        Id = 3,
            //        HomeTeam = "Chess Tournament",
            //        AwayTeam = "Board Games",
            //        HomeTeamScore = "",
            //        AwayTeamScore = "",
            //        IsSelected = false
            //    },
            //    new()
            //    {   
            //        Id = 4,
            //        HomeTeam = "Tennis Match",
            //        AwayTeam = "Sports",
            //        HomeTeamScore = "",
            //        AwayTeamScore = "",
            //        IsSelected = false
            //    },
            //    new()
            //    {
            //        Id = 5,
            //        HomeTeam = "Video Game Contest",
            //        AwayTeam = "E-Sports",
            //        HomeTeamScore = "",
            //        AwayTeamScore = "",
            //        IsSelected = false
            //    }
            //};
        }
        // ========================================
        // EVENT HANDLERS
        // ========================================
        private void DoCalculation()
        {
            var selectedGames = games.Where(g => g.IsSelected).ToList();
            if(!selectedGames.Any())
            {
                calculationResult = "No games selected!";
                return;
            }
            // Example calculation: count selected games and sum scores
            int totalScore = 0;
            foreach(var game in selectedGames)
            {
                totalScore += game.Team1Score ?? 0;
            }
            calculationResult = $"Selected {selectedGames.Count} game(s).Total Score: {totalScore}";
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
