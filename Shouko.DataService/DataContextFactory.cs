using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Shouko.DataService;

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var envFilePath = Path.Combine(solutionRoot, ".env");
        if (!File.Exists(envFilePath))
        {
            Console.WriteLine($"no .env file found at {solutionRoot}");
            throw new FileNotFoundException("no .env file found");
        }

        var envFileContent = File.ReadAllLines(envFilePath);
        var connectionString = string.Empty;
        foreach (var line in envFileContent)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                continue;
            }

            var parts = line.Split("=", 2);
            if (parts.Length != 2)
            {
                continue;
            }

            if (parts[0].Trim() != "CONNECTION_STRING")
            {
                continue;
            }
            
            connectionString = parts[1].Trim();
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("CONNECTION_STRING not found in .env");
            throw new ArgumentException("CONNECTION_STRING not specified");
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        return new DataContext(optionsBuilder.Options);
    }
}