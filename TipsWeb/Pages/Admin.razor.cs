namespace TipsWeb.Pages
{
    public partial class Admin
    {
        // ========================================
        // FIELDS
        // ========================================
        private List <GameItem> games = new();
        private string calculationResult = string.Empty;
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
                    Name = "Football Match",
                    Category = "Sports",
                    Score = "",
                    Result = "", 
                    IsSelected = false
                },
                new()
                {
                    Id = 2,
                    Name = "Basketball Game",
                    Category = "Sports",
                    Score = "",
                    Result = "",
                    IsSelected = false
                },
                new()
                {
                    Id = 3,
                    Name = "Chess Tournament",
                    Category = "Board Games",
                    Score = "",
                    Result = "",
                    IsSelected = false
                },
                new()
                {   
                    Id = 4,
                    Name = "Tennis Match",
                    Category = "Sports",
                    Score = "", 
                    Result = "",
                    IsSelected = false
                },
                new()
                {
                    Id = 5,
                    Name = "Video Game Contest",
                    Category = "E-Sports",
                    Score = "",
                    Result = "",
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
                if(int.TryParse(game.Score, out int score))
                {
                    totalScore += score;
                }
            }
            calculationResult = $"Selected {selectedGames.Count} game(s).Total Score: {totalScore}";
        }
        private void AddGames()
        {
            var newId = games.Any() ? games.Max(g => g.Id) + 1 : 1;
            games.Add(new GameItem
            {
                Id = newId, 
                Name = $"New Game {newId}", 
                Category = "Uncategorized", 
                Score = "",
                Result = "",
                IsSelected = false
            });
            calculationResult = $"Added new game with ID {newId}";
        }
        // ========================================
        // MODELS
        // ========================================
        public class GameItem
        {
            public int Id {get; set;}
            public string Name {get; set;} = "";
            public string Category {get; set;} = "";
            public string Score {get; set;} = "";
            public string Result {get; set;} = "";
            public bool IsSelected {get; set;}
        }
    }
}
