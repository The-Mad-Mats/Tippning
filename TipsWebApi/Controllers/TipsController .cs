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
                Token = user.Token
            };
            return userDto;
        }
        return new Models.User();
    }
    [HttpPost]
    [Route("UserLeagues")]
    public List<Models.League> GetUserLeague(GetUserLeagueReq req)
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

