using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TipsWebApi.Entities;
using TipsWebApi.Models;

namespace TipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("GetCompetitions")]
    public List<Models.Competition> GetCompetitions(GetDefaultReq req)
    {
        try
        {
            if (CheckUser(req.UserId, req.Token))
            {
                var myCompetitions = _context.Competitions.OrderByDescending(y => y.Deadline).ToList();

                var competitions = new List<Models.Competition>();
                foreach (var comp in myCompetitions)
                {
                    var compDto = new Models.Competition
                    {
                        Id = comp.Id,
                        Name = comp.Name,
                        Deadline = comp.Deadline
                    };
                    competitions.Add(compDto);
                }
                return competitions;
            }
            return new List<Models.Competition>();
        }
        catch (Exception)
        {
            return new List<Models.Competition>();
        }
    }

    [HttpPost]
    [Route("AddGame")]
    public Models.Game AddGame(AddGameReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {

            try
            {
                var game = new Entities.Game()
                {
                    CompetitionId = req.CompetitionId,
                    GameTime = req.Date,
                    Team1 = req.HomeTeam,
                    Team2 = req.AwayTeam,
                    Team1Score = null,
                    Team2Score = null
                };
                _context.Games.Add(game);
                var userId = _context.SaveChanges();
                var addedGame = _context.Games.AsNoTracking().FirstOrDefault(x => x.Id == game.Id);
                var newGame = new Models.Game()
                {
                    Id = addedGame.Id,
                    CompetitionId = addedGame.CompetitionId,
                    GameTime = addedGame.GameTime,
                    Team1 = addedGame.Team1,
                    Team2 = addedGame.Team2,
                    Team1Score = addedGame.Team1Score,
                    Team2Score = addedGame.Team2Score,
                    Team1Flag = $"images/{addedGame.Team1}.png",
                    Team2Flag = $"images/{addedGame.Team2}.png"
                };
                return newGame;
            }
            catch (Exception ex)
            {
                return new Models.Game();
            }
        }
        return new Models.Game();
    }

    [HttpPost]
    [Route("GetGames")]
    public List<Models.Game> GetGames(GetGamesReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            try
            {
                var games = new List<Models.Game>();
                games = _context.Games.Where(x => x.CompetitionId == req.CompetitionId).AsNoTracking().Select(g => new Models.Game()
                {
                    Id = g.Id,
                    GameTime = g.GameTime,
                    Team1 = g.Team1,
                    Team2 = g.Team2,
                    Team1Score = g.Team1Score,
                    Team2Score = g.Team2Score,
                    Team1Flag = $"images/{g.Team1}.png",
                    Team2Flag = $"images/{g.Team2}.png"
                }).ToList();
                return games;
            }
            catch (Exception ex)
            {
                return new List<Models.Game>();
            }
        }
        return new List<Models.Game>();
    }

    [HttpPost]
    [Route("CalculateResult")]
    public bool CalculateResult(CalcResultReq req)
    { 
        if (CheckUser(req.UserId, req.Token))
        {
            try
            {
                foreach (var gameReq in req.Games)
                {
                    var game = _context.Games.Include(y => y.UserGames).FirstOrDefault(x => x.Id == gameReq.Id);
                    if (game != null)
                    {
                        if (game.Team1Score == gameReq.Team1Score && game.Team2Score == gameReq.Team2Score)
                        {
                            continue; // No change in result, skip calculation
                        }
                        if(game.Team1Score != null && game.Team2Score != null)
                        {
                            // Resultatet har ändrats, återställ poäng för alla användare som tippat på detta spel
                            if (game.UserGames != null)
                            {
                                foreach (var userGame in game.UserGames)
                                {
                                    if (userGame.Points != null)
                                    {
                                        var currentUser = _context.Users.Include(y => y.UserCompetitions.Where(z => z.CompetitionId == req.CompetitionId)).First(x => x.Id == userGame.UserId);
                                        currentUser.UserCompetitions!.First().Points -= userGame.Points.Value;
                                        userGame.Points = 0; // Reset points before recalculation
                                    }
                                }
                            }
                        }
                        game.Team1Score = gameReq.Team1Score;
                        game.Team2Score = gameReq.Team2Score;
                        if (game.UserGames != null)
                        {
                            foreach (var userGame in game.UserGames)
                            {
                                int points = 0;
                                if (userGame.Team1Score == gameReq.Team1Score)
                                {
                                    points += 1; // Team1 correct score
                                }
                                if (userGame.Team2Score == gameReq.Team2Score)
                                {
                                    points += 1; //Team2 correct score
                                }
                                if ((userGame.Team1Score - userGame.Team2Score) == (gameReq.Team1Score - gameReq.Team2Score))
                                {
                                    points += 1; // Correct goal difference
                                }
                                if ((userGame.Team1Score > userGame.Team2Score && gameReq.Team1Score > gameReq.Team2Score) ||
                                         (userGame.Team1Score < userGame.Team2Score && gameReq.Team1Score < gameReq.Team2Score) ||
                                         (userGame.Team1Score == userGame.Team2Score && gameReq.Team1Score == gameReq.Team2Score))
                                {
                                    points += 1; // Correct outcome
                                }
                                if (points == 4)
                                {
                                    points += 2; // Bonus for perfect tip
                                }
                                userGame.Points = points;
                                var currentUser = _context.Users.Include(y => y.UserCompetitions.Where(z => z.CompetitionId == req.CompetitionId)).First(x => x.Id == userGame.UserId);
                                currentUser.UserCompetitions!.First().Points += points;
                            }
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //return ex.Message + " " + ex.InnerException?.Message;
                return false;
            }
        }
        return false;
    }

    [HttpPost]
    [Route("GetRankCompetitions")]
    public List<Models.RankCompetition> GetRankCompetitions(GetDefaultReq req)
    {
        try
        {
            if (CheckUser(req.UserId, req.Token))
            {
                var myCompetitions = _context.RankCompetitions.OrderByDescending(y => y.Deadline).ToList();

                var competitions = new List<Models.RankCompetition>();
                foreach (var comp in myCompetitions)
                {
                    var compDto = new Models.RankCompetition
                    {
                        Id = comp.Id,
                        Name = comp.Name,
                        Deadline = comp.Deadline
                    };
                    competitions.Add(compDto);
                }
                return competitions;
            }
            return new List<Models.RankCompetition>();
        }
        catch (Exception)
        {
            return new List<Models.RankCompetition>();
        }
    }

    [HttpPost]
    [Route("GetTeamRank")]
    public List<Models.TeamRank> GetTeamRank(GetDefaultReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            var teamranks = new List<Models.TeamRank>();
            var allGames = _context.TeamRanks.Where(x => x.RankCompetitionId == req.CompetitionId);
            foreach (var myGame in allGames)
            {
                teamranks.Add(new Models.TeamRank
                {
                    Id = 0,
                    Team = myGame.Team,
                    TeamId = myGame.Id,
                    Rank = myGame.Rank
                });
            }
            return teamranks;
        }
        return new List<Models.TeamRank>();
    }

    [HttpPost]
    [Route("SaveCurrentRank")]
    public bool SaveCurrentRank(SaveTeamRankReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            try
            {
                foreach (var teamReq in req.TeamRanks)
                {
                    var team = _context.TeamRanks.FirstOrDefault(x => x.Id == teamReq.TeamId);
                    if (team != null)
                    {
                        team.Rank = teamReq.Rank;
                        var userRanks = _context.UserRanks.Where(x => x.TeamRankId == teamReq.TeamId).ToList();
                        foreach (var userRank in userRanks)
                        {
                            userRank.Points = Math.Abs(teamReq.Rank - userRank.Rank);
                        }
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        return false;
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

