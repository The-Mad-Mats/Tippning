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
                games = _context.Games.AsNoTracking().Select(g => new Models.Game()
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
                                        var currentUser = _context.Users.First(x => x.Id == userGame.UserId);
                                        currentUser.Points -= userGame.Points.Value;
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
                                var currentUser = _context.Users.First(x => x.Id == userGame.UserId);
                                currentUser.Points += points;
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

