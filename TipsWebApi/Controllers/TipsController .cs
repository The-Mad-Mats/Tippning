using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TipsWebApi.Entities;
using TipsWebApi.Models;

namespace TipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TipsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TipsController(ApplicationDbContext context)
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
                var myOngoingCompetitionIds = _context.UserLeagues.Include(z => z.League).Where(x => x.UserId == req.UserId).Select(x => x.League!.CompetitionId).ToList();
                var myCompetitions = _context.Competitions.Where(x => x.Deadline > DateTime.Now || myOngoingCompetitionIds.Contains(x.Id)).OrderByDescending(y => y.Deadline).ToList();

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
    [Route("GetUserLeagues")]
    public List<Models.League> GetUserLeagues(GetDefaultReq req)
    {
        if (CheckUser(req.UserId,req.Token))
        {
            var leagues = new List<Models.League>();
            var userleagues = _context.UserLeagues.Include(y => y.League).Where(x => x.UserId == req.UserId && x.League!.CompetitionId == req.CompetitionId).ToList();
            foreach (var ul in userleagues)
            {
                if (ul.League != null)
                {
                    var leagueDto = new Models.League
                    {
                        Id = ul.League.Id,
                        Name = ul.League.Name,
                    };
                    leagues.Add(leagueDto);
                }
            }
            var worldLeagueDto = new Models.League
            {
                Id = 0,
                Name = "Världen",
            };
            leagues.Add(worldLeagueDto);
            return leagues;
        }
        return new List<Models.League>();
    }

    [HttpPost]
    [Route("GetUserGames")]
    public List<Models.Game> GetUserGames(GetDefaultReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            var games = new List<Models.Game>();
            var allGames = _context.Games.Where(x => x.CompetitionId == req.CompetitionId);
            var myGames = _context.UserGames.Include(y => y.Game).Where(x => x.UserId == req.UserId && x.Game!.CompetitionId == req.CompetitionId).ToList();
            foreach (var myGame in myGames)
            {
                allGames.Single(x => x.Id == myGame.GameId).UserGames!.Add(myGame);
            }
            foreach (var game in allGames)
            {
                games.Add(new Models.Game
                {
                    Id = game.Id,
                    GameTime = game.GameTime,
                    Team1 = game.Team1,
                    Team2 = game.Team2,
                    Team1Flag = $"images/{game  .Team1}.png",
                    Team2Flag = $"images/{game.Team2}.png",
                    Team1Result = game.Team1Score,
                    Team2Result = game.Team2Score,
                    Team1Score = game.UserGames?.FirstOrDefault(x => x.GameId == game.Id)?.Team1Score,
                    Team2Score = game.UserGames?.FirstOrDefault(x => x.GameId == game.Id)?.Team2Score,
                });
            }
            return games;
        }
        return new List<Models.Game>();
    }

    [HttpPost]
    [Route("GetLeague")]
    public LeagueResult GetLeague(GetLeagueReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            var leagueMembers = new List<int>();
            var leagueResult = new LeagueResult();
            leagueResult.Rows = new List<LeagueRow>();
            if (req.LeagueId == 0)
            {
                //LeaguId 0 är världsligan. Hämta de 10 bästa spelarna i världen och deras poäng, samt den aktuella användarens position och poäng
                var userComps = _context.UserCompetitions.Include(x => x.User).Where(y => y.CompetitionId == req.CompetitionId).OrderByDescending(x => x.Points).Take(10).ToList();
                var position = 1;
                foreach (var userComp in userComps)
                {
                    var leagueDto = new LeagueRow
                    {
                        Position = position++,
                        UserName = userComp.User.UserName,
                        Team = userComp.User.Team,
                        Points = userComp.Points
                    };
                    leagueResult.Rows.Add(leagueDto);
                    leagueMembers.Add(userComp.User.Id);
                }
                if(leagueMembers.Contains(req.UserId) == false)
                {
                    //Aktuell användare är inte i topp 10, hämta dennes position och lägg till i resultatet
                    var index = userComps.FindIndex(x => x.Id == req.UserId);
                    var leagueDto = new LeagueRow
                    {
                        Position = index++,
                        UserName = userComps.First(x => x.Id == req.UserId).User.UserName,
                        Team = userComps.First(x => x.Id == req.UserId).User.Team,
                        Points = userComps.First(x => x.Id == req.UserId).Points
                    };
                }
            }
            else
            {
                var userleagues = _context.UserLeagues.Include(y => y.User).Where(x => x.LeagueId == req.LeagueId).ToList();
                leagueMembers = userleagues.Select(x => x.UserId).ToList();
                var userComps = _context.UserCompetitions.Include(y => y.User).Where(x => x.CompetitionId == req.CompetitionId && leagueMembers.Contains(x.UserId)).ToList();
                var position = 1;
                foreach (var ul in userComps.OrderByDescending(x => x.Points))
                {
                    if (ul.User != null)
                    {
                        var leagueDto = new LeagueRow
                        {
                            Position = position++,
                            UserName = ul.User.UserName,
                            Team = ul.User.Team,
                            Points = ul.Points
                        };
                        leagueResult.Rows.Add(leagueDto);
                    }
                }
                leagueResult.Matches = new List<Match>();
                var games = _context.Games.Include(x => x.UserGames).ThenInclude(y => y.User).Where(z => z.CompetitionId == req.CompetitionId).ToList();
                foreach (var game in games.Where(x => x.GameTime < DateTime.Now))
                {
                    var match = new Match
                    {
                        GameTime = game.GameTime,
                        Team1 = game.Team1,
                        Team2 = game.Team2,
                        Team1Flag = $"images/{game.Team1}.png",
                        Team2Flag = $"images/{game.Team2}.png",
                        Team1Score = game.Team1Score,
                        Team2Score = game.Team2Score,
                        UserMatches = new List<UserMatch>()
                    };
                    foreach (var userGame in game.UserGames.Where(x => leagueMembers.Contains(x.UserId)))
                    {
                        var userMatch = new UserMatch
                        {
                            Team = userGame.User.Team,
                            Owner = userGame.User.UserName,
                            Team1Score = userGame.Team1Score,
                            Team2Score = userGame.Team2Score,
                            Points = userGame.Points,
                        };
                        match.UserMatches.Add(userMatch);
                    }
                    leagueResult.Matches.Add(match);
                }
            }
            return leagueResult;
        }
        return new LeagueResult();
    }

    [HttpPost]
    [Route("SaveGames")]
    public bool SaveGames(SaveGamesReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            try
            {
                foreach (var gameReq in req.Games)
                {
                    var userGame = _context.UserGames.FirstOrDefault(x => x.UserId == req.UserId && x.GameId == gameReq.GameId);
                    if (userGame != null)
                    {
                        userGame.Team1Score = gameReq.Team1Score;
                        userGame.Team2Score = gameReq.Team2Score;
                    }
                    else
                    {
                        userGame = new Entities.UserGame()
                        {
                            UserId = req.UserId,
                            GameId = gameReq.GameId,
                            Team1Score = gameReq.Team1Score,
                            Team2Score = gameReq.Team2Score,
                            Points = 0
                        };
                        _context.UserGames.Add(userGame);
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

    [HttpPost]
    [Route("CreateLeague")]
    public bool CreateLeague(CreateOrJoinLeageReq req)
    {
        try
        {
            if (CheckUser(req.UserId, req.Token))
            {

                var league = new Entities.League()
                {
                    Name = req.LeagueName,
                    Password = req.LeaguePassword,
                    CompetitionId = req.CompetitionId
                };
                _context.Leagues.Add(league);
                _context.SaveChanges();
                var userleague = new Entities.UserLeague()
                {
                    UserId = req.UserId,
                    LeagueId = league.Id
                };
                _context.UserLeagues.Add(userleague);
                _context.SaveChanges();
                var ucompetition = _context.UserCompetitions.FirstOrDefault(x => x.UserId == req.UserId && x.CompetitionId == req.CompetitionId);
                if (ucompetition == null)
                {
                    var usercompetition = new Entities.UserCompetition()
                    {
                        UserId = req.UserId,
                        CompetitionId = req.CompetitionId,
                        Points = 0
                    };
                    _context.UserCompetitions.Add(usercompetition);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    [HttpPost]
    [Route("JoinLeague")]
    public bool JoinLeague(CreateOrJoinLeageReq req)
    {
        try
        {
            if (CheckUser(req.UserId, req.Token))
            {

                var league = _context.Leagues.FirstOrDefault(x => x.Name == req.LeagueName && x.Password == req.LeaguePassword && x.CompetitionId == req.CompetitionId);
                if (league != null)
                {
                    var userleague = new Entities.UserLeague
                    {
                        LeagueId = league.Id,
                        UserId = req.UserId
                    };
                    _context.UserLeagues.Add(userleague);
                    _context.SaveChanges();
                    var ucompetition = _context.UserCompetitions.FirstOrDefault(x => x.UserId == req.UserId && x.CompetitionId == req.CompetitionId);
                    if (ucompetition == null)
                    {

                        var usercompetition = new Entities.UserCompetition()
                        {
                            UserId = req.UserId,
                            CompetitionId = req.CompetitionId,
                            Points = 0
                        };
                        _context.UserCompetitions.Add(usercompetition);
                        _context.SaveChanges();
                    }
                    return true;
                }
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
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

