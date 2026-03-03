using Microsoft.AspNetCore.Components;
using TipsWeb.Models;

namespace TipsWeb.Pages
{

    public partial class Test
    {
        [Inject] public Proxy Proxy { get; set; }

        private Models.RankLeague rankLeague = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var req = new GetLeagueReq 
            { 
                LeagueId = 1,
                UserId = 2
            };
            rankLeague = await Proxy.GetRankLeague(req);
            // Sample data - replace with your actual data loading
            //rankLeague = new Models.RankLeague
            //{
            //    TeamRanks = new List<TeamRank>
            //{
            //    new TeamRank { Id = 1, Team = "Arsenal", TeamId = 1, Rank = 1 },
            //    new TeamRank { Id = 2, Team = "Liverpool", TeamId = 2, Rank = 2 },
            //    new TeamRank { Id = 3, Team = "Man City", TeamId = 3, Rank = 3 },
            //    new TeamRank { Id = 4, Team = "Chelsea", TeamId = 4, Rank = 4 }
            //},
            //    UserTeamRanks = new List<UserTeamRank>
            //{
            //    new UserTeamRank
            //    {
            //        UserName = "John",
            //        Team = "John's Picks",
            //        UserRanks = new List<UserRank>
            //        {
            //            new UserRank { TeamId = 1, Rank = 1, Points = 10 },
            //            new UserRank { TeamId = 2, Rank = 2, Points = 8 },
            //            new UserRank { TeamId = 3, Rank = 3, Points = 6 },
            //            new UserRank { TeamId = 4, Rank = 4, Points = 4 }
            //        }
            //    },
            //    new UserTeamRank
            //    {
            //        UserName = "Sarah",
            //        Team = "Sarah's Team",
            //        UserRanks = new List<UserRank>
            //        {
            //            new UserRank { TeamId = 1, Rank = 2, Points = 8 },
            //            new UserRank { TeamId = 2, Rank = 1, Points = 10 },
            //            new UserRank { TeamId = 3, Rank = 4, Points = 4 },
            //            new UserRank { TeamId = 4, Rank = 3, Points = 6 }
            //        }
            //    },
            //    new UserTeamRank
            //    {
            //        UserName = "Mike",
            //        Team = "Mike's Squad",
            //        UserRanks = new List<UserRank>
            //        {
            //            new UserRank { TeamId = 1, Rank = 3, Points = 6 },
            //            new UserRank { TeamId = 2, Rank = 4, Points = 4 },
            //            new UserRank { TeamId = 3, Rank = 1, Points = 10 },
            //            new UserRank { TeamId = 4, Rank = 2, Points = 8 }
            //        }
            //    }
            //}
            //};
        }

    }

}