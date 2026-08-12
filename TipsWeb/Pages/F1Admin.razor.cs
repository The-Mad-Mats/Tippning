using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.PortableExecutable;
using TipsWeb.Models.F1;

namespace TipsWeb.Pages
{
    public partial class F1Admin
    {
        [Inject] public Proxy _proxy { get; set; }
        private List<IBrowserFile> selectedFiles = new();
        private string seasonText = "";

        private async Task HandleSelectedFiles(InputFileChangeEventArgs e)
        {
            var saveFilesReq = new SaveFilesReq
            {
                UserId = AppState.CurrentUser.Id,
                Token = AppState.CurrentUser.Token,
                Seasons = new List<F1Season>()
            };
            var season = new F1Season
            {
                Name = seasonText,
                Qualifyings = new List<F1Qualifying>(),
                Races = new List<F1Race>()
            };
            saveFilesReq.Seasons.Add(season);
            foreach (var file in e.GetMultipleFiles(100))
            {
                using var stream = file.OpenReadStream(maxAllowedSize: 10_000_000); // 10 MB
                if (file.Name.Contains("Quali"))
                {
                    var date = file.Name.Split()[1];
                    var year = date.Substring(0, 4);
                    var month = date.Substring(5, 2);
                    var day = date.Substring(8, 2);
                    string hour;
                    string minute;
                    if (date.Length < 16)
                    {
                        hour = "00";
                        minute = "00";
                    }
                    else
                    {
                        hour = date.Substring(11, 2);
                        minute = date.Substring(14, 2);
                    }
                    var qualifying = new F1Qualifying
                    {
                        QualiDate = DateTime.Parse($"{year}-{month}-{day} {hour}:{minute}"),
                        Track = file.Name.Split()[0],
                        QualifyingRows = new List<F1QualifyingRows>()
                    };
                    using (StreamReader reader = new StreamReader(stream))
                    {

                        while (true)
                        {
                            string line = await reader.ReadLineAsync();
                            if (line == null)
                                break;
                            var lines = line.Split(',').ToList();
                            if (lines[0] == "Position")
                            {
                            }
                            else if (lines[0].StartsWith("Gap"))
                            {
                            }
                            else
                            {
                                var row = new F1QualifyingRows();
                                row.Position = lines[0];
                                row.Driver = lines[1];
                                row.Team = lines[2];
                                row.Best = lines[3];
                                row.Gap = lines[4];
                                qualifying.QualifyingRows.Add(row);
                            }

                        }
                    }
                    season.Qualifyings.Add(qualifying);
                }
                else if (file.Name.Contains("Race"))
                {
                    var date = file.Name.Split()[1];
                    var year = date.Substring(0, 4);
                    var month = date.Substring(5, 2);
                    var day = date.Substring(8, 2);
                    string hour;
                    string minute;
                    if (date.Length < 16)
                    {
                        hour = "00";
                        minute = "00";
                    }
                    else
                    {
                        hour = date.Substring(11, 2);
                        minute = date.Substring(14, 2);
                    }
                    var secondRace = file.Name.Split()[2].Contains("2");
                    var trackName = secondRace ? file.Name.Split()[0] + " 2" : file.Name.Split()[0];
                    var race = new F1Race
                    {
                        RaceDate = DateTime.Parse($"{year}-{month}-{day} {hour}:{minute}"),
                        Track = trackName,
                        RaceRows = new List<F1RaceRows>()
                    };
                    using (StreamReader reader = new StreamReader(stream))
                    {

                        while (true)
                        {
                            string line = await reader.ReadLineAsync();
                            if (line == null)
                                break;
                            var lines = line.Split(',').ToList();
                            if (lines[0] == "Position")
                            {
                            }
                            else if (lines[0].StartsWith("Gap"))
                            {
                            }
                            else
                            {
                                var row = new F1RaceRows();
                                row.Position = lines[0];
                                row.Driver = lines[1];
                                row.Team = lines[2];
                                row.Grid = lines[3];
                                row.Stops = lines[4];
                                row.Best = lines[5];
                                row.Time = lines[6];
                                row.Points = lines[7];
                                row.Penalties = lines[8];
                                row.PenTime = lines[9];
                                race.RaceRows.Add(row);
                            }

                        }
                    }
                    season.Races.Add(race);
                }
            }
            //saveFilesReq.Seasons.Add(season);
            _proxy.SaveFile(saveFilesReq);
        }
    }
}
