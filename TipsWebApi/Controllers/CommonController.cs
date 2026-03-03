using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TipsWebApi.Entities;
using TipsWebApi.Models;

namespace TipsWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommonController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApplicationDbContext _context;

    public CommonController(ILogger<WeatherForecastController> logger, ApplicationDbContext context)
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
            user.LastLogin = DateTime.Now;
            _context.SaveChanges();
            return userDto;
        }
        return new Models.User();
    }

    [HttpPost]
    [Route("CreateUser")]
    public bool CreateUser(CreateUserReq req)
    {
        try
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.UserName == req.UserName);
            if (existingUser != null)
            {
                return false;
            }
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
        catch (Exception ex)
        {
            //return ex.InnerException.Message;
            //return false;
            throw ex;
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

