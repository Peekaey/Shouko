using Microsoft.EntityFrameworkCore;
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
using Shouko.BackgroundService;
using Shouko.BusinessService.Interfaces;
using Shouko.BusinessService.Services;
using Shouko.DataService;
using Shouko.DataService.Interfaces;
using Shouko.DataService.Services;
using Shouko.Helpers;
using Shouko.Helpers.Extensions;
using Shouko.Models;
using Shouko.Models.Enums;

namespace Shouko;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting Shouko Bot...");
        
        var solutionRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
        var envFilePath = Path.Combine(solutionRoot, ".env");
        var appSettings = new ApplicationConfigurationSettings();
        if (File.Exists(envFilePath))
        {
            Console.WriteLine($".env file found at {envFilePath}, processing configurations from file..");
            appSettings = LoadEnv(envFilePath);
        }
        else
        {
            Console.WriteLine($".env not found at root project, attempting to load from environment variables.");
            appSettings = LoadEnvFromArguements();
        }
        
        
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
        // UseGatewayEventHandlers to automatically register gateway event handlers (Respond to Interactions, etc..)
        host.AddModules(typeof(Program).Assembly);
        host.UseGatewayEventHandlers();
        
        // Additional Functionality that will be run right after the bot starts
        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(void () =>
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
        services.AddSingleton<IApiRequestLimitCounterService, ApiRequestLimitCounterService>();
        services.AddSingleton<IApiRequestLimitCounterBusinessService, ApiRequestLimitCounterBusinessService>();
        services.AddSingleton<IApiServiceBusinessService, ApiServiceBusinessService>();
        services.AddSingleton<IApiRequestCountersBusinessService, ApiRequestCountersBusinessService>();
        services.AddSingleton<IApiRequestCountersService, ApiRequestCountersService>();
        
        
        if (envConfigurations.EnableGeminiTextModelDailyLimit ||
            envConfigurations.EnableGeminiTextModelMinuteLimit ||
            envConfigurations.EnableGeminiImageModelDailyLimit ||
            envConfigurations.EnableGeminiImageModelMinuteLimit)
        {
            services.AddHostedService<ApiRequestLimitBackgroundWorker>();
        }
        
        // Slash Command Service
        services.AddApplicationCommands<SlashCommandInteraction, SlashCommandContext, AutocompleteInteractionContext>();

        // Rest Client to support API interaction without responding to an interaction first.
        services.AddSingleton<RestClient>(_ => new RestClient(envConfigurations.EntityToken));
        
        
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
    
        private static ApplicationConfigurationSettings LoadEnvFromArguements()
    {
        Console.WriteLine("Loading Environment Variables From Arguements...");
        ApplicationConfigurationSettings appSettings = new ApplicationConfigurationSettings();

        // Required field
        appSettings.DiscordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        if (string.IsNullOrEmpty(appSettings.DiscordToken))
        {
            Console.WriteLine("DISCORD_TOKEN is not set in environment variables.");
            throw new ApplicationException("DISCORD_TOKEN is required.");
        }
        
        // Required field
        appSettings.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        if (string.IsNullOrEmpty(appSettings.ConnectionString))
        {
            Console.WriteLine("CONNECTION_STRING is not set in environment variables.");
            throw new ApplicationException("CONNECTION_STRING is required.");
        }

        // Required field
        appSettings.DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
        if (string.IsNullOrEmpty(appSettings.DatabaseName)) 
        {
            Console.WriteLine("DATABASE_NAME is not set in environment variables.");
            throw new ApplicationException("DATABASE_NAME is required.");
        }
        
        // Required field
        appSettings.DatabaseUserName = Environment.GetEnvironmentVariable("DATABASE_USERNAME");
        if (string.IsNullOrEmpty(appSettings.DatabaseUserName)) 
        {
            Console.WriteLine("DATABASE_USERNAME is not set in environment variables.");
            throw new ApplicationException("DATABASE_USERNAME is required.");
        }
        
        // Required field
        appSettings.DatabasePassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        if (string.IsNullOrEmpty(appSettings.DatabasePassword)) 
        {
            Console.WriteLine("DATABASE_PASSWORD is not set in environment variables.");
            throw new ApplicationException("DATABASE_PASSWORD is required.");
        }

        // Required field
        var databasePort = Environment.GetEnvironmentVariable("DATABASE_PORT");
        if (string.IsNullOrEmpty(databasePort) || !int.TryParse(databasePort, out var port))
        {
            Console.WriteLine("DATABASE_PORT is not set or invalid in environment variables. Defaulting to 5432.");
            port = 5432; // Default PostgreSQL port
        }
        appSettings.DatabasePort = port;
        
        appSettings.Debug = Environment.GetEnvironmentVariable("DEBUG")?.ToLower() == "true";
        
        appSettings.DeepSeekApiKey = Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY");
        
        // Required field
        appSettings.GeminiApiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        if (string.IsNullOrEmpty(appSettings.GeminiApiKey))
        {
            Console.WriteLine("GEMINI_API_KEY is not set in environment variables.");
            throw new ApplicationException("GEMINI_API_KEY is required.");
        }
        
        appSettings.DeepSeekApiUrl = Environment.GetEnvironmentVariable("DEEPSEEK_API_URL");
        
        // Required field
        appSettings.GeminiApiUrl = Environment.GetEnvironmentVariable("GEMINI_API_URL");
        
        appSettings.DeepSeekModel = Environment.GetEnvironmentVariable("DEEPSEEK_MODEL");
        
        // Required field
        appSettings.GeminiTextModel = Environment.GetEnvironmentVariable("GEMINI_TEXT_MODEL");
        if (string.IsNullOrEmpty(appSettings.GeminiTextModel))
        {
            Console.WriteLine("GEMINI_TEXT_MODEL is not set in environment variables.");
            throw new ApplicationException("GEMINI_TEXT_MODEL is required.");
        }
        
        // Required field
        appSettings.GeminiImageModel = Environment.GetEnvironmentVariable("GEMINI_IMAGE_MODEL");
        if (string.IsNullOrEmpty(appSettings.GeminiImageModel))
        {
            Console.WriteLine("GEMINI_IMAGE_MODEL is not set in environment variables.");
            throw new ApplicationException("GEMINI_IMAGE_MODEL is required.");
        }
        
        var geminiTextModelMinuteLimit = Environment.GetEnvironmentVariable("GEMINI_TEXT_MODEL_MINUTE_LIMIT");
        if (int.TryParse(geminiTextModelMinuteLimit, out var textModelMinuteLimit))
        {
            appSettings.GeminiTextModelMinuteLimit = textModelMinuteLimit;
        }
        else
        {
            Console.WriteLine("GEMINI_TEXT_MODEL_MINUTE_LIMIT is not set or invalid in environment variables. Defaulting to 0.");
            appSettings.GeminiTextModelMinuteLimit = 0; // Default to no limit
        }
        
        var geminiImageModelMinuteLimit = Environment.GetEnvironmentVariable("GEMINI_IMAGE_MODEL_MINUTE_LIMIT");
        if (int.TryParse(geminiImageModelMinuteLimit, out var imageModelMinuteLimit))
        {
            appSettings.GeminiImageModelMinuteLimit = imageModelMinuteLimit;
        }
        else
        {
            Console.WriteLine("GEMINI_IMAGE_MODEL_MINUTE_LIMIT is not set or invalid in environment variables. Defaulting to 0.");
            appSettings.GeminiImageModelMinuteLimit = 0; // Default to no limit
        }
        
        var geminiTextModelDailyLimit = Environment.GetEnvironmentVariable("GEMINI_TEXT_MODEL_DAILY_LIMIT");
        if (int.TryParse(geminiTextModelDailyLimit, out var textModelDailyLimit))
        {
            appSettings.GeminiTextModelDailyLimit = textModelDailyLimit;
        }
        else
        {
            Console.WriteLine("GEMINI_TEXT_MODEL_DAILY_LIMIT is not set or invalid in environment variables. Defaulting to 0.");
            appSettings.GeminiTextModelDailyLimit = 0; // Default to no limit
        }
        var geminiImageModelDailyLimit = Environment.GetEnvironmentVariable("GEMINI_IMAGE_MODEL_DAILY_LIMIT");
        if (int.TryParse(geminiImageModelDailyLimit, out var imageModelDailyLimit))
        {
            appSettings.GeminiImageModelDailyLimit = imageModelDailyLimit;
        }
        else
        {
            Console.WriteLine("GEMINI_IMAGE_MODEL_DAILY_LIMIT is not set or invalid in environment variables. Defaulting to 0.");
            appSettings.GeminiImageModelDailyLimit = 0; // Default to no limit
        }
        if (appSettings.Debug)
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
        
        if (appSettings.GeminiTextModelDailyLimit != 0)
        {
            appSettings.EnableGeminiTextModelDailyLimit = true;
        }
        
        if (appSettings.GeminiTextModelMinuteLimit != 0)
        {
            appSettings.EnableGeminiTextModelMinuteLimit = true;
        }
        
        if (appSettings.GeminiImageModelDailyLimit != 0)
        {
            appSettings.EnableGeminiImageModelDailyLimit = true;
        }
        
        if (appSettings.GeminiImageModelMinuteLimit != 0)
        {
            appSettings.EnableGeminiImageModelMinuteLimit = true;
        }

        return appSettings;
    }

    private static ApplicationConfigurationSettings LoadEnv(string envFilePath)
    {
        Console.WriteLine("Loading Environment Variables...");
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
                case "GEMINI_TEXT_MODEL_MINUTE_LIMIT":
                    appSettings.GeminiTextModelMinuteLimit = int.Parse(parts[1].Trim());
                    break;
                case "GEMINI_IMAGE_MODEL_MINUTE_LIMIT":
                    appSettings.GeminiImageModelMinuteLimit = int.Parse(parts[1].Trim());
                    break;
                case "GEMINI_TEXT_MODEL_DAILY_LIMIT":
                    appSettings.GeminiTextModelDailyLimit = int.Parse(parts[1].Trim());
                    break;
                case "GEMINI_IMAGE_MODEL_DAILY_LIMIT":
                    appSettings.GeminiImageModelDailyLimit = int.Parse(parts[1].Trim());
                    break;
                default:
                    Console.WriteLine($"Unknown .env key found: {key} with value {parts[1].Trim()}");
                    break;
            }
            
        }
        
        if (appSettings.Debug)
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

        if (appSettings.GeminiTextModelDailyLimit != 0)
        {
            appSettings.EnableGeminiTextModelDailyLimit = true;
        }
        
        if (appSettings.GeminiTextModelMinuteLimit != 0)
        {
            appSettings.EnableGeminiTextModelMinuteLimit = true;
        }
        
        if (appSettings.GeminiImageModelDailyLimit != 0)
        {
            appSettings.EnableGeminiImageModelDailyLimit = true;
        }
        
        if (appSettings.GeminiImageModelMinuteLimit != 0)
        {
            appSettings.EnableGeminiImageModelMinuteLimit = true;
        }
        
        // TODO Add Program Stop if missing specific mandatory values
        var nullProperties = appSettings.GetType().GetProperties()
            .Where(p => p.GetValue(appSettings) == null || string.IsNullOrEmpty(p.GetValue(appSettings)?.ToString()))
            .Select(p => p.Name)
            .ToList();
        
        
        return appSettings;
    }
}