using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebScraper.Models;

namespace WebScraper.Data;

public class WebScraperContext : DbContext
{
    public DbSet<BasketballGame> BasketballGames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = ConfigurationManager.AppSettings.Get("dbPath");
        optionsBuilder.UseSqlServer(dbPath);
    }
}
