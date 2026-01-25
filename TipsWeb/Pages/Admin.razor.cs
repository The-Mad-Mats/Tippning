namespace TipsWeb.Pages
{
    public partial class Admin
    {
        // ========================================
        // FIELDS
        // ========================================
        private List <GameItem> games = new();
        private string calculationResult = string.Empty;
        private bool showPopup = false;
        private GameItem newGame = new();
        // ========================================
        // LIFECYCLE METHODS
        // ========================================
        protected override void OnInitialized()
        {
            LoadInitialGames();
        }
        // ========================================
        // DATA METHODS
        // ========================================
        private void LoadInitialGames()
        {
            games = new List<GameItem>
            {
                new()
                {
                    Id = 1,
                    HomeTeam = "Football Match",
                    AwayTeam = "Sports",
                    HomeTeamScore = "",
                    AwayTeamScore    = "", 
                    IsSelected = false
                },
                new()
                {
                    Id = 2,
                    HomeTeam = "Basketball Game",
                    AwayTeam = "Sports",
                    HomeTeamScore = "",
                    AwayTeamScore = "",
                    IsSelected = false
                },
                new()
                {
                    Id = 3,
                    HomeTeam = "Chess Tournament",
                    AwayTeam = "Board Games",
                    HomeTeamScore = "",
                    AwayTeamScore = "",
                    IsSelected = false
                },
                new()
                {   
                    Id = 4,
                    HomeTeam = "Tennis Match",
                    AwayTeam = "Sports",
                    HomeTeamScore = "",
                    AwayTeamScore = "",
                    IsSelected = false
                },
                new()
                {
                    Id = 5,
                    HomeTeam = "Video Game Contest",
                    AwayTeam = "E-Sports",
                    HomeTeamScore = "",
                    AwayTeamScore = "",
                    IsSelected = false
                }
            };
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
                if(int.TryParse(game.HomeTeamScore, out int score))
                {
                    totalScore += score;
                }
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
            newGame = new GameItem
            {
                Date = DateTime.Now
            };
            showPopup = true;
        }
        private void ClosePopup()
        {
            showPopup = false;
            newGame = new GameItem();
        }
        private void SaveGame()
        {
            if(string.IsNullOrWhiteSpace(newGame.HomeTeam) || string.IsNullOrWhiteSpace(newGame.AwayTeam))
            {
                // You can add validation message here
                return;
            }
            games.Add(new GameItem
            {
                Id = games.Any() ? games.Max(g =>g.Id) + 1 : 1,
                Date = newGame.Date,
                HomeTeam = newGame.HomeTeam,
                AwayTeam = newGame.AwayTeam
            });
            ClosePopup();
        }

        // ========================================
        // MODELS
        // ========================================
        public class GameItem
        {
            public int Id {get; set;}
            public DateTime Date { get; set; } = DateTime.Now;
            public string HomeTeam { get; set; } = "";
            public string AwayTeam { get; set; } = "";
            public string HomeTeamScore {get; set;} = "";
            public string AwayTeamScore {get; set;} = "";
            public bool IsSelected {get; set;}
        }

    }
}
