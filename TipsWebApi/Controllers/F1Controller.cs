using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TipsWebApi.Entities;
using TipsWebApi.Models.F1;

namespace TipsWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class F1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public F1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("GetF1Seasons")]
        public List<Models.F1.F1Season> GetF1Seasons(GetF1SeasonsReq req)
        {
            var season = _context.F1Seasons.OrderByDescending(x => x.Name).ToList();
            var result = new List<Models.F1.F1Season>();
            foreach (var s in season)
            {
                result.Add(new Models.F1.F1Season
                {
                    Id = s.Id,
                    Name = s.Name,
                });
            }
            return result;
        }

        [HttpPost]
        [Route("GetF1Tracks")]
        public List<Models.F1.F1Track> GetF1Tracks(GetF1TracksReq req)
        {
            var tracks = _context.F1Qualifyings
                .Where(x => x.F1SeasonId == req.SeasonId)
                .Select(x => new
                {
                    Track = x.Track,
                    Date = x.QualiDate
                })
                .ToList();

            tracks.AddRange(
                _context.F1Races
                    .Where(x => x.F1SeasonId == req.SeasonId)
                    .Select(x => new
                    {
                        Track = x.Track,
                        Date = x.RaceDate
                    })
                    .ToList()
            ); var result = new List<Models.F1.F1Track>();
            var orderedDistinctTracks = tracks
                .GroupBy(t => t.Track)
                .Select(g => new
                {
                    Track = g.Key,
                    Date = g.Min(x => x.Date)   // earliest date for that track
                })
                .OrderBy(x => x.Date)
                .ToList();
            foreach (var track in orderedDistinctTracks)
            {
                result.Add(new Models.F1.F1Track
                {
                    Name = track.Track,
                });
            }
            return result;
        }

        [HttpPost]
        [Route("GetF1TrackResult")]
        public F1TrackResult GetF1TrackResult(GetF1TrackResultReq req)
        {
            var result = new F1TrackResult
            {
                Qualifying = new Models.F1.F1Qualifying() { QualifyingRows = new List<Models.F1.F1QualifyingRows>() },
                Race = new Models.F1.F1Race() { RaceRows = new List<Models.F1.F1RaceRows>() },
                Standing = new Models.F1.F1Standing() { Rows = new List<Models.F1.F1StandingRow>() }
            };
            var quali = _context.F1Qualifyings.FirstOrDefault(x => x.F1SeasonId == req.SeasonId && x.Track == req.TrackName);
            if (quali != null)
            {
                var qualiRows = _context.F1QualifyingRows.Where(x => x.F1QualifyingId == quali.Id).OrderBy(y => Convert.ToInt32(y.Position)).ToList();
                foreach (var row in qualiRows)
                {
                    var qualiRow = new Models.F1.F1QualifyingRows
                    {
                        Position = row.Position,
                        Driver = row.Driver,
                        Team = row.Team,
                        Best = row.Best,
                        Gap = row.Gap
                    };
                    result.Qualifying.QualifyingRows.Add(qualiRow);
                }
            }
            var race = _context.F1Races.FirstOrDefault(x => x.F1SeasonId == req.SeasonId && x.Track == req.TrackName);
            if (race != null)
            {
                var raceRows = _context.F1RaceRows.Where(x => x.F1RaceId == race.Id).OrderBy(y => Convert.ToInt32(y.Position)).ToList();
                foreach (var row in raceRows)
                {
                    var raceRow = new Models.F1.F1RaceRows
                    {
                        Position = row.Position,
                        Driver = row.Driver,
                        Team = row.Team,
                        Grid = row.Grid,
                        Stops = row.Stops,
                        Best = row.Best,
                        Time = row.Time,
                        Points = row.Points,
                        Penalties = row.Penalties,
                        PenTime = row.PenTime
                    };
                    result.Race.RaceRows.Add(raceRow);
                }
            }
            var standings = _context.F1RaceRows
                .Where(rr => rr.Race.F1SeasonId == req.SeasonId)
                .GroupBy(rr => rr.Driver)
                .Select(g => new
                {
                    Driver = g.Key,
                    TotalPoints = g.Sum(x => Convert.ToInt32(x.Points))
                })
                .OrderByDescending(x => x.TotalPoints)
                .ToList();
            var position = 1;
            foreach (var standingRow in standings)
            {
                var srow = new Models.F1.F1StandingRow
                {
                    Position = position++.ToString(),
                    Driver = standingRow.Driver,
                    Points = standingRow.TotalPoints.ToString()
                };
                result.Standing.Rows.Add(srow); 
            }

            return result;
        }

        //Admin
        [HttpPost]
        [Route("SaveFiles")]
        public void SaveFiles(SaveFilesReq req)
        {
            if (CheckUser(req.UserId, req.Token))
            {
                foreach (var season in req.Seasons)
                {
                    var existingSeason = _context.F1Seasons.FirstOrDefault(s => s.Name == season.Name);
                    if (existingSeason == null)
                    {
                        // Create new season
                        existingSeason = new Entities.F1Season
                        {
                            Name = season.Name
                        };
                        _context.F1Seasons.Add(existingSeason);
                        _context.SaveChanges(); // Save changes to get the Id of the new season
                        existingSeason = _context.F1Seasons.FirstOrDefault(s => s.Name == season.Name); // Retrieve the newly created season with its Id
                    }
                    foreach (var qualifying in season.Qualifyings)
                    {
                        var existingQualifying = _context.F1Qualifyings.FirstOrDefault(q => q.Track == qualifying.Track &&
                                                                                        q.QualiDate == qualifying.QualiDate &&
                                                                                        q.F1SeasonId == existingSeason.Id);
                        if (existingQualifying == null)
                        {
                            // Create new qualifying
                            existingQualifying = new Entities.F1Qualifying
                            {
                                Track = qualifying.Track,
                                QualiDate = qualifying.QualiDate,
                                F1SeasonId = existingSeason.Id
                            };
                            _context.F1Qualifyings.Add(existingQualifying);
                            _context.SaveChanges(); // Save changes to get the Id of the new qualifying
                            existingQualifying = _context.F1Qualifyings.FirstOrDefault(q => q.Track == qualifying.Track &&
                                                                                  q.QualiDate == qualifying.QualiDate &&
                                                                                  q.F1SeasonId == existingSeason.Id); // Retrieve the newly created qualifying with its Id
                        }
                        if (qualifying.QualifyingRows != null)
                        {
                            foreach (var rows in qualifying.QualifyingRows)
                            {
                                var existingQualifyingRow = _context.F1QualifyingRows.FirstOrDefault(q => q.F1QualifyingId == existingQualifying.Id && 
                                                                                                        q.Driver == rows.Driver);
                                if (existingQualifyingRow != null)
                                {
                                    continue;
                                }
                                // Create new qualifying
                                existingQualifyingRow = new Entities.F1QualifyingRows
                                {
                                    F1QualifyingId = existingQualifying.Id,
                                    Position = rows.Position,
                                    Driver = rows.Driver,
                                    Team = rows.Team,
                                    Best = rows.Best,
                                    Gap = rows.Gap
                                };
                                _context.F1QualifyingRows.Add(existingQualifyingRow);
                            }
                        }
                    }
                    foreach (var race in season.Races)
                    {
                        var existingRace = _context.F1Races.FirstOrDefault(q => q.Track == race.Track &&
                                                                                    q.RaceDate == race.RaceDate &&
                                                                                    q.F1SeasonId == existingSeason.Id);
                        if (existingRace == null)
                        {
                            existingRace = new Entities.F1Race
                            {
                                Track = race.Track,
                                RaceDate = race.RaceDate,
                                F1SeasonId = existingSeason.Id
                            };
                            _context.F1Races.Add(existingRace);
                            _context.SaveChanges(); // Save changes to get the Id of the new race
                            existingRace = _context.F1Races.FirstOrDefault(r => r.Track == race.Track &&
                                                                                r.RaceDate == race.RaceDate &&
                                                                                r.F1SeasonId == existingSeason.Id); // Retrieve the newly created race with its Id
                        }

                        if (race.RaceRows != null)
                        {
                            foreach (var raceRow in race.RaceRows)
                            {
                                var existingRaceRow = _context.F1RaceRows.FirstOrDefault(r => r.F1RaceId == existingRace.Id && r.Driver == raceRow.Driver);
                                if (existingRaceRow != null)
                                {
                                    return;
                                }
                                // Create new race
                                existingRaceRow = new Entities.F1RaceRows
                                {
                                    F1RaceId = existingRace.Id,
                                    Position = raceRow.Position,
                                    Driver = raceRow.Driver,
                                    Team = raceRow.Team,
                                    Best = raceRow.Best,
                                    Grid = raceRow.Grid,
                                    Penalties = raceRow.Penalties,
                                    PenTime = raceRow.PenTime,
                                    Points = raceRow.Points,
                                    Stops = raceRow.Stops,
                                    Time = raceRow.Time,
                                };
                                _context.F1RaceRows.Add(existingRaceRow);
                            }
                        }
                    }
                }
                _context.SaveChanges();
            }
        }

        private bool CheckUser(int userId, string token)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                return user.Token == token;
            }
            return false;
        }

    }
}
