using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using Shouko.Api;
using Shouko.Api.Interfaces;
using Shouko.Api.Services;
using Shouko.BusinessService.Interfaces;
using Shouko.BusinessService.Services;
using Shouko.DataService;
using Shouko.Helpers;
using Shouko.Helpers.Extensions;
using Shouko.Models;

namespace Shouko;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting Shouko Bot...");
        var appSettings = LoadEnv();
        
        var builder = Host.CreateApplicationBuilder(args);
        ConfigureServices(builder.Services, appSettings);
        ConfigureDatabaseService(builder.Services,appSettings.ConnectionString);
        var host = builder.Build();
        InitialiseDatabase(host);
        ConfigureHost(host);
        host.Run();
    }

    private static void ConfigureHost(IHost host)
    {
        Console.WriteLine("Configuring Host...");
        
        // Additional NetCord Configuration - AddModules to automatically register command modules
        // UseGatewayEventHandlers to automatically register gateway event handlers (Respond to Interactions, etc)
        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();
        
        // Additional Functionality that will be run right after the bot starts
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(async void () =>
        {
            try
            {
                // Execute Post Startup Utilities
                Console.WriteLine("----------Shouko Startup Complete---------");
            }
            catch (Exception e)
            {
                Console.WriteLine(" Error occurred while running post startup utilities: " + e.Message);
                lifetime.StopApplication();
            }
        });
        
    }

    private static void ConfigureServices(IServiceCollection services, ApplicationConfigurationSettings envConfigurations)
    {
        Console.WriteLine("Configuring Services...");
        services.AddLogging(logger =>
        {
            logger.ClearProviders();
            logger.AddConsole();
            logger.AddDebug();
        });
        
        services.AddSingleton(envConfigurations);
        
        // Bot Related Services
        // Intents
        services.AddDiscordGateway(options =>
        {
            options.Intents = GatewayIntents.GuildMessages
                              | GatewayIntents.GuildUsers
                              | GatewayIntents.DirectMessages
                              | GatewayIntents.MessageContent
                              | GatewayIntents.Guilds;
            options.Token = envConfigurations.DiscordToken;
        });

        // services.AddHttpClient();
        services.AddHttpClient<IApiManager, ApiManager>(client =>
        {
            // client.BaseAddress = new Uri("https://api.github.com");
            client.Timeout = TimeSpan.FromSeconds(60);
        });
        
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<IApiManager, ApiManager>();
        services.AddSingleton<IApiResponseHelper, ApiResponseHelper>();
        services.AddSingleton<IDiscordInteractionsService, DiscordInteractionsService>();
        services.AddSingleton<IDiscordInteractionsBusinessService, DiscordInteractionsBusinessService>();
        services.AddSingleton<IApiResponseHelper, ApiResponseHelper>();
        services.AddSingleton<IApiResponseBusinessService, ApiResponseBusinessService>();
        services.AddSingleton<IApiResponsesService, ApiResponsesService>();

        
        // Slash Command Service
        services.AddApplicationCommands<SlashCommandInteraction, SlashCommandContext, AutocompleteInteractionContext>();

        // Rest Client to support API interaction without responding to an interaction first.
        services.AddSingleton<RestClient>(serviceProvider => new RestClient(envConfigurations.EntityToken));
        
        
    }

    private static ApplicationConfigurationSettings LoadEnv()
    {
        Console.WriteLine("Loading Environment Variables...");
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var envFilePath = Path.Combine(solutionRoot, ".env");

        if (!File.Exists(envFilePath))
        {
            Console.WriteLine($"No .env file found at {envFilePath}");
            throw new FileNotFoundException($"No .env file found at {envFilePath}");
        }
        
        ApplicationConfigurationSettings appSettings = new ApplicationConfigurationSettings();
        
        var envFileContent = File.ReadAllLines(envFilePath);

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
            
            var key = parts[0].Trim();
            
            switch (key)
            {
                case "DISCORD_TOKEN":
                    appSettings.DiscordToken = parts[1].Trim();
                    break;
                case "CONNECTION_STRING":
                    appSettings.ConnectionString = parts[1].Trim();
                    break;
                case "DATABASE_NAME":
                    appSettings.DatabaseName = parts[1].Trim();
                    break;
                case "DATABASE_USERNAME":
                    appSettings.DatabaseUserName = parts[1].Trim();
                    break;
                case "DATABASE_PASSWORD":
                    appSettings.DatabasePassword = parts[1].Trim();
                    break;
                case "DATABASE_PORT":
                    appSettings.DatabasePort = int.Parse(parts[1].Trim());
                    break;
                case "DEBUG":
                    appSettings.Debug = true;
                    break;
                case "DEEPSEEK_API_KEY":
                    appSettings.DeepSeekApiKey = parts[1].Trim();
                    break;
                case "GEMINI_API_KEY":
                    appSettings.GeminiApiKey = parts[1].Trim();
                    break;
                case "DEEPSEEK_API_URL":
                    appSettings.DeepSeekApiUrl = parts[1].Trim();
                    break;
                case "GEMINI_API_URL":
                    appSettings.GeminiApiUrl = parts[1].Trim();
                    break;
                case "DEEPSEEK_MODEL":
                    appSettings.DeepSeekModel = parts[1].Trim();
                    break;
                case "GEMINI_TEXT_MODEL":
                    appSettings.GeminiTextModel = parts[1].Trim();
                    break;
                case "GEMINI_IMAGE_MODEL":
                    appSettings.GeminiImageModel = parts[1].Trim();
                    break;
                default:
                    Console.WriteLine($"Unknown .env key found: {key} with value {parts[1].Trim()}");
                    break;
            }
            
        }
        
        if (appSettings.Debug == true)
        {
            Console.WriteLine("Debug mode is enabled.");
            Console.WriteLine("Printing Environment Variables:");
            Console.WriteLine("===================================");
            foreach (var item in appSettings.GetType().GetProperties())
            {
                Console.WriteLine($"{item.Name}: {item.GetValue(appSettings)}");
            }
        }
        else
        {
            Console.WriteLine("Debug mode is disabled.");
        }
        
        // TODO Add Program Stop if missing specific mandatory values
        var nullProperties = appSettings.GetType().GetProperties()
            .Where(p => p.GetValue(appSettings) == null || string.IsNullOrEmpty(p.GetValue(appSettings)?.ToString()))
            .Select(p => p.Name)
            .ToList();
        
        
        return appSettings;
    }
    
    private static void ConfigureDatabaseService(IServiceCollection services, string postgresConnectionString)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(postgresConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("Shouko.DataService");
            });
            options.UseNpgsql(postgresConnectionString)
                .LogTo(Console.WriteLine, LogLevel.Information);
        });
    }
    
    private static void InitialiseDatabase(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (!dbContext.Database.CanConnect())
                {
                    throw new ApplicationException("Unable to connect to database.");
                }

                dbContext.Database.Migrate();
                Console.WriteLine("Database initialisation complete.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while initialising database: " + e.Message);
                throw;
            }
        }
    }
}