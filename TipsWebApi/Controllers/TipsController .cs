using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
    public User Login(LoginReq loginReq)
    {
        var user = _context.Users.FirstOrDefault(x => x.UserName == loginReq.Username && x.Password == loginReq.Password);
        if (user != null)
        {
            return user;
        }
        return new User();
    }
    [HttpPost]
    [Route("UserLeagues")]
    public List<League> GetUserLeague(GetUserLeagueReq req)
    {
        if (CheckUser(req.UserId,req.Token))
        {
            var leagues = new List<League>();
            var userleagues = _context.UserLeagues.Where(x => x.User == req.UserId).Include(y => y.League);
            foreach (var ul in userleagues)
            {
            }
        }
        return new List<League>();
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

