using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TipsWebApi.Entities;

public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLeague>()
            .HasOne(ul => ul.League)
            .WithMany(l => l.UserLeagues)
            .HasForeignKey(ul => ul.LeagueId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade, depending on your needs

        modelBuilder.Entity<UserLeague>()
            .HasOne(ul => ul.User)
            .WithMany(u => u.UserLeagues)
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserGame>()
            .HasOne(ul => ul.Game)
            .WithMany(l => l.UserGames)
            .HasForeignKey(ul => ul.GameId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade, depending on your needs

        modelBuilder.Entity<UserGame>()
            .HasOne(ul => ul.User)
            .WithMany(u => u.UserGames)
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TipsWebApi.Models.WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLeague> UserLeagues { get; set; }
    public DbSet<UserGame> UserGames{ get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Game> Games { get; set; }
}
