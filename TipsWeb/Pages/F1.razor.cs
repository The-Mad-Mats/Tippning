using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using TipsWeb.Models;
using TipsWeb.Models.F1;

namespace TipsWeb.Pages
{
    public partial class F1
    {
        [Inject] public Proxy _proxy { get; set; }

        // -------------------------
        // DROPDOWNS
        // -------------------------
        private int selectedSeason = 0;
        private string selectedTrack = "";

        private List<F1Season> Seasons = new();
        private List<F1Track> Tracks = new();

        private F1TrackResult TrackResult;


        protected override async Task OnInitializedAsync()
        {
            if (AppState.CurrentUser != null)
            {
                Seasons = await _proxy.GetF1Seasons(new GetF1SeasonsReq { UserId = AppState.CurrentUser.Id, Token = AppState.CurrentUser.Token });
                selectedSeason = Seasons.FirstOrDefault()?.Id ?? 0;
                await OnSeasonChanged();

            }
        }

        private async Task OnSeasonChanged()
        {
            if (selectedSeason == 0)
            {
                Seasons = new List<F1Season> { };
            }
            else
            {
                if (AppState.CurrentUser != null)
                {
                    Tracks = await _proxy.GetF1Tracks(new GetF1TracksReq { UserId = AppState.CurrentUser.Id, Token = AppState.CurrentUser.Token, SeasonId = selectedSeason });
                    selectedTrack = Tracks.FirstOrDefault().Name;
                    await OnTrackChanged();
                }
            }
        }

        private async Task OnTrackChanged()
        {
            TrackResult = await _proxy.GetF1TrackResult(new GetF1TrackResultReq
            {
                UserId = AppState.CurrentUser.Id,
                Token = AppState.CurrentUser.Token,
                SeasonId = selectedSeason,
                TrackName = selectedTrack
            });
        }

        // -------------------------
        // TABS
        // -------------------------
        private string activeTab = "qual";

        private void SetTab(string tab)
        {
            activeTab = tab;
        }

        // -------------------------
        // DATA MODELS
        // -------------------------
    //    public class QualItem
    //    {
    //        public int Position { get; set; }
    //        public string Driver { get; set; }
    //        public string Time { get; set; }
    //    }

    //    public class RaceItem
    //    {
    //        public int Position { get; set; }
    //        public string Driver { get; set; }
    //        public string Time { get; set; }
    //        public int Grid { get; set; }
    //        public int Stops { get; set; }
    //        public string Penalty { get; set; }
    //        public bool Expanded { get; set; }
    //    }

    //    public class StandItem
    //    {
    //        public int Position { get; set; }
    //        public string Driver { get; set; }
    //        public int Points { get; set; }
    //    }

    //    // -------------------------
    //    // SAMPLE DATA
    //    // -------------------------
    //    private List<QualItem> Qualification = new()
    //{
    //    new() { Position = 1, Driver = "Verstappen", Time = "1:23.456" },
    //    new() { Position = 2, Driver = "Leclerc", Time = "1:23.789" },
    //    new() { Position = 3, Driver = "Norris", Time = "1:24.012" }
    //};

    //    private List<RaceItem> Race = new()
    //{
    //    new() { Position = 1, Driver = "Verstappen", Time = "1:32:10", Grid = 1, Stops = 2, Penalty = "None" },
    //    new() { Position = 2, Driver = "Norris", Time = "1:32:44", Grid = 3, Stops = 2, Penalty = "None" },
    //    new() { Position = 3, Driver = "Leclerc", Time = "1:33:01", Grid = 2, Stops = 3, Penalty = "5s" }
    //};

    //    private List<StandItem> Standings = new()
    //{
    //    new() { Position = 1, Driver = "Verstappen", Points = 350 },
    //    new() { Position = 2, Driver = "Norris", Points = 290 },
    //    new() { Position = 3, Driver = "Leclerc", Points = 275 }
    //};

        private void ToggleRaceExpand(F1RaceRows item)
        {
            item.Expanded = !item.Expanded;
        }
    }
}
