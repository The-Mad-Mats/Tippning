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
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApplicationDbContext _context;

    public AdminController(ILogger<WeatherForecastController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
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
                    DateTime = req.Date,
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
                    DateTime = addedGame.DateTime,
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
                games = _context.Games.AsNoTracking().Select(g => new Models.Game()
                {
                    Id = g.Id,
                    DateTime = g.DateTime,
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

