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
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApplicationDbContext _context;

    public TipsController(ILogger<WeatherForecastController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost]
    [Route("Login")]
    public Models.User Login(LoginReq loginReq)
    {
        var user = _context.Users.FirstOrDefault(x => x.UserName == loginReq.Username && x.Password == loginReq.Password);
        if (user != null)
        {
            var userDto = new Models.User
            {
                Id = user.Id,
                UserName = user.UserName,
                Team = user.Team,
                Points = user.Points,
                Token = user.Token,
                Admin = user.Admin,
            };
            return userDto;
        }
        return new Models.User();
    }
    
    [HttpPost]
    [Route("GetUserLeagues")]
    public List<Models.League> GetUserLeagues(GetUserLeagueReq req)
    {
        if (CheckUser(req.UserId,req.Token))
        {
            var leagues = new List<Models.League>();
            var userleagues = _context.UserLeagues.Include(y => y.League).Where(x => x.UserId == req.UserId).ToList();
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
            return leagues;
        }
        return new List<Models.League>();
    }

    [HttpPost]
    [Route("GetLeague")]
    public List<Models.LeagueRow> GetLeague(GetLeagueReq req)
    {
        if (CheckUser(req.UserId, req.Token))
        {
            var leagues = new List<Models.LeagueRow>();
            var userleagues = _context.UserLeagues.Include(y => y.User).Where(x => x.LeagueId == req.LeagueId).ToList();
            var position = 1;
            foreach (var ul in userleagues.OrderBy(x => x.User.Points))
            {
                if (ul.User != null)
                {
                    var leagueDto = new Models.LeagueRow
                    {
                        Position = position++,
                        UserName = ul.User.UserName,
                        Team = ul.User.Team,
                        Points = ul.User.Points
                    };
                    leagues.Add(leagueDto);
                }
            }
            return leagues;
        }
        return new List<Models.LeagueRow>();
    }

    [HttpPost]
    [Route("CreateUser")]
    public bool CreateUser(CreateUserReq req)
    {
        try
        {
            var user = new Entities.User()
            {
                UserName = req.UserName,
                Password = req.Password,
                Team = req.Team,
                Token = Guid.NewGuid().ToString(),
                Points = 0,
                Admin = false
            };
            _context.Users.Add(user);
            var userId = _context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
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
                    Password = req.LeaguePassword
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

                var league = _context.Leagues.FirstOrDefault(x => x.Name == req.LeagueName && x.Password == req.LeaguePassword);
                if (league != null)
                {
                    var userleague = new Entities.UserLeague
                    {
                        LeagueId = league.Id,
                        UserId = req.UserId
                    };
                    _context.UserLeagues.Add(userleague);
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

