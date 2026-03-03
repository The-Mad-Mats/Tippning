using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TipsWebApi.Entities;
using TipsWebApi.Models;

namespace TipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RankController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApplicationDbContext _context;
    private DateTime StartTime = new DateTime(2026, 3, 6, 2, 30, 0);

    public RankController(ILogger<WeatherForecastController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    [Route("GetUserRanks")]
    public List<Models.TeamRank> GetUserRanks(GetDefaultReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            var teamranks = new List<Models.TeamRank>();
            var allGames = _context.TeamRanks;
            var myGames = _context.UserRanks.Include(y => y.TeamRank).Where(x => x.UserId == req.UserId).ToList();
            if (myGames.Any())
            {
                foreach (var myGame in myGames)
                {
                    teamranks.Add(new Models.TeamRank
                    {
                        Id = myGame.Id,
                        Team = myGame.TeamRank.Team,
                        TeamId = myGame.TeamRank.Id,
                        Rank = myGame.Rank
                    });
                }
            }
            else
            {
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
            }
            return teamranks;
        }
        return new List<Models.TeamRank>();
    }

    [HttpPost]
    [Route("SaveUserRanks")]
    public bool SaveUserRank(SaveTeamRankReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            try
            {
                foreach (var teamReq in req.TeamRanks)
                {
                    var userGame = _context.UserRanks.FirstOrDefault(x => x.UserId == req.UserId && x.TeamRankId == teamReq.TeamId);
                    if (userGame != null)
                    {
                        userGame.Rank = teamReq.Rank;
                    }
                    else
                    {
                        userGame = new Entities.UserRank()
                        {
                            UserId = req.UserId,
                            TeamRankId = teamReq.TeamId,
                            Rank = teamReq.Rank,
                            Points = 0
                        };
                        _context.UserRanks.Add(userGame);
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
    [Route("GetRankLeagues")]
    public List<Models.League> GetRankLeagues(GetDefaultReq req)
    {
        if (CheckUser(req.UserId,req.Token))
        {
            var leagues = new List<Models.League>();
            var userleagues = _context.UserRankLeagues.Include(y => y.RankLeague).Where(x => x.UserId == req.UserId).ToList();
            foreach (var ul in userleagues)
            {
                if (ul.RankLeague != null)
                {
                    var leagueDto = new Models.League
                    {
                        Id = ul.RankLeague.Id,
                        Name = ul.RankLeague.Name,
                    };
                    leagues.Add(leagueDto);
                }
            }
            //var worldLeagueDto = new Models.League
            //{
            //    Id = 0,
            //    Name = "Världen",
            //};
            //leagues.Add(worldLeagueDto);
            return leagues;
        }
        return new List<Models.League>();
    }


    [HttpPost]
    [Route("GetRankLeague")]
    public Models.RankLeague GetRankLeague(GetLeagueReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            var leagueMembers = new List<int>();
            var rankLeague = new Models.RankLeague
            {
                TeamRanks = new List<Models.TeamRank>(),
                UserTeamRanks = new List<Models.UserTeamRank>()
            };

            if (req.LeagueId == 0)
            {
                ////LeaguId 0 är världsligan. Hämta de 10 bästa spelarna i världen och deras poäng, samt den aktuella användarens position och poäng
                //var users = _context.Users.ToList();
                //var position = 1;
                //foreach (var user in users.OrderByDescending(x => x.Points).Take(10))
                //{
                //    var leagueDto = new LeagueRow
                //    {
                //        Position = position++,
                //        UserName = user.UserName,
                //        Team = user.Team,
                //        Points = user.Points
                //    };
                //    leagueResult.Rows.Add(leagueDto);
                //    leagueMembers.Add(user.Id);
                //}
                //if (leagueMembers.Contains(req.UserId) == false)
                //{
                //    //Aktuell användare är inte i topp 10, hämta dennes position och lägg till i resultatet
                //    var index = users.FindIndex(x => x.Id == req.UserId);
                //    var leagueDto = new LeagueRow
                //    {
                //        Position = index++,
                //        UserName = users.First(x => x.Id == req.UserId).UserName,
                //        Team = users.First(x => x.Id == req.UserId).Team,
                //        Points = users.First(x => x.Id == req.UserId).Points
                //    };
                //}
            }
            else
            {
                var userleagues = _context.UserRankLeagues.Include(y => y.User).Where(x => x.RankLeagueId == req.LeagueId).ToList();
                leagueMembers = userleagues.Select(x => x.UserId).ToList();
                var position = 1;
                foreach (var rank in _context.TeamRanks.OrderBy(x => x.Rank).ToList())
                {
                    rankLeague.TeamRanks.Add(new Models.TeamRank
                    {
                        Id = rank.Id,
                        Team = rank.Team,
                        TeamId = rank.Id,
                        Rank = rank.Rank
                    });
                }
                foreach (var ul in userleagues)
                {
                    var utr = new Models.UserTeamRank
                    {
                        Team = ul.User.Team,
                        UserName = ul.User.UserName,
                        UserRanks = new List<Models.UserRank>()
                    };
                    if (DateTime.Now > StartTime)
                    {
                        foreach (var ur in _context.UserRanks.Where(x => x.UserId == ul.UserId))
                        {
                            var utrur = new Models.UserRank
                            {
                                Points = ur.Points,
                                Rank = ur.Rank,
                                TeamId = ur.TeamRankId
                            };
                            utr.UserRanks.Add(utrur);
                        }
                    }
                    rankLeague.UserTeamRanks.Add(utr);
                }
            }
            return rankLeague;
        }
        return new Models.RankLeague();
    }


    [HttpPost]
    [Route("CreateRankLeague")]
    public bool CreateRankLeague(CreateOrJoinLeageReq req)
    {
        try
        {
            if (CheckUser(req.UserId, req.Token))
            {

                var league = new Entities.RankLeague()
                {
                    Name = req.LeagueName,
                    Password = req.LeaguePassword
                };
                _context.RankLeagues.Add(league);
                _context.SaveChanges();
                var userleague = new Entities.UserRankLeague()
                {
                    UserId = req.UserId,
                    RankLeagueId = league.Id
                };
                _context.UserRankLeagues.Add(userleague);
                _context.SaveChanges();
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
    [Route("JoinRankLeague")]
    public bool JoiRanknLeague(CreateOrJoinLeageReq req)
    {
        try
        {
            if (CheckUser(req.UserId, req.Token))
            {

                var league = _context.RankLeagues.FirstOrDefault(x => x.Name == req.LeagueName && x.Password == req.LeaguePassword);
                if (league != null)
                {
                    var userleague = new Entities.UserRankLeague
                    {
                        RankLeagueId = league.Id,
                        UserId = req.UserId
                    };
                    _context.UserRankLeagues.Add(userleague);
                    _context.SaveChanges();
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

