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

        modelBuilder.Entity<UserRankLeague>()
            .HasOne(ul => ul.RankLeague)
            .WithMany(l => l.UserRankLeagues)
            .HasForeignKey(ul => ul.RankLeagueId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade, depending on your needs

        modelBuilder.Entity<UserRankLeague>()
            .HasOne(ul => ul.User)
            .WithMany(u => u.UserRankLeagues)
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRank>()
            .HasOne(ul => ul.TeamRank)
            .WithMany(l => l.UserRanks)
            .HasForeignKey(ul => ul.TeamRankId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade, depending on your needs

        modelBuilder.Entity<UserRank>()
            .HasOne(ul => ul.User)
            .WithMany(u => u.UserRanks)
            .HasForeignKey(ul => ul.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TeamRank>()
            .HasOne(ul => ul.RankCompetition)
            .WithMany(u => u.TeamRanks)
            .HasForeignKey(ul => ul.RankCompetitionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RankLeague>()
            .HasOne(ul => ul.RankCompetition)
            .WithMany(u => u.RankLeagues)
            .HasForeignKey(ul => ul.RankCompetitionId)
            .OnDelete(DeleteBehavior.Restrict);

    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TipsWebApi.Models.WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserLeague> UserLeagues { get; set; }
    public DbSet<UserGame> UserGames { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<UserRankLeague> UserRankLeagues { get; set; }
    public DbSet<UserRank> UserRanks { get; set; }
    public DbSet<RankLeague> RankLeagues { get; set; }
    public DbSet<TeamRank> TeamRanks { get; set; }
    public DbSet<RankCompetition> RankCompetitions { get; set; }
}
