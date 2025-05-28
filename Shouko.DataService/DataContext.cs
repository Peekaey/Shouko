using Microsoft.EntityFrameworkCore;
using Shouko.Models.DatabaseModels;

namespace Shouko.DataService;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<DiscordInteraction> DiscordInteractions { get; set; }
    public DbSet<ApiResponse> ApiResponses { get; set; }
    
}