using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{
    public partial class League
    {
        [Inject] public Proxy _proxy { get; set; }
        private List<LeagueRow> products = new();
        private List<LeagueRow> filteredProducts = new();
        private List<Models.League> Leagues = new List<Models.League> { };
        private string selectedCategory = "";
        private User user = new();
        protected override async Task OnInitializedAsync()
        {
            user = AppState.SelectedProduct;
            Leagues = await _proxy.GetUserleague(new GetUserLeagueReq { UserId = user.Id, Token = user.Token });
            // Initialize sample data
            //products = new List<LeagueRow>
            //{
            //    new LeagueRow
            //    {
            //        Position = 1,
            //        Team = "FC Jonathanós",
            //        Liga = "UFL",
            //        Points = 100,
            //        Owner = "Jonathan Eklund"
            //    },
            //    new LeagueRow
            //    {
            //        Position = 2,
            //        Team = "Matts Flawless",
            //        Liga = "UFL",
            //        Points = 3,
            //        Owner = "Mats Eklund"
            //    },
            //    new LeagueRow
            //    {
            //        Position = 3,
            //        Team = "Gelbander",
            //        Liga = "UFL",
            //        Points = 20,
            //        Owner = "Gustaf"
            //    },
            //    new LeagueRow
            //    {
            //        Position = 4,
            //        Team = "FC Desk Lamp",
            //        Liga = "UFL",
            //        Points = 35,
            //        Owner = "Me"
            //    },
            //    new LeagueRow
            //    {
            //        Position = 5,
            //        Team = "6-7",
            //        Liga = "UFL",
            //        Points = 100,
            //        Owner = "Ungdomar"
            //    },
            //    new LeagueRow
            //    {
            //        Position = 6,
            //        Team = "hej",
            //        Liga = "UFL",
            //        Points = 75,
            //        Owner = "hej hej"
            //    },
            //};
            //// Get unique categories
            //categories = products.Select(p => p.Liga).Distinct().OrderBy(c => c).ToList();
            //// Initially show all products
            //filteredProducts = products;
        }
        private void OnCategoryChanged()
        {
            if(string.IsNullOrEmpty(selectedCategory))
            {
                filteredProducts = products;
            }
            else
            {
                filteredProducts = products.Where(p => p.Liga == selectedCategory).ToList();
            }
        }
        public class LeagueRow
        {
            public int Position {get; set; }
            public string Team {get; set; } = string.Empty;
            public string Liga {get; set; } = string.Empty;
            public string Owner { get; set; } 
            public int Points { get; set; }
            
        }
    }
}